namespace BloggieWeb1.Repositories
{
    public interface IImageRepository
    {
        Task<string> UploadAsync(IFormFile file);
        
    }
}
