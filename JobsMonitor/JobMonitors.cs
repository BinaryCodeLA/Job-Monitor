using System;
using System.Reflection.Metadata.Ecma335;
using JobsMonitor.Domain;
namespace JobsMonitor
{
    public class JobMonitor
    {



        public string ExecJobsMonitor()
        {
            string message;

            try
            {
                GetJobsMonitor monitor = new GetJobsMonitor();
                var ListJobs = monitor.GetDataJobs();

                if (ListJobs.Count > 0)
                {
                    message = monitor.InsertDataJobs(ListJobs);
                }
                else
                {
                    message = "OK: No se encontraron registros";
                }


            }
            catch (Exception ex)
            {
                message = ex.Message;
                return "ERROR: " + message;
            }

            return message;
        }


    }
}
