using System;
using System.Collections.Generic;
using System.Linq;
using TaskMaster.Interfaces;
using TaskMaster.Models;

namespace TaskMaster.Services
{
    public class TaskService : ITaskService
    {
        private List<TaskItem> _tasks;
        private int _nextId;

        public TaskService()
        {
            _tasks = new List<TaskItem>();
            _nextId = 1;
        }

        public void AddTask(string title)
        {
            var newTask = new TaskItem(title)
            {
                Id = _nextId++
            };
            _tasks.Add(newTask);
        }

        public List<TaskItem> GetAllTasks()
        {
            return _tasks.ToList(); // Returnera kopia, inte originalistan
        }

        public bool RemoveTask(int id)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                _tasks.Remove(task);
                return true;
            }
            return false;
        }

        public bool ToggleTaskCompletion(int id)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                return true;
            }
            return false;
        }

        public int GetTaskCount()
        {
            return _tasks.Count;
        }

        public TaskItem GetTaskById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }
    }
}