using InterTwitter.Helpers;
using Prism.Events;
using Prism.Mvvm;

namespace InterTwitter.ViewModels.Flyout
{
    public class FlyoutNavigationViewModel : BindableBase
    {
        public FlyoutNavigationViewModel(IEventAggregator aggregator)
        {
            aggregator.GetEvent<MenuVisibilityChangedEvent>().Subscribe(OnMenuVisibilityChanged);
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

        #endregion
    }
}
