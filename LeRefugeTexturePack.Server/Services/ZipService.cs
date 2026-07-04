using LeRefugeTexturePack.Server.Interfaces.Services;
using System.IO.Compression;

namespace LeRefugeTexturePack.Server.Services
{
    public class ZipService(ILogger<ZipService> logger) : IZipService
    {
        private readonly ILogger<ZipService> _logger = logger;


        private readonly string _zipPath = Path.Combine(Directory.GetCurrentDirectory(), "TexturePack", "Zip", "resetpack.zip");
        private readonly string _unzipPath = Path.Combine(Directory.GetCurrentDirectory(), "TexturePack", "Unzip", "resetpack");

        public async Task ZipTexturePack()
        {
            _logger.LogInformation("Début de la génération de l'archive du texture pack");
            _logger.LogInformation("Création du fichier Zip basé sur le path {_unzipPath}", _unzipPath);

            if (!Directory.Exists(_unzipPath))
                throw new Exception($"Le dossier {_unzipPath} n'existe pas");

            if (File.Exists(_zipPath))
                File.Delete(_zipPath);

            await ZipFile.CreateFromDirectoryAsync(_unzipPath, _zipPath);

            _logger.LogInformation("Zip du texture pack générée avec succès 👌");
        }
    }
}
