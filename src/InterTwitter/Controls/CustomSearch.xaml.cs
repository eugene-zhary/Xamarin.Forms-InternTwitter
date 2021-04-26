using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InterTwitter.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomSearch : Grid
    {
        public CustomSearch()
        {
            InitializeComponent();
        }

        public ICommand TextChangedCommand => new Command(OnTextChangedCommand);

        private void OnTextChangedCommand(object obj)
        {
            Console.WriteLine();
        }
    }
}