using System;
using System.Collections.Generic;

namespace UoftTimetableGenerator.DataModels
{
    public class Timetable
    {
        private TimetableSession[,] weeklyPlanner = new TimetableSession[24, 7];
        private int numSessions = 0;

        public void Show()
        {
            Console.WriteLine("\n\n\n");
            for (int i = 0; i < 24; i++)
            {                
                for (int j = 0; j < 7; j++)
                {
                    if (weeklyPlanner[i, j] != null)
                        Console.Write(weeklyPlanner[i, j].Session.ID + "\t");
                    else
                        Console.Write("* \t");
                }
                Console.WriteLine("\n================================================");
            }
            Console.WriteLine("Total number of spaces wasted in between classes: " + TotalSpacesBetweenClasses);
            Console.WriteLine("Earliest class time: " + EarliestClasstime + ":00");
            Console.WriteLine("Latest class time: " + LatestClasstime + ":00");
        }

        public bool DoesSessionFit(TimetableSession session)
        {
            foreach (Time time in session.Session.Times)
            {
                int dayIndex = GetIndexFromWeekday(time.Day);
                int curTime = Convert.ToInt32(time.StartTime);
                while (curTime < 24 && curTime <= time.EndTime)
                {
                    if (weeklyPlanner[curTime, dayIndex] != null)
                        return false;
                    curTime++;
                }
            }
            return true;
        }

        public void AddSession(TimetableSession session)
        {
            foreach (Time time in session.Session.Times)
            {
                int dayIndex = GetIndexFromWeekday(time.Day);
                int curTime = Convert.ToInt32(time.StartTime);

                while (curTime < 24 && curTime < time.EndTime)
                {
                    weeklyPlanner[curTime, dayIndex] = session;
                    curTime++;
                }
            }
            numSessions++;
        }

        public void RemoveSession(TimetableSession session)
        {
            foreach (Time time in session.Session.Times)
            {
                int dayIndex = GetIndexFromWeekday(time.Day);
                int curTime = Convert.ToInt32(time.StartTime);

                while (curTime < 24 && curTime < time.EndTime)
                {
                    weeklyPlanner[curTime, dayIndex] = null;
                    curTime++;
                }
            }
            numSessions--;
        }

        public int NumberOfSessions
        {
            get { return numSessions; }
        }

        public Timetable MakeCopyOfTimetable()
        {
            Timetable newTimetable = new Timetable();
            for (int i = 0; i < 24; i++)
                for (int j = 0; j < 7; j++)
                    newTimetable.weeklyPlanner[i, j] = weeklyPlanner[i, j];
            return newTimetable;
        }

        public int TotalSpacesBetweenClasses
        {
            get
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

                    for (int k = sessionsIndex[0]; k < sessionsIndex[sessionsIndex.Count -1]; k++)
                    {
                        if (weeklyPlanner[k, i] == null)
                            totalEmptySpaces += 1;
                    }
                }
                return totalEmptySpaces;
            }
        }

        public int EarliestClasstime
        {
            get
            {
                int minClassStarttime = int.MaxValue;
                for (int day = 0; day < 7; day++)
                {
                    int curHr = 0;
                    while (curHr < 24 && weeklyPlanner[curHr, day] == null)
                        curHr++;

                    if (curHr < 24)
                        minClassStarttime = Math.Min(minClassStarttime, curHr);
                }
                return minClassStarttime;
            }
        }

        public int LatestClasstime
        {
            get
            {
                int maxClassEndTime = 0;
                for (int day = 0; day < 7; day++)
                {
                    int curHr = 23;
                    while (curHr >= 0 && weeklyPlanner[curHr, day] == null)
                        curHr--;

                    if (curHr >= 0)
                        maxClassEndTime = Math.Max(maxClassEndTime, curHr);
                }
                return maxClassEndTime + 1;
            }
        }

        public int AverageWalkingDistance
        {
            get { return -1; }
        }

        public static int GetIndexFromWeekday(string weekday)
        {
            switch(weekday)
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

        public static bool AreTimetablesEqual(Timetable table1, Timetable table2)
        {
            // Compare the contents
            for (int i = 0; i < 24; i++)
                for (int j = 0; j < 7; j++)
                    if (table1.weeklyPlanner[i, j] != table2.weeklyPlanner[i, j])
                        return false;
            return true;
        }
    }
}