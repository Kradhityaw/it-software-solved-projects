package com.example.androidesemkarestaurant

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.androidesemkarestaurant.databinding.ActivityMainBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

class MainActivity : AppCompatActivity() {
    private lateinit var bind: ActivityMainBinding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMainBinding.inflate(layoutInflater)
        setContentView(bind.root)
        bind.homeTable.setText("8tY6mH")

        bind.homeLoginAsStaff.setOnClickListener {
            startActivity(Intent(this@MainActivity, MainActivity3::class.java))
            finish()
        }

        bind.homeSubmit.setOnClickListener {
            if (bind.homeTable.text.isNullOrEmpty()) {
                Toast.makeText(this@MainActivity, "Please fill all input field!", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            GlobalScope.launch(Dispatchers.IO) {
                try {
                    val connection = URL("http://10.0.2.2:5000/Api/Table/${bind.homeTable.text}").openConnection() as HttpURLConnection
                    connection.requestMethod = "POST"
                    connection.setRequestProperty("Content-Type", "application/json")
                    val responseCode = connection.responseCode
                    val inputStream = connection.inputStream.bufferedReader().readText()
                    val dataTable = JSONObject(inputStream)

                    runOnUiThread {
                        if (responseCode in 200..299) {
                            Session.table = "Table ${dataTable.getString("number")}"
                            Session.code = bind.homeTable.text.toString()
                            startActivity(Intent(this@MainActivity, MainActivity2::class.java))
                            finish()
                        } else {
                            val errorStream = connection.errorStream.bufferedReader().readText()
                            Toast.makeText(this@MainActivity, "Error : ${errorStream}", Toast.LENGTH_SHORT).show()
                        }
                    }
                }
                catch (e : Exception) {
                    Toast.makeText(this@MainActivity, "Error : ${e.message}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }
}