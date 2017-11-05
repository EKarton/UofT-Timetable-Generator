using System;
using System.Collections.Generic;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    /// <summary>
    /// A class used to hold the sections present in one term
    /// </summary>
    public class SeasonalTimetable : ITimetable
    {
        // A sorted tree used to hold the timetable (in order)
        private RedBlackTree<Session> tree = new RedBlackTree<Session>();

        // A list of all sections in this object 
        private List<Section> sections = new List<Section>();

        /// <summary>
        /// Get the average walk distance in between classes
        /// </summary>
        public int AverageWalkingDistance
        {
            get { return 0; }
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
            get { return new List<double>(); }
        }

        /// <summary>
        /// Get the time (24-hr time) of the latest class in this timetable
        /// </summary>
        public double LatestClassTime
        {
            get { return GetLatestClassTime(tree); }
        }

        /// <summary>
        /// Get the start time (24-hr time) of the earliest class in this timetable
        /// </summary>
        public double EarliestClassTime
        {
            get { return GetEarliestClassTime(tree); }
        }

        /// <summary>
        /// Get the total amount of time spent in between classes
        /// </summary>
        public double TotalTimeBetweenClasses
        {
            get { return GetTotalTimeBetweenClasses(tree); }
        }

        /// <summary>
        /// Get the total amount of time spent in class
        /// </summary>
        public double TimeInClass
        {
            get { return GetTotalTimeInClass(tree); }
        }

        /// <summary>
        /// Get the total number of days in class
        /// </summary>
        public int NumDaysInClass
        {
            get { return 0; }
        }

        /// <summary>
        /// Get the days in class
        /// </summary>
        public List<string> DaysInClass
        {
            get { return new List<string>(); }
        }

        /// <summary>
        /// Get a list of times between classes
        /// </summary>
        public List<double> TimeBetweenClasses
        {
            get { return new List<double>(); }
        }

        /// <summary>
        /// Add a section in this timetable if it fits
        /// </summary>
        /// <param name="section">A section</param>
        /// <returns>True if the section fits and has been added to the timetable; else false</returns>
        public bool AddSection(Section section)
        {
            // Check if it can fit
            if (DoesSectionFit(section) == false)
                return false;

            foreach (Session session in section.Sessions)
                tree.Add(session);

            sections.Add(section);
            return true;
        }

        /// <summary>
        /// Prints the tree into the console
        /// </summary>
        public void Show()
        {
            Show(tree);
        }

        /// <summary>
        /// Prints the tree onto the console
        /// </summary>
        /// <param name="tree">A node in the tree</param>
        /// <param name="tabs">The number of tabs to offset the println()</param>
        private void Show(RedBlackTree<Session> tree, string tabs = "")
        {
            if (tree.IsEmpty)
            {
                Console.WriteLine(tabs + " --NULL");
                return;
            }

            Console.WriteLine(tabs + " -" + tree.Content.StartTime + "_" + tree.Content.EndTime);
            Show(tree.LeftTree, tabs + "  ");
            Show(tree.RightTree, tabs + "  ");
        }

        /// <summary>
        /// Determines if a section is in this seasonal timetable
        /// </summary>
        /// <param name="section">A section</param>
        /// <returns>True if the section is in this seasonal timetable; else false</returns>
        public bool Contains(Section section)
        {
            return sections.Contains(section);
        }

        /// <summary>
        /// Finds and deletes the desired section from this timetable
        /// </summary>
        /// <param name="section">The section to delete</param>
        /// <returns>Returns true if the section was found and deleted; else false</returns>
        public bool DeleteSection(Section section)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines if a section fits in this timetable
        /// </summary>
        /// <param name="section">A section</param>
        /// <returns>True if it fits; else false</returns>
        public bool DoesSectionFit(Section section)
        {
            foreach (Session session in section.Sessions)
            {
                if (!tree.CanAdd(session))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Calculates and returns the earliest class time in this node
        /// </summary>
        /// <param name="node">A node in the tree</param>
        /// <returns>The earliest class time in this node</returns>
        private double GetEarliestClassTime(RedBlackTree<Session> node)
        {
            // If it is empty
            if (node.IsEmpty)
                return -1;

            // If it is the leaf
            if (node.IsLeaf)
                return node.Content.GetStartTime_Time();

            // Recurse to the next sessions
            double curStartTime = node.Content.GetStartTime_Time();
            if (!node.LeftTree.IsEmpty)
                curStartTime = Math.Min(curStartTime, GetEarliestClassTime(node.LeftTree));
            if (!node.RightTree.IsEmpty)
                curStartTime = Math.Min(curStartTime, GetEarliestClassTime(node.RightTree));

            return curStartTime;
        }

        /// <summary>
        /// Returns the latest class time in the current node
        /// </summary>
        /// <param name="node">A node in the red black tree</param>
        /// <returns>Latest class time in this node</returns>
        private double GetLatestClassTime(RedBlackTree<Session> node)
        {
            // If it is empty
            if (node.IsEmpty)
                return -1;

            // If it is the leaf
            if (node.IsLeaf)
                return node.Content.GetEndTime_Time();

            // Recurse to the next sessions
            double curEndTime = node.Content.GetEndTime_Time();
            if (!node.LeftTree.IsEmpty)
                curEndTime = Math.Max(curEndTime, GetLatestClassTime(node.LeftTree));
            if (!node.RightTree.IsEmpty)
                curEndTime = Math.Max(curEndTime, GetLatestClassTime(node.RightTree));

            return curEndTime;
        }

        /// <summary>
        /// Calculates and returns the total time spent in class
        /// </summary>
        /// <param name="node">The current node in the tree</param>
        /// <returns>The total time spent in class</returns>
        private double GetTotalTimeInClass(RedBlackTree<Session> node)
        {
            if (node.IsEmpty)
                return 0;

            double hrsOfClass = node.Content.GetEndTime_Time() - node.Content.GetStartTime_Time();
            hrsOfClass += GetTotalTimeInClass(node.LeftTree);
            hrsOfClass += GetTotalTimeInClass(node.RightTree);
            return hrsOfClass;
        }

        /// <summary>
        /// Calculates and returns the total time between classes
        /// </summary>
        /// <param name="node">The current node in the tree</param>
        /// <returns>The total time between classes</returns>
        private double GetTotalTimeBetweenClasses(RedBlackTree<Session> node)
        {
            if (node.IsEmpty)
                return 0;

            if (node.IsLeaf)
                return 0;

            // Getting the wasted time from the left and right trees
            double totalWastedTime = 0;
            totalWastedTime += GetTotalTimeBetweenClasses(node.LeftTree);
            totalWastedTime += GetTotalTimeBetweenClasses(node.RightTree);

            // Getting the time wasted in left trees
            if (!node.LeftTree.IsEmpty)
            {
                // If the previous session occured on the same day as this session
                if (node.LeftTree.Content.GetEndTime_WeekdayIndex() == node.Content.GetStartTime_WeekdayIndex())
                    totalWastedTime += node.Content.StartTime - node.LeftTree.Content.EndTime;
            }

            // Getting the time wasted in the right trees
            if (!node.RightTree.IsEmpty)
            {
                // If the next session occurs on the same day as this session
                if (node.RightTree.Content.GetStartTime_WeekdayIndex() == node.Content.GetEndTime_WeekdayIndex())
                    totalWastedTime += node.RightTree.Content.StartTime - node.Content.EndTime;
            }
            return totalWastedTime;
        }

        /// <summary>
        /// Returns the sections that exist in this timetable
        /// </summary>
        /// <returns>The sections in this timetable</returns>
        public List<Section> GetSections()
        {
            return sections;
        }

        /// <summary>
        /// Make a deep-copy of this timetable
        /// </summary>
        /// <returns></returns>
        public ITimetable MakeCopy()
        {
            SeasonalTimetable copy = new SeasonalTimetable();
            foreach (Section section in sections)
                copy.AddSection(section);
            return copy;
        }
    }
}