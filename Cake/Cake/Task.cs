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
        private readonly string _name;
        private readonly List<string> _dependencies;
        private Action _action;

        internal string Name
        {
            get { return _name; }
        }


        internal List<string> Dependencies
        {
            get { return _dependencies; }
        }


        internal TaskStatus Status { get; set; }

        /// <summary>
        /// Task constructor that is also registering newly created task to the <see cref="TaskManager"/>.
        /// </summary>
        /// <param name="name"></param>
        public Task(string name)
        {
            this._name = name;
            Status = TaskStatus.NotVisited;
            _dependencies = new List<string>();
            _action = () => { };
            TaskManager.RegisterTask(this);
        }

        /// <summary>
        /// Adds one or more Tasks that this task is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Names of depenedencies to be added to <see cref="Dependencies"/>.</param>
        /// <returns>The Task object is returned so that method chaining can be used in the script.</returns>
        public Task DependsOn(params string[] dependenciesToAdd)
        {
            foreach (var dependency in dependenciesToAdd.Where(dependency => _dependencies.All(added => added != dependency)))
            {
                _dependencies.Add(dependency);
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
            _action = actionToDo;
            return this;
        }

        internal void Execute()
        {
            _action();
            Logger.Log(LogLevel.Debug, String.Format("Task \"{0}\" executed.", Name));
        }
    }
}