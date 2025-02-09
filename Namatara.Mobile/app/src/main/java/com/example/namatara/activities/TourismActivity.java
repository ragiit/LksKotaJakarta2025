package com.example.namatara.activities;

import android.os.Bundle;
import android.view.MenuItem;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import com.example.namatara._Helper;
import com.example.namatara.adapters.CategoryAdapter;
import com.example.namatara.adapters.TourismAdapter;
import com.example.namatara.databinding.ActivityTourismBinding;
import com.example.namatara.models.ResponseModel;

import org.json.JSONException;
import org.json.JSONObject;

public class TourismActivity extends AppCompatActivity {

    private ActivityTourismBinding binding;
    public static JSONObject jsonObject;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityTourismBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        // Mengaktifkan tombol back di toolbar
        if (getSupportActionBar() != null) {
            getSupportActionBar().setDisplayHomeAsUpEnabled(true);  // Menambahkan tombol back
            getSupportActionBar().setDisplayShowHomeEnabled(true);  // Menampilkan icon home (back)
            try {
                getSupportActionBar().setTitle(jsonObject.getString("name"));
            } catch (JSONException e) {
                throw new RuntimeException(e);
            }
        }

        setupSearchView();
        getCategories("");
    }
    @Override
    public boolean onOptionsItemSelected(@NonNull MenuItem item) {
        // Cek apakah tombol back yang ditekan
        if (item.getItemId() == android.R.id.home) {
            // Menyelesaikan aktivitas ini (kembali ke aktivitas sebelumnya)
            onBackPressed();  // Memanggil onBackPressed untuk kembali ke aktivitas sebelumnya
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    private void setupSearchView() {
        // Pastikan binding menggunakan SearchView yang berasal dari androidx.appcompat.widget.SearchView
        binding.searchView.setOnQueryTextListener(new androidx.appcompat.widget.SearchView.OnQueryTextListener() {
            @Override
            public boolean onQueryTextSubmit(String query) {
                getCategories(query); // Panggil fungsi pencarian dengan query
                return true;
            }

            @Override
            public boolean onQueryTextChange(String newText) {
                getCategories(newText); // Panggil fungsi pencarian dengan query terbaru
                return true;
            }
        });
    }

    private void getCategories(String search) {
        try {
            String CategoryId = jsonObject.getString("id");
            String Url = "categories/" + CategoryId + "/attractions";
            if (!search.isEmpty())
                Url = "categories/" + CategoryId + "/attractions?search=" + search;

            ResponseModel responseModel = _Helper.httpHelper(Url);
            if (responseModel.code == 200) {
                try {
                    TourismAdapter adapter = new TourismAdapter(new JSONObject(responseModel.data).getJSONArray("data"));
                    binding.recyclerView.setAdapter(adapter);
                } catch (JSONException e) {
                    throw new RuntimeException(e);
                }
            }
        } catch (Exception ex) {

        }
    }
}