using System;

namespace Cake.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class TaskManagerTests
    {
        [TestInitialize]
        public void ClearTaskManagersTasks()
        {
            TaskManager.ClearTasks();
        }

        [TestMethod]
        [ExpectedException(typeof(TaskException), "A dependency on a non existing task was specified.")]
        public void RunTaskWithDependenciesShouldThrowWhenDependencyIsNotFound()
        {
            new Task("test task").DependsOn("non existing task");
            TaskManager.SetDefault("test task");
        }


        [TestMethod]
        [ExpectedException(typeof(TaskException), "Running a non existing task was ordered.")]
        public void SetDefaultShouldThrowWhenTaskIsNotFound()
        {
            TaskManager.SetDefault("non existing task");
        }

        [TestMethod]
        [ExpectedException(typeof(TaskException), "Running a task with a circular dependency was ordered.")]
        public void SetDefaultShouldThrowWhenTheresDependencyCycle()
        {
            new Task("first").DependsOn("second");
            new Task("second").DependsOn("first");

            TaskManager.SetDefault("first");
        }

        [TestMethod]
        [ExpectedException(typeof (TaskException))]
        public void SetDefaultShouldThrowWhenTaskIsSelfDependent()
        {
            TaskManager.SetDefault(new Task("first").DependsOn("first"));
        }

        [TestMethod]
        public void TasksShouldBeExecutedInRightOrderTriangle()
        {
            var counter = 0;

            var third = new Task("third")
                .DependsOn("second", "first")
                .Does(() =>
                {
                    Assert.AreEqual(3, ++counter);
                });

            var second = new Task("second")
                .DependsOn("first")
                .Does(() =>
                {
                    Assert.AreEqual(2, ++counter);
                });

            var first = new Task("first")
                .Does(() =>
                {
                    Assert.AreEqual(1, ++counter);
                });

            TaskManager.SetDefault(third);
        }

        [TestMethod]
        public void TasksShouldBeExecutedInRightOrderDiamond()
        {
            var counter = 0;
            Action counterAdd = () => counter++;

            var top = new Task("top")
                .DependsOn("middle 1", "middle 2", "middle 3", "middle 4")
                .Does(() =>
                {
                    Assert.AreEqual(6, ++counter);
                });

            new Task("middle 1")
                .DependsOn("middle 2", "bottom")
                .Does(counterAdd);            
            
            new Task("middle 2")
                .DependsOn("middle 3", "bottom")
                .Does(counterAdd);            
            
            new Task("middle 3")
                .DependsOn("middle 4", "bottom")
                .Does(counterAdd);            
            
            new Task("middle 4")
                .DependsOn("bottom")
                .Does(counterAdd);

            var bottom = new Task("bottom")
                .Does(() =>
                {
                    Assert.AreEqual(1, ++counter);
                });

            TaskManager.SetDefault(top);
        }
    }
}
