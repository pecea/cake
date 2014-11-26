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
            var task = new Task("test task").DependsOn("non existing task");
            TaskManager.RunTask("test task");
        }

        [TestMethod]
        [ExpectedException(typeof(TaskException), "Running a non existing task was ordered.")]
        public void RunTaskShouldThrowWhenTaskIsNotFound()
        {
            TaskManager.RunTask("non existing task");
        }
    }
}
