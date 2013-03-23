// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Form1.Designer.cs" company="fuckingbrit.com">
//   Copyright (c) 2013 FuckingBrit.com
//   Source code available under a total unrestrictive, free, it's all yours licence.
//   Use at your own risk.
// </copyright>
// <summary>
//   Defines the MainForm type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Strava_Centurion
{
    /// <summary>
    /// Main form.
    /// </summary>
    public partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// Just the default.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabTCX = new System.Windows.Forms.TabPage();
            this.logText = new System.Windows.Forms.TextBox();
            this.filename = new System.Windows.Forms.TextBox();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonProcess = new System.Windows.Forms.Button();
            this.tabReality = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.csvOut = new System.Windows.Forms.CheckBox();
            this.acceleration = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.temperature = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.riderWeight = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.bikeWeight = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dragCoefficient = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.frontalArea = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Crr = new System.Windows.Forms.TextBox();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.fileParserThread = new System.ComponentModel.BackgroundWorker();
            this.powerCalcThread = new System.ComponentModel.BackgroundWorker();
            this.tabControl.SuspendLayout();
            this.tabTCX.SuspendLayout();
            this.tabReality.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabTCX);
            this.tabControl.Controls.Add(this.tabReality);
            this.tabControl.Location = new System.Drawing.Point(2, 6);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(541, 228);
            this.tabControl.TabIndex = 0;
            // 
            // tabTCX
            // 
            this.tabTCX.Controls.Add(this.logText);
            this.tabTCX.Controls.Add(this.filename);
            this.tabTCX.Controls.Add(this.buttonOpen);
            this.tabTCX.Controls.Add(this.buttonProcess);
            this.tabTCX.Location = new System.Drawing.Point(4, 22);
            this.tabTCX.Name = "tabTCX";
            this.tabTCX.Padding = new System.Windows.Forms.Padding(3);
            this.tabTCX.Size = new System.Drawing.Size(533, 202);
            this.tabTCX.TabIndex = 0;
            this.tabTCX.Text = "TCX";
            this.tabTCX.UseVisualStyleBackColor = true;
            // 
            // logText
            // 
            this.logText.AcceptsReturn = true;
            this.logText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logText.Location = new System.Drawing.Point(4, 34);
            this.logText.Multiline = true;
            this.logText.Name = "logText";
            this.logText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logText.Size = new System.Drawing.Size(523, 130);
            this.logText.TabIndex = 3;
            // 
            // filename
            // 
            this.filename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filename.Location = new System.Drawing.Point(4, 7);
            this.filename.Name = "filename";
            this.filename.ReadOnly = true;
            this.filename.Size = new System.Drawing.Size(485, 20);
            this.filename.TabIndex = 2;
            // 
            // buttonOpen
            // 
            this.buttonOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpen.Location = new System.Drawing.Point(495, 5);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(32, 23);
            this.buttonOpen.TabIndex = 1;
            this.buttonOpen.Text = "...";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.ButtonOpenClick);
            // 
            // buttonProcess
            // 
            this.buttonProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonProcess.Enabled = false;
            this.buttonProcess.Location = new System.Drawing.Point(437, 166);
            this.buttonProcess.Name = "buttonProcess";
            this.buttonProcess.Size = new System.Drawing.Size(90, 33);
            this.buttonProcess.TabIndex = 0;
            this.buttonProcess.Text = "Power Xtreme";
            this.buttonProcess.UseVisualStyleBackColor = true;
            this.buttonProcess.Click += new System.EventHandler(this.ButtonProcessClick);
            // 
            // tabReality
            // 
            this.tabReality.Controls.Add(this.groupBox4);
            this.tabReality.Controls.Add(this.groupBox3);
            this.tabReality.Controls.Add(this.groupBox2);
            this.tabReality.Controls.Add(this.groupBox1);
            this.tabReality.Location = new System.Drawing.Point(4, 22);
            this.tabReality.Name = "tabReality";
            this.tabReality.Padding = new System.Windows.Forms.Padding(3);
            this.tabReality.Size = new System.Drawing.Size(533, 202);
            this.tabReality.TabIndex = 1;
            this.tabReality.Text = "Reality";
            this.tabReality.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.csvOut);
            this.groupBox4.Controls.Add(this.acceleration);
            this.groupBox4.Location = new System.Drawing.Point(269, 136);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(256, 57);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Options";
            // 
            // csvOut
            // 
            this.csvOut.AutoSize = true;
            this.csvOut.Checked = global::Strava_Centurion.Properties.Settings.Default.csvOut;
            this.csvOut.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Strava_Centurion.Properties.Settings.Default, "csvOut", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.csvOut.Location = new System.Drawing.Point(134, 18);
            this.csvOut.Name = "csvOut";
            this.csvOut.Size = new System.Drawing.Size(81, 17);
            this.csvOut.TabIndex = 1;
            this.csvOut.Text = "Create CSV";
            this.csvOut.UseVisualStyleBackColor = true;
            // 
            // acceleration
            // 
            this.acceleration.AutoSize = true;
            this.acceleration.Checked = global::Strava_Centurion.Properties.Settings.Default.incAccel;
            this.acceleration.CheckState = System.Windows.Forms.CheckState.Checked;
            this.acceleration.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Strava_Centurion.Properties.Settings.Default, "incAccel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.acceleration.Location = new System.Drawing.Point(11, 17);
            this.acceleration.Name = "acceleration";
            this.acceleration.Size = new System.Drawing.Size(114, 17);
            this.acceleration.TabIndex = 0;
            this.acceleration.Text = "Apply Acceleration";
            this.acceleration.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.temperature);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(269, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(257, 61);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Conditions";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(185, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "°C";
            // 
            // temperature
            // 
            this.temperature.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Strava_Centurion.Properties.Settings.Default, "temperature", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.temperature.Location = new System.Drawing.Point(79, 23);
            this.temperature.Name = "temperature";
            this.temperature.Size = new System.Drawing.Size(100, 20);
            this.temperature.TabIndex = 1;
            this.temperature.Text = global::Strava_Centurion.Properties.Settings.Default.temperature;
            this.temperature.TextChanged += new System.EventHandler(this.TemperatureTextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Temperature";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.riderWeight);
            this.groupBox2.Location = new System.Drawing.Point(269, 80);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(257, 52);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rider Details";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(206, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Kg";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Rider Weight";
            // 
            // riderWeight
            // 
            this.riderWeight.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Strava_Centurion.Properties.Settings.Default, "riderWeight", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.riderWeight.Location = new System.Drawing.Point(103, 21);
            this.riderWeight.Name = "riderWeight";
            this.riderWeight.Size = new System.Drawing.Size(100, 20);
            this.riderWeight.TabIndex = 0;
            this.riderWeight.Text = global::Strava_Centurion.Properties.Settings.Default.riderWeight;
            this.riderWeight.TextChanged += new System.EventHandler(this.RiderWeightTextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.bikeWeight);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dragCoefficient);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.frontalArea);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Crr);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 188);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bike Details";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(10, 102);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(98, 13);
            this.linkLabel1.TabIndex = 9;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "How to Work it Out";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(220, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Kg";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Bike Weight";
            // 
            // bikeWeight
            // 
            this.bikeWeight.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Strava_Centurion.Properties.Settings.Default, "bikeWeight", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.bikeWeight.Location = new System.Drawing.Point(130, 123);
            this.bikeWeight.Name = "bikeWeight";
            this.bikeWeight.Size = new System.Drawing.Size(84, 20);
            this.bikeWeight.TabIndex = 6;
            this.bikeWeight.Text = global::Strava_Centurion.Properties.Settings.Default.bikeWeight;
            this.bikeWeight.TextChanged += new System.EventHandler(this.BikeWeightTextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Drag Coefficient";
            // 
            // dragCoefficient
            // 
            this.dragCoefficient.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Strava_Centurion.Properties.Settings.Default, "coefficientDrag", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dragCoefficient.Location = new System.Drawing.Point(129, 75);
            this.dragCoefficient.Name = "dragCoefficient";
            this.dragCoefficient.Size = new System.Drawing.Size(85, 20);
            this.dragCoefficient.TabIndex = 4;
            this.dragCoefficient.Text = global::Strava_Centurion.Properties.Settings.Default.coefficientDrag;
            this.dragCoefficient.TextChanged += new System.EventHandler(this.DragCoefficientTextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Frontal Area";
            // 
            // frontalArea
            // 
            this.frontalArea.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Strava_Centurion.Properties.Settings.Default, "frontalArea", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.frontalArea.Location = new System.Drawing.Point(128, 49);
            this.frontalArea.Name = "frontalArea";
            this.frontalArea.Size = new System.Drawing.Size(84, 20);
            this.frontalArea.TabIndex = 2;
            this.frontalArea.Text = global::Strava_Centurion.Properties.Settings.Default.frontalArea;
            this.frontalArea.TextChanged += new System.EventHandler(this.FrontalAreaTextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "Coefficient of Rolling Resistence";
            // 
            // Crr
            // 
            this.Crr.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Strava_Centurion.Properties.Settings.Default, "Crr", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Crr.Location = new System.Drawing.Point(129, 19);
            this.Crr.Name = "Crr";
            this.Crr.Size = new System.Drawing.Size(84, 20);
            this.Crr.TabIndex = 0;
            this.Crr.Text = global::Strava_Centurion.Properties.Settings.Default.Crr;
            this.Crr.TextChanged += new System.EventHandler(this.CrrTextChanged);
            // 
            // openFile
            // 
            this.openFile.DefaultExt = "tcx";
            this.openFile.Filter = "TCX|*.tcx";
            this.openFile.RestoreDirectory = true;
            this.openFile.SupportMultiDottedExtensions = true;
            this.openFile.Title = "Open Ride";
            // 
            // fileParserThread
            // 
            this.fileParserThread.WorkerReportsProgress = true;
            this.fileParserThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.FileParserThreadDoWork);
            this.fileParserThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.FileParserThreadRunWorkerCompleted);
            // 
            // powerCalcThread
            // 
            this.powerCalcThread.WorkerReportsProgress = true;
            this.powerCalcThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PowerCalcThreadDoWork);
            this.powerCalcThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.PowerCalcThreadRunWorkerCompleted);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 239);
            this.Controls.Add(this.tabControl);
            this.MinimumSize = new System.Drawing.Size(564, 277);
            this.Name = "MainForm";
            this.Text = "Strava Centurion";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.tabControl.ResumeLayout(false);
            this.tabTCX.ResumeLayout(false);
            this.tabTCX.PerformLayout();
            this.tabReality.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabTCX;
        private System.Windows.Forms.TabPage tabReality;
        private System.Windows.Forms.TextBox filename;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonProcess;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.TextBox logText;
        private System.ComponentModel.BackgroundWorker fileParserThread;
        private System.ComponentModel.BackgroundWorker powerCalcThread;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox Crr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox frontalArea;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox dragCoefficient;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox riderWeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox bikeWeight;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox temperature;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox acceleration;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox csvOut;
    }
}

