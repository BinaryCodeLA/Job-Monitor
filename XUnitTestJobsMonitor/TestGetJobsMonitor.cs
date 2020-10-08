using System;
using Xunit;
using System.Collections.Generic;
using System.Text;
using JobsMonitor.Domain;
using Microsoft.Extensions.Configuration;
using JobsMonitor.Data;

namespace XUnitTestJobsMonitor
{
   public class TestGetJobsMonitor
    {
        [Fact]
        public void TestConstructor()
        {
            GetJobsMonitor monitor = new GetJobsMonitor();
            var _Dataconfig = monitor._config;

            var _config = new ConfigurationBuilder()
                 .AddJsonFile("configs.json", optional: true, reloadOnChange: true)
                 .Build();

            Assert.Equal(_Dataconfig.GetSection("Instances").Value , _config.GetSection("Instances").Value);
        }


        [Fact]
        public void TestGetDataJobs()
        {
            GetJobsMonitor monitor = new GetJobsMonitor();
            var datos = monitor.GetDataJobs();
            Assert.True(datos.Count > 0);
        }

        [Fact]
        public void TestInsertDataJobs()
        {
            var ListJobs = new List<JobsHistory>(){
            new JobsHistory {
                Instancia = "InstanciaPrueba3",
                  Nombre_Job ="Prueba3",
                 Fecha_ejecucion = "2020-10-04 00:00:00.000",
                 Estado = "Failed"
                             },
            new JobsHistory {
               Instancia = "InstanciaPrueba4",
                  Nombre_Job ="Prueba4",
                 Fecha_ejecucion = "2020-10-04 00:00:00.000",
                 Estado = "Failed"
                            }
             };

            GetJobsMonitor monitor = new GetJobsMonitor();
            string resultado = monitor.InsertDataJobs(ListJobs);
            Assert.Contains("OK", resultado);

        }


    }
}
