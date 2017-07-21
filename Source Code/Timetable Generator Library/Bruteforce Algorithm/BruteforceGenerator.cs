using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    public class BruteforceGenerator : ITimetableGenerator
    {
        static List<TimetableSession[]> sessions = new List<TimetableSession[]>();
        static Timetable timetable = new Timetable();
        static List<Timetable> timetables = new List<Timetable>();

        static int num = 0;
        static void Generate(int sessionIndex)
        {
            num++;

            // Basecase: There are no more sessions to add to the timetable
            if (sessionIndex >= sessions.Count)
            {
                if (timetable.NumberOfSessions == sessions.Count)
                    timetables.Add(timetable.MakeCopyOfTimetable());
                return;
            }

            // Recurse
            foreach (TimetableSession session in sessions[sessionIndex])
            {
                if (timetable.DoesSessionFit(session))
                {
                    timetable.AddSession(session);
                    Generate(sessionIndex + 1);
                    timetable.RemoveSession(session);
                }
            }
        }

        static bool AreTimetablesDuplicated()
        {
            for (int i = 0; i < timetables.Count; i++)
                for (int j = i + 1; j < timetables.Count; j++)
                    if (Timetable.AreTimetablesEqual(timetables[i], timetables[j]))
                        return true;
            return false;
        }

        static void SortMeetingSessions()
        {
            sessions.Sort(delegate (TimetableSession[] sessions1, TimetableSession[] sessions2)
            {
                if (sessions1.Length > sessions2.Length)
                    return 1;
                else if (sessions1.Length > sessions2.Length)
                    return -1;
                else
                    return 0;
            });
        }

        static void SortTimetables()
        {
            timetables.Sort(delegate (Timetable table1, Timetable table2)
            {
                if (table1.TotalSpacesBetweenClasses > table2.TotalSpacesBetweenClasses)
                    return 1;
                else if (table1.TotalSpacesBetweenClasses < table2.TotalSpacesBetweenClasses)
                    return -1;
                else
                    return 0;
            });
        }

        static void FilterTimetables(int earliestTime, int latestTime)
        {
            List<Timetable> filteredTimetables = new List<Timetable>();
            foreach (Timetable table in timetables)
            {
                if (table.EarliestClasstime >= earliestTime && table.LatestClasstime <= latestTime)
                    filteredTimetables.Add(table);
            }

            timetables = filteredTimetables;
        }

        public List<Timetable> GetTimetables(string[] filters)
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

            SortMeetingSessions();
            Console.WriteLine(sessions.Count);

            Generate(0);

            Console.WriteLine("Successful generation of " + timetables.Count + " tables " + num);
            Console.ReadKey();
            Console.WriteLine("Are there any duplicates? " + AreTimetablesDuplicated());
            Console.ReadKey();

            SortTimetables();
            FilterTimetables(9, 16);
            Console.WriteLine("Polished Timetables: " + timetables.Count);
            foreach (Timetable table in timetables)
            {
                table.Show();
                Console.ReadKey();
            }
            return timetables;
        }
    }
}
