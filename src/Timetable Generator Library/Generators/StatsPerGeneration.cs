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

        /// <summary>
        /// Add two stats together
        /// </summary>
        /// <param name="stats1">Stats 1</param>
        /// <param name="stats2">Stats 2</param>
        /// <returns>The sum of the two stats</returns>
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

        /// <summary>
        /// Divide stats'properties by a value
        /// </summary>
        /// <param name="stats1">A stats obj</param>
        /// <param name="value">A value</param>
        /// <returns>The result of dividing the stats by a value</returns>
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
