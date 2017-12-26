using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UoftTimetableGenerator.DataModels;

namespace Tests.DataModelTests
{
    [TestClass]
    public class BuildingDistanceCalculatorTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            //BuildingDistanceCalculator calculator = new BuildingDistanceCalculator();
            //BuildingDistance distance = calculator.GetInfoBetweenBuildings("walking", "Queens Park, Toronto, ON", "Bahen");
            Tuple<string, string> tuple1 = new Tuple<string, string>("wasd", "wasddd");
            Dictionary<Tuple<string, string>, int> dictionary = new Dictionary<Tuple<string, string>, int>();
            dictionary.Add(tuple1, 12);

            Tuple<string, string> tuple2 = new Tuple<string, string>("wasd", "wasddd");
            Console.WriteLine(dictionary.ContainsKey(tuple2));
            Console.WriteLine(dictionary[tuple2]);
        }
    }
}
