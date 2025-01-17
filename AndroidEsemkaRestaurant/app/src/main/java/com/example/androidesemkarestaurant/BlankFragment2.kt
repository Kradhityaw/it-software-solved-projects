package com.example.androidesemkarestaurant

import android.app.Activity
import android.graphics.BitmapFactory
import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.recyclerview.widget.GridLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.androidesemkarestaurant.BlankFragment.MenuHolder
import com.example.androidesemkarestaurant.databinding.CardMenuLayoutBinding
import com.example.androidesemkarestaurant.databinding.FragmentBlank2Binding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import java.text.NumberFormat
import java.util.Locale

class BlankFragment2(val menuId: String) : Fragment() {
    private lateinit var bind: FragmentBlank2Binding

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        bind = FragmentBlank2Binding.inflate(layoutInflater)

        GlobalScope.launch(Dispatchers.IO) {
            try {
                val connection = URL("http://10.0.2.2:5000/Api/Menu/$menuId").openConnection() as HttpURLConnection
                connection.requestMethod = "GET"
                val responseCode = connection.responseCode
                val inputStream = connection.inputStream.bufferedReader().readText()

                GlobalScope.launch(Dispatchers.Main) {
                    if (responseCode in 200..299) {
                        val getMenu = JSONObject(inputStream)
                        val numberFormat = NumberFormat.getCurrencyInstance(Locale("in", "ID"))
                        val shared = requireActivity().getSharedPreferences("cart", Activity.MODE_PRIVATE)
                        val edit = shared.edit()
                        val getShared = shared.getString("cartArray", "[]")
                        val cartArray = JSONArray(getShared)

                        Log.d("Keranjang", cartArray.toString())

                        bind.detailName.text = getMenu.getString("name")
                        bind.detailPrice.text = numberFormat.format(getMenu.getDouble("price"))
                        bind.detailDescription.text = getMenu.getString("description")
                        GlobalScope.launch(Dispatchers.IO) {
                            try {
                                val imageConnection = BitmapFactory.decodeStream(URL("http://10.0.2.2:5000/Api/Menu/${getMenu.getString("menuId")}/Photo").openStream())
                                GlobalScope.launch(Dispatchers.Main) {
                                    bind.detailImage.setImageBitmap(imageConnection)
                                }
                            }
                            catch (e : Exception) {
                                GlobalScope.launch(Dispatchers.Main) {
                                    bind.detailImage.setImageResource(R.drawable.logo)
                                }
                            }
                        }

                        bind.detailIncrement.setOnClickListener {
                            val text = bind.detailQty.text.toString().toInt() + 1
                            bind.detailQty.text = text.toString()
                        }

                        bind.detailDecrement.setOnClickListener {
                            val text = bind.detailQty.text.toString().toInt()
                            if (text > 1) {
                                bind.detailQty.text = (text - 1).toString()
                            }
                        }

                        bind.detailAddToCart.setOnClickListener {
                            for (e in cartArray.length() - 1 downTo 0) {
                                if (getMenu.getString("menuId") == cartArray.getJSONObject(e).getString("menuId")) {
                                    cartArray.getJSONObject(e).apply {
                                        put("qty", cartArray.getJSONObject(e).getInt("qty") + bind.detailQty.text.toString().toInt())
                                    }

                                    edit.putString("cartArray", cartArray.toString())
                                    edit.apply()

                                    Toast.makeText(context, "Success add item to cart!", Toast.LENGTH_SHORT).show()
                                    return@setOnClickListener
                                }
                            }

                            cartArray.apply {
                                put(getMenu.apply {
                                    put("qty", bind.detailQty.text.toString().toInt())
                                })
                            }

                            edit.putString("cartArray", cartArray.toString())
                            edit.apply()

                            Toast.makeText(context, "Success add item to cart!", Toast.LENGTH_SHORT).show()
                        }

                    } else {
                        val error = connection.errorStream.bufferedReader().readText()
                        Toast.makeText(context, "Error : $error", Toast.LENGTH_SHORT).show()
                    }
                }
            }
            catch (e : Exception) {
                Toast.makeText(context, "Error : ${e.message}", Toast.LENGTH_SHORT).show()
            }
        }

        return bind.root
    }
}