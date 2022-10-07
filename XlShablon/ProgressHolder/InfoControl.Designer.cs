namespace GH.XlShablon
{
    partial class InfoControl
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
            this.components = new System.ComponentModel.Container();
            this.labelInfo = new DevExpress.XtraEditors.LabelControl();
            this._dataSource = new System.Windows.Forms.BindingSource(this.components);
            this.panelInfo = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this._dataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // labelInfo
            // 
            this.labelInfo.Appearance.BackColor = System.Drawing.Color.White;
            this.labelInfo.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelInfo.Appearance.Options.UseBackColor = true;
            this.labelInfo.Appearance.Options.UseForeColor = true;
            this.labelInfo.Appearance.Options.UseTextOptions = true;
            this.labelInfo.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelInfo.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo.Location = new System.Drawing.Point(3, 37);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(530, 110);
            this.labelInfo.TabIndex = 0;
            this.labelInfo.Text = "Нет данных...";
            // 
            // panelInfo
            // 
            this.panelInfo.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInfo.Location = new System.Drawing.Point(3, 3);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(530, 34);
            this.panelInfo.TabIndex = 1;
            this.panelInfo.Visible = false;
            // 
            // InfoControlNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.panelInfo);
            this.Name = "InfoControlNew";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(536, 150);
            ((System.ComponentModel.ISupportInitialize)(this._dataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelInfo;
        private System.Windows.Forms.BindingSource _dataSource;
        private DevExpress.XtraEditors.PanelControl panelInfo;
    }
}
