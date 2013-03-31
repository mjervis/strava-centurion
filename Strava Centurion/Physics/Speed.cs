// -----------------------------------------------------------------------
// <copyright file="Speed.cs" company="FuckingBrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// -----------------------------------------------------------------------

namespace Strava_Centurion
{
    /// <summary>
    /// Units of measure for speed.
    /// </summary>
    public enum SpeedUnits
    {
        /// <summary>
        /// Speed is in meters per second.
        /// </summary>
        MetersSecond,
        /// <summary>
        /// Speed is in Km/H
        /// </summary>
        KilometersHour,
        /// <summary>
        /// Speed is in Feet/s
        /// </summary>
        FeetSecond,
        /// <summary>
        /// Speed is in Miles/hour
        /// </summary>
        MilesHour
    }

    /// <summary>
    /// Encapsulate speed and conversions.
    /// </summary>
    public class Speed
    {
        /// <summary>
        /// Gets a speed of zero.
        /// </summary>
        public static Speed Zero
        {
            get
            {
                return new Speed(0);
            }
        }

        /// <summary>
        /// Gets an unknown speed.
        /// </summary>
        public static Speed Unknown
        {
            get
            {
                return new Speed(double.NaN);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Speed"/> class with the given speed in m/s.
        /// </summary>
        /// <param name="metersPerSecond">
        /// The speed in meters per second. Must be positive.
        /// </param>
        public Speed(double metersPerSecond)
        {
            if (metersPerSecond < 0.0)
            {
                throw new System.ArgumentException("Speed must be >=0, negative speed not valid.");
            }

            this.MetersPerSecond = metersPerSecond;
        }

        public Speed(double speed, SpeedUnits unit)
        {
            switch(unit)
            {
                case SpeedUnits.MetersSecond:
                    this.MetersPerSecond = speed;
                    break;
                case SpeedUnits.KilometersHour:
                    this.MetersPerSecond = speed * 0.277777778;
                    break;
                case SpeedUnits.MilesHour:
                    this.MetersPerSecond = speed * 0.44704;
                    break;
                case SpeedUnits.FeetSecond:
                    this.MetersPerSecond = speed * 0.3048;
                    break;
            }
        }

        /// <summary>
        /// Gets the speed in Meters Per Second
        /// </summary>
        public double MetersPerSecond { get; private set; }

        /// <summary>
        /// Gets speed in KmHour
        /// </summary>
        public double KmHour
        {
            get
            {
                return this.MetersPerSecond * 3.6;
            }
        }

        /// <summary>
        /// Gets speed in Feet/s
        /// </summary>
        public double FeetPerSecond
        {
            get
            {
                return this.MetersPerSecond * 3.2808;
            }
        }

        /// <summary>
        /// Gets speed in miles/hour
        /// </summary>
        public double MilesPerHour
        {
            get
            {
                return this.MetersPerSecond * 2.2369;
            }
        }
    }
}
