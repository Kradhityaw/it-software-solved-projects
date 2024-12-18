package com.example.mobileezemkofi

import android.animation.AnimatorSet
import android.animation.ObjectAnimator
import android.annotation.SuppressLint
import android.app.Activity
import android.graphics.BitmapFactory
import android.graphics.Color
import android.icu.text.DecimalFormat
import android.os.Bundle
import android.util.Log
import android.view.animation.AccelerateDecelerateInterpolator
import android.view.animation.AccelerateInterpolator
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.mobileezemkofi.databinding.ActivityDetailBinding
import kotlinx.coroutines.DelicateCoroutinesApi
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

class DetailActivity : AppCompatActivity() {
    private lateinit var bind : ActivityDetailBinding
    private var selectedSize = "M"

    @SuppressLint("SetTextI18n")
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityDetailBinding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.detailBack.setOnClickListener { finish() }
        bind.detailIncrement.setOnClickListener { bind.detailQty.text = (bind.detailQty.text.toString().toInt() + 1).toString() }
        bind.detailDecrement.setOnClickListener { if (bind.detailQty.text.toString().toInt() != 1) bind.detailQty.text = (bind.detailQty.text.toString().toInt() - 1).toString() }

        getCoffee()
    }

    @SuppressLint("SetTextI18n", "CommitPrefEdits")
    @OptIn(DelicateCoroutinesApi::class)
    private fun getCoffee() {
        GlobalScope.launch(Dispatchers.IO) {
            val conn = URL("http://10.0.2.2:5000/api/coffee/${intent.getStringExtra("idCoffee")}").openConnection() as HttpURLConnection
            conn.setRequestProperty("Authorization", "Bearer ${Session.token}")

            val inputStream = conn.inputStream.bufferedReader().readText()
            val convertToJsonObject = JSONObject(inputStream)
            val image = BitmapFactory.decodeStream(URL("http://10.0.2.2:5000/images/${convertToJsonObject.getString("imagePath")}").openStream())

            runOnUiThread {
                bind.detailImage.setImageBitmap(image)
                bind.detailName.text = convertToJsonObject.getString("name")
                bind.detailPrice.text = convertToJsonObject.getDouble("price").toString()
                bind.detailDescription.text = convertToJsonObject.getString("description")
                bind.detailRating.text = convertToJsonObject.getDouble("rating").toString()

                val newPrice = DecimalFormat("#.##")

                bind.detailS.setOnClickListener {
                    rotateImage(0.85f)
                    activedBtn(true, false, false)
                    bind.detailPrice.text = newPrice.format(sumPrice(convertToJsonObject.getDouble("price"), 0.85))
                    selectedSize = "S"
                }

                bind.detailM.setOnClickListener {
                    rotateImage(1.00f)
                    activedBtn(false, true, false)
                    bind.detailPrice.text = newPrice.format(sumPrice(convertToJsonObject.getDouble("price"), 1.00))
                    selectedSize = "M"
                }

                bind.detailL.setOnClickListener {
                    rotateImage(1.15f)
                    activedBtn(false, false, true)
                    bind.detailPrice.text = newPrice.format(sumPrice(convertToJsonObject.getDouble("price"), 1.15))
                    selectedSize = "L"
                }

                bind.detailAddtocart.setOnClickListener {
                    convertToJsonObject.apply {
                        put("size", selectedSize)
                        put("qty", bind.detailQty.text.toString().toInt())
                        put("priceSize", bind.detailPrice.text.toString().toDouble())
                        put("totalPrice", bind.detailPrice.text.toString().toDouble() * bind.detailQty.text.toString().toInt())
                    }

                    val shared = getSharedPreferences("cardCoffee", Activity.MODE_PRIVATE)
                    val editor = shared.edit()

                    val jsonArr = JSONArray(shared.getString("coffeeData", "[]"))

                    for (data in 0 until jsonArr.length()) {
                        val jsonObj = jsonArr.getJSONObject(data)

                        if (jsonObj.getString("id") == convertToJsonObject.getString("id") && jsonObj.getString("size") == convertToJsonObject.getString("size")) {
                            val sum = jsonObj.getInt("qty") + bind.detailQty.text.toString().toInt()
                            jsonObj.apply {
                                put("qty", sum)
                                put("totalPrice", bind.detailPrice.text.toString().toDouble() * sum)
                            }

                            editor.putString("coffeeData", jsonArr.toString())
                            editor.apply()

                            Toast.makeText(this@DetailActivity, "Complete add to cart!", Toast.LENGTH_SHORT).show()
                            Log.d("getCoffee", shared.getString("coffeeData", "[]").toString())

                            return@setOnClickListener
                        }
                    }

                    jsonArr.put(convertToJsonObject)

                    editor.putString("coffeeData", jsonArr.toString())
                    editor.apply()

                    Toast.makeText(this@DetailActivity, "Complete add to cart!", Toast.LENGTH_SHORT).show()
                    Log.d("getCoffee", shared.getString("coffeeData", "[]").toString())
                }
            }
        }
    }

    private fun activedBtn(s: Boolean, m: Boolean, l: Boolean) {
        bind.detailS.setBackgroundResource(if (s) R.drawable.detail_button_selected else R.drawable.detail_button_unselect)
        bind.detailS.setTextColor(if (s) resources.getColor(R.color.white) else resources.getColor(R.color.green))

        bind.detailM.setBackgroundResource(if (m) R.drawable.detail_button_selected else R.drawable.detail_button_unselect)
        bind.detailM.setTextColor(if (m) resources.getColor(R.color.white) else resources.getColor(R.color.green))

        bind.detailL.setBackgroundResource(if (l) R.drawable.detail_button_selected else R.drawable.detail_button_unselect)
        bind.detailL.setTextColor(if (l) resources.getColor(R.color.white) else resources.getColor(R.color.green))
    }

    @SuppressLint("Recycle")
    private fun rotateImage(s1: Float) {
        val animator = AnimatorSet()

        val scaleX = ObjectAnimator.ofFloat(bind.detailImage, "scaleX", s1)
        val scaleY = ObjectAnimator.ofFloat(bind.detailImage, "scaleY", s1)
        val rotate = ObjectAnimator.ofFloat(bind.detailImage, "rotation", 0f ,360f)

        animator.playTogether(scaleY, scaleX, rotate)
        animator.duration = 600
        animator.interpolator = AccelerateDecelerateInterpolator()
        animator.start()
    }

    private fun sumPrice(price: Double, percentage: Double) : Double {
        val sum = price * percentage
        return sum
    }
}