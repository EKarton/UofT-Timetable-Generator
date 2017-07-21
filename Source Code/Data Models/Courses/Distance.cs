using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.DataModels
{
    public struct Distance
    {
        public Distance(double kilometer, double meters, double centimeters, double millimeters)
        {
            Kilometers = kilometer;
            Meters = meters;
            Centimeters = centimeters;
            Millimeters = millimeters;
        }

        public Distance(double meters, double centimeters, double millimeters)
        {
            Kilometers = 0;
            Meters = meters;
            Centimeters = centimeters;
            Millimeters = millimeters;
        }

        public Distance(double centimeters, double millimeters)
        {
            Kilometers = 0;
            Meters = 0;
            Centimeters = centimeters;
            Millimeters = millimeters;
        }

        public Distance(double millimeters)
        {
            Kilometers = 0;
            Meters = 0;
            Centimeters = 0;
            Millimeters = millimeters;
        }

        public double Kilometers { get; set; }
        public double Meters { get; set; }
        public double Centimeters { get; set; }
        public double Millimeters { get; set; }
    }
}
