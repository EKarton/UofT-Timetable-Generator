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
        private List<Section[]> requiredSections = new List<Section[]>();
        private List<char> terms = new List<char>();
        private Preferences preferences;
        private Restrictions restrictions;
        private int numTimetablesToGenerate = 20;

        private List<int[]> timetables = new List<int[]>();

        public GreedyScheduler(List<Course> courses, Preferences preferences, Restrictions restrictions)
        {
            this.preferences = preferences;
            this.restrictions = restrictions;

            // Populating requiredSections[] and its associated terms[]
            foreach (Course course in courses)
            {
                char term = course.Term[0];
                foreach (Activity activity in course.Activities)
                {
                    requiredSections.Add(activity.Sections.ToArray());
                    terms.Add(term);
                }
            }
        }

        public int NumTimetablesToGenerate
        {
            get { return numTimetablesToGenerate; }
            set { numTimetablesToGenerate = value; }
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
                    Section section = requiredSections[i][curTable[i]];
                    char term = terms[i];
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
                    Section[] potentialSections = requiredSections[i];

                    for (int j = 0; j < potentialSections.Length; j++)
                    {
                        // Check if the section meet the restrictions
                        bool areRestrictionsMet = true;
                        for (int k = 0; k < potentialSections[j].Sessions.Count; k++) {
                            if (DoesItSatisfyRestrictions(potentialSections[j].Sessions[k]) == false)
                            {
                                areRestrictionsMet = false;
                                break;
                            }
                        }

                        if (timetable.DoesSectionFit(potentialSections[j]) && areRestrictionsMet)
                            nextSessions.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            return nextSessions;
        }

        private bool DoesItSatisfyRestrictions(Session session)
        {
            if (restrictions.EarliestClass != null)
            {
                if (restrictions.EarliestClass.GetValueOrDefault(0) > session.GetStartTime_Time())
                    return false;
            }
            if (restrictions.LatestClass != null)
            {
                if (restrictions.LatestClass.GetValueOrDefault(0) < session.GetEndTime_Time())
                    return false;
            }
            return true;
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
            int[] emptyTable = new int[requiredSections.Count];
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
                    Section section = requiredSections[j][table[j]];
                    char term = terms[j];
                    formattedTimetable.AddSection(section);
                }
                formattedTimetables.Add(formattedTimetable);
            }

            return formattedTimetables;
        }
    }
}
