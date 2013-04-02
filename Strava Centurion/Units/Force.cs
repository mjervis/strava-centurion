// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Force.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   Defines the Force type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturion.Units
{
    /// <summary>
    /// A struct that represents a force.
    /// </summary>
    public struct Force
    {
        /// <summary>
        /// Gets or sets the force in N.
        /// </summary>
        public double Newtons;

        /// <summary>
        /// Initializes a new instance of the <see cref="Force"/> struct.
        /// </summary>
        /// <param name="forceInNewtons">
        /// The initial force in N.
        /// </param>
        public Force(double forceInNewtons)
        {
            this.Newtons = forceInNewtons;
        }

        /// <summary>
        /// Gets a force of zero.
        /// </summary>
        public static Force Zero
        {
            get
            {
                return new Force(0);
            }
        }

        /// <summary>
        /// Gets an unknown force.
        /// </summary>
        public static Force Unknown
        {
            get
            {
                return new Force(double.NaN);
            }
        }

        /// <summary>
        /// Implicit conversion to a double.
        /// </summary>
        /// <param name="force">The force to convert.</param>
        /// <returns>The force in newtons.</returns>
        public static implicit operator double(Force force)
        {
            return force.Newtons;
        }

        /// <summary>
        /// Adds two forces.
        /// </summary>
        /// <param name="a">The first force.</param>
        /// <param name="b">The second force.</param>
        /// <returns>The two forces added together.</returns>
        public static Force operator +(Force a, Force b)
        {
            return new Force(a.Newtons + b.Newtons);
        }

        /// <summary>
        /// Subtracts one force from another.
        /// </summary>
        /// <param name="a">The first force.</param>
        /// <param name="b">The second force.</param>
        /// <returns>The first force - second force.</returns>
        public static Force operator -(Force a, Force b)
        {
            return new Force(a.Newtons - b.Newtons);
        }

        /// <summary>
        /// Divides a force by a double.
        /// </summary>
        /// <param name="force">The force.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Force operator /(Force force, double divisor)
        {
            return new Force(force.Newtons / divisor);
        }
    }
}
