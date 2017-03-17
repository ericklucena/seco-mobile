using SecoMobile.Models;
using SecoMobile.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SecoMobile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewManhole : Page
    {
        private Manhole manhole;

        public ViewManhole()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            manhole = e.Parameter as Manhole;
            FillInfo();
        }

        private void FillInfo()
        {
            if (manhole != null)
            {
                TextName.Text = manhole.Name;
                TextAddress.Text = manhole.Street;
                TextCurrentVolume.Text = manhole.FillRatio * 100 + " %";
                TextCurrentHeight.Text = string.Format("{0:0.00} cm", manhole.CurrentHeight);
                TextDimensions.Text = manhole.Dimensions.ToString();

                var date = manhole.LastManteinance.Date;
                TextManteinance.Text = string.Format("{0:00}/{1:00}/{2:00}", date.Day, date.Month, date.Year);

                date = manhole.LastUpdated.Date;
                var time = manhole.LastUpdated.TimeOfDay;
                TextUpdated.Text = string.Format("{0:00}/{1:00}/{2:00}", date.Day, date.Month, date.Year);
                TextUpdatedTime.Text = string.Format("{0:00}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds);

                switch (manhole.GasState)
                {
                    case EImportanceState.Normal:
                        TextStatus.Text = "Normal";
                        break;
                    case EImportanceState.Alert:
                        TextStatus.Text = "Em alerta";
                        break;
                    case EImportanceState.Critical:
                        TextStatus.Text = "Estado Crítico";
                        break;
                    default:
                        break;
                }
            }
        }

        private async void ManteinanceButton_Click(object sender, RoutedEventArgs e)
        {
            if (manhole != null)
            {
                TextManteinance.Text = "Enviando ao servidor...";
                await Network.RegisterMaintenance(manhole.Id);
                manhole.LastManteinance = DateTime.UtcNow;
                FillInfo();
            }
        }
    }
}
