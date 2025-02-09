package com.example.namatara.fragments;

import android.os.Bundle;

import androidx.fragment.app.Fragment;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.example.namatara._Helper;
import com.example.namatara.adapters.CategoryAdapter;
import com.example.namatara.databinding.FragmentCategoryBinding;
import com.example.namatara.models.ResponseModel;

import org.json.JSONException;
import org.json.JSONObject;

public class CategoryFragment extends Fragment {

    private FragmentCategoryBinding binding;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        binding = FragmentCategoryBinding.inflate(getLayoutInflater());

        setupSearchView();
        getCategories("");

        return binding.getRoot();
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
        String Url = "categories";
        if (!search.isEmpty())
            Url = "categories?search=" + search;

        ResponseModel responseModel = _Helper.httpHelper(Url);
        if (responseModel.code == 200) {
            try {
                CategoryAdapter adapter = new CategoryAdapter(new JSONObject(responseModel.data).getJSONArray("data"));
                binding.recyclerView.setAdapter(adapter);
            } catch (JSONException e) {
                throw new RuntimeException(e);
            }
        }
    }
}