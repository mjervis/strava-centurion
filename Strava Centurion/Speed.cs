// -----------------------------------------------------------------------
// <copyright file="Speed.cs" company="FuckingBrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// -----------------------------------------------------------------------

namespace Strava_Centurion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Speed
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Speed"/> class with the given speed.
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
