using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;
using UoftTimetableGenerator.Generator;

namespace Timetable_Generator_Library
{
    class AlgorithmRunner
    {
        static List<TimetableSession[]> sessions = new List<TimetableSession[]>();
        static List<Timetable> timetables = new List<Timetable>();

        static void LoadData()
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

        static void Main(string[] args)
        {
            // Make the console window bigger
            Console.SetBufferSize(1000, 800);

            LoadData();

            const int NUM_TRIALS = 100;
            int[] avgScores = new int[100];
            for (int i = 0; i < NUM_TRIALS; i++)
            {
                GAGenerator gen = new GAGenerator(sessions);
                int[] avgFits = gen.GetAvgScoresPerGeneration();

                for (int j = 0; j < 100; j++)
                    avgScores[j] += avgFits[j];
            }
            for (int i = 0; i < avgScores.Length; i++)
                avgScores[i] /= NUM_TRIALS;

            for (int i = 0; i < avgScores.Length; i++)
                Console.WriteLine(avgScores[i]);
            Console.ReadKey();
        }
    }
}
