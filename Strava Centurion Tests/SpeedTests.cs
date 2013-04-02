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

namespace StravaCenturionTests
{
    using NUnit.Framework;

    using StravaCenturion.Units;

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
            Assert.AreEqual(10.0, speed.MetresPerSecond);
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

            Assert.AreEqual(0.0, speed.MetresPerSecond);
        }

        [Test]
        public void ConstructWithStandingStillAllWork()
        {
            var speed = new Speed(0.0);

            Assert.AreEqual(0.0, speed.MetresPerSecond);
            Assert.AreEqual(0.0, speed.KilometresPerHour);
        }

        [Test]
        public void ConvertToKmHPositive()
        {
            var speed = new Speed(10.0);

            Assert.AreEqual(36.0, speed.KilometresPerHour);
        }
    }
}