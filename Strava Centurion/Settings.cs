// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="fuckingbrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// <summary>
//   Defines the Settings type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion.Properties
{
    using System;

    /// <summary>
    /// This class allows you to handle specific events on the settings class:
    /// The SettingChanging event is raised before a setting's value is changed.
    /// The PropertyChanged event is raised after a setting's value is changed.
    /// The SettingsLoaded event is raised after the setting values are loaded.
    /// The SettingsSaving event is raised before the setting values are saved.
    /// </summary>
    internal sealed partial class Settings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            // // To add event handlers for saving and changing settings, uncomment the lines below:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
        }

        /// <summary>
        /// Add code to handle the SettingChangingEvent event here.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            // Add code to handle the SettingChangingEvent event here.
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// // Add code to handle the SettingsSaving event here.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) 
        {
            throw new NotImplementedException();
        }
    }
}
