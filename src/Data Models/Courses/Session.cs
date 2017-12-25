using System;

namespace UoftTimetableGenerator.DataModels
{
    /// <summary>
    /// A class used to represent a session
    /// </summary>
    public class Session : IComparable
    {
        /// <summary>
        /// Create an empty session
        /// </summary>
        public Session()
        {
        }

        /// <summary>
        /// Constructor used to convert datacontext.session to datamodels.session
        /// </summary>
        /// <param name="oldSession">The raw session data from the database</param>
        /// <param name="sectionAttachedTo">The section that this session belongs to</param>
        internal Session(DataContext.Session oldSession, Section sectionAttachedTo)
        {
            Section = sectionAttachedTo;

            if (oldSession.Building == null)
                FallBuilding = null;
            else
                FallBuilding = new Building(oldSession.Building);

            if (oldSession.Building1 == null)
                WinterBuilding = null;
            else
                WinterBuilding = new Building(oldSession.Building1);

            FallRoomNumber = oldSession.Fall_RoomNumber;
            WinterRoomNumber = oldSession.Winter_RoomNumber;
            StartTimeWithWeekday = oldSession.StartTime.GetValueOrDefault(0);
            EndTimeWithWeekday = oldSession.EndTime.GetValueOrDefault(0);
        }

        /// <summary>
        /// Get / set the section that this session belongs to
        /// </summary>
        public Section Section { get; set; }

        /// <summary>
        /// Get / set the fall building code of this session
        /// </summary>
        public Building FallBuilding { get; set; }

        /// <summary>
        /// Get / set the winter building code of this session
        /// </summary>
        public Building WinterBuilding { get; set; }

        /// <summary>
        /// Get / set the room number in the fall term of this session
        /// </summary>
        public string FallRoomNumber { get; set; }

        /// <summary>
        /// Get / set the room number in the winter term of this session
        /// </summary>
        public string WinterRoomNumber { get; set; }

        /// <summary>
        /// Get the compressed start time of this session 
        /// (this includes the weekday and 24-hr start time)
        /// </summary>
        public double StartTimeWithWeekday { get; set; }

        /// <summary>
        /// Get the compressed end time of this session 
        /// (this includes the weekday and 24-hr end time)
        /// </summary>
        public double EndTimeWithWeekday { get; set; }

        /// <summary>
        /// Get the 24-hr start time of this session
        /// </summary>
        /// <returns></returns>
        public double StartTime
        {
            get { return StartTimeWithWeekday % 100; }
        }

        /// <summary>
        /// Get the 24-hr end time of this session
        /// </summary>
        /// <returns></returns>
        public double EndTime
        {
            get { return EndTimeWithWeekday % 100; }
        }

        /// <summary>
        /// Get the weekday which this session starts on
        /// </summary>
        /// <returns></returns>
        public int StartDay
        {
            get { return ((int)StartTimeWithWeekday) / 100; }
        }

        /// <summary>
        /// Get the weekday which this session ends on
        /// </summary>
        /// <returns></returns>
        public int EndDay
        {
            get { return ((int)EndTimeWithWeekday) / 100; }
        }

        /// <summary>
        /// Returns -1 if this object is before <paramref name="obj"/>.
        /// Returns 1 if this object comes after <paramref name="obj"/>
        /// Returns 0 if this object intersects with <paramref name="obj"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (!(obj is Session))
                return 0;

            else
            {
                Session otherSession = (Session)obj;
                if (EndTimeWithWeekday <= otherSession.StartTimeWithWeekday)
                    return -1;
                else if (otherSession.EndTimeWithWeekday <= StartTimeWithWeekday)
                    return 1;
                else
                    return 0;
            }
        }

        public override string ToString()
        {
            return StartTimeWithWeekday + "-" + EndTimeWithWeekday;
        }
    }
}