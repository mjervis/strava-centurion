﻿// -----------------------------------------------------------------------
// <copyright file="TcxPoint.cs" company="FuckingBrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// -----------------------------------------------------------------------

namespace Strava_Centurion
{
    using System;
    using System.Globalization;
    using System.Xml;

    /// <summary>
    /// Represent a track point in a TCX file and methods for accessing it.
    /// </summary>
    public class TcxPoint
    {
        #region XPath Constants for parsing node.
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
        #endregion

        #region XML Objects
        /// <summary>
        /// Namespace manager for resolving xpath.
        /// </summary>
        private readonly XmlNamespaceManager nsmgr;

        /// <summary>
        /// Cached XmlNode for the node for updates to the XML.
        /// </summary>
        private readonly XmlNode sourceNode;
        #endregion

        /// <summary>
        /// Indicates if the TPX had source distance.
        /// </summary>
        private readonly bool noDistance = true;

        /// <summary>
        /// The power in watts.
        /// </summary>
        private double powerInWatts;

        /// <summary>
        /// Initializes a new instance of the <see cref="TcxPoint"/> class. 
        /// Create a track point from an XML track point node.
        /// </summary>
        /// <param name="node">
        /// XML Track point from a TCX file.
        /// </param>
        /// <param name="manager">
        /// The namespace manager to use for resolving queries.
        /// </param>
        public TcxPoint(XmlNode node, XmlNamespaceManager manager)
        {
            DateTime dateTime;
            double dble;
            this.nsmgr = manager;
            this.sourceNode = node;
            if (DateTime.TryParse(this.SafeGetNodeText(node, DatetimeXpath), out dateTime))
            {
                this.DateTime = dateTime;
            }

            if (double.TryParse(this.SafeGetNodeText(node, AltitudeXpath), out dble))
            {
                this.AltitudeInMetres = dble;
            }

            this.CadenceInRpm = this.SafeGetNodeText(node, CadenceXpath);

            if (double.TryParse(this.SafeGetNodeText(node, DistanceXpath), out dble))
            {
                this.DistanceInMetres = dble;
                this.noDistance = false;
            }
            else
            {
                this.noDistance = true;
            }

            this.HeartrateInBpm = this.SafeGetNodeText(node, HeartrateXpath);

            if (double.TryParse(this.SafeGetNodeText(node, LatitudeXpath), out dble))
            {
                this.LatitudeInDegrees = dble;
            }

            if (double.TryParse(this.SafeGetNodeText(node, LongitudeXpath), out dble))
            {
                this.LongitudeInDegrees = dble;
            }

            if (double.TryParse(this.SafeGetNodeText(node, SpeedXpath), out dble))
            {
                this.SpeedInKmPerHour = dble;
            }

            if (double.TryParse(this.SafeGetNodeText(node, PowerXPath), out dble))
            {
                this.powerInWatts = dble;
            }
        }

        #region Properties.    

        /// <summary>
        /// Gets the altitude in meters
        /// </summary>
        public double AltitudeInMetres { get; private set; }

        /// <summary>
        /// Gets the heart rate in beats per minute.
        /// </summary>
        public string HeartrateInBpm { get; private set; }

        /// <summary>
        /// Gets the cadence in revolutions per minute.
        /// </summary>
        public string CadenceInRpm { get; private set; }

        /// <summary>
        /// Gets or sets the power in watts.
        /// </summary>
        public double PowerInWatts
        {
            get
            {
                return this.powerInWatts;
            }

            set
            {
                this.powerInWatts = value;
                this.UpdatePowerNode();
            }
        }

        /// <summary>
        /// Gets the speed in kilometers per hour.
        /// </summary>
        public double SpeedInKmPerHour { get; private set; }

        /// <summary>
        /// Gets the distance in meters.
        /// </summary>
        public double DistanceInMetres { get; private set; }

        /// <summary>
        /// Gets or sets the date and time that the point was recorded
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the longitude in degrees.
        /// </summary>
        public double LongitudeInDegrees { get; set; }

        /// <summary>
        /// Gets or sets the latitude in degrees.
        /// </summary>
        public double LatitudeInDegrees { get; set; }

        /// <summary>
        /// Gets a value indicating whether the TPX had source distance.
        /// </summary>
        public bool NoDistance
        {
            get
            {
                return this.noDistance;
            }
        }
        #endregion

        /// <summary>
        /// Get's the contents of a node from a parent node via Xpath.
        /// </summary>
        /// <param name="node">XML node to find children of.</param>
        /// <param name="xpath">Child node to fetch.</param>
        /// <returns>The content of the node.</returns>
        private string SafeGetNodeText(XmlNode node, string xpath)
        {
            string ret = string.Empty;
            XmlNode subNode = node.SelectSingleNode(xpath, this.nsmgr);
            if (subNode != null)
            {
                ret = subNode.InnerText;
            }

            return ret;
        }

        /// <summary>
        /// Update or create a Watts element with the current power in watts into the XML object.
        /// </summary>
        private void UpdatePowerNode()
        {
            XmlNode powerNode = this.sourceNode.SelectSingleNode(PowerXPath, this.nsmgr);

            if (powerNode == null)
            {
                XmlNode tpxNode = this.GetTpxNode();
// ReSharper disable PossibleNullReferenceException - we always have an owner document here.
                powerNode = this.sourceNode.OwnerDocument.CreateElement(
// ReSharper restore PossibleNullReferenceException
                    "ns3", "Watts", "http://www.garmin.com/xmlschemas/ActivityExtension/v2");
                powerNode.InnerText = this.powerInWatts.ToString(CultureInfo.InvariantCulture);
                tpxNode.AppendChild(powerNode);
            }
            else
            {
                powerNode.InnerText = this.powerInWatts.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Get or create and return a TPX node.
        /// </summary>
        /// <returns>XmlNode for the TPX node in the document.</returns>
        private XmlNode GetTpxNode()
        {
            XmlNode tpxNode = this.sourceNode.SelectSingleNode(TpxXpath, this.nsmgr);
            if (tpxNode == null)
            {
                XmlNode extensionNode = this.GetExtensionsNode();
// ReSharper disable PossibleNullReferenceException - the node is from a document, so owner will never be null.
                tpxNode = this.sourceNode.OwnerDocument.CreateElement("ns3", "TPX", "http://www.garmin.com/xmlschemas/ActivityExtension/v2");
// ReSharper restore PossibleNullReferenceException
                extensionNode.AppendChild(tpxNode);
            }

            return tpxNode;
        }

        /// <summary>
        /// Get or create and return the Extensions node.
        /// </summary>
        /// <returns>XmlNode for the Extensions node in the document.</returns>
        private XmlNode GetExtensionsNode()
        {
            XmlNode extensionsNode = this.sourceNode.SelectSingleNode(ExtensionsXPath, this.nsmgr);
            if (extensionsNode == null)
            {
// ReSharper disable PossibleNullReferenceException - the node is from a document, so owner will never be null.
                extensionsNode = this.sourceNode.OwnerDocument.CreateElement("Extensions");
// ReSharper restore PossibleNullReferenceException
                this.sourceNode.AppendChild(extensionsNode);
            }

            return extensionsNode;
        }
    }
}
