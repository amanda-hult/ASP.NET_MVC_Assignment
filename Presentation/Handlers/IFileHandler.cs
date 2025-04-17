namespace Presentation.Handlers;

public interface IFileHandler
{
    Task<string> UploadFileAsync(IFormFile file);
}