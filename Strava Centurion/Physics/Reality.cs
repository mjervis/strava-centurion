// -----------------------------------------------------------------------
// <copyright file="Reality.cs" company="FuckingBrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// -----------------------------------------------------------------------

namespace Strava_Centurion
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Container class to hold reality settings, plus, calculate some physics.
    /// </summary>
    public class Reality
    {
        #region Physics Constants

        /// <summary>
        /// acceleration due to gravity – 9.8 m/s2
        /// </summary>
        public const double G = 9.80665;

        /// <summary>
        /// The Specific Constant for dry air.
        /// Physics: this is a set thing in reality.
        /// </summary>
        public const double SpeicificGasConstantDryAir = 287.05;

        /// <summary>
        /// Molecular Weight of Dry Air in kg/mole.
        /// </summary>
        public const double MolecularWeightOfDryAir = 0.0289644;

        /// <summary>
        /// The Specific Constant for Water Vapor.
        /// Physics: this is a set thing in reality.
        /// </summary>
        public const double SpecificGasConstanthWaterVapor = 461.495;

        /// <summary>
        /// This is the standard density of dry air at sea level.
        /// Theoretical, probably never really happens.
        /// </summary>
        public const double AirDenistyDefault = 1.2466;

        /// <summary>
        /// The gas constant.
        /// </summary>
        public const double GasConstant = 8.31447;

        /// <summary>
        /// The temperature lapse rate of the troposphere. In Degrees Kelvin a m.
        /// </summary>
        public const double TemperatureLapseRate = 0.0065;

        /// <summary>
        /// Standard temperature at sea level in degrees Kelvin.
        /// </summary>
        public const double SeaLevelStandardTemp = 288.15;

        /// <summary>
        /// Standard pressure at sea level in kg/m^3
        /// </summary>
        public const double SeaLevelStandardPressure = 101325;

        #endregion

        #region Private Member variables for properties
        /// <summary>
        /// The Coefficient of Rolling Resistance.
        /// This will vary in reality over a ride based on the road surfaces.
        /// http://4.bp.blogspot.com/_urSQl6wUA5g/TTZFTUOs6FI/AAAAAAAAIe8/zeE_bHhBpOE/s1600/drag_chart.jpg
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here, picking up words in the url as misspelled.")]
        private double coefficientOfRollingResistance = 0.0045;

        /// <summary>
        /// This is the drag coefficient. I'm not sure what this is.
        /// </summary>
        private double dragCoefficient = 0.5;

        /// <summary>
        /// This is the effective frontal area of rider and bike.
        /// Obviously working this out is rather hard, so it's a guess.
        /// </summary>
        private double effectiveFrontalArea = 0.399483;

        /// <summary>
        /// Average temp for the ride.
        /// </summary>
        private double temperature = 15.0;
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Coefficient of Rolling Resistance.
        /// This will vary in reality over a ride based on the road surfaces.
        /// </summary>
        public double CoefficientOfRollingResistance
        {
            get
            {
                return this.coefficientOfRollingResistance;
            }

            set
            {
                this.coefficientOfRollingResistance = value;
            }
        }

        /// <summary>
        /// Gets or sets the drag coefficient. I'm not sure what this is.
        /// </summary>
        public double DragCoefficient
        {
            get
            {
                return this.dragCoefficient;
            }

            set
            {
                this.dragCoefficient = value;
            }
        }

        /// <summary>
        /// Gets or sets the effective frontal area of rider and bike.
        /// Obviously working this out is rather hard, so it's a guess.
        /// </summary>
        public double EffectiveFrontalArea
        {
            get
            {
                return this.effectiveFrontalArea;
            }

            set
            {
                this.effectiveFrontalArea = value;
            }
        }

        /// <summary>
        /// Gets or sets the temperature of the ride.
        /// This is used to calculate air density at altitude.
        /// </summary>
        public double Temperature
        {
            get
            {
                return this.temperature;
            }

            set
            {
                this.temperature = value;
            }
        }

        /// <summary>
        /// Gets G.
        /// </summary>
        public double AccelerationDueToGravity
        {
            get
            {
                return G;
            }
        }

        #endregion

        /// <summary>
        /// Calculate the estimated air density at a given altitude.
        /// <a href="http://wahiduddin.net/calc/density_altitude.htm">Reference</a>
        /// </summary>
        /// <param name="altitude">Height in meters</param>
        /// <returns>air density in kg/m^3</returns>
        public double AirDensity(double altitude)
        {
            double density;
            density = (this.PressureAtAltitude(altitude) * MolecularWeightOfDryAir) / (GasConstant * this.CelciusToKelvin(this.Temperature));
            return density;
        }

        /// <summary>
        /// Calculate a pressure at a given altitude.
        /// </summary>
        /// <param name="altitude">
        /// The altitude in m.
        /// </param>
        /// <returns>
        /// Pressure in Pa
        /// </returns>
        public double PressureAtAltitude(double altitude)
        {
            double gM = this.AccelerationDueToGravity * MolecularWeightOfDryAir;
            double RL = SpecificGasConstanthWaterVapor * TemperatureLapseRate;
            double gMOverRL = gM / RL;

            altitude = altitude / 1000.0; // meters to km;
            double LH = TemperatureLapseRate * altitude;
            double pressure = SeaLevelStandardPressure * Math.Pow(1.0 - (LH / SeaLevelStandardTemp), gMOverRL);
            return pressure; // pa
        }

        /// <summary>
        /// Convert from Degrees Celcius to Degrees Kelvin (c + 273.15)
        /// </summary>
        /// <param name="celcius">Temperature in celcius</param>
        /// <returns>double, temperature in kelvin.</returns>
        private double CelciusToKelvin(double celcius)
        {
            return celcius + 273.15;
        }
    }
}