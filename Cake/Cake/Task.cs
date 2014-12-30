namespace Cake
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;

    /// <summary>
    /// Represents a task to be executed at some point.
    /// </summary>
    public class Task
    {
        private readonly string name;
        private readonly List<string> dependencies;
        private Action action;

        /// <summary>
        /// Task's name.
        /// </summary>
        internal string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Names of the tasks this task is dependent on.
        /// </summary>
        internal List<string> Dependencies
        {
            get { return dependencies; }
        }

        /// <summary>
        /// Determines whether the task has executed already or is executed during <see cref="TaskManager.SetDefault(string)"/> 
        /// or <see cref="TaskManager.SetDefault(Cake.Task)"/> execution.
        /// After TaskManager.SetDefault is finished, it is set to Done.
        /// </summary>
        internal TaskStatus Status { get; set; }

        /// <summary>
        /// Task constructor that is also registering newly created task to the <see cref="TaskManager"/>.
        /// </summary>
        /// <param name="name"></param>
        public Task(string name)
        {
            this.name = name;
            Status = TaskStatus.NotVisited;
            dependencies = new List<string>();
            action = () => { };
            TaskManager.RegisterTask(this);
        }

        /// <summary>
        /// Adds one or more Tasks that this task is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Names of depenedencies to be added to <see cref="Dependencies"/>.</param>
        /// <returns>The Task object is returned so that method chaining can be used in the script.</returns>
        public Task DependsOn(params string[] dependenciesToAdd)
        {
            foreach (var dependency in dependenciesToAdd.Where(dependency => dependencies.All(added => added != dependency)))
            {
                dependencies.Add(dependency);
            }
            return this;
        }

        /// <summary>
        /// Adds one or more Tasks that this task is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Tasks that this task will be dependent on.</param>
        /// <returns>The Task object is returned so that method chaining can be used in the script.</returns>
        public Task DependsOn(params Task[] dependenciesToAdd)
        {
            return DependsOn(dependenciesToAdd.Select(dependency => dependency.Name).ToArray());
        }

        /// <summary>
        /// Defines an <see cref="Action"/> that can be performed by this task.
        /// </summary>
        /// <param name="actionToDo">Action delegate to be passed to this task.</param>
        /// <returns>The Task object is returned so that method chaining can be used in the script.</returns>
        public Task Does(Action actionToDo)
        {
            action = actionToDo;
            return this;
        }

        /// <summary>
        /// Invokes Task's action.
        /// </summary>
        internal void Execute()
        {
            action();
            Logger.Log(LogLevel.Debug, String.Format("Task \"{0}\" executed.", Name));
        }
    }
}