package com.example.androidvoto

import android.annotation.SuppressLint
import android.app.Activity
import android.content.Intent
import android.graphics.BitmapFactory
import android.os.Bundle
import android.util.Log
import android.view.ViewGroup
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.activity.result.contract.ActivityResultContracts
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.androidvoto.databinding.ActivityMain4Binding
import com.example.androidvoto.databinding.CardCheckoutLayoutBinding
import com.example.androidvoto.databinding.CardChildCoLayoutBinding
import com.google.android.material.snackbar.Snackbar
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import java.net.URL
import java.text.NumberFormat
import java.util.Locale

class MainActivity4 : AppCompatActivity() {
    private lateinit var bind: ActivityMain4Binding
    private var launcher =
        registerForActivityResult(ActivityResultContracts.StartActivityForResult()) {
            if (it.resultCode == RESULT_OK) {
                val shared = getSharedPreferences("cart", MODE_PRIVATE)
                val editor = shared.edit()
                val sharedGet = shared.getString("cartArray", "[]")
                val arrayCart = JSONArray(sharedGet)

                for (e in 0 until arrayCart.length()) {
                    val itemsArray = arrayCart.getJSONObject(e).getJSONArray("items")
                    if (itemsArray.length() == 0) {
                        arrayCart.remove(e)
                    }
                }

                editor.putString("cartArray", arrayCart.toString())
                editor.apply()
            }
            loadCart()
        }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain4Binding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.backButton.setOnClickListener {
            finish()
        }

        loadCart()
    }

    private fun loadCart() {
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

        bind.totalPrice.text = "Total : ${rupiahFormat.format(totalPrice.toDouble())}"

        bind.cartRv.adapter = object : RecyclerView.Adapter<ParentHolder>() {
            override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ParentHolder {
                return ParentHolder(
                    CardCheckoutLayoutBinding.inflate(
                        layoutInflater,
                        parent,
                        false
                    )
                )
            }

            override fun getItemCount(): Int = arrayCart.length()

            override fun onBindViewHolder(
                holder: ParentHolder,
                @SuppressLint("RecyclerView") position: Int
            ) {
                arrayCart.getJSONObject(position).let { cam ->
                    holder.binding.cameraStore.text = cam.getString("seller")
                    holder.binding.cameraStore.isChecked = cam.getBoolean("isCheked")

                    val listBool = mutableListOf<Boolean>()
                    for (e in 0 until arrayCart.getJSONObject(position).getJSONArray("items")
                        .length()) {
                        listBool.add(
                            arrayCart.getJSONObject(position).getJSONArray("items")
                                .getJSONObject(e).getBoolean("isChecked")
                        )
                    }
                    holder.binding.cameraStore.isChecked =
                        listBool.count { it } != 0

                    bind.buttonCheckout.setOnClickListener {
                        if (arrayCart.length() == 0) {
                            Snackbar.make(
                                bind.root,
                                "Cannot checkout with empty item!",
                                Snackbar.LENGTH_SHORT
                            )
                                .show()
                            return@setOnClickListener
                        }

                        if (listBool.count { it } == 0) {
                            Snackbar.make(
                                bind.root,
                                "Cannot checkout with empty item!",
                                Snackbar.LENGTH_SHORT
                            )
                                .show()
                            return@setOnClickListener
                        }

                        launcher.launch(Intent(this@MainActivity4, MainActivity5::class.java))
                    }

                    holder.binding.itemsRv.adapter =
                        object : RecyclerView.Adapter<ChildrenHolder>() {
                            override fun onCreateViewHolder(
                                parent: ViewGroup,
                                viewType: Int
                            ): ChildrenHolder {
                                return ChildrenHolder(
                                    CardChildCoLayoutBinding.inflate(
                                        layoutInflater,
                                        parent,
                                        false
                                    )
                                )
                            }

                            override fun getItemCount(): Int = cam.getJSONArray("items").length()

                            override fun onBindViewHolder(
                                childHolder: ChildrenHolder,
                                childPosition: Int
                            ) {
                                cam.getJSONArray("items").getJSONObject(childPosition)
                                    .let { child ->
                                        childHolder.child.cameraPrice.text =
                                            rupiahFormat.format(
                                                (child.getDouble("price") * child.getInt(
                                                    "qty"
                                                ))
                                            )
                                        childHolder.child.cameraQty.text = child.getString("qty")
                                        childHolder.child.cameraChecked.isChecked =
                                            child.getBoolean("isChecked")
                                        childHolder.child.cameraName.text = child.getString("name")

                                        holder.binding.cameraStore.addOnCheckedStateChangedListener { checkBox, state ->
                                            childHolder.child.cameraChecked.isChecked = state != 0
                                        }

                                        childHolder.child.cameraIncrement.setOnClickListener {
                                            val qty = childHolder.child.cameraQty.text.toString()
                                                .toInt() + 1
                                            childHolder.child.cameraQty.text = qty.toString()

                                            arrayCart.getJSONObject(position).getJSONArray("items")
                                                .getJSONObject(childPosition).apply {
                                                    put(
                                                        "qty",
                                                        childHolder.child.cameraQty.text.toString()
                                                            .toInt()
                                                    )
                                                }

                                            editor.putString("cartArray", arrayCart.toString())
                                            editor.apply()

                                            childHolder.child.cameraPrice.text =
                                                rupiahFormat.format(
                                                    (child.getDouble("price") * child.getInt(
                                                        "qty"
                                                    ))
                                                )

                                            totalPrice = 0

                                            for (e in 0 until arrayCart.length()) {
                                                val bySeller =
                                                    arrayCart.getJSONObject(e).getJSONArray("items")
                                                for (i in 0 until bySeller.length()) {
                                                    val getCamera = bySeller.getJSONObject(i)
                                                    if (getCamera.getBoolean("isChecked")) {
                                                        totalPrice += (getCamera.getInt("price") * getCamera.getInt(
                                                            "qty"
                                                        ))
                                                    }
                                                }
                                            }

                                            bind.totalPrice.text =
                                                "Total : ${rupiahFormat.format(totalPrice.toDouble())}"
                                        }

                                        childHolder.child.cameraDecrement.setOnClickListener {
                                            val qty = childHolder.child.cameraQty.text.toString()
                                                .toInt() - 1
                                            if (qty >= 1) {
                                                childHolder.child.cameraQty.text = qty.toString()

                                                arrayCart.getJSONObject(position)
                                                    .getJSONArray("items")
                                                    .getJSONObject(childPosition).apply {
                                                        put(
                                                            "qty",
                                                            childHolder.child.cameraQty.text.toString()
                                                                .toInt()
                                                        )
                                                    }

                                                editor.putString("cartArray", arrayCart.toString())
                                                editor.apply()

                                                childHolder.child.cameraPrice.text =
                                                    rupiahFormat.format(
                                                        (child.getDouble("price") * child.getInt(
                                                            "qty"
                                                        ))
                                                    )
                                            } else {
                                                if (arrayCart.getJSONObject(position)
                                                        .getJSONArray("items").length() == 1
                                                ) {
                                                    arrayCart.remove(position)
                                                } else {
                                                    arrayCart.getJSONObject(position)
                                                        .getJSONArray("items").remove(childPosition)
                                                }

                                                editor.putString(
                                                    "cartArray",
                                                    arrayCart.toString()
                                                )
                                                editor.apply()

                                                loadCart()
                                            }

                                            totalPrice = 0

                                            for (e in 0 until arrayCart.length()) {
                                                val bySeller =
                                                    arrayCart.getJSONObject(e).getJSONArray("items")
                                                for (i in 0 until bySeller.length()) {
                                                    val getCamera = bySeller.getJSONObject(i)
                                                    if (getCamera.getBoolean("isChecked")) {
                                                        totalPrice += (getCamera.getInt("price") * getCamera.getInt(
                                                            "qty"
                                                        ))
                                                    }
                                                }
                                            }

                                            bind.totalPrice.text =
                                                "Total : ${rupiahFormat.format(totalPrice.toDouble())}"
                                        }

                                        childHolder.child.cameraChecked.addOnCheckedStateChangedListener { checkBox, statec ->
                                            arrayCart.getJSONObject(position).getJSONArray("items")
                                                .getJSONObject(childPosition).apply {
                                                    put("isChecked", statec != 0)
                                                }

                                            editor.putString("cartArray", arrayCart.toString())
                                            editor.apply()

                                            listBool[childPosition] =
                                                childHolder.child.cameraChecked.isChecked

                                            holder.binding.cameraStore.isChecked =
                                                listBool.count { it } != 0

                                            arrayCart.getJSONObject(position).apply {
                                                put(
                                                    "isChecked",
                                                    holder.binding.cameraStore.isChecked
                                                )
                                            }

                                            editor.putString("cartArray", arrayCart.toString())
                                            editor.apply()

                                            totalPrice = 0

                                            for (e in 0 until arrayCart.length()) {
                                                val bySeller =
                                                    arrayCart.getJSONObject(e).getJSONArray("items")
                                                for (i in 0 until bySeller.length()) {
                                                    val getCamera = bySeller.getJSONObject(i)
                                                    if (getCamera.getBoolean("isChecked")) {
                                                        totalPrice += (getCamera.getInt("price") * getCamera.getInt(
                                                            "qty"
                                                        ))
                                                    }
                                                }
                                            }

                                            bind.totalPrice.text =
                                                "Total : ${rupiahFormat.format(totalPrice.toDouble())}"
                                        }

                                        GlobalScope.launch(Dispatchers.IO) {
                                            val image = BitmapFactory.decodeStream(
                                                URL(
                                                    "http://10.0.2.2:5000/images/${
                                                        child.getString("photo")
                                                    }"
                                                ).openStream()
                                            )
                                            runOnUiThread {
                                                childHolder.child.cameraImage.setImageBitmap(image)
                                            }
                                        }
                                    }
                            }

                        }
                    holder.binding.itemsRv.layoutManager = LinearLayoutManager(this@MainActivity4)
                }
            }

        }
        bind.cartRv.layoutManager = LinearLayoutManager(this@MainActivity4)
    }

    class ParentHolder(val binding: CardCheckoutLayoutBinding) :
        RecyclerView.ViewHolder(binding.root)

    class ChildrenHolder(val child: CardChildCoLayoutBinding) : RecyclerView.ViewHolder(child.root)
}