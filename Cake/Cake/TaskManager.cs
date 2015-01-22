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
        private static Dictionary<string, Task> _tasks;

        internal static string TaskToRun { get; set; }

        static TaskManager()
        {
            _tasks = new Dictionary<string, Task>();
        }

        /// <summary>
        /// Registers <see cref="Task"/> by adding it to the <see cref="_tasks"/> dictionary.
        /// </summary>
        /// <param name="task">A <see cref="Task"/> to be added.</param>
        public static void RegisterTask(Task task)
        {
            _tasks.Add(task.Name, task);
            Logger.Log(LogLevel.Debug, String.Format("Task \"{0}\" registered.", task.Name));
        }

        /// <summary>
        /// Runs <see cref="RunTaskWithDependencies"/> recursive function 
        /// and resets <see cref="Task.Status"/> property of each <see cref="Task"/> from <see cref="_tasks"/> after it is finished.
        /// </summary>
        /// <param name="name">Name of a <see cref="Task"/> to be executed.</param>
        public static void SetDefault(string name)
        {
            if (!String.IsNullOrEmpty(TaskToRun)) name = TaskToRun;

            RunTaskWithDependencies(name);
            foreach (var task in _tasks)
            {
                task.Value.Status = TaskStatus.NotVisited;
            }
        }

        /// <summary>
        /// Runs <see cref="RunTaskWithDependencies"/> recursive function 
        /// and resets <see cref="Task.Status"/> property of each <see cref="Task"/> from <see cref="_tasks"/> after it is finished.
        /// </summary>
        /// <param name="task">A <see cref="Task"/> to be executed.</param>
        public static void SetDefault(Task task)
        {
            SetDefault(task.Name);
        }

        /// <summary>
        /// Clears TaskManager's task list.
        /// Used in unit testing.
        /// </summary>
        public static void ClearTasks()
        {
            _tasks = new Dictionary<string, Task>();
        }

        private static void RunTaskWithDependencies(string name)
        {
            Task task;
            try
            {
                task = _tasks[name];
            }
            catch (KeyNotFoundException e)
            {
                var taskException = new TaskException(String.Format("Could not find the definition of Task \"{0}\".", name), e.Source);
                Logger.LogException(LogLevel.Fatal, taskException, "A fatal error has occured.");
                throw taskException;
            }

            if (task.Status == TaskStatus.Done) return;
            if (task.Status == TaskStatus.Pending)
                throw new TaskException(String.Format("There is a circular dependency defined in the script. Task visited twice for dependency examination: {0}.", task.Name));

            task.Status = TaskStatus.Pending;
            foreach (var dependency in task.Dependencies)
            {
                RunTaskWithDependencies(dependency);
            }

            task.Execute();
            task.Status = TaskStatus.Done;
        }
    }
}