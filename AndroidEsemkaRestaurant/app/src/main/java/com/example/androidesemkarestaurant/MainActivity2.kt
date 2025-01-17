package com.example.androidesemkarestaurant

import android.os.Bundle
import android.view.ViewGroup
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.RecyclerView
import com.example.androidesemkarestaurant.databinding.ActivityMain2Binding
import com.example.androidesemkarestaurant.databinding.CardMenuLayoutBinding
import com.google.android.material.tabs.TabLayout
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import java.net.HttpURLConnection
import java.net.URL

class MainActivity2 : AppCompatActivity() {
    private lateinit var bind: ActivityMain2Binding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        bind = ActivityMain2Binding.inflate(layoutInflater)
        setContentView(bind.root)

        bind.mainToolbar.title = "Esemka Restaurant - ${Session.table}"

        supportFragmentManager.beginTransaction().replace(bind.menuView.id, BlankFragment()).commit()

        bind.mainBottomNav.addOnTabSelectedListener(object : TabLayout.OnTabSelectedListener {
            override fun onTabSelected(tab: TabLayout.Tab?) {
                when (tab?.position) {
                    0 -> supportFragmentManager.beginTransaction().replace(bind.menuView.id, BlankFragment()).commit()
                    1 -> supportFragmentManager.beginTransaction().replace(bind.menuView.id, BlankFragment3()).commit()
                    2 -> supportFragmentManager.beginTransaction().replace(bind.menuView.id, BlankFragment4()).commit()
                }
            }
            override fun onTabUnselected(tab: TabLayout.Tab?) {}
            override fun onTabReselected(tab: TabLayout.Tab?) {}
        })

    }
}