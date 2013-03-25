// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerRangerTests.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   The power ranger tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion_Tests_2010
{
    using NUnit.Framework;

    using Rhino.Mocks;

    using Strava_Centurion;

    /// <summary>
    /// The power ranger tests.
    /// </summary>
    [TestFixture]
    public class PowerRangerTests
    {
        /// <summary>
        /// The mock for the reality.
        /// </summary>
        private Reality mockReality;

        /// <summary>
        /// The mock for the rider.
        /// </summary>
        private Rider mockRider;

        /// <summary>
        /// The test fixture setup which occurs once before all tests are run.  This method
        /// is used to set up the mocks that will always be the same for all tests.
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            this.mockReality = MockRepository.GenerateStub<Reality>();
            this.mockRider = MockRepository.GenerateStub<Rider>(85.1, 7.3);
        }

        /// <summary>
        /// A test to ensure the power ranger can be constructed.
        /// </summary>
        [Test]
        public void CanConstructWithoutException()
        {
            // arrange

            // act
            var powerRanger = new PowerRanger(this.mockReality, this.mockRider);

            // assert
            Assert.IsNotNull(powerRanger);
        }

        /// <summary>
        /// A test to ensure the power ranger can be constructed.
        /// </summary>
        [Test]
        public void CalculateRollingResistanceForceReturnsCorrectValue()
        {
            // test figures validated at http://www.analyticcycling.com/

            // arrange
            this.mockReality.CoefficientOfRollingResistance = 0.004;

            // act
            var powerRanger = new PowerRanger(this.mockReality, this.mockRider);
            var result = powerRanger.CalculateRollingResistanceForce();

            // assert
            Assert.IsInstanceOf<Force>(result);
            Assert.AreEqual(3.6245, result.Newtons, 0.0001);
        }
    }
}
