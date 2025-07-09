using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyService.Model;

namespace CompanyService.Services
{
public interface IAraciKurumService
{
    Task<List<AraciKurum>> GetAllAsync();
    }
}