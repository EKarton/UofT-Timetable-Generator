using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    public partial class GAGenerator : ITimetableGenerator
    {
        private static Random random = new Random();
        private double mutationRate = 0.1;
        private double crossoverRate = 0.9;
        private int populationSize = 16;
        private int numGenerations = 100;
        private string crossoverType = "Old Crossover";

        private List<TimetableSession[]> requiredSessions = new List<TimetableSession[]>();
        private List<int[]> population = new List<int[]>();
        private int[] fitnessScores = null;

        public GAGenerator(List<TimetableSession[]> requiredSessions)
        {
            this.requiredSessions = requiredSessions;
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
            // Compute the fitness scores for each table
            for (int i = 0; i < population.Count; i++)
                fitnessScores[i] = FitnessCalculator.GetFitnessValue(population[i], requiredSessions);

            // Store the best one
            int bestTable = GetBestTable(0, population.Count);
            int[] temp = population[0];
            int tempScore = fitnessScores[0];
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

            int[] child = new int[requiredSessions.Count];

            int randIndex = random.Next(0, requiredSessions.Count);
            for (int i = 0; i < randIndex; i++)
                child[i] = parent1[i];

            for (int i = randIndex; i < requiredSessions.Count; i++)
                child[i] = parent2[i];

            return child;
        }

        private void PerformMutation(int[] table)
        {
            for (int i = 0; i < requiredSessions.Count; i++)
            {
                if (random.NextDouble() <= mutationRate)
                {
                    int randSession = random.Next(0, requiredSessions[i].Length);
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
            int bestRank = -1;
            for (int i = leftIndex; i < rightIndex ; i++)
            {
                int curRank = fitnessScores[i];
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
            int[] table = new int[requiredSessions.Count];
            for (int i = 0; i < table.Length; i++)
            {
                int randSession = random.Next(0, requiredSessions[i].Length);
                table[i] = randSession;
            }
            return table;
        }

        public int[] GetMaxScoresPerGeneration()
        {
            fitnessScores = new int[populationSize];
            population.Clear();

            // Generate an initial population randomly
            for (int i = 0; i < populationSize; i++)
            {
                int[] randomTable = GenerateRandomTable();
                population.Add(randomTable);
            }

            // Compute average fit scores per generation
            int[] maxScores = new int[numGenerations];
            for (int i = 0; i < numGenerations; i++)
            {
                EvolvePopulation();
                int bestTableIndex = GetBestTable(0, population.Count);
                int maxScore = fitnessScores[bestTableIndex];
                maxScores[i] = maxScore;
            }
            return maxScores;
        }

        public int[] GetAvgScoresPerGeneration()
        {
            fitnessScores = new int[populationSize];
            population = new List<int[]>();

            // Generate an initial population randomly
            for (int i = 0; i < populationSize; i++)
            {
                int[] randomTable = GenerateRandomTable();
                population.Add(randomTable);
            }

            // Compute average fit scores per generation
            int[] avgScores = new int[numGenerations];
            for (int i = 0; i < numGenerations; i++)
            {
                EvolvePopulation();

                // Compute the fit scores
                int totalScore = 0;
                foreach (int[] table in population)
                    totalScore += FitnessCalculator.GetFitnessValue(table, requiredSessions);
                avgScores[i] = totalScore / population.Count;
            }
            return avgScores;
        }

        public List<Timetable> GetTimetables(string[] filters)
        {
            fitnessScores = new int[populationSize];
            population = new List<int[]>();

            // Generate an initial population randomly
            for (int i = 0; i < populationSize; i++)
            {
                int[] randomTable = GenerateRandomTable();
                population.Add(randomTable);
            }

            // Evolve the generations
            for (int i = 0; i < numGenerations; i++)
                EvolvePopulation();

            return new List<Timetable>();
        }
    }
}
