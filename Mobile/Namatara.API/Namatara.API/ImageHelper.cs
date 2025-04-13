namespace Namatara.API
{
    public class ImageHelper
    {
        private static readonly Random _random = new Random();

        private static readonly string[] _imageFiles = new string[]
        {
            "image1.jpg", // Ganti dengan nama file gambar yang ada di folder Images
            "image2.jpg",
            "image3.jpg",
            "image4.jpg",
            "image5.jpg"
        };

        public static string GetRandomImageUrl()
        {
            // Pilih gambar secara acak dari daftar _imageFiles
            var randomImage = _imageFiles[_random.Next(_imageFiles.Length)];

            // Return URL relatif ke folder wwwroot/Images
            return $"Images/{randomImage}";
        }
    }
}