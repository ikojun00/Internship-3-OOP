using Internship_3_OOP.Classes;
using Internship_3_OOP.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Xml.Linq;

namespace Internship_3_OOP
{
    public class Program
    {
        private static Dictionary<Project, List<Task>> projectTasks = new Dictionary<Project, List<Task>>();
        public static string ProjectStatusToCroatian(ProjectStatus status)
        {
            switch (status)
            {
                case ProjectStatus.Active:
                    return "Aktivan";
                case ProjectStatus.OnHold:
                    return "Na čekanju";
                case ProjectStatus.Completed:
                    return "Završen";
                default:
                    return status.ToString();
            }
        }
        public static string TaskStatusToCroatian(TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Active:
                    return "Aktivan";
                case TaskStatus.Completed:
                    return "Završen";
                case TaskStatus.Postponed:
                    return "Odgođen";
                default:
                    return status.ToString();
            }
        }
        private static Task SelectTask(Project project)
        {
            var tasks = projectTasks[project];
            for (int i = 0; i < tasks.Count; i++)
                Console.WriteLine($"{i + 1} - {tasks[i].Name}");

            Console.Write($"\nOdaberite zadatak iz projekta '{project.Name}' (broj): ");
            if (!int.TryParse(Console.ReadLine(), out int option) || option < 1 || option > tasks.Count)
                throw new ArgumentException("Nevaljan odabir zadatka.");

            return tasks[option - 1];
        }
        private static Project SelectProject()
        {
            var projects = projectTasks.Keys.ToList();
            for (int i = 0; i < projects.Count; i++)
                Console.WriteLine($"{i + 1} - {projects[i].Name}");

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
        private static void ShowProjectManagement(string projectName)
        {
            Console.WriteLine($"Upravljanje projektom '{projectName}'");
            Console.WriteLine("1 - Ispis svih zadataka unutar odabranog projekta");
            Console.WriteLine("2 - Prikaz detalja odabranog projekta");
            Console.WriteLine("3 - Uređivanje statusa projekta");
            Console.WriteLine("4 - Dodavanje zadatka unutar projekta");
            Console.WriteLine("5 - Brisanje zadatka iz projekta");
            Console.WriteLine("6 - Prikaz ukupno očekivanog vremena potrebnog za sve aktivne zadatke u projektu");
            Console.WriteLine("0 - Izlaz");
            Console.Write("\nOdabir: ");
        }
        private static void ShowTaskManagement(string taskName)
        {
            Console.WriteLine($"Upravljanje zadatkom '{taskName}'");
            Console.WriteLine("1 - Prikaz detalja odabranog zadatka");
            Console.WriteLine("2 - Uređivanje statusa zadatka");
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
                Console.WriteLine($"'{kvp.Key.Name}' ({ProjectStatusToCroatian(kvp.Key.Status)})");
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
            while(true)
            {
                Console.Write("Naziv projekta: ");
                name = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine($"Ime projekta ne može biti prazno.\n");
                    continue;
                }
                else if (projectTasks.Keys.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"Projekt s imenom '{name}' već postoji. Molimo odaberite drugo ime.\n");
                    continue;
                }
                break;
            }
            string description;
            while (true)
            {
                Console.Write("Opis projekta: ");
                description = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(description))
                {
                    Console.WriteLine($"Opis projekta ne može biti prazan.\n");
                    continue;
                }
                break;
            }

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
            if (!filteredProjects.Any())
            {
                Console.WriteLine("Nema projekata sa odabranim statusom.");
                return;
            }
            foreach (var project in filteredProjects)
            {
                Console.WriteLine($"\nProjekt: {project.Name}");
                Console.WriteLine($"Opis: {project.Description}");
                Console.WriteLine($"Datum početka: {project.StartDate:dd.MM.yyyy.}");
                Console.WriteLine($"Datum završetka: {project.EndDate:dd.MM.yyyy.}");
            }
        }
        private static void ShowProjectTasks(Project project)
        {
            var tasks = projectTasks[project];
            if (!tasks.Any())
            {
                Console.WriteLine("Nema zadataka u projektu.");
                return;
            }

            Console.WriteLine($"Zadaci projekta '{project.Name}':");
            foreach (var task in tasks)
            {
                Console.WriteLine($"- {task.Name} ({TaskStatusToCroatian(task.Status)})");
                Console.WriteLine($"  Rok: {task.Deadline:dd.MM.yyyy.}");
                Console.WriteLine($"  Trajanje: {task.Duration} minuta");
            }
        }
        private static void ShowProjectDetails(Project project)
        {
            Console.WriteLine($"Detalji projekta '{project.Name}':");
            Console.WriteLine($"Opis: {project.Description}");
            Console.WriteLine($"Status: {ProjectStatusToCroatian(project.Status)}");
            Console.WriteLine($"Datum početka: {project.StartDate:dd.MM.yyyy.}");
            Console.WriteLine($"Datum završetka: {project.EndDate:dd.MM.yyyy.}");
        }

        private static void UpdateProjectStatus(Project project)
        {
            if (project.Status == ProjectStatus.Completed)
            {
                Console.WriteLine("Ne možete mijenjati status završenog projekta.");
                return;
            }

            Console.WriteLine("1 - Aktivan");
            Console.WriteLine("2 - Na čekanju");
            Console.WriteLine("3 - Završen");
            Console.Write("\nOdabir: ");
            string choice = Console.ReadLine();
            ProjectStatus newStatus;

            switch (choice)
            {
                case "1":
                    newStatus = ProjectStatus.Active;
                    break;
                case "2":
                    newStatus = ProjectStatus.OnHold;
                    break;
                case "3":
                    newStatus = ProjectStatus.Completed;
                    break;
                default:
                    Console.WriteLine("Nepostojeća opcija. Pokušajte ponovno.");
                    return;
            }


            project.Status = newStatus;
            if (newStatus == ProjectStatus.Completed)
            {
                foreach (var task in projectTasks[project]) {
                    if(task.Status != TaskStatus.Completed)
                    task.Status = TaskStatus.Completed; 
                }
            }
            Console.WriteLine("\nStatus projekta je uspješno ažuriran.");
        }

        private static void AddTaskToProject(Project project)
        {
            if (project.Status == ProjectStatus.Completed)
            {
                Console.WriteLine("Ne možete dodavati zadatke u završeni projekt.");
                return;
            }

            Console.WriteLine("NOVI ZADATAK\n");
            string name;
            while (true)
            {
                Console.Write("Naziv zadatka: ");
                name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine($"Ime zadatka ne može biti prazno.\n");
                    continue;
                }
                else if (projectTasks[project].Any(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"Zadatak s imenom '{name}' već postoji. Molimo odaberite drugo ime.");
                    continue;
                }
                break;
            }
            string description;
            while (true)
            {
                Console.Write("Opis zadatka: ");
                description = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(description))
                {
                    Console.WriteLine($"Opis projekta ne može biti prazan.\n");
                    continue;
                }
                break;
            }

            DateTime deadline;
            while (true)
            {
                Console.Write("Rok (dd.MM.yyyy.): ");
                try
                {
                    deadline = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy.", null);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Pogrešan format datuma. Molimo unesite datum u formatu DD.MM.YYYY.");
                }
            }
            int duration;
            while (true)
            {
                Console.Write("Trajanje (minute): ");
                if (!int.TryParse(Console.ReadLine(), out duration))
                {
                    Console.WriteLine("Unos za trajanje nije broj.");
                    continue;
                }
                break;
            }
            

            var task = new Task(name, description, deadline, duration, project);
            projectTasks[project].Add(task);

            Console.WriteLine("\nZadatak uspješno dodan!");
        }

        private static void DeleteTaskFromProject(Project project)
        {
            if (project.Status == ProjectStatus.Completed)
            {
                Console.WriteLine("Ne možete brisati zadatke iz završenog projekta.");
                return;
            }

            if (!projectTasks[project].Any())
            {
                Console.WriteLine("Nema zadataka za brisanje.");
                return;
            }

            var task = SelectTask(project);

            Console.Write("Jeste li sigurni da želite izbrisati zadatak? (da/ne): ");
            if (Console.ReadLine().ToLower() == "da")
            {
                projectTasks[project].Remove(task);
                Console.WriteLine("\nZadatak je uspješno izbrisan!");
            }
        }

        private static void ShowTotalActiveTime(Project project)
        {
            if (project.Status == ProjectStatus.Completed)
            {
                Console.WriteLine("Nema aktivnih zadataka u završenom projektu.");
                return;
            }

            int totalMinutes = projectTasks[project]
                .Where(t => t.Status == TaskStatus.Active)
                .Sum(t => t.Duration);

            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;

            Console.WriteLine($"Ukupno očekivano vrijeme za aktivne zadatke: {hours} sati i {minutes} minuta");
        }
        private static void ShowTaskDetails(Task task)
        {
            Console.WriteLine($"Detalji projekta '{task.Name}':");
            Console.WriteLine($"Opis: {task.Description}");
            Console.WriteLine($"Status: {TaskStatusToCroatian(task.Status)}");
            Console.WriteLine($"Rok: {task.Deadline:dd.MM.yyyy.}");
            Console.WriteLine($"Trajanje: {task.Duration} minuta");
        }
        private static void UpdateTaskStatus(Task task)
        {
            if (task.Status == TaskStatus.Completed)
            {
                Console.WriteLine("Ne možete mijenjati status završenog zadatka.");
                return;
            }

            Console.WriteLine("1 - Aktivan");
            Console.WriteLine("2 - Završen");
            Console.WriteLine("3 - Odgođen");
            Console.Write("\nOdabir: ");
            string choice = Console.ReadLine();
            TaskStatus newStatus;

            switch (choice)
            {
                case "1":
                    newStatus = TaskStatus.Active;
                    break;
                case "2":
                    newStatus = TaskStatus.Completed;
                    break;
                case "3":
                    newStatus = TaskStatus.Postponed;
                    break;
                default:
                    Console.WriteLine("Nepostojeća opcija. Pokušajte ponovno.");
                    return;
            }


            task.Status = newStatus;
            Console.WriteLine("\nStatus zadatka je uspješno ažuriran.");
        }
        private static void ManageProject()
        {
            var project = SelectProject();
            Console.Clear();
            while (true)
            {
                try
                {
                    ShowProjectManagement(project.Name);
                    string choice = Console.ReadLine();
                    Console.Clear();
                    switch (choice)
                    {
                        case "1":
                            ShowProjectTasks(project);
                            break;
                        case "2":
                            ShowProjectDetails(project);
                            break;
                        case "3":
                            UpdateProjectStatus(project);
                            break;
                        case "4":
                            AddTaskToProject(project);
                            break;
                        case "5":
                            DeleteTaskFromProject(project);
                            break;
                        case "6":
                            ShowTotalActiveTime(project);
                            break;
                        case "7":
                            ManageTask();
                            break;
                        case "0":
                            Console.WriteLine($"Izašli ste iz izbornika za upravljanje projektom '{project.Name}'.");
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
        private static void ManageTask() {
            var project = SelectProject();
            Console.Clear();
            var task = SelectTask(project);
            Console.Clear();
            while (true)
            {
                try
                {
                    ShowTaskManagement(task.Name);
                    string choice = Console.ReadLine();
                    Console.Clear();
                    switch (choice)
                    {
                        case "1":
                            ShowTaskDetails(task);
                            break;
                        case "2":
                            UpdateTaskStatus(task);
                            break;
                        case "0":
                            Console.WriteLine($"Izašli ste iz izbornika za upravljanje zadatkom '{task.Name}'.");
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
