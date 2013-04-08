// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TcxDataSegmentWriter.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   The TCX data segment writer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturion.IO
{
    using System;
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
        /// <param name="gpsDeviceInfo">The GPS device information to write.</param>
        public void Write(List<DataSegment> dataSegments, GpsDeviceInfo gpsDeviceInfo)
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
                streamWriter.WriteLine("        <MaximumSpeed>" + dataSegments.Max(s => s.Speed.KilometresPerHour) + "</MaximumSpeed>");

                if (dataSegments.Any(s => !s.Heartrate.IsUnknown))
                {
                    streamWriter.WriteLine("        <AverageHeartRateBpm xsi:type=\"HeartRateInBeatsPerMinute_t\">");
                    streamWriter.WriteLine("          <Value>" + dataSegments.Where(s => !s.Heartrate.IsUnknown).Average(s => s.Heartrate.PerMinute) + "</Value>");
                    streamWriter.WriteLine("        </AverageHeartRateBpm>");
                    streamWriter.WriteLine("        <MaximumHeartRateBpm xsi:type=\"HeartRateInBeatsPerMinute_t\">");
                    streamWriter.WriteLine("          <Value>" + dataSegments.Where(s => !s.Heartrate.IsUnknown).Max(s => s.Heartrate.PerMinute) + "</Value>");
                    streamWriter.WriteLine("        </MaximumHeartRateBpm>");
                }

                streamWriter.WriteLine("        <Intensity>Active</Intensity>");

                if (dataSegments.Any(s => !s.Cadence.IsUnknown))
                {
                    streamWriter.WriteLine("        <Cadence>" + dataSegments.Where(s => !s.Cadence.IsUnknown).Average(s => s.Cadence.PerMinute) + "</Cadence>");
                }

                streamWriter.WriteLine("        <TriggerMethod>Manual</TriggerMethod>");
                streamWriter.WriteLine("        <Track>");

                foreach (var dataSegment in dataSegments)
                {
                    streamWriter.WriteLine("          <Trackpoint>");
                    streamWriter.WriteLine("            <Time>" + dataSegment.End.DateTime.ToString("s") + "Z</Time>");

                    if (!dataSegment.Latitude.IsUnknown || !dataSegment.Longitude.IsUnknown)
                    {
                        streamWriter.WriteLine("            <Position>");

                        if (!dataSegment.Latitude.IsUnknown)
                        {
                            streamWriter.WriteLine("              <LatitudeDegrees>" + dataSegment.End.Latitude.Degrees + "</LatitudeDegrees>");
                        }

                        if (!dataSegment.Longitude.IsUnknown)
                        {
                            streamWriter.WriteLine("              <LongitudeDegrees>" + dataSegment.End.Longitude.Degrees + "</LongitudeDegrees>");
                        }

                        streamWriter.WriteLine("            </Position>");
                    }

                    if (!dataSegment.Altitude.IsUnknown)
                    {
                        streamWriter.WriteLine("            <AltitudeMeters>" + dataSegment.Altitude.Metres + "</AltitudeMeters>");
                    }

                    if (!dataSegment.End.TotalDistance.IsUnknown)
                    {
                        streamWriter.WriteLine("            <DistanceMeters>" + dataSegment.End.TotalDistance.Metres + "</DistanceMeters>");
                    }

                    streamWriter.WriteLine("            <Extensions>");
                    streamWriter.WriteLine("              <TPX xmlns=\"http://www.garmin.com/xmlschemas/ActivityExtension/v2\">");
                   // streamWriter.WriteLine("                <Speed>" + dataSegment.Speed.KilometresPerHour + "</Speed>");
                    streamWriter.WriteLine("                <Watts>" + Math.Round(dataSegment.Power.Watts,0) + "</Watts>");
                    streamWriter.WriteLine("              </TPX>");
                    streamWriter.WriteLine("            </Extensions>");

                    if (!dataSegment.Heartrate.IsUnknown)
                    {
                        streamWriter.WriteLine("            <HeartRateBpm>");
                        streamWriter.WriteLine("              <Value>" + dataSegment.Heartrate.PerMinute + "</Value>");
                        streamWriter.WriteLine("            </HeartRateBpm>");
                    }

                    if (!dataSegment.Cadence.IsUnknown)
                    {
                        streamWriter.WriteLine("            <Cadence>" + dataSegment.Cadence.PerMinute + "</Cadence>");
                    }

                    streamWriter.WriteLine("          </Trackpoint>");    
                }

                streamWriter.WriteLine("        </Track>");
                streamWriter.WriteLine("      </Lap>");
                streamWriter.WriteLine("      <Creator xsi:type=\"Device_t\">");
                streamWriter.WriteLine("        <Name>{0}</Name>", gpsDeviceInfo.Name);
                streamWriter.WriteLine("        <UnitId>{0}</UnitId>", gpsDeviceInfo.UnitId);
                streamWriter.WriteLine("        <ProductID>{0}</ProductID>", gpsDeviceInfo.ProductId);
                streamWriter.WriteLine("        <Version>");
                streamWriter.WriteLine("          <VersionMajor>{0}</VersionMajor>", gpsDeviceInfo.FirmwareVersion.Major);
                streamWriter.WriteLine("          <VersionMinor>{0}</VersionMinor>", gpsDeviceInfo.FirmwareVersion.Minor);
                streamWriter.WriteLine("          <BuildMajor>{0}</BuildMajor>", gpsDeviceInfo.FirmwareVersion.Build);
                streamWriter.WriteLine("          <BuildMinor>{0}</BuildMinor>", gpsDeviceInfo.FirmwareVersion.Revision);
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