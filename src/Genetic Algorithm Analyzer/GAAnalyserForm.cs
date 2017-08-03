using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using UoftTimetableGenerator.DataModels;
using UoftTimetableGenerator.Generator;

namespace Genetic_Algorithm_Analyzer
{
    public partial class GAAnalyzerForm : Form
    {
        private List<Course> courses = new List<Course>();

        public GAAnalyzerForm()
        {
            InitializeComponent();
            crossoverMethodBox.SelectedIndex = 0;
        }

        private void GAAnalyzerForm_Load(object sender, EventArgs e)
        {
            LoadData();
            UpdateStats();
        }

        private void LoadData()
        {
            courses = UoftDatabaseService.GetCourses(new string[] { "MAT137Y1-Y", "COG250Y1-Y", "CSC148H1-F", "CSC165H1-F", "ENV100H1-F" });
        }

        private void UpdateStats()
        {
            // Get the parameters
            double mutationRate = Convert.ToDouble(mutationRateTxtbox.Text);
            double crossoverRate = Convert.ToDouble(crossoverRateTxtbox.Text);
            int numGenerations = Convert.ToInt32(numGenTxtbox.Text);
            int populationSize = Convert.ToInt32(popSizeTxtbox.Text);
            string crossoverType = crossoverMethodBox.Text;

            // Get average scores per generation for over 30 trials
            int numTrials = 30;
            double[] avgScores = new double[numGenerations];
            double[] maxScores = new double[numGenerations];
            for (int i = 0; i < numTrials; i++)
            {
                // Make generator with params
                GAGenerator generator = new GAGenerator(courses)
                {
                    MutationRate = mutationRate,
                    CrossoverRate = crossoverRate,
                    NumGenerations = numGenerations,
                    PopulationSize = populationSize,
                    CrossoverType = crossoverType
                };

                // Run the generator and get scores
                double[] curAvgScores = generator.GetAvgScoresPerGeneration();
                double[] curMaxScores = generator.GetMaxScoresPerGeneration();
                for (int j = 0; j < numGenerations; j++)
                {
                    avgScores[j] += curAvgScores[j];
                    maxScores[j] += curMaxScores[j];
                }
            }
            for (int i = 0; i < numGenerations; i++)
            {
                avgScores[i] /= numTrials;
                maxScores[i] /= numTrials;
            }

            // Set up the data
            chart.Series.Clear();
            chart.ChartAreas[0].AxisY.Minimum = 0;
            chart.Series.Add("Average Scores");
            chart.Series["Average Scores"].ChartType = SeriesChartType.Line;
            chart.Series.Add("Max Scores");
            chart.Series["Max Scores"].ChartType = SeriesChartType.Line;

            // Add the average / maxscores to the chart by their type
            for (int i = 0; i < numGenerations; i++)
            {
                int generation = i;
                double avgScore = avgScores[i];
                double maxScore = maxScores[i];
                chart.Series["Average Scores"].Points.Add(new DataPoint(generation, avgScore));
                chart.Series["Max Scores"].Points.Add(new DataPoint(generation, maxScore));
            }
        }

        private void runBttn_Click(object sender, EventArgs e)
        {
            UpdateStats();
        }
    }
}
