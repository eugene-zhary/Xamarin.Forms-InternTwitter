﻿using InterTwitter.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InterTwitter.Views.Flyout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutTabbedView : CustomTabbedPage
    {
        public FlyoutTabbedView()
        {

            InitializeComponent();
        }
    }
}