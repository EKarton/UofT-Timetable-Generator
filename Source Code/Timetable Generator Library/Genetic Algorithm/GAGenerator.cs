using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    public partial class GAGenerator : ITimetableGenerator
    {
        private List<TimetableSession[]> requiredSessions = new List<TimetableSession[]>();
        private static Random random = new Random();
        private const double MUTATION_RATE = 0.001;
        private const double CROSSOVER_RATE = 0.70;

        public GAGenerator(List<TimetableSession[]> requiredSessions)
        {
            this.requiredSessions = requiredSessions;
        }

        private int[] MakeCopyOfTable(int[] table)
        {
            int[] newTable = new int[table.Length];
            for (int i = 0; i < table.Length; i++)
                newTable[i] = table[i];
            return newTable;
        }

        private void EvolvePopulation(List<int[]> population)
        {
            int bestTable = GetBestTable(population);
            population[0] = population[bestTable];

            // Perform crossovers
            for (int i = 1; i < population.Count; i++)
            {
                int parent1Index = PerformTournamentSelection(population);
                int parent2Index = PerformTournamentSelection(population);

                int[] child = PerformOldCrossover(population[parent1Index], population[parent2Index]);
                if (child != null)
                    population[parent1Index] = child;
            }

            // Perform mutations
            for (int i = 1; i < population.Count; i++)
                PerformMutation(population[i]);
        }

        private Tuple<int[], int[]> PerformSinglePointCrossover(int[] parent1, int[] parent2)
        {
            if (random.Next(0, 101) > CROSSOVER_RATE)
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
            if (random.Next(0, 101) > CROSSOVER_RATE)
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
            if (random.Next(0, 101) > CROSSOVER_RATE)
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
                if (random.Next(0, 101) <= MUTATION_RATE)
                {
                    int randSession = random.Next(0, requiredSessions[i].Length);
                    table[i] = randSession;
                }
            }
        }

        private int PerformTournamentSelection(List<int[]> population)
        {
            int index1 = random.Next(0, population.Count);
            int index2 = random.Next(0, population.Count);
            List<int[]> subPopulation = new List<int[]>();

            if (index1 < index2)
            {
                for (int i = index1; i <= index2; i++)
                    subPopulation.Add(population[i]);
            }
            else
            {
                for (int i = index2; i <= index1; i++)
                    subPopulation.Add(population[i]);
            }

            return GetBestTable(subPopulation);
        }

        private int GetBestTable(List<int[]> population)
        {
            int bestTable = -1;
            int bestRank = -1;
            for (int i = 0; i < population.Count; i++)
            {
                int curRank = FitnessCalculator.GetFitnessValue(population[i], requiredSessions);
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

        public void PrintTable(int[] table)
        {
            for (int i = 0; i < table.Length; i++)
                Console.Write(table[i]);
            Console.Write(" | -> " + FitnessCalculator.GetFitnessValue(table, requiredSessions));
            Console.WriteLine("");
        }

        public List<Timetable> GetTimetables(string[] filters)
        {
            // Generate an initial population
            List<int[]> population = new List<int[]>();
            for (int i = 0; i < 20; i++)
                population.Add(GenerateRandomTable());

            for (int i = 0; i < 100; i++)
                EvolvePopulation(population);

            return new List<Timetable>();
        }
    }
}
