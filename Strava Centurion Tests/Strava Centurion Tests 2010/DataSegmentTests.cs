// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSegmentTests.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   A test fixture for data segment tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion_Tests_2010
{
    using NUnit.Framework;

    using Rhino.Mocks;

    using Strava_Centurion;

    /// <summary>
    /// A test fixture for data segment tests.
    /// </summary>
    [TestFixture]
    public class DataSegmentTests
    {
        /// <summary>
        /// A test to ensure a data segment can be constructed.
        /// </summary>
        [Test]
        public void CanConstructWithoutException()
        {
            // arrange
            var mockNode1 = MockRepository.GenerateMock<INode>();
            var mockDataPoint1 = MockRepository.GenerateMock<DataPoint>(mockNode1);

            var mockNode2 = MockRepository.GenerateMock<INode>();
            var mockDataPoint2 = MockRepository.GenerateMock<DataPoint>(mockNode2);

            // act
            var segment = new DataSegment(mockDataPoint1, mockDataPoint2);

            // assert
            Assert.IsNotNull(segment);
        }
    }
}
