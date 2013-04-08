// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Angle.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   Defines the Angle type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturion.Units
{
    using System;

    /// <summary>
    /// A struct that represents an angle.
    /// </summary>
    public struct Angle
    {
        /// <summary>
        /// An angle of zero.
        /// </summary>
        public static readonly Angle Zero = new Angle(0.0);

        /// <summary>
        /// An unknown angle.
        /// </summary>
        public static readonly Angle Unknown = new Angle(double.NaN);

        /// <summary>
        /// The angle in radians.
        /// </summary>
        private double radians;

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle"/> struct.
        /// </summary>
        /// <param name="angleInRadians">
        /// The initial angle in radians.
        /// </param>
        public Angle(double angleInRadians)
        {
            this.radians = angleInRadians;
        }

        /// <summary>
        /// Gets or sets the angle in radians.
        /// </summary>
        public double Radians
        {
            get
            {
                return this.radians;
            }

            set
            {
                this.radians = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this angle is unknown.
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
                return (this.Radians * 180.0) / Math.PI;
            }
        }

        /// <summary>
        /// Creates an angle from a specified number of degrees.
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
        /// <param name="angle">The angle to convert.</param>
        /// <returns>The angle in radians.</returns>
        public static implicit operator double(Angle angle)
        {
            return angle.Radians;
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
        /// <returns>The result of the subtraction.</returns>
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
