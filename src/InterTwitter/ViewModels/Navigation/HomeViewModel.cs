﻿using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using InterTwitter.ViewModels.Posts;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels.Navigation
{
    public class HomeViewModel : BaseTabViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IPostService _postManager;

        public HomeViewModel(INavigationService navigation, IEventAggregator eventAggregator, IPostService postManager) : base(navigation)
        {
            _eventAggregator = eventAggregator;
            _postManager = postManager;

            IconPath = "ic_home_gray.png";
            PostCollection = new ObservableCollection<BasePostViewModel>();
        }

        #region -- Public region --

        public ObservableCollection<BasePostViewModel> PostCollection { get; set; }

        private Thickness _Margin;
        public Thickness Margin
        {
            get => _Margin;
            set => SetProperty(ref _Margin, value);
        }

        public ICommand PicProfileTapGestureRecognizer => new Command(OnPicProfileTapGestureRecognizer);

        #endregion

        #region -- Overrides --

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            await UpdateCollecitonAsync();
        }

        public override void OnAppearing()
        {
            IconPath = "ic_home_blue.png";
        }

        public override void OnDisappearing()
        {
            IconPath = "ic_home_gray.png";
        }

        #endregion

        #region -- Private helpers --

        private async Task<AOResult> UpdateCollecitonAsync()
        {
            var result = new AOResult();

            try
            {
                PostCollection.Clear();

                var posts = await _postManager.GetPostsAsync();
                posts.Result.ToList().ForEach(PostCollection.Add);

                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(UpdateCollecitonAsync)}", "Something went wrong", ex);
            }

            return result;
        }
        private void OnPicProfileTapGestureRecognizer()
        {
            _eventAggregator.GetEvent<MenuVisibilityChangedEvent>().Publish(true);
        }

        #endregion
    }
}
