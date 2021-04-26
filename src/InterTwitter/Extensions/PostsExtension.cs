using InterTwitter.Enums;
using InterTwitter.Models;
using InterTwitter.Services;
using InterTwitter.ViewModels.Posts;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace InterTwitter.Extensions
{
    public static class PostsExtension
    {
        #region -- Public methods --

        public static BasePostViewModel ToViewModel(this Post postModel, IPostService postManager, int currentUserId, INavigationService navigationService)
        { 
            BasePostViewModel postViewModel = null;

            if (postModel != null)
            {
                var userModel = postManager.GetPostAuthorAsync(postModel.UserId).Result;
                
                switch (postModel.MediaType)
                {
                    case EMediaType.Photo:
                        postViewModel = new PhotoPostViewModel(userModel, postModel, postManager, navigationService);
                        break;

                    case EMediaType.Gallery:
                        postViewModel = new GalleryPostViewModel(userModel, postModel, postManager, navigationService);
                        break;

                    case EMediaType.Video:
                        postViewModel = new VideoPostViewModel(userModel, postModel, postManager, navigationService);
                        break;
                }

                postViewModel.IsLiked = postModel.LikedUserIds.Contains(currentUserId);
                postViewModel.IsBookmarked = postModel.BookmarkedUserIds.Contains(currentUserId);
            }

            return postViewModel;
        }

        public static IEnumerable<BasePostViewModel> ToViewModelCollection(this IEnumerable<Post> postCollection, IPostService postManager, int currentUserId, INavigationService navigationService)
        {
            List<BasePostViewModel> viewModelCollection = null;

            if (postCollection.Any())
            {
                viewModelCollection = new List<BasePostViewModel>();
                postCollection.ToList().ForEach(p => viewModelCollection.Add(p.ToViewModel(postManager, currentUserId, navigationService)));
            }

            return viewModelCollection;
        }

        #endregion
    }
}
