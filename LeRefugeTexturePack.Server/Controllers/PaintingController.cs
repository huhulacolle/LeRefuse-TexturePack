using LeRefugeTexturePack.Server.Interfaces.Services;
using LeRefugeTexturePack.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeRefugeTexturePack.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaintingController(ILogger<PaintingController> logger, ITexturesService texturesService) : ControllerBase
    {
        private readonly ITexturesService _texturesService = texturesService;
        private readonly ILogger<PaintingController> _logger = logger;

        [HttpGet]
        [ProducesResponseType(typeof(List<FileModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<FileModel>> GetAllPainting()
        {
            try
            {
                var painting = _texturesService.GetAllPainting();
                return Ok(painting);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"une erreur inconnu est survenu {e.Message}");
            }
        }

        [HttpPost("{namePaiting}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadPaint(IFormFile file, string namePaiting)
        {
            try
            {
                var result = await _texturesService.UploadPaint(file, namePaiting);
                if (result.IsFailed)
                {
                    return BadRequest(result.Errors[0].Message);
                }
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError("erreur UploadPaint() : {e}", e.Message);
                return StatusCode(500, $"Erreur inconnue" );
            }
        }
    }
}
