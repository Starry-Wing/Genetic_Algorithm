using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.AutoPathFind
{
    internal class AutoPathFind
    {
        const int population_size = 100;    //种群规模
        const double base_fitness = 100;    //基础适应度       
        const int iter_num = 200000;   //迭代次数(超过100万次可能需要几分钟)
        const int parent_num = 3;   //父母个数

        List<WalkingRobot> population = new List<WalkingRobot>();

        int[,] map = 
        {
            {0, 0, 0, 1, 0, 0 },
            {0, 0, 0, 1, 0, 0 },
            {1, 1, 0, 1, 0, 0 },
            {0, 0, 0, 0, 0 ,0 },
            {1, 1, 1, 1, 1, 0 },
            {0, 0, 0, 0, 0, 0 }
        }; //地图

        Tuple<int, int> start_position = Tuple.Create(0, 0);    //起点
        Tuple<int, int> end_position = Tuple.Create(5, 0);      //终点

        //创建种群
        public void CreatePopulation()
        {
            for (int i = 0; i < population_size; i++)
            {
                population.Add(new WalkingRobot());
            }
        }

        //进化
        public void Evolution()
        {
            for (int i = 0; i < iter_num; i++)
            {
                WalkingTest();
                Iteration();
            }
            WalkingTest();
        }

        //格式化输出种群
        public void FormatPopulation()
        {
            population.ForEach(robot =>
            {
                robot.FormatRobot();
            });
        }

        //选择
        public void WalkingTest()
        {
            population.ForEach(robot =>
            {
                //寻路功能测试
                robot.StartWalking(start_position, end_position, map);
                //计算适应度
                robot.CalcFiteness(base_fitness);
            });
            //根据适应度排序
            population.Sort((a, b) => b.GetFitness().CompareTo(a.GetFitness()));
        }

        //迭代
        public void Iteration()
        {
            Random random = new Random();
            List<WalkingRobot> new_population = new List<WalkingRobot>();
            for(int i = 0; i < population_size; i++)
            {
                WalkingRobot robot;

                //这里的population是已经经过选择并排序的，population[0]则为适应度最高的个体，基因直接复制给下一代
                if (i == 0)
                {
                    robot = new WalkingRobot(new List<WalkingRobot> { population[0] });
                    
                }
                //对父母(均为适应度排列前半的个体，个数自定义)基因进行基因重组，产生后代
                else
                {
                    List<WalkingRobot> parents = new List<WalkingRobot>();
                    for(int j=0; j<parent_num; j++)
                    {
                        parents.Add(population[random.Next(population_size / 2)]);
                    }
                    robot = new WalkingRobot(parents);
                }

                //变异
                robot.Mutation();

                new_population.Add(robot);

            }

            this.population = new_population;

        }



    }
}
