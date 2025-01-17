package com.example.androidesemkarestaurant

import android.graphics.BitmapFactory
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.recyclerview.widget.GridLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.androidesemkarestaurant.databinding.CardMenuLayoutBinding
import com.example.androidesemkarestaurant.databinding.FragmentBlankBinding
import com.google.android.material.tabs.TabLayout
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import java.net.HttpURLConnection
import java.net.URL
import java.text.NumberFormat
import java.util.Locale

class BlankFragment : Fragment() {
    private lateinit var bind: FragmentBlankBinding

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        bind = FragmentBlankBinding.inflate(layoutInflater, container, false)
        getMenu("Ayam")

        val listCategory = listOf("Ayam", "Camilan", "DagingSapi", "HappyMeal", "Ikan", "MakananPenutup", "Minuman", "PaketFamily", "SarapanPagi")

        for (e in listCategory) {
            val newTab = bind.menuCategoryTabLayout.newTab()
            newTab.text = e
            newTab.tag = e
            bind.menuCategoryTabLayout.addTab(newTab)
        }

        bind.menuCategoryTabLayout.addOnTabSelectedListener(object : TabLayout.OnTabSelectedListener {
            override fun onTabSelected(tab: TabLayout.Tab?) {
                getMenu(tab?.tag as String)
            }
            override fun onTabUnselected(tab: TabLayout.Tab?) {}
            override fun onTabReselected(tab: TabLayout.Tab?) {}
        })

        return bind.root
    }

    private fun getMenu(category: String) {
        GlobalScope.launch(Dispatchers.IO) {
            try {
                val connection = URL("http://10.0.2.2:5000/Api/Menu/Category/${category}").openConnection() as HttpURLConnection
                connection.requestMethod = "GET"
                val responseCode = connection.responseCode
                val inputStream = connection.inputStream.bufferedReader().readText()

                GlobalScope.launch(Dispatchers.Main) {
                    if (responseCode in 200..299) {
                        val dataMenu = JSONArray(inputStream)
                        val numberFormat = NumberFormat.getCurrencyInstance(Locale("in", "ID"))

                        bind.menuRv.adapter = object : RecyclerView.Adapter<MenuHolder>() {
                            override fun onCreateViewHolder(
                                parent: ViewGroup,
                                viewType: Int
                            ): MenuHolder {
                                return MenuHolder(CardMenuLayoutBinding.inflate(layoutInflater, parent, false))
                            }

                            override fun getItemCount(): Int {
                                return dataMenu.length()
                            }

                            override fun onBindViewHolder(holder: MenuHolder, position: Int) {
                                val getMenu = dataMenu.getJSONObject(position)
                                holder.binding.menuName.text = getMenu.getString("name")
                                holder.binding.menuPrice.text = numberFormat.format(getMenu.getDouble("price"))
                                GlobalScope.launch(Dispatchers.IO) {
                                    try {
                                        val imageConnection = BitmapFactory.decodeStream(URL("http://10.0.2.2:5000/Api/Menu/${getMenu.getString("id")}/Photo").openStream())
                                        GlobalScope.launch(Dispatchers.Main) {
                                            holder.binding.menuImage.setImageBitmap(imageConnection)
                                        }
                                    }
                                    catch (e : Exception) {
                                        GlobalScope.launch(Dispatchers.Main) {
                                            holder.binding.menuImage.setImageResource(R.drawable.logo)
                                        }
                                    }
                                }

                                holder.itemView.setOnClickListener {
                                    val transaction = requireActivity().supportFragmentManager.beginTransaction()
                                    transaction.replace(R.id.menuView, BlankFragment2(getMenu.getString("id")))
                                    transaction.addToBackStack(null)
                                    transaction.commit()
                                }
                            }
                        }
                        bind.menuRv.layoutManager = GridLayoutManager(requireContext(), 2)
                    } else {
                        val error = connection.errorStream.bufferedReader().readText()
                        Toast.makeText(context, "Error : $error", Toast.LENGTH_SHORT).show()
                    }
                }
            }
            catch (e : Exception) {
                Toast.makeText(context, "Error : ${e.message}", Toast.LENGTH_SHORT).show()
            }
        }
    }

    class MenuHolder(val binding: CardMenuLayoutBinding) : RecyclerView.ViewHolder(binding.root)
}