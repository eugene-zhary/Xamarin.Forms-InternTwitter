using System;
using System.Collections.Generic;
using System.Text;

namespace InterTwitter.Helpers
{
    public interface IViewActionsHandler
    {
        void OnAppearing();
        void OnDisappearing();
    }
}
