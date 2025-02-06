package com.example.namatara.activities;

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
import com.example.namatara.databinding.ActivitySignUpBinding;
import com.example.namatara.models.ResponseModel;

import org.json.JSONException;
import org.json.JSONObject;

public class SignUpActivity extends AppCompatActivity {

    private ActivitySignUpBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivitySignUpBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

    }

    public void clickSignIn(View view) {
        finish();
    }

    private void signUpVoid(String username, String fullName, String password, String dateOfBirth) {
        try {
            JSONObject jsonObject = new JSONObject()
                    .put("username", username)
                    .put("password", password)
                    .put("fullName", fullName)
                    .put("dateOfBirth", dateOfBirth);

            ResponseModel responseModel = _Helper.httpHelper("sign-up", jsonObject.toString());
            if (responseModel.code == 201) {
                Toast.makeText(this, "Successfully Sign Up!", Toast.LENGTH_SHORT).show();
                finish();
            } else if (responseModel.code == 409) {
                Toast.makeText(this, "Username already exist!", Toast.LENGTH_SHORT).show();
            } else {
                Toast.makeText(this, responseModel.data, Toast.LENGTH_SHORT).show();
            }
        } catch (JSONException e) {
            Toast.makeText(this, e.getMessage(), Toast.LENGTH_SHORT).show();
        }
    }

    public void clickSignUp(View view) {
        String username = binding.edtUsername.getEditText().getText().toString().trim();
        String fullName = binding.edtFullName.getEditText().getText().toString().trim();
        String password = binding.edtPassword.getEditText().getText().toString().trim();
        String confirmPassword = binding.edtConfirmPassword.getEditText().getText().toString().trim();
        String dateOfBirth = binding.edtDateOfBirth.getEditText().getText().toString().trim();

        if (username.isEmpty() || fullName.isEmpty() || password.isEmpty() || confirmPassword.isEmpty() || dateOfBirth.isEmpty()) {
            Toast.makeText(this, "Field can't be Empty!", Toast.LENGTH_SHORT).show();
        } else if (!password.equals(confirmPassword)) {
            Toast.makeText(this, "Password & Confirm password must be same!", Toast.LENGTH_SHORT).show();
        } else {
            signUpVoid(username, fullName, password, dateOfBirth);
        }
    }
}