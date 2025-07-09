// Services/AraciKurumService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CompanyService.Model;
using CompanyService.Data;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace CompanyService.Services
{
    public class AraciKurumService : IAraciKurumService
    {
        private const string CacheKey = "AraciKurum:All";
        private readonly IDistributedCache _cache;
        private readonly AppDbContext _context;
        private readonly ILogger<AraciKurumService> _logger;

        public AraciKurumService(AppDbContext context, IDistributedCache cache, ILogger<AraciKurumService> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        public async Task<List<AraciKurum>> GetAllAsync()
        {
            // Redisten almaya çalıştık varsa veriyi döndük
            var cachedData = await _cache.GetStringAsync(CacheKey);
            if(!string.IsNullOrEmpty(cachedData)){
                _logger.LogInformation("Cache HIT for key: {CacheKey}", CacheKey);
                return JsonSerializer.Deserialize<List<AraciKurum>>(cachedData);
            }

            _logger.LogInformation("Cache MISS for key: {CacheKey}", CacheKey);

            //Redis'te yoksa veritabanından alıyoruz
            var araciKurumlar = await _context.ARACI_KURUM.ToListAsync();

            var options = new DistributedCacheEntryOptions{
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            var payload = JsonSerializer.Serialize(araciKurumlar);
            await _cache.SetStringAsync(CacheKey,payload,options);
            _logger.LogInformation("Data cached for key: {CacheKey}", CacheKey);

            return araciKurumlar;   
        }
    }
}