// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Rider.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   Defines the Rider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturion
{
    /// <summary>
    /// The rider.
    /// </summary>
    public class Rider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rider"/> class. 
        /// </summary>
        /// <param name="massInKg">The mass of the rider (person).</param>
        /// <param name="bikeMassInKg">The mass of the bike.</param>
        public Rider(double massInKg, double bikeMassInKg)
        {
            this.Mass = massInKg;
            this.BikeMass = bikeMassInKg;
        }

        /// <summary>
        /// Gets the rider weight.
        /// </summary>
        public double Mass { get; private set; }

        /// <summary>
        /// Gets the bike weight.
        /// </summary>
        public double BikeMass { get; private set; }

        /// <summary>
        /// Gets the total weight.
        /// </summary>
        public double MassIncludingBike
        {
            get
            {
                return this.Mass + this.BikeMass;
            }
        }
    }
}
