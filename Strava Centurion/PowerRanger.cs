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
        /// Building a CSV of track point calculations for analysis.
        /// </summary>
        private readonly StringBuilder csv;

        /// <summary>
        /// Cache the previous speed for calculating acceleration between track points.
        /// </summary>
        private double previousSpeed;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerRanger"/> class. 
        /// Scoped in a given reality (physics constants, and time etc)
        /// </summary>
        /// <param name="reality">
        /// The reality in which we wish to range.
        /// </param>
        public PowerRanger(Reality reality)
        {
            this.RiderWeight = 76.7;
            this.BikeWeight = 10.5;

            this.reality = reality; // snigger;
            this.csv =
                new StringBuilder(
                    "distance,gradient,time,speed,rollingpower,hillpower,windpower,accellerationpower,totalPower,wattage")
                    .AppendLine();
        }

        #region Properties.

        /// <summary>
        /// Gets or sets the weight in kg of the rider.
        /// </summary>
        public double RiderWeight { get; set; }

        /// <summary>
        /// Gets or sets the weight in kg of the bike
        /// </summary>
        public double BikeWeight { get; set; }

        /// <summary>
        /// Gets the total weight of rider plus bike.
        /// </summary>
        public double TotalWeight
        {
            get
            {
                return this.BikeWeight + this.RiderWeight;
            }
        }

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
            DataPoint start = file.TrackPoints[0];
            start.PowerInWatts = 0.0;
            for (int i = 1; i < file.TrackPoints.Count; i++)
            {
                DataPoint end = file.TrackPoints[i];
                this.GeneratePower(start, end);
                start = end;
            }
        }

        /// <summary>
        /// Generate the power taken to move from <see cref="DataPoint"/> to <see cref="DataPoint"/> and output
        /// to CSV in the format of distance,gradient,time,speed,rollingpower,hillpower,windpower,accellerationpower,totalPower,wattage
        /// </summary>
        /// <param name="start">The <see cref="DataPoint"/> the rider started at.</param>
        /// <param name="end">The <see cref="DataPoint"/> the rider ended at.</param>
        private void GeneratePower(DataPoint start, DataPoint end)
        {
            var distance = start.DistanceToPoint(end).Metres;
            var gradient = start.GradientToPoint(end);
            var time = end.DateTime.Subtract(start.DateTime).TotalSeconds;
            var speed = distance / time;

            this.Csv.AppendFormat("{0},{1},{2},{3},", distance, gradient, time, speed);

            // TODO: this is surely not right - I might be free wheeling now but not so at start
            // TODO: this needs to be a check that average cadence between two points is not 0.
            if (end.CadenceInRpm == 0)
            {
                this.Csv.AppendFormat("{0},{1},{2},{3},{4},{5}", 0, 0, 0, 0, 0, 0).AppendLine();
            }
            else
            {
                var rollingResistanceForce = this.CalculateRollingResistanceForce();
                var accelerationForce = this.CalculateAccelerationForce(this.previousSpeed, speed, time);
                var hillForce = this.CalculateHillForce(gradient);
                var windForce = this.CalculateWindForce(speed, end.Altitude);

                var totalPower = rollingResistanceForce + accelerationForce + hillForce + windForce;

                this.Csv.AppendFormat("{0},{1},{2},{3},{4},{5}", rollingResistanceForce, hillForce, windForce, accelerationForce, totalPower, totalPower * speed).AppendLine();

                end.PowerInWatts = totalPower * speed;
            }

            this.previousSpeed = speed;
        }

        /// <summary>
        /// Calculate the force necessary to overcome wind resistance. Based on effective
        /// frontal area, drag coefficient, speed and air density.
        /// </summary>
        /// <param name="speed">Speed in m/s</param>
        /// <param name="altitude">Altitude in m</param>
        /// <returns>Force required in Newtons.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here, Newtons is a word.")]
        private double CalculateWindForce(double speed, double altitude)
        {
            return 0.5 * this.reality.EffectiveFrontalArea * this.reality.DragCoefficient * this.reality.AirDensity(altitude) * (speed * speed);
        }

        /// <summary>
        /// Calculate the force necessary to overcome the weight of the bike due
        /// to gravity given the gradient of the hill.
        /// </summary>
        /// <param name="gradient">Gradient as the ratio of ascent to distance.</param>
        /// <returns>Force required in Newtons.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here, Newtons is a word.")]
        private double CalculateHillForce(double gradient)
        {
            return this.TotalWeight * this.reality.AccelerationDueToGravity * gradient;
        }

        /// <summary>
        /// Calculate the force required to accelerate from a start speed to an end speed.
        /// </summary>
        /// <param name="startSpeed">Speed the rider started at in m/s</param>
        /// <param name="endSpeed">Speed the rider ended at in m/s</param>
        /// <param name="time">Time taken in s</param>
        /// <returns>Force required in Newtons.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here, Newtons is a word.")]
        private double CalculateAccelerationForce(double startSpeed, double endSpeed, double time)
        {
            // TODO: Check this - Surely decelleration denotes power being taken from the system?
            if (endSpeed > startSpeed)
            {
                var acceleration = (endSpeed - startSpeed) / time;

                return this.TotalWeight * acceleration;
            }

            return 0;
        }

        /// <summary>
        /// Gets the force of rolling resistance based on weight, gravity and coefficient of rolling resistance.
        /// </summary>
        private double CalculateRollingResistanceForce()
        {
            return this.TotalWeight * this.reality.AccelerationDueToGravity * this.reality.CoefficientOfRollingResistance;
        }
    }
}