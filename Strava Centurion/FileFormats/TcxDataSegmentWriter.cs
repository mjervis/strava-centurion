// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TcxDataSegmentWriter.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   The TCX data segment writer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion.FileFormats
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The TCX data segment writer.
    /// </summary>
    public class TcxDataSegmentWriter : IDataSegmentWriter
    {
        /// <summary>
        /// The stream used to write the TCX file.
        /// </summary>
        private readonly Stream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="TcxDataSegmentWriter"/> class.
        /// </summary>
        /// <param name="stream">The stream used to write the TCX file.</param>
        public TcxDataSegmentWriter(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// A method to write out data points.
        /// </summary>
        /// <param name="dataSegments">The data segments to write.</param>
        public void Write(List<DataSegment> dataSegments)
        {
            using (var streamWriter = new StreamWriter(this.stream))
            {
                // TODO: This is all just a quick hack so the tool still works, given all of the refactoring.
                // TODO: This really needs to use XmlDocument instead like the reader....
                // TODO: Also... we need to use averages, not the .End point for stuff.
                streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                streamWriter.WriteLine("<TrainingCenterDatabase xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2\">");
                streamWriter.WriteLine("  <Activities>");
                streamWriter.WriteLine("    <Activity Sport=\"Biking\">");
                streamWriter.WriteLine("      <Id>" + dataSegments.First().End.DateTime.ToString("s") + "Z</Id>");
                streamWriter.WriteLine("      <Lap StartTime=\"" + dataSegments.First().End.DateTime.ToString("s") + "Z\">");
                streamWriter.WriteLine("        <TotalTimeSeconds>" + (dataSegments.Last().End.DateTime - dataSegments.First().End.DateTime).TotalSeconds + "</TotalTimeSeconds>");
                streamWriter.WriteLine("        <DistanceMeters>" + dataSegments.Sum(s => s.Length) + "</DistanceMeters>");
                streamWriter.WriteLine("        <MaximumSpeed>" + dataSegments.Max(s => s.Speed.KmHour) + "</MaximumSpeed>");
                /* <Calories>3438</Calories> */
                streamWriter.WriteLine("        <AverageHeartRateBpm xsi:type=\"HeartRateInBeatsPerMinute_t\">");
                streamWriter.WriteLine("          <Value>" + dataSegments.Average(s => s.End.HeartrateInBpm) + "</Value>");
                streamWriter.WriteLine("        </AverageHeartRateBpm>");
                streamWriter.WriteLine("        <MaximumHeartRateBpm xsi:type=\"HeartRateInBeatsPerMinute_t\">");
                streamWriter.WriteLine("          <Value>" + dataSegments.Max(s => s.End.HeartrateInBpm) + "</Value>");
                streamWriter.WriteLine("        </MaximumHeartRateBpm>");
                streamWriter.WriteLine("        <Intensity>Active</Intensity>");
                streamWriter.WriteLine("        <Cadence>" + dataSegments.Average(s => s.Cadence) + "</Cadence>");
                streamWriter.WriteLine("        <TriggerMethod>Manual</TriggerMethod>");
                streamWriter.WriteLine("        <Track>");

                foreach (var dataSegment in dataSegments)
                {
                    streamWriter.WriteLine("          <Trackpoint>");
                    streamWriter.WriteLine("            <Time>" + dataSegment.End.DateTime.ToString("s") + "Z</Time>");
                    streamWriter.WriteLine("            <Position>");
                    streamWriter.WriteLine("              <LatitudeDegrees>" + dataSegment.End.Latitude.Degrees + "</LatitudeDegrees>");
                    streamWriter.WriteLine("              <LongitudeDegrees>" + dataSegment.End.Longitude.Degrees + "</LongitudeDegrees>");
                    streamWriter.WriteLine("            </Position>");
                    streamWriter.WriteLine("            <AltitudeMeters>" + dataSegment.End.Altitude.Metres + "</AltitudeMeters>");
                    streamWriter.WriteLine("            <DistanceMeters>" + dataSegment.End.TotalDistance.Metres + "</DistanceMeters>");
                    streamWriter.WriteLine("            <Extensions>");
                    streamWriter.WriteLine("              <TPX xmlns=\"http://www.garmin.com/xmlschemas/ActivityExtension/v2\">");
                    streamWriter.WriteLine("                <Speed>" + dataSegment.Speed.KmHour + "</Speed>");
                    streamWriter.WriteLine("                <Watts>" + dataSegment.Power.Watts + "</Watts>");
                    streamWriter.WriteLine("              </TPX>");
                    streamWriter.WriteLine("            </Extensions>");
                    streamWriter.WriteLine("            <HeartRateBpm>");
                    streamWriter.WriteLine("              <Value>" + dataSegment.End.HeartrateInBpm + "</Value>");
                    streamWriter.WriteLine("            </HeartRateBpm>");
                    streamWriter.WriteLine("            <Cadence>" + dataSegment.Cadence + "</Cadence>");
                    streamWriter.WriteLine("          </Trackpoint>");    
                }

                streamWriter.WriteLine("        </Track>");
                streamWriter.WriteLine("      </Lap>");
                streamWriter.WriteLine("      <Creator xsi:type=\"Device_t\">");
                streamWriter.WriteLine("        <Name>Edge 800 (Unit ID 3845823864)</Name>");
                streamWriter.WriteLine("        <UnitId>3845823864</UnitId>");
                streamWriter.WriteLine("        <ProductID>1169</ProductID>");
                streamWriter.WriteLine("        <Version>");
                streamWriter.WriteLine("          <VersionMajor>2</VersionMajor>");
                streamWriter.WriteLine("          <VersionMinor>40</VersionMinor>");
                streamWriter.WriteLine("          <BuildMajor>0</BuildMajor>");
                streamWriter.WriteLine("          <BuildMinor>0</BuildMinor>");
                streamWriter.WriteLine("        </Version>");
                streamWriter.WriteLine("      </Creator>");
                streamWriter.WriteLine("    </Activity>");
                streamWriter.WriteLine("  </Activities>");
                streamWriter.WriteLine("</TrainingCenterDatabase>");
            }
        }

        /// <summary>
        /// Disposes of this reader.
        /// </summary>
        public void Dispose()
        {
        }
    }
}