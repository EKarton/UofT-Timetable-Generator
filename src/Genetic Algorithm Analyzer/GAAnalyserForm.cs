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

namespace UoftTimetableGenerator.DataModels.GeneratorAnalyzer
{
    /// <summary>
    /// A class that handles interactions with the UI
    /// </summary>
    public partial class GAAnalyzerForm : Form
    {
        private List<Course> courses = new List<Course>();

        // Default preferences
        private Preferences preferences = new Preferences()
        {
            ClassType = Preferences.Day.Undefined,
            WalkingDistance = Preferences.Quantity.Undefined,
            NumDaysInClass = Preferences.Quantity.Undefined,
            TimeBetweenClasses = Preferences.Quantity.Undefined,
            LunchPeriod = 0
        };

        // Default restrictions
        private Restrictions restrictions = new Restrictions()
        {
            EarliestClass = null,
            LatestClass = null,
            WalkDurationInBackToBackClasses = 20
        };

        /// <summary>
        /// Constructs a new window for the GA analyzer
        /// </summary>
        public GAAnalyzerForm()
        {
            InitializeComponent();
            crossoverMethodBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles when the window is loaded and displayed
        /// </summary>
        /// <param name="sender">The window</param>
        /// <param name="e">Arguements passed to this event handler</param>
        private void GAAnalyzerForm_Load(object sender, EventArgs e)
        {
            LoadData();
            UpdateStats();
        }

        /// <summary>
        /// Get data from the database
        /// </summary>
        private void LoadData()
        {
            courses = UoftServices.GetService().GetCourseDetails(new string[] { "MAT137Y1-Y", "COG250Y1-Y", "CSC148H1-F", "CSC165H1-F", "ENV100H1-F" });
        }

        /// <summary>
        /// Update the statistical data in the charts
        /// </summary>
        private void UpdateStats()
        {
            // Get the parameters
            double mutationRate = Convert.ToDouble(mutationRateTxtbox.Text);
            double crossoverRate = Convert.ToDouble(crossoverRateTxtbox.Text);
            int numGenerations = Convert.ToInt32(numGenTxtbox.Text);
            int populationSize = Convert.ToInt32(popSizeTxtbox.Text);
            string crossoverType = crossoverMethodBox.Text;

            // Get average scores per generation for over 30 trials
            int numTrials = 100;
            StatsPerGeneration[][] fullStats = new StatsPerGeneration[numTrials][];

            /*
            Parallel.For(0, numTrials, delegate (int i) 
            {
                // Make generator with params
                GAGenerator generator = new GAGenerator(courses, preferences, restrictions)
                {
                    MutationRate = mutationRate,
                    CrossoverRate = crossoverRate,
                    NumGenerations = numGenerations,
                    PopulationSize = populationSize,
                    CrossoverType = crossoverType
                };

                // Run the generator and get stats
                StatsPerGeneration[] curStats = generator.GenerateTimetablesWithStats();
                fullStats[i] = curStats;
            });
            */

            for (int i = 0; i < numTrials; i++)
            {
                // Make generator with params
                
                GeneticScheduler<YearlyTimetable> generator = new GeneticScheduler<YearlyTimetable>(null, null, preferences, restrictions)
                {
                    MutationRate = mutationRate,
                    CrossoverRate = crossoverRate,
                    NumGenerations = numGenerations,
                    PopulationSize = populationSize
                };                

                // Run the generator and get stats
                StatsPerGeneration[] curStats = generator.GenerateTimetablesWithStats();
                fullStats[i] = curStats;
            }

            // Compute the average over the x trials
            StatsPerGeneration[] avgStats = new StatsPerGeneration[numGenerations];
            for (int i = 0; i < numGenerations; i++)
            {
                StatsPerGeneration sum = fullStats[0][i];
                for (int j = 1; j < numTrials; j++)
                    sum += fullStats[j][i];

                avgStats[i] = sum / numTrials;
            }

            // Set up the data
            scoresChart.Series.Clear();
            scoresChart.ChartAreas[0].AxisY.Minimum = 0;
            scoresChart.Series.Add("Average Scores");
            scoresChart.Series["Average Scores"].ChartType = SeriesChartType.Spline;
            scoresChart.Series.Add("Max Scores");
            scoresChart.Series["Max Scores"].ChartType = SeriesChartType.Spline;

            // Add the average / maxscores to the chart by their type
            for (int i = 0; i < numGenerations; i++)
            {
                scoresChart.Series["Average Scores"].Points.Add(new DataPoint(i, avgStats[i].AverageScores));
                scoresChart.Series["Max Scores"].Points.Add(new DataPoint(i, avgStats[i].MaxScores));
            }

            // Add performance data
            performanceChart.Series.Clear();
            performanceChart.ChartAreas[0].AxisY.Minimum = 0;
            performanceChart.Series.Add("Time ellapsed (ms)");
            performanceChart.Series["Time ellapsed (ms)"].ChartType = SeriesChartType.Spline;

            for (int i = 0; i < numGenerations; i++)
                performanceChart.Series["Time ellapsed (ms)"].Points.Add(new DataPoint(i, avgStats[i].Runtime));

            // Add diversity data
            diversityChart.Series.Clear();
            diversityChart.ChartAreas[0].AxisY.Minimum = 0;
            diversityChart.Series.Add("Diversity (%)");
            diversityChart.Series["Diversity (%)"].ChartType = SeriesChartType.Spline;

            for (int i = 0; i < numGenerations; i++)
                diversityChart.Series["Diversity (%)"].Points.Add(new DataPoint(i, avgStats[i].PopulationDiversity * 100));
        }

        /// <summary>
        /// Handles when the run button is clicked
        /// </summary>
        /// <param name="sender">The button that called this function</param>
        /// <param name="e">The parameters to this event handler</param>
        private void runBttn_Click(object sender, EventArgs e)
        {
            UpdateStats();
        }
    }
}
