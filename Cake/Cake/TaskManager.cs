namespace Cake
{
    using System;
    using System.Collections.Generic;

    using Common;

    /// <summary>
    /// Registers tasks read from the script, figures out task execution order and executes them.
    /// </summary>
    public static class TaskManager
    {
        private static readonly Dictionary<string, Task> Tasks;

        /// <summary>
        /// Initialises the <see cref="TaskManager"/>.
        /// </summary>
        static TaskManager()
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
            Logger.Log(LogLevel.Debug, String.Format("Task \"{0}\" registered.", task.Name));
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
            Task task;
            try
            {
                task = Tasks[name];
            }
            catch (KeyNotFoundException e)
            {
                var taskException = new TaskException(String.Format("Could not find the definition of Task \"{0}\".", name), e.Source);
                Logger.LogException(LogLevel.Fatal, taskException, "A fatal error has occured.");
                throw taskException;
            }

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