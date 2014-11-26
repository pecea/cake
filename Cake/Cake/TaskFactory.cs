namespace Cake
{
    using System.Collections.Generic;

    /// <summary>
    /// Registers tasks read from the script, figures out task execution order and executes them.
    /// </summary>
    public static class TaskFactory
    {
        private static readonly Dictionary<string, Task> Tasks;

        /// <summary>
        /// Initialises the <see cref="TaskFactory"/>.
        /// </summary>
        static TaskFactory()
        {
            Tasks = new Dictionary<string, Task>();
        }

        /// <summary>
        /// Registers <see cref="Task"/> by adding it to the <see cref="Tasks"/> dictionary.
        /// </summary>
        /// <param name="task">A <see cref="Task"/> to be added.</param>
        public static void RegisterTask(Task task)
        {
            Tasks.Add(task.Name, task);
        }

        /// <summary>
        /// Runs <see cref="RunTaskWithDependencies"/> recursive function and resets <see cref="Task.Done"/> property of each <see cref="Task"/> from <see cref="Tasks"/> after it is finished.
        /// </summary>
        /// <param name="name">A <see cref="Task"/> to be executed.</param>
        public static void RunTask(string name)
        {
            RunTaskWithDependencies(name);
            foreach (var task in Tasks)
            {
                task.Value.Done = false;
            }
        }

        /// <summary>
        /// Executes <see cref="Task"/>'s action executing its dependencies' actions first.
        /// A dependency is executed only once, even if occurs on the created stack more than once.
        /// </summary>
        /// <param name="name">A <see cref="Task"/> to be executed.</param>
        private static void RunTaskWithDependencies(string name)
        {
            var task = Tasks[name];
            if (task.Done) return;

            foreach (var dependency in task.Dependencies)
            {
                RunTaskWithDependencies(dependency);
            }

            task.Execute();
            task.Done = true;
        }
    }
}