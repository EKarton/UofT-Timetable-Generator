namespace Genetic_Algorithm_Analyzer
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
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
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // chart
            // 
            chartArea5.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.chart.Legends.Add(legend5);
            this.chart.Location = new System.Drawing.Point(12, 88);
            this.chart.Name = "chart";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.chart.Series.Add(series5);
            this.chart.Size = new System.Drawing.Size(1078, 651);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart1";
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
            // GAAnalyzerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1102, 756);
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
            this.Controls.Add(this.chart);
            this.Name = "GAAnalyzerForm";
            this.Text = "Genetic Algorithm Analyzer";
            this.Load += new System.EventHandler(this.GAAnalyzerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
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
    }
}

