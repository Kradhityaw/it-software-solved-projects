package com.example.mobileezemkofi

import android.annotation.SuppressLint
import android.content.Intent
import android.graphics.BitmapFactory
import android.os.Bundle
import android.view.ViewGroup
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.mobileezemkofi.databinding.ActivityMainBinding
import com.example.mobileezemkofi.databinding.CardCoffeeLayoutBinding
import com.example.mobileezemkofi.databinding.CardTopPicksLayoutBinding
import com.google.android.material.tabs.TabLayout
import com.google.android.material.tabs.TabLayout.OnTabSelectedListener
import kotlinx.coroutines.DelicateCoroutinesApi
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

class MainActivity : AppCompatActivity() {
    private lateinit var bind : ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMainBinding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.searchBtn.setOnClickListener {
            startActivity(Intent(this@MainActivity, SearchActivity::class.java))
        }

        bind.mainToCart.setOnClickListener {
            startActivity(Intent(this@MainActivity, CartActivity::class.java))
        }

        getCoffies("4")
        getCategories()
        getTopPicks()
        getUserData()
    }

    @OptIn(DelicateCoroutinesApi::class)
    private fun getCategories() {
        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:5000/api/coffee-category").openConnection() as HttpURLConnection
            conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

            val inputStream = conn.inputStream.bufferedReader().readText()
            val convertToJsonArray = JSONArray(inputStream)

            runOnUiThread {
                for (e in 0 until convertToJsonArray.length()) {
                    val getObject = convertToJsonArray.getJSONObject(e)
                    val tab = bind.categotyTl.newTab()
                    tab.text = getObject.getString("name")
                    tab.tag = getObject
                    bind.categotyTl.addTab(tab)
                }

                bind.categotyTl.addOnTabSelectedListener(object : OnTabSelectedListener {
                    override fun onTabSelected(tab: TabLayout.Tab?) {
                        val cat = tab?.tag as JSONObject
                        getCoffies(cat.getString("id"))
                    }

                    override fun onTabUnselected(tab: TabLayout.Tab?) {}

                    override fun onTabReselected(tab: TabLayout.Tab?) {
                        bind.categotyTl.selectTab(null, true)
                    }
                })
            }
        }
    }

    @OptIn(DelicateCoroutinesApi::class)
    private fun getCoffies(catId: String) {
        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:5000/api/coffee?coffeeCategoryID=$catId").openConnection() as HttpURLConnection
            conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

            val inputStream = conn.inputStream.bufferedReader().readText()
            val convertToJsonArray = JSONArray(inputStream)

            runOnUiThread {
                bind.coffeRv.adapter = object : RecyclerView.Adapter<CoffeHolder>() {
                    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): CoffeHolder {
                        val inflate = CardCoffeeLayoutBinding.inflate(layoutInflater, parent, false)
                        return CoffeHolder(inflate)
                    }

                    @SuppressLint("SetTextI18n")
                    override fun onBindViewHolder(holder: CoffeHolder, position: Int) {
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
                            startActivity(Intent(this@MainActivity, DetailActivity::class.java).apply {
                                putExtra("idCoffee", getData.getString("id"))
                            })
                        }
                    }

                    override fun getItemCount(): Int = convertToJsonArray.length()
                }
                bind.coffeRv.layoutManager = LinearLayoutManager(this@MainActivity, LinearLayoutManager.HORIZONTAL, false)
            }
        }
    }

    @OptIn(DelicateCoroutinesApi::class)
    private fun getTopPicks() {
        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:5000/api/coffee/top-picks").openConnection() as HttpURLConnection
            conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

            val inputStream = conn.inputStream.bufferedReader().readText()
            val convertToJsonArray = JSONArray(inputStream)

            runOnUiThread {
                bind.topPicksRv.adapter = object : RecyclerView.Adapter<TopPicksHolder>() {
                    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): TopPicksHolder {
                        val inflate = CardTopPicksLayoutBinding.inflate(layoutInflater, parent, false)
                        return TopPicksHolder(inflate)
                    }

                    @SuppressLint("SetTextI18n")
                    override fun onBindViewHolder(holder: TopPicksHolder, position: Int) {
                        val getData = convertToJsonArray.getJSONObject(position)
                        holder.binding.cardName.text = getData.getString("name")
                        holder.binding.cardRating.text = getData.getDouble("rating").toString()
                        holder.binding.cardPrice.text = "$${getData.getDouble("price")}"

                        GlobalScope.launch(Dispatchers.IO) {
                            val image = BitmapFactory.decodeStream(URL("http://10.0.2.2:5000/images/${getData.getString("imagePath")}").openStream())

                            runOnUiThread {
                                holder.binding.cardImage.setImageBitmap(image)
                            }
                        }

                        holder.itemView.setOnClickListener {
                            startActivity(Intent(this@MainActivity, DetailActivity::class.java).apply {
                                putExtra("idCoffee", getData.getString("id"))
                            })
                        }
                    }

                    override fun getItemCount(): Int = convertToJsonArray.length()
                }
                bind.topPicksRv.layoutManager = LinearLayoutManager(this@MainActivity)
            }
        }
    }

    @OptIn(DelicateCoroutinesApi::class)
    private fun getUserData() {
        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:5000/api/me").openConnection() as HttpURLConnection
            conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

            val inputStream = conn.inputStream.bufferedReader().readText()
            val getUser = JSONObject(inputStream)

            runOnUiThread {
                bind.userFullName.text = getUser.getString("fullName")
            }
        }
    }

    class CoffeHolder(val binding: CardCoffeeLayoutBinding) : RecyclerView.ViewHolder(binding.root)
    class TopPicksHolder(val binding: CardTopPicksLayoutBinding) : RecyclerView.ViewHolder(binding.root)
}