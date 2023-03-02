using Genetic;
using Genetic.AutoPathFind;

internal class Program
{
    private static void Main(string[] args)
    {
        WalkingRobotTest();
    }

    static void WalkingRobotTest()
    {
        AutoPathFind autoPathFind = new AutoPathFind();
        autoPathFind.CreatePopulation();
        autoPathFind.Evolution();
        autoPathFind.FormatPopulation();
    }



}