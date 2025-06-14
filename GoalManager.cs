// Central manager for Eternal Quest program - handles goals, scoring, streaks, and persistence

using System;
using System.Collections.Generic;
using System.IO;

namespace EternalQuest
{
    public class GoalManager
    {
        private List<Goal> _goals;
        private int _score;
        private int _streakDays;           // Creative feature: track consecutive-day streak
        private DateTime _lastEventDate;

        public GoalManager()
        {
            _goals = new List<Goal>();
            _score = 0;
            _streakDays = 0;
            _lastEventDate = DateTime.MinValue;
        }

        public void Start()
        {
            LoadGoals();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nEternal Quest Menu:");
                Console.WriteLine("1. Create Goal");
                Console.WriteLine("2. List Goals");
                Console.WriteLine("3. Record Event");
                Console.WriteLine("4. Display Score");
                Console.WriteLine("5. Save Goals");
                Console.WriteLine("6. Load Goals");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine()!.Trim();
                switch (choice)
                {
                    case "1":
                        CreateGoal();
                        break;
                    case "2":
                        ListGoalDetails();
                        break;
                    case "3":
                        RecordEvent();
                        break;
                    case "4":
                        DisplayPlayerInfo();
                        break;
                    case "5":
                        SaveGoals();
                        break;
                    case "6":
                        LoadGoals();
                        break;
                    case "7":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        public void DisplayPlayerInfo()
        {
            Console.WriteLine($"\nYour score: {_score}");
            Console.WriteLine($"Current streak: {_streakDays} day(s)");
        }

        public void ListGoalDetails()
        {
            Console.WriteLine();
            for (int i = 0; i < _goals.Count; i++)
                Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
        }

        public void CreateGoal()
        {
            // 1. Read and validate goal type
            string type;
            while (true)
            {
                Console.Write("Enter goal type (simple, eternal, checklist): ");
                type = Console.ReadLine()!.Trim().ToLower();
                if (type == "simple" || type == "eternal" || type == "checklist")
                    break;
                Console.WriteLine("  → Please enter exactly \"simple\", \"eternal\" or \"checklist\".");
            }

            // 2. Read non‑empty name
            string name;
            do
            {
                Console.Write("Name: ");
                name = Console.ReadLine()!.Trim();
                if (name.Length == 0)
                    Console.WriteLine("  → Name cannot be blank.");
            } while (name.Length == 0);

            // 3. Read non‑empty description
            string desc;
            do
            {
                Console.Write("Description: ");
                desc = Console.ReadLine()!.Trim();
                if (desc.Length == 0)
                    Console.WriteLine("  → Description cannot be blank.");
            } while (desc.Length == 0);

            // 4. Read and validate points
            int pts;
            while (true)
            {
                Console.Write("Points: ");
                string input = Console.ReadLine()!.Trim();
                if (int.TryParse(input, out pts) && pts >= 0)
                    break;
                Console.WriteLine("  → Please enter a non‑negative integer for points.");
            }

            switch (type)
            {
                case "simple":
                    _goals.Add(new SimpleGoal(name, desc, pts));
                    break;

                case "eternal":
                    _goals.Add(new EternalGoal(name, desc, pts));
                    break;

                case "checklist":
                    // 5a. Read and validate target count
                    int tgt;
                    while (true)
                    {
                        Console.Write("Target count: ");
                        if (int.TryParse(Console.ReadLine()!.Trim(), out tgt) && tgt > 0)
                            break;
                        Console.WriteLine("  → Enter an integer > 0.");
                    }
                    // 5b. Read and validate bonus
                    int bon;
                    while (true)
                    {
                        Console.Write("Bonus points: ");
                        if (int.TryParse(Console.ReadLine()!.Trim(), out bon) && bon >= 0)
                            break;
                        Console.WriteLine("  → Enter a non‑negative integer for bonus.");
                    }
                    _goals.Add(new ChecklistGoal(name, desc, pts, tgt, bon));
                    break;
            }

            Console.WriteLine("Goal created.");
        }

        public void RecordEvent()
        {
            ListGoalNames();

            // Read and validate goal selection
            int idx;
            while (true)
            {
                Console.Write("Select goal number: ");
                string input = Console.ReadLine()!.Trim();
                if (int.TryParse(input, out idx) && idx >= 1 && idx <= _goals.Count)
                {
                    idx--;  // zero-based index
                    break;
                }
                Console.WriteLine($"  → Enter a number between 1 and {_goals.Count}.");
            }

            int earned = _goals[idx].RecordEvent();
            _score += earned;

            // Update streak days
            DateTime today = DateTime.Today;
            if (_lastEventDate == today.AddDays(-1))
                _streakDays++;
            else if (_lastEventDate != today)
                _streakDays = 1;
            _lastEventDate = today;

            Console.WriteLine($"You earned {earned} points!");
        }

        private void ListGoalNames()
        {
            Console.WriteLine();
            for (int i = 0; i < _goals.Count; i++)
                Console.WriteLine($"{i + 1}. {_goals[i].ShortName}");
        }

        public void SaveGoals()
        {
            using (StreamWriter writer = new StreamWriter("goals.txt"))
            {
                writer.WriteLine(_score);
                writer.WriteLine(_streakDays);
                writer.WriteLine(_lastEventDate.ToString("o"));
                foreach (var g in _goals)
                    writer.WriteLine(g.GetStringRepresentation());
            }
            Console.WriteLine("Goals saved.");
        }

        public void LoadGoals()
        {
            if (!File.Exists("goals.txt")) return;

            var lines = File.ReadAllLines("goals.txt");
            int idx = 0;

            _score = int.Parse(lines[idx++]);
            _streakDays = int.Parse(lines[idx++]);
            _lastEventDate = DateTime.Parse(lines[idx++]);

            _goals.Clear();
            for (; idx < lines.Length; idx++)
            {
                string line = lines[idx];
                var parts = line.Split(':', 2);
                var d = parts[1].Split(',');

                switch (parts[0])
                {
                    case "SimpleGoal":
                        _goals.Add(new SimpleGoal(d[0], d[1], int.Parse(d[2])));
                        break;

                    case "EternalGoal":
                        _goals.Add(new EternalGoal(d[0], d[1], int.Parse(d[2])));
                        break;

                    case "ChecklistGoal":
                        var cg = new ChecklistGoal(
                            d[0], d[1],
                            int.Parse(d[2]),
                            int.Parse(d[4]),
                            int.Parse(d[5])
                        );
                        // restore progress count
                        int completed = int.Parse(d[3]);
                        for (int i = 0; i < completed; i++)
                            cg.RecordEvent();
                        _goals.Add(cg);
                        break;
                }
            }

            Console.WriteLine("Goals loaded.");
        }
    }
}
