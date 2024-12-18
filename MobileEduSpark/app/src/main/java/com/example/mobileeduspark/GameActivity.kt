package com.example.mobileeduspark

import android.content.Intent
import android.graphics.BitmapFactory
import android.os.Bundle
import android.util.Log
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.mobileeduspark.databinding.ActivityGameBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import java.net.HttpURLConnection
import java.net.URL

class GameActivity : AppCompatActivity() {
    private lateinit var bind : ActivityGameBinding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityGameBinding.inflate(layoutInflater)
        setContentView(bind.root)

        loadGame(intent.getStringExtra("id").toString())
    }

    private fun loadGame(id: String) {
        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:5000/api/words/$id").openStream().bufferedReader().readText()
            val data = JSONArray(conn)
            var index = 0
            var getFirst = data.getJSONObject(index)
            var imageConn = BitmapFactory.decodeStream(URL("http://10.0.2.2:5000/images/${getFirst.getString("image")}").openStream())
            val progress = JSONArray(conn)

            for (e in 0 until progress.length()) {
                val getobj = progress.getJSONObject(e)
                getobj.put("answer", "")
            }

            runOnUiThread {
                bind.gameWord.text = stringSwipe(getFirst.getString("word"))
                bind.gameImage.setImageBitmap(imageConn)

                bind.gameNext.setOnClickListener {
//                    getFirst.apply {
//                        put("point", if (bind.gameAnswer.text.toString().toUpperCase() != getFirst.getString("word")) 0 else 10)
//                        put("answer", bind.gameAnswer.text.toString())
//                    }
                    progress.getJSONObject(index).apply {
                        put("point", if (bind.gameAnswer.text.toString().toUpperCase() != getFirst.getString("word")) 0 else 10)
                        put("answer", bind.gameAnswer.text.toString())
                    }

                    bind.gameAnswer.setText("")
                    Log.d("datagame", progress.toString())

                    if (index == data.length() - 1) {
                        startActivity(Intent(this@GameActivity, ScoreActivity::class.java).apply {
                            putExtra("id", id)
                            putExtra("data", progress.toString())
                        })
                        finish()
                        return@setOnClickListener
                    }

                    if (index == data.length() - 2) {
                        bind.gameNext.text = "Finish"
                    }

                    index += 1

                    val get = progress.getJSONObject(index)
                    bind.gameAnswer.setText(get.getString("answer"))

                    getFirst = data.getJSONObject(index)
                    GlobalScope.launch(Dispatchers.IO) {
                        imageConn = BitmapFactory.decodeStream(URL("http://10.0.2.2:5000/images/${getFirst.getString("image")}").openStream())

                        runOnUiThread {
                            bind.gameWord.text = stringSwipe(getFirst.getString("word"))
                            bind.gameImage.setImageBitmap(imageConn)
                        }
                    }
                }

                bind.gamePrev.setOnClickListener {
                    bind.gameNext.text = "Next"

                    progress.getJSONObject(index).apply {
                        put("point", if (bind.gameAnswer.text.toString().toUpperCase() != getFirst.getString("word")) 0 else 10)
                        put("answer", bind.gameAnswer.text.toString())
                    }

                    if (index <= 0) {
                        return@setOnClickListener
                    }

                    index -= 1

                    val get = progress.getJSONObject(index)
                    bind.gameAnswer.setText(get.getString("answer"))

                    getFirst = data.getJSONObject(index)
                    GlobalScope.launch(Dispatchers.IO) {
                        imageConn = BitmapFactory.decodeStream(URL("http://10.0.2.2:5000/images/${getFirst.getString("image")}").openStream())

                        runOnUiThread {
                            bind.gameWord.text = stringSwipe(getFirst.getString("word"))
                            bind.gameImage.setImageBitmap(imageConn)
                        }
                    }
                }
            }
        }
    }

    private fun stringSwipe(text: String) : String {
        val convert = text.toMutableList()
        convert.shuffle()
        val build = convert.joinToString("")
        return build
    }
}