using DLToolkit.Forms.Controls;
using InterTwitter.Services;
using InterTwitter.Services.Authorization;
using InterTwitter.Services.Permission;
using InterTwitter.Services.Settings;
using InterTwitter.Services.UserService;
using InterTwitter.ViewModels;
using InterTwitter.ViewModels.Flyout;
using InterTwitter.ViewModels.Navigation;
using InterTwitter.Views;
using InterTwitter.Views.Flyout;
using InterTwitter.Views.Navigation;
using Plugin.Media;
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
            containerRegistry.RegisterInstance<IMockService>(Container.Resolve<MockService>());
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());

            containerRegistry.RegisterInstance<IPostService>(Container.Resolve<PostService>());

            containerRegistry.RegisterInstance<IPermissionManager>(Container.Resolve<PermissionManager>());

            // Services
            containerRegistry.RegisterSingleton<IUserService, UserService>();
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());

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
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterForNavigation<SearchPostsView, SearchPostsViewModel>();
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            FlowListView.Init();


            IAuthorizationService authorizationService = Container.Resolve<AuthorizationService>();

            authorizationService.Authorize(2);

            await NavigationService.NavigateAsync($"/{nameof(FlyoutNavigationView)}");
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
