using LeRefugeTexturePack.Server.Interfaces.Services;
using LeRefugeTexturePack.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeRefugeTexturePack.Server.Controllers
{
    /// <summary>
    /// Endpoints pour gérer les peintures du texture pack.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SoundsController(ILogger<SoundsController> logger, ITexturesService texturesService) : ControllerBase
    {
        private readonly ITexturesService _texturesService = texturesService;
        private readonly ILogger<SoundsController> _logger = logger;

        /// <summary>
        /// Récupère toutes les musiques disponibles.
        /// </summary>
        /// <returns>Liste des fichiers représentant les peintures.</returns>
        /// <response code="200">Retourne un tableau avec toutes les liens pour accéder aux peintures</response>
        /// <response code="500">Erreur serveur si ça t'arrive tu m'appelle</response>
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
                return StatusCode(500, $"une erreur un peu chelou est survenu {e.Message}");
            }
        }

        /// <summary>
        /// Upload la musique vers le texture pack (n'importe qu'elle fichier son, la conversion en ogg se fait automatiquement).
        /// </summary>
        /// <param name="file">Fichier image envoyé en multipart/form-data.</param>
        /// <param name="namePaiting">Nom souhaité pour la peinture (ex: 'chirp.ogg').</param>
        /// <response code="200">Upload réussi.</response>
        /// <response code="400">Paramètres invalides ou validation échouée.</response>
        /// <response code="500">Erreur serveur lors de l'upload.</response>
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
                return StatusCode(500, $"Erreur chelou");
            }
        }
    }
}
