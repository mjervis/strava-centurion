// -----------------------------------------------------------------------
// <copyright file="TcxPointTests.cs" company="FuckingBrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// -----------------------------------------------------------------------

namespace Strava_Centurion_Tests
{
    using System.Xml;

    using NUnit.Framework;

    using Rhino.Mocks;

    using Strava_Centurion;

    [TestFixture]
    public class TcxPointTests
    {
        [Test]
        public void CanConstructWithoutException()
        {
            // arrange
            var mockXmlNode = MockRepository.GenerateMock<INode>();

            // act
            var tcxPoint = new DataPoint(mockXmlNode);

            // assert
            Assert.IsNotNull(tcxPoint);
        }

        // and we could then write tests like this that assert the correct xml access is going on...

        /*
        [Test]
        public void WhenConstructedXmlFileAccessOccurs()
        {
            // arrange
            var mockXmlNode = MockRepository.GenerateMock<INode>();
            mockXmlNode.Stub(s => s.SafeGetNodeText(Arg<string>.Is.Anything)).Return(50);

            // act
            var tcxPoint = new DataPoint(mockXmlNode);

            // assert
            mockXmlNode.AssertWasCalled(s => SafeGetNodeText("tcx:AltitudeMeters"));
            mockXmlNode.AssertWasCalled(s => SafeGetNodeText("tcx:DistanceMeters"));
            mockXmlNode.AssertWasCalled(s => SafeGetNodeText("tcx:Cadence"));
            etc.
        }
         * */
    }
}
