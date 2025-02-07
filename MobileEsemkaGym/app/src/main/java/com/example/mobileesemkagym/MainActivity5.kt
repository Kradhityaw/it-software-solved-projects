package com.example.mobileesemkagym

import android.content.Intent
import android.os.Bundle
import android.view.MenuItem
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.ActionBarDrawerToggle
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.mobileesemkagym.databinding.ActivityMain5Binding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

class MainActivity5 : AppCompatActivity() {
    private lateinit var bind: ActivityMain5Binding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain5Binding.inflate(layoutInflater)
        setContentView(bind.root)

        setSupportActionBar(bind.adminToolbar)
        ActionBarDrawerToggle(this@MainActivity5, bind.main, bind.adminToolbar, R.string.app_name, R.string.app_name).let {
            bind.main.addDrawerListener(it)
            it.syncState()
        }

        bind.adminNavigationView.setNavigationItemSelectedListener {
            when (it.itemId) {
                R.id.manage_member -> startActivity(Intent(this@MainActivity5, MainActivity6::class.java))
                R.id.logout -> {
                    startActivity(Intent(this@MainActivity5, MainActivity::class.java))
                    Session.token = ""
                    finish()
                }
                R.id.report -> startActivity(Intent(this@MainActivity5, MainActivity7::class.java))
                R.id.checkin_code -> bind.main.closeDrawer(bind.adminNavigationView)
            }
            false
        }

        bind.dateNow.text = LocalDateTime.now().format(DateTimeFormatter.ofPattern("dd MMMM yyyy"))

        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:8081/api/attendance/checkin/code").openConnection() as HttpURLConnection
            conn.requestMethod = "GET"
            conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

            if (conn.responseCode in 200..299) {
                val inputStream = conn.inputStream.bufferedReader().readText()
                val dataKode = JSONObject(inputStream)

                runOnUiThread {
                    bind.codeCode.setText(dataKode.getString("code"))
                }
            }
        }
    }
}