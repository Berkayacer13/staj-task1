// Controller/FiramLogoLinkleriController.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompanyService.DTO;
using CompanyService.Services;

namespace CompanyService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class FirmaLogoLinkleriController : ControllerBase
    {
        private readonly IFirmaLogoLinkleriService _service;

        public FirmaLogoLinkleriController(IFirmaLogoLinkleriService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get logo links by hisse adi
        /// </summary>
        /// <param name="hisseAdi">Hisse adi to search for</param>
        /// <returns>List of logo URLs</returns>
        [HttpGet("hisseadi/{hisseAdi}")]
        public async Task<ActionResult<List<string>>> GetLogoLinksByHisseAdi(string hisseAdi)
        {
            var links = await _service.GetLinksByHisseAdiAsync(hisseAdi);
            
            if (!links.Any())
            {
                return NotFound($"No logo links found for hisse adi: {hisseAdi}");
            }
            
            var urls = links.Select(x => x.Url).ToList();
            return Ok(urls);
        }

        /// <summary>
        /// Get logo links by firma ID
        /// </summary>
        /// <param name="firmaId">Firma ID to search for</param>
        /// <returns>List of logo URLs</returns>
        [HttpGet("firma/{firmaId}")]
        public async Task<ActionResult<List<string>>> GetLogoLinksByFirmaId(int firmaId)
        {
            var links = await _service.GetLinksByFirmaIdAsync(firmaId);
            
            if (!links.Any())
            {
                return NotFound($"No logo links found for firma ID: {firmaId}");
            }
            
            var urls = links.Select(x => x.Url).ToList();
            return Ok(urls);
        }

        /// <summary>
        /// Create a new logo link
        /// </summary>
        /// <param name="logoLinkDto">Logo link data</param>
        /// <returns>Created logo link</returns>
        [HttpPost]
        public async Task<ActionResult<LogoLinkDto>> CreateLogoLink([FromBody] LogoLinkDto logoLinkDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.CreateLogoLinkAsync(logoLinkDto);
                return CreatedAtAction(nameof(GetLogoLinksByHisseAdi), new { hisseAdi = result.HisseAdi }, result);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Error creating logo link: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing logo link
        /// </summary>
        /// <param name="firmaId">Firma ID</param>
        /// <param name="hisseAdi">Hisse adi</param>
        /// <param name="logoLinkDto">Updated logo link data</param>
        /// <returns>Updated logo link</returns>
        [HttpPut("{firmaId}/{hisseAdi}")]
        public async Task<ActionResult<LogoLinkDto>> UpdateLogoLink(int firmaId, string hisseAdi, [FromBody] LogoLinkDto logoLinkDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.UpdateLogoLinkAsync(firmaId, hisseAdi, logoLinkDto);
                return Ok(result);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Error updating logo link: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a logo link
        /// </summary>
        /// <param name="firmaId">Firma ID</param>
        /// <param name="hisseAdi">Hisse adi</param>
        /// <returns>Success status</returns>
        [HttpDelete("{firmaId}/{hisseAdi}")]
        public async Task<ActionResult> DeleteLogoLink(int firmaId, string hisseAdi)
        {
            var result = await _service.DeleteLogoLinkAsync(firmaId, hisseAdi);
            
            if (!result)
                return NotFound("Logo link not found");
                
            return NoContent();
        }
    }
}