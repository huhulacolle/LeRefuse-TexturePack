using LeRefugeTexturePack.Server.Interfaces.Services;
using LeRefugeTexturePack.Server.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LeRefugeTexturePack.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(NoCacheFilter))]
    public class DownloadController(IDownloadService downloadService) : ControllerBase
    {
        private readonly IDownloadService _downloadService = downloadService;

        [HttpGet("zip")]
        [Produces("application/zip")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Zip()
        {
            try
            {
                var result = _downloadService.DownloadTextureZip();
                return PhysicalFile(result, "application/zip", "resetpack.zip");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("image/{filename}")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Image(string filename)
        {
            try
            {
                var result = _downloadService.DownloadImage(filename);

                return PhysicalFile(result, "image/png", filename);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }        
        }

        [HttpGet("sound/{filename}")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Sound(string filename)
        {
            try
            {
                var result = _downloadService.DownloadSound(filename);

                return PhysicalFile(result, "audio/ogg", filename);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
