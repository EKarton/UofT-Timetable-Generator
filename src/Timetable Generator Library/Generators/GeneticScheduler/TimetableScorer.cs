using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    /// <summary>
    /// A class used to score the timetables.
    /// Timetables with a high score are typically the better solutions while timetables
    /// with a low score have worser solutions.
    /// </summary>
    class TimetableScorer
    {
        private Restrictions restrictions;
        private Preferences preferences;

        public TimetableScorer(Restrictions restrictions, Preferences preferences)
        {
            this.restrictions = restrictions;
            this.preferences = preferences;
        }

        public Restrictions Restrictions { get => restrictions; set => restrictions = value; }
        public Preferences Preferences { get => preferences; set => preferences = value; }

        private double GetRestrictionsScore(ITimetable timetable)
        {
            // Check if it meets the restrictions
            if (restrictions.EarliestClass != null && timetable.EarliestClassTime < restrictions.EarliestClass)
                return 0;
            if (restrictions.LatestClass != null && timetable.LatestClassTime > restrictions.LatestClass)
                return 0;
            if (restrictions.WalkDurationInBackToBackClasses != null)
            {
                foreach (double dur in timetable.WalkDurationInBackToBackClasses)
                    if (dur > restrictions.WalkDurationInBackToBackClasses)
                        return 0;
            }
            return 1000;
        }

        private double GetPreferencesScore(ITimetable timetable)
        {
            double score = 1000;

            // Get scores associated by their preferences
            switch (preferences.ClassType)
            {
                case Preferences.Day.Undefined:
                    break;
                case Preferences.Day.Morning: // (12am - 12pm)
                    if (0 < timetable.EarliestClassTime && timetable.LatestClassTime < 12)
                        score += 100;
                    break;
                case Preferences.Day.Afternoon: // (12pm - 5pm)
                    if (12 <= timetable.EarliestClassTime && timetable.LatestClassTime < 17)
                        score += 100;
                    break;
                case Preferences.Day.Evening: // (5pm - 8pm)
                    if (17 <= timetable.EarliestClassTime && timetable.LatestClassTime < 20)
                        score += 100;
                    break;
                case Preferences.Day.Night: // (9pm - 12pm)
                    if (20 <= timetable.EarliestClassTime && timetable.LatestClassTime <= 24)
                        score += 100;
                    break;
                default:
                    throw new Exception("Class type not handled before!");
            }
            switch (preferences.TimeBetweenClasses)
            {
                case Preferences.Quantity.Undefined:
                    break;
                case Preferences.Quantity.Minimum:
                    score -= timetable.TotalTimeBetweenClasses;
                    break;
                case Preferences.Quantity.Maximum:
                    score += timetable.TotalTimeBetweenClasses;
                    break;
                default:
                    throw new Exception("Time between class is not handled before!");
            }
            switch (preferences.NumDaysInClass)
            {
                case Preferences.Quantity.Undefined:
                    break;
                case Preferences.Quantity.Minimum:
                    score -= timetable.NumDaysInClass * 100;
                    break;
                case Preferences.Quantity.Maximum:
                    score += timetable.NumDaysInClass * 100;
                    break;
            }
            return score;
        }


        public double GetFitnessScore(ITimetable timetable)
        {
            // If the table is an invalid table, then its score is 0
            if (timetable == null)
                return 0;

            if (GetRestrictionsScore(timetable) > 0)
            {
                return GetPreferencesScore(timetable);
            }
            return 0;
        }
    }
}
