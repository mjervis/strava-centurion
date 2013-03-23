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

        [Test]
        public void WhenConstructedSpeedIsRead()
        {
            // arrange
            var mockNode = MockRepository.GenerateMock<INode>();
            mockNode.Stub(s => s.GetSpeed()).Return(50);

            // act
            var tcxPoint = new DataPoint(mockNode);

            // assert
            mockNode.AssertWasCalled(s => s.GetSpeed());
        }

        // etc.
    }
}
