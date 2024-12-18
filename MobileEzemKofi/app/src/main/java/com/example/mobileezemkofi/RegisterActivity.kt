package com.example.mobileezemkofi

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.mobileezemkofi.databinding.ActivityRegisterBinding
import kotlinx.coroutines.DelicateCoroutinesApi
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

class RegisterActivity : AppCompatActivity() {
    private lateinit var bind : ActivityRegisterBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityRegisterBinding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.registerTologin.setOnClickListener {
            startActivity(Intent(this@RegisterActivity, LoginActivity::class.java))
            finish()
        }

        bind.registerButton.setOnClickListener {
            if (bind.registerPassword.text.toString().length < 4) {
                Toast.makeText(this@RegisterActivity, "Password must more than 4 character!", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            if (bind.registerConfirmpassword.text.toString() != bind.registerPassword.text.toString()) {
                Toast.makeText(this@RegisterActivity, "Confirm password not same with password!", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            registerProcess(bind.registerUsername.text.toString(), bind.registerFullname.text.toString(), bind.registerEmail.text.toString(), bind.registerConfirmpassword.text.toString())
        }
    }

    @OptIn(DelicateCoroutinesApi::class)
    private fun registerProcess(username: String, fullName: String, email: String, password: String) {
        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:5000/api/register").openConnection() as HttpURLConnection
            conn.requestMethod = "POST"
            conn.setRequestProperty("Content-Type", "application/json")

            val registerData = JSONObject().apply {
                put("username", username)
                put("fullname", fullName)
                put("email", email)
                put("password", password)
            }

            val outpustSteram = conn.outputStream
            outpustSteram.write(registerData.toString().toByteArray())
            outpustSteram.flush()
            outpustSteram.close()

            val responseCode = conn.responseCode

            runOnUiThread {
                if (responseCode in 200..299) {
                    val token = conn.inputStream.bufferedReader().readText()
                    Session.token = token
                    startActivity(Intent(this@RegisterActivity, MainActivity::class.java))
                    finish()
                }
                else {
                    Toast.makeText(this@RegisterActivity, "Failed to register!", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }
}