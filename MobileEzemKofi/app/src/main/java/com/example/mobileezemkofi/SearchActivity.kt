package com.example.mobileezemkofi

import android.annotation.SuppressLint
import android.content.Intent
import android.graphics.BitmapFactory
import android.os.Bundle
import android.text.Editable
import android.view.ViewGroup
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.core.widget.addTextChangedListener
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.mobileezemkofi.MainActivity.CoffeHolder
import com.example.mobileezemkofi.databinding.ActivitySearchBinding
import com.example.mobileezemkofi.databinding.CardCoffeeLayoutBinding
import com.example.mobileezemkofi.databinding.CardTopPicksLayoutBinding
import kotlinx.coroutines.DelicateCoroutinesApi
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import java.net.HttpURLConnection
import java.net.URL

class SearchActivity : AppCompatActivity() {
    private lateinit var bind : ActivitySearchBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivitySearchBinding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.detailBack.setOnClickListener {
            finish()
        }

        bind.searchCoffee.addTextChangedListener { text: Editable? ->  getCoffies(text.toString())  }

        getCoffies("")
    }

    @OptIn(DelicateCoroutinesApi::class)
    private fun getCoffies(key: String) {
        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:5000/api/coffee?search=$key").openConnection() as HttpURLConnection
            conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

            val inputStream = conn.inputStream.bufferedReader().readText()
            val convertToJsonArray = JSONArray(inputStream)

            runOnUiThread {
                bind.coffeRv.adapter = object : RecyclerView.Adapter<CoffeeHolder>() {
                    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): CoffeeHolder {
                        val inflate = CardTopPicksLayoutBinding.inflate(layoutInflater, parent, false)
                        return CoffeeHolder(inflate)
                    }

                    @SuppressLint("SetTextI18n")
                    override fun onBindViewHolder(holder: CoffeeHolder, position: Int) {
                        val getData = convertToJsonArray.getJSONObject(position)
                        holder.binding.cardName.text = getData.getString("name")
                        holder.binding.cardRating.text = getData.getDouble("rating").toString()
                        holder.binding.cardPrice.text = getData.getDouble("price").toString()

                        GlobalScope.launch(Dispatchers.IO) {
                            val image = BitmapFactory.decodeStream(URL("http://10.0.2.2:5000/images/${getData.getString("imagePath")}").openStream())

                            runOnUiThread {
                                holder.binding.cardImage.setImageBitmap(image)
                            }
                        }

                        holder.itemView.setOnClickListener {
                            startActivity(Intent(this@SearchActivity, DetailActivity::class.java).apply {
                                putExtra("idCoffee", getData.getString("id"))
                            })
                        }
                    }

                    override fun getItemCount(): Int = convertToJsonArray.length()
                }
                bind.coffeRv.layoutManager = LinearLayoutManager(this@SearchActivity, LinearLayoutManager.VERTICAL, false)
            }
        }
    }

    class CoffeeHolder(val binding: CardTopPicksLayoutBinding) : RecyclerView.ViewHolder(binding.root)
}