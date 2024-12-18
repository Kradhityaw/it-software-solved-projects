package com.example.mobileezemkofi

import android.annotation.SuppressLint
import android.app.Activity
import android.graphics.BitmapFactory
import android.icu.text.DecimalFormat
import android.os.Bundle
import android.util.Log
import android.view.ViewGroup
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.mobileezemkofi.databinding.ActivityCartBinding
import com.example.mobileezemkofi.databinding.CardCartLayoutBinding
import kotlinx.coroutines.DelicateCoroutinesApi
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

class CartActivity : AppCompatActivity() {
    private lateinit var bind : ActivityCartBinding

    @OptIn(DelicateCoroutinesApi::class)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityCartBinding.inflate(layoutInflater)
        setContentView(bind.root)
        loadCart()

        bind.detailBack.setOnClickListener {
            finish()
        }
    }

    private fun loadCart() {
        val shared = getSharedPreferences("cardCoffee", Activity.MODE_PRIVATE)
        val editor = shared.edit()

        val jsonArr = JSONArray(shared.getString("coffeeData", "[]"))
        val jsonCart = JSONArray()

        for (e in 0 until jsonArr.length()) {
            val ob = jsonArr.getJSONObject(e)
            jsonCart.put(JSONObject().apply {
                put("coffeeId", ob.getInt("id"))
                put("size", ob.getString("size"))
                put("qty", ob.getInt("qty"))
            })
        }

        bind.cartRv.adapter = object : RecyclerView.Adapter<CartHolder>() {
            override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): CartHolder {
                val inflate = CardCartLayoutBinding.inflate(layoutInflater, parent, false)
                return CartHolder(inflate)
            }

            @SuppressLint("SetTextI18n")
            override fun onBindViewHolder(holder: CartHolder, position: Int) {
                val data = jsonArr.getJSONObject(position)
                val format = DecimalFormat("#.##")

                holder.binding.cardName.text = data.getString("name")
                holder.binding.cardCategory.text = data.getString("category")
                holder.binding.cardPrice.text = "$${format.format(data.getDouble("totalPrice"))}"
                holder.binding.cardQty.text = data.getString("qty")
                holder.binding.cardSize.text = "Size: ${data.getString("size")}"

                GlobalScope.launch(Dispatchers.IO) {
                    val image = BitmapFactory.decodeStream(URL("http://10.0.2.2:5000/images/${data.getString("imagePath")}").openStream())

                    runOnUiThread {
                        holder.binding.cardImage.setImageBitmap(image)
                    }
                }

                holder.binding.cardIncrement.setOnClickListener {
                    holder.binding.cardQty.text = (holder.binding.cardQty.text.toString().toInt() + 1).toString()
                    holder.binding.cardPrice.text = "$${data.getDouble("priceSize") * holder.binding.cardQty.text.toString().toInt()}"
                    jsonCart.getJSONObject(position).put("qty", holder.binding.cardQty.text.toString().toInt())
                }

                holder.binding.cardDecrement.setOnClickListener {
                    if (holder.binding.cardQty.text.toString().toInt() == 1) {
                        jsonArr.remove(position)
                        jsonCart.remove(position)
                        notifyDataSetChanged()
                    } else {
                        holder.binding.cardQty.text = (holder.binding.cardQty.text.toString().toInt() - 1).toString()
                        holder.binding.cardPrice.text = "$${data.getDouble("priceSize") * holder.binding.cardQty.text.toString().toInt()}"
                        jsonCart.getJSONObject(position).put("qty", holder.binding.cardQty.text.toString().toInt())
                        jsonArr.getJSONObject(position).put("qty", holder.binding.cardQty.text.toString().toInt())
                        notifyDataSetChanged()
                    }
                    editor.putString("coffeeData", jsonArr.toString())
                    editor.apply()
                }

                bind.checkoutBtn.setOnClickListener {
                    if (jsonArr.length() < 1) {
                        Toast.makeText(this@CartActivity, "Cannot checkout with empty cart!", Toast.LENGTH_SHORT).show()
                        return@setOnClickListener
                    }
                    GlobalScope.launch(Dispatchers.IO) {
                        val conn = URL("http://10.0.2.2:5000/api/checkout").openConnection() as HttpURLConnection
                        conn.requestMethod = "POST"
                        conn.setRequestProperty("Content-Type", "application/json")
                        conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

                        conn.outputStream.write(jsonCart.toString().toByteArray())

                        val resCode = conn.responseCode

                        runOnUiThread {
                            if (resCode in 200..299) {
                                Toast.makeText(this@CartActivity, "Success checkout coffees!", Toast.LENGTH_SHORT).show()
                                editor.clear()
                                editor.apply()
                                loadCart()
                            } else {
                                Toast.makeText(this@CartActivity, "Failed checkout coffees!", Toast.LENGTH_SHORT).show()
                            }
                        }
                    }
                }
            }

            override fun getItemCount(): Int = jsonArr.length()

        }

        bind.cartRv.layoutManager = LinearLayoutManager(this@CartActivity)
    }

    class CartHolder(val binding: CardCartLayoutBinding) : RecyclerView.ViewHolder(binding.root)
}