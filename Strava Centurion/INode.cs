// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INode.cs" company="RuPC">
//   Copyright 2013 RuPC
// </copyright>
// <summary>
//   Defines the INode type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion
{
    using System;

    /// <summary>
    /// An interface for accessing workout and track data (such as garmin or polar file formats).
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Gets the date and time of the data.
        /// </summary>
        /// <returns>The date and time that the rest of the data is relevant for.</returns>
        DateTime GetDateTime();

        /// <summary>
        /// Gets the altitude data.
        /// </summary>
        /// <returns>The altitude.</returns>
        double GetAltitude();

        /// <summary>
        /// Gets the cadence data.
        /// </summary>
        /// <returns>The cadence.</returns>
        string GetCadence();

        /// <summary>
        /// Gets the total distance data.
        /// </summary>
        /// <returns>The total distance</returns>
        double GetTotalDistance();

        /// <summary>
        /// Gets the heart rate data.
        /// </summary>
        /// <returns>The heart rate.</returns>
        string GetHeartrate();

        /// <summary>
        /// Gets the longitude data.
        /// </summary>
        /// <returns>The longitude.</returns>
        double GetLongitude();

        /// <summary>
        /// Gets the latitude data.
        /// </summary>
        /// <returns>The latitude.</returns>
        double GetLatitude();

        /// <summary>
        /// Gets the speed data.
        /// </summary>
        /// <returns>The speed data.</returns>
        double GetSpeed();

        /// <summary>
        /// Gets the power data.
        /// </summary>
        /// <returns>The power data.</returns>
        double GetPower();

        /// <summary>
        /// Sets the power data.
        /// </summary>
        /// <param name="power">The power value to update with.</param>
        void SetPower(double power);
    }
}
