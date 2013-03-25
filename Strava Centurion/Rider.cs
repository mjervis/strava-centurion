// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Rider.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   Defines the Rider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion
{
    /// <summary>
    /// The rider.
    /// </summary>
    public class Rider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rider"/> class. 
        /// </summary>
        /// <param name="weight">The weight of the rider (person).</param>
        /// <param name="bikeWeight">The weight of the bike.</param>
        public Rider(double weight, double bikeWeight)
        {
            this.Weight = weight;
            this.BikeWeight = bikeWeight;
        }

        /// <summary>
        /// Gets the rider weight.
        /// </summary>
        public double Weight { get; private set; }

        /// <summary>
        /// Gets the bike weight.
        /// </summary>
        public double BikeWeight { get; private set; }

        /// <summary>
        /// Gets the total weight.
        /// </summary>
        public double WeightIncludingBike
        {
            get
            {
                return this.Weight + this.BikeWeight;
            }
        }
    }
}
