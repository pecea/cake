namespace Cake.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    /// <summary>
    /// Class for testing Task Manager methods
    /// </summary>
    [TestClass]
    public class TaskManagerTests
    {
        /// <summary>
        /// Test method for throwing an exception when dependency is not found
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TaskException), "A dependency on a non existing task was specified.")]
        public void RunTaskWithDependenciesShouldThrowWhenDependencyIsNotFound()
        {
            new Task("test task").DependsOn("non existing task");
            TaskManager.RunTask("test task");
        }
        /// <summary>
        /// Test method for throwing an exception when task is not found
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TaskException), "Running a non existing task was ordered.")]
        public void RunTaskShouldThrowWhenTaskIsNotFound()
        {
            TaskManager.RunTask("non existing task");
        }
    }
}
