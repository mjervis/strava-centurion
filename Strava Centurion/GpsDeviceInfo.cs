// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GpsDeviceInfo.cs" company="Xellepher">
//   Copyright (c) 2013 Xellepher
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// <summary>
//   Defines the Gps Device Information type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StravaCenturion
{
    using System;

    /// <summary>
    /// GPS Device Infomation
    /// </summary>
    public class GpsDeviceInfo
    {
        /// <summary>
        /// GPS Device Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Unit ID
        /// </summary>
        public string UnitId { get; set; }

        /// <summary>
        /// Product ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Firmware Version
        /// </summary>
        public Version FirmwareVersion { get; set; }
    }
}