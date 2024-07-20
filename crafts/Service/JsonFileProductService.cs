using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using crafts.Model;
using Microsoft.AspNetCore.Hosting;

namespace crafts.Service
{
    public class JsonFileProductService
    {
        private readonly ILogger<JsonFileProductService> _logger;
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment, ILogger<JsonFileProductService> logger)
        {
            WebHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "product.json"); }
        }

        public IEnumerable<Product> GetProducts()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<Product[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        public void AddRating(string productId, int rating)
        {
            var products = GetProducts();
            _logger.LogInformation("Products: {Products}", JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true }));

            var query = products.First(x => x.Id==productId);
            if (query.Ratings == null)
            {
                query.Ratings = new int[] { rating };
            }
            else
            {
                var ratings = query.Ratings.ToList();
                ratings.Add(rating);
                query.Ratings.ToArray();
            }

            using (var outputstream = File.Open(JsonFileName, FileMode.Truncate))
            {
                JsonSerializer.Serialize<IEnumerable<Product>>(
                    new Utf8JsonWriter(outputstream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    products);
            }
        }
    }
}
