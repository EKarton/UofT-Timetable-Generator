namespace UoftTimetableGenerator.DataModels
{
    /// <summary>
    /// A class used to represent a session
    /// </summary>
    public class Session
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
            StartTime = oldSession.StartTime.GetValueOrDefault(0);
            EndTime = oldSession.EndTime.GetValueOrDefault(0);
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
        public double StartTime { get; set; }

        /// <summary>
        /// Get the compressed end time of this session 
        /// (this includes the weekday and 24-hr end time)
        /// </summary>
        public double EndTime { get; set; }

        /// <summary>
        /// Get the 24-hr start time of this session
        /// </summary>
        /// <returns></returns>
        public double GetStartTime_Time()
        {
            return StartTime % 100;
        }

        /// <summary>
        /// Get the 24-hr end time of this session
        /// </summary>
        /// <returns></returns>
        public double GetEndTime_Time()
        {
            return EndTime % 100;
        }

        /// <summary>
        /// Get the weekday which this session starts on
        /// </summary>
        /// <returns></returns>
        public int GetStartTime_WeekdayIndex()
        {
            return ((int) StartTime) / 100;
        }

        /// <summary>
        /// Get the weekday which this session ends on
        /// </summary>
        /// <returns></returns>
        public int GetEndTime_WeekdayIndex()
        {
            return ((int)EndTime) / 100;
        }
    }
}