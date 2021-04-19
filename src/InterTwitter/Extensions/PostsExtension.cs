using InterTwitter.Enums;
using InterTwitter.Models;
using InterTwitter.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterTwitter.Extensions
{
    public static class PostsExtension
    {
        #region -- Public methods --

        public static BasePostViewModel ToViewModel(this Post postModel)
        {
            BasePostViewModel postViewModel = null;

            if(postModel != null)
            {
                switch(postModel.MediaType)
                {
                    case EMediaType.Photo:
                        postViewModel = new PhotoPostViewModel(postModel);
                        break;
                }
            }

            return postViewModel;
        }

        public static IEnumerable<BasePostViewModel> ToViewModelCollection(this IEnumerable<Post> postCollection)
        {
            List<BasePostViewModel> viewModelCollection = null;

            if(postCollection.Any())
            {
                viewModelCollection = new List<BasePostViewModel>();
                postCollection.ToList().ForEach(p => viewModelCollection.Add(p.ToViewModel()));
            }

            return viewModelCollection;
        }

        #endregion
    }
}
