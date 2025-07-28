namespace PressureTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            panel2 = new Panel();
            panel1 = new Panel();
            DGV_Exported = new DataGridView();
            panel5 = new Panel();
            button4 = new Button();
            LabelMonitoringHistories = new Label();
            panel3 = new Panel();
            label2 = new Label();
            LabelPressureValue = new Label();
            panel4 = new Panel();
            LabelError = new Label();
            button2 = new Button();
            button3 = new Button();
            button1 = new Button();
            ChartSensor = new ScottPlot.WinForms.FormsPlot();
            menuStrip1 = new MenuStrip();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            cOMSettingToolStripMenuItem = new ToolStripMenuItem();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DGV_Exported).BeginInit();
            panel5.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(panel4);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 24);
            panel2.Name = "panel2";
            panel2.Size = new Size(1904, 1017);
            panel2.TabIndex = 6;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(DGV_Exported);
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(LabelMonitoringHistories);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(1581, 83);
            panel1.Name = "panel1";
            panel1.Size = new Size(323, 934);
            panel1.TabIndex = 14;
            // 
            // DGV_Exported
            // 
            DGV_Exported.AllowUserToAddRows = false;
            DGV_Exported.AllowUserToDeleteRows = false;
            DGV_Exported.AllowUserToResizeColumns = false;
            DGV_Exported.AllowUserToResizeRows = false;
            DGV_Exported.BackgroundColor = Color.White;
            DGV_Exported.BorderStyle = BorderStyle.None;
            DGV_Exported.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DGV_Exported.Dock = DockStyle.Bottom;
            DGV_Exported.EditMode = DataGridViewEditMode.EditProgrammatically;
            DGV_Exported.Location = new Point(0, 17);
            DGV_Exported.MultiSelect = false;
            DGV_Exported.Name = "DGV_Exported";
            DGV_Exported.RowHeadersVisible = false;
            DGV_Exported.Size = new Size(321, 845);
            DGV_Exported.TabIndex = 8;
            DGV_Exported.SelectionChanged += DGV_Exported_SelectionChanged;
            // 
            // panel5
            // 
            panel5.BackColor = Color.White;
            panel5.Controls.Add(button4);
            panel5.Dock = DockStyle.Bottom;
            panel5.Location = new Point(0, 862);
            panel5.Name = "panel5";
            panel5.Size = new Size(321, 70);
            panel5.TabIndex = 10;
            // 
            // button4
            // 
            button4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button4.BackColor = Color.Cyan;
            button4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button4.Location = new Point(6, 7);
            button4.Name = "button4";
            button4.Size = new Size(309, 56);
            button4.TabIndex = 0;
            button4.Text = "Download Report";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // LabelMonitoringHistories
            // 
            LabelMonitoringHistories.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            LabelMonitoringHistories.AutoSize = true;
            LabelMonitoringHistories.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LabelMonitoringHistories.Location = new Point(5, 16);
            LabelMonitoringHistories.Name = "LabelMonitoringHistories";
            LabelMonitoringHistories.Size = new Size(168, 21);
            LabelMonitoringHistories.TabIndex = 9;
            LabelMonitoringHistories.Text = "Monitoring Histories";
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(label2);
            panel3.Controls.Add(LabelPressureValue);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(1581, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(323, 83);
            panel3.TabIndex = 12;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(6, 21);
            label2.Name = "label2";
            label2.Size = new Size(182, 21);
            label2.TabIndex = 11;
            label2.Text = "Current Pressure Value";
            // 
            // LabelPressureValue
            // 
            LabelPressureValue.AutoEllipsis = true;
            LabelPressureValue.BackColor = Color.White;
            LabelPressureValue.BorderStyle = BorderStyle.FixedSingle;
            LabelPressureValue.Dock = DockStyle.Bottom;
            LabelPressureValue.Font = new Font("Franklin Gothic Medium", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LabelPressureValue.Location = new Point(0, 16);
            LabelPressureValue.Name = "LabelPressureValue";
            LabelPressureValue.Size = new Size(323, 67);
            LabelPressureValue.TabIndex = 7;
            LabelPressureValue.Text = "0 psi";
            LabelPressureValue.TextAlign = ContentAlignment.BottomCenter;
            // 
            // panel4
            // 
            panel4.Controls.Add(LabelError);
            panel4.Controls.Add(button2);
            panel4.Controls.Add(button3);
            panel4.Controls.Add(button1);
            panel4.Controls.Add(ChartSensor);
            panel4.Dock = DockStyle.Left;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(1581, 1017);
            panel4.TabIndex = 13;
            // 
            // LabelError
            // 
            LabelError.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            LabelError.BackColor = Color.White;
            LabelError.Location = new Point(513, 946);
            LabelError.Name = "LabelError";
            LabelError.Size = new Size(1045, 60);
            LabelError.TabIndex = 4;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button2.BackColor = Color.FromArgb(255, 255, 128);
            button2.Font = new Font("Franklin Gothic Medium", 14.25F);
            button2.Location = new Point(155, 946);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(140, 60);
            button2.TabIndex = 1;
            button2.Text = "PAUSE";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button3.BackColor = Color.FromArgb(255, 128, 128);
            button3.Font = new Font("Franklin Gothic Medium", 14.25F);
            button3.Location = new Point(299, 946);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.Size = new Size(140, 60);
            button3.TabIndex = 2;
            button3.Text = "STOP";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button1.BackColor = Color.FromArgb(128, 255, 128);
            button1.Font = new Font("Franklin Gothic Medium", 14.25F);
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(11, 946);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(140, 60);
            button1.TabIndex = 0;
            button1.Text = "START";
            button1.TextImageRelation = TextImageRelation.ImageBeforeText;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // ChartSensor
            // 
            ChartSensor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ChartSensor.BackColor = Color.White;
            ChartSensor.DisplayScale = 1F;
            ChartSensor.Location = new Point(0, 0);
            ChartSensor.Name = "ChartSensor";
            ChartSensor.Size = new Size(1581, 930);
            ChartSensor.TabIndex = 3;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1904, 24);
            menuStrip1.TabIndex = 7;
            menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cOMSettingToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // cOMSettingToolStripMenuItem
            // 
            cOMSettingToolStripMenuItem.Name = "cOMSettingToolStripMenuItem";
            cOMSettingToolStripMenuItem.Size = new Size(142, 22);
            cOMSettingToolStripMenuItem.Text = "COM Setting";
            cOMSettingToolStripMenuItem.Click += cOMSettingToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1904, 1041);
            Controls.Add(panel2);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(2);
            Name = "Form1";
            Text = "Pressure Analyzers";
            panel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DGV_Exported).EndInit();
            panel5.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panel2;
        private Button button3;
        private Button button2;
        private Button button1;
        private ScottPlot.WinForms.FormsPlot ChartSensor;
        private Label LabelPressureValue;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem cOMSettingToolStripMenuItem;
        private DataGridView DGV_Exported;
        private Label label2;
        private Label LabelMonitoringHistories;
        private Panel panel3;
        private Panel panel4;
        private Panel panel1;
        private Panel panel5;
        private Button button4;
        private Label LabelError;
    }
}
