global using Namatara.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Namatara.API;
using Namatara.API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpContextAccessor();
// Menambahkan DbContext ke container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var key = builder.Configuration["Jwt:Key"] ?? "SuperSecretKey12345";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Tambahkan konfigurasi Bearer Token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Masukkan token JWT dengan format: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();  // Apply any pending migrations

    SeedData(dbContext);  // Call the seed method
}

static void SeedData(ApplicationDbContext dbContext)
{
    // Check if the User data already exists to avoid duplicate seeding
    var adminUserId = Guid.Parse("49B35962-536D-4DE4-93C7-E602172BD9D8");  // Admin's GUID (replace it with a fixed GUID if needed)
    if (!dbContext.Users.Any(u => u.Username == "admin"))  // Check if the admin user already exists
    {
        // Seed User
        dbContext.Users.AddRange(
            new User
            {
                Id = adminUserId,
                Username = "admin",
                Password = new PasswordHelper().HashPassword("adminpassword"),  // You should hash the password in a real-world app
                Role = UserRole.Admin,  // Set the role to Admin
                FullName = "Admin",
                DateOfBirth = DateTime.UtcNow,
                ImageUrl = ImageHelper.GetRandomImageUrl(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );

        dbContext.SaveChanges();  // Save the changes to the database
    }

    // Hardcode Category IDs
    var wisataCategoryId = Guid.Parse("D99E5C64-F169-4B3C-8BE5-502CA48D66D5");
    var umkmCategoryId = Guid.Parse("C8A59E96-8D7E-4E1E-BEB1-79FB9A69C3A2");
    var homestayCategoryId = Guid.Parse("A8E587D9-CE38-4FB8-9549-5108315A35A5");
    var paketWisataCategoryId = Guid.Parse("95FEC8C3-86A2-4790-AF90-B1D16B440C57");
    var acaraFestivalCategoryId = Guid.Parse("2D981623-08A9-45A0-AF72-8A9CC8763E68");

    // Check if the Categories already exist
    var existingCategoryNames = dbContext.Categories.Select(c => c.Name).ToList();

    var categoriesToAdd = new List<Category>
    {
        new()
        {
            Id = wisataCategoryId,
            Name = "Wisata",
            Description = "Kategori yang mencakup berbagai tempat wisata yang dapat dikunjungi oleh wisatawan.",
            CreatedBy = adminUserId,
            CreatedAt = DateTime.UtcNow
        },
        new()
        {
            Id = umkmCategoryId,
            Name = "UMKM",
            Description = "Kategori untuk Usaha Mikro, Kecil, dan Menengah yang dapat dipromosikan melalui platform ini.",
            CreatedBy = adminUserId,
            CreatedAt = DateTime.UtcNow
        },
        new()
        {
            Id = homestayCategoryId,
            Name = "Homestay",
            Description = "Kategori penginapan yang menawarkan pengalaman tinggal seperti di rumah sendiri.",
            CreatedBy = adminUserId,
            CreatedAt = DateTime.UtcNow
        },
        new()
        {
            Id = paketWisataCategoryId,
            Name = "Paket Wisata & Edukasi",
            Description = "Kategori untuk paket wisata yang juga mencakup elemen edukasi, seperti tur sejarah atau pelatihan.",
            CreatedBy = adminUserId,
            CreatedAt = DateTime.UtcNow
        },
        new()
        {
            Id = acaraFestivalCategoryId,
            Name = "Acara & Festival",
            Description = "Kategori untuk berbagai acara dan festival yang diadakan di berbagai lokasi wisata.",
            CreatedBy = adminUserId,
            CreatedAt = DateTime.UtcNow
        }
    };

    // Add categories only if they do not already exist in the database
    foreach (var category in categoriesToAdd)
    {
        if (!existingCategoryNames.Contains(category.Name))
        {
            dbContext.Categories.Add(category);
        }
    }

    dbContext.SaveChanges();  // Save the changes to the database

    // Seed TourismAttractions
    var attractionsToAdd = new List<TourismAttraction>
    {
        // Kategori Wisata
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = wisataCategoryId,
            Price = 50000,
            Name = "Pantai Kuta",
            Description = "Pantai yang terkenal dengan pasir putihnya dan ombak yang cocok untuk berselancar.",
            Location = "Kuta, Bali",
            OpeningHours = "24 Jam",
            Rating = 4.7m,
            ImageUrl = "https://example.com/images/pantai-kuta.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = wisataCategoryId,
            Price = 30000,
            Name = "Air Terjun Gitgit",
            Description = "Air terjun dengan pemandangan alam yang indah, terletak di daerah pegunungan Bali.",
            Location = "Gitgit, Buleleng, Bali",
            OpeningHours = "08:00 - 17:00",
            Rating = 4.5m,
            ImageUrl = "https://example.com/images/air-terjun-gitgit.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = wisataCategoryId,
            Price = 100000,
            Name = "Taman Nasional Bali Barat",
            Description = "Taman nasional yang menawarkan berbagai aktivitas alam dan wisata petualangan.",
            Location = "Bali Barat, Bali",
            OpeningHours = "07:00 - 18:00",
            Rating = 4.6m,
            ImageUrl = "https://example.com/images/taman-nasional-bali-barat.jpg"
        },

        // Kategori UMKM
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = umkmCategoryId,
            Price = 10000,
            Name = "Pasar Seni Ubud",
            Description = "Pasar seni yang menjual berbagai kerajinan tangan dan produk lokal Bali.",
            Location = "Ubud, Bali",
            OpeningHours = "08:00 - 19:00",
            Rating = 4.3m,
            ImageUrl = "https://example.com/images/pasar-seni-ubud.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = umkmCategoryId,
            Price = 5000,
            Name = "Pasar Badung",
            Description = "Pasar tradisional di Denpasar yang menjual berbagai barang, mulai dari bahan makanan hingga kerajinan tangan.",
            Location = "Denpasar, Bali",
            OpeningHours = "06:00 - 20:00",
            Rating = 4.2m,
            ImageUrl = "https://example.com/images/pasar-badung.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = umkmCategoryId,
            Price = 20000,
            Name = "Pasar Sukawati",
            Description = "Pasar seni yang terkenal dengan barang-barang seni dan kerajinan tangan asli Bali.",
            Location = "Sukawati, Bali",
            OpeningHours = "08:00 - 18:00",
            Rating = 4.4m,
            ImageUrl = "https://example.com/images/pasar-sukawati.jpg"
        },

        // Kategori Homestay
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = homestayCategoryId,
            Price = 150000,
            Name = "Homestay Tepi Laut",
            Description = "Homestay dengan pemandangan laut langsung, memberikan pengalaman menginap yang nyaman.",
            Location = "Jalan Raya Pantai, Bali",
            OpeningHours = "Tersedia 24 jam",
            Rating = 4.6m,
            ImageUrl = "https://example.com/images/homestay-tepi-laut.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = homestayCategoryId,
            Price = 120000,
            Name = "Villa Bali Seaview",
            Description = "Villa dengan pemandangan laut dan fasilitas modern untuk pengalaman menginap yang luar biasa.",
            Location = "Sanur, Bali",
            OpeningHours = "Tersedia 24 jam",
            Rating = 4.8m,
            ImageUrl = "https://example.com/images/villa-bali-seaview.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = homestayCategoryId,
            Price = 180000,
            Name = "Bali Mountain Retreat",
            Description = "Retreat yang menawarkan pengalaman menginap dengan suasana pegunungan dan ketenangan.",
            Location = "Bedugul, Bali",
            OpeningHours = "Tersedia 24 jam",
            Rating = 4.7m,
            ImageUrl = "https://example.com/images/bali-mountain-retreat.jpg"
        },

        // Kategori Acara & Festival
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = acaraFestivalCategoryId,
            Price = 0,
            Name = "Festival Kesenian Ubud",
            Description = "Festival seni tahunan yang menampilkan berbagai pertunjukan seni tradisional Bali.",
            Location = "Ubud, Bali",
            OpeningHours = "10:00 - 22:00",
            Rating = 4.8m,
            ImageUrl = "https://example.com/images/festival-kesenian-ubud.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = acaraFestivalCategoryId,
            Price = 0,
            Name = "Festival Bali Arts",
            Description = "Festival seni yang menampilkan seni rupa, tari, musik, dan budaya Bali.",
            Location = "Denpasar, Bali",
            OpeningHours = "09:00 - 21:00",
            Rating = 4.6m,
            ImageUrl = "https://example.com/images/festival-bali-arts.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = acaraFestivalCategoryId,
            Price = 0,
            Name = "Bali International Film Festival",
            Description = "Festival film internasional yang menampilkan karya-karya terbaik dari seluruh dunia.",
            Location = "Jimbaran, Bali",
            OpeningHours = "18:00 - 23:00",
            Rating = 4.7m,
            ImageUrl = "https://example.com/images/bali-international-film-festival.jpg"
        }
    };

    dbContext.TourismAttractions.AddRange(attractionsToAdd);
    dbContext.SaveChanges();
}

app.Run();