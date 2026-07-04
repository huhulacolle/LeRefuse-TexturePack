using LeRefugeTexturePack.Server.Interfaces.Services;
using LeRefugeTexturePack.Server.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LeRefugeTexturePack.Server.Controllers
{
    /// <summary>
    /// Endpoints pour télécharger les ressources du texture pack (zip, images, sons).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(NoCacheFilter))]
    public class DownloadController(IDownloadService downloadService) : ControllerBase
    {
        private readonly IDownloadService _downloadService = downloadService;

        /// <summary>
        /// Télécharge le texture pack en Zip pour le serveur
        /// </summary>
        /// <returns>Fichier ZIP à télécharger.</returns>
        /// <response code="200">Retourne le texture pack en ZIP.</response>
        /// <response code="500">Wtf le zip n'existe pas ???</response>
        [HttpGet("zip")]
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
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Pour afficher l'image de la peinture sur le site 
        /// </summary>
        /// <param name="filename">Nom du fichier image (ex: 'humble.png').</param>
        /// <returns>Fichier image PNG.</returns>
        /// <response code="200">Retourne la peinture</response>
        /// <response code="400">ta mis un nom bidon</response>

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

        /// <summary>
        /// Pour mettre la musique sur le site 
        /// </summary>
        /// <param name="filename">Nom du fichier son (ex: 'chirp.ogg').</param>
        /// <returns>Fichier image PNG.</returns>
        /// <response code="200">Retourne le son </response>
        /// <response code="400">ta mis un nom bidon</response>
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
