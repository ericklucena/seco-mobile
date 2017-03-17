using Newtonsoft.Json;
using SecoMobile.Models;
using SecoMobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace SecoMobile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public ObservableCollection<UserLocation> CurrentCoordinate { get; set; }
        private List<SecoSensor> _secoSensors;
        private List<Manhole> _manholes;

        private DispatcherTimer updateTimer;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            CurrentCoordinate = new ObservableCollection<UserLocation>();

            updateTimer = new DispatcherTimer();
            updateTimer.Tick += predictionsTimer_Tick;
            updateTimer.Interval = new TimeSpan(0, 0, 15);
            updateTimer.Start();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            CenterMap();
            CollectData();

        }

        private void predictionsTimer_Tick(object sender, object e)
        {
            CollectData();
        }

        private async void InsertManholeButton_Click(object sender, RoutedEventArgs e)
        {
            InsertManholeDialog dialog = new InsertManholeDialog();
            dialog.CurrentLocation = CurrentCoordinate?.First();
            await dialog.ShowAsync();
        }

        private void _RefreshMap()
        {

        }
        private async void CollectData()
        {
            try
            {
                string currentJsonData = await Network.SecoData();
                _secoSensors = _ParseSecoSensors(currentJsonData);

                _manholes = _secoSensors?.Select(m => m.CreateManhole()).ToList();

                mapManholes.ItemsSource = _manholes;
            }
            catch
            {

            }
        }

        private List<SecoSensor> _ParseSecoSensors(string json)
        {
            return JsonConvert.DeserializeObject<List<SecoSensor>>(json);
        }

        private async void CenterMap()
        {
            string errorMessage = null;

            try
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    Geolocator geolocator = new Geolocator();
                    Geoposition geoposition = await geolocator.GetGeopositionAsync(
                        maximumAge: TimeSpan.FromMinutes(5),
                        timeout: TimeSpan.FromSeconds(10)
                        );
                    CurrentCoordinate.Clear();
                    CurrentCoordinate.Add(new UserLocation(geoposition.Coordinate));

                    await MapControl.TrySetViewAsync(geoposition.Coordinate.Point, 16, 0, 0, Windows.UI.Xaml.Controls.Maps.MapAnimationKind.Bow);
                    //mapControl.Center = geoposition.Coordinate.Point;
                    //mapControl.ZoomLevel = 16;
                });
                mapUsers.ItemsSource = CurrentCoordinate;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            if (errorMessage != null)
            {
                MessageDialog msg = new MessageDialog("Não foi possível localizar o seu dispositivo. Por favor verifique se o sistema de localização de seu telefone está ativado para uma melhor experiência de uso.", "Seu GPS está ativado?");
                await msg.ShowAsync();
            }
        }

        private void manholeIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ViewManhole), (sender as Image).Tag);
        }
    }
}
