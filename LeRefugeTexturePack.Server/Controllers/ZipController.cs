using LeRefugeTexturePack.Server.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeRefugeTexturePack.Server.Controllers
{
    /// <summary>
    /// Endpoints pour lancer la génération/archivage du texture pack en ZIP.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ZipController(IZipService zipService) : ControllerBase
    {
        private readonly IZipService _zipService = zipService;

        /// <summary>
        /// Lance la création d'un fichier ZIP contenant le texture pack.
        /// </summary>
        /// <remarks>
        /// Appel typique : GET /api/zip
        /// </remarks>
        /// <returns>200 si l'archivage s'est déroulé correctement.</returns>
        /// <response code="200">Archivage réussi.</response>
        /// <response code="500">Erreur serveur lors de la création de l'archive.</response>
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
