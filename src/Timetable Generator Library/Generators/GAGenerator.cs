using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    /// <summary>
    /// A class used to generate UofT timetables with a genetic algorithm
    /// </summary>
    public class GAGenerator<T> : ITimetableGenerator<T> where T : ITimetable, new()
    {
        private static Random random = new Random();
        private double mutationRate = 0.1;
        private double crossoverRate = 0.9;
        private int populationSize = 16;
        private int numGenerations = 100;

        // The preferences / restrictions (makes up the fitness model)
        private Preferences preferences;
        private Restrictions restrictions;

        // Represents the number of sections that needs to be in the timetable
        private List<Section[]> requiredSections = new List<Section[]>();

        private int maxSessions = 0;

        // Represents the term that requiredSections[i] belong to                                                   
        private List<int[]> population = new List<int[]>();
        private double[] fitnessScores;

        // Cached SeasonalTimetable
        private Dictionary<string, T> cachedTimetables = new Dictionary<string, T>();

        /// <summary>
        /// Constructs the GAGenerator object
        /// </summary>
        /// <param name="courses">A list of courses</param>
        /// <param name="preferences">The preferences</param>
        /// <param name="restrictions">The restrictions</param>
        public GAGenerator(List<Course> courses, Preferences preferences, Restrictions restrictions)
        {
            this.preferences = preferences;
            this.restrictions = restrictions;

            // Populating requiredSections[] and its associated terms[]
            foreach (Course course in courses)
            {
                foreach (Activity activity in course.Activities)
                    requiredSections.Add(activity.Sections.ToArray());
            }

            // Calculate the max number of sessions
            foreach (Section[] sessions in requiredSections)
            {
                if (sessions.Length > maxSessions)
                    maxSessions = sessions.Length;
            }
        }

        /// <summary>
        /// Get / set the mutation rate of the genetic algorithm
        /// </summary>
        public double MutationRate
        {
            get { return mutationRate; }
            set { mutationRate = value; }
        }

        /// <summary>
        /// Get / set the crossover rate of the genetic algorithm
        /// </summary>
        public double CrossoverRate
        {
            get { return crossoverRate; }
            set { crossoverRate = value; }
        }

        /// <summary>
        /// Get / set the population size of the genetic algorithm
        /// </summary>
        public int PopulationSize
        {
            get { return populationSize; }
            set { populationSize = value; }
        }

        /// <summary>
        /// Get / set the number of generations  for the genetic algorithm
        /// </summary>
        public int NumGenerations
        {
            get { return numGenerations; }
            set { numGenerations = value; }
        }

        /// <summary>
        /// Serialize a table into a string
        /// This is done by having each digit in the table[] padded to the left with x amount of zeros,
        /// where 'x' is the max number of sections available in requiredSections[]
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private string SerializeTable(int[] table)
        {
            string serializedTable = "";
            foreach (int sectionIndex in table)
                serializedTable += sectionIndex.ToString().PadLeft(maxSessions, '0');
            return serializedTable;
        }

        /// <summary>
        /// Perform one evolution in the population
        /// </summary>
        private void EvolvePopulation()
        {
            // Store the best one
            int bestTable = GetBestTable(0, population.Count);
            int[] temp = population[0];
            double tempScore = fitnessScores[0];
            population[0] = population[bestTable];
            fitnessScores[0] = fitnessScores[bestTable];
            population[bestTable] = temp;
            fitnessScores[bestTable] = tempScore;

            List<int[]> newGeneration = new List<int[]>();

            // Perform crossovers
            for (int i = 0; i < population.Count; i++)
            {
                int parent1Index = PerformTournamentSelection();
                int parent2Index = PerformTournamentSelection();
                int[] parent1 = population[parent1Index];
                int[] parent2 = population[parent2Index];

                int[] child = PerformOldCrossover(parent1, parent2);
                if (child != null)
                {
                    PerformMutation(child);
                    newGeneration.Add(child);
                }
                else
                {
                    if (fitnessScores[parent1Index] > fitnessScores[parent2Index])
                        newGeneration.Add(parent1);
                    else
                        newGeneration.Add(parent2);
                }
            }

            // Replace the old generation
            for (int i = 0; i < population.Count; i++)
                population[i] = newGeneration[i];

            // Compute the fitness scores for each table
            for (int i = 0; i < population.Count; i++)
                fitnessScores[i] = GetFitnessScore(population[i]);
        }

        /// <summary>
        /// Perform the standard crossover with two parents
        /// </summary>
        /// <param name="parent1">The gene to the first parent (table)</param>
        /// <param name="parent2">The gene to the second parent (table)</param>
        /// <returns>
        /// A child, represented with a gene. 
        /// If the parents cannot reproduce, then it will return null
        /// </returns>
        private int[] PerformOldCrossover(int[] parent1, int[] parent2)
        {
            if (random.NextDouble() > crossoverRate)
                return null;

            int[] child = new int[requiredSections.Count];

            int randIndex = random.Next(0, requiredSections.Count);
            for (int i = 0; i < randIndex; i++)
                child[i] = parent1[i];

            for (int i = randIndex; i < requiredSections.Count; i++)
                child[i] = parent2[i];

            return child;
        }

        /// <summary>
        /// Performs random mutation on a table
        /// </summary>
        /// <param name="table">The gene to a table</param>
        private void PerformMutation(int[] table)
        {
            for (int i = 0; i < requiredSections.Count; i++)
            {
                if (random.NextDouble() <= mutationRate)
                {
                    int randSession = random.Next(0, requiredSections[i].Length);
                    table[i] = randSession;
                }
            }
        }

        /// <summary>
        /// Perform a tournament selection on the current population[]
        /// </summary>
        /// <returns>The index to a table in population[]</returns>
        private int PerformTournamentSelection()
        {
            int index1 = random.Next(0, population.Count);
            int index2 = random.Next(0, population.Count);
            if (index1 < index2)
                return GetBestTable(index1, index2 + 1);
            else
                return GetBestTable(index2, index1 + 1);
        }

        /// <summary>
        /// Get the best table in a range of tables in population[] 
        /// based on their fitness scores, stored in fitnessScores[]
        /// </summary>
        /// <param name="leftIndex">The left range in population[]</param>
        /// <param name="rightIndex">The right range in population[]</param>
        /// <returns>The index to the best table in population[]</returns>
        private int GetBestTable(int leftIndex, int rightIndex)
        {
            int bestTable = -1;
            double bestRank = -1;
            for (int i = leftIndex; i < rightIndex; i++)
            {
                double curRank = fitnessScores[i];
                if (curRank > bestRank)
                {
                    bestRank = curRank;
                    bestTable = i;
                }
            }
            return bestTable;
        }

        /// <summary>
        /// Generates a random table
        /// </summary>
        /// <returns>A random table</returns>
        private int[] GenerateRandomTable()
        {
            int[] table = new int[requiredSections.Count];
            for (int i = 0; i < table.Length; i++)
            {
                int randSession = random.Next(0, requiredSections[i].Length);
                table[i] = randSession;
            }
            return table;
        }

        /// <summary>
        /// Gets a timetable by either creating one or seeing if it already exists in the cache
        /// </summary>
        /// <param name="table">A gene to the table</param>
        /// <returns>An obj representation of a table</returns>
        private T GetTimetable(int[] table)
        {
            // Check if it already exists
            string serializedTable = SerializeTable(table);
            if (cachedTimetables.ContainsKey(serializedTable))
                return cachedTimetables[serializedTable];

            else
            {
                // Check if it can be added
                T newTable = new T();
                for (int i = 0; i < table.Length; i++)
                {
                    Section section = requiredSections[i][table[i]];
                    bool success = newTable.AddSection(section);
                    if (success == false)
                        return default(T);
                }
                return newTable;
            }
        }

        /// <summary>
        /// Get the fitness score of a table
        /// </summary>
        /// <param name="table">The gene of a table</param>
        /// <returns>The fitness score of the table</returns>
        public double GetFitnessScore(int[] table)
        { 
            T timetable = GetTimetable(table);

            // If the table is an invalid table, then its score is 0
            if (timetable == null)
                return 0;

            // Check if it meets the restrictions
            if (restrictions.EarliestClass != null && timetable.EarliestClassTime < restrictions.EarliestClass)
                return 0;
            if (restrictions.LatestClass != null && timetable.LatestClassTime > restrictions.LatestClass)
                return 0;
            if (restrictions.WalkDurationInBackToBackClasses != null)
            {
                foreach (double dur in timetable.WalkDurationInBackToBackClasses)
                    if (dur > restrictions.WalkDurationInBackToBackClasses)
                        return 0;
            }

            double score = 1000;

            // Get scores associated by their preferences
            switch(preferences.ClassType)
            {
                case Preferences.Day.Undefined:
                    break;
                case Preferences.Day.Morning: // (12am - 12pm)
                    if (0 < timetable.EarliestClassTime && timetable.LatestClassTime < 12)
                        score += 100;
                    break;
                case Preferences.Day.Afternoon: // (12pm - 5pm)
                    if (12 <= timetable.EarliestClassTime && timetable.LatestClassTime < 17)
                        score += 100;
                    break;
                case Preferences.Day.Evening: // (5pm - 8pm)
                    if (17 <= timetable.EarliestClassTime && timetable.LatestClassTime < 20)
                        score += 100;
                    break;
                case Preferences.Day.Night: // (9pm - 12pm)
                    if (20 <= timetable.EarliestClassTime && timetable.LatestClassTime <= 24)
                        score += 100;
                    break;
                default:
                    throw new Exception("Class type not handled before!");
            }
            switch(preferences.TimeBetweenClasses)
            {
                case Preferences.Quantity.Undefined:
                    break;
                case Preferences.Quantity.Minimum:
                    score -= timetable.TotalTimeBetweenClasses;
                    break;
                case Preferences.Quantity.Maximum:
                    score += timetable.TotalTimeBetweenClasses;
                    break;
                default:
                    throw new Exception("Time between class is not handled before!");
            }
            return score;
        }

        /// <summary>
        /// Run the algorithm and get the stats per generation
        /// </summary>
        /// <returns>The stats per generation</returns>
        public StatsPerGeneration[] GenerateTimetablesWithStats()
        {
            fitnessScores = new double[populationSize];
            population = new List<int[]>();

            // Generate an initial population randomly
            for (int i = 0; i < populationSize; i++)
            {
                int[] randomTable = GenerateRandomTable();
                population.Add(randomTable);
            }

            // Compute the fitness scores for each table
            for (int i = 0; i < population.Count; i++)
                fitnessScores[i] = GetFitnessScore(population[i]);

            // Compute average fit scores per generation
            Stopwatch watch = new Stopwatch();
            var stats = new StatsPerGeneration[numGenerations];
            for (int i = 0; i < numGenerations; i++)
            {
                watch.Reset();
                watch.Start();
                EvolvePopulation();
                watch.Stop();

                // Compute the average fitness scores in this generation
                double totalScore = 0;
                foreach (int[] table in population)
                    totalScore += GetFitnessScore(table);
                double avgScore = totalScore / population.Count;

                // Compute the max fitness score in this generation
                double maxScore = 0;
                foreach (int[] table in population)
                {
                    double curScore = GetFitnessScore(table);
                    if (curScore > maxScore)
                        maxScore = curScore;
                }

                // Calculate diversity
                Dictionary<string, int> tableCount = new Dictionary<string, int>();
                foreach (int[] table in population)
                {
                    string serializedTable = SerializeTable(table);
                    if (tableCount.ContainsKey(serializedTable))
                        tableCount[serializedTable] += 1;
                    else
                        tableCount.Add(serializedTable, 1);
                }
                double diversityPercentage = tableCount.Count / (population.Count * 1.0);

                // Save the stats
                StatsPerGeneration curStats = new StatsPerGeneration()
                {
                    Runtime = watch.ElapsedMilliseconds,
                    PopulationDiversity = diversityPercentage,
                    AverageScores = avgScore,
                    MaxScores = maxScore
                };
                stats[i] = curStats;
            }
            return stats;
        }

        /// <summary>
        /// Generate the timetables
        /// </summary>
        /// <returns>A list of timetables generated</returns>
        public List<T> GetTimetables()
        {
            fitnessScores = new double[populationSize];
            population = new List<int[]>();

            // Generate an initial population randomly
            for (int i = 0; i < populationSize; i++)
            {
                int[] randomTable = GenerateRandomTable();
                population.Add(randomTable);
            }

            // Compute the fitness scores for each table
            for (int i = 0; i < population.Count; i++)
                fitnessScores[i] = GetFitnessScore(population[i]);

            // Evolve the generations
            for (int i = 0; i < numGenerations; i++)
                EvolvePopulation();

            // Getting timetables that has every single required activity
            List<T> timetables = new List<T>();
            for (int i = 0; i < population.Count; i++)
            {
                int[] table = population[i];
                if (fitnessScores[i] > 0)
                {
                    T timetable = new T();
                    for (int j = 0; j < table.Length; j++)
                    {
                        Section section = requiredSections[j][table[j]];
                        timetable.AddSection(section);
                    }
                    timetables.Add(timetable);
                }
            }

            return timetables;
        }
    }
}
