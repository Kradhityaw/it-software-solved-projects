package com.example.androidvoto

import android.content.Intent
import android.os.Bundle
import android.util.Log
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.androidvoto.databinding.ActivityMainBinding
import com.google.android.material.snackbar.Snackbar
import org.json.JSONArray
import org.json.JSONObject

class MainActivity : AppCompatActivity() {
    private lateinit var bind: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMainBinding.inflate(layoutInflater)
        setContentView(bind.root)

        Log.d(
            "dataKeranjang",
            getSharedPreferences("cart", MODE_PRIVATE).getString("cartArray", "[]").toString()
        )

        bind.loginEmail.setText("mahdi@gmail.com")
        bind.loginPassword.setText("1234")
        bind.loginBtn.setOnClickListener {
            val apiHelper = ApiHelper()
            val dataUser = JSONObject().apply {
                put("email", bind.loginEmail.text)
                put("password", bind.loginPassword.text)
            }

            apiHelper.makeRequest(
                url = "http://10.0.2.2:5000/api/auth",
                method = "POST",
                headers = mapOf("Content-Type" to "application/json"),
                body = dataUser.toString(),
            ) { result, error ->
                if (error != null) {
                    Snackbar.make(bind.root, "Error : ${error.message}", Snackbar.LENGTH_LONG)
                        .show()
                } else {
                    Session.token = result.toString()

                    startActivity(Intent(this@MainActivity, MainActivity2::class.java))
                    finish()
                }
            }
        }
    }
}