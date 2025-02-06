package com.example.namatara.fragments;

import android.os.Bundle;

import androidx.fragment.app.Fragment;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.example.namatara.R;
import com.example.namatara._Helper;
import com.example.namatara.databinding.FragmentCategoryBinding;
import com.example.namatara.models.ResponseModel;

import org.json.JSONArray;
import org.json.JSONException;

public class CategoryFragment extends Fragment {

    private FragmentCategoryBinding binding;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        binding = FragmentCategoryBinding.inflate(getLayoutInflater());

        getCategories();

        return binding.getRoot();
    }

    private void getCategories() {
        ResponseModel responseModel = _Helper.httpHelper("categories");
        if (responseModel.code == 200) {
//            try {
////                CategoryAdapter adapter = new CategoryAdapter(new JSONArray(responseModel.data));
////                binding.recyclerView.setAdapter(adapter);
//            } catch (JSONException e) {
//                throw new RuntimeException(e);
//            }
        }
    }
}