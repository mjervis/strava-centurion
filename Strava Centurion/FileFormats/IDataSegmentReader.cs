// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataSegmentReader.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   An interface for a data segment reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion.FileFormats
{
    using System.Collections.Generic;

    /// <summary>
    /// An interface for a data segment reader.
    /// </summary>
    public interface IDataSegmentReader
    {
        /// <summary>
        /// A method to read data points.
        /// </summary>
        /// <returns>A list of data segments</returns>
        IEnumerable<DataSegment> Read();
    }
}
