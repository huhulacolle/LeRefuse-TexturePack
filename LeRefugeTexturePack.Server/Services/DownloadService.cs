using LeRefugeTexturePack.Server.Interfaces.Services;

namespace LeRefugeTexturePack.Server.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly string _zipPath = Path.Combine(Directory.GetCurrentDirectory(), "TexturePack", "Zip", "resetpack.zip");
        private readonly string _unzipPath = Path.Combine(Directory.GetCurrentDirectory(), "TexturePack", "Unzip", "resetpack");


        public string DownloadTextureZip()
        {
            if (!File.Exists(_zipPath))
            {
                throw new Exception($"Le fichier zip n'existe pas");
            }

            return _zipPath;
        }

        public string DownloadImage(string filename)
        {
            var path = Path.Combine(_unzipPath, "assets", "minecraft", "textures", "painting", filename);
            if (!File.Exists(path))
            {
                throw new Exception($"L'image {filename} n'existe pas");
            }

            return path;
        }

        public string DownloadSound(string filename)
        {
            var path = Path.Combine(_unzipPath, "assets", "minecraft", "sounds", "records", filename);
            if (!File.Exists(path))
            {
                throw new Exception($"L'image {filename} n'existe pas");
            }

            return path;
        }

    }
}