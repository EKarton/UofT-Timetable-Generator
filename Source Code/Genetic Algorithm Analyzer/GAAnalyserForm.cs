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
        private List<TimetableSession[]> sessions = new List<TimetableSession[]>();
        private List<Timetable> timetables = new List<Timetable>();

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
            List<Course> courses = new List<Course>();
            foreach (string filename in Directory.GetFiles("Sample Input"))
            {
                Console.WriteLine("\t Loaded " + filename);
                courses.Add(JsonConvert.DeserializeObject<Course>(File.ReadAllText(filename)));
            }
            Console.WriteLine("Successful read");

            foreach (Course c in courses)
            {
                if (c.Lectures.Length > 0)
                {
                    TimetableSession[] s = new TimetableSession[c.Lectures.Length];
                    for (int i = 0; i < s.Length; i++)
                        s[i] = new TimetableSession(c.Lectures[i], c);
                    sessions.Add(s);
                }
                if (c.Tutorials.Length > 0)
                {
                    TimetableSession[] s = new TimetableSession[c.Tutorials.Length];
                    for (int i = 0; i < s.Length; i++)
                        s[i] = new TimetableSession(c.Tutorials[i], c);
                    sessions.Add(s);
                }
                if (c.Practicals.Length > 0)
                {
                    TimetableSession[] s = new TimetableSession[c.Practicals.Length];
                    for (int i = 0; i < s.Length; i++)
                        s[i] = new TimetableSession(c.Practicals[i], c);
                    sessions.Add(s);
                }
            }
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
            int numTrials = 100;
            int[] avgScores = new int[numGenerations];
            int[] maxScores = new int[numGenerations];
            for (int i = 0; i < numTrials; i++)
            {
                // Make generator with params
                GAGenerator generator = new GAGenerator(sessions)
                {
                    MutationRate = mutationRate,
                    CrossoverRate = crossoverRate,
                    NumGenerations = numGenerations,
                    PopulationSize = populationSize,
                    CrossoverType = crossoverType
                };

                // Run the generator and get scores
                int[] curAvgScores = generator.GetAvgScoresPerGeneration();
                int[] curMaxScores = generator.GetMaxScoresPerGeneration();
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
            chart.Series.Add("Average Scores");
            chart.Series["Average Scores"].ChartType = SeriesChartType.Line;
            chart.Series.Add("Max Scores");
            chart.Series["Max Scores"].ChartType = SeriesChartType.Line;

            // Add the average / maxscores to the chart by their type
            for (int i = 0; i < numGenerations; i++)
            {
                int generation = i;
                int avgScore = avgScores[i];
                int maxScore = maxScores[i];
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
