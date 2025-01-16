package com.example.androidvoto

import android.os.Bundle
import android.util.Log
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.androidvoto.databinding.ActivityMain5Binding
import com.google.android.material.snackbar.Snackbar
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import org.json.JSONObject
import java.net.URL
import java.text.NumberFormat
import java.util.Locale

class MainActivity5 : AppCompatActivity() {
    private lateinit var bind: ActivityMain5Binding
    private var userVotoken = 0
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain5Binding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.backButton.setOnClickListener { finish() }

        val apiHelper = ApiHelper()
        val rupiahFormat = NumberFormat.getCurrencyInstance(Locale("in", "ID"))
        val shared = getSharedPreferences("cart", MODE_PRIVATE)
        val editor = shared.edit()
        val sharedGet = shared.getString("cartArray", "[]")
        val arrayCart = JSONArray(sharedGet)
        var totalPrice = 0

        for (e in 0 until arrayCart.length()) {
            val bySeller = arrayCart.getJSONObject(e).getJSONArray("items")
            for (i in 0 until bySeller.length()) {
                val getCamera = bySeller.getJSONObject(i)
                if (getCamera.getBoolean("isChecked")) {
                    totalPrice += (getCamera.getInt("price") * getCamera.getInt("qty"))
                }
            }
        }

        bind.userInformation.addOnCheckedStateChangedListener { checkBox, state ->
            if (state == 1) {
                apiHelper.makeRequest(
                    url = "http://10.0.2.2:5000/api/me",
                    headers = mapOf("Authorization" to "Bearer ${Session.token}")
                ) { result, error ->
                    if (error != null) {
                        Snackbar.make(bind.root, "Error : ${error.message}", Snackbar.LENGTH_LONG)
                            .show()
                    } else {
                        JSONObject(result.toString()).let { user ->
                            bind.userName.setText(user.getString("name"))
                            bind.userPhoneNumber.setText(user.getString("phoneNumber"))
                            bind.userAddress.setText(user.getString("address"))
                            userVotoken = user.getInt("votoken")

                            bind.userName.isEnabled = false
                            bind.userAddress.isEnabled = false
                            bind.userPhoneNumber.isEnabled = false
                        }
                    }
                }
            } else {
                bind.userName.text?.clear()
                bind.userAddress.text?.clear()
                bind.userPhoneNumber.text?.clear()

                bind.userName.isEnabled = true
                bind.userAddress.isEnabled = true
                bind.userPhoneNumber.isEnabled = true
            }
        }
        bind.subtotal.text = rupiahFormat.format(totalPrice.toDouble())
        bind.deliveryFee.text = rupiahFormat.format(30000)
        bind.totalPrice.text = "Total : ${rupiahFormat.format(totalPrice.toDouble() + 30000)}"
        bind.buttonCheckout.setOnClickListener {
            if (bind.userName.text.isNullOrEmpty() || bind.userAddress.text.isNullOrEmpty() || bind.userPhoneNumber.text.isNullOrEmpty()) {
                Snackbar.make(bind.root, "Plase fill all input fields!", Snackbar.LENGTH_SHORT)
                    .show()
                return@setOnClickListener
            }

            val dataItems = JSONObject().apply {
                put("recipientName", bind.userName.text)
                put("recipientPhoneNumber", bind.userPhoneNumber.text)
                put("shippingAddress", bind.userAddress.text)
                put("totalPrice", totalPrice + 30000)
                put("items", JSONArray().apply {
                    for (e in 0 until arrayCart.length()) {
                        val bySeller = arrayCart.getJSONObject(e).getJSONArray("items")
                        for (i in 0 until bySeller.length()) {
                            val getCamera = bySeller.getJSONObject(i)
                            if (getCamera.getBoolean("isChecked")) {
                                put(JSONObject().apply {
                                    put("cameraID", getCamera.getInt("id"))
                                    put("qty", getCamera.getInt("qty"))
                                    put(
                                        "subtotal",
                                        getCamera.getInt("price") * getCamera.getInt("qty")
                                    )
                                })
                            }
                        }
                    }
                })
            }

            Log.d("keranjang", dataItems.toString())

            apiHelper.makeRequest(
                url = "http://10.0.2.2:5000/api/transaction",
                method = "POST",
                headers = mapOf(
                    "Authorization" to "Bearer ${Session.token}",
                    "Content-Type" to "application/json"
                ),
                body = dataItems.toString()
            ) { result, error ->
                if (error != null) {
                    Snackbar.make(bind.root, "Error : ${error.message}", Snackbar.LENGTH_LONG)
                        .show()
                } else {
                    Toast.makeText(this@MainActivity5, "Success To Checkout!", Toast.LENGTH_SHORT)
                        .show()

                    for (e in 0 until arrayCart.length()) {
                        val itemsArray = arrayCart.getJSONObject(e)?.getJSONArray("items")
                        if (itemsArray != null && itemsArray.length() != 0) {
                            for (i in itemsArray.length() - 1 downTo 0) {
                                if (itemsArray.getJSONObject(i)?.getBoolean("isChecked") == true) {
                                    itemsArray.remove(i)
                                }
                            }
                        }
                    }

                    editor.putString("cartArray", arrayCart.toString())
                    editor.apply()

                    val helper2 = ApiHelper()
                    val total = userVotoken + (totalPrice * 0.001f).toInt()

                    helper2.makeRequest(
                        url = "http://10.0.2.2:5000/api/me/votoken/update?value=${total}",
                        method = "PUT",
                        headers = mapOf("Authorization" to "Bearer ${Session.token}")
                    ) { r, e ->
                        if (error != null) {
                            Snackbar.make(
                                bind.root,
                                "Error : ${error.message}",
                                Snackbar.LENGTH_LONG
                            )
                                .show()
                        } else {
                            Toast.makeText(
                                this@MainActivity5,
                                "You get ${total} Votoken",
                                Toast.LENGTH_SHORT
                            ).show()
                        }
                    }

                    setResult(RESULT_OK)

                    finish()
                }
            }
        }
    }
}