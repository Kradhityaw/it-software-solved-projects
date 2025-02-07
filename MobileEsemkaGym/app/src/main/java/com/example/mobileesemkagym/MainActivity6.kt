package com.example.mobileesemkagym

import android.os.Bundle
import android.view.View
import android.view.ViewGroup
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.core.widget.addTextChangedListener
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.mobileesemkagym.databinding.ActivityMain6Binding
import com.example.mobileesemkagym.databinding.CardMemberLayoutBinding
import com.google.android.material.tabs.TabLayout
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import java.net.HttpURLConnection
import java.net.URL
import java.time.LocalDate
import java.time.temporal.ChronoUnit

class MainActivity6 : AppCompatActivity() {
    private lateinit var bind: ActivityMain6Binding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain6Binding.inflate(layoutInflater)
        setContentView(bind.root)

        var status = "ACTIVE"

        bind.memberTab.addOnTabSelectedListener(object : TabLayout.OnTabSelectedListener {
            override fun onTabSelected(tab: TabLayout.Tab?) {
                when (tab?.position) {
                    0 -> {
                        getMember(bind.memberSearch.text.toString(),"ACTIVE")
                        status = "ACTIVE"
                    }
                    1 -> {
                        getMember(bind.memberSearch.text.toString(),"INACTIVE")
                        status = "INACTIVE"
                    }
                    2 -> {
                        getMember(bind.memberSearch.text.toString(),"PENDING_APPROVAL")
                        status = "PENDING_APPROVAL"
                    }
                }
            }
            override fun onTabUnselected(tab: TabLayout.Tab?) {}
            override fun onTabReselected(tab: TabLayout.Tab?) {}
        })

        bind.memberSearch.addTextChangedListener { getMember(bind.memberSearch.text.toString(), status) }

        getMember(bind.memberSearch.text.toString(), status)
    }

    private fun getMember(name: String, status: String) {
        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:8081/api/member?name=$name&status=$status").openConnection() as HttpURLConnection
            conn.requestMethod = "GET"
            conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

            if (conn.responseCode in 200..299) {
                val inputStream = conn.inputStream.bufferedReader().readText()
                val dataMember = JSONArray(inputStream)

                GlobalScope.launch(Dispatchers.Main) {
                    val adapter = object : RecyclerView.Adapter<Holder>() {
                        override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): Holder {
                            return Holder(CardMemberLayoutBinding.inflate(layoutInflater, parent, false))
                        }

                        override fun getItemCount(): Int {
                            return dataMember.length()
                        }

                        override fun onBindViewHolder(holder: Holder, position: Int) {
                            dataMember.getJSONObject(position).let {
                                holder.binding.memberName.text = it.getString("name")
                                holder.binding.memberCurrent.text = if (status == "ACTIVE" || status == "INACTIVE") "Member Until ${it.getString("membershipEnd")}" else "Register At ${it.getString("registerAt")}"

                                if (it.getString("joinedMemberAt") != "null") {
                                    val dateNow = LocalDate.now()
                                    val getDate = LocalDate.parse(it.getString("membershipEnd"))
                                    val selisih = ChronoUnit.DAYS.between(dateNow, getDate)

                                    if (selisih <= 7) {
                                        holder.binding.memberButton.visibility = View.VISIBLE
                                    } else {
                                        holder.binding.memberButton.visibility = View.GONE
                                    }
                                }

                                holder.binding.memberButton.text = if (status == "ACTIVE" || status == "INACTIVE") "Resume" else "Confirm"

                                holder.binding.memberButton.setOnClickListener { e ->
                                    if (status == "ACTIVE" || status == "INACTIVE") {
                                        GlobalScope.launch(Dispatchers.IO) {
                                            val conn = URL("http://10.0.2.2:8081/api/member/${it.getString("id")}/resume").openConnection() as HttpURLConnection
                                            conn.requestMethod = "PUT"
                                            conn.setRequestProperty("Content-Type", "application/json")
                                            conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

                                            if (conn.responseCode in 200..299) {
                                                runOnUiThread {
                                                    getMember(name, status)
                                                }
                                            }
                                        }
                                    } else {
                                        GlobalScope.launch(Dispatchers.IO) {
                                            val conn = URL("http://10.0.2.2:8081/api/member/${it.getString("id")}/approve").openConnection() as HttpURLConnection
                                            conn.requestMethod = "PUT"
                                            conn.setRequestProperty("Content-Type", "application/json")
                                            conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

                                            if (conn.responseCode in 200..299) {
                                                runOnUiThread {
                                                    getMember(name, status)
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    bind.memberActive.adapter = adapter
                    bind.memberActive.layoutManager = LinearLayoutManager(this@MainActivity6)
                }
            }
        }
    }

    class Holder(val binding: CardMemberLayoutBinding) : RecyclerView.ViewHolder(binding.root)
}