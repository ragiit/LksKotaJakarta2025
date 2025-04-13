package com.example.namatara.activities;

import android.os.Bundle;
import android.view.MenuItem;

import androidx.activity.EdgeToEdge;
import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.fragment.app.Fragment;

import com.example.namatara.R;
import com.example.namatara.databinding.ActivityMainBinding;
import com.example.namatara.fragments.CategoryFragment;
import com.example.namatara.fragments.ProfileFragment;
import com.google.android.material.bottomnavigation.BottomNavigationView;

public class MainActivity extends AppCompatActivity {
    private ActivityMainBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        if (getSupportActionBar() != null) {
            getSupportActionBar().setTitle("Home");
//            getSupportActionBar().hide();
            changeFragment(new CategoryFragment());
        }

        binding.bottomNavigation.setOnNavigationItemSelectedListener(new BottomNavigationView.OnNavigationItemSelectedListener() {
            @Override
            public boolean onNavigationItemSelected(@NonNull MenuItem item) {
                if (item.getItemId() == R.id.page_category) {
                    if (getSupportActionBar() != null) {
                        changeFragment(new CategoryFragment());
                        getSupportActionBar().setTitle("Categories");
                        return true;
                    }
                } else {
                    if (getSupportActionBar() != null) {
                        changeFragment(new ProfileFragment());
                        getSupportActionBar().setTitle("My Profile");
                        return true;
                    }
                }
                return false;
            }
        });
    }

    public void changeFragment(Fragment fragment) {
        getSupportFragmentManager()
                .beginTransaction()
                .replace(R.id.frameLayout, fragment)
                .commit();
    }
}
