package com.example.mobileesemkagym

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.lifecycle.lifecycleScope
import com.example.mobileesemkagym.databinding.ActivityMainBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import java.time.LocalDate
import java.time.LocalDateTime

class MainActivity : AppCompatActivity() {
    private lateinit var bind: ActivityMainBinding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMainBinding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.loginEmail.setText("admin@gmail.com")
        bind.loginPassword.setText("admin")

//        bind.loginEmail.setText("ada.lovelace@gmail.com")
//        bind.loginPassword.setText("ada.lovelace")

        bind.loginSignUp.setOnClickListener {
            startActivity(Intent(this@MainActivity, MainActivity2::class.java))
        }

        bind.loginButton.setOnClickListener {
            if (bind.loginEmail.text.isNullOrEmpty() || bind.loginPassword.text.isNullOrEmpty()) {
                Toast.makeText(this@MainActivity, "Please fill all input fields!", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            try {
                GlobalScope.launch(Dispatchers.IO) {
                    val conn = URL("http://10.0.2.2:8081/api/login").openConnection() as HttpURLConnection
                    conn.requestMethod = "POST"
                    conn.setRequestProperty("Content-Type", "application/json")

                    val dataUser = JSONObject().apply {
                        put("email", bind.loginEmail.text)
                        put("password", bind.loginPassword.text)
                    }

                    conn.outputStream.write(dataUser.toString().toByteArray())

                    val responseCode = conn.responseCode

                    runOnUiThread {
                        if (responseCode in 200..299) {
                            GlobalScope.launch(Dispatchers.IO) {
                                val input = conn.inputStream.bufferedReader().readText()
                                val dataLogin = JSONObject(input)

                                runOnUiThread {
                                    Session.token = dataLogin.getString("token")
                                    Session.loginData = dataLogin.toString()

                                    val dateJoin = dataLogin.getJSONObject("user")
                                    if (dateJoin.getString("joinedMemberAt") == "null") {
                                        Session.userPassword = dataUser.getString("password")
                                        startActivity(Intent(this@MainActivity, MainActivity4::class.java))
                                        return@runOnUiThread
                                    }

                                    if (dateJoin.getBoolean("admin")) {
                                        startActivity(Intent(this@MainActivity, MainActivity5::class.java))
                                        finish()
                                        return@runOnUiThread
                                    }

//                                    val dateConvert = LocalDate.parse(dateJoin.getString("membershipEnd"))
//                                    val dateNow = LocalDate.now()
//
//                                    if (dateConvert < dateNow) {
//                                        Session.loginData = dataLogin.toString()
//                                        Session.userPassword = dataUser.getString("password")
//                                        startActivity(Intent(this@MainActivity, MainActivity4::class.java))
//                                        return@runOnUiThread
//                                    }

                                    startActivity(Intent(this@MainActivity, MainActivity3::class.java))
                                    finish()
                                }
                            }
                        } else {
                            Toast.makeText(this@MainActivity, "Failed login to esemka gym!", Toast.LENGTH_SHORT).show()
                        }
                    }
                }
            } catch (e: Exception) {
                e.printStackTrace()
            }
        }
    }
}