using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UoftTimetableGenerator.DataModels;
using System.Collections.Generic;
using UoftTimetableGenerator.Generator;

namespace Uoft_Timetable_Generator_Tests
{
    [TestClass]
    public class GAGeneratorTests
    {
        public class Integer : IComparable
        {
            private int value = 0;

            public Integer(int val)
            {
                value = val;
            }

            public int CompareTo(object obj)
            {
                if (obj is Integer == false)
                    return 0;
                Integer other = (Integer)obj;
                if (value <= other.value)
                    return -1;
                else if (other.value <= value)
                    return 1;
                else
                    return 0;
            }

            public override string ToString()
            {
                return value + "";
            }
        }

        [TestMethod]
        public void SimpleTest()
        {
            RedBlackTree<Integer> tree = new RedBlackTree<Integer>();
            tree.Add(new Integer(1));
            tree.Add(new Integer(2));
            tree.Add(new Integer(3));
            tree.Add(new Integer(4));
            tree.Show();
            List<Integer> contents = tree.GetContents();
            foreach (Integer i in contents)
                Console.WriteLine(i);
        }
    }
}
