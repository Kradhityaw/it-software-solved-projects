package com.example.mobileesemkagym

import android.os.Bundle
import android.view.ViewGroup
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.mobileesemkagym.MainActivity3.Holder
import com.example.mobileesemkagym.databinding.ActivityMain7Binding
import com.example.mobileesemkagym.databinding.CardAttendanceAdminLayoutBinding
import com.example.mobileesemkagym.databinding.CardAttendanceLayoutBinding
import com.example.mobileesemkagym.databinding.ChartBarFemaleBinding
import com.example.mobileesemkagym.databinding.ChartBarMaleBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

class MainActivity7 : AppCompatActivity() {
    private lateinit var bind: ActivityMain7Binding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain7Binding.inflate(layoutInflater)
        setContentView(bind.root)

        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:8081/api/attendance").openConnection() as HttpURLConnection
            conn.requestMethod = "GET"
            conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

            if (conn.responseCode in 200..299) {
                val dataJadwal = JSONArray(conn.inputStream.bufferedReader().readText())

                var countPeople = 0
                for (e in 0 until dataJadwal.length()) {
                    val parsingChekin = LocalDateTime.parse(dataJadwal.getJSONObject(e).getString("checkIn"))
                    val dateNow = LocalDateTime.now()

                    if (parsingChekin.toString().substring(0, 10) == dateNow.toString().substring(0, 10)) {
                        countPeople++
                    }
                }

                var maleCount = 0
                var femaleCount = 0

                for (e in 0 until dataJadwal.length()) {
                    if (dataJadwal.getJSONObject(e).getJSONObject("user").getString("gender") == "FEMALE") {
                        femaleCount++
                    }
                    else {
                        maleCount++
                    }
                }

                runOnUiThread {
                    bind.countPeople.text = countPeople.toString()

                    bind.maleRv.adapter = object : RecyclerView.Adapter<MaleHolder>() {
                        override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MaleHolder {
                            return MaleHolder(ChartBarMaleBinding.inflate(layoutInflater, parent, false))
                        }

                        override fun getItemCount(): Int {
                            return maleCount
                        }

                        override fun onBindViewHolder(holder: MaleHolder, position: Int) {}
                    }
                    bind.maleRv.layoutManager = LinearLayoutManager(this@MainActivity7, LinearLayoutManager.HORIZONTAL, false)

                    bind.femaleRv.adapter = object : RecyclerView.Adapter<FemaleHolder>() {
                        override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): FemaleHolder {
                            return FemaleHolder(ChartBarFemaleBinding.inflate(layoutInflater, parent, false))
                        }

                        override fun getItemCount(): Int {
                            return femaleCount
                        }

                        override fun onBindViewHolder(holder: FemaleHolder, position: Int) {}
                    }
                    bind.femaleRv.layoutManager = LinearLayoutManager(this@MainActivity7, LinearLayoutManager.HORIZONTAL, false)

                    bind.maleCount.text = maleCount.toString()
                    bind.femaleCount.text = femaleCount.toString()

                    bind.attendaceRv.adapter = object : RecyclerView.Adapter<AttendaceHolder>() {
                        override fun onCreateViewHolder(
                            parent: ViewGroup,
                            viewType: Int
                        ): AttendaceHolder {
                            return AttendaceHolder(CardAttendanceAdminLayoutBinding.inflate(layoutInflater, parent, false))
                        }

                        override fun getItemCount(): Int {
                            return dataJadwal.length()
                        }

                        override fun onBindViewHolder(holder: AttendaceHolder, position: Int) {
                            dataJadwal.getJSONObject(position).let {
                                holder.binding.profileName.text = it.getJSONObject("user").getString("name")
                                holder.binding.checkinDate.text = LocalDateTime.parse(it.getString("checkIn")).format(
                                    DateTimeFormatter.ofPattern("yyyy-MM-dd\nHH:mm:ss"))
                                holder.binding.checkoutDate.text = if (it.getString("checkOut") != "null") LocalDateTime.parse(it.getString("checkOut")).format(
                                    DateTimeFormatter.ofPattern("yyyy-MM-dd\nHH:mm:ss")) else ""

                                if (it.getJSONObject("user").getString("gender") == "FEMALE") {
                                    holder.binding.profilePhoto.setImageResource(R.drawable.female)
                                }
                            }
                        }

                    }
                    bind.attendaceRv.layoutManager = LinearLayoutManager(this@MainActivity7)
                }
            }
        }
    }

    class AttendaceHolder(val binding: CardAttendanceAdminLayoutBinding) : RecyclerView.ViewHolder(binding.root)
    class MaleHolder(val binding: ChartBarMaleBinding) : RecyclerView.ViewHolder(binding.root)
    class FemaleHolder(val binding: ChartBarFemaleBinding) : RecyclerView.ViewHolder(binding.root)
}