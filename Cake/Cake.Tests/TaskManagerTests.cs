namespace Cake.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains tests concerning Task class
    /// </summary>
    [TestClass]
    public class TaskManagerTests
    {
        /// <summary>
        /// Tests whether an exception is thrown when a task is dependent on a non existing one.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TaskException), "A dependency on a non existing task was specified.")]
        public void RunTaskWithDependenciesShouldThrowWhenDependencyIsNotFound()
        {
            new Task("test task").DependsOn("non existing task");
            TaskManager.SetDefault("test task");
        }

        /// <summary>
        /// Tests whether an exception is thrown when a non existing task is run.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TaskException), "Running a non existing task was ordered.")]
        public void SetDefaultShouldThrowWhenTaskIsNotFound()
        {
            TaskManager.SetDefault("non existing task");
        }

        /// <summary>
        /// Tests whether an exception is thrown when a circular dependency is defined.
        /// </summary>
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
