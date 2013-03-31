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
            var mockDataPoint1 = MockRepository.GenerateMock<DataPoint>();
            var mockDataPoint2 = MockRepository.GenerateMock<DataPoint>();

            // act
            var segment = new DataSegment(mockDataPoint1, mockDataPoint2);

            // assert
            Assert.IsNotNull(segment);
        }
    }
}
