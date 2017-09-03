using System;
using System.Collections.Generic;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    /// <summary>
    /// A class used to hold the sections present in one term
    /// </summary>
    public class SeasonalTimetable
    {
        // A list of all sections in this object 
        private List<Section> sections = new List<Section>();

        // The current session stored in this node
        private Session session = null;

        // Red-black-tree properties
        private string color = "Black";
        private SeasonalTimetable leftTree = null;
        private SeasonalTimetable rightTree = null;
        private SeasonalTimetable parent = null;

        /// <summary>
        /// Constructs an empty seasonal timetable
        /// </summary>
        public SeasonalTimetable() {  }

        /// <summary>
        /// Constructs a seasonal timetable-leaf
        /// </summary>
        /// <param name="session"></param>
        private SeasonalTimetable(Session session)
        {
            this.session = session;
            leftTree = new SeasonalTimetable();
            rightTree = new SeasonalTimetable();
        }

        /// <summary>
        /// Get the sibling of this red-black-tree
        /// </summary>
        private SeasonalTimetable Sibling
        {
            get
            {
                if (parent != null && parent.rightTree != null && parent.rightTree.session != null && parent.rightTree.session != session)
                    return parent.rightTree;
                else if (parent != null && parent.leftTree != null && parent.leftTree.session != null && parent.leftTree.session != session)
                    return parent.leftTree;
                else
                    return null;
            }
        }

        /// <summary>
        /// Determines if this red-black-tree is empty or not
        /// </summary>
        public bool IsEmpty
        {
            get { return session == null; }
        }

        /// <summary>
        /// Get / set the sections present in this red-black-tree
        /// </summary>
        public List<Section> Sections
        {
            get { return sections; }
        }

        /// <summary>
        /// True if this tree is a leaf; else false
        /// </summary>
        public bool IsLeaf
        {
            get
            {
                if (leftTree != null && rightTree != null)
                {
                    if (leftTree.IsEmpty && rightTree.IsEmpty && session != null)
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Determines is a session is in this red-black-tree; else false
        /// </summary>
        /// <param name="session">A session</param>
        /// <returns>True if it is in the red-black-tree; else false</returns>
        private bool Contains(Session session)
        {
            if (IsEmpty)
                return false;

            if (this.session.StartTime == session.StartTime && session.EndTime == session.EndTime)
                return true;

            if (leftTree != null && session.EndTime < this.session.StartTime)
                return leftTree.Contains(session);
            else if (rightTree != null && session.StartTime > this.session.EndTime)
                return rightTree.Contains(session);
            else
                return false;
        }

        /// <summary>
        /// Determines if a section is in this seasonal timetable
        /// </summary>
        /// <param name="section">A section</param>
        /// <returns>True if the section is in this seasonal timetable; else false</returns>
        public bool Contains(Section section)
        {
            foreach (Session session in section.Sessions)
                if (Contains(session) == false)
                    return false;
            return true;
        }

        /// <summary>
        /// Determines if a session fits in this red-black-tree
        /// </summary>
        /// <param name="session">A session</param>
        /// <returns>True if it fits; else false</returns>
        private bool DoesSessionFit(Session session)
        {
            if (IsEmpty)
                return true;

            if (leftTree != null && session.EndTime < this.session.StartTime)
                return leftTree.DoesSessionFit(session);
            else if (rightTree != null && this.session.EndTime < session.StartTime)
                return rightTree.DoesSessionFit(session);
            else
                return false;
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
                bool p = DoesSessionFit(session);
                if (DoesSessionFit(session) == false)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Get a list of sessions present in this red-black-tree
        /// </summary>
        /// <returns>A list of sessions in this tree</returns>
        private List<Session> GetSessions()
        {
            if (IsEmpty)
                return new List<Session>();

            // Perform an in-order traversal
            List<Session> leftSessions = leftTree.GetSessions();
            List<Session> curSession = new List<Session>() { session };
            List<Session> rightSession = rightTree.GetSessions();

            List<Session> sortedSessions = new List<Session>();
            sortedSessions.AddRange(leftSessions);
            sortedSessions.AddRange(curSession);
            sortedSessions.AddRange(rightSession);
            return sortedSessions;
        }

        /// <summary>
        /// Make a deep-copy of this timetable
        /// </summary>
        /// <returns></returns>
        public SeasonalTimetable MakeCopyOfTimetable()
        {
            if (IsEmpty)
                return new SeasonalTimetable();

            SeasonalTimetable newTree = new SeasonalTimetable(session);
            newTree.color = color;

            SeasonalTimetable leftSubtree_Copy = leftTree.MakeCopyOfTimetable();
            SeasonalTimetable rightSubtree_Copy = rightTree.MakeCopyOfTimetable();

            newTree.leftTree = leftSubtree_Copy;
            leftSubtree_Copy.parent = newTree;
            newTree.rightTree = rightSubtree_Copy;
            rightSubtree_Copy.parent = newTree;
            return newTree;
        }

        /// <summary>
        /// Determine if another timetable is the same as this timetable (by value)
        /// </summary>
        /// <param name="otherTable">Another timetable</param>
        /// <returns>True if it is the same; else false</returns>
        public bool IsEqualTo(SeasonalTimetable otherTable)
        {
            if (session != otherTable.session)
                return false;

            if (IsLeaf && otherTable.IsLeaf)
                return true;

            if (leftTree != null && otherTable.leftTree != null)
            {
                if (leftTree.parent != this || otherTable.leftTree.parent != otherTable)
                    return false;
            }

            if (rightTree != null && otherTable.rightTree != null)
            {
                if (rightTree.parent != this || otherTable.rightTree.parent != otherTable)
                    return false;
            }

            bool isLeftTreesEqual = leftTree.IsEqualTo(otherTable.leftTree);
            bool isRightTreesEqual = rightTree.IsEqualTo(otherTable.rightTree);
            return isLeftTreesEqual && isRightTreesEqual;
        }

        /// <summary>
        /// Print the current timetable in the console
        /// </summary>
        /// <param name="tabs"></param>
        public void Show(string tabs = "")
        {
            if (IsEmpty)
            {
                Console.WriteLine(tabs + " --NULL");
                return;
            }

            Console.WriteLine(tabs + " -" + session.StartTime + "_" + session.EndTime);
            leftTree.Show(tabs + "  ");
            rightTree.Show(tabs + "  ");
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
                AddSession(session);

            sections.Add(section);
            return true;
        }

        /// <summary>
        /// Add a session to this tree if it fits
        /// </summary>
        /// <param name="session">A session</param>
        /// <returns>True if the session fits and is added to this tree; else false</returns>
        private bool AddSession(Session session)
        {
            // If it is an empty tree
            if (IsEmpty)
            {
                this.session = session;
                color = "Black";
                leftTree = new SeasonalTimetable();
                rightTree = new SeasonalTimetable();
                return true;
            }

            // Left tree
            if (session.EndTime < this.session.StartTime)
            {
                if (leftTree.IsEmpty)
                {
                    leftTree = new SeasonalTimetable(session);
                    leftTree.parent = this;
                    leftTree.color = "Red";
                    RebalanceTree(leftTree);
                    return true;
                }
                else
                    return leftTree.AddSession(session);
            }

            // Right tree
            else if (session.StartTime > this.session.EndTime)
            {
                if (rightTree.IsEmpty)
                {
                    rightTree = new SeasonalTimetable(session);
                    rightTree.parent = this;
                    rightTree.color = "Red";
                    RebalanceTree(rightTree);
                    return true;
                }
                else
                    return rightTree.AddSession(session);
            }
            return false;
        }

        /// <summary>
        /// Rebalance the tree
        /// </summary>
        /// <param name="addedSession">The added session</param>
        private void RebalanceTree(SeasonalTimetable addedSession)
        {
            // Case 1
            if (color == "Black")
                return;

            // Case 2
            else if (color == "Red" && Sibling != null)
            {
                // Case 2A:
                if (Sibling.IsEmpty || Sibling.color == "Black")
                {
                    if (addedSession == leftTree && this == parent.leftTree)
                        PerformInsertionReordering1(addedSession);

                    else if (addedSession == rightTree && this == parent.leftTree)
                        PerformInsertionReordering2(addedSession);

                    else if (addedSession == rightTree && this == parent.rightTree)
                        PerformInsertionReordering3(addedSession);

                    else if (addedSession == leftTree && this == parent.rightTree)
                        PerformInsertionReordering4(addedSession);
                }

                // Case 2B:
                else if (Sibling.color == "Red")
                {
                    PerformInsertionRecoloring(addedSession);
                }
            }
        }

        /// <summary>
        /// Perform insertion reordering 1
        /// </summary>
        /// <param name="newNode">A child node in this tree that has the newly added session</param>
        private void PerformInsertionReordering1(SeasonalTimetable newNode)
        {
            SeasonalTimetable k = newNode;
            SeasonalTimetable p = this;
            SeasonalTimetable g = parent;
            SeasonalTimetable s = Sibling;

            SeasonalTimetable newG = new SeasonalTimetable(g.session);
            newG.color = "Red";
            newG.leftTree = p.rightTree;
            p.rightTree.parent = newG;
            newG.rightTree = s;
            s.parent = newG;

            g.session = p.session;
            g.leftTree = k;
            k.parent = g;
            g.rightTree = newG;
            newG.parent = g;
        }

        /// <summary>
        /// Perform insertion reordering 2
        /// </summary>
        /// <param name="newNode">A child node in this tree that has the newly added session</param>
        private void PerformInsertionReordering2(SeasonalTimetable newNode)
        {
            SeasonalTimetable k = newNode;
            SeasonalTimetable p = this;
            SeasonalTimetable g = parent;
            SeasonalTimetable s = Sibling;

            SeasonalTimetable newG = new SeasonalTimetable(g.session);
            newG.color = "Red";
            newG.rightTree = s;
            s.parent = newG;

            g.session = k.session;
        }

        /// <summary>
        /// Perform insertion reordering 3
        /// </summary>
        /// <param name="newNode">A child node in this tree that has the newly added session</param>
        private void PerformInsertionReordering3(SeasonalTimetable newNode)
        {
            SeasonalTimetable k = newNode;
            SeasonalTimetable p = this;
            SeasonalTimetable g = parent;
            SeasonalTimetable s = Sibling;

            SeasonalTimetable newG = new SeasonalTimetable(g.session);
            newG.color = "Red";
            newG.leftTree = s;
            s.parent = newG;
            newG.rightTree = p.leftTree;
            p.leftTree.parent = newG;

            g.session = p.session;
            g.leftTree = newG;
            newG.parent = g;
            g.rightTree = k;
            k.parent = g;
        }

        /// <summary>
        /// Perform insertion reordering 4
        /// </summary>
        /// <param name="newNode">A child node in this tree that has the newly added session</param>
        private void PerformInsertionReordering4(SeasonalTimetable newNode)
        {
            SeasonalTimetable k = newNode;
            SeasonalTimetable p = this;
            SeasonalTimetable g = parent;
            SeasonalTimetable s = Sibling;

            SeasonalTimetable newG = new SeasonalTimetable(g.session);
            newG.color = "Red";
            newG.leftTree = s;
            s.parent = newG;
            newG.rightTree = p.leftTree;
            p.leftTree.parent = newG;

            g.session = p.session;
            g.leftTree = newG;
            newG.parent = g;
            g.rightTree = k;
            k.parent = g;
        }

        /// <summary>
        /// Perform insertion recoloring
        /// </summary>
        /// <param name="newNode">A child node in this tree that has the newly added session</param>
        private void PerformInsertionRecoloring(SeasonalTimetable newNode)
        {
            SeasonalTimetable k = newNode;
            SeasonalTimetable p = this;
            SeasonalTimetable g = parent;
            SeasonalTimetable s = Sibling;

            g.color = "Red";
            p.color = "Black";
            s.color = "Black";

            // Handle case if g is the root node
            if (g.parent == null)
                g.color = "Black";

            else if (g.parent.color == "Red")
                g.parent.RebalanceTree(g);
        }

        /// <summary>
        /// Get the total amount of time spent in class
        /// </summary>
        public double TimeInClass
        {
            get
            {
                if (IsEmpty)
                    return 0;

                double hrsOfClass = session.GetEndTime_Time() - session.GetStartTime_Time();
                hrsOfClass += leftTree.TimeInClass;
                hrsOfClass += rightTree.TimeInClass;
                return hrsOfClass;
            }
        }

        /// <summary>
        /// Get the total amount of time spent in between classes
        /// </summary>
        public double TotalTimeBetweenClasses
        {
            get
            {
                if (IsEmpty)
                    return 0;

                if (IsLeaf)
                    return 0;

                // Getting the wasted time from the left and right trees
                double totalWastedTime = 0;
                totalWastedTime += leftTree.TotalTimeBetweenClasses;
                totalWastedTime += rightTree.TotalTimeBetweenClasses;

                // Getting the time wasted in left trees
                if (!leftTree.IsEmpty)
                {
                    // If the previous session occured on the same day as this session
                    if (leftTree.session.GetEndTime_WeekdayIndex() == session.GetStartTime_WeekdayIndex())
                        totalWastedTime += session.StartTime - leftTree.session.EndTime;
                }

                // Getting the time wasted in the right trees
                if (!rightTree.IsEmpty)
                {
                    // If the next session occurs on the same day as this session
                    if (rightTree.session.GetStartTime_WeekdayIndex() == session.GetEndTime_WeekdayIndex())
                        totalWastedTime += rightTree.session.StartTime - session.EndTime;
                }

                return totalWastedTime;
            }
        }

        /// <summary>
        /// Get the start time (24-hr time) of the earliest class in this timetable
        /// </summary>
        public double EarliestClasstime
        {
            get
            {
                // If it is empty
                if (IsEmpty)
                    return -1;

                // If it is the leaf
                if (IsLeaf)
                    return session.GetStartTime_Time();

                // Recurse to the next sessions
                double curStartTime = session.GetStartTime_Time();
                if (!leftTree.IsEmpty)
                    curStartTime = Math.Min(curStartTime, leftTree.EarliestClasstime);
                if (!rightTree.IsEmpty)
                    curStartTime = Math.Min(curStartTime, rightTree.EarliestClasstime);

                return curStartTime;
            }
        }

        /// <summary>
        /// Get the time (24-hr time) of the latest class in this timetable
        /// </summary>
        public double LatestClasstime
        {
            get
            {
                // If it is empty
                if (IsEmpty)
                    return -1;

                // If it is the leaf
                if (IsLeaf)
                    return session.GetEndTime_Time();

                // Recurse to the next sessions
                double curEndTime = session.GetEndTime_Time();
                if (!leftTree.IsEmpty)
                    curEndTime = Math.Max(curEndTime, leftTree.LatestClasstime);
                if (!rightTree.IsEmpty)
                    curEndTime = Math.Max(curEndTime, rightTree.LatestClasstime);

                return curEndTime;
            }
        }

        /// <summary>
        /// Get / set the walk duration in each back to back classes
        /// </summary>
        public List<double> WalkDurationInBackToBackClasses
        {
            get { return new List<double>(); }
        }

        /// <summary>
        /// Get / set the total walk duration in between classes
        /// </summary>
        public double TotalWalkDuration
        {
            get { return 0; }
        }

        /// <summary>
        /// Get the average walk distance in between classes
        /// </summary>
        public int AverageWalkingDistance
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}