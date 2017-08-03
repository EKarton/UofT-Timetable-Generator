using System;
using System.Collections.Generic;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    public class SeasonalTimetable
    {
        private List<Section> sections = new List<Section>();
        private Session session = null;
        private string color = "Black";
        private SeasonalTimetable leftTree = null;
        private SeasonalTimetable rightTree = null;
        private SeasonalTimetable parent = null;

        public SeasonalTimetable() {  }

        private SeasonalTimetable(Session session)
        {
            this.session = session;
            leftTree = new SeasonalTimetable();
            rightTree = new SeasonalTimetable();
        }

        private SeasonalTimetable Sibling
        {
            get
            {
                if (parent.rightTree.session != session)
                    return parent.rightTree;
                else if (parent.leftTree.session != session)
                    return parent.leftTree;
                return null;
            }
        }

        public bool IsEmpty
        {
            get { return session == null; }
        }

        public List<Section> Sections
        {
            get { return sections; }
        }

        public bool IsLeaf()
        {
            if (leftTree != null && rightTree != null)
            {
                if (leftTree.IsEmpty && rightTree.IsEmpty && session != null)
                    return true;
            }
            return false;
        }

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

        public bool Contains(Section section)
        {
            foreach (Session session in section.Sessions)
                if (Contains(session) == false)
                    return false;
            return true;
        }

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

        public bool IsEqualTo(SeasonalTimetable otherTable)
        {
            if (session != otherTable.session)
                return false;

            if (IsLeaf() && otherTable.IsLeaf())
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

        private void RebalanceTree(SeasonalTimetable addedSession)
        {
            // Case 1
            if (color == "Black")
                return;

            // Case 2
            else if (color == "Red")
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

        public double HoursOfClass
        {
            get
            {
                if (IsEmpty)
                    return 0;

                double hrsOfClass = session.EndTime - session.StartTime;
                hrsOfClass += leftTree.HoursOfClass;
                hrsOfClass += rightTree.HoursOfClass;
                return hrsOfClass;
            }
        }

        public double TotalSpacesBetweenClasses
        {
            get
            {
                if (IsEmpty)
                    return 0;

                if (leftTree.IsEmpty && rightTree.IsEmpty)
                    return 0;

                // Getting the wasted time from the left and right trees
                double totalWastedTime = 0;
                totalWastedTime += leftTree.TotalSpacesBetweenClasses;
                totalWastedTime += rightTree.TotalSpacesBetweenClasses;

                // Getting the time wasted in left trees
                if (!leftTree.IsEmpty)
                {
                    // If the previous session occured on the same day as this session
                    if (leftTree.session.GetEndTime_WeekdayIndex() == session.GetEndTime_WeekdayIndex())
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

        public int EarliestClasstime
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int LatestClasstime
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int AverageWalkingDistance
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}