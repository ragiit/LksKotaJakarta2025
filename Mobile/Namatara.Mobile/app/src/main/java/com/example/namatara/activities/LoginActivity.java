package com.example.namatara.activities;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.namatara.R;
import com.example.namatara._Helper;
import com.example.namatara.databinding.ActivityLoginBinding;
import com.example.namatara.models.ResponseModel;

import org.json.JSONException;
import org.json.JSONObject;

public class LoginActivity extends AppCompatActivity {
    private ActivityLoginBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityLoginBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

    //    binding.edtUsername.getEditText().setText("admin");
//        binding.edtPassword.getEditText().setText("adminpassword");

//        binding.edtUsername.getEditText().setText("dkiley3");
//        binding.edtPassword.getEditText().setText("uO4tF2");

//        binding.edtUsername.setText("dkiley3");
//        binding.edtPassword.setText("uO4tF2");

        binding.btnLogin.setOnClickListener(v -> {
            String username = binding.edtUsername.getEditText().getText().toString().trim();
            String password = binding.edtPassword.getEditText().getText().toString().trim();
            if (username.isEmpty() || password.isEmpty()) {
                Toast.makeText(this, "Please insert the Username & Password!", Toast.LENGTH_SHORT).show();
            } else {
                loginVoid(username, password);
            }
        });
    }

    private void loginVoid(String username, String password) {
        try {
            JSONObject jsonObject = new JSONObject()
                    .put("username", username)
                    .put("password", password);

            ResponseModel responseModel = _Helper.httpHelper("auth/sign-in", jsonObject.toString());
            if (responseModel.code == 200) {
                _Helper.TOKEN = new JSONObject(responseModel.data).getJSONObject("data").getString("token");
                startActivity(new Intent(this, MainActivity.class));
            } else if (responseModel.code == 404) {
                Toast.makeText(this, "Invalid Username or Password!", Toast.LENGTH_SHORT).show();
            } else {
                Toast.makeText(this, responseModel.data, Toast.LENGTH_SHORT).show();
            }
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }
    }

    public void clickLogin(View view) {
        startActivity(new Intent(this, MainActivity.class));
    }

    public void clickSignUp(View view) {
        startActivity(new Intent(this, SignUpActivity.class));
    }
}