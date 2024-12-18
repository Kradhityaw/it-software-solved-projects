package com.example.mobileezemkofi

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.mobileezemkofi.databinding.ActivityLoginBinding
import kotlinx.coroutines.DelicateCoroutinesApi
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

class LoginActivity : AppCompatActivity() {
    private lateinit var bind : ActivityLoginBinding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityLoginBinding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.loginUsername.setText("mahdi")
        bind.loginPassword.setText("1234")

        bind.loginButton.setOnClickListener {
            loginProcess(bind.loginUsername.text.toString(), bind.loginPassword.text.toString())
        }

        bind.loginToregister.setOnClickListener {
            startActivity(Intent(this@LoginActivity, RegisterActivity::class.java))
            finish()
        }
    }

    @OptIn(DelicateCoroutinesApi::class)
    fun loginProcess(username: String, password: String) {
        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:5000/api/auth").openConnection() as HttpURLConnection
            conn.setRequestProperty("Content-Type", "application/json")
            conn.requestMethod = "POST"

            val dataUser = JSONObject().apply {
                put("username", username)
                put("password", password)
            }

            val outputStream = conn.outputStream
            outputStream.write(dataUser.toString().toByteArray())
            outputStream.flush()
            outputStream.close()

            val responseCode = conn.responseCode

            runOnUiThread {
                if (responseCode in 200..299) {
                    val token = conn.inputStream.bufferedReader().readText()
                    Session.token = token
                    startActivity(Intent(this@LoginActivity, MainActivity::class.java))
                    finish()
                } else {
                    Toast.makeText(this@LoginActivity, "Username Or Password is incorrect!", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }
}