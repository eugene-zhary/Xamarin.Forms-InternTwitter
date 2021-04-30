using InterTwitter.Extensions;
using InterTwitter.Helpers;
using Prism.Events;
using Prism.Navigation;
using System;
using System.ComponentModel;

namespace InterTwitter.ViewModels.Flyout
{
    public class DetailTabbedViewMode : BaseViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public DetailTabbedViewMode(INavigationService navigation, IEventAggregator aggregator) : base(navigation)
        {
            _eventAggregator = aggregator;
            aggregator.GetEvent<MenuItemChangedEvent>().Subscribe(OnMenuItemChanged);
        }

        #region -- Public properties --

        private Type _selectedTabType;
        public Type SelectedTabType
        {
            get => _selectedTabType;
            set => SetProperty(ref _selectedTabType, value, nameof(SelectedTabType));
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch(args.PropertyName)
            {
                case nameof(SelectedTabType):
                    _eventAggregator.GetEvent<MenuItemChangedEvent>().Publish(SelectedTabType);
                    break;
            }
        }

        #endregion

        #region -- Private helpers --

        private async void OnMenuItemChanged(Type parameter)
        {
            if(parameter != null && SelectedTabType != null && SelectedTabType != parameter)
            {
                await NavigationService.SelectTabFromFlyoutAsync(parameter.Name);
            }
        }

        #endregion
    }
}
