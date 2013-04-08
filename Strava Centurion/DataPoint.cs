// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPoint.cs" company="RuPC">
//   Copyright 2013 RuPC.
// </copyright>
// <summary>
//   Represent a track data point.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturion
{
    using System;

    using StravaCenturion.Units;

    /// <summary>
    /// Represent a track data point.
    /// </summary>
    public class DataPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataPoint"/> class. 
        /// Create a track point from an XML track point node.
        /// </summary>
        /// <param name="dateTime">The date and time.</param>
        /// <param name="altitude">The altitude.</param>
        /// <param name="cadence">The cadence.</param>
        /// <param name="totalDistance">The total distance.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="heartrateInBpm">The heart rate In beats per minute.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public DataPoint(DateTime dateTime, Distance altitude, Frequency cadence, Distance totalDistance, Speed speed, Frequency heartrateInBpm, Angle latitude, Angle longitude)
        {
            this.DateTime = dateTime;
            this.Altitude = altitude;
            this.Cadence = cadence;
            this.TotalDistance = totalDistance;
            this.Speed = speed;
            this.Heartrate = heartrateInBpm;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        /// <summary>
        /// Gets the altitude.
        /// </summary>
        public Distance Altitude { get; private set; }

        /// <summary>
        /// Gets the heart rate in beats per minute.
        /// </summary>
        public Frequency Heartrate { get; private set; }

        /// <summary>
        /// Gets the cadence in revolutions per minute.
        /// </summary>
        public Frequency Cadence { get; private set; }

        /// <summary>
        /// Gets the speed.
        /// </summary>
        public Speed Speed { get; set; }

        /// <summary>
        /// Gets the distance.
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
            if (this.TotalDistance.IsUnknown || other.TotalDistance.IsUnknown)
            {
                var ascent = this.AscentToPoint(other);
                var haversine = this.HaversineDistanceToPoint(other);

                return new Distance(Math.Sqrt((ascent * ascent) + (haversine * haversine)));
            }

            return other.TotalDistance - this.TotalDistance;
        }

        /// <summary>
        /// Calculates the ascent from this point to the point specified.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>The ascent in meters.</returns>
        public Distance AscentToPoint(DataPoint other)
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
            double distance = this.HaversineDistanceToPoint(other);

            // if we've not moved then the gradient must be 0
            if (Math.Abs(distance - 0.0) < 0.0001)
            {
                return 0.0;
            }

            return this.AscentToPoint(other) / distance;   
        }

        /// <summary>
        /// Calculates the distance in meters between this point and another using latitude and longitude.
        /// <a href="http://www.movable-type.co.uk/scripts/latlong.html">Reference</a>
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>Double, the distance in meters as the crow flies.</returns>
        public Distance HaversineDistanceToPoint(DataPoint other)
        {
            var latitudeDelta = other.Latitude - this.Latitude;
            var longitudeDelta = other.Longitude - this.Longitude;

            var a = (Math.Sin(latitudeDelta / 2.0) * Math.Sin(latitudeDelta / 2.0))
                    + (Math.Sin(longitudeDelta / 2.0) * Math.Sin(longitudeDelta / 2.0) * Math.Cos(this.Latitude)
                       * Math.Cos(other.Latitude));

            var r = this.GetRadiusOfEarth(this.Latitude);

            var c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            return new Distance(c * r);
        }

        /// <summary>
        /// Gets the approximate radius of the earth at a given latitude based on WGS84 reference ellipsoid.
        /// </summary>
        /// <param name="latitude">The latitude in degrees.</param>
        /// <returns>The radius.</returns>
        private Distance GetRadiusOfEarth(Angle latitude)
        {
            return new Distance(6378000.0 - (21000.0 * Math.Sin(latitude)));
        }
    }
}
