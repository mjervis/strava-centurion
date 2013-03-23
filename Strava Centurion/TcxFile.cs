// -----------------------------------------------------------------------
// <copyright file="TcxFile.cs" company="FuckingBrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// -----------------------------------------------------------------------

namespace Strava_Centurion
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Represents a Garmin Training Centre XML file (TCX).
    /// </summary>
    public class TcxFile
    {
        /// <summary>
        /// Xpath to find ride nodes.
        /// </summary>
        private const string NodesXpath = "//tcx:Trackpoint";

        /// <summary>
        /// Path to the source TCX file.
        /// </summary>
        private readonly string sourceFileName = string.Empty;

        /// <summary>
        /// Parsed XML version of the file.
        /// </summary>
        private XmlDocument xmlDocument;

        /// <summary>
        /// XmlNamespaceManager for resolving xpath queries.
        /// </summary>
        private XmlNamespaceManager xmlNamespaceManager;

        /// <summary>
        /// List of track points in the TCX file.
        /// </summary>
        private List<TcxPoint> trackPoints = new List<TcxPoint>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TcxFile"/> class. 
        /// Open a TCX file loading as an XML DOM.
        /// </summary>
        /// <param name="path">
        /// valid path to a TCX file
        /// </param>
        public TcxFile(string path)
        {
            this.sourceFileName = path;
            this.LoadXml();
            this.IterateNodes();
        }

        /// <summary>
        /// Gets or sets the list of track points in the TCX file.
        /// </summary>
        public List<TcxPoint> TrackPoints
        {
            get
            {
                return this.trackPoints;
            }

            set
            {
                this.trackPoints = value;
            }
        }

        /// <summary>
        /// Process a TCX file out to a new file.
        /// </summary>
        /// <param name="path">
        /// The path to save to.
        /// </param>
        public void SaveAs(string path)
        {
            this.xmlDocument.Save(path);
        }

        /// <summary>
        /// Load the XML into the DOM that represents the track.
        /// </summary>
        /// <exception cref="FileNotFoundException">
        /// Thrown if the file doesn't exist.
        /// </exception>
        private void LoadXml()
        {
            if (!File.Exists(this.sourceFileName))
            {
                throw new FileNotFoundException("Invalid path.");
            }

            this.xmlDocument = new XmlDocument();
            this.xmlDocument.Load(this.sourceFileName);
            this.xmlNamespaceManager = new XmlNamespaceManager(this.xmlDocument.NameTable);
            this.xmlNamespaceManager.AddNamespace("tcx", "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2");
            this.xmlNamespaceManager.AddNamespace("ns3", "http://www.garmin.com/xmlschemas/ActivityExtension/v2");
        }

        /// <summary>
        /// Loop over each node of data capture, caching as a TCXPoint in the Track Points list.
        /// </summary>
        private void IterateNodes()
        {
            XmlNodeList nodes = this.xmlDocument.SelectNodes(NodesXpath, this.xmlNamespaceManager);
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    var tcxPoint = new TcxPoint(node, this.xmlNamespaceManager);
                    this.TrackPoints.Add(tcxPoint);
                }
            }
        }
    }
}
