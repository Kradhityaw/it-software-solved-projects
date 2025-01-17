package com.example.androidesemkarestaurant

import android.app.Dialog
import android.content.Intent
import android.os.Bundle
import android.view.ViewGroup
import android.view.Window
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.activity.result.contract.ActivityResultContracts
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.androidesemkarestaurant.databinding.ActivityMain4Binding
import com.example.androidesemkarestaurant.databinding.AlertAddTableLayoutBinding
import com.example.androidesemkarestaurant.databinding.CardListLayoutBinding
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.google.android.material.dialog.MaterialDialogs
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import java.net.HttpURLConnection
import java.net.URL
import java.text.NumberFormat
import java.util.Locale

class MainActivity4 : AppCompatActivity() {
    private lateinit var bind: ActivityMain4Binding
    private var launcher = registerForActivityResult(ActivityResultContracts.StartActivityForResult()) {
        getTable()
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain4Binding.inflate(layoutInflater)
        setContentView(bind.root)
        getTable()

        bind.listAdd.setOnClickListener {
            val binding = AlertAddTableLayoutBinding.inflate(layoutInflater)

            val dialog = MaterialAlertDialogBuilder(this)
                .setView(binding.root)
                .setCancelable(false)
                .create()

            binding.addCancel.setOnClickListener {
                dialog.dismiss()
            }

            binding.addOpen.setOnClickListener {

                if (binding.homeTable.text.isNullOrEmpty() || binding.homeTable.text.toString().toInt() !in 1..50) {
                    Toast.makeText(this@MainActivity4, "Table must be filled and between 1 - 50!", Toast.LENGTH_SHORT).show()
                    return@setOnClickListener
                }

                GlobalScope.launch(Dispatchers.IO) {
                    try {
                        val connection = URL("http://10.0.2.2:5000/Api/Table?number=${binding.homeTable.text}").openConnection() as HttpURLConnection
                        connection.requestMethod = "POST"
                        connection.setRequestProperty("Authorization", "Bearer ${Session.token}")
                        connection.setRequestProperty("Content-Type", "application/json")

                        val responseCode = connection.responseCode

                        runOnUiThread {
                            if (responseCode in 200..299) {
                                Toast.makeText(this@MainActivity4, "Success Add Table!", Toast.LENGTH_SHORT).show()
                                dialog.dismiss()
                                getTable()
                            }
                            else {
                                Toast.makeText(this@MainActivity4, "${connection.errorStream.bufferedReader().readText()}", Toast.LENGTH_SHORT).show()
                            }
                        }
                    }
                    catch (e : Exception) {
                        e.printStackTrace()
                    }
                }
            }

            dialog.show()
        }
    }

    private fun getTable() {
        GlobalScope.launch(Dispatchers.IO) {
            try {
                val connection = URL("http://10.0.2.2:5000/Api/Table").openConnection() as HttpURLConnection
                connection.requestMethod = "GET"
                connection.setRequestProperty("Authorization", "Bearer ${Session.token}")

                val responseCode = connection.responseCode
                val inputStream = connection.inputStream.bufferedReader().readText()

                runOnUiThread {
                    if (responseCode in 200..299) {
                        val dataTable = JSONArray(inputStream)
                        val numberFormat = NumberFormat.getCurrencyInstance(Locale("in", "ID"))

                        bind.listRv.adapter = object : RecyclerView.Adapter<Holder>() {
                            override fun onCreateViewHolder(
                                parent: ViewGroup,
                                viewType: Int
                            ): Holder {
                                return Holder(CardListLayoutBinding.inflate(layoutInflater, parent, false))
                            }

                            override fun getItemCount(): Int {
                                return dataTable.length()
                            }

                            override fun onBindViewHolder(holder: Holder, position: Int) {
                                val getTable = dataTable.getJSONObject(position)
                                holder.binding.listTable.text = "Table ${getTable.getString("number")}"
                                holder.binding.listCode.text = getTable.getString("code")
                                holder.binding.listPrice.text = numberFormat.format(getTable.getDouble("total"))

                                holder.itemView.setOnClickListener {
                                    launcher.launch(Intent(this@MainActivity4, MainActivity5::class.java).apply {
                                        putExtra("id", getTable.getString("id"))
                                    })
                                }
                            }

                        }
                        bind.listRv.layoutManager = LinearLayoutManager(this@MainActivity4)
                    }
                    else {
                        Toast.makeText(this@MainActivity4, "Request timeout!", Toast.LENGTH_SHORT).show()
                    }
                }
            }
            catch (e : Exception) {
                runOnUiThread {
                    Toast.makeText(this@MainActivity4, "Error : ${e.message}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    class Holder(val binding: CardListLayoutBinding) : RecyclerView.ViewHolder(binding.root)
}