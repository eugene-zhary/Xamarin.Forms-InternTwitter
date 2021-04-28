using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using InterTwitter.ViewModels.Posts;
using InterTwitter.Views.Flyout;
using Prism.Commands;
using Prism.Mvvm;
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
    public class SearchPostsViewModel : BaseViewModel
    {
        private readonly IPostService _postService;
        private string Tag;
        public SearchPostsViewModel(INavigationService navigationService, IPostService postService) : base(navigationService)
        {
            _postService = postService;
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(nameof(Tag), out Tag))
            {
                await UpdateCollecitonAsync();
            }
        }
        private ObservableCollection<BasePostViewModel> _searcedPost = new ObservableCollection<BasePostViewModel>();
        public ObservableCollection<BasePostViewModel> SearchedPosts 
        {
            get => _searcedPost;
            set => SetProperty(ref _searcedPost, value);
        }

        public ICommand NavigationToSearchCommand => new Command(OnNavigationToSearchCommand);

        private async void OnNavigationToSearchCommand(object obj)
        {
            await NavigationService.NavigateAsync($"/{nameof(FlyoutNavigationView)}");
        }

        private async Task<AOResult> UpdateCollecitonAsync()
        {
            var result = new AOResult();

            try
            {
                SearchedPosts.Clear();

                var posts = await _postService.GetPostsAsync();

                posts.Result.Where(u => u.PostModel.Text.Contains(Tag)).ToList().ForEach(SearchedPosts.Add);

                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(UpdateCollecitonAsync)}", "Something went wrong", ex);
            }

            return result;
        }
    }
}
