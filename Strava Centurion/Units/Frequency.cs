// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Frequency.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   Defines the Frequency type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturion.Units
{
    /// <summary>
    /// A struct that represents a frequency.
    /// </summary>
    public struct Frequency
    {
        /// <summary>
        /// Gets or sets the frequency per minute.
        /// </summary>
        public int PerMinute;

        /// <summary>
        /// Initializes a new instance of the <see cref="Frequency"/> struct.
        /// </summary>
        /// <param name="frequencyPerMinute">
        /// The Frequency per minute.
        /// </param>
        public Frequency(int frequencyPerMinute)
        {
            this.PerMinute = frequencyPerMinute;
        }

        /// <summary>
        /// Gets a Frequency of zero.
        /// </summary>
        public static Frequency Zero
        {
            get
            {
                return new Frequency(0);
            }
        }

        /// <summary>
        /// Gets an unknown Frequency.
        /// </summary>
        public static Frequency Unknown
        {
            get
            {
                return new Frequency(int.MinValue);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this Frequency is known.
        /// </summary>
        public bool IsUnknown
        {
            get
            {
                return this.PerMinute == int.MinValue;
            }
        }

        /// <summary>
        /// Implicit conversion to an integer.
        /// </summary>
        /// <param name="frequency">The Frequency to convert.</param>
        /// <returns>The Frequency in revolutions per minute.</returns>
        public static implicit operator int(Frequency frequency)
        {
            return frequency.PerMinute;
        }

        /// <summary>
        /// Adds two Frequencies.
        /// </summary>
        /// <param name="a">The first Frequency.</param>
        /// <param name="b">The second Frequency.</param>
        /// <returns>The two Frequencies added together.</returns>
        public static Frequency operator +(Frequency a, Frequency b)
        {
            return new Frequency(a.PerMinute + b.PerMinute);
        }

        /// <summary>
        /// Subtracts one Frequency from another.
        /// </summary>
        /// <param name="a">The first Frequency.</param>
        /// <param name="b">The second Frequency.</param>
        /// <returns>The first Frequency - second Frequency.</returns>
        public static Frequency operator -(Frequency a, Frequency b)
        {
            return new Frequency(a.PerMinute - b.PerMinute);
        }

        /// <summary>
        /// Divides a Frequency by a double.
        /// </summary>
        /// <param name="frequency">The angle.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Frequency operator /(Frequency frequency, double divisor)
        {
            return new Frequency((int)(frequency.PerMinute / divisor));
        }
    }
}
