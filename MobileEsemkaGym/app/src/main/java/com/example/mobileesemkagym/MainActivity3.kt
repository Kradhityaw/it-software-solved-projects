package com.example.mobileesemkagym

import android.content.DialogInterface
import android.content.Intent
import android.graphics.Color
import android.os.Bundle
import android.renderscript.ScriptGroup.Binding
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.mobileesemkagym.databinding.ActivityMain3Binding
import com.example.mobileesemkagym.databinding.CardAttendanceLayoutBinding
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import java.time.LocalDate
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

class MainActivity3 : AppCompatActivity() {
    private lateinit var bind: ActivityMain3Binding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain3Binding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.homeToolbar.setOnMenuItemClickListener {
            when (it?.itemId) {
                R.id.logout -> {
                    startActivity(Intent(this@MainActivity3, MainActivity::class.java))
                    finish()
                }
            }
            true
        }

        bind.homeDate.text = LocalDateTime.now().format(DateTimeFormatter.ofPattern("dd MMMM yyyy"))

        bind.homeCheckIn.setOnClickListener {
            val dataLogin = JSONObject(Session.loginData)
            val dateConvert = LocalDate.parse(dataLogin.getJSONObject("user").getString("membershipEnd"))
            val dateNow = LocalDate.now()

            if (dateConvert < dateNow) {
                val alert = MaterialAlertDialogBuilder(this@MainActivity3).apply {
                    setTitle("Membership Ended")
                    setMessage("Your membership has ended at ${dateConvert}, resume your membership when visiting the gym next time")
                    setPositiveButton("Ok", null)
                }
                alert.show()
                return@setOnClickListener
            }
            
            if (bind.homeCode.text.isNullOrEmpty()) {
                bind.homeCode.error = "Please input daily checkin code!"
                return@setOnClickListener
            }

            try {
                GlobalScope.launch(Dispatchers.IO) {
                    val conn = URL("http://10.0.2.2:8081/api/attendance/checkin/${bind.homeCode.text}").openConnection() as HttpURLConnection
                    conn.requestMethod = "POST"
                    conn.setRequestProperty("Content-Type", "application/json")
                    conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

                    val responseCode = conn.responseCode
                    
                    runOnUiThread {
                        if (responseCode in 200..299) {
                            bind.homeCheckIn.visibility = View.GONE
                            bind.homeCheckOut.visibility = View.VISIBLE
                            bind.homeCodeWrapper.visibility = View.INVISIBLE
                            bind.homeCode.visibility = View.INVISIBLE

                            getLog()
                            Toast.makeText(this@MainActivity3, "Success check in!", Toast.LENGTH_SHORT).show()
                        } else {
                            bind.homeCode.error = "Code is not valid!"
                        }
                    }
                }
            } catch (e: Exception) {
                e.printStackTrace()
            }
        }

        bind.homeCheckOut.setOnClickListener {
            try {
                GlobalScope.launch(Dispatchers.IO) {
                    val conn = URL("http://10.0.2.2:8081/api/attendance/checkout").openConnection() as HttpURLConnection
                    conn.requestMethod = "POST"
                    conn.setRequestProperty("Content-Type", "application/json")
                    conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

                    val responseCode = conn.responseCode

                    runOnUiThread {
                        if (responseCode in 200..299) {
                            bind.homeCheckIn.visibility = View.VISIBLE
                            bind.homeCheckOut.visibility = View.GONE
                            bind.homeCodeWrapper.visibility = View.VISIBLE
                            bind.homeCode.visibility = View.VISIBLE

                            bind.homeCode.setText("")

                            Toast.makeText(this@MainActivity3, "Success check out!", Toast.LENGTH_SHORT).show()
                            getLog()
                        } else {
                            Toast.makeText(this@MainActivity3, "Failed to checkout!", Toast.LENGTH_SHORT).show()
                        }
                    }
                }
            } catch (e: Exception) {
                e.printStackTrace()
            }
        }

        val dataLogin = JSONObject(Session.loginData)
        val dateConvert = LocalDate.parse(dataLogin.getJSONObject("user").getString("membershipEnd"))
        val dateNow = LocalDate.now()

        if (dateConvert < dateNow) {
            bind.homeCode.isEnabled = false
        }

        getLog()
    }

    private fun getLog() {
        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:8081/api/attendance").openConnection() as HttpURLConnection
            conn.requestMethod = "GET"
            conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

            if (conn.responseCode in 200..299) {
                val dataJadwal = JSONArray(conn.inputStream.bufferedReader().readText())

                for (e in 0 until dataJadwal.length()) {
                    if (dataJadwal.getJSONObject(e).getJSONObject("user").getInt("id") != JSONObject(Session.loginData).getJSONObject("user").getInt("id")) {
                        dataJadwal.remove(e)
                    }
                }

                runOnUiThread {
                    val adapter = object : RecyclerView.Adapter<Holder>() {
                        override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): Holder {
                            return Holder(CardAttendanceLayoutBinding.inflate(layoutInflater, parent, false))
                        }

                        override fun getItemCount(): Int = dataJadwal.length()

                        override fun onBindViewHolder(holder: Holder, position: Int) {
                            dataJadwal.getJSONObject(position).let {
                                val parsingChekin = LocalDateTime.parse(it.getString("checkIn")).format(
                                    DateTimeFormatter.ofPattern("dd MMMM yyyy HH:mm"))

                                var getDateOut = ""

                                if (it.getString("checkOut") != "null") {
                                    getDateOut = LocalDateTime.parse(it.getString("checkOut")).format(
                                        DateTimeFormatter.ofPattern("dd MMMM yyyy HH:mm"))
                                }

                                holder.binding.checkinDate.text = parsingChekin
                                holder.binding.checkoutDate.text = getDateOut
                            }
                        }
                    }
                    bind.attendanceRv.adapter = adapter
                    bind.attendanceRv.layoutManager = LinearLayoutManager(this@MainActivity3)
                }
            }
        }
    }

    class Holder(val binding: CardAttendanceLayoutBinding) : RecyclerView.ViewHolder(binding.root)
}