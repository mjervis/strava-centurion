// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TcxDeviceInfoReader.cs" company="Xellepher">
//   Copyright (c) 2013 Xellepher
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// <summary>
//   Class to read GPS Device Infomation from a tcx filestream
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturion
{
    using System;
    using System.IO;
    using System.Xml;
    
    /// <summary>
    /// Provides functionality to read the GPS Device Infomation from a tcx file.
    /// </summary>
    public class TcxDeviceInfoReader : IDisposable
    {
        /// <summary>Xpath to find device information nodes.</summary>
        private const string NodesXpath = "//tcx:Creator";
        private const string NameXpath = "tcx:Name";
        private const string UnitIdXpath = "tcx:UnitId";
        private const string ProductIdXpath = "tcx:ProductID";
        private const string FirmwareVersionXPathVersionMajor = "tcx:Version/tcx:VersionMajor";
        private const string FirmwareVersionXPathVersionMinor = "tcx:Version/tcx:VersionMinor";
        private const string FirmwareVersionXPathBuildMajor = "tcx:Version/tcx:BuildMajor";
        private const string FirmwareVersionXPathBuildMinor = "tcx:Version/tcx:BuildMinor";

        private readonly FileStream _stream;

        /// <summary>
        /// Provides functionality to read the GPS Device Infomation from a tcx file.
        /// </summary>
        /// <param name="filepath">A filepath to a tcx file containing GPS Device Info</param>
        public TcxDeviceInfoReader(string filepath)
        {
            //open a new filestream using the specified file path.
            //todo: oops, no error handling.
            _stream = new FileStream(filepath, FileMode.Open);
        }

        /// <summary>
        /// Disposes of this reader.
        /// </summary>
        public void Dispose()
        {
            _stream.Close();
        }

        /// <summary>
        /// Read the GpsDeviceInfo from the file stream
        /// </summary>
        /// <returns></returns>
        public GpsDeviceInfo Read()
        {
            if(!_stream.CanRead)
            {
                throw new ApplicationException("Unable to read from file stream");
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(_stream);

            var xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            xmlNamespaceManager.AddNamespace("tcx", "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2");
            xmlNamespaceManager.AddNamespace("ext", "http://www.garmin.com/xmlschemas/ActivityExtension/v2");

            var info = GetDeviceInfo(xmlDocument, xmlNamespaceManager);

            return info;
        }


        /// <summary>
        /// Gets the gpsDeviceInfo from an xml document.
        /// </summary>
        /// <param name="xmlNode">The xml node of the document</param>
        /// <param name="xmlNamespaceManager">The xml namespace manager.</param>
        /// <returns>A populated <code>GpsDeviceInfo</code> object.</returns>
        private static GpsDeviceInfo GetDeviceInfo(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager)
        {
            var nodes = xmlNode.SelectNodes(NodesXpath, xmlNamespaceManager);

            if (nodes == null || nodes.Count != 1)
            {
                throw new ApplicationException("File appears to have been authored by more than one device.");
            }
            
            XmlNode node = nodes[0];
            var info = new GpsDeviceInfo
                           {
                               Name = SafeGetSubNodeInnerText(node, xmlNamespaceManager, NameXpath),
                               UnitId = SafeGetSubNodeInnerText(node, xmlNamespaceManager, UnitIdXpath),
                               ProductId = SafeGetSubNodeInnerText(node, xmlNamespaceManager, ProductIdXpath),
                               FirmwareVersion = SafeGetVersionNode(node, xmlNamespaceManager)
                           };

            return info;
        }

        /// <summary>
        /// Retrieves the inner text of the first XmlNode found using the provided xPath expression
        /// </summary>
        /// <param name="xmlNode">Xml to retrieve text from</param>
        /// <param name="xmlNamespaceManager">The xml namespace manager</param>
        /// <param name="xPath">The xml node to search for within the provided XmlNode</param>
        /// <returns>The inner text of the first XmlNode found, or an empty string if there is no matching XmlNode</returns>
        private static string SafeGetSubNodeInnerText(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager, string xPath)
        {
            var subNode = xmlNode.SelectSingleNode(xPath, xmlNamespaceManager);
            
            return subNode == null ? string.Empty : subNode.InnerText;
        }

        /// <summary>
        /// Retrieves the Firmware Version infomation from the provided Xml
        /// </summary>
        /// <param name="xmlNode">Xml to retrieve the device information from</param>
        /// <param name="xmlNamespaceManager">The xml namespace manager</param>
        /// <returns>The firmware version infomation from the provided Xml structure</returns>
        private static Version SafeGetVersionNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager)
        {
            var major = xmlNode.SelectSingleNode(FirmwareVersionXPathVersionMajor, xmlNamespaceManager);
            int versionMajor = major == null ? 0 : int.Parse(major.InnerText);

            var minor = xmlNode.SelectSingleNode(FirmwareVersionXPathVersionMinor, xmlNamespaceManager);
            int versionMinor = minor == null ? 0 : int.Parse(minor.InnerText);

            var bMajor = xmlNode.SelectSingleNode(FirmwareVersionXPathBuildMajor, xmlNamespaceManager);
            int buildMajor = bMajor == null ? 0 : int.Parse(bMajor.InnerText);
            
            var bMinor = xmlNode.SelectSingleNode(FirmwareVersionXPathBuildMinor, xmlNamespaceManager);
            int buildMinor = bMinor == null ? 0 : int.Parse(bMinor.InnerText);

            return new Version(versionMajor, versionMinor, buildMajor, buildMinor);
        }
    }
}
