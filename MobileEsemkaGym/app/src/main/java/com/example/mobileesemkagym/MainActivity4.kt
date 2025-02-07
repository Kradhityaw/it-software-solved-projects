package com.example.mobileesemkagym

import android.content.Intent
import android.os.Bundle
import android.os.Handler
import android.os.Looper
import android.util.Log
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.lifecycle.lifecycleScope
import com.example.mobileesemkagym.databinding.ActivityMain4Binding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import java.time.LocalDate

class MainActivity4 : AppCompatActivity() {
    private lateinit var bind: ActivityMain4Binding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain4Binding.inflate(layoutInflater)
        setContentView(bind.root)

        val dataLogin = JSONObject(Session.loginData)

        var isRegisted = false
        lifecycleScope.launch {
            while (!isRegisted) {
                withContext(Dispatchers.IO) {
                    val conn = URL("http://10.0.2.2:8081/api/login").openConnection() as HttpURLConnection
                    conn.requestMethod = "POST"
                    conn.setRequestProperty("Content-Type", "application/json")

                    val dataUser = JSONObject().apply {
                        put("email", dataLogin.getJSONObject("user").getString("email"))
                        put("password", Session.userPassword)
                    }

                    conn.outputStream.write(dataUser.toString().toByteArray())

                    val responseCode = conn.responseCode
                    val inputStream = conn.inputStream.bufferedReader().readText()
                    val getjoined = JSONObject(inputStream)

                    if (responseCode in 200..299) {
                        val dateJoin = getjoined.getJSONObject("user")
                        if (dateJoin.getString("joinedMemberAt") != "null") {
                            Session.token = getjoined.getString("token")
                            startActivity(Intent(this@MainActivity4, MainActivity3::class.java).apply {
                                flags = Intent.FLAG_ACTIVITY_CLEAR_TASK or Intent.FLAG_ACTIVITY_NEW_TASK
                            })
                            isRegisted = true
                        }
                    }
                }
                delay(5000)
            }
        }
    }
}