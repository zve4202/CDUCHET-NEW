namespace Tester
{
    partial class mainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.mainPages = new DevExpress.XtraTab.XtraTabControl();
            this.xlVxlPage = new DevExpress.XtraTab.XtraTabPage();
            this.excelControl1 = new Tester.forms.ExcelVsExcelControl();
            this.xlVdbPage = new DevExpress.XtraTab.XtraTabPage();
            this.excelVsDbControl1 = new Tester.forms.ExcelVsDbControl();
            ((System.ComponentModel.ISupportInitialize)(this.mainPages)).BeginInit();
            this.mainPages.SuspendLayout();
            this.xlVxlPage.SuspendLayout();
            this.xlVdbPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPages
            // 
            this.mainPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPages.HeaderOrientation = DevExpress.XtraTab.TabOrientation.Horizontal;
            this.mainPages.Location = new System.Drawing.Point(5, 5);
            this.mainPages.MultiLine = DevExpress.Utils.DefaultBoolean.False;
            this.mainPages.Name = "mainPages";
            this.mainPages.SelectedTabPage = this.xlVxlPage;
            this.mainPages.ShowTabHeader = DevExpress.Utils.DefaultBoolean.True;
            this.mainPages.Size = new System.Drawing.Size(1000, 494);
            this.mainPages.TabIndex = 0;
            this.mainPages.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xlVxlPage,
            this.xlVdbPage});
            // 
            // xlVxlPage
            // 
            this.xlVxlPage.Controls.Add(this.excelControl1);
            this.xlVxlPage.Name = "xlVxlPage";
            this.xlVxlPage.Padding = new System.Windows.Forms.Padding(5);
            this.xlVxlPage.Size = new System.Drawing.Size(994, 466);
            this.xlVxlPage.Text = "Excel vs Excel";
            // 
            // excelControl1
            // 
            this.excelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.excelControl1.Location = new System.Drawing.Point(5, 5);
            this.excelControl1.Name = "excelControl1";
            this.excelControl1.Padding = new System.Windows.Forms.Padding(5);
            this.excelControl1.Size = new System.Drawing.Size(984, 456);
            this.excelControl1.TabIndex = 0;
            // 
            // xlVdbPage
            // 
            this.xlVdbPage.Controls.Add(this.excelVsDbControl1);
            this.xlVdbPage.Name = "xlVdbPage";
            this.xlVdbPage.Padding = new System.Windows.Forms.Padding(5);
            this.xlVdbPage.Size = new System.Drawing.Size(994, 466);
            this.xlVdbPage.Text = "Excel vs Database";
            // 
            // excelVsDbControl1
            // 
            this.excelVsDbControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.excelVsDbControl1.Location = new System.Drawing.Point(5, 5);
            this.excelVsDbControl1.Name = "excelVsDbControl1";
            this.excelVsDbControl1.Padding = new System.Windows.Forms.Padding(5);
            this.excelVsDbControl1.Size = new System.Drawing.Size(984, 456);
            this.excelVsDbControl1.TabIndex = 0;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 504);
            this.Controls.Add(this.mainPages);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mainForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Различные сравнения";
            ((System.ComponentModel.ISupportInitialize)(this.mainPages)).EndInit();
            this.mainPages.ResumeLayout(false);
            this.xlVxlPage.ResumeLayout(false);
            this.xlVdbPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl mainPages;
        private DevExpress.XtraTab.XtraTabPage xlVxlPage;
        private forms.ExcelVsExcelControl excelControl1;
        private DevExpress.XtraTab.XtraTabPage xlVdbPage;
        private forms.ExcelVsDbControl excelVsDbControl1;
    }
}

