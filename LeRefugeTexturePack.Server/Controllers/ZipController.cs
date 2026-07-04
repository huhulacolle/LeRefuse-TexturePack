using LeRefugeTexturePack.Server.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeRefugeTexturePack.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipController(IZipService zipService) : ControllerBase
    {
        private readonly IZipService _zipService = zipService;

        [HttpGet] 
        public async Task<IActionResult> ZipTexturePack()
        {
            try
            {
                await _zipService.ZipTexturePack();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
