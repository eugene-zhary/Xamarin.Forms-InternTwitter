using InterTwitter.Services.UserService;
using InterTwitter.ViewModels;
using InterTwitter.ViewModels.Flyout;
using InterTwitter.ViewModels.Navigation;
using InterTwitter.Views;
using InterTwitter.Views.Flyout;
using InterTwitter.Views.Navigation;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;

namespace InterTwitter
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
        }

        #region -- Overrides --

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignUpStartPage, SignUpStartPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpEndPage, SignUpEndPageViewModel>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInPageViewModel>();
            containerRegistry.RegisterForNavigation<FlyoutMenuView, FlyoutMenuViewModel>();
            containerRegistry.RegisterForNavigation<FlyoutTabbedView, FlyoutTabbedViewMode>();
            containerRegistry.RegisterForNavigation<FlyoutNavigationView, FlyoutNavigationViewModel>();
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>();
            containerRegistry.RegisterForNavigation<SearchView, SearchViewModel>();
            containerRegistry.RegisterForNavigation<NotifycationView, NotifycationViewModel>();
            containerRegistry.RegisterForNavigation<BookmarksView, BookmarksViewModel>();
            containerRegistry.RegisterForNavigation<ProfileView, ProfileViewModel>();
            containerRegistry.RegisterForNavigation<ChangeProfileView, ChangeProfileViewModel>();

            // Services
            containerRegistry.RegisterSingleton<IUserService, UserService>();
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync($"{nameof(FlyoutNavigationView)}");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #endregion
    }
}
