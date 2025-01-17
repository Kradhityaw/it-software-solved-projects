package com.example.androidesemkarestaurant

import android.icu.text.NumberFormat
import android.icu.text.SimpleDateFormat
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.androidesemkarestaurant.databinding.CardOrdersChildLayoutBinding
import com.example.androidesemkarestaurant.databinding.CardOrdersParentLayoutBinding
import com.example.androidesemkarestaurant.databinding.FragmentBlank4Binding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import java.net.HttpURLConnection
import java.net.URL
import java.time.LocalDate
import java.time.ZonedDateTime
import java.time.format.DateTimeFormatter
import java.util.Date
import java.util.Locale

class BlankFragment4 : Fragment() {
    private lateinit var bind: FragmentBlank4Binding

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        bind = FragmentBlank4Binding.inflate(layoutInflater)

        GlobalScope.launch(Dispatchers.IO) {
            try {
                val connection = URL("http://10.0.2.2:5000/Api/Table/${Session.code}/Orders").openConnection() as HttpURLConnection
                connection.requestMethod = "GET"
                val responseCode = connection.responseCode
                val inputStream = connection.inputStream.bufferedReader().readText()

                GlobalScope.launch(Dispatchers.Main) {
                    if (responseCode in 200..299) {
                        val ordersData = JSONArray(inputStream)
                        val numberFormat = NumberFormat.getCurrencyInstance(Locale("in", "ID"))
                        var totalPrice = 0

                        for (e in ordersData.length() - 1 downTo 0) {
                            val items = ordersData.getJSONObject(e).getJSONArray("orderDetails")
                            for (i in items.length() - 1 downTo 0) {
                                totalPrice += items.getJSONObject(i).getInt("subTotal")
                            }
                        }

                        bind.ordersTotal.text = numberFormat.format(totalPrice)

                        bind.ordersRv.adapter = object : RecyclerView.Adapter<ParentHolder>() {
                            override fun onCreateViewHolder(
                                parent: ViewGroup,
                                viewType: Int
                            ): ParentHolder {
                                return ParentHolder(CardOrdersParentLayoutBinding.inflate(layoutInflater, parent, false))
                            }

                            override fun getItemCount(): Int {
                                return ordersData.length()
                            }

                            override fun onBindViewHolder(holder: ParentHolder, position: Int) {
                                val getOrders = ordersData.getJSONObject(position)
                                val date = getOrders.getString("createdAt").substring(0, 19) + "Z"
                                val formattedDate = ZonedDateTime.parse(date).format(DateTimeFormatter.ofPattern("yy MMM dd HH:mm:ss"))

                                holder.parent.orderDate.text = "Order ${position + 1} - ${formattedDate}"
                                holder.parent.orderStatus.text = getOrders.getString("status")

                                holder.parent.orderItems.adapter = object : RecyclerView.Adapter<ChildHolder>() {
                                    override fun onCreateViewHolder(
                                        parent: ViewGroup,
                                        viewType: Int
                                    ): ChildHolder {
                                        return ChildHolder(CardOrdersChildLayoutBinding.inflate(layoutInflater, parent, false))
                                    }

                                    override fun getItemCount(): Int {
                                        return getOrders.getJSONArray("orderDetails").length()
                                    }

                                    override fun onBindViewHolder(
                                        holder: ChildHolder,
                                        position: Int
                                    ) {
                                        val dataItems = getOrders.getJSONArray("orderDetails").getJSONObject(position)
                                        holder.child.itemNameQty.text = "${dataItems.getString("quantity")} ${dataItems.getJSONObject("menu").getString("name")}"
                                        holder.child.itemPrice.text = numberFormat.format(dataItems.getDouble("subTotal"))
                                    }

                                }
                                holder.parent.orderItems.layoutManager = LinearLayoutManager(context)
                            }

                        }
                        bind.ordersRv.layoutManager = LinearLayoutManager(context)
                    }
                    else {
                        Toast.makeText(requireContext(), "Request Timeout!", Toast.LENGTH_SHORT).show()
                    }
                }
            }
            catch (e : Exception) {
                Toast.makeText(requireContext(), "Error : ${e.message}", Toast.LENGTH_SHORT).show()
            }
        }

        return bind.root
    }

    class ParentHolder(val parent: CardOrdersParentLayoutBinding) : RecyclerView.ViewHolder(parent.root)
    class ChildHolder(val child: CardOrdersChildLayoutBinding) : RecyclerView.ViewHolder(child.root)
}