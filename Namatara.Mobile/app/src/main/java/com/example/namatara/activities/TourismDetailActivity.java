package com.example.namatara.activities;

import android.os.Bundle;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.namatara.R;
import com.example.namatara._Helper;
import com.example.namatara.adapters.TourismAdapter;
import com.example.namatara.databinding.ActivityTourismBinding;
import com.example.namatara.databinding.ActivityTourismDetailBinding;
import com.example.namatara.models.ResponseModel;

import org.json.JSONException;
import org.json.JSONObject;

public class TourismDetailActivity extends AppCompatActivity {

    private ActivityTourismDetailBinding binding;
    public static JSONObject jsonObject;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityTourismDetailBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        // Event saat rating berubah
        binding.ratingBar.setOnRatingBarChangeListener((ratingBar, rating, fromUser) -> {
            if (fromUser) {
                sendRatingToAPI(rating);
            }
        });

        // Event klik tombol Bookmark
        binding.btnBookmark.setOnClickListener(v -> toggleBookmark());

        // Mengambil data JSON yang telah dikirimkan dari Activity sebelumnya (misal dari Intent)
        try {

            ResponseModel responseModel = _Helper.httpHelper("tourismattractions/" + jsonObject.getString("id"));
            if (responseModel.code == 200) {
                try {
                    JSONObject tourismData = new JSONObject(responseModel.data).getJSONObject("data");

                    // Mengatur title di ActionBar
                    if (getSupportActionBar() != null) {
                        getSupportActionBar().hide();
                    }

                    // Menampilkan informasi pariwisata
                    binding.tvName.setText(tourismData.getString("name"));
                    binding.tvDescription.setText(tourismData.getString("description"));
                    binding.tvLocation.setText("Location: " + tourismData.getString("location"));
                    binding.tvOpeningHours.setText("Opening Hours: " + tourismData.getString("openingHours"));
                    binding.tvPrice.setText("Rp." + tourismData.getDouble("price"));
//                    binding.tvRating.setText("Rating: " + tourismData.getDouble("rating"));
                    float rating = (float) tourismData.getDouble("rating");
                    binding.ratingBar.setRating(rating);

                    // Mengambil gambar dan menampilkannya
                    String imageUrl = tourismData.getString("imageUrl");
                    _Helper.httpGetImage(this, imageUrl, binding.ivTourismImage);
                } catch (JSONException e) {
                    throw new RuntimeException(e);
                }
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }
    private boolean isBookmarked = false;

    private void toggleBookmark() {
        isBookmarked = !isBookmarked;
        binding.btnBookmark.setImageResource(isBookmarked ? R.drawable.baseline_bookmark_24 : R.drawable.baseline_bookmark_border_24);
        Toast.makeText(this, isBookmarked ? "Added to bookmarks" : "Removed from bookmarks", Toast.LENGTH_SHORT).show();
    }

    private void sendRatingToAPI(float rating) {
        try {
            JSONObject postData = new JSONObject();
            postData.put("tourismAttractionId", jsonObject.getString("id"));
            postData.put("rating", rating);
            postData.put("review", "Great place!");

            ResponseModel responseModel = _Helper.httpHelper("tourismattractions/ratings", postData.toString());

            if (responseModel.code == 200) {
                Toast.makeText(this, "Rating submitted!", Toast.LENGTH_SHORT).show();
            } else {
                Toast.makeText(this, "Failed to submit rating", Toast.LENGTH_SHORT).show();
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

}