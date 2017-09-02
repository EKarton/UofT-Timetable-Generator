namespace UoftTimetableGenerator.DataModels.GeneratorAnalyzer
{
    partial class GAAnalyzerForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.scoresChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mutationRateTxtbox = new System.Windows.Forms.TextBox();
            this.crossoverRateTxtbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numGenTxtbox = new System.Windows.Forms.TextBox();
            this.popSizeTxtbox = new System.Windows.Forms.TextBox();
            this.crossoverMethodBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.runBttn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.performanceChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.diversityChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.scoresChart)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.performanceChart)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.diversityChart)).BeginInit();
            this.SuspendLayout();
            // 
            // scoresChart
            // 
            this.scoresChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.scoresChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.scoresChart.Legends.Add(legend1);
            this.scoresChart.Location = new System.Drawing.Point(6, 6);
            this.scoresChart.Name = "scoresChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.scoresChart.Series.Add(series1);
            this.scoresChart.Size = new System.Drawing.Size(1064, 620);
            this.scoresChart.TabIndex = 0;
            this.scoresChart.Text = "chart1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mutation Rate:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Crossover Rate";
            // 
            // mutationRateTxtbox
            // 
            this.mutationRateTxtbox.Location = new System.Drawing.Point(144, 10);
            this.mutationRateTxtbox.Name = "mutationRateTxtbox";
            this.mutationRateTxtbox.Size = new System.Drawing.Size(100, 26);
            this.mutationRateTxtbox.TabIndex = 3;
            this.mutationRateTxtbox.Text = "0.01";
            // 
            // crossoverRateTxtbox
            // 
            this.crossoverRateTxtbox.Location = new System.Drawing.Point(144, 43);
            this.crossoverRateTxtbox.Name = "crossoverRateTxtbox";
            this.crossoverRateTxtbox.Size = new System.Drawing.Size(100, 26);
            this.crossoverRateTxtbox.TabIndex = 4;
            this.crossoverRateTxtbox.Text = "0.9";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(276, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Num. of Generations";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(276, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Population Size:";
            // 
            // numGenTxtbox
            // 
            this.numGenTxtbox.Location = new System.Drawing.Point(438, 10);
            this.numGenTxtbox.Name = "numGenTxtbox";
            this.numGenTxtbox.Size = new System.Drawing.Size(100, 26);
            this.numGenTxtbox.TabIndex = 7;
            this.numGenTxtbox.Text = "100";
            // 
            // popSizeTxtbox
            // 
            this.popSizeTxtbox.Location = new System.Drawing.Point(438, 43);
            this.popSizeTxtbox.Name = "popSizeTxtbox";
            this.popSizeTxtbox.Size = new System.Drawing.Size(100, 26);
            this.popSizeTxtbox.TabIndex = 8;
            this.popSizeTxtbox.Text = "16";
            // 
            // crossoverMethodBox
            // 
            this.crossoverMethodBox.FormattingEnabled = true;
            this.crossoverMethodBox.Items.AddRange(new object[] {
            "Old Crossover",
            "Single Point Crossover",
            "Double Point Crossover",
            "Uniform Crossover"});
            this.crossoverMethodBox.Location = new System.Drawing.Point(714, 8);
            this.crossoverMethodBox.Name = "crossoverMethodBox";
            this.crossoverMethodBox.Size = new System.Drawing.Size(121, 28);
            this.crossoverMethodBox.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(566, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(142, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "Crossover Method:";
            // 
            // runBttn
            // 
            this.runBttn.Location = new System.Drawing.Point(984, 8);
            this.runBttn.Name = "runBttn";
            this.runBttn.Size = new System.Drawing.Size(106, 61);
            this.runBttn.TabIndex = 11;
            this.runBttn.Text = "Run";
            this.runBttn.UseVisualStyleBackColor = true;
            this.runBttn.Click += new System.EventHandler(this.runBttn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 75);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1084, 665);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.scoresChart);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1076, 632);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Scores";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.performanceChart);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1076, 632);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Performance";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // performanceChart
            // 
            this.performanceChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.Name = "ChartArea1";
            this.performanceChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.performanceChart.Legends.Add(legend2);
            this.performanceChart.Location = new System.Drawing.Point(0, 0);
            this.performanceChart.Name = "performanceChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.performanceChart.Series.Add(series2);
            this.performanceChart.Size = new System.Drawing.Size(1080, 632);
            this.performanceChart.TabIndex = 0;
            this.performanceChart.Text = "chart1";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.diversityChart);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1076, 632);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Diversity";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // diversityChart
            // 
            this.diversityChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea3.Name = "ChartArea1";
            this.diversityChart.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.diversityChart.Legends.Add(legend3);
            this.diversityChart.Location = new System.Drawing.Point(3, 3);
            this.diversityChart.Name = "diversityChart";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.diversityChart.Series.Add(series3);
            this.diversityChart.Size = new System.Drawing.Size(1070, 626);
            this.diversityChart.TabIndex = 0;
            this.diversityChart.Text = "chart1";
            // 
            // GAAnalyzerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1108, 752);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.runBttn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.crossoverMethodBox);
            this.Controls.Add(this.popSizeTxtbox);
            this.Controls.Add(this.numGenTxtbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.crossoverRateTxtbox);
            this.Controls.Add(this.mutationRateTxtbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GAAnalyzerForm";
            this.Text = "Genetic Algorithm Analyzer";
            this.Load += new System.EventHandler(this.GAAnalyzerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.scoresChart)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.performanceChart)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.diversityChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart scoresChart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox mutationRateTxtbox;
        private System.Windows.Forms.TextBox crossoverRateTxtbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox numGenTxtbox;
        private System.Windows.Forms.TextBox popSizeTxtbox;
        private System.Windows.Forms.ComboBox crossoverMethodBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button runBttn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataVisualization.Charting.Chart performanceChart;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataVisualization.Charting.Chart diversityChart;
    }
}

