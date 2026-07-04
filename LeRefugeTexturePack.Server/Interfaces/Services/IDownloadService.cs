using FluentResults;

namespace LeRefugeTexturePack.Server.Interfaces.Services
{
    public interface IDownloadService
    {
        string DownloadTextureZip();
        string DownloadImage(string filename);
        string DownloadSound(string filename);
    }
}
