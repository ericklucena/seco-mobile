using SecoMobile.Models;
using SecoMobile.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SecoMobile
{
    public sealed partial class InsertManholeDialog : ContentDialog
    {
        private UserLocation _currentLocation;

        public UserLocation CurrentLocation
        {
            get
            {
                return _currentLocation;
            }

            set
            {
                _currentLocation = value;

                if (CurrentLocation != null)
                {
                    lat.Text = CurrentLocation.Geopoint.Position.Latitude.ToString();
                    lng.Text = CurrentLocation.Geopoint.Position.Longitude.ToString();
                }
            }
        }

        public InsertManholeDialog()
        {
            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            progressBar.Visibility = Visibility.Visible;
            string response = await Network.SecoInsertion(id.Text, name.Text, street.Text, lat.Text, lng.Text, dimX.Text, dimY.Text, dimZ.Text);
            progressBar.Visibility = Visibility.Collapsed;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
