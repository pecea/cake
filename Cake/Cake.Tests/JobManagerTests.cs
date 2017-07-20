using System;

namespace Cake.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class JobManagerTests
    {
        [TestInitialize]
        public void ClearTaskManagersTasks()
        {
            JobManager.ClearJobs();
        }


        [TestMethod]
        [TestCategory("CakeMethods")]
        [TestCategory("JobManagerMethods")]
        public void InnerExceptionDebugging()
        {
            new Job("t2").Does(() => { System.Console.WriteLine("test"); });

            new Job("t1").DependsOn("t2").Does(() => {
                var i = 1;
                var j = 2;
                System.Console.WriteLine((i + j).ToString());
            });

            JobManager.SetDefault("t1");
        }

        [TestMethod]
        [TestCategory("CakeMethods")]
        [TestCategory("JobManagerMethods")]
        [ExpectedException(typeof(JobException), "A dependency on a non existing task was specified.")]
        public void RunTaskWithDependenciesResultShouldThrowWhenDependencyIsNotFound()
        {
            new Job("test task").DependsOn("non existing task");
            JobManager.SetDefault("test task");
        }


        [TestMethod]
        [TestCategory("CakeMethods")]
        [TestCategory("JobManagerMethods")]
        [ExpectedException(typeof(JobException), "Running a non existing task was ordered.")]
        public void SetDefaultShouldThrowWhenTaskIsNotFound()
        {
            JobManager.SetDefault("non existing task");
        }

        [TestMethod]
        [TestCategory("CakeMethods")]
        [TestCategory("JobManagerMethods")]
        [ExpectedException(typeof(JobException), "Running a task with a circular dependency was ordered.")]
        public void SetDefaultShouldThrowWhenTheresDependencyCycle()
        {
            new Job("first").DependsOn("second");
            new Job("second").DependsOn("first");

            JobManager.SetDefault("first");
        }

        [TestMethod]
        [TestCategory("CakeMethods")]
        [TestCategory("JobManagerMethods")]
        [ExpectedException(typeof (JobException))]
        public void SetDefaultShouldThrowWhenTaskIsSelfDependent()
        {
            JobManager.SetDefault(new Job("first").DependsOn("first"));
        }

        [TestMethod]
        [TestCategory("CakeMethods")]
        [TestCategory("JobManagerMethods")]
        public void TasksShouldBeExecutedInRightOrderTriangle()
        {
            var counter = 0;

            var third = new Job("third")
                .DependsOn("second", "first")
                .Does(() =>
                {
                    Assert.AreEqual(3, ++counter);
                });

            var second = new Job("second")
                .DependsOn("first")
                .Does(() =>
                {
                    Assert.AreEqual(2, ++counter);
                });

            var first = new Job("first")
                .Does(() =>
                {
                    Assert.AreEqual(1, ++counter);
                });

            JobManager.SetDefault(third);
        }

        [TestMethod]
        [TestCategory("CakeMethods")]
        [TestCategory("JobManagerMethods")]
        public void TasksShouldBeExecutedInRightOrderDiamond()
        {
            var counter = 0;
            Action counterAdd = () => counter++;

            var top = new Job("top")
                .DependsOn("middle 1", "middle 2", "middle 3", "middle 4")
                .Does(() =>
                {
                    Assert.AreEqual(6, ++counter);
                });

            new Job("middle 1")
                .DependsOn("middle 2", "bottom")
                .Does(counterAdd);            
            
            new Job("middle 2")
                .DependsOn("middle 3", "bottom")
                .Does(counterAdd);            
            
            new Job("middle 3")
                .DependsOn("middle 4", "bottom")
                .Does(counterAdd);            
            
            new Job("middle 4")
                .DependsOn("bottom")
                .Does(counterAdd);

            var bottom = new Job("bottom")
                .Does(() =>
                {
                    Assert.AreEqual(1, ++counter);
                });

            JobManager.SetDefault(top);
        }
    }
}
