using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler;

namespace SimplifiedTaskScheduler
{
    public static class SimpleTask
    {
        public enum TASK_CREATION : int
        {
            /// <summary>
            /// The Task Scheduler service checks the syntax of the XML that describes the task 
            /// but does not register the task. This constant cannot be combined with the 
            /// TASK_CREATE, TASK_UPDATE, or TASK_CREATE_OR_UPDATE values.
            /// </summary>
            TASK_VALIDATE_ONLY = 0x1,
            /// <summary>
            /// The Task Scheduler service registers the task as a new task.
            /// </summary>
            TASK_CREATE = 0x2,
            /// <summary>
            /// The Task Scheduler service registers the task as an updated version of an 
            /// existing task. When a task with a registration trigger is updated, the 
            /// task will execute after the update occurs.
            /// </summary>
            TASK_UPDATE = 0x4,
            /// <summary>
            /// Recommended by default!
            /// The Task Scheduler service either registers the task as a new task or as an 
            /// updated version if the task already exists. 
            /// Equivalent to TASK_CREATE | TASK_UPDATE.
            /// </summary>
            TASK_CREATE_OR_UPDATE = 0x6,
            /// <summary>
            /// The Task Scheduler service registers the disabled task. 
            /// A disabled task cannot run until it is enabled. 
            /// For more information, see Enabled Property of ITaskSettings and 
            /// Enabled Property of IRegisteredTask.
            /// </summary>
            TASK_DISABLE = 0x8,
            /// <summary>
            /// The Task Scheduler service is prevented from adding the allow access-control 
            /// entry (ACE) for the context principal. 
            /// When the ITaskFolder::RegisterTaskDefinition or ITaskFolder::RegisterTask 
            /// functions are called with this flag to update a task, the Task Scheduler 
            /// service does not add the ACE for the new context principal and does not 
            /// remove the ACE from the old context principal.
            /// </summary>
            TASK_DONT_ADD_PRINCIPAL_ACE = 0x10,
            /// <summary>
            /// The Task Scheduler service creates the task, but ignores the registration 
            /// triggers in the task. By ignoring the registration triggers, the task 
            /// will not execute when it is registered unless a time-based trigger 
            /// causes it to execute on registration.
            /// </summary>
            TASK_IGNORE_REGISTRATION_TRIGGERS = 0x20
        }

        /// <summary>
        /// This function will define a new Task inside TaskScheduler application.<br></br>
        /// We can run any program how ever we want to, for instance after every other day, 
        /// week, month or every startup.<br></br>
        /// Best part? It is not detectable by Task Manager nor Windows Defender.<br></br>
        /// YAY!
        /// </summary>
        /// <param name="appLocation">Full path to the application. "C:\Windows\notepad.exe". This parameter cannot be null!</param>
        /// <param name="taskName">Name of the task. If it's null, it will be generated automatically via Guid</param>
        /// <param name="rootFolderName">Special location for the task inside TaskScheduler application. If it's null, the task will be created in root folder = "\"</param>
        /// <param name="customFolderName">Basically a subFolder where the task will be created. If it's set to null, it will not create a subfolder</param>
        /// <param name="triggerType">Tell the TaskScheduler in which certain situation should it run the application. Default is on Logon</param>
        /// <param name="creation">Defines how the Task Scheduler service creates, updates, or disables the task</param>
        /// <param name="runLevel">Set the application privileges. 0 = User, 1 = Administrator</param>
        public static void CreateTaskScheduler(string appLocation, string taskName, string rootFolderName,
            string customFolderName,
            _TASK_TRIGGER_TYPE2 triggerType = _TASK_TRIGGER_TYPE2.TASK_TRIGGER_LOGON,
            TASK_CREATION creation = TASK_CREATION.TASK_CREATE_OR_UPDATE,
            _TASK_RUNLEVEL runLevel = _TASK_RUNLEVEL.TASK_RUNLEVEL_HIGHEST)
        {
            if (taskName == null)
                taskName = Guid.NewGuid().ToString();

            if (rootFolderName == null)
                rootFolderName = @"\";

            TaskScheduler.TaskScheduler ts = new TaskScheduler.TaskScheduler();
            ts.Connect();

            ITaskFolder rootFolder = ts.GetFolder(rootFolderName);

            ITaskFolder customFolder;

            if (customFolderName != null)
                customFolder = rootFolder.CreateFolder(customFolderName, null);
            else
                customFolder = rootFolder;

            ITaskDefinition td = ts.NewTask(0);
            td.Principal.LogonType = _TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN;
            td.Principal.RunLevel = runLevel;

            ITrigger trigger = td.Triggers.Create(triggerType);

            IAction action = td.Actions.Create(_TASK_ACTION_TYPE.TASK_ACTION_EXEC);
            IExecAction execAction = (IExecAction)action;
            execAction.Path = appLocation;

            customFolder.RegisterTaskDefinition(taskName, td, (int)creation, null, null,
                _TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN, null);
        }

        /// <summary>
        /// Checks if the specific subfolder in TaskScheduler exists.<br></br>
        /// This function should be used only if the subfolder was created!
        /// </summary>
        /// <param name="taskFolder">Full path to the specific task folder</param>
        /// <returns>If the task folder exist true, otherwise false</returns>
        public static bool IsTaskDefined(string taskFolder)
        {
            try
            {
                TaskScheduler.TaskScheduler ts = new TaskScheduler.TaskScheduler();
                ts.Connect();

                ITaskFolder folder = ts.GetFolder(taskFolder);

                return folder != null;

            }
            catch
            {
                return false;
            }
        }
    }
}
