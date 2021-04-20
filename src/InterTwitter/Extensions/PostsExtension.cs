using InterTwitter.Enums;
using InterTwitter.Models;
using InterTwitter.Services;
using InterTwitter.ViewModels.Posts;
using System.Collections.Generic;
using System.Linq;

namespace InterTwitter.Extensions
{
    public static class PostsExtension
    {
        #region -- Public methods --

        public static BasePostViewModel ToViewModel(this Post postModel, IPostManager postManager)
        {
            BasePostViewModel postViewModel = null;

            if(postModel != null)
            {
                User userModel = postManager.GetPostAuthor(postModel.UserId);

                switch(postModel.MediaType)
                {
                    case EMediaType.Photo:
                        postViewModel = new PhotoPostViewModel(userModel, postModel, postManager);
                        break;
                }
            }

            return postViewModel;
        }

        public static IEnumerable<BasePostViewModel> ToViewModelCollection(this IEnumerable<Post> postCollection, IPostManager postManager)
        {
            List<BasePostViewModel> viewModelCollection = null;

            if(postCollection.Any())
            {
                viewModelCollection = new List<BasePostViewModel>();
                postCollection.ToList().ForEach(p => viewModelCollection.Add(p.ToViewModel(postManager)));
            }

            return viewModelCollection;
        }

        #endregion
    }
}
