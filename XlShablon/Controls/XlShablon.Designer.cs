namespace GH.XlShablon
{
    partial class XlShablon
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XlShablon));
            this.panelProc = new DevExpress.XtraEditors.PanelControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.trackBar = new DevExpress.XtraEditors.TrackBarControl();
            this.panelExcel = new DevExpress.XtraEditors.PanelControl();
            this.labelState = new DevExpress.XtraEditors.LabelControl();
            this.panelButtons = new DevExpress.XtraEditors.PanelControl();
            this.progressBar = new DevExpress.XtraEditors.ProgressBarControl();
            this.acceptButton = new DevExpress.XtraEditors.SimpleButton();
            this.clearButton = new DevExpress.XtraEditors.SimpleButton();
            this.stopButton = new DevExpress.XtraEditors.SimpleButton();
            this.loadButton = new DevExpress.XtraEditors.SimpleButton();
            this.openXlDialog = new System.Windows.Forms.OpenFileDialog();
            this.panelMenus = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelProc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelExcel)).BeginInit();
            this.panelExcel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelButtons)).BeginInit();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelMenus)).BeginInit();
            this.panelMenus.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelProc
            // 
            this.panelProc.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelProc.Location = new System.Drawing.Point(5, 43);
            this.panelProc.Name = "panelProc";
            this.panelProc.Size = new System.Drawing.Size(311, 221);
            this.panelProc.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(316, 43);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 221);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // trackBar
            // 
            this.trackBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.trackBar.EditValue = null;
            this.trackBar.Location = new System.Drawing.Point(585, 43);
            this.trackBar.Name = "trackBar";
            this.trackBar.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.trackBar.Properties.InvertLayout = true;
            this.trackBar.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.trackBar.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.trackBar.Properties.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar.Properties.TickFrequency = 10;
            this.trackBar.Properties.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar.Size = new System.Drawing.Size(45, 221);
            this.trackBar.TabIndex = 4;
            this.trackBar.TabStop = false;
            // 
            // panelExcel
            // 
            this.panelExcel.Controls.Add(this.labelState);
            this.panelExcel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelExcel.Location = new System.Drawing.Point(322, 43);
            this.panelExcel.Name = "panelExcel";
            this.panelExcel.Size = new System.Drawing.Size(263, 221);
            this.panelExcel.TabIndex = 5;
            // 
            // labelState
            // 
            this.labelState.Appearance.BackColor = System.Drawing.Color.White;
            this.labelState.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelState.Appearance.Options.UseBackColor = true;
            this.labelState.Appearance.Options.UseForeColor = true;
            this.labelState.Appearance.Options.UseTextOptions = true;
            this.labelState.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelState.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelState.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelState.Location = new System.Drawing.Point(2, 2);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(259, 217);
            this.labelState.TabIndex = 0;
            this.labelState.Text = "Нет данных...";
            // 
            // panelButtons
            // 
            this.panelButtons.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelButtons.Controls.Add(this.progressBar);
            this.panelButtons.Controls.Add(this.acceptButton);
            this.panelButtons.Controls.Add(this.clearButton);
            this.panelButtons.Controls.Add(this.stopButton);
            this.panelButtons.Controls.Add(this.loadButton);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(2, 2);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(5);
            this.panelButtons.Size = new System.Drawing.Size(621, 34);
            this.panelButtons.TabIndex = 1;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(398, 8);
            this.progressBar.Name = "progressBar";
            this.progressBar.Properties.ShowTitle = true;
            this.progressBar.Properties.TextOrientation = DevExpress.Utils.Drawing.TextOrientation.Horizontal;
            this.progressBar.Size = new System.Drawing.Size(215, 18);
            this.progressBar.TabIndex = 6;
            this.progressBar.Visible = false;
            // 
            // acceptButton
            // 
            this.acceptButton.Appearance.Options.UseTextOptions = true;
            this.acceptButton.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.acceptButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("acceptButton.ImageOptions.Image")));
            this.acceptButton.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.acceptButton.ImageOptions.ImageToTextIndent = 5;
            this.acceptButton.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.acceptButton.Location = new System.Drawing.Point(300, 6);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(92, 22);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.TabStop = false;
            this.acceptButton.Text = "Применить";
            this.acceptButton.ToolTip = "Применить шаблон к данным и продолжить";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Appearance.Options.UseTextOptions = true;
            this.clearButton.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.clearButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("clearButton.ImageOptions.Image")));
            this.clearButton.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.clearButton.ImageOptions.ImageToTextIndent = 5;
            this.clearButton.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.clearButton.Location = new System.Drawing.Point(202, 6);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(92, 22);
            this.clearButton.TabIndex = 0;
            this.clearButton.TabStop = false;
            this.clearButton.Text = "Очистить";
            this.clearButton.ToolTip = "Очистить шаблон";
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Appearance.Options.UseTextOptions = true;
            this.stopButton.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.stopButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("stopButton.ImageOptions.Image")));
            this.stopButton.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.stopButton.ImageOptions.ImageToTextIndent = 5;
            this.stopButton.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.stopButton.Location = new System.Drawing.Point(104, 6);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(92, 22);
            this.stopButton.TabIndex = 0;
            this.stopButton.TabStop = false;
            this.stopButton.Text = "Прервать";
            this.stopButton.ToolTip = "Прервать загрузку";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Appearance.Options.UseTextOptions = true;
            this.loadButton.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.loadButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("loadButton.ImageOptions.Image")));
            this.loadButton.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.loadButton.ImageOptions.ImageToTextIndent = 5;
            this.loadButton.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.loadButton.Location = new System.Drawing.Point(6, 6);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(92, 22);
            this.loadButton.TabIndex = 0;
            this.loadButton.TabStop = false;
            this.loadButton.Text = "Загрузить";
            this.loadButton.ToolTip = "Перезагрузить тое же файл";
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // openXlDialog
            // 
            this.openXlDialog.Filter = "Excel 2003-2007|*.xls;*.xlsx|CSV Files csv-txt |*.csv;*.txt";
            // 
            // panelMenus
            // 
            this.panelMenus.AutoSize = true;
            this.panelMenus.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelMenus.Controls.Add(this.panelButtons);
            this.panelMenus.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMenus.Location = new System.Drawing.Point(5, 5);
            this.panelMenus.Name = "panelMenus";
            this.panelMenus.Size = new System.Drawing.Size(625, 38);
            this.panelMenus.TabIndex = 6;
            // 
            // XlShablon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelExcel);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panelProc);
            this.Controls.Add(this.panelMenus);
            this.Name = "XlShablon";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(635, 269);
            this.Load += new System.EventHandler(this.XlShablon_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelProc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelExcel)).EndInit();
            this.panelExcel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelButtons)).EndInit();
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelMenus)).EndInit();
            this.panelMenus.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelProc;
        private System.Windows.Forms.Splitter splitter1;
        private DevExpress.XtraEditors.TrackBarControl trackBar;
        private DevExpress.XtraEditors.PanelControl panelExcel;
        private DevExpress.XtraEditors.LabelControl labelState;
        private DevExpress.XtraEditors.PanelControl panelButtons;
        private DevExpress.XtraEditors.SimpleButton loadButton;
        private DevExpress.XtraEditors.SimpleButton acceptButton;
        private DevExpress.XtraEditors.SimpleButton clearButton;
        private DevExpress.XtraEditors.SimpleButton stopButton;
        private System.Windows.Forms.OpenFileDialog openXlDialog;
        private DevExpress.XtraEditors.ProgressBarControl progressBar;
        private DevExpress.XtraEditors.PanelControl panelMenus;
    }
}
