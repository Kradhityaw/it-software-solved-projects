package com.example.androidvoto

import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import java.net.HttpURLConnection
import java.net.URL

class ApiHelper {

    fun makeRequest(
        url: String,
        method: String = "GET",
        headers: Map<String, String> = emptyMap(),
        body: String? = null,
        callback: (result: String?, error: Exception?) -> Unit
    ) {
        GlobalScope.launch(Dispatchers.IO) {
            try {
                val url = URL(url)
                val conn = url.openConnection() as HttpURLConnection
                conn.requestMethod = method

                headers.forEach { (key, value) ->
                    conn.setRequestProperty(key, value)
                }

                if (!body.isNullOrEmpty() && method in listOf("POST", "PUT", "PATCH")) {
                    conn.doOutput = true
                    val outputStream = conn.outputStream
                    outputStream.write(body.toString().toByteArray())
                    outputStream.flush()
                    outputStream.close()
                }

                val resCode = conn.responseCode
                val inputStream = if (resCode in 200..299) {
                    conn.inputStream
                } else {
                    conn.errorStream
                }

                val response = inputStream.bufferedReader().readText()

                withContext(Dispatchers.Main) {
                    callback(response, null)
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    callback(null, e)
                }
            }
        }
    }

}