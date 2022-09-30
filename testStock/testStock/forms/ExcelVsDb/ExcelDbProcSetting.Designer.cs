namespace Tester.forms
{
    partial class ExcelDbProcSetting
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
            this.panelTypes = new DevExpress.XtraEditors.PanelControl();
            this.comboType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelStocks = new DevExpress.XtraEditors.PanelControl();
            this.comboStock = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.panelClients = new DevExpress.XtraEditors.PanelControl();
            this.comboClients = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelTypes)).BeginInit();
            this.panelTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelStocks)).BeginInit();
            this.panelStocks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboStock.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelClients)).BeginInit();
            this.panelClients.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboClients.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTypes
            // 
            this.panelTypes.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelTypes.Controls.Add(this.comboType);
            this.panelTypes.Controls.Add(this.labelControl1);
            this.panelTypes.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTypes.Location = new System.Drawing.Point(0, 0);
            this.panelTypes.Name = "panelTypes";
            this.panelTypes.Padding = new System.Windows.Forms.Padding(5);
            this.panelTypes.Size = new System.Drawing.Size(276, 30);
            this.panelTypes.TabIndex = 0;
            // 
            // comboType
            // 
            this.comboType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboType.EditValue = "Склад (приод-остаток)";
            this.comboType.Location = new System.Drawing.Point(107, 5);
            this.comboType.Name = "comboType";
            this.comboType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboType.Properties.Items.AddRange(new object[] {
            "Склад (приод-остаток)",
            "Реаризатор(ы) (остатки)",
            "Склад + Реаризаторы"});
            this.comboType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboType.Size = new System.Drawing.Size(164, 20);
            this.comboType.TabIndex = 3;
            this.comboType.ToolTip = "Выбрать тип сканирования";
            this.comboType.SelectedIndexChanged += new System.EventHandler(this.comboType_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelControl1.Location = new System.Drawing.Point(5, 5);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Padding = new System.Windows.Forms.Padding(3);
            this.labelControl1.Size = new System.Drawing.Size(102, 19);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Тип сканирования:";
            // 
            // panelStocks
            // 
            this.panelStocks.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelStocks.Controls.Add(this.comboStock);
            this.panelStocks.Controls.Add(this.labelControl2);
            this.panelStocks.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelStocks.Location = new System.Drawing.Point(276, 0);
            this.panelStocks.Name = "panelStocks";
            this.panelStocks.Padding = new System.Windows.Forms.Padding(5);
            this.panelStocks.Size = new System.Drawing.Size(185, 30);
            this.panelStocks.TabIndex = 1;
            // 
            // comboStock
            // 
            this.comboStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboStock.EditValue = "По всем складам";
            this.comboStock.Location = new System.Drawing.Point(67, 5);
            this.comboStock.Name = "comboStock";
            this.comboStock.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboStock.Properties.Items.AddRange(new object[] {
            "По всем складам",
            "NEW",
            "USED"});
            this.comboStock.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboStock.Size = new System.Drawing.Size(113, 20);
            this.comboStock.TabIndex = 1;
            this.comboStock.ToolTip = "Выбрать склад для проверки...";
            // 
            // labelControl2
            // 
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelControl2.Location = new System.Drawing.Point(5, 5);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Padding = new System.Windows.Forms.Padding(3);
            this.labelControl2.Size = new System.Drawing.Size(62, 19);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "По складу:";
            // 
            // panelClients
            // 
            this.panelClients.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelClients.Controls.Add(this.comboClients);
            this.panelClients.Controls.Add(this.labelControl3);
            this.panelClients.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelClients.Location = new System.Drawing.Point(461, 0);
            this.panelClients.Name = "panelClients";
            this.panelClients.Padding = new System.Windows.Forms.Padding(5);
            this.panelClients.Size = new System.Drawing.Size(298, 30);
            this.panelClients.TabIndex = 2;
            this.panelClients.Visible = false;
            // 
            // comboClients
            // 
            this.comboClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboClients.Location = new System.Drawing.Point(74, 5);
            this.comboClients.Name = "comboClients";
            this.comboClients.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboClients.Properties.NullText = "";
            this.comboClients.Properties.PopupSizeable = false;
            this.comboClients.Size = new System.Drawing.Size(219, 20);
            this.comboClients.TabIndex = 3;
            this.comboClients.ToolTip = "Выбрать реализаторра";
            // 
            // labelControl3
            // 
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.labelControl3.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelControl3.Location = new System.Drawing.Point(5, 5);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Padding = new System.Windows.Forms.Padding(3);
            this.labelControl3.Size = new System.Drawing.Size(69, 19);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "Реализатор:";
            // 
            // ExcelDbProcSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelClients);
            this.Controls.Add(this.panelStocks);
            this.Controls.Add(this.panelTypes);
            this.Name = "ExcelDbProcSetting";
            this.Size = new System.Drawing.Size(870, 30);
            ((System.ComponentModel.ISupportInitialize)(this.panelTypes)).EndInit();
            this.panelTypes.ResumeLayout(false);
            this.panelTypes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelStocks)).EndInit();
            this.panelStocks.ResumeLayout(false);
            this.panelStocks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboStock.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelClients)).EndInit();
            this.panelClients.ResumeLayout(false);
            this.panelClients.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboClients.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelTypes;
        private DevExpress.XtraEditors.PanelControl panelStocks;
        private DevExpress.XtraEditors.ComboBoxEdit comboStock;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit comboType;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelClients;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LookUpEdit comboClients;
    }
}
