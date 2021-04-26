using InterTwitter.Models;
using InterTwitter.Services;
using Prism.Navigation;

namespace InterTwitter.ViewModels.Posts
{
    public class GalleryPostViewModel : BasePostViewModel
    {
        public GalleryPostViewModel(User userModel, Post postModel, IPostService postManager, INavigationService navigationService) : base(userModel, postModel, postManager, navigationService)
        {

        }
    }
}
