// -----------------------------------------------------------------------
// <copyright file="PowerRanger.cs" company="FuckingBrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// -----------------------------------------------------------------------

namespace Strava_Centurion
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    /// <summary>
    /// Provide the physics power calculations.
    /// </summary>
    public class PowerRanger
    {
        #region Fields and constants.

        /// <summary>
        /// Mean radius of the earth in m.
        /// </summary>
        private const double RadiusOfEarth = 6378100.0;

        /// <summary>
        /// The reality in which we exist.
        /// </summary>
        private readonly Reality reality;

        /// <summary>
        /// Building a CSV of track point calculations for analysis.
        /// </summary>
        private readonly StringBuilder csv;

        /// <summary>
        /// Weight in kg of the rider.
        /// </summary>
        private double wkgRider = 76.7;

        /// <summary>
        /// Weight in kg of the bike
        /// </summary>
        private double wkgBike = 10.5;

        /// <summary>
        /// Total weight of rider plus bike.
        /// </summary>
        private double wkg = 87.2;

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
        public double RiderWeight
        {
            get
            {
                return this.wkgRider;
            }

            set
            {
                this.wkgRider = value;
                this.wkg = this.wkgBike + this.wkgRider;
            }
        }

        /// <summary>
        /// Gets or sets the weight in kg of the bike
        /// </summary>
        public double BikeWeight
        {
            get
            {
                return this.wkgBike;
            }

            set
            {
                this.wkgBike = value;
                this.wkg = this.wkgBike + this.wkgRider;
            }
        }

        /// <summary>
        /// Gets the total weight of rider plus bike.
        /// </summary>
        public double TotalWeight
        {
            get
            {
                return this.wkg;
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

        /// <summary>
        /// Gets the force of rolling resistance based on weight, gravity and coefficient of rolling resistance.
        /// </summary>
        private double ForceRollingResistance
        {
            get
            {
                return this.wkg * this.reality.AccelerationDueToGravity * this.reality.CoefficientOfRollingResistance;
            }
        }

        #endregion

        /// <summary>
        /// Calculate power over a <see cref="TcxFile"/>.
        /// </summary>
        /// <param name="file">The <see cref="TcxFile"/> to Morph</param>
        public void Morph(TcxFile file)
        {
            TcxPoint start = file.TrackPoints[0];
            start.PowerInWatts = 0.0;
            for (int i = 1; i < file.TrackPoints.Count; i++)
            {
                TcxPoint end = file.TrackPoints[i];
                this.GeneratePower(start, end);
                start = end;
            }
        }

        /// <summary>
        /// Generate the power taken to move from <see cref="TcxPoint"/> to <see cref="TcxPoint"/>
        /// </summary>
        /// <param name="start">The <see cref="TcxPoint"/> the rider started at.</param>
        /// <param name="end">The <see cref="TcxPoint"/> the rider ended at.</param>
        private void GeneratePower(TcxPoint start, TcxPoint end)
        {
            // Distance is sometimes literal, and sometimes we need to trig it, depending on source data..
            double distance = this.GetDistance(start, end);
            double time = end.DateTime.Subtract(start.DateTime).TotalSeconds;
            double speed = distance / time;
            if (end.CadenceInRpm == "0")
            {
                // "distance,gradient,time,speed,rollingpower,hillpower,windpower,accellerationpower,totalPower,wattage"
                this.Csv.AppendFormat(
                    "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", distance, "--", time, speed, 0, 0, 0, 0, 0, 0).AppendLine();
                this.previousSpeed = speed;
            }
            else
            {
                double ascent = end.AltitudeInMetres - start.AltitudeInMetres;

                /*
                 * Trig:
                 *  h = sqrt(x^2 + y^2) where x and y are the lengths of the other two sides.
                 *  y is ascent, we know that.
                 *  h is distance, we know that.
                 *  We need x, so rarranging:
                 *  h^2 = x^2 + y^2
                 *  h^2 - y^2 = x ^2
                 *  sqrt(h^2 - y^2) = x
                 *  h is distance, y is acent
                 */
                double x = Math.Sqrt((distance * distance) - (ascent * ascent));
                double gradient = ascent / x;

                // "distance,gradient,time,speed,rollingpower,hillpower,windpower,accellerationpower,totalPower,wattage"
                this.Csv.AppendFormat("{0},{1},{2},{3},", distance, gradient, time, speed);
                end.PowerInWatts = this.TotalPower(speed, end.AltitudeInMetres, gradient, time, this.previousSpeed);
                this.previousSpeed = speed;
            }
        }

        /// <summary>
        /// Fetch or calculate the distance between two points.
        /// </summary>
        /// <param name="start">
        /// The start point.
        /// </param>
        /// <param name="end">
        /// The end point
        /// </param>
        /// <returns>
        /// Distance in m between two points.
        /// </returns>
        private double GetDistance(TcxPoint start, TcxPoint end)
        {
            double ret;
            if (end.NoDistance || start.NoDistance)
            {
                ret = this.DistanceBetweenTwoPoints(start, end);
            }
            else
            {
                ret = end.DistanceInMetres - start.DistanceInMetres;
            }

            return ret;
        }

        /// <summary>
        /// Calculate the distance between two points based on the latitude and longitude and elevation gain.
        /// </summary>
        /// <param name="start">The point of start</param>
        /// <param name="end">The point of end</param>
        /// <returns>The distance between two points as a function of latitude and longitude difference and altitude gain. Via standard trig.</returns>
        private double DistanceBetweenTwoPoints(TcxPoint start, TcxPoint end)
        {
            double ascent = end.AltitudeInMetres - start.AltitudeInMetres;
            double x = this.Haversine(start, end);
            return Math.Sqrt((ascent * ascent) + (x * x));
        }

        /// <summary>
        /// Calculate the Haversine distance between a point of lat and long:
        /// <a href="http://www.movable-type.co.uk/scripts/latlong.html">Reference</a>
        /// </summary>
        /// <param name="start">Start Point</param>
        /// <param name="end">End Point</param>
        /// <returns>Double, the distance in meters as the crow flies.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Haversine is the right name for the forumla.")]
        private double Haversine(TcxPoint start, TcxPoint end)
        {
            double differenceInLat = this.ToRad(end.LatitudeInDegrees - start.LatitudeInDegrees);
            double differenceInLon = this.ToRad(end.LongitudeInDegrees - start.LongitudeInDegrees);

            double a = (Math.Sin(differenceInLat / 2) * Math.Sin(differenceInLat / 2))
                       +
                       (Math.Cos(this.ToRad(start.LatitudeInDegrees)) * Math.Cos(this.ToRad(end.LatitudeInDegrees))
                        * Math.Sin(differenceInLon / 2) * Math.Sin(differenceInLon / 2));
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = RadiusOfEarth * c;

            return d;
        }

        /// <summary>
        /// Convert degrees to radians.
        /// </summary>
        /// <param name="degrees">Degrees to convert.</param>
        /// <returns>Angle in radians.</returns>
        private double ToRad(double degrees)
        {
            return Math.PI * degrees / 180.0;
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
        private double WindForce(double speed, double altitude)
        {
            return 0.5 * this.reality.EffectiveFrontalArea * this.reality.DragCoefficient
                   * this.reality.AirDensity(altitude) * (speed * speed);
        }

        /// <summary>
        /// Calculate the force necessary to overcome the weight of the bike due
        /// to gravity given the gradient of the hill.
        /// </summary>
        /// <param name="gradient">Gradient as the ratio of ascent to distance.</param>
        /// <returns>Force required in Newtons.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here, Newtons is a word.")]
        private double HillForce(double gradient)
        {
            return this.wkg * this.reality.AccelerationDueToGravity * gradient;
        }

        /// <summary>
        /// Calculate the force required to accelerate from a start speed to an end speed.
        /// </summary>
        /// <param name="startSpeed">Speed the rider started at in m/s</param>
        /// <param name="endSpeed">Speed the rider ended at in m/s</param>
        /// <param name="time">Time taken in s</param>
        /// <returns>Force required in Newtons.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here, Newtons is a word.")]
        private double AccelerationPower(double startSpeed, double endSpeed, double time)
        {
            double acceleration = (endSpeed - startSpeed) / time;
            return this.wkg * acceleration;
        }

        /// <summary>
        /// Calculate the total power required to move from point a to point b.
        /// </summary>
        /// <param name="currentSpeed">Speed over segment at in m/s.</param>
        /// <param name="altitude">Altitude in m</param>
        /// <param name="gradient">Gradient of segment</param>
        /// <param name="time">Time taken in s</param>
        /// <param name="startSpeed">Speed at the start of segment in m/s</param>
        /// <returns>Power required to move in watts.</returns>
        private double TotalPower(double currentSpeed, double altitude, double gradient, double time, double startSpeed)
        {
            double totalPower = this.ForceRollingResistance;
            if (currentSpeed > startSpeed)
            {
                totalPower += this.AccelerationPower(startSpeed, currentSpeed, time);
            }

            totalPower += this.HillForce(gradient) + this.WindForce(currentSpeed, altitude);

            // "rollingpower,hillpower,windpower,accellerationpower,totalPower,wattage"
            this.Csv.AppendFormat(
                "{0},{1},{2},{3},{4},{5}",
                this.ForceRollingResistance,
                this.HillForce(gradient),
                this.WindForce(currentSpeed, altitude),
                this.AccelerationPower(startSpeed, currentSpeed, time),
                totalPower,
                totalPower * currentSpeed).AppendLine();

            return totalPower * currentSpeed;
        }
    }
}