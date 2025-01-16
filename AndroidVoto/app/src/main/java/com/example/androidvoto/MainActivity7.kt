package com.example.androidvoto

import android.animation.Animator
import android.animation.ObjectAnimator
import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.animation.addListener
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.androidvoto.databinding.ActivityMain7Binding
import com.google.android.material.snackbar.Snackbar
import org.json.JSONObject
import kotlin.random.Random

class MainActivity7 : AppCompatActivity() {
    private lateinit var bind: ActivityMain7Binding
    private var degrees = 0f
    private var lastAngele = 0f
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain7Binding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.backButton.setOnClickListener { finish() }

        loadData()
    }

    private fun loadData() {
        val apiHelper = ApiHelper()

        apiHelper.makeRequest(
            url = "http://10.0.2.2:5000/api/me",
            headers = mapOf("Authorization" to "Bearer ${Session.token}")
        ) { result, error ->
            if (error != null) {
                Snackbar.make(bind.root, "Error : ${error.message}", Snackbar.LENGTH_LONG)
                    .show()
            } else {
                JSONObject(result.toString()).let { user ->
                    bind.votoken.text = user.getString("votoken")

                    bind.imageView2.setOnClickListener {
                        if (user.getInt("votoken") < 2200) {
                            Toast.makeText(
                                this@MainActivity7,
                                "Your votoken is insufficient!",
                                Toast.LENGTH_SHORT
                            ).show()
                            return@setOnClickListener
                        }

                        val helper2 = ApiHelper()

                        helper2.makeRequest(
                            url = "http://10.0.2.2:5000/api/me/votoken/update?value=${user.getInt("votoken") - 2200}",
                            method = "PUT",
                            headers = mapOf("Authorization" to "Bearer ${Session.token}")
                        ) { result, error ->
                            if (error != null) {
                                Snackbar.make(
                                    bind.root,
                                    "Error : ${error.message}",
                                    Snackbar.LENGTH_LONG
                                )
                                    .show()
                            } else {
                                bind.jarum.pivotX = bind.jarum.width / 2f
                                bind.jarum.pivotY = bind.jarum.height.toFloat() - 34

                                val angle = Random.nextInt(360) + 1
                                degrees += angle.toFloat()
                                val totalAngle = lastAngele + angle
                                val amount = (totalAngle + 360f * 3)

                                val animator =
                                    ObjectAnimator.ofFloat(
                                        bind.jarum,
                                        "Rotation",
                                        amount
                                    )
                                animator.duration = 3000

                                animator.addListener(onEnd = { anim: Animator ->
                                    lastAngele = amount

                                    val rotate = bind.jarum.rotation % 360

                                    val price: Int = when (rotate) {
                                        in 0f..59f -> +3000
                                        in 60f..119f -> (15000..30000).random()
                                        in 120f..179f -> +2500
                                        in 180f..239f -> +2200
                                        in 240f..299f -> +1500
                                        in 300f..360f -> -2200
                                        else -> 0
                                    }

                                    val helper4 = ApiHelper()

                                    helper4.makeRequest(
                                        url = "http://10.0.2.2:5000/api/me",
                                        headers = mapOf("Authorization" to "Bearer ${Session.token}")
                                    ) { result, error ->
                                        if (error != null) {
                                            Snackbar.make(
                                                bind.root,
                                                "Error : ${error.message}",
                                                Snackbar.LENGTH_LONG
                                            )
                                                .show()
                                        } else {
                                            JSONObject(result.toString()).let { us ->
                                                val helper3 = ApiHelper()

                                                helper3.makeRequest(
                                                    url = "http://10.0.2.2:5000/api/me/votoken/update?value=${
                                                        us.getInt(
                                                            "votoken"
                                                        ) + price
                                                    }",
                                                    method = "PUT",
                                                    headers = mapOf("Authorization" to "Bearer ${Session.token}")
                                                ) { result, error ->
                                                    if (error != null) {
                                                        Snackbar.make(
                                                            bind.root,
                                                            "Error : ${error.message}",
                                                            Snackbar.LENGTH_LONG
                                                        )
                                                            .show()
                                                    } else {
                                                        loadData()
                                                        Toast.makeText(
                                                            this@MainActivity7,
                                                            (bind.jarum.rotation % 360).toString(),
                                                            Toast.LENGTH_SHORT
                                                        ).show()
                                                    }
                                                }
                                            }
                                        }
                                    }
                                })

                                animator.start()
                            }
                        }
                    }
                }
            }
        }
    }
}