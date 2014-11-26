﻿namespace Cake
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        /// Determines whether the task has executed already during <see cref="TaskFactory.RunTask"/> execution.
        /// After <see cref="TaskFactory.RunTask"/> is finished, it is set to false.
        /// </summary>
        internal bool Done { get; set; }

        /// <summary>
        /// Task constructor that is also registering newly created task to the <see cref="TaskFactory"/>.
        /// </summary>
        /// <param name="name"></param>
        public Task(string name)
        {
            this.name = name;
            dependencies = new List<string>();
            TaskFactory.RegisterTask(this);
        }

        /// <summary>
        /// Adds one or more Tasks that this task is dependent on.
        /// </summary>
        /// <param name="dependenciesToAdd">Names of depenedencies to be added to <see cref="Dependencies"/></param>
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
            Done = true;
        }
    }
}