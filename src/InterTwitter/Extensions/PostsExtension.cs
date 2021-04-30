using InterTwitter.Enums;
using InterTwitter.Models;
using InterTwitter.ViewModels.Posts;

namespace InterTwitter.Extensions
{
    public static class PostsExtension
    {
        #region -- Public methods --

        public static BasePostViewModel ToViewModel(this Post postModel, User authorModel, int currentUserId)
        { 
            BasePostViewModel postViewModel = null;

            if (postModel != null)
            {
                switch(postModel.MediaType)
                {
                    case EMediaType.Photo:
                    case EMediaType.Gif:
                    case EMediaType.Video:
                        postViewModel = new OneMediaPostViewModel(authorModel, postModel);
                        break;

                    case EMediaType.Empty:
                    case EMediaType.Gallery:
                        postViewModel = new BasePostViewModel(authorModel, postModel);
                        break;
                }

                postViewModel.IsLiked = postModel.LikedUserIds.Contains(currentUserId);
                postViewModel.IsBookmarked = postModel.BookmarkedUserIds.Contains(currentUserId);
            }

            return postViewModel;
        }

        #endregion
    }
}
