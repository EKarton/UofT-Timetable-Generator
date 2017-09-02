using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.Generator
{
    /// <summary>
    /// Class contains statistical data during one generation
    /// </summary>
    public class StatsPerGeneration
    {
        /// <summary>
        /// Get the time elapsed when trying to complete a generation
        /// </summary>
        public double Runtime { get; set; }

        /// <summary>
        /// Get the diversity of the population at the current generation
        /// </summary>
        public double PopulationDiversity { get; set; }

        /// <summary>
        /// Get the average fitness scores in this generation
        /// </summary>
        public double AverageScores { get; set; }

        /// <summary>
        /// Get the max fitness scores in this generation
        /// </summary>
        public double MaxScores { get; set; }

        public static StatsPerGeneration operator +(StatsPerGeneration stats1, StatsPerGeneration stats2)
        {
            return new StatsPerGeneration()
            {
                Runtime = stats1.Runtime + stats2.Runtime,
                PopulationDiversity = stats1.PopulationDiversity + stats2.PopulationDiversity,
                AverageScores = stats1.AverageScores + stats2.AverageScores,
                MaxScores = stats1.MaxScores + stats2.MaxScores
            };
        }

        public static StatsPerGeneration operator /(StatsPerGeneration stats1, int value)
        {
            return new StatsPerGeneration()
            {
                Runtime = stats1.Runtime / value,
                PopulationDiversity = stats1.PopulationDiversity / value,
                AverageScores = stats1.AverageScores / value,
                MaxScores = stats1.MaxScores / value
            };
        }
    }
}
