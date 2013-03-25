// -----------------------------------------------------------------------
// <copyright file="PowerRanger.cs" company="FuckingBrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// -----------------------------------------------------------------------

namespace Strava_Centurion
{
    using System.Diagnostics.CodeAnalysis;
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
        public void Morph(TcxFile file)
        {
            var start = file.TrackPoints[0];
            start.PowerInWatts = 0.0;

            for (var i = 1; i < file.TrackPoints.Count; i++)
            {
                var end = file.TrackPoints[i];
                this.GeneratePower(new DataSegment(start, end));    // TODO: would we want to maintain a list of data segments instead?

                start = end;
            }
        }

        /// <summary>
        /// Generate the power taken to move from <see cref="DataPoint"/> to <see cref="DataPoint"/> and output
        /// to CSV in the format of distance,gradient,time,speed,rolling power,hill power,wind power,acceleration power,total Power,wattage
        /// </summary>
        /// <param name="segment">The <see cref="DataSegment"/> the rider has ridden.</param>
        private void GeneratePower(DataSegment segment)
        {
            // TODO: csv output should probably be abstracted away
            // TODO: this should just be an output of some sort - encapsulate the formatting.
            this.Csv.AppendFormat("{0},{1},{2},{3},", segment.Distance.Metres, segment.Gradient, segment.ElapsedTime, segment.Speed.MetersPerSecond);

            var rollingResistanceForce = this.CalculateRollingResistanceForce();
            var accelerationForce = this.CalculateAccelerationForce(segment);
            var hillForce = this.CalculateHillForce(segment);
            var windForce = this.CalculateWindForce(segment);

            var totalPower = rollingResistanceForce + accelerationForce + hillForce + windForce;
            if (totalPower < 0.0)
            {
                totalPower = 0.0; // can't do negative power.
            }

            segment.End.PowerInWatts = totalPower * segment.Speed.MetersPerSecond;

            this.Csv.AppendFormat("{0},{1},{2},{3},{4},{5}", rollingResistanceForce, hillForce, windForce, accelerationForce, totalPower, segment.End.PowerInWatts).AppendLine();              
        }

        /// <summary>
        /// Calculate the force necessary to overcome wind resistance. Based on effective
        /// frontal area, drag coefficient, speed and air density.
        /// </summary>
        /// <param name="segment">The data segment.</param>
        /// <returns>Force required in Newtons.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here, Newtons is a word.")]
        private double CalculateWindForce(DataSegment segment)
        {
            return 0.5 * this.reality.EffectiveFrontalArea * this.reality.DragCoefficient * this.reality.AirDensity(segment.End.Altitude) * (segment.Speed.MetersPerSecond * segment.Speed.MetersPerSecond);
        }

        /// <summary>
        /// Calculate the force necessary to overcome the weight of the bike due
        /// to gravity given the gradient of the hill.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <returns>Force required.</returns>
        private double CalculateHillForce(DataSegment segment)
        {
            return this.rider.WeightIncludingBike * this.reality.AccelerationDueToGravity * segment.Gradient;
        }

        /// <summary>
        /// Calculate the force required to accelerate from a start speed to an end speed.
        /// </summary>
        /// <param name="segment">The data segment</param>
        /// <returns>Force required.</returns>
        private double CalculateAccelerationForce(DataSegment segment)
        {
            // TODO: Check this - Surely decelleration denotes power being taken from the system?
            if (segment.End.Speed.MetersPerSecond > segment.Start.Speed.MetersPerSecond)
            {
                return this.rider.WeightIncludingBike * segment.Acceleration;
            }

            return 0;
        }

        /// <summary>
        /// Gets the force of rolling resistance based on weight, gravity and coefficient of rolling resistance.
        /// </summary>
        /// <returns>Returns the force.</returns>
        private double CalculateRollingResistanceForce()
        {
            return this.rider.WeightIncludingBike * this.reality.AccelerationDueToGravity * this.reality.CoefficientOfRollingResistance;
        }
    }
}