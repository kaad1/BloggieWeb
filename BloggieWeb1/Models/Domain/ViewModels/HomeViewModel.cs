namespace BloggieWeb1.Models.Domain.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<BlogPost> BlogpPosts { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
