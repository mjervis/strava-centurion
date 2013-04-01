// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPointTests.cs" company="FuckingBrit.com">
//   Copyright (c) 2013 FuckingBrit.com
// </copyright>
// <summary>
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion_Tests_2010
{
    using System;

    using NUnit.Framework;

    using Strava_Centurion;

    [TestFixture]
    public class DataPointTests
    {
        [Test]
        public void DistanceToPointIsCorrect_BothTotalDistancesAreKnown()
        {
            var point1 = new DataPoint(DateTime.Now, Distance.Zero, 0, new Distance(123.45), new Speed(0), 0, new Angle(0), new Angle(0));
            var point2 = new DataPoint(DateTime.Now, Distance.Zero, 0, new Distance(234.56), new Speed(0), 0, new Angle(0), new Angle(0));

            Assert.AreEqual(111.11, point1.DistanceToPoint(point2).Metres);
        }

        [Test]
        public void DistanceToPointIsCorrect_BothTotalDistancesAreKnownAndSame()
        {
            var point1 = new DataPoint(DateTime.Now, Distance.Zero, 0, new Distance(123.45), new Speed(0), 0, new Angle(0), new Angle(0));
            var point2 = new DataPoint(DateTime.Now, Distance.Zero, 0, new Distance(123.45), new Speed(0), 0, new Angle(0), new Angle(0));

            Assert.AreEqual(0.0, point1.DistanceToPoint(point2).Metres);
        }

        [Test]
        public void DistanceToPointIsCorrect_TotalDistanceIsNotKnown_AltitudeIsKnownAndSame_PositionIsKnownAndSame()
        {
            var point1 = new DataPoint(DateTime.Now, new Distance(5), 0, Distance.Unknown, new Speed(0), 0, new Angle(6), new Angle(6));
            var point2 = new DataPoint(DateTime.Now, new Distance(5), 0, new Distance(234.56), new Speed(0), 0, new Angle(6), new Angle(6));

            Assert.AreEqual(0.0, point1.DistanceToPoint(point2).Metres);
        }

        [Test]
        public void DistanceToPointIsCorrect_TotalDistanceIsNotKnown_AltitudeIsKnownAndSame_PositionIsKnown()
        {
            var point1 = new DataPoint(DateTime.Now, new Distance(5), 0, Distance.Unknown, new Speed(0), 0, new Angle(53.296161), new Angle(-1.513128));
            var point2 = new DataPoint(DateTime.Now, new Distance(5), 0, new Distance(234.56), new Speed(0), 0, new Angle(53.295997), new Angle(-1.513449));

            Assert.AreEqual(2287.02, point1.DistanceToPoint(point2).Metres, 0.005);
        }

        [Test]
        public void DistanceToPointIsCorrect_TotalDistanceIsNotKnown_AltitudeIsKnown_PositionIsKnownAndSame()
        {
            var point1 = new DataPoint(DateTime.Now, new Distance(5), 0, Distance.Unknown, new Speed(0), 0, new Angle(53.296161), new Angle(-1.513128));
            var point2 = new DataPoint(DateTime.Now, new Distance(10), 0, new Distance(234.56), new Speed(0), 0, new Angle(53.296161), new Angle(-1.513128));

            Assert.AreEqual(5.0, point1.DistanceToPoint(point2).Metres, 0.005);
        }

        [Test]
        public void DistanceToPointIsCorrect_TotalDistanceIsNotKnown_AltitudeIsNotKnown_PositionIsKnown()
        {
            var point1 = new DataPoint(DateTime.Now, Distance.Unknown, 0, Distance.Unknown, new Speed(0), 0, new Angle(53.296161), new Angle(-1.513128));
            var point2 = new DataPoint(DateTime.Now, new Distance(10), 0, new Distance(234.56), new Speed(0), 0, new Angle(54.296161), new Angle(-1.513128));

            Assert.IsTrue(point1.DistanceToPoint(point2).IsUnknown);
        }

        [Test]
        public void DistanceToPointIsCorrect_TotalDistanceIsNotKnown_AltitudeIsKnown_PositionIsNotKnown()
        {
            var point1 = new DataPoint(DateTime.Now, new Distance(5), 0, Distance.Unknown, new Speed(0), 0, Angle.Unknown, new Angle(-1.513128));
            var point2 = new DataPoint(DateTime.Now, new Distance(10), 0, new Distance(234.56), new Speed(0), 0, new Angle(53.296161), new Angle(-1.513128));

            Assert.IsTrue(point1.DistanceToPoint(point2).IsUnknown);
        }

        /*

        /// <summary>
        /// Calculates the ascent from this point to the point specified.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>The ascent in meters.</returns>
        public Distance AscentToPoint(DataPoint other)
        {
            return other.Altitude - this.Altitude;
        }

        /// <summary>
        /// Calculate the gradient from this point to another point as the ratio of the
        /// distance in m climbed over the distance in m travelled.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>Ratio of ascent to distance.</returns>
        public double GradientToPoint(DataPoint other)
        {
            double distance = this.HaversineDistanceToPoint(other);

            // if we've not moved then the gradient must be 0
            if (Math.Abs(distance - 0.0) < 0.0001)
            {
                return 0.0;
            }

            return this.AscentToPoint(other) / distance;   
        }

        /// <summary>
        /// Calculates the distance in meters between this point and another using latitude and longitude.
        /// <a href="http://www.movable-type.co.uk/scripts/latlong.html">Reference</a>
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>Double, the distance in meters as the crow flies.</returns>
        public Distance HaversineDistanceToPoint(DataPoint other)
        {
            var latitudeDelta = other.Latitude - this.Latitude;
            var longitudeDelta = other.Longitude - this.Longitude;

            var h = this.Haversine(latitudeDelta) + (Math.Cos(this.Latitude) * Math.Cos(other.Latitude) * this.Haversine(longitudeDelta));

            var radius = this.GetRadiusOfEarth(this.Latitude);

            var distance = 2.0 * radius * Math.Asin(Math.Sqrt(h));

            return new Distance(distance);
        }
         * 
         * */

        /*
        [Test]
        public void WhenConstructedAltitudeIsRead()
        {
            // arrange
            var mockNode = MockRepository.GenerateMock<INode>();
            mockNode.Stub(s => s.GetAltitude()).Return(50);

            // act
            var tcxPoint = new DataPoint(mockNode);

            // assert
            mockNode.AssertWasCalled(s => s.GetAltitude());
        }

        [Test]
        public void WhenConstructedCadenceIsRead()
        {
            // arrange
            var mockNode = MockRepository.GenerateMock<INode>();
            mockNode.Stub(s => s.GetCadence()).Return(83);

            // act
            var tcxPoint = new DataPoint(mockNode);

            // assert
            mockNode.AssertWasCalled(s => s.GetCadence());
        }

        [Test]
        public void WhenConstructedDateTimeIsRead()
        {
            // arrange
            var mockNode = MockRepository.GenerateMock<INode>();
            mockNode.Stub(s => s.GetDateTime()).Return(DateTime.Now);

            // act
            var tcxPoint = new DataPoint(mockNode);

            // assert
            mockNode.AssertWasCalled(s => s.GetDateTime());
        }

        [Test]
        public void WhenConstructedHeartrateIsRead()
        {
            // arrange
            var mockNode = MockRepository.GenerateMock<INode>();
            mockNode.Stub(s => s.GetHeartrate()).Return("46");

            // act
            var tcxPoint = new DataPoint(mockNode);

            // assert
            mockNode.AssertWasCalled(s => s.GetHeartrate());
        }

        [Test]
        public void WhenConstructedLatitudeIsRead()
        {
            // arrange
            var mockNode = MockRepository.GenerateMock<INode>();
            mockNode.Stub(s => s.GetLatitude()).Return(50);

            // act
            var tcxPoint = new DataPoint(mockNode);

            // assert
            mockNode.AssertWasCalled(s => s.GetLatitude());
        }

        [Test]
        public void WhenConstructedLongitudeIsRead()
        {
            // arrange
            var mockNode = MockRepository.GenerateMock<INode>();
            mockNode.Stub(s => s.GetLongitude()).Return(50);

            // act
            var tcxPoint = new DataPoint(mockNode);

            // assert
            mockNode.AssertWasCalled(s => s.GetLongitude());
        }

        [Test]
        public void WhenConstructedPowerIsRead()
        {
            // arrange
            var mockNode = MockRepository.GenerateMock<INode>();
            mockNode.Stub(s => s.GetPower()).Return(50);

            // act
            var tcxPoint = new DataPoint(mockNode);

            // assert
            mockNode.AssertWasCalled(s => s.GetPower());
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

        [Test]
        public void WhenConstructedTotalDistanceIsRead()
        {
            // arrange
            var mockNode = MockRepository.GenerateMock<INode>();
            mockNode.Stub(s => s.GetTotalDistance()).Return(50);

            // act
            var tcxPoint = new DataPoint(mockNode);

            // assert
            mockNode.AssertWasCalled(s => s.GetTotalDistance());
        }

        [Test]
        public void DistanceInMetersWithNoTotalDistanceTest()
        {
            // arrange
            var mockNodeStart = MockRepository.GenerateMock<INode>();
            mockNodeStart.Stub(s => s.GetLatitude()).Return(53.296161);
            mockNodeStart.Stub(s => s.GetLongitude()).Return(-1.513128);
            mockNodeStart.Stub(s => s.GetAltitude()).Return(5);
            mockNodeStart.Stub(s => s.GetTotalDistance()).Return(double.NaN);

            var mockNodeEnd = MockRepository.GenerateMock<INode>();
            mockNodeEnd.Stub(s => s.GetLatitude()).Return(53.295997);
            mockNodeEnd.Stub(s => s.GetLongitude()).Return(-1.513449);
            mockNodeEnd.Stub(s => s.GetAltitude()).Return(10);
            mockNodeEnd.Stub(s => s.GetTotalDistance()).Return(double.NaN);

            var tcxPointStart = new DataPoint(mockNodeStart);
            var tcxPointEnd = new DataPoint(mockNodeEnd);

            // act
            var result = tcxPointStart.DistanceToPoint(tcxPointEnd).Metres;

            // assert
            Assert.AreEqual(28.465, result, 0.001);
        }

        [Test]
        public void HaversineTest()
        {
            // arrange
            var mockNodeStart = MockRepository.GenerateMock<INode>();
            mockNodeStart.Stub(s => s.GetLatitude()).Return(53.296161);
            mockNodeStart.Stub(s => s.GetLongitude()).Return(-1.513128);

            var mockNodeEnd = MockRepository.GenerateMock<INode>();
            mockNodeEnd.Stub(s => s.GetLatitude()).Return(53.295997);
            mockNodeEnd.Stub(s => s.GetLongitude()).Return(-1.513449);

            var tcxPointStart = new DataPoint(mockNodeStart);
            var tcxPointEnd = new DataPoint(mockNodeEnd);

            // act
            var result = tcxPointStart.HaversineDistanceToPoint(tcxPointEnd);

            // assert
            Assert.AreEqual(28.022, result, 0.001);
        }*/
    }
}
