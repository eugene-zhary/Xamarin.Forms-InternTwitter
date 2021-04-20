using InterTwitter.Enums;
using InterTwitter.Models;
using InterTwitter.Services;
using InterTwitter.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity;

namespace InterTwitter.Extensions
{
    public static class PostsExtension
    {
        #region -- Public methods --

        public static BasePostViewModel ToViewModel(this Post postModel, IMockManager mock)
        {
            BasePostViewModel postViewModel = null;

            if (postModel != null)
            {
                User userModel = mock.GetMockedUsers(user => user.Id == postModel.UserId).FirstOrDefault();
                switch (postModel.MediaType)
                {
                    case EMediaType.Photo:
                        postViewModel = new PhotoPostViewModel(userModel, postModel);
                        break;
                }
            }

            return postViewModel;
        }

        public static IEnumerable<BasePostViewModel> ToViewModelCollection(this IEnumerable<Post> postCollection, IMockManager mock)
        {
            List<BasePostViewModel> viewModelCollection = null;

            if (postCollection.Any())
            {
                viewModelCollection = new List<BasePostViewModel>();
                postCollection.ToList().ForEach(p => viewModelCollection.Add(p.ToViewModel(mock)));
            }

            return viewModelCollection;
        }

        #endregion
    }
}
