// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSegment.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   Defines the DataSegment type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion
{
    using System;

    /// <summary>
    /// A segment between two data points.
    /// </summary>
    public class DataSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSegment"/> class. 
        /// </summary>
        /// <param name="start">The starting point of the segment.</param>
        /// <param name="end">The end point of the segment.</param>
        public DataSegment(DataPoint start, DataPoint end)
        {
            this.Start = start;
            this.End = end;
            if ((end.Speed.MetersPerSecond == 0.0) && (this.Speed.MetersPerSecond > 0.0))
            {
                end.Speed = this.Speed;   
            }
        }

        /// <summary>
        /// Gets the start point of the segment.
        /// </summary>
        public DataPoint Start { get; private set; }

        /// <summary>
        /// Gets the end point of the segment.
        /// </summary>
        public DataPoint End { get; private set; }

        /// <summary>
        /// Gets the distance between the start and end point of this segment.
        /// </summary>
        public Distance Distance
        {
            get
            {
                return this.Start.DistanceToPoint(this.End);
            }
        }

        /// <summary>
        /// Gets the gradient of slope between the start and end point of this segment.
        /// </summary>
        public double Gradient
        {
            get
            {
                return this.Start.GradientToPoint(this.End);
            }
        }

        /// <summary>
        /// Gets the time elapsed between the recording of the start and end points of the segment.
        /// </summary>
        public double ElapsedTime
        {
            get
            {
                return this.End.DateTime.Subtract(this.Start.DateTime).TotalSeconds;
            }
        }

        /// <summary>
        /// Gets the speed along the segment.
        /// </summary>
        public Speed Speed
        {
            get
            {
                var speed = new Speed(0.0);
                
                if (Math.Abs(this.Distance - 0.0) > 0.0)
                {
                    speed = new Speed(this.Distance / this.ElapsedTime);    
                }

                return speed;
            }
        }

        /// <summary>
        /// Gets the average cadence along the segment.
        /// </summary>
        public double Cadence
        {
            get
            {
                return (this.Start.CadenceInRpm + this.End.CadenceInRpm) / 2;
            }
        }

        /// <summary>
        /// Gets the acceleration between the start and end points of the segment in m/s/s.
        /// </summary>
        public double Acceleration
        {
            get
            {
                return (this.End.Speed.MetersPerSecond - this.Start.Speed.MetersPerSecond) / this.ElapsedTime;       
            }
        }

        // TODO: could some of these properties have guard added to them...
        // TODO:   can rolling resistance ever be a negative power?
        // TODO:   can acceleration ever be negative power for a positive acceleration, and vice-versa
        // TODO:   can power ever be negative for a positive gradient, and vice-versa

        /// <summary>
        /// Gets or sets the rolling resistance force.
        /// </summary>
        public Force RollingResistanceForce { get; set; }

        /// <summary>
        /// Gets or sets the acceleration force.
        /// </summary>
        public Force AccelerationForce { get; set; }

        /// <summary>
        /// Gets or sets the hill force.
        /// </summary>
        public Force HillForce { get; set; }

        /// <summary>
        /// Gets or sets the wind force.
        /// </summary>
        public Force WindForce { get; set; }

        /// <summary>
        /// Gets the total force.
        /// </summary>
        public Force TotalForce
        {
            get
            {
                return new Force(Math.Max(0.0, this.RollingResistanceForce + this.AccelerationForce + this.HillForce + this.WindForce));
            }
        }
    }
}
