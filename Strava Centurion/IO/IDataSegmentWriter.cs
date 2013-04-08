// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataSegmentWriter.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   An interface for writing out data segments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturion.IO
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An interface for writing out data points.
    /// </summary>
    public interface IDataSegmentWriter : IDisposable
    {
        /// <summary>
        /// A method to write out data points.
        /// </summary>
        /// <param name="dataSegments">The data segments to write.</param>
        /// <param name="gpsDeviceInfo">The GPS Device Info to write.</param>
        void Write(List<DataSegment> dataSegments, GpsDeviceInfo gpsDeviceInfo);
    }
}
