// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSegmentTests.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   A test fixture for data segment tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturionTests
{
    using NUnit.Framework;

    using Rhino.Mocks;

    using StravaCenturion;

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
