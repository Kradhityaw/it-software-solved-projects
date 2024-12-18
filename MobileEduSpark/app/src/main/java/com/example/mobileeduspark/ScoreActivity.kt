package com.example.mobileeduspark

import android.os.Bundle
import android.view.ViewGroup
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.mobileeduspark.databinding.ActivityScoreBinding
import com.example.mobileeduspark.databinding.CardScoreBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

class ScoreActivity : AppCompatActivity() {
    private var totalPoint = 0
    private lateinit var bind : ActivityScoreBinding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityScoreBinding.inflate(layoutInflater)
        setContentView(bind.root)

        val jsonArray = JSONArray(intent.getStringExtra("data"))
        totalPoint = getScore(jsonArray)
        bind.scoreScore.text = getScore(jsonArray).toString()
        getLeaderboard(intent.getStringExtra("id").toString())

        bind.scoreHome.setOnClickListener {
            finish()
        }

        bind.scoreSubmit.setOnClickListener {
            if (bind.scoreNickname.text.isNullOrEmpty()) {
                Toast.makeText(this@ScoreActivity, "Cannot processed!", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }
            submitScore(totalPoint, intent.getStringExtra("id").toString().toInt())
            bind.scoreSubmit.isEnabled = false
            bind.scoreNickname.isEnabled = false
            bind.scoreNickname.setText("")
        }
    }

    private fun getLeaderboard(id: String) {
        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:5000/api/leaderboards/$id").openStream().bufferedReader().readText()
            val data = JSONArray(conn)

            runOnUiThread {
                bind.scoreRv.adapter = object : RecyclerView.Adapter<ScoreHolder>() {
                    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ScoreHolder {
                        val inflater = CardScoreBinding.inflate(layoutInflater, parent, false)
                        return ScoreHolder(inflater)
                    }

                    override fun onBindViewHolder(holder: ScoreHolder, position: Int) {
                        val geter = data.getJSONObject(position)
                        holder.binding.scoreNickname.text = "${position + 1}. ${geter.getString("nickname")}"
                        holder.binding.scoreScore.text = geter.getString("totalPoint")
                    }

                    override fun getItemCount(): Int = data.length()

                }
                bind.scoreRv.layoutManager = LinearLayoutManager(this@ScoreActivity)
            }
        }
    }

    private fun submitScore(totalPoint: Int, gameId: Int) {
        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:5000/api/leaderboards").openConnection() as HttpURLConnection
            conn.requestMethod = "POST"
            conn.setRequestProperty("Content-Type", "application/json")

            val data = JSONObject().apply {
                put("gameID", gameId)
                put("nickname", bind.scoreNickname.text.toString())
                put("totalPoint", totalPoint)
            }

            conn.outputStream.write(data.toString().toByteArray())

            val rescode = conn.responseCode

            runOnUiThread {
                if (rescode in 200..299) {
                    getLeaderboard(gameId.toString())
                }
            }
        }
    }

    private fun getScore(data: JSONArray): Int {
        var score = 0
        for (e in 0 until data.length()) {
            val geter = data.getJSONObject(e)
            score += geter.getInt("point")
        }
        return score
    }

    class ScoreHolder(val binding: CardScoreBinding) : RecyclerView.ViewHolder(binding.root)
}