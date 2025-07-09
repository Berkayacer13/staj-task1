# Company Service API

Bu proje, modern .NET 9.0 teknolojileri kullanılarak geliştirilmiş kapsamlı bir şirket yönetim API'sidir. Proje sürecinde Entity Framework Core ile veritabanı entegrasyonu, Redis ile yüksek performanslı önbellekleme sistemi, dış API entegrasyonu (randomuser.me), API Key tabanlı güvenlik sistemi, Prometheus ile metrik toplama, Docker containerization ve kapsamlı loglama sistemi implementasyonu gerçekleştirilmiştir.

**Geliştirme Sürecinde Yapılan İşler:**
- ✅ **Veritabanı Tasarımı**: Mevcut SQL Server veritabanı ile Entity Framework Core entegrasyonu
- ✅ **Cache Sistemi**: Redis ile cache hit/miss loglama ve otomatik cache invalidation
- ✅ **Güvenlik**: X-API-Key header ile kimlik doğrulama sistemi
- ✅ **Monitoring**: Health check endpoint'i ve Prometheus metrikleri
- ✅ **External API**: Random user API entegrasyonu ile şirket verisi simülasyonu
- ✅ **Docker**: Multi-stage Dockerfile ve docker-compose ile tam ortam
- ✅ **Logging**: Structured logging ile cache performans takibi
- ✅ **Documentation**: Swagger/OpenAPI ile kapsamlı API dokümantasyonu

**Teknik Başarılar:**
- Performans optimizasyonu ile cache hit oranlarının artırılması
- Güvenli API erişimi için middleware implementasyonu
- Mikroservis mimarisine uygun containerization
- Production-ready monitoring ve observability çözümleri

## 🚀 Özellikler

### Temel İşlevsellik
- **Şirket Yönetimi**: Şirket verileri için CRUD işlemleri
- **Logo Link Yönetimi**: Hisse adi ve firma ID'ye göre şirket logo linklerini yönetme
- **Dış API Entegrasyonu**: Dış API'lerden rastgele şirket verisi çekme
- **Redis Önbellekleme**: Cache hit/miss loglama ile yüksek performanslı önbellekleme

### Güvenlik ve Gözlemlenebilirlik
- **API Key Kimlik Doğrulama**: X-API-Key header ile güvenli API erişimi
- **Sağlık İzleme**: Servis sağlık kontrolleri için `/health` endpoint'i
- **Prometheus Metrikleri**: Kapsamlı metrik toplama ve izleme
- **Yapılandırılmış Loglama**: Cache performans takibi ile detaylı loglama

### Teknik Altyapı
- **.NET 9.0**: En son .NET framework
- **Entity Framework Core**: Veritabanı ORM
- **SQL Server**: Ana veritabanı
- **Redis**: Dağıtık önbellekleme
- **AutoMapper**: Nesne eşleme
- **Swagger/OpenAPI**: API dokümantasyonu
- **Prometheus**: Metrik toplama

## 📋 Gereksinimler

- .NET 9.0 SDK
- SQL Server (veya Docker)
- Redis (veya Docker)
- Docker & Docker Compose (konteynerli dağıtım için)


## 🔐 Kimlik Doğrulama

Tüm API endpoint'leri (health ve metrics hariç) API key kimlik doğrulaması gerektirir.

### API Key Kullanımı
```http
GET /api/FirmaLogoLinkleri/hisseadi/THYAO
X-API-Key: your-super-secret-api-key-2024
```

### cURL Örneği
```bash
curl -X GET "https://localhost:7299/api/FirmaLogoLinkleri/hisseadi/THYAO" \
  -H "X-API-Key: your-super-secret-api-key-2024" \
  -k
```

## 📚 API Endpoint'leri

### Şirket Logo Linkleri
- `GET /api/FirmaLogoLinkleri/hisseadi/{hisseAdi}` - Hisse adi'ne göre logo linklerini getir
- `GET /api/FirmaLogoLinkleri/firma/{firmaId}` - Firma ID'ye göre logo linklerini getir
- `POST /api/FirmaLogoLinkleri` - Yeni logo linki oluştur
- `PUT /api/FirmaLogoLinkleri/{firmaId}/{hisseAdi}` - Logo linkini güncelle
- `DELETE /api/FirmaLogoLinkleri/{firmaId}/{hisseAdi}` - Logo linkini sil

### Aracı Kurum
- `GET /api/AraciKurum` - Tüm aracı kurum verilerini getir

### Dış Şirketler
- `GET /api/external/companies/random` - Dış API'den rastgele şirket getir
- `GET /api/external/companies/random/{count}` - Birden fazla rastgele şirket getir

### Sistem Endpoint'leri
- `GET /health` - Sağlık kontrolü (kimlik doğrulama gerekmez)
- `GET /metrics` - Prometheus metrikleri (kimlik doğrulama gerekmez)
- `GET /` - Swagger UI (kimlik doğrulama gerekmez)

## 📊 İzleme ve Gözlemlenebilirlik

### Sağlık Kontrolü
```bash
curl http://localhost:5245/health
```

**Yanıt:**
```json
{
  "status": "Healthy",
  "timestamp": "2024-01-15T10:30:00Z",
  "services": {
    "application": "Healthy",
    "database": "Healthy",
    "redis": "Healthy"
  }
}
```

### Prometheus Metrikleri
```bash
curl http://localhost:5245/metrics
```

**Mevcut Metrikler:**
- `http_requests_total` - Toplam HTTP istekleri
- `http_request_duration_seconds` - İstek süresi
- `cache_hits_total` - Cache hit sayısı
- `cache_misses_total` - Cache miss sayısı
- `database_query_duration_seconds` - Veritabanı sorgu süresi

### Cache Performans Loglama
Uygulama cache performansını loglar:
```
info: Cache HIT for key: FirmaLogoLinkleri:LinksByHisseAdi:THYAO
info: Cache MISS for key: FirmaLogoLinkleri:LinksByHisseAdi:THYAO
```

## 🐳 Docker Yapılandırması

### Servisler
- **companyservice**: Ana uygulama (port 5245, 7299)
- **sqlserver**: SQL Server veritabanı (port 1433)
- **redis**: Redis önbellek (port 6379)
- **prometheus**: Metrik toplama (port 9090)

### Ortam Değişkenleri
```yaml
ASPNETCORE_ENVIRONMENT: Production
ConnectionStrings__DefaultConnection: Server=sqlserver;Database=ekofin_backup;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;
ConnectionStrings__Redis: redis:6379
ApiKey: your-super-secret-api-key-2024
```

## 🔧 Yapılandırma

### appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "CompanyService.Services": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "your-connection-string",
    "Redis": "your-redis-connection"
  },
  "ApiKey": "your-api-key"
}
```

## 📈 Performans Özellikleri

### Redis Önbellekleme
- Veri değişikliklerinde otomatik cache invalidation
- Yapılandırılabilir cache TTL
- Cache hit/miss loglama ve metrikleri

### Veritabanı Optimizasyonu
- Optimize edilmiş sorgular ile Entity Framework Core
- Bağlantı havuzlama
- Sorgu performans izleme

### API Performansı
- İstek süresi takibi
- Yanıt süresi metrikleri
- Hata oranı izleme

## 🚨 Sorun Giderme

### Yaygın Sorunlar

1. **Redis Bağlantısı Başarısız**
   - Redis servisinin çalıştığını kontrol edin
   - appsettings.json'daki bağlantı dizisini doğrulayın
   - Ağ bağlantısını kontrol edin

2. **Veritabanı Bağlantısı Başarısız**
   - SQL Server'ın çalıştığını doğrulayın
   - Bağlantı dizisini kontrol edin
   - Veritabanının var olduğundan emin olun

3. **API Key Kimlik Doğrulama Başarısız**
   - X-API-Key header'ının mevcut olduğunu doğrulayın
   - API key'in yapılandırma ile eşleştiğini kontrol edin
   - Header adının doğru olduğundan emin olun (büyük/küçük harf duyarlı)

### Loglar
Uygulama logları şu yerlerde mevcuttur:
- Konsol çıktısı (geliştirme)
- Docker logları: `docker-compose logs companyservice`
- Korelasyon ID'leri ile yapılandırılmış loglama

## 🔒 Güvenlik Hususları

- API key güvenli bir şekilde saklanmalıdır (production'da ortam değişkenleri)
- Production ortamlarında HTTPS kullanın
- API key'leri düzenli olarak değiştirin
- Metrikler aracılığıyla şüpheli aktiviteleri izleyin
- Production kullanımı için rate limiting uygulayın

## 📝 Geliştirme

### Proje Yapısı
```
CompanyService/
├── Controller/          # API Controllers
├── Data/               # Veritabanı context ve migration'lar
├── DTO/                # Veri Transfer Nesneleri
├── Middleware/         # Özel middleware
├── Model/              # Entity modelleri
├── Services/           # İş mantığı servisleri
├── Migrations/         # EF Core migration'ları
└── Properties/         # Uygulama özellikleri
```

### Yeni Özellik Ekleme
1. `Model/` dizininde model oluşturun
2. `DTO/` dizininde DTO ekleyin
3. Servis arayüzü ve uygulaması oluşturun
4. Uygun kimlik doğrulama ile controller ekleyin
5. Gerekirse sağlık kontrollerini güncelleyin
6. İzleme için metrikler ekleyin

