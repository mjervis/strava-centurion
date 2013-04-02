// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSegment.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   Defines the DataSegment type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturion
{
    using System;

    using StravaCenturion.Units;

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

            this.WindForce = Force.Zero;
            this.AccelerationForce = Force.Zero;
            this.HillForce = Force.Zero;
            this.RollingResistanceForce = Force.Zero;
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
        /// Gets the length of this segment
        /// </summary>
        public Distance Length
        {
            get
            {
                return new Distance(Math.Abs(this.Start.DistanceToPoint(this.End).Metres));
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
                if (this.Start.Speed.IsUnknown || this.End.Speed.IsUnknown)
                {
                    if (this.Length <= 0.0 || this.ElapsedTime <= 0.0)
                    {
                        return new Speed(0.0);
                    }

                    return new Speed(this.Length / this.ElapsedTime);
                }

                return (this.Start.Speed + this.End.Speed) / 2.0;
            }
        }

        /// <summary>
        /// Gets the latitude of this point.
        /// </summary>
        public Angle Latitude
        {
            get
            {
                if (this.Start.Latitude.IsUnknown || this.End.Latitude.IsUnknown)
                {
                    return Angle.Unknown;
                }

                return (this.Start.Latitude + this.End.Latitude) / 2.0;
            }
        }

        /// <summary>
        /// Gets the longitude of this point.
        /// </summary>
        public Angle Longitude
        {
            get
            {
                if (this.Start.Longitude.IsUnknown || this.End.Longitude.IsUnknown)
                {
                    return Angle.Unknown;
                }

                return (this.Start.Longitude + this.End.Longitude) / 2.0;
            }
        }

        /// <summary>
        /// Gets the altitude of this segment.
        /// </summary>
        public Distance Altitude
        {
            get
            {
                if (this.Start.Altitude.IsUnknown || this.End.Altitude.IsUnknown)
                {
                    return Distance.Unknown;
                }

                return (this.Start.Altitude + this.End.Altitude) / 2.0;
            }
        }

        /// <summary>
        /// Gets the average cadence along the segment.
        /// </summary>
        public Frequency Cadence
        {
            get
            {
                if (this.Start.Cadence.IsUnknown)
                {
                    if (this.End.Cadence.IsUnknown)
                    {
                        return Frequency.Unknown;
                    }

                    return this.End.Cadence;
                }

                if (this.End.Cadence.IsUnknown)
                {
                    return this.Start.Cadence;
                }

                return (this.Start.Cadence + this.End.Cadence) / 2.0;
            }
        }

        /// <summary>
        /// Gets the average heart rate along the segment.
        /// </summary>
        public Frequency Heartrate
        {
            get
            {
                if (this.Start.Heartrate.IsUnknown)
                {
                    if (this.End.Heartrate.IsUnknown)
                    {
                        return Frequency.Unknown;
                    }

                    return this.End.Heartrate;
                }

                if (this.End.Heartrate.IsUnknown)
                {
                    return this.Start.Heartrate;
                }

                return (this.Start.Heartrate + this.End.Heartrate) / 2.0;
            }
        }

        /// <summary>
        /// Gets the acceleration between the start and end points of the segment in m/s/s.
        /// </summary>
        public double Acceleration
        {
            get
            {
                if (this.Start.Speed.IsUnknown || this.End.Speed.IsUnknown)
                {
                    throw new Exception("TODO");
                }

                return (this.End.Speed.MetresPerSecond - this.Start.Speed.MetresPerSecond) / this.ElapsedTime;
            }
        }

        /// <summary>
        /// Gets the rolling resistance force.
        /// </summary>
        public Force RollingResistanceForce { get; private set; }

        /// <summary>
        /// Gets the acceleration force.
        /// </summary>
        public Force AccelerationForce { get; private set; }

        /// <summary>
        /// Gets the hill force.
        /// </summary>
        public Force HillForce { get; private set; }

        /// <summary>
        /// Gets the wind force.
        /// </summary>
        public Force WindForce { get; private set; }

        /// <summary>
        /// Gets the total force.
        /// </summary>
        public Force TotalForce
        {
            get
            {
                return new Force(Math.Max(0.0, this.WindForce + this.AccelerationForce + this.HillForce + this.RollingResistanceForce));
            }
        }

        /// <summary>
        /// Gets the power.
        /// </summary>
        public Power Power
        {
            get
            {
                return new Power(Math.Max(0.0, this.TotalForce * this.Speed.MetresPerSecond));
            }
        }

        /// <summary>
        /// Calculates the forces based on a rider and reality.
        /// </summary>
        /// <param name="rider">The rider.</param>
        /// <param name="reality">The reality.</param>
        public void Calculate(Rider rider, Reality reality)
        {
            this.RollingResistanceForce = this.GetRollingResistanceForce(rider, reality);
            this.AccelerationForce = this.GetAccelerationForce(rider);
            this.HillForce = this.GetHillForce(rider, reality);
            this.WindForce = this.GetWindForce(reality);
        }

        // TODO: could some of these methods have a guard added to them...
        // TODO:   can rolling resistance ever be a negative power?
        // TODO:   can acceleration ever be negative power for a positive acceleration, and vice-versa
        // TODO:   can power ever be negative for a positive gradient, and vice-versa

        /// <summary>
        /// Gets the rolling resistance force.
        /// </summary>
        /// <param name="rider">The rider.</param>
        /// <param name="reality">The reality.</param>
        /// <returns>A force.</returns>
        private Force GetRollingResistanceForce(Rider rider, Reality reality)
        {
            return new Force(rider.MassIncludingBike * reality.AccelerationDueToGravity * reality.CoefficientOfRollingResistance);
        }

        /// <summary>
        /// Gets the acceleration force.
        /// </summary>
        /// <param name="rider">The rider.</param>
        /// <returns>A force.</returns>
        private Force GetAccelerationForce(Rider rider)
        {
            return new Force(rider.MassIncludingBike * this.Acceleration);
        }

        /// <summary>
        /// Gets or sets the hill force.
        /// </summary>
        /// <param name="rider">The rider.</param>
        /// <param name="reality">The reality.</param>
        /// <returns>A force.</returns>
        private Force GetHillForce(Rider rider, Reality reality)
        {
            return new Force(rider.MassIncludingBike * reality.AccelerationDueToGravity * this.Gradient);
        }

        /// <summary>
        /// Gets or sets the wind force.
        /// </summary>
        /// <param name="reality">The reality.</param>
        /// <returns>A force.</returns>
        private Force GetWindForce(Reality reality)
        {
            return new Force(0.5 * reality.EffectiveFrontalArea * reality.DragCoefficient * reality.AirDensity(this.End.Altitude) * (this.Speed.MetresPerSecond * this.Speed.MetresPerSecond));
        }
    }
}
