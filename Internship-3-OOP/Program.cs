using Internship_3_OOP.Classes;
using Internship_3_OOP.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Internship_3_OOP
{
    public class Program
    {
        private static Dictionary<Project, List<Task>> projectTasks = new Dictionary<Project, List<Task>>();
        static void Main(string[] args)
        {
            LoadInitialData();
            ShowAllProjects();

            Console.WriteLine("\nPritisnite bilo koju tipku za izlaz...");
            Console.ReadKey();
        }
        private static void LoadInitialData()
        {
            Project webShop = new Project(
                "Dump Days",
                "Razvoj web stranice za Dump Days",
                DateTime.Now,
                DateTime.Now.AddMonths(3)
            );

            projectTasks.Add(webShop, new List<Task>());

            var task1 = new Task(
                "Dizajn baze",
                "Kreiranje ERA dijagrama i implementacija baze podataka",
                DateTime.Now.AddDays(14),
                360,
                webShop
            );

            var task2 = new Task(
                "Dizajn sučelja",
                "Implementacija low-fidelity i high-fidelity dijagrama u Figmi",
                DateTime.Now.AddDays(7),
                240,
                webShop
            );

            projectTasks[webShop].Add(task1);
            projectTasks[webShop].Add(task2);

            task1.Status = TaskStatus.Completed;
        }
        private static void ShowAllProjects()
        {
            foreach (var kvp in projectTasks)
            {
                var project = kvp.Key;
                var tasks = kvp.Value;

                Console.WriteLine($"{project.Name}\n");
                Console.WriteLine($"Status: {project.Status}");
                Console.WriteLine($"Opis: {project.Description}");
                Console.WriteLine($"Datum početka: {project.StartDate:dd.MM.yyyy.}");
                Console.WriteLine($"Datum završetka: {project.EndDate:dd.MM.yyyy.}");

                Console.WriteLine("\nZadaci:");
                if (!tasks.Any())
                {
                    Console.WriteLine("- Nema zadataka");
                }
                else
                {
                    foreach (var task in tasks)
                    {
                        Console.WriteLine($"\n  - {task.Name}");
                        Console.WriteLine($"    Status: {task.Status}");
                        Console.WriteLine($"    Rok: {task.Deadline:dd.MM.yyyy.}");
                        Console.WriteLine($"    Trajanje: {task.Duration} minuta");
                    }
                }

                Console.WriteLine("\n" + new string('-', 50) + "\n");
            }
        }
    }
}
