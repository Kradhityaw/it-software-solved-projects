package com.example.androidesemkarestaurant

import android.app.Activity
import android.content.Intent
import android.os.Bundle
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.androidesemkarestaurant.MainActivity4.Holder
import com.example.androidesemkarestaurant.databinding.ActivityMain5Binding
import com.example.androidesemkarestaurant.databinding.AlertAddTableLayoutBinding
import com.example.androidesemkarestaurant.databinding.AlertEditTableLayoutBinding
import com.example.androidesemkarestaurant.databinding.CardListLayoutBinding
import com.example.androidesemkarestaurant.databinding.CardOrdersChildLayoutBinding
import com.example.androidesemkarestaurant.databinding.CardOrdersParentLayoutBinding
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import java.text.NumberFormat
import java.time.ZonedDateTime
import java.time.format.DateTimeFormatter
import java.util.Locale

class MainActivity5 : AppCompatActivity() {
    private lateinit var bind: ActivityMain5Binding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain5Binding.inflate(layoutInflater)
        setContentView(bind.root)

        loadTable()
    }

    fun loadTable() {
        GlobalScope.launch(Dispatchers.IO) {
            try {
                val connection = URL("http://10.0.2.2:5000/Api/Table/${intent.getStringExtra("id")}").openConnection() as HttpURLConnection
                connection.requestMethod = "GET"
                connection.setRequestProperty("Authorization", "Bearer ${Session.token}")

                val responseCode = connection.responseCode
                val inputStream = connection.inputStream.bufferedReader().readText()

                runOnUiThread {
                    if (responseCode in 200..299) {
                        val dataTable = JSONObject(inputStream)
                        val item = dataTable.getJSONArray("orders")
                        val numberFormat = NumberFormat.getCurrencyInstance(Locale("in", "ID"))

                        var prices = 0
                        for (e in item.length() - 1 downTo 0) {
                            for (i in item.getJSONObject(e).getJSONArray("orderDetails").length() - 1 downTo 0) {
                                prices += item.getJSONObject(e).getJSONArray("orderDetails").getJSONObject(i).getInt("subTotal")
                            }
                        }

                        bind.orderCloseTable.setOnClickListener {
                            GlobalScope.launch(Dispatchers.IO) {
                                val connection = URL("http://10.0.2.2:5000/Api/Table/${intent.getStringExtra("id")}/Close").openConnection() as HttpURLConnection
                                connection.requestMethod = "PUT"
                                connection.setRequestProperty("Authorization", "Bearer ${Session.token}")

                                val responseCode = connection.responseCode

                                runOnUiThread {
                                    if (responseCode in 200..299) {
                                        Toast.makeText(this@MainActivity5, "Success Close Table!", Toast.LENGTH_SHORT).show()
                                        finish()
                                    }
                                    else {
                                        Toast.makeText(this@MainActivity5, "Request timeout!", Toast.LENGTH_SHORT).show()
                                    }
                                }
                            }
                        }

                        bind.ordersTotal.text = numberFormat.format(prices)

                        bind.ordersTable.text = "Table ${dataTable.getString("number")}"
                        bind.ordersCode.text = dataTable.getString("code")

                        bind.ordersRv.adapter = object : RecyclerView.Adapter<ParentHolder>() {
                            override fun onCreateViewHolder(
                                parent: ViewGroup,
                                viewType: Int
                            ): ParentHolder {
                                return ParentHolder(CardOrdersParentLayoutBinding.inflate(layoutInflater, parent, false))
                            }

                            override fun getItemCount(): Int {
                                return item.length()
                            }

                            override fun onBindViewHolder(holder: ParentHolder, position: Int) {
                                val getItem = item.getJSONObject(position)
                                val date = getItem.getString("createdAt").substring(0, 19) + "Z"
                                val formattedDate = ZonedDateTime.parse(date).format(
                                    DateTimeFormatter.ofPattern("yy MMM dd HH:mm:ss"))

                                holder.parent.orderDate.text = "Order ${position + 1} - ${formattedDate}"
                                holder.parent.orderStatus.text = getItem.getString("status")

                                holder.parent.orderItems.adapter = object : RecyclerView.Adapter<ChildHolder>() {
                                    override fun onCreateViewHolder(
                                        parent: ViewGroup,
                                        viewType: Int
                                    ): ChildHolder {
                                        return ChildHolder(CardOrdersChildLayoutBinding.inflate(layoutInflater, parent, false))
                                    }

                                    override fun getItemCount(): Int {
                                        return getItem.getJSONArray("orderDetails").length()
                                    }

                                    override fun onBindViewHolder(
                                        holder: ChildHolder,
                                        position: Int
                                    ) {
                                        val dataItems = getItem.getJSONArray("orderDetails").getJSONObject(position)
                                        holder.child.itemNameQty.text = "${dataItems.getString("quantity")} ${dataItems.getJSONObject("menu").getString("name")}"
                                        holder.child.itemPrice.text = numberFormat.format(dataItems.getDouble("subTotal"))

                                        holder.itemView.setOnClickListener {
                                            val binding = AlertEditTableLayoutBinding.inflate(layoutInflater)

                                            val listItem = listOf("Ordered", "OnCooking", "Cooked", "Done")

                                            binding.statusDropdown.setAdapter(ArrayAdapter(this@MainActivity5, android.R.layout.simple_list_item_single_choice, listItem))

                                            var selected = ""

                                            binding.statusDropdown.setOnItemClickListener { adapterView, view, i, l ->
                                                when (i) {
                                                    0 -> selected = "Ordered"
                                                    1 -> selected = "OnCooking"
                                                    2 -> selected = "Cooked"
                                                    3 -> selected = "Done"
                                                }
                                            }

                                            val dialog = MaterialAlertDialogBuilder(this@MainActivity5)
                                                .setView(binding.root)
                                                .setCancelable(false)
                                                .create()

                                            binding.addCancel.setOnClickListener {
                                                dialog.dismiss()
                                            }

                                            binding.addOpen.setOnClickListener { oke ->
                                                if (selected.isEmpty()) {
                                                    Toast.makeText(this@MainActivity5, "Please fill all inputs!", Toast.LENGTH_SHORT)
                                                        .show()
                                                    return@setOnClickListener
                                                }

                                                GlobalScope.launch(Dispatchers.IO) {
                                                    try {
                                                        val connection = URL("http://10.0.2.2:5000/Api/Table/${dataTable.getString("tableId")}/Order/${getItem.getString("orderId")}?status=$selected").openConnection() as HttpURLConnection
                                                        connection.requestMethod = "PUT"
                                                        connection.setRequestProperty("Authorization", "Bearer ${Session.token}")
                                                        connection.setRequestProperty("Content-Type", "application/json")

                                                        val responseCode = connection.responseCode

                                                        runOnUiThread {
                                                            if (responseCode in 200..299) {
                                                                Toast.makeText(this@MainActivity5, "Success Save Changes!", Toast.LENGTH_SHORT).show()
                                                                dialog.dismiss()
                                                                loadTable()
                                                            }
                                                            else {
                                                                Toast.makeText(this@MainActivity5, "Try Again", Toast.LENGTH_SHORT).show()
                                                            }
                                                        }
                                                    }
                                                    catch (e : Exception) {
                                                        runOnUiThread {
                                                            Toast.makeText(this@MainActivity5, "Error5 : ${e.message}", Toast.LENGTH_SHORT)
                                                                .show()
                                                        }
                                                    }
                                                }
                                            }

                                            dialog.show()
                                        }
                                    }

                                }

                                holder.parent.orderItems.layoutManager = LinearLayoutManager(this@MainActivity5)
                            }

                        }

                        bind.ordersRv.layoutManager = LinearLayoutManager(this@MainActivity5)
                    }
                    else {
                        Toast.makeText(this@MainActivity5, "Request timeout!", Toast.LENGTH_SHORT).show()
                    }
                }
            }
            catch (e : Exception) {
                runOnUiThread {
                    Toast.makeText(this@MainActivity5, "Error : ${e.message}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

//    private fun getTotalPrice() {
//        val numberFormat = android.icu.text.NumberFormat.getCurrencyInstance(Locale("in", "ID"))
//        val shared = getSharedPreferences("cart", Activity.MODE_PRIVATE)
//        val getShared = shared.getString("cartArray", "[]")
//        val cartArray = JSONArray(getShared)
//        var totalPrice = 0
//
//        for (e in cartArray.length() - 1 downTo 0) {
//            totalPrice += (cartArray.getJSONObject(e).getInt("price") * cartArray.getJSONObject(e).getInt("qty"))
//        }
//
//        bind.ordersTotal.text = numberFormat.format(totalPrice)
//    }

    class ParentHolder(val parent: CardOrdersParentLayoutBinding) : RecyclerView.ViewHolder(parent.root)
    class ChildHolder(val child: CardOrdersChildLayoutBinding)  : RecyclerView.ViewHolder(child.root)
}