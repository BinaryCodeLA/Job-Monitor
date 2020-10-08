using System;
using JobsMonitor;
namespace JobsClient
{
    class Program
    {
        static void Main(string[] args)
        {
            JobMonitor monitor = new JobMonitor();
            string resultado = monitor.ExecJobsMonitor();

            Console.WriteLine("Resultado de Monitoreo de Jobs: " + resultado);
            Console.ReadLine();
        }
    }
}
