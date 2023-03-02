using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.AutoPathFind
{
    internal class WalkingRobot
    {
        const double subsititution_mutation_pro = 0.06; //置换突变概率
        const double frameshift_mutation_pro = 0.06;    //移码突变概率
        const double deletion_mutation_pro = 0.05;      //缺失突变概率
        const double insertion_mutation_pro = 0.05;     //插入突变概率
        const int gene_size = 5;   //初代基因序列长度
        const int gene_limit = 30;  //最大基因序列长度

        List<int> gene; //基因
        int model_num; //型号（第几代robot）      
        double fitness; //适应度

        List<Tuple<int, int>> way = new List<Tuple<int, int>>(); //行走路线
        bool is_collide; //是否撞墙
        bool is_achieved; //是否到达目的地

        //创造初代robot
        public WalkingRobot() 
        {
            gene = new List<int>();
            this.model_num = 0;
            fitness = 0;
            is_collide = false;
            is_achieved = false;
            Random random = new Random();
            for(int i=0;i < gene_size; i++)
            {
                gene.Add(random.Next(4));
            }

        }

        //遗传
        public WalkingRobot(List<WalkingRobot> parents)
        {
            this.gene = new List<int>();
            Random random = new Random();
            int maxlength = parents[0].gene.Count;
            int minlength = parents[0].gene.Count;
            int max_model_num = parents[0].model_num;


            parents.ForEach(parent =>
            {
                if (parent.gene.Count > maxlength)
                {
                    maxlength = parent.gene.Count;
                }
                if (parent.gene.Count < minlength)
                {
                    minlength = parent.gene.Count;
                }

                if (parent.model_num > max_model_num)
                {
                    max_model_num = parent.model_num;
                }

            });

            int length = random.Next(minlength, maxlength + 1);
            int k = 0;
            while (k < length)
            {
                WalkingRobot target_parent = parents[random.Next(parents.Count)];
                if (target_parent.gene.Count > k)
                {
                    this.gene.Add(target_parent.gene[k]);
                    k++;
                }
            }

            this.model_num = max_model_num+1;
            fitness = 0;
            is_collide = false;
            is_achieved = false;

        }

        //开始行走测试
        public void StartWalking(Tuple<int, int> start_position, Tuple<int, int> end_position, int[,] map)
        {
            int step = 0;
            List<int> new_gene = new List<int>();
            int[] now_position = { start_position.Item1, start_position.Item2 };
            way.Add(new Tuple<int, int>(now_position[0], now_position[1]));
            int len0 = map.GetLength(0);
            int len1 = map.GetLength(1);
            for (int i = 0; i < gene.Count; i++)
            {
                step ++;
                if (step > gene_limit)
                {
                    break;
                }
                switch (gene[i])
                {
                    //向上移动
                    case 0:
                        now_position[0]--;
                        break;
                    //向右移动
                    case 1:
                        now_position[1]++;
                        break;
                    //向下移动
                    case 2:
                        now_position[0]++;
                        break;
                    //向左移动
                    case 3:
                        now_position[1]--;
                        break;
                }

                if (way.Exists(p => (p.Item1 == now_position[0] && p.Item2 == now_position[1]) ))
                {
                    break;
                }
                
                way.Add(new Tuple<int, int>(now_position[0], now_position[1]));

                if (now_position[0] < 0 || now_position[0] >= len0 || now_position[1] < 0 || now_position[1] >= len1)
                {
                    is_collide = true;
                    break;
                }
                else if (map[now_position[0], now_position[1]] == 1)
                {
                    is_collide = true;
                    break;
                }
                else if (now_position[0] == end_position.Item1 && now_position[1] == end_position.Item2)
                {
                    new_gene.Add(gene[i]);
                    is_achieved = true;
                    
                    break;
                }
                new_gene.Add(gene[i]);

            }
            this.gene = new_gene;
        }

        //计算适应度
        public void CalcFiteness(double base_fitness)
        {
            int step = gene.Count;
            base_fitness = base_fitness + step;

            if (is_collide)
            {
                base_fitness = base_fitness / 10;
            }
            if (is_achieved)
            {
                base_fitness = base_fitness * 100;
                base_fitness = base_fitness / step;
            }



            this.fitness = base_fitness;
        }


        //基因突变
        public void Mutation()
        {
            Random random = new Random();
            if (gene.Count != 0)
            {
                if (random.NextDouble() < subsititution_mutation_pro)
                {
                    SubsititutionMutation();
                }
                if (random.NextDouble() < frameshift_mutation_pro)
                {
                    FrameshiftMutation();
                }
                if (random.NextDouble() < deletion_mutation_pro)
                {
                    DeletionMutation();
                }
                if (random.NextDouble() < insertion_mutation_pro)
                {
                    InsertionMutation();
                }
            }

           
        }
        //置换突变
        private void SubsititutionMutation()
        {
            Random random = new Random();
            gene[random.Next(gene.Count)] = random.Next(4);

        }
        //移码突变
        private void FrameshiftMutation()
        {
            Random random = new Random();
            int k1 = random.Next(gene.Count);
            int k2 = random.Next(gene.Count);
            int gene_segment = gene[k1];
            gene.RemoveAt(k1);
            gene.Insert(k2, gene_segment);
        }
        //缺失突变
        private void DeletionMutation()
        {
            Random random = new Random();
            gene.RemoveAt(random.Next(gene.Count));
        }
        //插入突变
        private void InsertionMutation()
        {
            Random random = new Random();
            gene.Insert(random.Next(gene.Count), random.Next(4));
        }

        //获取适应度
        public double GetFitness()
        {
            return fitness;
        }

        //格式化输出Robot信息
        public void FormatRobot()
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("型号：第{0}代Robot", model_num);
            Console.Write("基因：[");
            gene.ForEach(x =>
            {
                Console.Write("{0}, ", x);
            });
            Console.WriteLine("]");
            Console.WriteLine("适应度：{0}", fitness);
            Console.WriteLine("是否撞墙：{0}", is_collide);
            Console.WriteLine("是否到达目的地：{0}", is_achieved);
            Console.WriteLine("行走路线：");
            way.ForEach(x =>
            {
                Console.Write("{0}->", x);
            });
            Console.WriteLine();
            Console.WriteLine("-------------------------------");
        }



    }
}
