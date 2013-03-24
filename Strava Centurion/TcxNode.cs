// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TcxNode.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   The tcx file node.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion
{
    using System;
    using System.Globalization;
    using System.Xml;

    /// <summary>
    /// The TCX file node.
    /// </summary>
    public class TcxNode : INode
    {
        /// <summary>
        /// Xpath expression to locate the date and time in a track point.
        /// </summary>
        private const string DatetimeXpath = "tcx:Time";

        /// <summary>
        /// Xpath expression for altitude.
        /// </summary>
        private const string AltitudeXpath = "tcx:AltitudeMeters";

        /// <summary>
        /// Xpath expression for cadence.
        /// </summary>
        private const string CadenceXpath = "tcx:Cadence";

        /// <summary>
        /// Xpath expression for distance.
        /// </summary>
        private const string DistanceXpath = "tcx:DistanceMeters";

        /// <summary>
        /// Xpath expression for heart rate.
        /// </summary>
        private const string HeartrateXpath = "tcx:HeartRateBpm/tcx:Value";

        /// <summary>
        /// Xpath expression for latitude.
        /// </summary>
        private const string LatitudeXpath = "tcx:Position/tcx:LatitudeDegrees";

        /// <summary>
        /// Xpath expression for longitude.
        /// </summary>
        private const string LongitudeXpath = "tcx:Position/tcx:LongitudeDegrees";

        /// <summary>
        /// Xpath expression for speed.
        /// </summary>
        private const string SpeedXpath = "tcx:Extensions/tcx:TPX/tcx:Speed";

        /// <summary>
        /// Xpath expression for power.
        /// </summary>
        private const string PowerXPath = "tcx:Extensions/ns3:TPX/ns3:Watts";

        /// <summary>
        /// Xpath expression for TPX.
        /// </summary>
        private const string TpxXpath = "tcx:Extensions/tcx:TPX";

        /// <summary>
        /// Xpath expression for Extensions.
        /// </summary>
        private const string ExtensionsXPath = "tcx:Extensions";

        /// <summary>
        /// Namespace manager for resolving xpath.
        /// </summary>
        private readonly XmlNamespaceManager namespaceManager;

        /// <summary>
        /// Cached XmlNode for the node for updates to the XML.
        /// </summary>
        private readonly XmlNode xmlNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="TcxNode"/> class. 
        /// Creates a track point from an XML track point node.
        /// </summary>
        /// <param name="xmlNode">
        /// XML Track point from a TCX file.
        /// </param>
        /// <param name="namespaceManager">
        /// The namespace manager to use for resolving queries.
        /// </param>
        public TcxNode(XmlNode xmlNode, XmlNamespaceManager namespaceManager)
        {
            this.xmlNode = xmlNode;
            this.namespaceManager = namespaceManager;
        }

        /// <summary>
        /// Gets the date and time of this node, all nodes must have a date
        /// and time so will throw an exception if one cannot be parsed.
        /// </summary>
        /// <returns>The date and time of this node.</returns>
        public DateTime GetDateTime()
        {
            return DateTime.Parse(this.SafeGetNodeText(DatetimeXpath));
        }

        /// <summary>
        /// Gets the altitude of this node (will return zero if the altitude
        /// cannot be parsed).
        /// </summary>
        /// <returns>The altitude of this node.</returns>
        public double GetAltitude()
        {
            double altitude;

            double.TryParse(this.SafeGetNodeText(AltitudeXpath), out altitude);

            return altitude;
        }

        /// <summary>
        /// Gets the cadence for this node.
        /// </summary>
        /// <returns>The cadence.</returns>
        public double GetCadence()
        {
            double cadence;

            if (double.TryParse(this.SafeGetNodeText(CadenceXpath), out cadence))
            {
                return cadence;
            }

            return 0;
        }

        /// <summary>
        /// Gets the total distance data.
        /// </summary>
        /// <returns>The total distance.</returns>
        public double GetTotalDistance()
        {
            double totalDistanceInMetres;

            if (double.TryParse(this.SafeGetNodeText(DistanceXpath), out totalDistanceInMetres))
            {
                return totalDistanceInMetres;
            }

            return double.NaN;
        }

        /// <summary>
        /// Gets the heart rate data.
        /// </summary>
        /// <returns>The heart rate.</returns>
        public string GetHeartrate()
        {
            return this.SafeGetNodeText(HeartrateXpath);
        }

        /// <summary>
        /// Gets the longitude data.
        /// </summary>
        /// <returns>The longitude.</returns>
        public double GetLongitude()
        {
            return double.Parse(this.SafeGetNodeText(LongitudeXpath));
        }

        /// <summary>
        /// Gets the latitude data.
        /// </summary>
        /// <returns>The latitude.</returns>
        public double GetLatitude()
        {
            return double.Parse(this.SafeGetNodeText(LatitudeXpath));
        }

        /// <summary>
        /// Gets the speed data (will return 0 if speed cannot be parsed).
        /// </summary>
        /// <returns>The speed.</returns>
        public double GetSpeed()
        {
            double speed;

            if (double.TryParse(this.SafeGetNodeText(SpeedXpath), out speed))
            {
                return speed;
            }

            return 0;
        }

        /// <summary>
        /// Gets the power data.
        /// </summary>
        /// <returns>The power.</returns>
        public double GetPower()
        {
            double power;

            if (double.TryParse(this.SafeGetNodeText(PowerXPath), out power))
            {
                return power;
            }

            return 0;
        }

        /// <summary>
        /// Update or create a Watts element with the current power in watts into the XML object.
        /// </summary>
        /// <param name="power">The updated power.</param>
        public void SetPower(double power)
        {
            var powerNode = this.xmlNode.SelectSingleNode(PowerXPath, this.namespaceManager);

            if (powerNode == null)
            {
                var tpxNode = this.GetTpxNode();

                var ownerDocument = this.xmlNode.OwnerDocument;
                if (ownerDocument != null)
                {
                    powerNode = ownerDocument.CreateElement("ns3", "Watts", "http://www.garmin.com/xmlschemas/ActivityExtension/v2");
                }

                if (powerNode != null)
                {
                    powerNode.InnerText = power.ToString(CultureInfo.InvariantCulture);

                    tpxNode.AppendChild(powerNode);
                }
            }
            else
            {
                powerNode.InnerText = power.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Get or create and return a TPX node.
        /// </summary>
        /// <returns>XmlNode for the TPX node in the document.</returns>
        private XmlNode GetTpxNode()
        {
            var tpxNode = this.xmlNode.SelectSingleNode(TpxXpath, this.namespaceManager);
            if (tpxNode == null)
            {
                var extensionNode = this.GetExtensionsNode();

                var ownerDocument = this.xmlNode.OwnerDocument;
                if (ownerDocument != null)
                {
                    tpxNode = ownerDocument.CreateElement("ns3", "TPX", "http://www.garmin.com/xmlschemas/ActivityExtension/v2");
                }

                if (tpxNode != null)
                {
                    extensionNode.AppendChild(tpxNode);
                }
            }

            return tpxNode;
        }

        /// <summary>
        /// Get or create and return the Extensions node.
        /// </summary>
        /// <returns>XmlNode for the Extensions node in the document.</returns>
        private XmlNode GetExtensionsNode()
        {
            var extensionsNode = this.xmlNode.SelectSingleNode(ExtensionsXPath, this.namespaceManager);
            if (extensionsNode == null)
            {
                var ownerDocument = this.xmlNode.OwnerDocument;
                if (ownerDocument != null)
                {
                    extensionsNode = ownerDocument.CreateElement("Extensions");
                }

                if (extensionsNode != null)
                {
                    this.xmlNode.AppendChild(extensionsNode);
                }
            }

            return extensionsNode;
        }

        /// <summary>
        /// Get's the contents of a node from a parent node via Xpath.
        /// </summary>
        /// <param name="xpath">Child node to fetch.</param>
        /// <returns>The content of the node.</returns>
        private string SafeGetNodeText(string xpath)
        {
            var result = string.Empty;

            var subNode = this.xmlNode.SelectSingleNode(xpath, this.namespaceManager);

            if (subNode != null)
            {
                result = subNode.InnerText;
            }

            return result;
        }
    }
}
