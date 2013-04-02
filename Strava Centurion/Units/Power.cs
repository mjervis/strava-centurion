// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Power.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   Defines the Force type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturion.Units
{
    /// <summary>
    /// A struct that represents a power.
    /// </summary>
    public struct Power
    {
        /// <summary>
        /// Gets or sets the power in watts.
        /// </summary>
        public double Watts;

        /// <summary>
        /// Initializes a new instance of the <see cref="Power"/> struct.
        /// </summary>
        /// <param name="powerInWatts">The initial power in watts.</param>
        public Power(double powerInWatts)
        {
            this.Watts = powerInWatts;
        }

        /// <summary>
        /// Gets a power of zero.
        /// </summary>
        public static Power Zero
        {
            get
            {
                return new Power(0);
            }
        }

        /// <summary>
        /// Gets an unknown power.
        /// </summary>
        public static Power Unknown
        {
            get
            {
                return new Power(double.NaN);
            }
        }

        /// <summary>
        /// Implicit conversion to a double.
        /// </summary>
        /// <param name="power">The power to convert.</param>
        /// <returns>The power in watts.</returns>
        public static implicit operator double(Power power)
        {
            return power.Watts;
        }

        /// <summary>
        /// Adds two powers.
        /// </summary>
        /// <param name="a">The first power.</param>
        /// <param name="b">The second power.</param>
        /// <returns>The two powers added together.</returns>
        public static Power operator +(Power a, Power b)
        {
            return new Power(a.Watts + b.Watts);
        }

        /// <summary>
        /// Subtracts one power from another.
        /// </summary>
        /// <param name="a">The first power.</param>
        /// <param name="b">The second power.</param>
        /// <returns>The first power - second power.</returns>
        public static Power operator -(Power a, Power b)
        {
            return new Power(a.Watts - b.Watts);
        }

        /// <summary>
        /// Divides a power by a double.
        /// </summary>
        /// <param name="power">The power.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Power operator /(Power power, double divisor)
        {
            return new Power(power.Watts / divisor);
        }
    }
}
