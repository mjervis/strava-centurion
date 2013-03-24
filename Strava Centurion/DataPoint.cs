// -----------------------------------------------------------------------
// <copyright file="DataPoint.cs" company="FuckingBrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// -----------------------------------------------------------------------

namespace Strava_Centurion
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represent a track point in a TCX file and methods for accessing it.
    /// </summary>
    public class DataPoint
    {
        /// <summary>
        /// The node used to access the underlying storage.
        /// </summary>
        private readonly INode node;

        /// <summary>
        /// The power in watts.
        /// </summary>
        private double powerInWatts;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPoint"/> class. 
        /// Create a track point from an XML track point node.
        /// </summary>
        /// <param name="node">
        /// A data node used to access the underlying persisted node.
        /// </param>
        public DataPoint(INode node)
        {
            this.node = node;

            this.DateTime = node.GetDateTime();
            this.Altitude = new Distance(node.GetAltitude());
            this.CadenceInRpm = node.GetCadence();
            this.TotalDistance = new Distance(node.GetTotalDistance());
            this.SpeedInKmPerHour = node.GetSpeed();
            this.HeartrateInBpm = node.GetHeartrate();
            this.Latitude = new Angle { Degrees = node.GetLatitude() };
            this.Longitude = new Angle { Degrees = node.GetLongitude() };
            this.PowerInWatts = node.GetPower();
        }

        #region Properties.    

        /// <summary>
        /// Gets the altitude in meters
        /// </summary>
        public Distance Altitude { get; private set; }

        /// <summary>
        /// Gets the heart rate in beats per minute.
        /// </summary>
        public string HeartrateInBpm { get; private set; }

        /// <summary>
        /// Gets the cadence in revolutions per minute.
        /// </summary>
        public string CadenceInRpm { get; private set; }

        /// <summary>
        /// Gets or sets the power in watts.
        /// </summary>
        public double PowerInWatts
        {
            get
            {
                return this.powerInWatts;
            }

            set
            {
                this.powerInWatts = value;
                this.node.SetPower(this.powerInWatts);
            }
        }

        /// <summary>
        /// Gets the speed in kilometers per hour.
        /// </summary>
        public double SpeedInKmPerHour { get; private set; }

        /// <summary>
        /// Gets the distance in meters.
        /// </summary>
        public Distance TotalDistance { get; private set; }

        /// <summary>
        /// Gets or sets the date and time that the point was recorded
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the longitude in degrees.
        /// </summary>
        public Angle Longitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude in degrees.
        /// </summary>
        public Angle Latitude { get; set; }

        #endregion

        /// <summary>
        /// Fetch or calculate the distance between this point and another.
        /// </summary>
        /// <param name="other">
        /// The other point.
        /// </param>
        /// <returns>
        /// The distance in meters between the two points.
        /// </returns>
        public Distance DistanceToPoint(DataPoint other)
        {
            Distance result;

            if (double.IsNaN(this.TotalDistance) || double.IsNaN(other.TotalDistance))
            {
                var ascent = this.AscentInMetresToPoint(other);
                var haversine = this.HaversineDistanceInMetresToPoint(other);

                result = new Distance(Math.Sqrt((ascent * ascent) + (haversine * haversine)));
            }
            else
            {
                result = other.TotalDistance - this.TotalDistance;
            }

            return result;
        }

        /// <summary>
        /// Calculates the ascent from this point to the point specified.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>The ascent in meters.</returns>
        public Distance AscentInMetresToPoint(DataPoint other)
        {
            return other.Altitude - this.Altitude;
        }

        /// <summary>
        /// Calculate the gradient from this point to another point as the ratio of the
        /// distance in m climbed over the distance in m travelled.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>Ratio of ascent to distance.</returns>
        public double GradientToPoint(DataPoint other)
        {
            /*
             * Trig:
             *  h = sqrt(x^2 + y^2) where x and y are the lengths of the other two sides.
             *  we have lots of x, y and h.
             *  Gradient, as a ration is y/x 
             */
            return this.AscentInMetresToPoint(other) / this.HaversineDistanceInMetresToPoint(other);
        }

        /// <summary>
        /// Calculates the haversine distance in meters between this point and another using latitude and longitude:
        /// <a href="http://www.movable-type.co.uk/scripts/latlong.html">Reference</a>
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>Double, the distance in meters as the crow flies.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public double HaversineDistanceInMetresToPoint(DataPoint other)
        {
            var differenceInLat = other.Latitude - this.Latitude;
            var differenceInLon = other.Longitude - this.Longitude;

            var a = Math.Sin(differenceInLat / 2) * Math.Sin(differenceInLat / 2) +
                    Math.Cos(this.Latitude) * Math.Cos(other.Latitude) *
                    Math.Sin(differenceInLon / 2) * Math.Sin(differenceInLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var d = this.GetRadiusOfEarth(this.Latitude) * c;

            return d;
        }

        /// <summary>
        /// Gets the approximate radius of the earth at a given latitude based on WGS84 reference ellipsoid.
        /// </summary>
        /// <param name="latitude">The latitude in degrees.</param>
        /// <returns>The radius.</returns>
        private double GetRadiusOfEarth(Angle latitude)
        {
            return 6378000 - (21000 * Math.Sin(latitude));
        }
    }
}
