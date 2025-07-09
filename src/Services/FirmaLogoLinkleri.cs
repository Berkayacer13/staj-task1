using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using CompanyService.Data;
using CompanyService.DTO;
using CompanyService.Model;
using Prometheus;

namespace CompanyService.Services
{
    public class FirmaLogoLinkleriService : IFirmaLogoLinkleriService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly ILogger<FirmaLogoLinkleriService> _logger;
        private const string CacheKey = "FirmaLogoLinkleri:";

        public FirmaLogoLinkleriService(AppDbContext context, IMapper mapper, IDistributedCache cache, ILogger<FirmaLogoLinkleriService> logger)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }
        
        public async Task<List<LogoLinkDto>> GetLinksByHisseAdiAsync(string hisseAdi)
        {
            var cacheKey = $"{CacheKey}LinksByHisseAdi:{hisseAdi}";
            var cachedData = await _cache.GetStringAsync(cacheKey);
            if(!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation("Cache HIT for key: {CacheKey}", cacheKey);
                MetricsService.CacheHitCounter.WithLabels(cacheKey).Inc();
                return JsonSerializer.Deserialize<List<LogoLinkDto>>(cachedData);
            }
            
            _logger.LogInformation("Cache MISS for key: {CacheKey}", cacheKey);
            MetricsService.CacheMissCounter.WithLabels(cacheKey).Inc();
            
            var links = await _context.FirmaLogoLinkleri // store prossedure tamam baba bakarım 
                .Where(x => x.Hisse_Adi == hisseAdi)
                .ToListAsync();
            var result = _mapper.Map<List<LogoLinkDto>>(links);
            
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result));
            _logger.LogInformation("Data cached for key: {CacheKey}", cacheKey);
            
            return result;
        }

        public async Task<List<LogoLinkDto>> GetLinksByFirmaIdAsync(int firmaId)
        {
            var cacheKey = $"{CacheKey}LinksByFirmaId:{firmaId}";
            var cachedData = await _cache.GetStringAsync(cacheKey);
            if(!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation("Cache HIT for key: {CacheKey}", cacheKey);
                MetricsService.CacheHitCounter.WithLabels(cacheKey).Inc();
                return JsonSerializer.Deserialize<List<LogoLinkDto>>(cachedData);
            }
            
            _logger.LogInformation("Cache MISS for key: {CacheKey}", cacheKey);
            MetricsService.CacheMissCounter.WithLabels(cacheKey).Inc();
            
            var links = await _context.FirmaLogoLinkleri
                .Where(x => x.Firma_Id == firmaId)
                .ToListAsync();
            var result = _mapper.Map<List<LogoLinkDto>>(links);
            
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result));
            _logger.LogInformation("Data cached for key: {CacheKey}", cacheKey);
            
            return result;
        }

        public async Task<LogoLinkDto> CreateLogoLinkAsync(LogoLinkDto logoLinkDto)
        {
            var entity = _mapper.Map<FirmaLogoLinkleri>(logoLinkDto);
            _context.FirmaLogoLinkleri.Add(entity);
            await _context.SaveChangesAsync();
            
            // Invalidate cache
            await InvalidateCacheAsync(logoLinkDto.HisseAdi);
            await InvalidateCacheByFirmaIdAsync(logoLinkDto.FirmaId);
            
            return _mapper.Map<LogoLinkDto>(entity);
        }

        public async Task<LogoLinkDto> UpdateLogoLinkAsync(int firmaId, string hisseAdi, LogoLinkDto logoLinkDto)
        {
            var existingEntity = await _context.FirmaLogoLinkleri
                .FirstOrDefaultAsync(x => x.Firma_Id == firmaId && x.Hisse_Adi == hisseAdi);
                
            if (existingEntity == null)
                throw new InvalidOperationException("Logo link not found");
            
            // Update properties
            existingEntity.LogoLink = logoLinkDto.Url;
            
            await _context.SaveChangesAsync();
            
            // Invalidate cache
            await InvalidateCacheAsync(hisseAdi);
            await InvalidateCacheByFirmaIdAsync(firmaId);
            
            return _mapper.Map<LogoLinkDto>(existingEntity);
        }

        public async Task<bool> DeleteLogoLinkAsync(int firmaId, string hisseAdi)
        {
            var entity = await _context.FirmaLogoLinkleri
                .FirstOrDefaultAsync(x => x.Firma_Id == firmaId && x.Hisse_Adi == hisseAdi);
                
            if (entity == null)
                return false;
            
            _context.FirmaLogoLinkleri.Remove(entity);
            await _context.SaveChangesAsync();
            
            // Invalidate cache
            await InvalidateCacheAsync(hisseAdi);
            await InvalidateCacheByFirmaIdAsync(firmaId);
            
            return true;
        }

        private async Task InvalidateCacheAsync(string hisseAdi)
        {
            var cacheKey = $"{CacheKey}LinksByHisseAdi:{hisseAdi}";
            await _cache.RemoveAsync(cacheKey);
            _logger.LogInformation("Cache invalidated for key: {CacheKey}", cacheKey);
        }

        private async Task InvalidateCacheByFirmaIdAsync(int firmaId)
        {
            var cacheKey = $"{CacheKey}LinksByFirmaId:{firmaId}";
            await _cache.RemoveAsync(cacheKey);
            _logger.LogInformation("Cache invalidated for key: {CacheKey}", cacheKey);
        }
    }
}

