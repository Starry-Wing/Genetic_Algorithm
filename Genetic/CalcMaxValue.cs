using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Genetic
{
    public class CalcMaxValue
    {
        int population_size;    //种群规模
        double heteromorphosis_probability;     //变异率
        int min;        //基因最大值
        int max;        //基因最小值
        int iter_num;   //迭代次数

        private List<Tuple<int, int, int>> population;  //种群

        public CalcMaxValue(int population_size, double heteromorphosis_probability, int min, int max, int iter_num)
        {
            this.population_size = population_size;
            this.heteromorphosis_probability = heteromorphosis_probability;
            this.min = min;
            this.max = max;
            this.iter_num = iter_num;
        }

        //适应度函数
        public double FitnessFun(int x1, int x2, int x3)
        {
            double fitnese = Math.Cos(x1) * Math.Sin(x1) * Math.Acos(x1) + x2 + x3;
            return fitnese;
        }

      
        //适应度计算
        private double CalcFiteness(Tuple<int, int, int> individuality)
        {
            double fitnese = FitnessFun(individuality.Item1, individuality.Item2, individuality.Item3);
            return fitnese;
        }

        //创建种群
        public void CreatePopulation()
        {
            List<Tuple<int, int, int>> population = new List<Tuple<int, int, int>>();
            Random random = new Random();
            for(int i=0; i<population_size; i++)
            {
                int x1 = random.Next(min, max);
                int x2 = random.Next(min, max);
                int x3 = random.Next(min, max);
                population.Add(new Tuple<int, int, int>(x1, x2, x3));
            }
            this.population = population;
        }

        //选择
        private void Select()
        {
            population.Sort((a, b) => CalcFiteness(b).CompareTo(CalcFiteness(a)));
            population.RemoveRange(population_size/2-1, population_size/2);
        }

        //交叉
        private void Crossover()
        {
            List<Tuple<int, int, int>> new_population = new List<Tuple<int, int, int>>();
            int k = 0;
            Random random = new Random();
            for (int i = 0; i < population_size/4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    int x1 = population[random.Next(k, k + 2)].Item1;
                    int x2 = population[random.Next(k, k + 2)].Item2;
                    int x3 = population[random.Next(k, k + 2)].Item3;
                    new_population.Add(new Tuple<int, int, int>(x1, x2, x3));
                }
            }
            this.population = new_population;
        }

        //变异
        private void Mutation()
        {
            Random random = new Random();
            for (int i = 0;i < population_size;i++)
            {
                if (random.NextDouble()<heteromorphosis_probability)
                {
                    int rd = random.Next(min, max);
                    Tuple<int, int, int> individuality = population[i];
                    switch (random.Next(3))
                    {
                        case 0:
                            population[i] = new Tuple<int, int, int>(rd, individuality.Item2, individuality.Item3);
                            break;
                        case 1:
                            population[i] = new Tuple<int, int, int>(individuality.Item1, rd, individuality.Item3);
                            break;
                        case 2:
                            population[i] = new Tuple<int, int, int>(individuality.Item1, individuality.Item2, rd);
                            break;
                    }
                }
            }
        }

        //进化
        public void evolution()
        {
            for(int i = 0; i < iter_num; i++)
            {
                Select();
                Crossover();
                Mutation();
            }
            
        }

        public List<Tuple<int, int, int>> GetPopulation()
        {
            return population;
        }

        public void FormatPopulation()
        {
            population.Sort((a, b) => CalcFiteness(b).CompareTo(CalcFiteness(a)));
            population.ForEach(individuality =>
            {
                Console.WriteLine("基因:{0}, 适应度:{1}", individuality, CalcFiteness(individuality));
            });
        }

    }
}
