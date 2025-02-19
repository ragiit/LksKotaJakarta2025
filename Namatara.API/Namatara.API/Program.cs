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

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); // Apply any pending migrations

    SeedData(dbContext); // Call the seed method
}

static void SeedData(ApplicationDbContext dbContext)
{
    // Check if the User data already exists to avoid duplicate seeding
    var adminUserId =
        Guid.Parse("49B35962-536D-4DE4-93C7-E602172BD9D8"); // Admin's GUID (replace it with a fixed GUID if needed)
    if (!dbContext.Users.Any(u => u.Username == "admin")) // Check if the admin user already exists
    {
        // Seed User
        dbContext.Users.AddRange(
            new User
            {
                Id = adminUserId,
                Username = "admin",
                Password = new PasswordHelper()
                    .HashPassword("adminpassword"), // You should hash the password in a real-world app
                Role = UserRole.Admin, // Set the role to Admin
                FullName = "Admin",
                DateOfBirth = DateTime.UtcNow,
                ImageUrl = ImageHelper.GetRandomImageUrl(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "argi",
                Password = new PasswordHelper().HashPassword("123"),
                Role = UserRole.User,
                FullName = "Argi",
                DateOfBirth = DateTime.UtcNow,
                ImageUrl = ImageHelper.GetRandomImageUrl(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "budi",
                Password = new PasswordHelper().HashPassword("123"),
                Role = UserRole.User,
                FullName = "Budi",
                DateOfBirth = DateTime.UtcNow,
                ImageUrl = ImageHelper.GetRandomImageUrl(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "charlie",
                Password = new PasswordHelper().HashPassword("123"),
                Role = UserRole.User,
                FullName = "Charlie",
                DateOfBirth = DateTime.UtcNow,
                ImageUrl = ImageHelper.GetRandomImageUrl(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "delta",
                Password = new PasswordHelper().HashPassword("123"),
                Role = UserRole.User,
                FullName = "Delta",
                DateOfBirth = DateTime.UtcNow,
                ImageUrl = ImageHelper.GetRandomImageUrl(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "echo",
                Password = new PasswordHelper().HashPassword("123"),
                Role = UserRole.User,
                FullName = "Echo",
                DateOfBirth = DateTime.UtcNow,
                ImageUrl = ImageHelper.GetRandomImageUrl(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "foxtrot",
                Password = new PasswordHelper().HashPassword("123"),
                Role = UserRole.User,
                FullName = "Foxtrot",
                DateOfBirth = DateTime.UtcNow,
                ImageUrl = ImageHelper.GetRandomImageUrl(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "golf",
                Password = new PasswordHelper().HashPassword("123"),
                Role = UserRole.User,
                FullName = "Golf",
                DateOfBirth = DateTime.UtcNow,
                ImageUrl = ImageHelper.GetRandomImageUrl(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );

        dbContext.SaveChanges(); // Save the changes to the database
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
            ImageUrl = "Images/category_wisata.jpeg",
            CreatedBy = adminUserId,
            CreatedAt = DateTime.UtcNow
        },
        new()
        {
            Id = umkmCategoryId,
            Name = "UMKM",
            Description =
                "Kategori untuk Usaha Mikro, Kecil, dan Menengah yang dapat dipromosikan melalui platform ini.",
            ImageUrl = "Images/category_umkm.jpeg",
            CreatedBy = adminUserId,
            CreatedAt = DateTime.UtcNow
        },
        new()
        {
            Id = homestayCategoryId,
            Name = "Homestay",
            Description = "Kategori penginapan yang menawarkan pengalaman tinggal seperti di rumah sendiri.",
            ImageUrl = "Images/category_homestay.jpeg",
            CreatedBy = adminUserId,
            CreatedAt = DateTime.UtcNow
        },
        new()
        {
            Id = paketWisataCategoryId,
            Name = "Paket Wisata & Edukasi",
            Description =
                "Kategori untuk paket wisata yang juga mencakup elemen edukasi, seperti tur sejarah atau pelatihan.",
            ImageUrl = "Images/category_wisata_edukasi.jpg",
            CreatedBy = adminUserId,
            CreatedAt = DateTime.UtcNow
        },
        new()
        {
            Id = acaraFestivalCategoryId,
            Name = "Acara & Festival",
            Description = "Kategori untuk berbagai acara dan festival yang diadakan di berbagai lokasi wisata.",
            ImageUrl = "Images/category_acara_festival.jpg",
            CreatedBy = adminUserId,
            CreatedAt = DateTime.UtcNow
        }
    };

    // Add categories only if they do not already exist in the database
    foreach (var category in categoriesToAdd.Where(category => !existingCategoryNames.Contains(category.Name)))
    {
        dbContext.Categories.Add(category);
    }

    dbContext.SaveChanges(); // Save the changes to the database

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
            Description =
                "Pantai Kuta adalah salah satu pantai paling terkenal di Bali, Indonesia. Pantai ini dikenal dengan pasir putihnya yang lembut serta ombaknya yang menantang, menjadikannya destinasi favorit bagi para peselancar dari seluruh dunia. Selain itu, Pantai Kuta menawarkan pemandangan matahari terbenam yang sangat indah, menciptakan suasana romantis bagi para pengunjung. Di sekitar pantai, terdapat banyak restoran, kafe, dan pusat perbelanjaan yang memudahkan wisatawan menikmati liburan mereka. Berbagai aktivitas juga bisa dilakukan di sini, seperti berjalan-jalan di tepi pantai, bermain voli pantai, atau sekadar bersantai menikmati deburan ombak.",
            Location = "Kuta, Bali",
            OpeningHours = "24 Jam",
            Rating = 4.7m,
            ImageUrl = "Images/pantai-kuta.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = wisataCategoryId,
            Price = 30000,
            Name = "Air Terjun Gitgit",
            Description =
                "Air Terjun Gitgit merupakan salah satu air terjun paling menawan di Bali yang terletak di kawasan pegunungan yang asri di Kabupaten Buleleng. Air terjun ini memiliki ketinggian sekitar 35 meter dan dikelilingi oleh hutan tropis yang rimbun, menciptakan suasana sejuk dan menenangkan. Suara gemuruh air yang jatuh dari ketinggian menambah kesan magis dari tempat ini. Untuk mencapai air terjun, pengunjung harus melakukan trekking melewati jalan setapak yang dikelilingi pepohonan rindang dan suara alam yang menyegarkan. Di sekitar air terjun, terdapat beberapa warung kecil yang menjual makanan dan minuman lokal. Tempat ini juga menjadi lokasi favorit bagi fotografer yang ingin mengabadikan keindahan alam Bali.",
            Location = "Gitgit, Buleleng, Bali",
            OpeningHours = "08:00 - 17:00",
            Rating = 4.5m,
            ImageUrl = "Images/air-terjun-gitgit.jpeg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = wisataCategoryId,
            Price = 100000,
            Name = "Taman Nasional Bali Barat",
            Description =
                "Taman Nasional Bali Barat adalah salah satu kawasan konservasi alam terbesar di Bali yang menawarkan keindahan ekosistem yang sangat beragam. Terletak di bagian barat Pulau Bali, taman nasional ini mencakup hutan hujan tropis, hutan mangrove, padang rumput, serta terumbu karang yang kaya akan kehidupan bawah laut. Pengunjung yang datang ke sini dapat menikmati berbagai aktivitas menarik seperti trekking di hutan, birdwatching untuk melihat burung jalak Bali yang langka, snorkeling, dan menyelam di perairan Pulau Menjangan. Tempat ini sangat cocok bagi wisatawan yang menyukai petualangan dan ingin lebih dekat dengan alam. Selain itu, terdapat beberapa area berkemah bagi mereka yang ingin menikmati malam di bawah langit penuh bintang.",
            Location = "Bali Barat, Bali",
            OpeningHours = "07:00 - 18:00",
            Rating = 4.6m,
            ImageUrl = "Images/taman-nasional-bali-barat.jpg"
        },

        // Kategori UMKM
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = umkmCategoryId,
            Price = 10000,
            Name = "Pasar Seni Ubud",
            Description =
                "Pasar Seni Ubud adalah pusat perbelanjaan tradisional yang menjual berbagai macam kerajinan tangan khas Bali, mulai dari patung kayu, lukisan, tas anyaman, pakaian tradisional, hingga perhiasan perak yang dibuat secara manual oleh pengrajin lokal. Pasar ini menjadi salah satu tujuan utama wisatawan yang ingin membawa pulang oleh-oleh khas Bali dengan harga yang lebih terjangkau dibandingkan toko-toko di daerah wisata lainnya. Selain berbelanja, pengunjung juga dapat menikmati interaksi langsung dengan para seniman dan pengrajin yang menjual barang dagangan mereka di sini. Tempat ini juga sering dijadikan lokasi fotografi karena suasana tradisional dan warna-warni barang yang dipajang di kios-kiosnya.",
            Location = "Ubud, Bali",
            OpeningHours = "08:00 - 19:00",
            Rating = 4.3m,
            ImageUrl = "Images/pasar-seni-ubud.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = umkmCategoryId,
            Price = 5000,
            Name = "Pasar Badung",
            Description =
                "Pasar Badung merupakan pasar tradisional terbesar di Denpasar, Bali. Pasar ini menawarkan berbagai macam barang dagangan, mulai dari bahan makanan segar seperti sayuran, buah-buahan, ikan, dan daging, hingga pakaian, kain batik, serta kerajinan tangan khas Bali. Salah satu daya tarik utama pasar ini adalah suasananya yang ramai dan interaksi sosial yang hidup antara para pedagang dan pembeli. Banyak wisatawan datang ke pasar ini tidak hanya untuk berbelanja, tetapi juga untuk merasakan atmosfer pasar tradisional Bali yang autentik. Bangunan pasar ini terdiri dari beberapa lantai, sehingga pengunjung dapat dengan mudah menemukan berbagai jenis barang yang mereka butuhkan. Selain itu, harga di Pasar Badung cenderung lebih murah dibandingkan dengan pusat perbelanjaan modern, terutama jika pandai menawar harga dengan para pedagang lokal.",
            Location = "Denpasar, Bali",
            OpeningHours = "06:00 - 20:00",
            Rating = 4.2m,
            ImageUrl = "Images/pasar-badung.jpeg"
        },

        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = umkmCategoryId,
            Price = 20000,
            Name = "Pasar Sukawati",
            Description =
                "Pasar seni yang terkenal dengan barang-barang seni dan kerajinan tangan asli Bali. Di pasar ini, pengunjung dapat menemukan berbagai macam lukisan, patung, pakaian tradisional, tas anyaman, serta perhiasan khas Bali dengan harga yang terjangkau. Suasana pasar yang ramai dipenuhi oleh pedagang lokal yang menawarkan hasil karya seni mereka, menciptakan pengalaman belanja yang unik dan otentik.",
            Location = "Sukawati, Bali",
            OpeningHours = "08:00 - 18:00",
            Rating = 4.4m,
            ImageUrl = "Images/pasar-sukawati.jpg"
        },

// Kategori Homestay
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = homestayCategoryId,
            Price = 150000,
            Name = "Homestay Tepi Laut",
            Description =
                "Homestay dengan pemandangan laut langsung, memberikan pengalaman menginap yang nyaman dan menenangkan. Lokasinya yang strategis di pinggir pantai memungkinkan tamu menikmati matahari terbit dan terbenam langsung dari balkon kamar. Dilengkapi dengan fasilitas modern seperti WiFi gratis, sarapan pagi, dan akses mudah ke tempat-tempat wisata di sekitar, menjadikan homestay ini pilihan ideal bagi wisatawan yang mencari ketenangan dan kenyamanan.",
            Location = "Jalan Raya Pantai, Bali",
            OpeningHours = "Tersedia 24 jam",
            Rating = 4.6m,
            ImageUrl = "Images/homestay-tepi-laut.jpeg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = homestayCategoryId,
            Price = 120000,
            Name = "Villa Bali Seaview",
            Description =
                "Villa dengan pemandangan laut dan fasilitas modern untuk pengalaman menginap yang luar biasa. Setiap kamar di villa ini dirancang dengan gaya arsitektur tropis yang mengutamakan kenyamanan dan privasi. Tersedia kolam renang pribadi, restoran dengan menu khas Bali, serta akses langsung ke pantai. Tempat ini sangat cocok untuk pasangan yang mencari suasana romantis atau keluarga yang ingin menikmati waktu berkualitas bersama.",
            Location = "Sanur, Bali",
            OpeningHours = "Tersedia 24 jam",
            Rating = 4.8m,
            ImageUrl = "Images/villa-bali-seaview.jpeg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = homestayCategoryId,
            Price = 180000,
            Name = "Bali Mountain Retreat",
            Description =
                "Retreat yang menawarkan pengalaman menginap dengan suasana pegunungan dan ketenangan. Dikelilingi oleh hutan tropis yang hijau dan udara segar, retreat ini menjadi tempat sempurna bagi mereka yang ingin melepaskan diri dari kesibukan kota. Berbagai aktivitas seperti yoga, meditasi, dan trekking dapat dilakukan di sini untuk menyegarkan tubuh dan pikiran.",
            Location = "Bedugul, Bali",
            OpeningHours = "Tersedia 24 jam",
            Rating = 4.7m,
            ImageUrl = "Images/bali-mountain-retreat.jpg"
        },

// Kategori Wisata & Edukasi 
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = paketWisataCategoryId,
            Price = 250000,
            Name = "Museum Sejarah Nusantara",
            Description =
                "Museum yang menyajikan koleksi artefak sejarah dari berbagai zaman di Nusantara. Pengunjung dapat melihat berbagai peninggalan bersejarah, mulai dari zaman kerajaan Hindu-Buddha hingga masa kolonial. Museum ini juga memiliki berbagai diorama dan presentasi interaktif yang membuat pengalaman belajar sejarah semakin menarik.",
            Location = "Jalan Proklamasi, Jakarta",
            OpeningHours = "09.00 - 17.00",
            Rating = 4.8m,
            ImageUrl = "Images/museum-sejarah.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = paketWisataCategoryId,
            Price = 50000,
            Name = "Kebun Edukasi Tani",
            Description =
                "Kunjungan edukasi pertanian untuk mengenal lebih dekat proses bercocok tanam. Di sini, pengunjung dapat belajar tentang teknik bertani organik, hidroponik, dan peran penting pertanian dalam kehidupan sehari-hari. Selain itu, mereka juga dapat langsung mencoba menanam sayuran dan memanen hasil kebun, menciptakan pengalaman yang menyenangkan dan edukatif.",
            Location = "Desa Hijau, Yogyakarta",
            OpeningHours = "08.00 - 16.00",
            Rating = 4.5m,
            ImageUrl = "Images/kebun-edukasi.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = paketWisataCategoryId,
            Price = 175000,
            Name = "Planetarium Bintang Timur",
            Description =
                "Planetarium Bintang Timur menghadirkan pengalaman luar angkasa interaktif dengan teknologi canggih. " +
                "Pengunjung dapat menikmati simulasi langit malam, menjelajahi rasi bintang, serta memahami pergerakan planet dalam tata surya. " +
                "Program edukasi ini cocok untuk segala usia, menjadikannya destinasi wisata edukatif yang menarik di Bandung.",
            Location = "Jalan Observatorium, Bandung",
            OpeningHours = "10:00 - 18:00",
            Rating = 4.7m,
            ImageUrl = "Images/planetarium.jpg"
        },

// Kategori Acara & Festival
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = acaraFestivalCategoryId,
            Price = 0,
            Name = "Festival Kesenian Ubud",
            Description = "Festival seni tahunan di Ubud yang merayakan kekayaan seni dan budaya Bali. " +
                          "Acara ini menampilkan tarian tradisional, pertunjukan wayang kulit, serta pameran seni rupa dari seniman lokal maupun internasional. " +
                          "Festival ini menjadi ajang penting dalam melestarikan budaya serta menarik wisatawan dari berbagai belahan dunia.",
            Location = "Ubud, Bali",
            OpeningHours = "10:00 - 22:00",
            Rating = 4.8m,
            ImageUrl = "Images/festival-kesenian-ubud.jpeg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = acaraFestivalCategoryId,
            Price = 0,
            Name = "Festival Bali Arts",
            Description =
                "Festival Bali Arts adalah perayaan besar seni dan budaya yang menghadirkan pameran seni rupa, pertunjukan tari, dan konser musik. " +
                "Festival ini menjadi wadah bagi seniman lokal untuk memamerkan karya mereka, termasuk ukiran kayu, kain batik khas Bali, dan berbagai kerajinan tangan. " +
                "Dengan suasana yang penuh warna dan semangat, festival ini menjadi daya tarik utama bagi para pencinta seni dan budaya.",
            Location = "Denpasar, Bali",
            OpeningHours = "09:00 - 21:00",
            Rating = 4.6m,
            ImageUrl = "Images/festival-bali-arts.jpeg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            CategoryId = acaraFestivalCategoryId,
            Price = 0,
            Name = "Bali International Film Festival",
            Description =
                "Bali International Film Festival, atau Balinale, adalah ajang perfilman bergengsi yang menghadirkan film berkualitas dari berbagai negara. " +
                "Festival ini menjadi tempat bagi sineas lokal dan internasional untuk berbagi cerita, menampilkan film independen, serta berdiskusi dalam sesi tanya jawab. " +
                "Selain pemutaran film, Balinale juga menyelenggarakan lokakarya dan seminar seputar industri film dan teknik penyutradaraan.",
            Location = "Jimbaran, Bali",
            OpeningHours = "18:00 - 23:00",
            Rating = 4.7m,
            ImageUrl = "Images/bali-international-film-festival.jpeg"
        }
    };

    var existingAttractions = dbContext.TourismAttractions.Select(t => t.Name).ToList();

    foreach (var attraction in attractionsToAdd.Where(attraction => !existingAttractions.Contains(attraction.Name)))
    {
        dbContext.TourismAttractions.Add(attraction);
    }

    dbContext.SaveChanges();

    // if (attractionsToAdd.Count == 0)
    attractionsToAdd = dbContext.TourismAttractions.AsNoTracking().ToList();

    var bookmarks = new List<TourismAttractionBookmark>
    {
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[0].Id,
            UserId = dbContext.Users.Where(u => u.Username == "argi").Select(u => u.Id).FirstOrDefault(),
            CreatedBy = adminUserId,
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[1].Id,
            UserId = dbContext.Users.Where(u => u.Username == "argi").Select(u => u.Id).FirstOrDefault()
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[2].Id,
            UserId = dbContext.Users.Where(u => u.Username == "argi").Select(u => u.Id).FirstOrDefault()
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[3].Id,
            UserId = dbContext.Users.Where(u => u.Username == "foxtrot").Select(u => u.Id).FirstOrDefault()
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[4].Id,
            UserId = dbContext.Users.Where(u => u.Username == "golf").Select(u => u.Id).FirstOrDefault()
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[5].Id,
            UserId = dbContext.Users.Where(u => u.Username == "golf").Select(u => u.Id).FirstOrDefault()
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[6].Id,
            UserId = dbContext.Users.Where(u => u.Username == "budi").Select(u => u.Id).FirstOrDefault()
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[7].Id,
            UserId = dbContext.Users.Where(u => u.Username == "budi").Select(u => u.Id).FirstOrDefault()
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[8].Id,
            UserId = dbContext.Users.Where(u => u.Username == "budi").Select(u => u.Id).FirstOrDefault()
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[9].Id,
            UserId = dbContext.Users.Where(u => u.Username == "charlie").Select(u => u.Id).FirstOrDefault()
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[10].Id,
            UserId = dbContext.Users.Where(u => u.Username == "charlie").Select(u => u.Id).FirstOrDefault()
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[11].Id,
            UserId = dbContext.Users.Where(u => u.Username == "delta").Select(u => u.Id).FirstOrDefault()
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[12].Id,
            UserId = dbContext.Users.Where(u => u.Username == "delta").Select(u => u.Id).FirstOrDefault()
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[13].Id,
            UserId = dbContext.Users.Where(u => u.Username == "echo").Select(u => u.Id).FirstOrDefault()
        },
    };

    var existingBookmarks = dbContext.TourismAttractionBookmarks.Select(t => new
    {
        TourismAttractionId = t.TourismAttractionId,
        UserId = t.UserId
    }).ToList();

    foreach (var x in bookmarks.Where(attraction =>
                 existingBookmarks.All(b => b.TourismAttractionId != attraction.Id && b.UserId != attraction.UserId)))
    {
        dbContext.TourismAttractionBookmarks.Add(x);
    }

    var ratings = new List<TourismAttractionRating>
    {
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[0].Id,
            UserId = dbContext.Users.Where(u => u.Username == "argi").Select(u => u.Id).FirstOrDefault(),
            Rating = 4.5m
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[1].Id,
            UserId = dbContext.Users.Where(u => u.Username == "argi").Select(u => u.Id).FirstOrDefault(),
            Rating = 3.8m
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[2].Id,
            UserId = dbContext.Users.Where(u => u.Username == "argi").Select(u => u.Id).FirstOrDefault(),
            Rating = 5.0m
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[3].Id,
            UserId = dbContext.Users.Where(u => u.Username == "foxtrot").Select(u => u.Id).FirstOrDefault(),
            Rating = 4.2m
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[4].Id,
            UserId = dbContext.Users.Where(u => u.Username == "golf").Select(u => u.Id).FirstOrDefault(),
            Rating = 3.5m
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[5].Id,
            UserId = dbContext.Users.Where(u => u.Username == "golf").Select(u => u.Id).FirstOrDefault(),
            Rating = 4.7m
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[6].Id,
            UserId = dbContext.Users.Where(u => u.Username == "budi").Select(u => u.Id).FirstOrDefault(),
            Rating = 4.0m
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[7].Id,
            UserId = dbContext.Users.Where(u => u.Username == "budi").Select(u => u.Id).FirstOrDefault(),
            Rating = 3.9m
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[8].Id,
            UserId = dbContext.Users.Where(u => u.Username == "budi").Select(u => u.Id).FirstOrDefault(),
            Rating = 4.8m
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[9].Id,
            UserId = dbContext.Users.Where(u => u.Username == "charlie").Select(u => u.Id).FirstOrDefault(),
            Rating = 4.3m
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[10].Id,
            UserId = dbContext.Users.Where(u => u.Username == "charlie").Select(u => u.Id).FirstOrDefault(),
            Rating = 3.6m
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[11].Id,
            UserId = dbContext.Users.Where(u => u.Username == "delta").Select(u => u.Id).FirstOrDefault(),
            Rating = 4.9m
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[12].Id,
            UserId = dbContext.Users.Where(u => u.Username == "delta").Select(u => u.Id).FirstOrDefault(),
            Rating = 3.7m
        },
        new()
        {
            Id = Guid.NewGuid(),
            TourismAttractionId = attractionsToAdd[13].Id,
            UserId = dbContext.Users.Where(u => u.Username == "echo").Select(u => u.Id).FirstOrDefault(),
            Rating = 4.1m
        },
    };

// Cek apakah rating sudah ada agar tidak duplikasi
    var existingRatings = dbContext.TourismAttractionRatings
        .Select(t => new { t.TourismAttractionId, t.UserId })
        .ToList();

    foreach (var rating in ratings.Where(r =>
                 existingRatings.All(e => e.TourismAttractionId != r.TourismAttractionId || e.UserId != r.UserId)))
    {
        dbContext.TourismAttractionRatings.Add(rating);
    }


    dbContext.SaveChanges();
}

app.Run();