using InterTwitter.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace InterTwitter.Controls
{
    public class FloationActionButton : Button
    {

        #region -- Public properties --

        public static readonly BindableProperty ScrollStateProperty
            = BindableProperty.Create(nameof(ScrollState), typeof(EScrollState), typeof(CustomCollectionView), propertyChanged: OnScrollStateChanged, defaultBindingMode: BindingMode.TwoWay);

        public EScrollState ScrollState
        {
            get => (EScrollState)GetValue(ScrollStateProperty);
            set => SetValue(ScrollStateProperty, value);
        }

        #endregion

        #region -- Private helpers --

        private static void OnScrollStateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var button = bindable as FloationActionButton;
            var oldState = (EScrollState)oldValue;
            var newState = (EScrollState)newValue;

            if(button!=null && oldState != newState)
            {
                if(newState == EScrollState.ScrollUp)
                {
                    button.TranslateTo(0, 100);
                }
                else
                {
                    button.TranslateTo(0, 0);
                }
            }
        }

        #endregion
    }
}
