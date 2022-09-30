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
            this.panelProc = new DevExpress.XtraEditors.PanelControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.trackBar = new DevExpress.XtraEditors.TrackBarControl();
            this.panelExcel = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelProc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelExcel)).BeginInit();
            this.SuspendLayout();
            // 
            // panelProc
            // 
            this.panelProc.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelProc.Location = new System.Drawing.Point(0, 0);
            this.panelProc.Name = "panelProc";
            this.panelProc.Size = new System.Drawing.Size(240, 269);
            this.panelProc.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(240, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 269);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // trackBar
            // 
            this.trackBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.trackBar.EditValue = null;
            this.trackBar.Location = new System.Drawing.Point(590, 0);
            this.trackBar.Name = "trackBar";
            this.trackBar.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.trackBar.Properties.InvertLayout = true;
            this.trackBar.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.trackBar.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.trackBar.Properties.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar.Properties.TickFrequency = 10;
            this.trackBar.Properties.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar.Size = new System.Drawing.Size(45, 269);
            this.trackBar.TabIndex = 4;
            this.trackBar.TabStop = false;
            // 
            // panelExcel
            // 
            this.panelExcel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelExcel.Location = new System.Drawing.Point(246, 0);
            this.panelExcel.Name = "panelExcel";
            this.panelExcel.Size = new System.Drawing.Size(344, 269);
            this.panelExcel.TabIndex = 5;
            // 
            // XlShablon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelExcel);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panelProc);
            this.Name = "XlShablon";
            this.Size = new System.Drawing.Size(635, 269);
            ((System.ComponentModel.ISupportInitialize)(this.panelProc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelExcel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelProc;
        private System.Windows.Forms.Splitter splitter1;
        private DevExpress.XtraEditors.TrackBarControl trackBar;
        private DevExpress.XtraEditors.PanelControl panelExcel;
    }
}
