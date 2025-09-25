using System;
using System.Windows.Forms;
using TaskMaster.Interfaces;
using TaskMaster.Services;
using TaskMaster.UI;

namespace TaskMaster
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Skapa våra objekt
            var taskService = new TaskService();
            var mainForm = new MainForm(taskService);

            // Starta WinForms-applikationen
            Application.Run(mainForm);
        }
    }
}