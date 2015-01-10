namespace Cake.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class TaskManagerTests
    {

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
    }
}
