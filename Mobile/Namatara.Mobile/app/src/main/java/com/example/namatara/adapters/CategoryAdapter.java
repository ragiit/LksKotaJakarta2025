package com.example.namatara.adapters;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.namatara._Helper;
import com.example.namatara.activities.TourismActivity;
import com.example.namatara.databinding.CategoryListBinding;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class CategoryAdapter extends RecyclerView.Adapter<CategoryAdapter.VH> {

    private final JSONArray jsonArray;

    public CategoryAdapter(JSONArray jsonArray) {
        this.jsonArray = jsonArray;
    }

    @NonNull
    @Override
    public CategoryAdapter.VH onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        return new VH(
                CategoryListBinding.inflate(LayoutInflater.from(parent.getContext()), parent, false)
        );
    }

    @Override
    public void onBindViewHolder(@NonNull CategoryAdapter.VH holder, int position) {
        try {
            JSONObject jsonObject = jsonArray.getJSONObject(position);
            _Helper.httpGetImage(holder.context, jsonObject.getString("imageUrl"), holder.binding.icCategory);
            holder.binding.name.setText(jsonObject.getString("name"));
            holder.binding.description.setText(jsonObject.getString("description"));

            holder.itemView.setOnClickListener(v -> {
                TourismActivity.jsonObject = jsonObject;
                holder.context.startActivity(new Intent(holder.context, TourismActivity.class));
            });

        } catch (JSONException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public int getItemCount() {
        return jsonArray.length();
    }

    public class VH extends RecyclerView.ViewHolder {
        private final CategoryListBinding binding;
        private final Context context;

        public VH(CategoryListBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
            this.context = binding.getRoot().getContext();
        }
    }
}