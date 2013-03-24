﻿// -----------------------------------------------------------------------
// <copyright file="TcxPointTests.cs" company="FuckingBrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// -----------------------------------------------------------------------

namespace Strava_Centurion_Tests
{
    using System;
    using System.Xml;

    using NUnit.Framework;

    using Rhino.Mocks;

    using Strava_Centurion;

    [TestFixture]
    public class DataPointTests
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
            mockNode.Stub(s => s.GetCadence()).Return("83");

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

            // assert
            Assert.AreEqual(28.51, Math.Round(tcxPointStart.DistanceInMetresToPoint(tcxPointEnd)), 2);
        }

        [Test]
        public void HaversineTest()
        {
            var mockNodeStart = MockRepository.GenerateMock<INode>();
            mockNodeStart.Stub(s => s.GetLatitude()).Return(53.296161);
            mockNodeStart.Stub(s => s.GetLongitude()).Return(-1.513128);
            var mockNodeEnd = MockRepository.GenerateMock<INode>();
            mockNodeEnd.Stub(s => s.GetLatitude()).Return(53.295997);
            mockNodeEnd.Stub(s => s.GetLongitude()).Return(-1.513449);

            var tcxPointStart = new DataPoint(mockNodeStart);
            var tcxPointEnd = new DataPoint(mockNodeEnd);

            // assert
            Assert.AreEqual(28.07, Math.Round(tcxPointStart.HaversineDistanceInMetresToPoint(tcxPointEnd)), 2);
        }
    }
}