using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace JobsMonitor.Data
{
    public class DBConnection
    {
       public SqlConnection dbconnection { get; set; }
       public string connectionStr { get; set; }

        //New Connection DB
        public DBConnection(string Instance, string UserId, string Password, string DB)
        {
         connectionStr = @"Data Source = " + Instance + "; Initial Catalog=" + DB + "; User ID=" + UserId + "; Password= " + Password + ";";           
        }
       
        //Open connection
        public SqlConnection DBOpen()
        {
            try
                {
                    dbconnection = new SqlConnection(connectionStr);
                    dbconnection.Open();
                    return this.dbconnection;
                }
            catch (SqlException ex)
                {

                    throw ex;
                }
            catch (TimeoutException ex)
                {

                    throw ex;
                }
            catch (Exception ex)
                {

                    throw ex;
                }
           
        }

        //Query SQL GetData
        public IEnumerable<JobsHistory> GetData(int JobsStatus, SqlConnection cnn, int days)
        {
            try
                {
         
                string sql = " SELECT server AS Instancia, ";
                sql += " (SELECT name from msdb.dbo.sysjobs WHERE job_id = jh.job_id)AS Nombre_Job, ";
                sql += " FailureDate AS Fecha_ejecucion, ";
                sql += "  CASE WHEN run_status = 0 THEN 'Failed' ";
                sql += "       WHEN run_status = 1 THEN 'Succeeded' ";
                sql += "       WHEN run_status = 2 THEN 'Retry' ";
                sql += "       WHEN run_status = 3 THEN 'Cancelled' ";
                sql += " ELSE 'Unknown' END AS Estado ";
                sql += " FROM(SELECT server, job_id,run_status, ";                
                sql += "(DATEADD(ss, CAST(SUBSTRING(CAST(jh.run_duration ";
                sql += "	   + 1000000 AS char(7)), 6, 2) AS int), ";
                sql += "	 DATEADD(mi, CAST(SUBSTRING(CAST(jh.run_duration ";
                sql += "	   + 1000000 AS char(7)), 4, 2) AS int), ";
                sql += "	 DATEADD(hh, CAST(SUBSTRING(CAST(jh.run_duration ";
                sql += "	   + 1000000 AS char(7)), 2, 2) AS int), ";
                sql += "	 DATEADD(ss, CAST(SUBSTRING(CAST(jh.run_time ";
                sql += "	   + 1000000 AS char(7)), 6, 2) AS int), ";
                sql += "	 DATEADD(mi, CAST(SUBSTRING(CAST(jh.run_time ";
                sql += "	   + 1000000 AS char(7)), 4, 2) AS int), ";
                sql += "	 DATEADD(hh, CAST(SUBSTRING(CAST(jh.run_time ";
                sql += "	   + 1000000 AS char(7)), 2, 2) AS int), ";
                sql += "			  CONVERT(datetime, CAST(jh.run_date AS char(8)))))))))) ";
                sql += "			AS FailureDate ";
                sql += " FROM  msdb.dbo.sysjobhistory AS   jh) AS jh ";
                sql += " WHERE(GETDATE() > jh.FailureDate) ";
                sql += " AND(jh.run_status = @ids ) ";
                //--Identify how many days to go back and look for   failures
                sql += " AND(DATEADD(dd, @day, GETDATE()) < jh.FailureDate) ";


                 var datos = cnn.Query<JobsHistory>(sql, new { ids = JobsStatus, day = days });
                    return datos;
                }
            catch (SqlException ex)
                {

                    throw ex;
                }
            catch (TimeoutException ex)
                {

                    throw ex;
                }
            catch (Exception ex)
                {

                    throw ex;
                }

        }

        //Query SQL InsertData
        public string PostData(List<JobsHistory> joblogs, string DataBase, string Table, SqlConnection cnn)
        {
            string message="";
            
            try
                {

                    string sql = @" INSERT INTO "+ DataBase +".."+ Table +" (Instancia, Nombre_Job, Fecha_ejecucion, Estado) ";
                           sql += " VALUES ( @Instancia, @Nombre_Job, @Fecha_ejecucion, @Estado ) ";
               
                var trans = cnn.BeginTransaction();     

                    int rowsAffected = cnn.Execute(sql, joblogs, transaction: trans);

                trans.Commit();    

                   if (rowsAffected > 0)
                        {
                            message = "OK: Se Guardaron " + rowsAffected.ToString() + " nuevos datos";
                        }
                     else
                        {
                            message = "ERROR: Problemas al guardar los registros";
                        }
                }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (TimeoutException ex)
            {

                throw ex;
            }
            catch (Exception ex)
                {

                throw ex;
                }


            return message;
        }
    }
}
