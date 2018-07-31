using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;

namespace Genetic8QueensSolutionWithGeneticSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            int vezirSayisi = 8;

            var selection = new EliteSelection();
            var crossover = new TwoPointCrossover();
            var mutation = new ReverseSequenceMutation();
            var fitness = new MyProblemFitness();
            var chromosome = new MyProblemChromosome(vezirSayisi);
            var population = new Population(5000, 5500, chromosome);

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation)
            {
                Termination = new FitnessThresholdTermination(vezirSayisi * (vezirSayisi - 1) / 2)
            };

            int index = 0;

            ga.GenerationRan += delegate
            {
                var bestChromosome = ga.Population.BestChromosome;

                Console.Write("Index: " + index);
                Console.Write(", Fitness: {0}", bestChromosome.Fitness);

                Console.Write(", Genes: {0}", string.Join("-", bestChromosome.GetGenes()));

                Console.WriteLine();

                index++;
            };

            Console.WriteLine("GA running...");

            ga.Start();

            Console.WriteLine("Best solution found has {0} fitness.", ga.BestChromosome.Fitness);

            Console.Read();
        }
    }

    public class MyProblemFitness : IFitness
    {
        public double Evaluate(IChromosome chromosome)
        {
            var genes = chromosome.GetGenes();

            double result = 0;

            for (int x1 = 0; x1 < genes.Length - 1; x1++)
            {
                int y1 = (int)genes[x1].Value;

                int sagdakiVezirSayisi = genes.Length - 1 - x1;

                for (int x2 = x1 + 1; x2 < genes.Length; x2++)
                {
                    int y2 = (int)genes[x2].Value;

                    if (y1 == y2 || x1 - y1 == x2 - y2 || x1 + y1 == x2 + y2)
                    {
                        sagdakiVezirSayisi -= 1;
                    }
                }

                result += sagdakiVezirSayisi;
            }


            return result;
        }
    }

    public class MyProblemChromosome : ChromosomeBase
    {
        public MyProblemChromosome(int length) : base(length)
        {
            CreateGenes();
        }

        public override Gene GenerateGene(int geneIndex)
        {
            var rnd = RandomizationProvider.Current;

            return new Gene(rnd.GetInt(0, Length));
        }

        public override IChromosome CreateNew()
        {
            return new MyProblemChromosome(Length);
        }
    }
}
