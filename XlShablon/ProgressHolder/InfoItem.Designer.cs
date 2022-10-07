namespace GH.XlShablon
{
    partial class InfoItem
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
            this.panelInfo = new DevExpress.XtraEditors.PanelControl();
            this.message = new DevExpress.XtraEditors.LabelControl();
            this.caption = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelInfo)).BeginInit();
            this.panelInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelInfo
            // 
            this.panelInfo.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.panelInfo.Appearance.Options.UseBackColor = true;
            this.panelInfo.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelInfo.Controls.Add(this.message);
            this.panelInfo.Controls.Add(this.caption);
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInfo.Location = new System.Drawing.Point(0, 0);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Padding = new System.Windows.Forms.Padding(3);
            this.panelInfo.Size = new System.Drawing.Size(371, 24);
            this.panelInfo.TabIndex = 0;
            // 
            // message
            // 
            this.message.Appearance.BackColor = System.Drawing.SystemColors.Info;
            this.message.Appearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.message.Appearance.Options.UseBackColor = true;
            this.message.Appearance.Options.UseBorderColor = true;
            this.message.Appearance.Options.UseTextOptions = true;
            this.message.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.message.AutoEllipsis = true;
            this.message.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.message.Dock = System.Windows.Forms.DockStyle.Fill;
            this.message.Location = new System.Drawing.Point(78, 3);
            this.message.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.message.Name = "message";
            this.message.Padding = new System.Windows.Forms.Padding(3);
            this.message.Size = new System.Drawing.Size(290, 18);
            this.message.TabIndex = 1;
            this.message.Text = "Message";
            // 
            // caption
            // 
            this.caption.Appearance.BackColor = System.Drawing.SystemColors.HighlightText;
            this.caption.Appearance.Options.UseBackColor = true;
            this.caption.Appearance.Options.UseTextOptions = true;
            this.caption.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.caption.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.caption.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.caption.Dock = System.Windows.Forms.DockStyle.Left;
            this.caption.Location = new System.Drawing.Point(3, 3);
            this.caption.Name = "caption";
            this.caption.Padding = new System.Windows.Forms.Padding(3);
            this.caption.Size = new System.Drawing.Size(75, 18);
            this.caption.TabIndex = 0;
            this.caption.Text = "Caption:";
            // 
            // InfoItemNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.panelInfo);
            this.Name = "InfoItemNew";
            this.Size = new System.Drawing.Size(371, 105);
            ((System.ComponentModel.ISupportInitialize)(this.panelInfo)).EndInit();
            this.panelInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelInfo;
        private DevExpress.XtraEditors.LabelControl message;
        private DevExpress.XtraEditors.LabelControl caption;
    }
}
