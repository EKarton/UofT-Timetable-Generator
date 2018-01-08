﻿using System;
using System.Collections.Generic;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    /// <summary>
    /// A class used to hold the sections present in one term
    /// </summary>
    public class SeasonalTimetable : WeeklyTimetable, IUniversityTimetable
    {
        // A sorted tree used to hold the timetable (in order)
        private IOrderedSet<Session> collection = new RedBlackTree<Session>();

        // A list of all sections in this object 
        private List<Section> sections = new List<Section>();

        /// <summary>
        /// Get the average walk distance in between classes
        /// </summary>
        public double AverageWalkingDistance
        {
            get
            {
                double totalWalkingDistance = 0;
                double numberOfDistancesCalculated = 0;
                List<Session> sessions = collection.GetContents();
                for (int i = 0; i < sessions.Count - 1; i++)
                {
                    Session session1 = sessions[i];
                    Session session2 = sessions[2];
                    if (session1.EndDay== session2.StartDay)
                    {
                        Building building1 = session1.FallBuilding;
                        Building building2 = session2.FallBuilding;
                        if (building1 != null && building2 != null)
                        {
                            BuildingDistance distances = UoftServices.GetService().GetDistancesBetweenBuildings(building1, building2);
                            if (distances != null && distances.WalkingDistance != null)
                            {
                                totalWalkingDistance += distances.WalkingDistance.GetValueOrDefault(0);
                                numberOfDistancesCalculated++;
                            }
                        }

                        building1 = sessions[i].WinterBuilding;
                        building2 = sessions[i + 1].WinterBuilding;
                        if (building1 != null & building2 != null)
                        {
                            BuildingDistance distances = UoftServices.GetService().GetDistancesBetweenBuildings(building1, building2);
                            if (distances != null && distances.WalkingDistance != null)
                            {
                                totalWalkingDistance += distances.WalkingDistance.GetValueOrDefault(0);
                                numberOfDistancesCalculated++;
                            }
                        }
                    }
                }
                return totalWalkingDistance / numberOfDistancesCalculated;
            }
        }

        /// <summary>
        /// Get / set the total walk duration in between classes
        /// </summary>
        public double TotalWalkDuration
        {
            get { return 0; }
        }

        /// <summary>
        /// Get / set the walk duration in each back to back classes
        /// </summary>
        public List<double> WalkDurationInBackToBackClasses
        {
            get
            {
                List<double> walkDurations = new List<double>();
                List<Session> sessions = collection.GetContents();
                for (int i = 0; i < sessions.Count - 1; i++)
                {
                    if (sessions[i].EndDay== sessions[i + 1].StartDay)
                    {
                        if (sessions[i].EndTime== sessions[i + 1].StartTime)
                        {
                            Building building1 = sessions[i].FallBuilding;
                            Building building2 = sessions[i + 1].FallBuilding;
                            if (building1 != null && building2 != null)
                            {
                                BuildingDistance distances = UoftServices.GetService().GetDistancesBetweenBuildings(building1, building2);
                                if (distances != null && distances.WalkingDistance != null)
                                {
                                    double walkingDuration = distances.WalkingDistance.GetValueOrDefault(0);
                                    walkDurations.Add(walkingDuration);
                                }
                            }

                            building1 = sessions[i].WinterBuilding;
                            building2 = sessions[i + 1].WinterBuilding;
                            if (building1 != null && building2 != null)
                            {
                                BuildingDistance distances = UoftServices.GetService().GetDistancesBetweenBuildings(building1, building2);
                                if (distances != null && distances.WalkingDistance != null)
                                {
                                    double walkingDuration = distances.WalkingDistance.GetValueOrDefault(0);
                                    walkDurations.Add(walkingDuration);
                                }
                            }
                        }
                    }
                }
                return walkDurations;
            }
        }

        /// <summary>
        /// Get the time (24-hr time) of the latest class in this timetable
        /// </summary>
        public double LatestClassTime
        {
            get
            {
                List<Session> items = collection.GetContents();
                double maxClassTime = 0;
                foreach (Session session in items)
                {
                    double endTime = session.EndTime;
                    if (endTime > maxClassTime)
                        maxClassTime = endTime;
                }
                return maxClassTime;
            }
        }

        /// <summary>
        /// Get the start time (24-hr time) of the earliest class in this timetable
        /// </summary>
        public double EarliestClassTime
        {
            get
            {
                List<Session> items = collection.GetContents();
                double minClassTime = 1200;
                foreach (Session session in items)
                {
                    double startTime = session.StartTime;
                    if (startTime < minClassTime)
                        minClassTime = startTime;
                }
                return minClassTime;
            }
        }

        /// <summary>
        /// Get the total amount of time spent in between classes
        /// </summary>
        public double TotalTimeBetweenClasses
        {
            get
            {
                double totalTimeBetweenClasses = 0;
                List<Session> items = collection.GetContents();
                for (int i = 0; i < items.Count - 1; i++)
                {
                    double timeBetweenClass = items[i + 1].EndTimeWithWeekday - items[i].StartTimeWithWeekday;
                    totalTimeBetweenClasses += timeBetweenClass;
                }
                return totalTimeBetweenClasses;
            }
        }

        /// <summary>
        /// Get the total amount of time spent in class
        /// </summary>
        public double TimeInClass
        {
            get
            {
                double timeInClass = 0;
                List<Session> items = collection.GetContents();
                foreach (Session s in items)
                    timeInClass += s.EndTimeWithWeekday - s.StartTimeWithWeekday;
                return timeInClass;
            }
        }

        /// <summary>
        /// Get the total number of days in class
        /// </summary>
        public int NumDaysInClass
        {
            get { return DaysInClass.Count; }
        }

        /// <summary>
        /// Get the days in class (all of the days are represented by indexes)
        /// </summary>
        public HashSet<int> DaysInClass
        {
            get
            {
                HashSet<int> daysInClass = new HashSet<int>();
                List<Session> sessions = collection.GetContents();
                foreach (Session session in sessions)
                {
                    if (!daysInClass.Contains(session.StartDay))
                        daysInClass.Add(session.StartDay);
                    if (daysInClass.Contains(session.EndDay))
                        daysInClass.Add(session.StartDay);
                }
                return daysInClass;
            }
        }

        /// <summary>
        /// Get a list of times between classes
        /// </summary>
        public List<double> TimeBetweenClasses
        {
            get { return new List<double>(); }
        }
    }
}