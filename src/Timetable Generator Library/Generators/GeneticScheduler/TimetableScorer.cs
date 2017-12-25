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
    public class TimetableScorer
    {
        private Restrictions restrictions;
        private Preferences preferences;

        /// <summary>
        /// Initializes the timetable scorer
        /// </summary>
        /// <param name="restrictions">The restrictions for a timetable</param>
        /// <param name="preferences">The prefernces for a timetable</param>
        public TimetableScorer(Restrictions restrictions, Preferences preferences)
        {
            this.restrictions = restrictions;
            this.preferences = preferences;
        }

        /// <summary>
        /// Get / set the restrictions.
        /// </summary>
        public Restrictions Restrictions { get => restrictions; set => restrictions = value; }

        /// <summary>
        /// Get / set the preferences
        /// </summary>
        public Preferences Preferences { get => preferences; set => preferences = value; }

        /// <summary>
        /// Computes the score of the timetable based on it satisfying the restrictions.
        /// Returns 0 if the timetable is outside of the restrictions; else returns a number > 0.
        /// </summary>
        /// <param name="timetable">A timetable to score</param>
        /// <returns>The score of the timetable</returns>
        private double GetRestrictionsScore(ITimetable timetable)
        {
            // Check if it meets the restrictions
            // Note that each session in the timetable must be within the start/end time restrictions
            if (restrictions.WalkDurationInBackToBackClasses != null)
            {
                foreach (double dur in timetable.WalkDurationInBackToBackClasses)
                    if (dur > restrictions.WalkDurationInBackToBackClasses)
                        return 0;
            }
            return 1000;
        }

        /// <summary>
        /// Computes the score of the timetable based on it satisfying the preferences
        /// </summary>
        /// <param name="timetable">A timetable to score</param>
        /// <returns>The score of the timetable</returns>
        private double GetPreferencesScore(ITimetable timetable)
        {
            double score = 2000;

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

        /// <summary>
        /// Return the overall score of the timetable.
        /// </summary>
        /// <param name="timetable">A timetable to score</param>
        /// <returns>The score of the timetable.</returns>
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
