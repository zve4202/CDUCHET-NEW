namespace Tester.forms
{
    partial class ExcelVsDbControl
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
            this.pages = new DevExpress.XtraTab.XtraTabControl();
            this.excel_1_Page = new DevExpress.XtraTab.XtraTabPage();
            this.excel_1 = new GH.XlShablon.ExcelVsDbShablon();
            this.excelVsDbSetting = new Tester.forms.ExcelDbProcSetting();
            this.resultPage = new DevExpress.XtraTab.XtraTabPage();
            this.excelResult = new Tester.forms.TestResultControl();
            ((System.ComponentModel.ISupportInitialize)(this.pages)).BeginInit();
            this.pages.SuspendLayout();
            this.excel_1_Page.SuspendLayout();
            this.resultPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // pages
            // 
            this.pages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pages.HeaderOrientation = DevExpress.XtraTab.TabOrientation.Horizontal;
            this.pages.Location = new System.Drawing.Point(5, 5);
            this.pages.Name = "pages";
            this.pages.SelectedTabPage = this.excel_1_Page;
            this.pages.ShowTabHeader = DevExpress.Utils.DefaultBoolean.True;
            this.pages.Size = new System.Drawing.Size(1047, 518);
            this.pages.TabIndex = 0;
            this.pages.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.excel_1_Page,
            this.resultPage});
            this.pages.SelectedPageChanging += new DevExpress.XtraTab.TabPageChangingEventHandler(this.pages_SelectedPageChanging);
            // 
            // excel_1_Page
            // 
            this.excel_1_Page.Controls.Add(this.excel_1);
            this.excel_1_Page.Name = "excel_1_Page";
            this.excel_1_Page.Padding = new System.Windows.Forms.Padding(5);
            this.excel_1_Page.Size = new System.Drawing.Size(1041, 490);
            this.excel_1_Page.Text = "Excel-файл для сравнения";
            // 
            // excel_1
            // 
            this.excel_1.AllowDrop = true;
            this.excel_1.DataProcessor = null;
            this.excel_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.excel_1.ExtMenuControl = this.excelVsDbSetting;
            this.excel_1.FileName = null;
            this.excel_1.Location = new System.Drawing.Point(5, 5);
            this.excel_1.Name = "excel_1";
            this.excel_1.Padding = new System.Windows.Forms.Padding(5);
            this.excel_1.RemoveNotUsedFields = false;
            this.excel_1.Size = new System.Drawing.Size(1031, 480);
            this.excel_1.TabIndex = 0;
            // 
            // excelVsDbSetting
            // 
            this.excelVsDbSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.excelVsDbSetting.Location = new System.Drawing.Point(0, 34);
            this.excelVsDbSetting.Name = "excelVsDbSetting";
            this.excelVsDbSetting.Size = new System.Drawing.Size(1021, 33);
            this.excelVsDbSetting.TabIndex = 0;
            // 
            // resultPage
            // 
            this.resultPage.Controls.Add(this.excelResult);
            this.resultPage.Name = "resultPage";
            this.resultPage.Size = new System.Drawing.Size(1041, 490);
            this.resultPage.Text = "Результат сравнения";
            // 
            // excelResult
            // 
            this.excelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.excelResult.Location = new System.Drawing.Point(0, 0);
            this.excelResult.Name = "excelResult";
            this.excelResult.Padding = new System.Windows.Forms.Padding(5);
            this.excelResult.Size = new System.Drawing.Size(1041, 490);
            this.excelResult.TabIndex = 0;
            // 
            // ExcelVsDbControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pages);
            this.Name = "ExcelVsDbControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(1057, 528);
            ((System.ComponentModel.ISupportInitialize)(this.pages)).EndInit();
            this.pages.ResumeLayout(false);
            this.excel_1_Page.ResumeLayout(false);
            this.resultPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl pages;
        private DevExpress.XtraTab.XtraTabPage excel_1_Page;
        private DevExpress.XtraTab.XtraTabPage resultPage;
        private GH.XlShablon.ExcelVsDbShablon excel_1;
        private TestResultControl excelResult;
        private ExcelDbProcSetting excelVsDbSetting;
    }
}
