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
                return new Speed(this.Distance / this.ElapsedTime);
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
    }
}
