// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Acceleration.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   Defines the Acceleration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturion.Units
{
    /// <summary>
    /// Represents an acceleration.
    /// </summary>
    public struct Acceleration
    {
        /// <summary>
        /// The acceleration of zero.
        /// </summary>
        public static readonly Acceleration Zero = new Acceleration(0.0);

        /// <summary>
        /// The acceleration of zero.
        /// </summary>
        public static readonly Acceleration Gravity = new Acceleration(9.80665);

        /// <summary>
        /// The unknown acceleration.
        /// </summary>
        public static readonly Acceleration Unknown = new Acceleration(double.NaN);

        /// <summary>
        /// The acceleration in meters per second per second.
        /// </summary>
        private double metresPerSecondPerSecond;

        /// <summary>
        /// Initializes a new instance of the <see cref="Acceleration"/> struct.
        /// </summary>
        /// <param name="accelerationInMetresPerSecondPerSecond">
        /// The acceleration in meters per second.
        /// </param>
        public Acceleration(double accelerationInMetresPerSecondPerSecond)
        {
            this.metresPerSecondPerSecond = accelerationInMetresPerSecondPerSecond;
        }

        /// <summary>
        /// Gets or sets the acceleration in meters per second per second.
        /// </summary>
        public double MetresPerSecondPerSecond
        {
            get
            {
                return this.metresPerSecondPerSecond;
            }

            set
            {
                this.metresPerSecondPerSecond = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this acceleration is unknown.
        /// </summary>
        public bool IsUnknown
        {
            get
            {
                return double.IsNaN(this.MetresPerSecondPerSecond);
            }
        }

        /// <summary>
        /// An implicit conversion to a double.
        /// </summary>
        /// <param name="distance">The acceleration to convert.</param>
        /// <returns>The number of meters as a double.</returns>
        public static implicit operator double(Acceleration distance)
        {
            return distance.MetresPerSecondPerSecond;
        }

        /// <summary>
        /// Adds two accelerations together.
        /// </summary>
        /// <param name="a">The first acceleration.</param>
        /// <param name="b">The second acceleration.</param>
        /// <returns>The result of the addition.</returns>
        public static Acceleration operator +(Acceleration a, Acceleration b)
        {
            return new Acceleration(a.MetresPerSecondPerSecond + b.MetresPerSecondPerSecond);
        }

        /// <summary>
        /// Subtracts one acceleration from another.
        /// </summary>
        /// <param name="a">The first acceleration.</param>
        /// <param name="b">The second acceleration.</param>
        /// <returns>The result of the subtraction.</returns>
        public static Acceleration operator -(Acceleration a, Acceleration b)
        {
            return new Acceleration(a.MetresPerSecondPerSecond - b.MetresPerSecondPerSecond);
        }

        /// <summary>
        /// Divides na acceleration by a divisor.
        /// </summary>
        /// <param name="distance">The acceleration to be divided.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Acceleration operator /(Acceleration distance, double divisor)
        {
            return new Acceleration(distance.MetresPerSecondPerSecond / divisor);
        }
    }
}
