// -----------------------------------------------------------------------
// <copyright file="RealityTests.cs" company="CSE Global UK">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Strava_Centurion_Tests_2010
{
    using NUnit.Framework;

    using Strava_Centurion;

    /// <summary>
    /// Tests for the reality class.
    /// Air density based on:
    /// http://www.denysschen.com/catalogue/density.aspx
    /// Pressure:
    /// http://www.calctool.org/CALC/phys/default/pres_at_alt
    /// </summary>
    [TestFixture]
    public class RealityTests
    {
        [Test]
        public void DensityAtSeaLevel15Degrees()
        {
            Reality reality = new Reality();
            reality.Temperature = 15.0;
            double pressure = reality.AirDensity(0);
            Assert.AreEqual(1.221, pressure, 0.0001);
        }

        [Test]
        public void DenistyAtSeaLevel45Degrees()
        {
            Reality reality = new Reality();
            reality.Temperature = 45.0;
            Assert.AreEqual(1.106, reality.AirDensity(0.0));
        }

        [Test]
        public void DensityAt200Meters15Degrees()
        {
            Reality reality = new Reality();
            reality.Temperature = 15.0;
            Assert.AreEqual(1.193, reality.AirDensity(200.0));
        }

        [Test]
        public void DensityAt150Meters2Degrees()
        {
            Reality reality = new Reality();
            reality.Temperature = 2.0;
            Assert.AreEqual(1.256, reality.AirDensity(150.0));
        }

        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void DensityAtNegativeAltitudeIsAnError()
        {
            Reality reality = new Reality();
            reality.Temperature = 2.0;
            double pressure = reality.AirDensity(-150.0);
        }

        [Test]  
        public void DensityBelowFreezing()
        {
            Reality reality = new Reality();
            reality.Temperature = -10.0;
            Assert.AreEqual(1.314, reality.AirDensity(150.0));
        }

        [Test]
        public void PressureAtZeroAltitude()
        {
            Reality reality = new Reality();
            Assert.AreEqual(101325, reality.PressureAtAltitude(0));
        }

        [Test]
        public void PressureAtPositiveAltitude()
        {
            Reality reality = new Reality();
            Assert.AreEqual(99176.8, reality.PressureAtAltitude(150));
        }

        [Test]
        public void PressureAtNegativeAltitude()
        {
            Reality reality = new Reality();
            Assert.AreEqual(103520, reality.PressureAtAltitude(-150.0));
        }
    }
}
