//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Cake.Tests
//{
//    /// <summary>
//    /// Test class for JobManager methods
//    /// </summary>
//    [TestClass]
//    public class JobManagerTests
//    {
//        /// <summary>
//        /// Initialize method to clean up jobs
//        /// </summary>
//        [TestInitialize]
//        public void ClearTaskManagersTasks()
//        {
//            JobManager.ClearJobs();
//        }
//        /// <summary>
//        /// Test method for non-existing dependency
//        /// </summary>
//        [TestMethod]
//        [TestCategory("CakeMethods")]
//        [TestCategory("JobManagerMethods")]
//        [ExpectedException(typeof(JobException), "A dependency on a non existing task was specified.")]
//        public void RunTaskWithDependenciesResultShouldThrowWhenDependencyIsNotFound()
//        {
//            new Job("test task").DependsOn("non existing task");
//            JobManager.SetDefault("test task");
//        }

//        /// <summary>
//        /// Test method for non-existing task
//        /// </summary>
//        [TestMethod]
//        [TestCategory("CakeMethods")]
//        [TestCategory("JobManagerMethods")]
//        [ExpectedException(typeof(JobException), "Running a non existing task was ordered.")]
//        public void SetDefaultShouldThrowWhenTaskIsNotFound()
//        {
//            JobManager.SetDefault("non existing task");
//        }
//        /// <summary>
//        /// Test method for circular dependency
//        /// </summary>
//        [TestMethod]
//        [TestCategory("CakeMethods")]
//        [TestCategory("JobManagerMethods")]
//        [ExpectedException(typeof(JobException), "Running a task with a circular dependency was ordered.")]
//        public void SetDefaultShouldThrowWhenTheresDependencyCycle()
//        {
//            new Job("first").DependsOn("second");
//            new Job("second").DependsOn("first");

//            JobManager.SetDefault("first");
//        }
//        /// <summary>
//        /// Test method for self-dependency
//        /// </summary>
//        [TestMethod]
//        [TestCategory("CakeMethods")]
//        [TestCategory("JobManagerMethods")]
//        [ExpectedException(typeof (JobException))]
//        public void SetDefaultShouldThrowWhenTaskIsSelfDependent()
//        {
//            JobManager.SetDefault(new VoidJob("first").DependsOn("first"));
//        }
//        /// <summary>
//        /// Test method for triangle dependency
//        /// </summary>
//        [TestMethod]
//        [TestCategory("CakeMethods")]
//        [TestCategory("JobManagerMethods")]
//        public void TasksShouldBeExecutedInRightOrderTriangle()
//        {
//            var counter = 0;

//            var third = new VoidJob("third")
//                .DependsOn("second", "first")
//                .Does(() =>
//                {
//                    Assert.AreEqual(3, ++counter);
//                });

//            new VoidJob("second")
//                .DependsOn("first")
//                .Does(() =>
//                {
//                    Assert.AreEqual(2, ++counter);
//                });

//            new VoidJob("first")
//                .Does(() =>
//                {
//                    Assert.AreEqual(1, ++counter);
//                });

//            JobManager.SetDefault(third);
//        }
//        /// <summary>
//        /// Test method for diamond dependency
//        /// </summary>
//        [TestMethod]
//        [TestCategory("CakeMethods")]
//        [TestCategory("JobManagerMethods")]
//        public void TasksShouldBeExecutedInRightOrderDiamond()
//        {
//            var counter = 0;
//            Action counterAdd;
//            counterAdd = () => counter++;

//            var top = new VoidJob("top")
//                .DependsOn("middle 1", "middle 2", "middle 3", "middle 4")
//                .Does(() =>
//                {
//                    Assert.AreEqual(6, ++counter);
//                });

//            new VoidJob("middle 1")
//                .DependsOn("middle 2", "bottom")
//                .Does(counterAdd);            
            
//            new VoidJob("middle 2")
//                .DependsOn("middle 3", "bottom")
//                .Does(counterAdd);            
            
//            new VoidJob("middle 3")
//                .DependsOn("middle 4", "bottom")
//                .Does(counterAdd);            
            
//            new VoidJob("middle 4")
//                .DependsOn("bottom")
//                .Does(counterAdd);

//            new VoidJob("bottom")
//                .Does(() =>
//                {
//                    Assert.AreEqual(1, ++counter);
//                });

//            JobManager.SetDefault(top);
//        }
//    }
//}
