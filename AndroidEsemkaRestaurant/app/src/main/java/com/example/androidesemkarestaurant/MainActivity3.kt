package com.example.androidesemkarestaurant

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.androidesemkarestaurant.databinding.ActivityMain3Binding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

class MainActivity3 : AppCompatActivity() {
    private lateinit var bind: ActivityMain3Binding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain3Binding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.loginEmail.setText("iclarricoates3@clickbank.net")
        bind.loginPassword.setText("fTa9aI71rEm")

        bind.loginLoginAsCustomer.setOnClickListener {
            startActivity(Intent(this@MainActivity3, MainActivity::class.java))
            finish()
        }

        bind.loginLogin.setOnClickListener {
            if (bind.loginEmail.text.isNullOrEmpty() || bind.loginPassword.text.isNullOrEmpty()) {
                Toast.makeText(this@MainActivity3, "Please fill all input fiels!", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            val dataUser = JSONObject().apply {
                put("email", bind.loginEmail.text)
                put("password", bind.loginPassword.text)
            }

            GlobalScope.launch(Dispatchers.IO) {
                try {
                    val connection = URL("http://10.0.2.2:5000/Api/Auth").openConnection() as HttpURLConnection
                    connection.requestMethod = "POST"
                    connection.setRequestProperty("Content-Type", "application/json")

                    val outputStream = connection.outputStream
                    outputStream.write(dataUser.toString().toByteArray())
                    outputStream.flush()
                    outputStream.close()

                    val responseCode = connection.responseCode

                    runOnUiThread {
                        if (responseCode in 200..299) {
                            val inputStream = connection.inputStream.bufferedReader().readText()
                            val getToken = JSONObject(inputStream)
                            Session.token = getToken.getString("token")

                            startActivity(Intent(this@MainActivity3, MainActivity4::class.java))
                            finish()
                        }
                        else {
                            Toast.makeText(this@MainActivity3, "incorrect email or password!", Toast.LENGTH_SHORT).show()
                        }
                    }
                }
                catch (e: Exception) {
                    Toast.makeText(this@MainActivity3, "Error : ${e.message}", Toast.LENGTH_SHORT).show()
                }
            }
        }

    }
}