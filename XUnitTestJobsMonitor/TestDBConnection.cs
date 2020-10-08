using System;
using Xunit;
using JobsMonitor.Data;
using JobsMonitor.Domain;
using System.Collections.Generic;

namespace XUnitTestJobsMonitor
{
    public class TestDBConnection
    {
        [Fact] //SET connection string in constructor
        public void TestDBConnectionConstructor()
        {
            GetJobsMonitor monitor = new GetJobsMonitor();
            var _config = monitor._config;
            string[] myInstances = _config.GetSection("Instances").Value.Split(",");

            foreach (var instance in myInstances)
            {
                DBConnection db = new DBConnection(
                                             instance,
                                              _config.GetSection("UserDB").Value,
                                              _config.GetSection("PasswordDB").Value, "msdb");

               string connectionStr = @"Data Source = " + instance + "; Initial Catalog=msdb; User ID=" + _config.GetSection("UserDB").Value + "; Password= " + _config.GetSection("PasswordDB").Value + ";";

                Assert.Equal(db.connectionStr, connectionStr);

            }

        }

        [Fact] //Open connection SQL
        public void TestDBConnectionDBOpen()
        {
            GetJobsMonitor monitor = new GetJobsMonitor();
            var _config = monitor._config;
            string[] myInstances = _config.GetSection("Instances").Value.Split(",");

            foreach (var instance in myInstances)
            {
                DBConnection db = new DBConnection(
                                             instance,
                                              _config.GetSection("UserDB").Value,
                                              _config.GetSection("PasswordDB").Value, "msdb");

                var cnn = db.DBOpen();
                Assert.True(cnn.State == System.Data.ConnectionState.Open);
                cnn.Close();

            }
        }

        [Fact]
        public void TestDBConnectionGetData()
        {
            GetJobsMonitor monitor = new GetJobsMonitor();
            List<JobsHistory> ListJobs = new List<JobsHistory>();
            var _config = monitor._config;
            string[] myInstances = _config.GetSection("Instances").Value.Split(",");
            int statusjob = Convert.ToInt32(_config.GetSection("EstadosJobs").Value);
            int days = Convert.ToInt32(_config.GetSection("days").Value);

            foreach (var instance in myInstances)
            {
        

                DBConnection db = new DBConnection(
                                             instance,
                                              _config.GetSection("UserDB").Value,
                                              _config.GetSection("PasswordDB").Value, "msdb");

                var cnn = db.DBOpen();
                var data = db.GetData(statusjob, cnn, days);

                //Add results to Object
                foreach (var item in data)
                {
                    ListJobs.Add(item);
                }
                cnn.Close();
            }

            Assert.True(ListJobs.Count > 0);


           
        }

        [Fact]
        public void TestDBConnextionPostData()
        {
            GetJobsMonitor monitor = new GetJobsMonitor();
            var _config = monitor._config;
            string Instance = _config.GetSection("Instance_Dest").Value;
            string USer = _config.GetSection("User_Dest").Value;
            string Password = _config.GetSection("Password_Dest").Value;
            string DB = _config.GetSection("DataBase_Dest").Value;
            string Table = _config.GetSection("Table_Dest").Value;

            DBConnection db = new DBConnection(Instance,USer,Password,DB);
           var cnn = db.DBOpen();
        var expected = new List<JobsHistory>(){
            new JobsHistory {
                Instancia = "InstanciaPrueba1",
                  Nombre_Job ="Prueba1",
                 Fecha_ejecucion = "2020-10-04 00:00:00.000",
                 Estado = "Failed"
                             },
            new JobsHistory {
               Instancia = "InstanciaPrueba2",
                  Nombre_Job ="Prueba2",
                 Fecha_ejecucion = "2020-10-04 00:00:00.000",
                 Estado = "Failed"
                            }
             };

            string resultado = db.PostData(expected, DB, Table, cnn);

            Assert.Contains("OK", resultado);
          
        }
    }
}
