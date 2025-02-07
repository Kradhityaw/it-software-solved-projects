package com.example.mobileesemkagym

import android.content.Intent
import android.os.Bundle
import android.widget.RadioButton
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.core.view.children
import com.example.mobileesemkagym.databinding.ActivityMain2Binding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import java.time.LocalDate

class MainActivity2 : AppCompatActivity() {
    private lateinit var bind: ActivityMain2Binding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain2Binding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.loginSignIp.setOnClickListener {
            finish()
        }

        bind.loginButton.setOnClickListener {
            if (bind.loginEmail.text.isNullOrEmpty() || bind.loginPassword.text.isNullOrEmpty() || bind.loginName.text.isNullOrEmpty()) {
                Toast.makeText(this@MainActivity2, "Please fill all input fields!", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            if (bind.loginRadio.children.count { f -> (f as RadioButton).isChecked } == 0) {
                Toast.makeText(this@MainActivity2, "Plase select gender!", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            try {
                GlobalScope.launch(Dispatchers.IO) {
                    val conn = URL("http://10.0.2.2:8081/api/signup").openConnection() as HttpURLConnection
                    conn.requestMethod = "POST"
                    conn.setRequestProperty("Content-Type", "application/json")

                    val dataUser = JSONObject().apply {
                        put("name", bind.loginName.text)
                        put("email", bind.loginEmail.text)
                        put("password", bind.loginPassword.text)
                        put("gender", if ((bind.loginRadio.getChildAt(0) as RadioButton).isChecked) "Male" else "Female")
                    }

                    conn.outputStream.write(dataUser.toString().toByteArray())

                    val responseCode = conn.responseCode

                    runOnUiThread {
                        if (responseCode in 200..299) {
                            Toast.makeText(this@MainActivity2, "Registration completes! Enter your email and password to login!", Toast.LENGTH_SHORT).show()
                            finish()
                        } else {
                            Toast.makeText(this@MainActivity2, "Failed register to esemka gym!", Toast.LENGTH_SHORT).show()
                        }
                    }
                }
            } catch (e: Exception) {
                e.printStackTrace()
            }
        }
    }
}