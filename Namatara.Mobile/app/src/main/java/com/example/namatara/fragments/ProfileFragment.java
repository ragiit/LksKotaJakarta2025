package com.example.namatara.fragments;

import android.content.Intent;
import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.LinearLayoutManager;

import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import com.example.namatara.R;
import com.example.namatara._Helper;
import com.example.namatara.activities.LoginActivity;
import com.example.namatara.activities.MainActivity;
import com.example.namatara.adapters.CategoryAdapter;
import com.example.namatara.adapters.RatingAdapter;
import com.example.namatara.databinding.FragmentCategoryBinding;
import com.example.namatara.databinding.FragmentProfileBinding;
import com.example.namatara.models.ResponseModel;
import com.google.android.material.bottomnavigation.BottomNavigationView;

import org.json.JSONException;
import org.json.JSONObject;

public class ProfileFragment extends Fragment {

    private FragmentProfileBinding binding;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        binding = FragmentProfileBinding.inflate(getLayoutInflater());

        ResponseModel responseModel = _Helper.httpHelper("me");

        if (responseModel.code == 200) {
            try {
                JSONObject data = new JSONObject(responseModel.data).getJSONObject("data");

                binding.tvFullName.setText(data.getString("fullName"));
                binding.tvUsername.setText(String.format("@%s", data.getString("username")));

                _Helper.httpGetImage(this.getContext(), data.getString("imageUrl"), binding.ivProfileImage);

                getRatings();
            } catch (JSONException e) {
                e.printStackTrace();
                Toast.makeText(getContext(), "Gagal mengambil data profil", Toast.LENGTH_SHORT).show();
            }

        } else if (responseModel.code == 401) {
            Intent intent = new Intent(getActivity(), LoginActivity.class);
            startActivity(intent);
            getActivity().finish();

        } else {
            Toast.makeText(getContext(), responseModel.data, Toast.LENGTH_SHORT).show();
        }

        binding.bottomNavigation.setOnNavigationItemSelectedListener(new BottomNavigationView.OnNavigationItemSelectedListener() {
            @Override
            public boolean onNavigationItemSelected(@NonNull MenuItem item) {
                if (item.getItemId() == R.id.page_ratings) {
                    getRatings();
                    return true;
                } else {
                    getBookmarks();
                    return true;
                }
            }
        });

        return binding.getRoot();
    }

    private void getRatings() {
        ResponseModel responseModel = _Helper.httpHelper("me/attraction-ratings");
        if (responseModel.code == 200) {
            try {
                RatingAdapter adapter = new RatingAdapter(new JSONObject(responseModel.data).getJSONArray("data"));
                binding.recyclerView.setLayoutManager(new GridLayoutManager(this.getContext(), 2));
                binding.recyclerView.setAdapter(adapter);

            } catch (JSONException e) {
                throw new RuntimeException(e);
            }
        }
    }

    private void getBookmarks() {
        ResponseModel responseModel = _Helper.httpHelper("me/bookmarks");
        if (responseModel.code == 200) {
            try {
                RatingAdapter adapter = new RatingAdapter(new JSONObject(responseModel.data).getJSONArray("data"));
                binding.recyclerView.setLayoutManager(new GridLayoutManager(this.getContext(), 2));
                binding.recyclerView.setAdapter(adapter);

            } catch (JSONException e) {
                throw new RuntimeException(e);
            }
        }
    }
}