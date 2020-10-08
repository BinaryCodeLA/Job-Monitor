using System;
using Xunit;
using JobsMonitor;
namespace XUnitTestJobsMonitor
{
    public class TestJobMonitor
    {
        [Fact] //End to End
        public void TestJobMonitorConstructor()            
        {
            JobMonitor mon = new JobMonitor();
            string resultado = mon.ExecJobsMonitor();
            Assert.Contains("OK", resultado);
        }
    }
}
