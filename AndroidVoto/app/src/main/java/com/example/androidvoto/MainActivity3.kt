package com.example.androidvoto

import android.content.Intent
import android.graphics.BitmapFactory
import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.activity.result.contract.ActivityResultContracts
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.androidvoto.databinding.ActivityMain3Binding
import com.google.android.material.snackbar.Snackbar
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import org.json.JSONObject
import java.net.URL
import java.text.NumberFormat
import java.util.Locale

class MainActivity3 : AppCompatActivity() {
    private lateinit var bind: ActivityMain3Binding


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain3Binding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.backButton.setOnClickListener {
            finish()
        }

        cartLogic()
    }

    private fun cartLogic() {
        val apiHelper = ApiHelper()

        apiHelper.makeRequest(
            url = "http://10.0.2.2:5000/api/camera/${intent.getStringExtra("id")}",
            headers = mapOf("Authorization" to "Bearer ${Session.token}")
        ) { result, error ->
            if (error != null) {
                Snackbar.make(bind.root, "Error : ${error.message}", Snackbar.LENGTH_LONG)
                    .show()
            } else {
                val rupiahFormat = NumberFormat.getCurrencyInstance(Locale("in", "ID"))
                val shared = getSharedPreferences("cart", MODE_PRIVATE)
                val editor = shared.edit()
                val sharedGet = shared.getString("cartArray", "[]")
                val arrayCart = JSONArray(sharedGet)

                JSONObject(result.toString()).let { cam ->
                    bind.cameraName.text = cam.getString("name")
                    bind.cameraStore.text = cam.getString("sellerShop")
                    bind.cameraSensor.text = cam.getString("sensor")
                    bind.cameraResolution.text = cam.getString("resolution")
                    bind.cameraAutoFocus.text = cam.getString("autoFocusSystem")
                    bind.cameraIso.text = cam.getString("isoRange")
                    bind.cameraShutterSpeed.text = cam.getString("shuterSpeedRange")
                    bind.cameraDimension.text = cam.getString("dimensions")
                    bind.cameraWeight.text = "${cam.getString("weight")}g"
                    bind.cameraWifi.text = if (cam.getBoolean("wiFi")) "Yes" else "No"
                    bind.cameraTouchScreen.text =
                        if (cam.getBoolean("touchScreen")) "Yes" else "No"
                    bind.cameraFlash.text = if (cam.getBoolean("flash")) "Yes" else "No"
                    bind.cameraBluetooth.text = if (cam.getBoolean("bluetooth")) "Yes" else "No"
                    bind.cameraPrice.text = rupiahFormat.format(cam.getDouble("price"))

                    bind.buttonCheckout.setOnClickListener {
                        for (e in 0 until arrayCart.length()) {
                            for (i in 0 until arrayCart.getJSONObject(e).getJSONArray("items")
                                .length()) {
                                val items = arrayCart.getJSONObject(e).getJSONArray("items")
                                    .getJSONObject(i)
                                if (cam.getString("name") == items.getString("name")) {
                                    items.put("qty", items.getInt("qty") + 1)

                                    editor.putString("cartArray", arrayCart.toString())
                                    editor.apply()

                                    Toast.makeText(
                                        this@MainActivity3,
                                        "Complete add ${cam.getString("name")} to cart!",
                                        Toast.LENGTH_SHORT
                                    ).show()
                                    startActivity(
                                        Intent(
                                            this@MainActivity3,
                                            MainActivity4::class.java
                                        )
                                    )
                                    return@setOnClickListener
                                }
                            }

                            if (arrayCart.getJSONObject(e)
                                    .getString("seller") == cam.getString("sellerShop")
                            ) {
                                arrayCart.getJSONObject(e).getJSONArray("items").apply {
                                    put(JSONObject().apply {
                                        put("id", cam.getInt("id"))
                                        put("name", cam.getString("name"))
                                        put("qty", 1)
                                        put("price", cam.getInt("price"))
                                        put("photo", cam.getString("photo"))
                                        put("isChecked", true)
                                    })
                                }

                                editor.putString("cartArray", arrayCart.toString())
                                editor.apply()

                                Toast.makeText(
                                    this@MainActivity3,
                                    "Complete add ${cam.getString("name")} to cart!",
                                    Toast.LENGTH_SHORT
                                ).show()
                                startActivity(Intent(this@MainActivity3, MainActivity4::class.java))
                                return@setOnClickListener
                            }
                        }

                        arrayCart.apply {
                            put(JSONObject().apply {
                                put("seller", cam.getString("sellerShop"))
                                put("isCheked", true)
                                put("items", JSONArray().apply {
                                    put(JSONObject().apply {
                                        put("id", cam.getInt("id"))
                                        put("name", cam.getString("name"))
                                        put("qty", 1)
                                        put("price", cam.getInt("price"))
                                        put("photo", cam.getString("photo"))
                                        put("isChecked", true)
                                    })
                                })
                            })
                        }

                        editor.putString("cartArray", arrayCart.toString())
                        editor.apply()

                        Toast.makeText(
                            this@MainActivity3,
                            "Complete add ${cam.getString("name")} to cart!",
                            Toast.LENGTH_SHORT
                        ).show()
                        startActivity(Intent(this@MainActivity3, MainActivity4::class.java))
                    }

                    GlobalScope.launch(Dispatchers.IO) {
                        val image =
                            BitmapFactory.decodeStream(
                                URL(
                                    "http://10.0.2.2:5000/images/${
                                        cam.getString(
                                            "photo"
                                        )
                                    }"
                                ).openStream()
                            )
                        runOnUiThread {
                            bind.cameraImage.setImageBitmap(image)
                        }
                    }
                }
            }
        }
    }
}