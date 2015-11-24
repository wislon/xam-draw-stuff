using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DrawAvatars01
{
    public partial class ImageButtonPage : ContentPage
    {
        private readonly string _dataPath;

        public ImageButtonPage(string dataPath)
        {
            InitializeComponent();

            _dataPath = dataPath;
        }
    }
}
