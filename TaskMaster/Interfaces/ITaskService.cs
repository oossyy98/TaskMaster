using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMaster.Models;

namespace TaskMaster.Interfaces
{
    public interface ITaskService
    {
        void AddTask(string title);
        List<TaskItem> GetAllTasks();
        bool RemoveTask(int id);
        bool ToggleTaskCompletion(int id);
        int GetTaskCount();
        TaskItem GetTaskById(int id);
    }
}
