// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataSegmentWriter.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   An interface for writing out data segments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion.FileFormats
{
    using System.Collections.Generic;

    /// <summary>
    /// An interface for writing out data points.
    /// </summary>
    public interface IDataSegmentWriter
    {
        /// <summary>
        /// A method to write out data points.
        /// </summary>
        /// <param name="dataSegments">The data segments to write.</param>
        void Write(IEnumerable<DataSegment> dataSegments);
    }
}
