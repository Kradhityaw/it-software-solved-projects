package com.example.mobileeduspark

import android.content.Intent
import android.os.Bundle
import android.view.ViewGroup
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.mobileeduspark.databinding.ActivityMainBinding
import com.example.mobileeduspark.databinding.CardGameBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import java.net.URL

class MainActivity : AppCompatActivity() {
    private lateinit var bind : ActivityMainBinding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMainBinding.inflate(layoutInflater)
        setContentView(bind.root)

        loadQuiz()
    }

    private fun loadQuiz() {
        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:5000/api/games").openStream().bufferedReader().readText()
            val data = JSONArray(conn)

            runOnUiThread {
                bind.homeRv.adapter = object : RecyclerView.Adapter<QuizHolder>() {
                    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): QuizHolder {
                        val inflater = CardGameBinding.inflate(layoutInflater, parent, false)
                        return QuizHolder(inflater)
                    }

                    override fun onBindViewHolder(holder: QuizHolder, position: Int) {
                        val geter = data.getJSONObject(position)
                        holder.binding.quizName.text = geter.getString("name")
                        holder.binding.quizCategory.text = geter.getString("category")
                        holder.binding.quizPlayers.text = "${geter.getInt("totalPlayer")} Players"

                        holder.itemView.setOnClickListener {
                            startActivity(Intent(this@MainActivity, GameActivity::class.java).apply {
                                putExtra("id", geter.getString("id"))
                            })
                        }
                    }

                    override fun getItemCount(): Int = data.length()

                }

                bind.homeRv.layoutManager = LinearLayoutManager(this@MainActivity)
            }
        }
    }

    class QuizHolder(val binding: CardGameBinding) : RecyclerView.ViewHolder(binding.root)
}