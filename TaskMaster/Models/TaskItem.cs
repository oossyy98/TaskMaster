using System;

namespace TaskMaster.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; } // ? = kan vara null
        public string Category { get; set; }

        //Constructor - körs när man skapar ny TaskItem
        public TaskItem(string title)
        {
            Title = title;
            IsCompleted = false;
            CreatedAt = DateTime.Now;
            Category = "Allmän";
        }

        public TaskItem()
        {
            // Tom för att kunna skapa en instans i framtiden utan att behöva skicka in en titel
        }

        //Override ToString() för att skriva ut TaskItem på ett fint sätt
        public override string ToString()
        {
            string status = IsCompleted ? "✅ Färdig" : "⏳ Pågående";
            return $"{Id}. [{status}] {Title}";
        }
    }
}