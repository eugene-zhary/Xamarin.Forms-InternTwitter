using InterTwitter.Enums;
using InterTwitter.Helpers;
using InterTwitter.ViewModels.Posts;
using InterTwitter.Views.Navigation;
using InterTwitter.Views.PostPage;
using Prism.Events;
using Prism.Navigation;
using System;

namespace InterTwitter.ViewModels.Flyout
{
    public class FlyoutNavigationViewModel : BaseViewModel
    {
        public FlyoutNavigationViewModel(INavigationService navigationService, IEventAggregator aggregator) : base(navigationService)
        {
            aggregator.GetEvent<MenuVisibilityChangedEvent>().Subscribe(OnMenuVisibilityChanged);
            aggregator.GetEvent<NavigationEvent>().Subscribe(OnNavigation);
        }

        #region -- Public properties --

        private bool _isMenuVisible;
        public bool IsMenuVisible
        {
            get => _isMenuVisible;
            set => SetProperty(ref _isMenuVisible, value, nameof(IsMenuVisible));
        }

        #endregion

        #region -- Private helpers --

        private void OnMenuVisibilityChanged(bool parameter)
        {
            IsMenuVisible = parameter;
        }

        private async void OnNavigation(BasePostViewModel arg)
        {
            var paramenters = new NavigationParameters
            {
                { nameof(BasePostViewModel), arg }
            };

            switch (arg.PostModel.MediaType)
            {
                case EMediaType.Gallery:
                    await NavigationService.NavigateAsync($"{nameof(GalleryPostPage)}", paramenters);
                    break;

                case EMediaType.Photo:
                    await NavigationService.NavigateAsync($"{nameof(PhotoPostPage)}", paramenters);
                    break;

                case EMediaType.Gif:
                    await NavigationService.NavigateAsync($"{nameof(GifPostPage)}", paramenters);
                    break;

                case EMediaType.Video:
                    await NavigationService.NavigateAsync($"{nameof(VideoPostPage)}", paramenters);
                    break;
            }
        }

        #endregion
    }
}
