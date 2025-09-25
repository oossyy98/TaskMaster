using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMaster.Interfaces;
using TaskMaster.Models;

namespace TaskMaster.UI
{
    public class ConsoleUI
    {
        private readonly ITaskService _taskService;

        public ConsoleUI(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public void Start()
        {
            Console.WriteLine("Välkommen till TaskMaster");
            while (true)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        ShowTasks();
                        break;
                    case "3":
                        RemoveTask();
                        break;
                    case "4":
                        ToggleTaskCompletion();
                        break;
                    case "5":
                        Console.WriteLine("Tack för att du använde TaskMaster!");
                        return; // Avsluta programmet
                    default:
                        Console.WriteLine("❌ Ogiltigt val! Välj 1-5.");
                        break;
                }

                Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("\n=== TASKMASER MENY ===");
            Console.WriteLine("Välj ett alternativ:");
            Console.WriteLine("1. Lägg till en ny uppgift");
            Console.WriteLine("2. Visa alla uppgifter");
            Console.WriteLine("3. Ta bort en uppgift");
            Console.WriteLine("4. Markera en uppgift som färdig");
            Console.WriteLine("5. Avsluta");
            Console.Write("Ditt val: ");
        }

        private void AddTask()
        {
            Console.Write("Skriv din uppgift: ");
            string title = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(title))
            {
                _taskService.AddTask(title);
                Console.WriteLine("✅ Uppgift tillagd!");
            }
            else
            {
                Console.WriteLine("❌ Du måste skriva något!");
            }
        }

        private void ShowTasks()
        {
            var tasks = _taskService.GetAllTasks();
            if (tasks.Count == 0)
            {
                Console.WriteLine("📝 Inga uppgifter än!");
                return;
            }

            Console.WriteLine("\n--- DINA UPPGIFTER ---");
            foreach (var task in tasks)
            {
                Console.WriteLine(task); // Använder vår ToString() override!
            }
        }

        private void RemoveTask()
        {
            if (_taskService.GetTaskCount() == 0)
            {
                Console.WriteLine("📝 Inga uppgifter att ta bort!");
                return;
            }

            ShowTasks();
            Console.Write("Vilket ID vill du ta bort? ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (_taskService.RemoveTask(id))
                {
                    Console.WriteLine("✅ Uppgift borttagen!");
                }
                else
                {
                    Console.WriteLine("❌ Kunde inte hitta uppgift med det ID:t!");
                }
            }
            else
            {
                Console.WriteLine("❌ Ogiltigt nummer!");
            }
        }

        private void ToggleTaskCompletion()
        {
            if (_taskService.GetTaskCount() == 0)
            {
                Console.WriteLine("📝 Inga uppgifter att markera!");
                return;
            }

            ShowTasks();
            Console.Write("Vilket ID vill du markera som klar/oklar? ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (_taskService.ToggleTaskCompletion(id))
                {
                    Console.WriteLine("✅ Status ändrad!");
                }
                else
                {
                    Console.WriteLine("❌ Kunde inte hitta uppgift med det ID:t!");
                }
            }
            else
            {
                Console.WriteLine("❌ Ogiltigt nummer!");
            }
        }
    }
}