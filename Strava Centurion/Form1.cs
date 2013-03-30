// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Form1.cs" company="fuckingbrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// <summary>
//   Class to do the main UI.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;

    using Strava_Centurion.FileFormats;
    using Strava_Centurion.Properties;

    /// <summary>
    /// Class to do the main UI.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Private Members
        /// <summary>
        /// Full, safe path to the selected TCX file.
        /// </summary>
        private string fullFileName = string.Empty;

        /// <summary>
        /// The TCX file.
        /// </summary>
        private TcxFile tcxFile;

        /// <summary>
        /// The reality in which we operate.
        /// </summary>
        private Reality reality;

        /// <summary>
        /// Hold the bike weight in kg for validating UI changes.
        /// </summary>
        private double cachedBikeWeight = 10.5;

        /// <summary>
        /// Hold the rider weight in kg for validating UI changes.
        /// </summary>
        private double cachedRiderWeight = 76.7;
        #endregion

        #region App start and cleanup.
        /// <summary>
        /// Initialize the reality etc.
        /// </summary>
        /// <param name="sender">Standard windows event sender.</param>
        /// <param name="e">Standard windows event arguments.</param>
        private void MainFormLoad(object sender, EventArgs e)
        {
            this.reality = new Reality();

            this.reality.Temperature = this.GetSafeDouble(this.temperature, this.reality.Temperature);
            this.reality.CoefficientOfRollingResistance = this.GetSafeDouble(
                this.Crr, this.reality.CoefficientOfRollingResistance);
            this.reality.DragCoefficient = this.GetSafeDouble(this.dragCoefficient, this.reality.DragCoefficient);
            this.reality.EffectiveFrontalArea = this.GetSafeDouble(this.frontalArea, this.reality.EffectiveFrontalArea);
            this.cachedBikeWeight = this.GetSafeDouble(this.bikeWeight, this.cachedBikeWeight);
            this.cachedRiderWeight = this.GetSafeDouble(this.riderWeight, this.cachedRiderWeight);
        }

        /// <summary>
        /// Save app settings on close.
        /// </summary>
        /// <param name="sender">Standard event sender.</param>
        /// <param name="e">Standard event e.</param>
        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            // todo: set settings from current state!
            Settings.Default.Save();
        }
        #endregion

        #region Helper Methods.
        /// <summary>
        /// Adds a message to the text box.
        /// </summary>
        /// <param name="msg">
        /// String, message to be logged.
        /// </param>
        private void LogMessage(string msg)
        {
            logText.Text += string.Format("[{0}] - [{1}]\r\n", DateTime.Now.ToString(CultureInfo.CurrentCulture), msg);
        }

        /// <summary>
        /// Parse a double from a text box, setting it back to a clean value if it's not right.
        /// </summary>
        /// <param name="tb">
        /// A text box to get the value from.
        /// </param>
        /// <param name="safeDefault">
        /// The safe default to restore to the text box if it's not valid.
        /// </param>
        /// <returns>
        /// The text of the <see cref="tb"/> if it's a valid double, else the safe default.
        /// </returns>
        private double GetSafeDouble(TextBox tb, double safeDefault)
        {
            double d;
            if (!double.TryParse(tb.Text, out d))
            {
                d = safeDefault;
                tb.Text = safeDefault.ToString(CultureInfo.CurrentCulture);
            }

            return d;
        }

        /// <summary>
        /// Generate an output filename.
        /// </summary>
        /// <param name="extension">The extension to create on the returned file.</param>
        /// <returns>
        /// String for a unique output file, based on the current operation file.
        /// </returns>
        private string OutputFilename(string extension)
        {
            string outFilename = string.Format("{0}.{1}.{2}", Path.GetFileNameWithoutExtension(this.fullFileName), DateTime.Now.ToString("yyyyMMddHHmmss"), extension);
            var s = this.fullFileName;
            if (s != null)
            {
                s = Path.GetDirectoryName(s);
                if (s != null)
                {
                    outFilename = Path.Combine(s, outFilename);
                }
            }

            return outFilename;
        }
        #endregion

        #region File Load Thread
        /// <summary>
        /// Launches the file browser, opens a new TCX file and loads it into a set of objects.
        /// </summary>
        /// <param name="sender">
        /// Standard windows event arguments, the sending object.
        /// </param>
        /// <param name="e">
        /// Standard windows event arguments, the event arguments.
        /// </param>
        private void ButtonOpenClick(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                filename.Text = openFile.SafeFileName;
                this.fullFileName = openFile.FileName;
                this.fileParserThread.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Having selected a new file, this will parse it into a new <see cref="TcxFile"/> instance.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void FileParserThreadDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            this.tcxFile = new TcxFile(this.fullFileName);
        }

        /// <summary>
        /// On completion of loading a file, change UI state and log some response to the user.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void FileParserThreadRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.LogMessage(string.Format("File loaded ({0} trackpoints).", this.tcxFile.TrackPoints.Count));
            this.buttonProcess.Enabled = this.tcxFile != null;
        }
        #endregion

        #region Power Calculation Thread
        /// <summary>
        /// Execute the power calculations on a background worker thread.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonProcessClick(object sender, EventArgs e)
        {
            powerCalcThread.RunWorkerAsync();
        }

        /// <summary>
        /// Set the options needed on the <see cref="PowerRanger"/> and <see cref="Reality"/>,
        /// then run the power calculations, optionally generating the output files.
        /// </summary>
        /// <param name="sender">Standard windows event sender.</param>
        /// <param name="e">Standard windows event arguments.</param>
        private void PowerCalcThreadDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var ranger = new PowerRanger(this.reality, new Rider(double.Parse(this.riderWeight.Text), double.Parse(this.bikeWeight.Text)));

            var segments = ranger.Morph(this.tcxFile);

            this.tcxFile.SaveAs(this.OutputFilename("tcx"));

            if (this.csvOut.Checked)
            {
                using (var stream = File.Open(this.OutputFilename("csv"), FileMode.CreateNew))
                {
                    var dataSegmentWriter = new CsvDataSegmentWriter(stream);
                    dataSegmentWriter.Write(segments);
                }
            }
        }

        /// <summary>
        /// Notify user that power calculation is complete.
        /// </summary>
        /// <param name="sender">
        /// Standard windows event argument: The sender.
        /// </param>
        /// <param name="e">
        /// Standard windows event argument: The e.
        /// </param>
        private void PowerCalcThreadRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.LogMessage("Completed power.");
        }
        #endregion
        
        #region Textbox Change events for options.
        /// <summary>
        /// Change the CRR value when text is changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void CrrTextChanged(object sender, EventArgs e)
        {
            this.reality.CoefficientOfRollingResistance = this.GetSafeDouble(
                this.Crr, this.reality.CoefficientOfRollingResistance);
        }

        /// <summary>
        /// Change the frontal areas in m2 when text is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void FrontalAreaTextChanged(object sender, EventArgs e)
        {
            this.reality.EffectiveFrontalArea = this.GetSafeDouble(this.frontalArea, this.reality.EffectiveFrontalArea);
        }

        /// <summary>
        /// Change the drag coefficient when text is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void DragCoefficientTextChanged(object sender, EventArgs e)
        {
            this.reality.DragCoefficient = this.GetSafeDouble(this.dragCoefficient, this.reality.DragCoefficient);
        }

        /// <summary>
        /// Reset the bike weight if it's changed from a valid, safe value.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void BikeWeightTextChanged(object sender, EventArgs e)
        {
            this.cachedBikeWeight = this.GetSafeDouble(this.bikeWeight, this.cachedBikeWeight);
        }

        /// <summary>
        /// Reset the rider weight if it's changed from a valid, safe value.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RiderWeightTextChanged(object sender, EventArgs e)
        {
            this.cachedRiderWeight = this.GetSafeDouble(this.riderWeight, this.cachedRiderWeight);
        }

        /// <summary>
        /// Reset the temperature if it's changed from a valid safe value.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TemperatureTextChanged(object sender, EventArgs e)
        {
            this.reality.Temperature = this.GetSafeDouble(this.temperature, this.reality.Temperature);
        }
        #endregion
    }
}
