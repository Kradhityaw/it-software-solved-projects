package com.example.androidvoto

import android.os.Bundle
import android.view.ViewGroup
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.androidvoto.databinding.ActivityMain6Binding
import com.example.androidvoto.databinding.CardHistoryItemsLayoutBinding
import com.example.androidvoto.databinding.CardHistoryLayoutBinding
import com.google.android.material.snackbar.Snackbar
import org.json.JSONArray
import java.text.NumberFormat
import java.util.Locale

class MainActivity6 : AppCompatActivity() {
    private lateinit var bind: ActivityMain6Binding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain6Binding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.backButton.setOnClickListener { finish() }

        val apiHelper = ApiHelper()

        apiHelper.makeRequest(
            url = "http://10.0.2.2:5000/api/me/transaction",
            headers = mapOf("Authorization" to "Bearer ${Session.token}")
        ) { result, error ->
            if (error != null) {
                Snackbar.make(bind.root, "Error : ${error.message}", Snackbar.LENGTH_LONG)
                    .show()
            } else {
                val transactioData = JSONArray(result)
                val rupiahFormat = NumberFormat.getCurrencyInstance(Locale("in", "ID"))

                bind.historyRv.adapter = object : RecyclerView.Adapter<ParentHolder>() {
                    override fun onCreateViewHolder(
                        parent: ViewGroup,
                        viewType: Int
                    ): ParentHolder {
                        return ParentHolder(
                            CardHistoryLayoutBinding.inflate(
                                layoutInflater,
                                parent,
                                false
                            )
                        )
                    }

                    override fun getItemCount(): Int = transactioData.length()

                    override fun onBindViewHolder(holder: ParentHolder, position: Int) {
                        transactioData.getJSONObject(position).let { transaction ->
                            holder.parent.historyName.text = transaction.getString("id")
                            holder.parent.historyStatus.text =
                                "Status ${transaction.getString("status")}"
                            holder.parent.historySubtotal.text =
                                "Total: ${rupiahFormat.format(transaction.getDouble("totalPrice"))}"

                            holder.parent.historyItems.adapter =
                                object : RecyclerView.Adapter<ChildHolder>() {
                                    override fun onCreateViewHolder(
                                        parent: ViewGroup,
                                        viewType: Int
                                    ): ChildHolder {
                                        return ChildHolder(
                                            CardHistoryItemsLayoutBinding.inflate(
                                                layoutInflater,
                                                parent,
                                                false
                                            )
                                        )
                                    }

                                    override fun getItemCount(): Int {
                                        return transaction.getJSONArray("transactions").length()
                                    }

                                    override fun onBindViewHolder(
                                        holder: ChildHolder,
                                        cposition: Int
                                    ) {
                                        transaction.getJSONArray("transactions")
                                            .getJSONObject(cposition).let { item ->
                                                holder.child.cameraName.text =
                                                    item.getString("name")
                                                holder.child.cameraQty.text =
                                                    "x${item.getString("qty")}"
                                                holder.child.cameraPrice.text =
                                                    rupiahFormat.format(item.getDouble("subtotal"))
                                            }
                                    }

                                }
                            holder.parent.historyItems.layoutManager =
                                LinearLayoutManager(this@MainActivity6)
                        }
                    }

                }
                bind.historyRv.layoutManager = LinearLayoutManager(this@MainActivity6)
            }
        }

    }

    class ParentHolder(val parent: CardHistoryLayoutBinding) : RecyclerView.ViewHolder(parent.root)
    class ChildHolder(val child: CardHistoryItemsLayoutBinding) :
        RecyclerView.ViewHolder(child.root)
}