using FluentResults;
using LeRefugeTexturePack.Server.Models;

namespace LeRefugeTexturePack.Server.Interfaces.Services
{
    public interface ITexturesService
    {
        List<FileModel> GetAllPainting();
        List<FileModel> GetAllSounds();
        Task<Result> UploadPaint(IFormFile file, string namePainting);
        Task<Result> UploadSound(IFormFile file, string namePainting);
    }
}
