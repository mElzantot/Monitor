using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class SubTaskController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHostingEnvironment hostingEnvironment;

        public SubTaskController(IUnitOfWork unitOfWork,IHostingEnvironment hostingEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index(string engineerID)
        {
            IEnumerable<SubTask> subTasks = unitOfWork.SubTasks.GetSubTasksByEngineerId(engineerID);      
            return View("Engineer",subTasks);
        }

        public IActionResult DisplayRow(int ID)
        {
            SubTask subTask = unitOfWork.SubTasks.GetSubWithItsFiles(ID);
            return PartialView("_SubTaskDataPartial", subTask);
        }


        public void EditProgress(int ID, int progress)        {            SubTask subTask = unitOfWork.SubTasks.GetSubTaskIncludingTask(ID);

            int subTaskLastProgress = subTask.Progress;

            //Shaker
            //Activity task = unitOfWork.Tasks.GetById(subTask.FK_TaskId);
            Activity task = subTask.Task;

            List<SubTask> subTasks = unitOfWork.SubTasks.GetSubTasksByTaskId(subTask.FK_TaskId);            int totalSubTaskDuration = 0;            foreach (var item in subTasks)            {                totalSubTaskDuration += (int)(item.EndDate - item.StartDate).Value.TotalDays;            }            int subtaskDuration = (int)(subTask.EndDate - subTask.StartDate).Value.TotalDays;            task.Progress += ((progress - subTaskLastProgress) * (subtaskDuration)) / (totalSubTaskDuration);            task = unitOfWork.Tasks.Edit(task);
            subTask.Progress = progress;            subTask = unitOfWork.SubTasks.Edit(subTask);        }

        


        public void EditIsUnderWork(int ID, bool Is)
        {
            SubTask subTask = unitOfWork.SubTasks.GetSubTaskIncludingProject(ID);
            subTask.IsUnderWork = Is;
           
            if (Is)
            {
                SubTaskSession subTaskSession = new SubTaskSession()
                {
                    FK_SubTaskID = ID,
                    SessStartDate = DateTime.Now
                };
                subTaskSession = unitOfWork.SubTaskSessions.Add(subTaskSession);
            }
            else
            {
                SubTaskSession subTaskSession = unitOfWork.SubTaskSessions.GetLastSessBySubTaskID(ID);
                subTaskSession.SessEndtDate = DateTime.Now;
                double hourDuration = (double)(subTaskSession.SessEndtDate - subTaskSession.SessStartDate).Value.TotalHours;
                subTaskSession.SessDuration = (int)Math.Round(hourDuration, 0);
                subTaskSession = unitOfWork.SubTaskSessions.Edit(subTaskSession);

                subTask.ActualDuration += subTaskSession.SessDuration;

                Activity task = unitOfWork.Tasks.GetById(subTask.FK_TaskId);
                int LastTaskDuaration = task.ActualDuratoin;
                task.ActualDuratoin += subTask.ActualDuration;
                task = unitOfWork.Tasks.Edit(task);
                Project project = unitOfWork.Projects.GetById(task.FK_ProjectId);
                project.ActualDuration += task.ActualDuratoin - LastTaskDuaration;
                unitOfWork.Projects.Edit(project);

            }
            subTask = unitOfWork.SubTasks.Edit(subTask);
        }

        public void EditStatus(int ID, Status status)
        {
            SubTask subTask = unitOfWork.SubTasks.GetById(ID);
            subTask.Status = status;
            unitOfWork.SubTasks.Edit(subTask);
        }
        public void EditPriority(int ID, Priority priority)
        {
            Activity task = unitOfWork.Tasks.GetById(ID);
            task.Priority = priority;
            unitOfWork.Tasks.Edit(task);
        }




        [HttpGet]
        public IActionResult displaySubTasks(int taskID)
        {
            var taskVM = new TaskViewModel
            {
                TaskId = taskID,
                taskDescription = unitOfWork.Tasks.GetById(taskID).Description,
                SubTasks = unitOfWork.SubTasks.GetSubTasksByTaskId(taskID),
            };
            return PartialView("_SubTaskDisplayPartial", taskVM);

        }

        [HttpGet]
        public IActionResult AddSubTask(int taskID, int teamId)
        {
            var subTask = new SubTaskViewModel
            {
                FK_TaskId = taskID,
                TeamMembers = unitOfWork.Engineers.GetEngineersInsideTeam(teamId).ToList()
            };
            return PartialView("_SubTaskModal", subTask);
        }

        [HttpPost]
        public IActionResult AddSubTask(SubTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var startDate = model.StartDate.Split('/').Select(Int32.Parse).ToList();
                var endDate = model.EndDate.Split('/').Select(Int32.Parse).ToList();
                var newSubTask = new SubTask
                {
                    Name = model.Name,
                    Description = model.Description,
                    FK_TaskId = model.FK_TaskId,
                    FK_EngineerID = model.Assignee,
                    Priority = model.Priority,
                    Status = model.Status,
                    StartDate = new DateTime(startDate[2], startDate[1], startDate[0]),
                    EndDate = new DateTime(endDate[2], endDate[1], endDate[0]),
                    
                };

                newSubTask = unitOfWork.SubTasks.Add(newSubTask);
                newSubTask.Engineer = unitOfWork.Engineers.GetById(model.Assignee);

                int subTaskId = newSubTask.Id;

                string uniqeFileName = null;
                if (model.files != null)
                {
                    //-------Get Files Folder path in Server
                    string uploaderFolder = Path.Combine(hostingEnvironment.WebRootPath, "files");

                    foreach (var item in model.files)
                    {
                        //-------Create New Guid fo each file
                        uniqeFileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                        //---------The full path for file
                        string filePath = Path.Combine(uploaderFolder, uniqeFileName);
                        //----------Copy file to server
                        item.CopyTo(new FileStream(filePath, FileMode.Create));

                        //------Creat instance of file
                        Files file = new Files
                        {
                            FilePath = uniqeFileName,
                            FK_SubTaskId = subTaskId,
                            RecieverId = newSubTask.FK_EngineerID,
                            FileType = Entities.Enums.FileType.Subtask,
                            Time = DateTime.Now,
                            

                        };
                        //-----Add file To DB
                        unitOfWork.Files.Add(file);
                    }
                }
                return PartialView("_NewSubTaskPartialView", newSubTask);
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public IActionResult AddFile(int staskid)
        {
            MySubTaskViewModel subtaskVM = new MySubTaskViewModel();

            subtaskVM.SubTask = unitOfWork.SubTasks.GetSubTaskIncludingTask(staskid);
            return View(subtaskVM);
        }

        [HttpPost]
        public IActionResult AddFile(MySubTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                SubTask subtask = new SubTask();
                subtask = model.SubTask;
                int subtaskid = subtask.Id;
                string uniqeFileName = null;
                if (model.Files != null)
                {
                    //-------Get Files Folder path in Server
                    string uploaderFolder = Path.Combine(hostingEnvironment.WebRootPath, "files");

                    foreach (var item in model.Files)
                    {
                        //-------Create New Guid fo each file
                        uniqeFileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                        //---------The full path for file
                        string filePath = Path.Combine(uploaderFolder, uniqeFileName);
                        //----------Copy file to server
                        item.CopyTo(new FileStream(filePath, FileMode.Create));

                        //------Creat instance of file
                        Files file = new Files
                        {
                            FilePath = uniqeFileName,
                            FK_SubTaskId = subtaskid,
                            FK_SenderId = subtask.FK_EngineerID,
                            FileType = Entities.Enums.FileType.Subtask,
                            Time = DateTime.Now,


                        };

                        //-----Add file To DB
                        unitOfWork.Files.Add(file);

                    }

                }
                return View(model);

            }
            else return View("Error");
        }

    }
}