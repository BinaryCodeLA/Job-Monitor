using System;
using System.IO;
using JobsMonitor.Data;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;


namespace JobsMonitor.Domain
{
  public class GetJobsMonitor
    {
       private List<JobsHistory> ListJobs { get; set; }
       public IConfiguration _config { get; set; }
        //constructor
        public GetJobsMonitor()
        {
            try
            {
                _config = new ConfigurationBuilder()
                  .AddJsonFile("configs.json", optional: true, reloadOnChange: true)
                  .Build();

             }
            catch(FileNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
         
        }

        //Data List All Instances
        public List<JobsHistory> GetDataJobs()
        {
            try
            {
                var Env = _config;
                string[] myInstances = Env.GetSection("Instances").Value.Split(",");
                string myUserDB = Env.GetSection("UserDB").Value;
                string myPasswDB = Env.GetSection("PasswordDB").Value;
                int JobStatus = Convert.ToInt32(Env.GetSection("EstadosJobs").Value);
                int days = Convert.ToInt32(Env.GetSection("days").Value);
                DBConnection db;
                ListJobs = new List<JobsHistory>();

                //Array instances
                foreach (var instance in myInstances)
                {
                    db = new DBConnection(instance, myUserDB, myPasswDB, "msdb");
                    var cnn = db.DBOpen();
                    var datos = db.GetData(JobStatus, cnn, days);
                    cnn.Close();

                    //Add results to Object
                    foreach (var item in datos)
                    {
                        ListJobs.Add(item);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
          

            return ListJobs;
        
        }

        //Insert List
        public string InsertDataJobs(List<JobsHistory> oJobs)
        {
            string message = "";
            try
            {
                var Env = _config;
                string myInstances = Env.GetSection("Instance_Dest").Value;
                string myUserDB = Env.GetSection("User_Dest").Value;
                string myPasswDB = Env.GetSection("Password_Dest").Value;
                string myDataBase = Env.GetSection("DataBase_Dest").Value;
                string myTable = Env.GetSection("Table_Dest").Value;


                DBConnection db = new DBConnection(myInstances, myUserDB, myPasswDB, myDataBase);
                var cnn = db.DBOpen();
                message = db.PostData(oJobs, myDataBase, myTable, cnn);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return message;  

        }

    }
}
