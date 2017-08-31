using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    public class GAGenerator
    {
        private static Random random = new Random();
        private double mutationRate = 0.1;
        private double crossoverRate = 0.9;
        private int populationSize = 16;
        private int numGenerations = 100;
        private string crossoverType = "Old Crossover";

        // The preferences / restrictions (makes up the fitness model)
        private Preferences preferences;
        private Restrictions restrictions;

        // Represents the number of sections that needs to be in the timetable
        private List<Section[]> requiredSections = new List<Section[]>();

        // Represents the term that requiredSections[i] belong to
        private List<char> terms = new List<char>();                                                       
        private List<int[]> population = new List<int[]>();
        private double[] fitnessScores;

        public GAGenerator(List<Course> courses, Preferences preferences, Restrictions restrictions)
        {
            this.preferences = preferences;
            this.restrictions = restrictions;

            // Populating requiredSections[] and its associated terms[]
            foreach (Course course in courses)
            {
                char term = course.Term[0];
                foreach (Activity activity in course.Activities)
                {
                    requiredSections.Add(activity.Sections.ToArray());
                    terms.Add(term);
                }
            }
        }

        public double MutationRate
        {
            get { return mutationRate; }
            set { mutationRate = value; }
        }

        public double CrossoverRate
        {
            get { return crossoverRate; }
            set { crossoverRate = value; }
        }

        public int PopulationSize
        {
            get { return populationSize; }
            set { populationSize = value; }
        }

        public int NumGenerations
        {
            get { return numGenerations; }
            set { numGenerations = value; }
        }

        public string CrossoverType
        {
            get { return crossoverType; }
            set { crossoverType = value; }
        }

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

                int[] child = PerformOldCrossover(population[parent1Index], population[parent2Index]);
                if (child != null)
                {
                    PerformMutation(child);
                    newGeneration.Add(child);
                }
                else
                {
                    if (fitnessScores[parent1Index] > fitnessScores[parent2Index])
                        newGeneration.Add(population[parent1Index]);
                    else
                        newGeneration.Add(population[parent2Index]);
                }
            }

            // Replace the old generation
            for (int i = 0; i < population.Count; i++)
                population[i] = newGeneration[i];

            // Compute the fitness scores for each table
            for (int i = 0; i < population.Count; i++)
                fitnessScores[i] = GetFitnessScore(population[i]);
        }

        private Tuple<int[], int[]> PerformSinglePointCrossover(int[] parent1, int[] parent2)
        {
            if (random.NextDouble() > crossoverRate)
                return null;

            int[] child1 = new int[parent1.Length];
            int[] child2 = new int[parent1.Length];
            int mid = random.Next(0, parent1.Length);

            for (int i = 0; i < mid; i++)
            {
                child1[i] = parent1[i];
                child2[i] = parent2[i];
            }

            for (int i = mid; i < parent1.Length; i++)
            {
                child1[i] = parent2[i];
                child2[i] = parent1[i];
            }

            return new Tuple<int[], int[]>(child1, child2);
        }

        private Tuple<int[], int[]> PerformDoublePointCrossover(int[] parent1, int[] parent2)
        {
            if (random.NextDouble() > crossoverRate)
                return null;

            int[] child1 = new int[parent1.Length];
            int[] child2 = new int[parent1.Length];

            int index1 = random.Next(0, parent1.Length);
            int index2 = random.Next(0, parent1.Length);
            int leftIndex = Math.Min(index1, index2);
            int rightIndex = Math.Min(index1, index2);

            for (int i = 0; i < leftIndex; i++)
            {
                child1[i] = parent1[i];
                child2[i] = parent2[i];
            }

            for (int i = leftIndex; i < rightIndex; i++)
            {
                child1[i] = parent2[i];
                child2[i] = parent1[i];
            }

            for (int i = rightIndex; i < parent1.Length; i++)
            {
                child1[i] = parent1[i];
                child2[i] = parent2[i];
            }

            return new Tuple<int[], int[]>(child1, child2);
        }

        private Tuple<int[], int[]> PerformUniformCrossover(int[] parent1, int[] parent2)
        {
            if (random.NextDouble() > crossoverRate)
                return null;

            int[] child1 = new int[parent1.Length];
            int[] child2 = new int[parent2.Length];
            for (int i = 0; i < parent1.Length; i++)
            {
                double coinToss = random.NextDouble();
                if (coinToss < 0.5)
                {
                    child1[i] = parent2[i];
                    child2[i] = parent1[i];
                }
                else
                {
                    child1[i] = parent2[i];
                    child2[i] = parent1[i];
                }
            }
            return new Tuple<int[], int[]>(child1, child2);
        }

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

        private int PerformTournamentSelection()
        {
            int index1 = random.Next(0, population.Count);
            int index2 = random.Next(0, population.Count);
            if (index1 < index2)
                return GetBestTable(index1, index2 + 1);
            else
                return GetBestTable(index2, index1 + 1);
        }

        private int GetBestTable(int leftIndex, int rightIndex)
        {
            int bestTable = -1;
            double bestRank = -1;
            for (int i = leftIndex; i < rightIndex ; i++)
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

        public double GetFitnessScore(int[] table)
        {
            double score = 0;

            // Check if it can be added
            YearlyTimetable newTable = new YearlyTimetable();
            for (int i = 0; i < table.Length; i++)
            {
                Section section = requiredSections[i][table[i]];
                char term = terms[i];
                bool success = newTable.AddSection(section, term);
                if (success == false)
                    return 0;
            }

            // Check if it meets the restrictions
            if (restrictions.EarliestClass != null && newTable.EarliestClassTime < restrictions.EarliestClass)
                return 0;
            if (restrictions.LatestClass != null && newTable.LatestClassTime > restrictions.LatestClass)
                return 0;
            if (restrictions.WalkDurationInBackToBackClasses != null)
            {
                foreach (double dur in newTable.WalkDurationInBackToBackClasses)
                    if (dur > restrictions.WalkDurationInBackToBackClasses)
                        return 0;
            }

            score += 1000;

            // Get scores associated by their preferences
            switch(preferences.ClassType)
            {
                case Preferences.Day.Undefined:
                    break;
                case Preferences.Day.Morning: // (12am - 12pm)
                    if (0 < newTable.EarliestClassTime && newTable.LatestClassTime < 12)
                        score += 100;
                    break;
                case Preferences.Day.Afternoon: // (12pm - 5pm)
                    if (12 <= newTable.EarliestClassTime && newTable.LatestClassTime < 17)
                        score += 100;
                    break;
                case Preferences.Day.Evening: // (5pm - 8pm)
                    if (17 <= newTable.EarliestClassTime && newTable.LatestClassTime < 20)
                        score += 100;
                    break;
                case Preferences.Day.Night: // (9pm - 12pm)
                    if (20 <= newTable.EarliestClassTime && newTable.LatestClassTime <= 24)
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
                    score -= newTable.TotalTimeBetweenClasses;
                    break;
                case Preferences.Quantity.Maximum:
                    score += newTable.TotalTimeBetweenClasses;
                    break;
                default:
                    throw new Exception("Time between class is not handled before!");
            }
            return score;
        }

        public double[] GetMaxScoresPerGeneration()
        {
            fitnessScores = new double[populationSize];
            population.Clear();

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
            double[] maxScores = new double[numGenerations];
            for (int i = 0; i < numGenerations; i++)
            {
                EvolvePopulation();
                int bestTableIndex = GetBestTable(0, population.Count);
                double maxScore = fitnessScores[bestTableIndex];
                maxScores[i] = maxScore;
            }
            return maxScores;
        }

        public double[] GetAvgScoresPerGeneration()
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
            double[] avgScores = new double[numGenerations];
            for (int i = 0; i < numGenerations; i++)
            {
                EvolvePopulation();

                // Compute the fit scores
                double totalScore = 0;
                foreach (int[] table in population)
                    totalScore += GetFitnessScore(table);
                avgScores[i] = totalScore / population.Count;
            }
            return avgScores;
        }

        public List<YearlyTimetable> GetTimetables()
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
            List<YearlyTimetable> timetables = new List<YearlyTimetable>();
            for (int i = 0; i < population.Count; i++)
            {
                int[] table = population[i];
                if (fitnessScores[i] > 0)
                {
                    YearlyTimetable timetable = new YearlyTimetable();
                    for (int j = 0; j < table.Length; j++)
                    {
                        Section section = requiredSections[j][table[j]];
                        char term = terms[j];
                        timetable.AddSection(section, term);
                    }
                    timetables.Add(timetable);
                }
            }

            return timetables;
        }
    }
}
