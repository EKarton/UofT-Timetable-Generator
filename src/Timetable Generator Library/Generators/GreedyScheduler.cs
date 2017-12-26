using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    /// <summary>
    /// A class that generates timetables using the Greedy algorithm
    /// </summary>
    public class GreedyScheduler<T> : IScheduler<T> where T: ITimetable, new()
    {
        // Note that the first dimension are the activities; and the second dimension are the sections of an activity.
        private List<Section[]> requiredActivities = new List<Section[]>();
        private Preferences preferences;
        private Restrictions restrictions;
        private int numTimetablesToGenerate = 200;

        private List<int[]> timetables = new List<int[]>();

        public GreedyScheduler(List<Activity> activities, Preferences preferences, Restrictions restrictions)
        {
            this.preferences = preferences;
            this.restrictions = restrictions;

            // Populating requiredSections[] and its associated terms[]
            // It only adds distinct sections that are within the start/end time restrictions 
            foreach (Activity activity in activities)
            {
                List<Section> allowedSections = new List<Section>();
                foreach (Section section in activity.Sections)
                {
                    if (IsSectionWithinStartEndTimes(section))
                        allowedSections.Add(section);
                }

                List<Section> distinctSections = GetDistinctSections(allowedSections.ToArray());
                requiredActivities.Add(distinctSections.ToArray());
            }

            // Sort the activities in requiredSections[] in ascending order based on its number of sections
            requiredActivities.Sort((a, b) => (a.Length.CompareTo(b.Length)));
        }

        public int NumTimetablesToGenerate
        {
            get { return numTimetablesToGenerate; }
            set { numTimetablesToGenerate = value; }
        }

        private bool IsSectionWithinStartEndTimes(Section section)
        {
            foreach (Session session in section.Sessions)
            {
                if (session.StartTime< restrictions.EarliestClass)
                    return false;
                else if (session.EndTime> restrictions.LatestClass)
                    return false;
            }
            return true;
        }

        private int[] MakeCopyOfTimetable(int[] validTable)
        {
            int[] copy = new int[validTable.Length];
            for (int i = 0; i < validTable.Length; i++)
                copy[i] = validTable[i];
            return copy;
        }

        private bool IsTimetablesComplete(int[] validTable)
        {
            foreach (int i in validTable)
                if (i == -1)
                    return false;
            return true;
        }

        private List<Tuple<int, int>> GetAvailableSessions(int[] curTable)
        {
            // Make the timetable equivalent
            YearlyTimetable timetable = new YearlyTimetable();
            for (int i = 0; i < curTable.Length; i++)
            {
                if (curTable[i] != -1)
                {
                    Section section = requiredActivities[i][curTable[i]];
                    timetable.AddSection(section);
                }
            }

            // Each tuple represent a potential next session to be added to the table
            // Tuple.item1 refers to the required activity to add
            // Tuple.item2 refers to the specific session in the activity to add
            List<Tuple<int, int>> nextSessions = new List<Tuple<int, int>>(); 

            for (int i = 0; i < curTable.Length; i++)
            {
                if (curTable[i] == -1)
                {
                    Section[] potentialSections = requiredActivities[i];

                    List<Section> distinctSections = GetDistinctSections(potentialSections);

                    for (int j = 0; j < potentialSections.Length; j++)
                    {
                        if (timetable.DoesSectionFit(potentialSections[j]))
                            nextSessions.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            return nextSessions;
        }

        private List<Section> GetDistinctSections(Section[] potentialSections)
        {
            List<Section> distinctSections = new List<Section>();
            HashSet<String> foundSections = new HashSet<string>();

            foreach (Section section in potentialSections)
            {
                string hashcode = "";
                foreach (Session session in section.Sessions)
                    hashcode += session.StartTimeWithWeekday + "-" + session.EndTimeWithWeekday + ",";

                if (!foundSections.Contains(hashcode))
                {
                    distinctSections.Add(section);
                    foundSections.Add(hashcode);
                }
            }
            return distinctSections;
        }

        private void GenerateTimetables(int[] curTable)
        {
            // Basecase: if there are enough timetables to generate
            if (timetables.Count > numTimetablesToGenerate)
                return;

            // Basecase: if the current timetable is complete
            if (IsTimetablesComplete(curTable))
            {
                timetables.Add(MakeCopyOfTimetable(curTable));
                return;
            }

            // Recurse
            // Note: In each (x, y) tuple, x points to a required activity, and y points to a section in that activity 
            List<Tuple<int, int>> nextSessions = GetAvailableSessions(curTable);
            foreach (Tuple<int, int> nextSession in nextSessions)
            {
                int sessionsIndex = nextSession.Item1;
                int sessionIndex = nextSession.Item2;
                curTable[sessionsIndex] = sessionIndex;
                GenerateTimetables(curTable);                
                curTable[sessionsIndex] = -1;
            }
        }

        public List<T> GetTimetables()
        {
            // Create an empty timetable
            int[] emptyTable = new int[requiredActivities.Count];
            for (int i = 0; i < emptyTable.Length; i++)
                emptyTable[i] = -1;

            // Generate timetables
            GenerateTimetables(emptyTable);

            // Getting timetables that has every single required activity
            List<T> formattedTimetables = new List<T>();
            for (int i = 0; i < timetables.Count; i++)
            {
                int[] table = timetables[i];
                T formattedTimetable = new T();
                for (int j = 0; j < table.Length; j++)
                {
                    Section section = requiredActivities[j][table[j]];
                    formattedTimetable.AddSection(section);
                }
                formattedTimetables.Add(formattedTimetable);
            }

            return formattedTimetables;
        }
    }
}
