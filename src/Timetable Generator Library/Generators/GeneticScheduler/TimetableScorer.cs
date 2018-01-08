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
        /// Get the amount of hours of a session spent in a certain time interval.
        /// For instance, if we have a session that spanned from [12:00-13:00],
        /// then GetHoursInTimeSpan(session, 12, 17) will return 1 because 1 hour 
        /// of that session is spent between 12:00-17:00.
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="startTime">The start time of the time span</param>
        /// <param name="endTime">The end time of the time span</param>
        /// <returns>The amount of hours spent in the session between the time span.</returns>
        private double GetHoursInTimespan(Session session, double startTime, double endTime)
        {
            // For instance, if trying to get the amount of hrs in the afternoon [from 12pm-17:00]
            // If it is between [12:00-17:00]
            if (session.StartTime >= startTime && session.EndTime <= endTime)
                return session.EndTime - session.StartTime;

            // If start time is less than 12 but end time between [12:00-17:00]
            else if (session.StartTime <= startTime && startTime <= session.EndTime && session.EndTime <= endTime)
                return session.EndTime - startTime;

            // If the start time is between [12:00-17:00] but end time is > 17
            else if (startTime <= session.StartTime && session.StartTime <= endTime && session.EndTime >= endTime)
                return endTime - session.StartTime;

            // If the session takes up more than the afternoon
            else if (session.StartTime <= startTime && session.EndTime >= endTime)
                return endTime - startTime;
            else
                return 0;
        }

        /// <summary>
        /// Returns the average distribution of the day spent in sessions in a timetable
        /// It returns an array of length of 4, where:
        /// - distribution[0] returns the distribution of the sessions in the morning [7:00-12:00]
        /// - distribution[1] returns the distribution of the sessions in the afternoon [12:00-17:00]
        /// - distribution[2] returns the distribution of the sessions in the evening [17:00-21:00]
        /// - distribution[3] returns the distribution of the sessions in the night [21:00-24:00]
        /// </summary>
        /// <param name="timetable">A timetable</param>
        /// <returns>The distribution of the day spent in sessions</returns>
        private double[] GetDayDistributions(IUniversityTimetable timetable)
        {
            double[] distribution = new double[4];
            List<Section> sections = timetable.GetSections();

            foreach (Section section in sections)
            {
                foreach (Session session in section.Sessions)
                {
                    distribution[0] += GetHoursInTimespan(session, 7, 12);
                    distribution[1] += GetHoursInTimespan(session, 12, 17);
                    distribution[2] += GetHoursInTimespan(session, 17, 21);
                    distribution[3] += GetHoursInTimespan(session, 21, 24);
                }
            }

            // Compute the average
            double numHrs = distribution[0] + distribution[1] + distribution[2] + distribution[3];
            if (numHrs == 0)
                return new double[4];
            else
            {
                distribution[0] /= numHrs;
                distribution[1] /= numHrs;
                distribution[2] /= numHrs;
                distribution[3] /= numHrs;
                return distribution;
            }
        }

        /// <summary>
        /// Computes the score of the timetable based on it satisfying the restrictions.
        /// Returns 0 if the timetable is outside of the restrictions; else returns a number > 0.
        /// </summary>
        /// <param name="timetable">A timetable to score</param>
        /// <returns>The score of the timetable</returns>
        private double GetRestrictionsScore(IUniversityTimetable timetable)
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
        private double GetPreferencesScore(IUniversityTimetable timetable)
        {
            double score = 2000;

            // Get scores by how well the sessions are distributed throughout the day
            double[] timeDistributions = GetDayDistributions(timetable);
            switch (Preferences.ClassType)
            {
                case Preferences.Day.Undefined:
                    break;
                case Preferences.Day.Morning:
                    if (timeDistributions[0] > 0.5)
                        score += 1000;
                    else
                        score -= 1000;
                    break;
                case Preferences.Day.Afternoon:
                    if (timeDistributions[1] > 0.5)
                        score += 1000;
                    else
                        score -= 1000;
                    break;
                case Preferences.Day.Evening:
                    if (timeDistributions[2] > 0.5)
                        score += 1000;
                    else
                        score -= 1000;
                    break;
                case Preferences.Day.Night:
                    if (timeDistributions[3] > 0.5)
                        score += 1000;
                    else
                        score -= 1000;
                    break;
                default:
                    throw new Exception("No class type is handled before!");
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
        public double GetFitnessScore(IUniversityTimetable timetable)
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
