// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TcxDataSegmentReader.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   The TCX data segment reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturion.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    using StravaCenturion.Units;

    /// <summary>
    /// The TCX data segment reader.
    /// </summary>
    public class TcxDataSegmentReader : IDataSegmentReader
    {
        /// <summary>Xpath to find ride nodes.</summary>
        private const string NodesXpath = "//tcx:Trackpoint";

        /// <summary>Xpath expression to locate the date and time in a track point.</summary>
        private const string DatetimeXpath = "tcx:Time";

        /// <summary>Xpath expression for altitude.</summary>
        private const string AltitudeXpath = "tcx:AltitudeMeters";

        /// <summary>Xpath expression for cadence.</summary>
        private const string CadenceXpath = "tcx:Cadence";

        /// <summary>Xpath expression for distance.</summary>
        private const string DistanceXpath = "tcx:DistanceMeters";

        /// <summary>Xpath expression for heart rate.</summary>
        private const string HeartrateXpath = "tcx:HeartRateBpm/tcx:Value";

        /// <summary>Xpath expression for latitude.</summary>
        private const string LatitudeXpath = "tcx:Position/tcx:LatitudeDegrees";

        /// <summary>Xpath expression for longitude.</summary>
        private const string LongitudeXpath = "tcx:Position/tcx:LongitudeDegrees";

        /// <summary>Xpath expression for speed.</summary>
        private const string SpeedXpath = "tcx:Extensions/ext:TPX/ext:Speed";

        /// <summary>
        /// The stream used to read the file.
        /// </summary>
        private readonly Stream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="TcxDataSegmentReader"/> class. 
        /// </summary>
        /// <param name="stream">The stream used to read the data.</param>
        public TcxDataSegmentReader(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// A method to read data points.
        /// </summary>
        /// <returns>A list of data segments</returns>
        public List<DataSegment> Read()
        {
            var dataSegments = new List<DataSegment>();

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(this.stream);

            var xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            xmlNamespaceManager.AddNamespace("tcx", "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2");
            xmlNamespaceManager.AddNamespace("ext", "http://www.garmin.com/xmlschemas/ActivityExtension/v2");

            var dataPoints = this.GetDataPoints(xmlDocument, xmlNamespaceManager);

            for (var speedLoop = 0; speedLoop < dataPoints.Count; speedLoop++ )
            {
                if (dataPoints[speedLoop].Speed.IsUnknown && (speedLoop == 0 || speedLoop == dataPoints.Count -1))
                {
                    dataPoints[speedLoop].Speed = new Speed(0.0);
                }
                else
                {
                    if (dataPoints[speedLoop].Speed.IsUnknown)
                    {
                        Distance distance = dataPoints[speedLoop].TotalDistance
                                            - dataPoints[speedLoop - 1].TotalDistance;
                        double time =
                            dataPoints[speedLoop].DateTime.Subtract(dataPoints[speedLoop - 1].DateTime).TotalSeconds;
                        var speed = distance.Metres / time;
                        dataPoints[speedLoop].Speed = new Speed(speed);
                    }
                }
                if(dataPoints[speedLoop].Speed.IsUnknown)
                {
                    dataPoints[speedLoop].Speed = new Speed(0.0);
                }
            }

            //ToDo: make this configurable from reality tab?
            const int chunkSize = 4;
            for (var loop = chunkSize; loop < dataPoints.Count - chunkSize; loop++)
            {
                dataSegments.Add(new DataSegment(dataPoints[loop - chunkSize], dataPoints[loop + chunkSize]));
            }

            return dataSegments;
        }

        /// <summary>
        /// Disposes of this reader.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Gets the data points from an xml document.
        /// </summary>
        /// <param name="xmlNode">The xml node.</param>
        /// <param name="xmlNamespaceManager">The xml namespace manager.</param>
        /// <returns>A list of data points.</returns>
        private List<DataPoint> GetDataPoints(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager)
        {
            var dataPoints = new List<DataPoint>();

            var nodes = xmlNode.SelectNodes(NodesXpath, xmlNamespaceManager);

            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    var dateTime = this.GetDateTimeNode(node, xmlNamespaceManager, DatetimeXpath);
                    var altitude = this.GetDistanceNode(node, xmlNamespaceManager, AltitudeXpath);
                    var cadence = this.GetFrequencyNode(node, xmlNamespaceManager, CadenceXpath);
                    var totalDistance = this.GetDistanceNode(node, xmlNamespaceManager, DistanceXpath);
                    var speed = this.GetSpeedNode(node, xmlNamespaceManager, SpeedXpath);
                    var heartrateInBpm = this.GetFrequencyNode(node, xmlNamespaceManager, HeartrateXpath);
                    var latitude = this.GetAngleNode(node, xmlNamespaceManager, LatitudeXpath);
                    var longitude = this.GetAngleNode(node, xmlNamespaceManager, LongitudeXpath);

                    var dataPoint = new DataPoint(dateTime, altitude, cadence, totalDistance, speed, heartrateInBpm, latitude, longitude);

                    dataPoints.Add(dataPoint);
                }
            }

            return dataPoints;
        }

        /// <summary>
        /// Gets an integer from an xml node using xpath.
        /// </summary>
        /// <param name="xmlNode">The xml node.</param>
        /// <param name="xmlNamespaceManager">The xml namespace manager.</param>
        /// <param name="xpath">The xpath to the node.</param>
        /// <returns>The integer content of the node.</returns>
        private Frequency GetFrequencyNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager, string xpath)
        {
            var subNode = xmlNode.SelectSingleNode(xpath, xmlNamespaceManager);

            if (subNode == null)
            {
                return Frequency.Unknown;
            }

            return new Frequency(int.Parse(subNode.InnerText));
        }

        /// <summary>
        /// Gets a speed from an xml node using xpath.
        /// </summary>
        /// <param name="xmlNode">The xml node.</param>
        /// <param name="xmlNamespaceManager">The xml namespace manager.</param>
        /// <param name="xpath">The xpath to the node.</param>
        /// <returns>The speed content of the node.</returns>
        private Speed GetSpeedNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager, string xpath)
        {
            var subNode = xmlNode.SelectSingleNode(xpath, xmlNamespaceManager);

            if (subNode == null)
            {
                return Speed.Unknown;
            }

            return new Speed(double.Parse(subNode.InnerText));
        }

        /// <summary>
        /// Gets an angle from an xml node using xpath.
        /// </summary>
        /// <param name="xmlNode">The xml node.</param>
        /// <param name="xmlNamespaceManager">The xml namespace manager.</param>
        /// <param name="xpath">The xpath to the node.</param>
        /// <returns>The angle content of the node.</returns>
        private Angle GetAngleNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager, string xpath)
        {
            var subNode = xmlNode.SelectSingleNode(xpath, xmlNamespaceManager);

            if (subNode == null)
            {
                return Angle.Unknown;
            }

            return Angle.FromDegrees(double.Parse(subNode.InnerText));
        }

        /// <summary>
        /// Gets the date time from an xml node using xpath.
        /// </summary>
        /// <param name="xmlNode">The xml node.</param>
        /// <param name="xmlNamespaceManager">The xml namespace manager.</param>
        /// <param name="xpath">The xpath to the node.</param>
        /// <returns>The date and time content of the node.</returns>
        private DateTime GetDateTimeNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager, string xpath)
        {
            var subNode = xmlNode.SelectSingleNode(xpath, xmlNamespaceManager);

            if (subNode == null)
            {
                throw new ArgumentOutOfRangeException("xpath");
            }

            return DateTime.Parse(subNode.InnerText);
        }

        /// <summary>
        /// Gets the distance from an xml node using xpath.
        /// </summary>
        /// <param name="xmlNode">The xml node.</param>
        /// <param name="xmlNamespaceManager">The xml namespace manager.</param>
        /// <param name="xpath">The xpath to the node.</param>
        /// <returns>The distance content of the node.</returns>
        private Distance GetDistanceNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager, string xpath)
        {
            var subNode = xmlNode.SelectSingleNode(xpath, xmlNamespaceManager);

            if (subNode == null)
            {
                return Distance.Unknown;
            }

            return new Distance(double.Parse(subNode.InnerText));
        }
    }
}