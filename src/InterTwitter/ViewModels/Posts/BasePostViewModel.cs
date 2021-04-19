using InterTwitter.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterTwitter.ViewModels.Posts
{
    public abstract class BasePostViewModel : BindableBase
    {
        #region -- Public properties --

        private Post _postModel;
        public Post PostModel
        {
            get => _postModel;
            set => SetProperty(ref _postModel, value, nameof(PostModel));
        }

        #endregion
    }
}
