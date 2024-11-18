using Internship_3_OOP.Classes;
using Internship_3_OOP.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Internship_3_OOP
{
    public class Program
    {
        private static Dictionary<Project, List<Task>> projectTasks = new Dictionary<Project, List<Task>>();
        private static Project SelectProject()
        {
            var projects = projectTasks.Keys.ToList();
            for (int i = 0; i < projects.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {projects[i].Name}");
            }

            Console.Write("\nOdaberite projekt (broj): ");
            if (!int.TryParse(Console.ReadLine(), out int option) || option < 1 || option > projects.Count)
                throw new ArgumentException("Nevaljan odabir projekta.");

            return projects[option - 1];
        }
        private static void ShowMainMenu()
        {
            Console.WriteLine("GLAVNI IZBORNIK");
            Console.WriteLine("1 - Ispis svih projekata s pripadajućim zadacima");
            Console.WriteLine("2 - Dodavanje novog projekta");
            Console.WriteLine("3 - Brisanje projekta");
            Console.WriteLine("4 - Prikaz svih zadataka s rokom u sljedećih 7 dana");
            Console.WriteLine("5 - Prikaz projekata filtriranih po statusu");
            Console.WriteLine("6 - Upravljanje pojedinim projektom");
            Console.WriteLine("7 - Upravljanje pojedinim zadatkom");
            Console.WriteLine("0 - Izlaz");
            Console.Write("\nOdabir: ");
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
            if (!projectTasks.Any())
            {
                Console.WriteLine("Nema projekata.");
                return;
            }
            foreach (var kvp in projectTasks)
            {
                Console.WriteLine($"Projekt: {kvp.Key.Name} ({kvp.Key.Status})");
                Console.WriteLine($"Opis: {kvp.Key.Description}");
                Console.WriteLine("Zadaci:");

                if (!kvp.Value.Any())
                    Console.WriteLine("- Nema zadataka");
                else
                    foreach (var task in kvp.Value)
                    {
                        Console.WriteLine($"- {task.Name}");
                        Console.WriteLine($"  Rok: {task.Deadline:dd.MM.yyyy.}");
                    }
                Console.WriteLine("\n" + new string('-', 50) + "\n");
            }
        }
        private static void AddNewProject() {
            Console.WriteLine("NOVI PROJEKT\n");
            string name;
            do
            {
                Console.Write("Naziv projekta: ");
                name = Console.ReadLine();
                if (projectTasks.Keys.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"Projekt s imenom '{name}' već postoji. Molimo odaberite drugo ime.");
                }
            } while (projectTasks.Keys.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));
            Console.Write("Opis projekta: ");
            string description = Console.ReadLine();

            DateTime startDate;
            while (true)
            {
                Console.Write("Datum početka (dd.MM.yyyy.): ");
                try
                {
                    startDate = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy.", null);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Pogrešan format datuma. Molimo unesite datum u formatu DD.MM.YYYY.");
                }
            }

            DateTime endDate;
            while (true)
            {
                Console.Write("Datum završetka (dd.MM.yyyy.): ");
                try
                {
                    endDate = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy.", null);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Pogrešan format datuma. Molimo unesite datum u formatu DD.MM.YYYY.");
                }
            }

            var project = new Project(name, description, startDate, endDate);
            projectTasks.Add(project, new List<Task>());

            Console.WriteLine("\nProjekt uspješno dodan!");
        }
        private static void DeleteProject() {
            Console.WriteLine("BRISANJE PROJEKTA\n");
            if (!projectTasks.Any())
            {
                Console.WriteLine("Nema projekata za brisanje.");
                return;
            }

            var project = SelectProject();

            Console.Write("Jeste li sigurni da želite izbrisati projekt? (da/ne): ");
            if (Console.ReadLine().ToLower() == "da")
            {
                projectTasks.Remove(project);
                Console.WriteLine("\nProjekt je uspješno izbrisan!");
            }
        }
        private static void ShowTasksNextWeek() {
            DateTime oneWeekFromNow = DateTime.Now.AddDays(7);
            var tasks = projectTasks
                .SelectMany(pt => pt.Value)
                .Where(t => t.Deadline <= oneWeekFromNow && t.Status == TaskStatus.Active)
                .OrderBy(t => t.Deadline);

            Console.WriteLine("ZADACI U SLJEDEĆIH 7 DANA\n");
            if (!tasks.Any())
            {
                Console.WriteLine("Nema zadataka u sljedećih 7 dana.");
                return;
            }
            foreach (var task in tasks)
            {
                Console.WriteLine($"- {task.Name} ({task.Project.Name})");
                Console.WriteLine($"  Rok: {task.Deadline:dd.MM.yyyy.}");
            }
        }
        private static void ShowProjectsByStatus() {
            Console.WriteLine("FILTRIRANJE PROJEKATA PO STATUSU");
            Console.WriteLine("1 - Aktivni");
            Console.WriteLine("2 - Na čekanju");
            Console.WriteLine("3 - Završeni");
            Console.Write("\nOdabir: ");
            string choice = Console.ReadLine();
            ProjectStatus status;

            switch (choice)
            {
                case "1":
                    status = ProjectStatus.Active;
                    break;
                case "2":
                    status = ProjectStatus.OnHold;
                    break;
                case "3":
                    status = ProjectStatus.Completed;
                    break;
                default:
                    Console.WriteLine("Nepostojeća opcija. Pokušajte ponovno.");
                    return;
            }

            var filteredProjects = projectTasks.Keys.Where(p => p.Status == status);

            foreach (var project in filteredProjects)
            {
                Console.WriteLine($"\nProjekt: {project.Name}");
                Console.WriteLine($"Opis: {project.Description}");
                Console.WriteLine($"Datum početka: {project.StartDate:dd.MM.yyyy.}");
                Console.WriteLine($"Datum završetka: {project.EndDate:dd.MM.yyyy.}");
            }
        }
        private static void ManageProject() { }
        private static void ManageTask() { }

        static void Main(string[] args)
        {
            LoadInitialData();

            while (true)
            {
                try
                {
                    ShowMainMenu();
                    string choice = Console.ReadLine();
                    Console.Clear();

                    switch (choice)
                    {
                        case "1":
                            ShowAllProjects();
                            break;
                        case "2":
                            AddNewProject();
                            break;
                        case "3":
                            DeleteProject();
                            break;
                        case "4":
                            ShowTasksNextWeek();
                            break;
                        case "5":
                            ShowProjectsByStatus();
                            break;
                        case "6":
                            ManageProject();
                            break;
                        case "7":
                            ManageTask();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Nepostojeća opcija. Pokušajte ponovno.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n" + e.Message);
                }

                Console.Write("\nPritisnite bilo koju tipku za nastavak...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
