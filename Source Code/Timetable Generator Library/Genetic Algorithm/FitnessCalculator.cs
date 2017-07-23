using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    public partial class GAGenerator : ITimetableGenerator
    {
        internal class FitnessCalculator
        {
            private static List<TimetableSession[]> requiredSessions = null;

            public static int GetFitnessValue(int[] table, List<TimetableSession[]> reqSessions)
            {
                requiredSessions = reqSessions;
                int score = 0;
                TimetableSession[,] createdTable = CreateWeeklyPlanner(table);

                if (IsTableValid(createdTable))
                {
                    score += 10000;
                    score += (100 - GetEmptySpacesBetweenClasses(createdTable));
                }
                return score;
            }

            private static TimetableSession[,] CreateWeeklyPlanner(int[] table)
            {
                TimetableSession[,] weeklyPlanner = new TimetableSession[24, 7];

                for (int i = 0; i < table.Length; i++)
                {
                    int sessionIndex = table[i];
                    TimetableSession session = requiredSessions[i][sessionIndex];

                    // Add session to weeklyPlanner[][]
                    foreach (Time time in session.Session.Times)
                    {
                        int dayIndex = GetIndexFromWeekday(time.Day);
                        int curTime = Convert.ToInt32(time.StartTime);

                        while (curTime < 24 && curTime < time.EndTime)
                        {
                            if (weeklyPlanner[curTime, dayIndex] != null)
                                return null;

                            weeklyPlanner[curTime, dayIndex] = session;
                            curTime++;
                        }
                    }
                }

                return weeklyPlanner;
            }

            private static bool IsTableValid(TimetableSession[,] table)
            {
                if (table == null)
                    return false;
                return true;
            }

            private static int GetEmptySpacesBetweenClasses(TimetableSession[,] weeklyPlanner)
            {
                int totalEmptySpaces = 0;
                for (int i = 0; i < 7; i++)
                {
                    List<int> sessionsIndex = new List<int>();
                    for (int j = 0; j < 24; j++)
                        if (weeklyPlanner[j, i] != null)
                            sessionsIndex.Add(j);

                    // Compute spaces between classes per day
                    if (sessionsIndex.Count == 0)
                        continue;

                    for (int k = sessionsIndex[0]; k < sessionsIndex[sessionsIndex.Count - 1]; k++)
                    {
                        if (weeklyPlanner[k, i] == null)
                            totalEmptySpaces += 1;
                    }
                }
                return totalEmptySpaces;
            }

            private static int GetIndexFromWeekday(string weekday)
            {
                switch (weekday)
                {
                    case "Sunday":
                        return 0;
                    case "Monday":
                        return 1;
                    case "Tuesday":
                        return 2;
                    case "Wednesday":
                        return 3;
                    case "Thursday":
                        return 4;
                    case "Friday":
                        return 5;
                    case "Saturday":
                        return 6;
                    default:
                        throw new Exception("This day cannot be determined!");
                }
            }
        }
    }
}
