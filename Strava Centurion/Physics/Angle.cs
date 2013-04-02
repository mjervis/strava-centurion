// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Angle.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   Defines the Angle type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion
{
    using System;

    /// <summary>
    /// A struct that represents an angle.
    /// </summary>
    public struct Angle
    {
        /// <summary>
        /// Gets or sets the angle in radians.
        /// </summary>
        public double Radians;

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle"/> struct.
        /// </summary>
        /// <param name="angleInRadians">
        /// The initial angle in radians.
        /// </param>
        public Angle(double angleInRadians)
        {
            this.Radians = angleInRadians;
        }

        /// <summary>
        /// Gets an angle of zero.
        /// </summary>
        public static Angle Zero
        {
            get
            {
                return new Angle(0);
            }
        }

        /// <summary>
        /// Gets an unknown angle.
        /// </summary>
        public static Angle Unknown
        {
            get
            {
                return new Angle(double.NaN);
            }
        }

        /// <summary>
        /// Is this an unknown angle.
        /// </summary>
        public bool IsUnknown
        {
            get
            {
                return double.IsNaN(this.Radians);
            }
        }

        /// <summary>
        /// Gets the angle in degrees.
        /// </summary>
        public double Degrees
        {
            get
            {
                return (this.Radians * 180) / Math.PI;
            }
        }

        /// <summary>
        /// Creates an angle from degrees.
        /// </summary>
        /// <param name="angleInDegrees">The angle in degrees.</param>
        /// <returns>An angle instance.</returns>
        public static Angle FromDegrees(double angleInDegrees)
        {
            return new Angle(Math.PI * angleInDegrees / 180.0);
        }

        /// <summary>
        /// Implicit conversion to a double.
        /// </summary>
        /// <param name="a">The angle to convert.</param>
        /// <returns>The angle in radians.</returns>
        public static implicit operator double(Angle a)
        {
            return a.Radians;
        }

        /// <summary>
        /// Adds two angles.
        /// </summary>
        /// <param name="a">The first angle.</param>
        /// <param name="b">The second angle.</param>
        /// <returns>The two angles added together.</returns>
        public static Angle operator +(Angle a, Angle b)
        {
            return new Angle(a.Radians + b.Radians);
        }

        /// <summary>
        /// Subtracts one angle from another.
        /// </summary>
        /// <param name="a">The first angle.</param>
        /// <param name="b">The second angle.</param>
        /// <returns>The first angle - second angle.</returns>
        public static Angle operator -(Angle a, Angle b)
        {
            return new Angle(a.Radians - b.Radians);
        }

        /// <summary>
        /// Divides an angle by a double.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Angle operator /(Angle angle, double divisor)
        {
            return new Angle(angle.Radians / divisor);
        }
    }
}
