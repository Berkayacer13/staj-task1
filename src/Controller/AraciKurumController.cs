using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanyService.Data;
using CompanyService.Model;
using CompanyService.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AraciKurumController : ControllerBase
    {
        private readonly IAraciKurumService _service;

        public AraciKurumController(IAraciKurumService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<AraciKurum>>> GetAll()
        {
            var araciKurumlar = await _service.GetAllAsync();
            return Ok(araciKurumlar);
        }
    }
} 