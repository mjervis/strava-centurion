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
        /// Mean radius of the earth in m.
        /// </summary>
        private const double RadiusOfEarth = 6378100.0;

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
            this.AltitudeInMetres = node.GetAltitude();
            this.CadenceInRpm = node.GetCadence();
            this.TotalDistanceInMetres = node.GetTotalDistance();
            this.SpeedInKmPerHour = node.GetSpeed();
            this.HeartrateInBpm = node.GetHeartrate();
            this.LatitudeInDegrees = node.GetLatitude();
            this.LongitudeInDegrees = node.GetLongitude();
            this.PowerInWatts = node.GetPower();
        }

        #region Properties.    

        /// <summary>
        /// Gets the altitude in meters
        /// </summary>
        public double AltitudeInMetres { get; private set; }

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
        public double TotalDistanceInMetres { get; private set; }

        /// <summary>
        /// Gets or sets the date and time that the point was recorded
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the longitude in degrees.
        /// </summary>
        public double LongitudeInDegrees { get; set; }

        /// <summary>
        /// Gets or sets the latitude in degrees.
        /// </summary>
        public double LatitudeInDegrees { get; set; }

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
        public double DistanceInMetresToPoint(DataPoint other)
        {
            double result;

            if (double.IsNaN(this.TotalDistanceInMetres) || double.IsNaN(other.TotalDistanceInMetres))
            {
                var ascent = this.AscentInMetresToPoint(other);
                var haversine = this.HaversineDistanceInMetresToPoint(other);

                result = Math.Sqrt((ascent * ascent) + (haversine * haversine));
            }
            else
            {
                result = other.TotalDistanceInMetres - this.TotalDistanceInMetres;
            }

            return result;
        }

        /// <summary>
        /// Calculates the ascent from this point to the point specified.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>The ascent in meters.</returns>
        public double AscentInMetresToPoint(DataPoint other)
        {
            return other.AltitudeInMetres - this.AltitudeInMetres;
        }

        /// <summary>
        /// Calculates the haversine distance in meters between this point and another using latitude and longitude:
        /// <a href="http://www.movable-type.co.uk/scripts/latlong.html">Reference</a>
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>Double, the distance in meters as the crow flies.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private double HaversineDistanceInMetresToPoint(DataPoint other)
        {
            var differenceInLat = this.ToRad(other.LatitudeInDegrees - this.LatitudeInDegrees);
            var differenceInLon = this.ToRad(other.LongitudeInDegrees - this.LongitudeInDegrees);

            var a = (Math.Sin(differenceInLat / 2) * Math.Sin(differenceInLat / 2)) +
                    (Math.Cos(this.ToRad(this.LatitudeInDegrees)) * Math.Cos(this.ToRad(other.LatitudeInDegrees))
                        * Math.Sin(differenceInLon / 2) * Math.Sin(differenceInLon / 2));
            var c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            var d = RadiusOfEarth * c;

            return d;
        }

        /// <summary>
        /// Convert degrees to radians.
        /// </summary>
        /// <param name="degrees">Degrees to convert.</param>
        /// <returns>Angle in radians.</returns>
        private double ToRad(double degrees)
        {
            return Math.PI * degrees / 180.0;
        }
    }
}
