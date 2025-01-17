package com.example.androidesemkarestaurant

import android.app.Activity
import android.graphics.BitmapFactory
import android.icu.text.NumberFormat
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.androidesemkarestaurant.databinding.CardCartLayoutBinding
import com.example.androidesemkarestaurant.databinding.FragmentBlank3Binding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import java.util.Locale


class BlankFragment3 : Fragment() {
    private lateinit var bind: FragmentBlank3Binding

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        bind = FragmentBlank3Binding.inflate(layoutInflater, container, false)
        getTotalPrice()
        getCart()

        val shared = requireActivity().getSharedPreferences("cart", Activity.MODE_PRIVATE)
        val edit = shared.edit()
        val getShared = shared.getString("cartArray", "[]")
        val cartArray = JSONArray(getShared)

        bind.cartOrder.setOnClickListener {
            if (cartArray.length() == 0) {
                Toast.makeText(requireContext(), "Cannot checkout with empty cart!", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            val cartData = JSONArray().apply {
                for (e in cartArray.length() - 1 downTo 0) {
                    put(JSONObject().apply {
                        put("menuId", cartArray.getJSONObject(e).getString("menuId"))
                        put("quantity", cartArray.getJSONObject(e).getInt("qty"))
                    })
                }
            }

            GlobalScope.launch(Dispatchers.IO) {
                val connection = URL("http://10.0.2.2:5000/Api/Table/${Session.code}/Order").openConnection() as HttpURLConnection
                connection.requestMethod = "POST"
                connection.setRequestProperty("Content-Type", "application/json")

                val outputStream = connection.outputStream
                outputStream.write(cartData.toString().toByteArray())
                outputStream.flush()
                outputStream.close()

                val responseCode = connection.responseCode

                GlobalScope.launch(Dispatchers.Main) {
                    if (responseCode in 200..299) {
                        Toast.makeText(requireContext(), "Success to checkout your items!", Toast.LENGTH_SHORT).show()

                        edit.putString("cartArray", "[]")
                        edit.apply()

                        getCart()
                        getTotalPrice()
                    } else {
                        Toast.makeText(requireContext(), "Failed to checkout!", Toast.LENGTH_SHORT).show()
                    }
                }
            }
        }

        return bind.root
    }

    private fun getCart() {
        val numberFormat = NumberFormat.getCurrencyInstance(Locale("in", "ID"))
        val shared = requireActivity().getSharedPreferences("cart", Activity.MODE_PRIVATE)
        val edit = shared.edit()
        val getShared = shared.getString("cartArray", "[]")
        val cartArray = JSONArray(getShared)

        bind.cartRv.adapter = object : RecyclerView.Adapter<Holder>() {
            override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): Holder {
                return Holder(CardCartLayoutBinding.inflate(layoutInflater, parent, false))
            }

            override fun getItemCount(): Int {
                return cartArray.length()
            }

            override fun onBindViewHolder(holder: Holder, position: Int) {
                val getData = cartArray.getJSONObject(position)
                holder.binding.itemName.text = getData.getString("name")
                holder.binding.itemPrice.text = numberFormat.format(getData.getInt("price") * getData.getInt("qty"))
                holder.binding.itemQty.text = getData.getString("qty")
                GlobalScope.launch(Dispatchers.IO) {
                    try {
                        val imageConnection = BitmapFactory.decodeStream(URL("http://10.0.2.2:5000/Api/Menu/${getData.getString("menuId")}/Photo").openStream())
                        GlobalScope.launch(Dispatchers.Main) {
                            holder.binding.itemImage.setImageBitmap(imageConnection)
                        }
                    }
                    catch (e : Exception) {
                        GlobalScope.launch(Dispatchers.Main) {
                            holder.binding.itemImage.setImageResource(R.drawable.logo)
                        }
                    }
                }

                holder.binding.itemIncrement.setOnClickListener {
                    val text = holder.binding.itemQty.text.toString().toInt() + 1
                    holder.binding.itemQty.text = text.toString()

                    cartArray.getJSONObject(position).put("qty", holder.binding.itemQty.text.toString().toInt())

                    edit.putString("cartArray", cartArray.toString())
                    edit.apply()

                    holder.binding.itemPrice.text = numberFormat.format(getData.getInt("price") * getData.getInt("qty"))
                    getTotalPrice()
                }

                holder.binding.itemDecrement.setOnClickListener {
                    val text = holder.binding.itemQty.text.toString().toInt()
                    if (text > 1) {
                        holder.binding.itemQty.text = (text - 1).toString()

                        cartArray.getJSONObject(position).put("qty", holder.binding.itemQty.text.toString().toInt())

                        edit.putString("cartArray", cartArray.toString())
                        edit.apply()

                        holder.binding.itemPrice.text = numberFormat.format(getData.getInt("price") * getData.getInt("qty"))
                        getTotalPrice()
                    }
                }

                holder.binding.itemRemove.setOnClickListener {
                    cartArray.remove(position)

                    edit.putString("cartArray", cartArray.toString())
                    edit.apply()

                    getTotalPrice()
                    getCart()
                }
            }
        }
        bind.cartRv.layoutManager = LinearLayoutManager(context)
    }

    private fun getTotalPrice() {
        val numberFormat = NumberFormat.getCurrencyInstance(Locale("in", "ID"))
        val shared = requireActivity().getSharedPreferences("cart", Activity.MODE_PRIVATE)
        val getShared = shared.getString("cartArray", "[]")
        val cartArray = JSONArray(getShared)
        var totalPrice = 0

        for (e in cartArray.length() - 1 downTo 0) {
            totalPrice += (cartArray.getJSONObject(e).getInt("price") * cartArray.getJSONObject(e).getInt("qty"))
        }

        bind.cartPrice.text = numberFormat.format(totalPrice)
    }

    class Holder(val binding: CardCartLayoutBinding) : RecyclerView.ViewHolder(binding.root)
}