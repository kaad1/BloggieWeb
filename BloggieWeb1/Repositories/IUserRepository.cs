using Microsoft.AspNetCore.Identity;

namespace BloggieWeb1.Repositories
{
    public interface IUserRepository
    {
       Task<IEnumerable<IdentityUser>> GetAll();
    }
}
