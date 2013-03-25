// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpeedTests.cs" company="FuckingBrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// <summary>
//   Test the speed class..
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion_Tests_2010
{
    using NUnit.Framework;

    using Strava_Centurion;

    /// <summary>
    /// Test the speed class. All assertions of equality are based on what Google thinks the right answer is.
    /// </summary>
    [TestFixture]
    public class SpeedTests
    {
        /// <summary>
        /// Check that construction works and sets the m/s speed right.
        /// </summary>
        [Test]
        public void CanConstructPositive()
        {
            Speed speed;
            speed = new Speed(10.0);
            Assert.AreEqual(10.0, speed.MetersPerSecond);
        }

        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void CantConstructNegative()
        {
            Speed speed = new Speed(-10.0);
        }

        [Test]
        public void ConstructWithStandingStill()
        {
            Speed speed = new Speed(0.0);

            Assert.AreEqual(0.0, speed.MetersPerSecond);
        }

        [Test]
        public void ConstructWithStandingStillAllWork()
        {
            Speed speed = new Speed(0.0);
            Assert.AreEqual(0.0, speed.MetersPerSecond);
            Assert.AreEqual(0.0, speed.FeetPerSecond);
            Assert.AreEqual(0.0, speed.KmHour);
            Assert.AreEqual(0.0, speed.MilesPerHour);
        }

        [Test]
        public void ConvertToKmHPositive()
        {
            Speed speed;
            speed = new Speed(10.0);
            Assert.AreEqual(36.0, speed.KmHour);
        }

        [Test]
        public void CovertToFeetPerSecond()
        {
            Speed speed = new Speed(10.0);
            Assert.AreEqual(32.808, speed.FeetPerSecond, 0.0001);
        }

        [Test]
        public void ConvertToMilesPerHour()
        {
            Speed speed = new Speed(10.0);
            Assert.AreEqual(22.369, speed.MilesPerHour, 0.001);
        }
    }
}