// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TcxNodeTests.cs" company="">
//   
// </copyright>
// <summary>
//   A test fixture for testing the garmin node.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion_Tests
{
    using System.Xml;

    using NUnit.Framework;

    using Strava_Centurion;

    /// <summary>
    /// A test fixture for testing the garmin node.
    /// </summary>
    [TestFixture]
    public class TcxNodeTests
    {
        /// <summary>
        /// The xml document.
        /// </summary>
        private XmlDocument xmlDocument;

        /// <summary>
        /// The xml namespace manager.
        /// </summary>
        private XmlNamespaceManager xmlNamespaceManager;

        /// <summary>
        /// The setup code run before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.xmlDocument = new XmlDocument();

            this.xmlNamespaceManager = new XmlNamespaceManager(this.xmlDocument.NameTable);
            this.xmlNamespaceManager.AddNamespace("tcx", "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2");
            this.xmlNamespaceManager.AddNamespace("ns3", "http://www.garmin.com/xmlschemas/ActivityExtension/v2");          
        }

        /// <summary>
        /// A test to ensure a garmin node can be constructed without exception.
        /// </summary>
        [Test]
        public void CanConstruct()
        {
            using (var textReader = new XmlTextReader("<SOME VALID TCX>"))
            {
                this.xmlDocument.Load(textReader);
            }

            var node = new TcxNode(this.xmlDocument.SelectSingleNode("TODO"), this.xmlNamespaceManager);
            
            Assert.IsNotNull(node);
        }

        [Test]
        public void GetDateTimeReturnsCorrectValue()
        {
            using (var textReader = new XmlTextReader("<SOME VALID TCX>"))
            {
                this.xmlDocument.Load(textReader);
            }

            Assert.Fail();
        }

        [Test]
        public void GetDateTimeThrowsExceptionIfValueIsMissing()
        {
            using (var textReader = new XmlTextReader("<TCX WITH MISSING DATETIME>"))
            {
                this.xmlDocument.Load(textReader);
            }

            Assert.Fail();
        }

        [Test]
        public void GetDateTimeThrowsExceptionIfValueCannotBeParsed()
        {
            using (var textReader = new XmlTextReader("<TCX WITH MANGLED DATETIME>"))
            {
                this.xmlDocument.Load(textReader);
            }

            Assert.Fail();
        }
    }
}
