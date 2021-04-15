using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace InterTwitter.Controls
{
    public class CustomTabbedPage : TabbedPage
    {

        #region -- Public properties --

        public static readonly BindableProperty SelectedTabTypeProperty = BindableProperty.Create(
            propertyName: nameof(SelectedTabType),
            returnType: typeof(Type),
            declaringType: typeof(CustomTabbedPage),
            defaultBindingMode: BindingMode.TwoWay);

        public Type SelectedTabType
        {
            get => (Type)GetValue(SelectedTabTypeProperty);
            set => SetValue(SelectedTabTypeProperty, value);
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            switch(propertyName)
            {
                case nameof(CurrentPage):
                    SelectedTabType = CurrentPage?.GetType();
                    break;
                    
            }
        }
        

        #endregion
    }
}
