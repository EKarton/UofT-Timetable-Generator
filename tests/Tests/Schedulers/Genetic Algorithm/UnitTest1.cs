using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UoftTimetableGenerator.DataModels;
using UoftTimetableGenerator.Generator;

namespace Tests.Schedulers.Genetic_Algorithm
{
    [TestClass]
    public class UnitTest1
    {
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

        private double[] GetTimeDistribution(IUniversityTimetable timetable)
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

        private YearlyTimetable timetable = new YearlyTimetable();

        [TestInitialize]
        public void Initialize()
        {
            Section mat235Lecture = new Section()
            {
                SectionCode = "LEC 0701",
                Sessions = new List<Session>()
                {
                    new Session()
                    {
                        StartTimeWithWeekday = 109.00,
                        EndTimeWithWeekday = 110.00
                    },
                    new Session()
                    {
                        StartTimeWithWeekday = 309,
                        EndTimeWithWeekday = 310
                    },
                    new Session()
                    {
                        StartTimeWithWeekday = 509,
                        EndTimeWithWeekday = 510
                    }
                }
            };

            Section mat235Tutorial = new Section()
            {
                SectionCode = "TUT 0302",
                Sessions = new List<Session>()
                {
                    new Session()
                    {
                        StartTimeWithWeekday = 215.00,
                        EndTimeWithWeekday = 216.00
                    }
                }
            };

            Section csc209Lecture = new Section()
            {
                SectionCode = "LEC 0101",
                Sessions = new List<Session>()
                {
                    new Session()
                    {
                        StartTimeWithWeekday = 110.00,
                        EndTimeWithWeekday = 111.00
                    },
                    new Session()
                    {
                        StartTimeWithWeekday = 310.00,
                        EndTimeWithWeekday = 311.00
                    }
                }
            };

            Section csc209Tutorial = new Section()
            {
                SectionCode = "TUT 0101",
                Sessions = new List<Session>()
                {
                    new Session()
                    {
                        StartTimeWithWeekday = 513.00,
                        EndTimeWithWeekday = 514.00
                    }
                }
            };

            Section csc263Lecture = new Section()
            {
                SectionCode = "LEC 0201",
                Sessions = new List<Session>()
                {
                    new Session()
                    {
                        StartTimeWithWeekday = 115.00,
                        EndTimeWithWeekday = 116.00
                    },
                    new Session()
                    {
                        StartTimeWithWeekday = 315.00,
                        EndTimeWithWeekday = 316.00
                    },
                    new Session()
                    {
                        StartTimeWithWeekday = 515.00,
                        EndTimeWithWeekday = 516.00
                    }
                }
            };

            Section csc258Lecture = new Section()
            {
                SectionCode = "LEC 0201",
                Sessions = new List<Session>()
                {
                    new Session()
                    {
                        StartTimeWithWeekday = 114.00,
                        EndTimeWithWeekday = 115.00
                    },
                    new Session()
                    {
                        StartTimeWithWeekday = 314.00,
                        EndTimeWithWeekday = 315.00
                    },
                    new Session()
                    {
                        StartTimeWithWeekday = 514.00,
                        EndTimeWithWeekday = 515.00
                    },
                    new Session()
                    {
                        StartTimeWithWeekday = 118.00,
                        EndTimeWithWeekday = 121.00
                    },
                }
            };
            Section sta248Lecture = new Section()
            {
                SectionCode = "LEC0101",
                Sessions = new List<Session>()
                {
                    new Session()
                    {
                        StartTimeWithWeekday = 210.00,
                        EndTimeWithWeekday = 212.00
                    },
                    new Session()
                    {
                        StartTimeWithWeekday = 411.00,
                        EndTimeWithWeekday = 412.00
                    }
                }
            };

            timetable.AddSection(mat235Lecture, "S");
            timetable.AddSection(mat235Tutorial, "S");
            timetable.AddSection(csc209Lecture, "S");
            timetable.AddSection(csc209Tutorial, "S");
            timetable.AddSection(csc258Lecture, "S");
            timetable.AddSection(csc263Lecture, "S");
            timetable.AddSection(sta248Lecture, "S");
        }

        [TestMethod]
        public void TestMethod1()
        {
            Session session = new Session()
            {
                StartTimeWithWeekday = 111.00,
                EndTimeWithWeekday = 114.00
            };
            double time = GetHoursInTimespan(session, 12, 17);
        }
    }
}
