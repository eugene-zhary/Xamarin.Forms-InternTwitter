using Prism.Mvvm;
using System;
using Xamarin.Forms;

namespace InterTwitter.ViewModels.Flyout
{
    public class MenuItemViewModel : BindableBase
    {
        #region -- Public properties --

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value, nameof(Title));
        }

        private string _iconSource;
        public string IconSource
        {
            get => _iconSource;
            set => SetProperty(ref _iconSource, value, nameof(IconSource));
        }

        private Color _textColor;
        public Color TextColor
        {
            get => _textColor;
            set => SetProperty(ref _textColor, value, nameof(TextColor));
        }

        private Type _targetType;
        public Type TargetType
        {
            get => _targetType;
            set => SetProperty(ref _targetType, value, nameof(TargetType));
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value, nameof(IsSelected));
        }

        #endregion
    }
}
