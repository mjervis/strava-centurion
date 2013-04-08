// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvDataSegmentWriter.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   A data segment writer for a comma separated file.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace StravaCenturion.IO
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// A data segment writer for a comma separated file.
    /// </summary>
    public class CsvDataSegmentWriter : IDataSegmentWriter
    {
        /// <summary>
        /// The stream used for writing out the data points.
        /// </summary>
        private readonly Stream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataSegmentWriter"/> class. 
        /// </summary>
        /// <param name="stream">The stream used to write the data points.</param>
        public CsvDataSegmentWriter(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// A method to write out data points.
        /// </summary>
        /// <param name="dataSegments">The data segments to write.</param>
        /// <param name="gpsDeviceInfo">The GPS Device Info to write.</param>
        public void Write(List<DataSegment> dataSegments, GpsDeviceInfo gpsDeviceInfo)
        {
            using (var streamWriter = new StreamWriter(this.stream))
            {
                streamWriter.WriteLine("distance,gradient,time,speed,rollingpower,hillpower,windpower,accellerationpower,totalPower,wattage");

                foreach (var segment in dataSegments)
                {
                    streamWriter.WriteLine(string.Join(
                        ",",
                        segment.Length.Metres,
                        segment.Gradient,
                        segment.ElapsedTime,
                        segment.Speed.MetresPerSecond,
                        segment.RollingResistanceForce.Newtons,
                        segment.HillForce.Newtons,
                        segment.WindForce.Newtons,
                        segment.AccelerationForce.Newtons,
                        segment.TotalForce.Newtons,
                        segment.Power.Watts));
                }
            }
        }

        /// <summary>
        /// Disposes of this data segment writer.
        /// </summary>
        public void Dispose()
        {
        }

    }
}