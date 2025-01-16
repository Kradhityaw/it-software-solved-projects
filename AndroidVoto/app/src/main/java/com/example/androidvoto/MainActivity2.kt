package com.example.androidvoto

import android.content.Intent
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.graphics.Color
import android.os.Bundle
import android.util.Log
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.core.widget.addTextChangedListener
import androidx.recyclerview.widget.GridLayoutManager
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.androidvoto.databinding.ActivityMain2Binding
import com.example.androidvoto.databinding.CardCameraLayoutBinding
import com.google.android.material.snackbar.Snackbar
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import org.json.JSONObject
import java.net.URL
import java.text.NumberFormat
import java.util.Locale

class MainActivity2 : AppCompatActivity() {
    private lateinit var bind: ActivityMain2Binding
    private var categoryId = ""
    private var textSerach = ""

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain2Binding.inflate(layoutInflater)
        setContentView(bind.root)
        getCamera(textSerach, categoryId)

        val apiHelper = ApiHelper()

        bind.cart.setOnClickListener {
            startActivity(Intent(this@MainActivity2, MainActivity4::class.java))
        }

        bind.history.setOnClickListener {
            startActivity(Intent(this@MainActivity2, MainActivity6::class.java))
        }

        bind.luckySpin.setOnClickListener {
            startActivity(Intent(this@MainActivity2, MainActivity7::class.java))
        }

        apiHelper.makeRequest(
            url = "http://10.0.2.2:5000/api/category",
            headers = mapOf("Authorization" to "Bearer ${Session.token}")
        ) { result, error ->
            if (error != null) {
                Snackbar.make(bind.root, "Error : ${error.message}", Snackbar.LENGTH_LONG)
                    .show()
            } else {
                val listItem = JSONArray(result)
                listItem.put(JSONObject().apply {
                    put("id", 0)
                    put("name", "-No Filter-")
                })
                val listCategory = mutableListOf<String>()

                for (e in 0 until listItem.length()) {
                    listCategory.add(listItem.getJSONObject(e).getString("name"))
                }

                bind.categoryAutoComplete.setAdapter(
                    ArrayAdapter(
                        this,
                        android.R.layout.simple_dropdown_item_1line,
                        listCategory
                    )
                )

                bind.categoryAutoComplete.setOnItemClickListener { adapterView, view, i, l ->
                    val pos = listItem.getJSONObject(i)

                    if (pos.getString("id") == "0") {
                        bind.categoryAutoComplete.text.clear()
                        getCamera(textSerach, "")
                        categoryId = ""
                        return@setOnItemClickListener
                    }

                    getCamera(textSerach, pos.getString("id"))
                    categoryId = pos.getString("id")
                }
            }
        }

        bind.searchCamera.addTextChangedListener { text ->
            getCamera(text.toString(), categoryId)
            textSerach = text.toString()
        }
    }

    private fun getCamera(text: String, categoryId: String) {
        val apiHelper = ApiHelper()
        val rupiahFormatter = NumberFormat.getCurrencyInstance(Locale("in", "ID"))

        apiHelper.makeRequest(
            url = "http://10.0.2.2:5000/api/camera?categoryID=$categoryId&search=$text",
            headers = mapOf("Authorization" to "Bearer ${Session.token}"),
        ) { result, error ->
            if (error != null) {
                Snackbar.make(bind.root, "Error : ${error.message}", Snackbar.LENGTH_LONG)
                    .show()
            } else {
                val dataCamera = JSONArray(result)
                val cameraAdapter = object : RecyclerView.Adapter<CameraHolder>() {
                    override fun onCreateViewHolder(
                        parent: ViewGroup,
                        viewType: Int
                    ): CameraHolder {
                        val view = CardCameraLayoutBinding.inflate(layoutInflater, parent, false)
                        return CameraHolder(view)
                    }

                    override fun getItemCount(): Int = dataCamera.length()

                    override fun onBindViewHolder(holder: CameraHolder, position: Int) {
                        dataCamera.getJSONObject(position).let { cam ->
                            holder.binding.cameraName.text = cam.getString("name")
                            holder.binding.cameraPixel.text = cam.getString("resolution")
                            holder.binding.cameraPrice.text =
                                rupiahFormatter.format(cam.getDouble("price"))

                            GlobalScope.launch(Dispatchers.IO) {
                                val image = BitmapFactory.decodeStream(
                                    URL(
                                        "http://10.0.2.2:5000/images/${
                                            cam.getString("photo")
                                        }"
                                    ).openStream()
                                )
                                runOnUiThread {
                                    holder.binding.cameraImage.setImageBitmap(image)
                                }
                            }

                            holder.itemView.setOnClickListener {
                                startActivity(
                                    Intent(
                                        this@MainActivity2,
                                        MainActivity3::class.java
                                    ).apply {
                                        putExtra("id", cam.getString("id"))
                                    })
                            }
                        }
                    }
                }

                bind.homeRv.adapter = cameraAdapter
                bind.homeRv.layoutManager = GridLayoutManager(this@MainActivity2, 2)
            }
        }
    }

    class CameraHolder(val binding: CardCameraLayoutBinding) : RecyclerView.ViewHolder(binding.root)
}