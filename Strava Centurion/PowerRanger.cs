// -----------------------------------------------------------------------
// <copyright file="PowerRanger.cs" company="FuckingBrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// -----------------------------------------------------------------------

namespace Strava_Centurion
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Provide the physics power calculations.
    /// </summary>
    public class PowerRanger
    {
        #region Fields and constants.

        /// <summary>
        /// The reality in which we exist.
        /// </summary>
        private readonly Reality reality;

        /// <summary>
        /// The rider being analyzed.
        /// </summary>
        private readonly Rider rider;

        /// <summary>
        /// Building a CSV of track point calculations for analysis.
        /// </summary>
        private readonly StringBuilder csv;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerRanger"/> class. 
        /// Scoped in a given reality (physics constants, and time etc)
        /// </summary>
        /// <param name="reality">The reality in which we wish to range.</param>
        /// <param name="rider">The rider being analyzed.</param>
        public PowerRanger(Reality reality, Rider rider)
        {
            this.reality = reality;
            this.rider = rider;

            this.csv = new StringBuilder("distance,gradient,time,speed,rollingpower,hillpower,windpower,accellerationpower,totalPower,wattage").AppendLine();
        }

        #region Properties.

        /// <summary>
        /// Gets the CSV of track point calculations for analysis.
        /// </summary>
        public StringBuilder Csv
        {
            get
            {
                return this.csv;
            }
        }

        #endregion

        /// <summary>
        /// Calculate power over a <see cref="TcxFile"/>.
        /// </summary>
        /// <param name="file">The <see cref="TcxFile"/> to Morph</param>
        public IEnumerable<DataSegment> Morph(TcxFile file)
        {
            var start = file.TrackPoints[0];
            start.Power = Power.Zero;

            var segments = new List<DataSegment>();

            for (var i = 1; i < file.TrackPoints.Count; i++)
            {
                var end = file.TrackPoints[i];

                // TODO: If start and end are at the same lat and long, then the segment needs to be start -> end + 1 and end needs power of end+1 adding :(
                var segment = new DataSegment(start, end);

                segment.RollingResistanceForce = this.CalculateRollingResistanceForce();
                segment.AccelerationForce = this.CalculateAccelerationForce(segment);
                segment.HillForce = this.CalculateHillForce(segment);
                segment.WindForce = this.CalculateWindForce(segment);

                segment.End.Power = new Power(segment.TotalForce * segment.Speed.MetersPerSecond);

                segments.Add(segment);

                start = end;
            }

            return segments;
        }

        /// <summary>
        /// Calculate the force necessary to overcome wind resistance. Based on effective
        /// frontal area, drag coefficient, speed and air density.
        /// </summary>
        /// <param name="segment">The data segment.</param>
        /// <returns>Force required in Newtons.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here, Newtons is a word.")]
        public Force CalculateWindForce(DataSegment segment)
        {
            return new Force(0.5 * this.reality.EffectiveFrontalArea * this.reality.DragCoefficient * this.reality.AirDensity(segment.End.Altitude) * (segment.Speed.MetersPerSecond * segment.Speed.MetersPerSecond));
        }

        /// <summary>
        /// Calculate the force necessary to overcome the weight of the bike due
        /// to gravity given the gradient of the hill.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <returns>Force required.</returns>
        public Force CalculateHillForce(DataSegment segment)
        {
            return new Force(this.rider.MassIncludingBike * this.reality.AccelerationDueToGravity * segment.Gradient);
        }

        /// <summary>
        /// Calculate the force required to accelerate from a start speed to an end speed.
        /// </summary>
        /// <param name="segment">The data segment</param>
        /// <returns>Force required.</returns>
        public Force CalculateAccelerationForce(DataSegment segment)
        {
            return new Force(this.rider.MassIncludingBike * segment.Acceleration);
        }

        /// <summary>
        /// Gets the force of rolling resistance based on weight, gravity and coefficient of rolling resistance.
        /// </summary>
        /// <returns>Returns the force.</returns>
        public Force CalculateRollingResistanceForce()
        {
            // weight = mass(kg's) * gravity(9.81 m/s2)
            // c = rolling resistance coefficient
            // resistance (newtons) = c * weight
            return new Force(this.rider.MassIncludingBike * this.reality.AccelerationDueToGravity * this.reality.CoefficientOfRollingResistance);
        }
    }
}