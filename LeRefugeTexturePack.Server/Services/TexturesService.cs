using FFMpegCore;
using FluentResults;
using LeRefugeTexturePack.Server.Interfaces.Services;
using LeRefugeTexturePack.Server.Models;
using SixLabors.ImageSharp;

namespace LeRefugeTexturePack.Server.Services
{
    public class TexturesService(ILogger<TexturesService> logger) : ITexturesService
    {
        private readonly ILogger<TexturesService> _logger = logger;
        private readonly string _unzipPath = Path.Combine(Directory.GetCurrentDirectory(), "TexturePack", "Unzip", "resetpack");

        public List<FileModel> GetAllPainting()
        {
            if (!Directory.Exists(_unzipPath))
            {
                throw new Exception($"Le dossier {_unzipPath} n'existe pas");
            }

            var paintingsPath = Path.Combine(_unzipPath, "assets", "minecraft", "textures", "painting");
            var paintings = Directory
                .GetFiles(paintingsPath)
                .Select(p => new FileModel
                {
                    FileName = Path.GetFileName(p),
                    Url = $"/api/Download/image/{Path.GetFileName(p)}"
                })
                .ToList();

            return paintings;
        }

        public List<FileModel> GetAllSounds()
        {
            if (!Directory.Exists(_unzipPath))
            {
                throw new Exception($"Le dossier {_unzipPath} n'existe pas");
            }

            var paintingsPath = Path.Combine(_unzipPath, "assets", "minecraft", "sounds", "records");
            var paintings = Directory
                .GetFiles(paintingsPath)
                .Select(p => new FileModel
                {
                    FileName = Path.GetFileName(p),
                    Url = $"/api/Download/image/{Path.GetFileName(p)}"
                })
                .ToList();

            return paintings;
        }

        public async Task<Result> UploadPaint(IFormFile file, string namePainting)
        {
            if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Fail(">:(");
            }

            List<FileModel> allPainting = GetAllPainting();

            if (!allPainting.Any(a => a.FileName == namePainting))
            {
                return Result.Fail("Nom de fichier incorrect");
            }

            string path = Path.Combine(
                _unzipPath, 
                "assets", 
                "minecraft", 
                "textures",
                "painting", 
                $"{namePainting}"
                );

            _logger.LogInformation("Enregistrement du fichier dans {path}", path);

            using var image = await Image.LoadAsync(file.OpenReadStream());
            await image.SaveAsPngAsync(path);

            _logger.LogInformation("Enregistrement effectué avec succès");

            return Result.Ok();
        }

        public async Task<Result> UploadSound(IFormFile file, string nameSound)
        {
            if (!file.ContentType.StartsWith("audio/", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Fail(">:(");
            }

            List<FileModel> allSounds = GetAllSounds();

            if (!allSounds.Any(a => a.FileName == nameSound))
            {
                return Result.Fail("Nom de fichier incorrect");
            }

            string path = Path.Combine(
                _unzipPath,
                "assets",
                "minecraft",
                "sounds",
                "records",
                $"{nameSound}"
                );

            if (Path.GetExtension(file.FileName) == ".ogg")
            {
                _logger.LogInformation("Enregistrement du son ogg dans {path}", path);
                await using var stream = File.Create(path);
                await file.CopyToAsync(stream);
                return Result.Ok();
            }
            
            var pathTemp = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}");
            _logger.LogInformation("Enregistrement du fichier son temporaire dans {path}", pathTemp);

            await using (var stream = File.Create(pathTemp))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation("Conversion tu fichier {pathTemp} en {path}", pathTemp, path);

            await FFMpegArguments
                .FromFileInput(pathTemp)
                .OutputToFile(path, true, options => options
                    .WithAudioCodec("libvorbis"))
                .ProcessAsynchronously();

            _logger.LogInformation("Enregistrement effectué avec succès");

            _logger.LogInformation("Suppression du son temporaire {pathTemp}", pathTemp);
            File.Delete(pathTemp);
            
            return Result.Ok();
        }
    }
}
