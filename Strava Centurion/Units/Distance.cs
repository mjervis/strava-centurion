// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Distance.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   Defines the Distance type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturion.Units
{
    /// <summary>
    /// Represents a distance.
    /// </summary>
    public struct Distance
    {
        /// <summary>
        /// The distance of zero.
        /// </summary>
        public static Distance Zero = new Distance(0.0);

        /// <summary>
        /// Gets an unknown distance.
        /// </summary>
        public static Distance Unknown = new Distance(double.NaN);

        /// <summary>
        /// The distance in meters.
        /// </summary>
        private double metres;

        /// <summary>
        /// Initializes a new instance of the <see cref="Distance"/> struct.
        /// </summary>
        /// <param name="distanceInMetres">
        /// The distance in meters.
        /// </param>
        public Distance(double distanceInMetres)
        {
            this.metres = distanceInMetres;
        }

        /// <summary>
        /// Gets or sets the distance in meters.
        /// </summary>
        public double Metres 
        {
            get
            {
                return this.metres;
            }

            set
            {
                this.metres = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this distance is unknown.
        /// </summary>
        public bool IsUnknown
        {
            get
            {
                return double.IsNaN(this.Metres);
            }
        }

        /// <summary>
        /// Gets the distance in kilometers.
        /// </summary>
        public double Kilometres
        {
            get
            {
                return this.Metres / 1000.0;
            }
        }

        /// <summary>
        /// Gets the distance in miles.
        /// </summary>
        public double Miles
        {
            get
            {
                return this.Metres * 0.000621371192;
            }
        }

        /// <summary>
        /// An implicit conversion to a double.
        /// </summary>
        /// <param name="distance">The distance to convert.</param>
        /// <returns>The number of meters as a double.</returns>
        public static implicit operator double(Distance distance)
        {
            return distance.Metres;
        }

        /// <summary>
        /// Adds two distances together.
        /// </summary>
        /// <param name="a">The first distance.</param>
        /// <param name="b">The second distance.</param>
        /// <returns>The result of the addition.</returns>
        public static Distance operator +(Distance a, Distance b)
        {
            return new Distance(a.Metres + b.Metres);
        }

        /// <summary>
        /// Subtracts one distance from another.
        /// </summary>
        /// <param name="a">The first distance.</param>
        /// <param name="b">The second distance.</param>
        /// <returns>The result of the subtraction.</returns>
        public static Distance operator -(Distance a, Distance b)
        {
            return new Distance(a.Metres - b.Metres);
        }

        /// <summary>
        /// Divides a distance by a divisor.
        /// </summary>
        /// <param name="distance">The distance to be divided.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Distance operator /(Distance distance, double divisor)
        {
            return new Distance(distance.Metres / divisor);
        }
    }
}
