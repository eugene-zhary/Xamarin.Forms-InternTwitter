using InterTwitter.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using InterTwitter.Views.Navigation;

namespace InterTwitter.ViewModels.Navigation
{
    public class SearchViewModel : BaseTabViewModel
    {
        private readonly IPostService _postService;
        private readonly IMockService _mockService;

        public SearchViewModel(INavigationService navigation, IPostService postService, IMockService mockService) : base(navigation)
        {
            IconPath = "ic_search_gray.png";
            _postService = postService;
            _mockService = mockService;
        }

        #region -- Overrides --

        public override void OnAppearing()
        {
            IconPath = "ic_search_blue.png";
        }

        public override void OnDisappearing()
        {
            IconPath = "ic_search_gray.png";
        }

        public ICommand TextChangedCommand => new Command<object>(OnTextChangedCommand);
        public ICommand NavigationToSearchCommand => new Command<PopularThemes>(OnNavigationToSearchCommand);

        private void OnNavigationToSearchCommand(PopularThemes popularThemes)
        {
            NavigationParameters keys = new NavigationParameters
            {
                { nameof(popularThemes.Tag), popularThemes.Tag }
            };

            NavigationService.NavigateAsync($"/{nameof(SearchPostsView)}", keys);
        }

        private void OnTextChangedCommand(object obj)
        {
            Console.WriteLine(obj);
        }

        private ObservableCollection<PopularThemes> _items;
        public ObservableCollection<PopularThemes> Items 
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Items = new ObservableCollection<PopularThemes>();

            var Posts = await _postService.GetPostsAsync();


            Posts.Result.ToList()
                .ForEach(u => u.PostModel.Text.Split(' ')
                .Where(u => (u.Contains('#') && !Items.Where(v => v.Tag == u).Any())).ToList()
                .ForEach(u => Items.Add(new PopularThemes { Tag = u.Replace(",", ""), Count = Posts.Result.Count(v => v.PostModel.Text.Contains(u)) })));

            Items = new ObservableCollection<PopularThemes>(Items.OrderByDescending(u => u.Count));
        }

        #endregion
    }

    public class PopularThemes
    {
        public string Tag { get; set; }
        public int Count { get; set; } = 0;
    }
}
