using InterTwitter.Models;
using InterTwitter.Services;

namespace InterTwitter.ViewModels.Posts
{
    public class GalleryPostViewModel : BasePostViewModel
    {
        public GalleryPostViewModel(User userModel, Post postModel, IPostService postManager) : base(userModel, postModel, postManager)
        {

        }
    }
}
