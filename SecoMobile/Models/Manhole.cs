using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace SecoMobile.Models
{
    public class Dimension
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public override string ToString()
        {
            return string.Format("{0} x {1} x {2}", X, Y, Z);
        }
    }

    public enum EImportanceState
    {
        Normal,
        Alert,
        Critical
    }

    public class Manhole
    {
        private const double MAX_HEIGHT = 400.0f;
        private const double MIN_HEIGHT = 5.0f;

        private double _currentHeight;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double CurrentHeight
        {
            get
            {
                return _currentHeight;
            }
            set
            {
                if (value >= MAX_HEIGHT)
                    _currentHeight = MAX_HEIGHT;
                else if (value <= MIN_HEIGHT)
                    _currentHeight = MIN_HEIGHT;
                else
                    _currentHeight = value;
                LastUpdated = DateTime.Now;

            }
        }
        public double FillRatio
        {
            get
            {
                return (Dimensions.Z - _currentHeight) / Dimensions.Z;
            }
        }

        public Dimension Dimensions { get; set; }
        public EImportanceState GasState { get; set; }
        public DateTime LastManteinance { get; set; }
        public DateTime LastUpdated { get; set; }

        public Manhole Self { get { return this; } }

        public EImportanceState VolumeState
        {
            get
            {
                if (FillRatio <= 0.50f)
                    return EImportanceState.Normal;
                else if (FillRatio <= 0.80f)
                    return EImportanceState.Alert;
                else
                    return EImportanceState.Critical;
            }
        }

        public Geopoint Geopoint
        {
            get
            {
                BasicGeoposition bg = new BasicGeoposition();
                bg.Latitude = Latitude;
                bg.Longitude = Longitude;
                Geopoint g = new Geopoint(bg);
                return g;
            }
        }
    }
}
