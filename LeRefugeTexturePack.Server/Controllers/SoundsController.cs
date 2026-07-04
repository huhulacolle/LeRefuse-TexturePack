using LeRefugeTexturePack.Server.Interfaces.Services;
using LeRefugeTexturePack.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeRefugeTexturePack.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoundsController(ILogger<SoundsController> logger, ITexturesService texturesService) : ControllerBase
    {
        private readonly ITexturesService _texturesService = texturesService;
        private readonly ILogger<SoundsController> _logger = logger;

        [HttpGet]
        [ProducesResponseType(typeof(List<FileModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<FileModel>> GetAll()
        {
            try
            {
                var painting = _texturesService.GetAllSounds();
                return Ok(painting);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"une erreur inconnu est survenu {e.Message}");
            }
        }

        [HttpPost("{nameSound}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadSound(IFormFile file, string nameSound)
        {
            try
            {
                var result = await _texturesService.UploadSound(file, nameSound);
                if (result.IsFailed)
                {
                    return BadRequest(result.Errors[0].Message);
                }
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError("erreur UploadSound() : {e}", e.Message);
                return StatusCode(500, $"Erreur inconnue");
            }
        }
    }
}
