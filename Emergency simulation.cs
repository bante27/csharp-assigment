using System;
using System.Collections.Generic;
using System.Threading;

namespace EmergencyCitySimulation
{

    abstract class EmergencyUnit
    {
        public string Name { get; set; }
        public int Speed { get; set; }

        public EmergencyUnit(string name, int speed)
        {
            Name = name;
            Speed = speed;
        }

        public abstract bool CanHandle(string incidentType);
        public abstract void RespondToIncident(Incident incident, int responseTime);
    }


    class Police : EmergencyUnit
    {
        public Police(string name, int speed) : base(name, speed) { }
        public override bool CanHandle(string incidentType) => incidentType == "Crime";
        public override void RespondToIncident(Incident incident, int responseTime)
        {
            Console.WriteLine($"{Name} is handling a Crime at {incident.Location} (Time: {responseTime}s).");
        }
    }

    class Firefighter : EmergencyUnit
    {
        public Firefighter(string name, int speed) : base(name, speed) { }
        public override bool CanHandle(string incidentType) => incidentType == "Fire";
        public override void RespondToIncident(Incident incident, int responseTime)
        {
            Console.WriteLine($"{Name} is extinguishing a Fire at {incident.Location} (Time: {responseTime}s).");
        }
    }

    class Ambulance : EmergencyUnit
    {
        public Ambulance(string name, int speed) : base(name, speed) { }
        public override bool CanHandle(string incidentType) => incidentType == "Medical";
        public override void RespondToIncident(Incident incident, int responseTime)
        {
            Console.WriteLine($"{Name} is responding to a Medical emergency at {incident.Location} (Time: {responseTime}s).");
        }
    }

    class HazmatTeam : EmergencyUnit
    {
        public HazmatTeam(string name, int speed) : base(name, speed) { }
        public override bool CanHandle(string incidentType) => incidentType == "Hazardous Material";
        public override void RespondToIncident(Incident incident, int responseTime)
        {
            Console.WriteLine($"{Name} is containing a Hazardous Material incident at {incident.Location} (Time: {responseTime}s).");
        }
    }

    class RescueTeam : EmergencyUnit
    {
        public RescueTeam(string name, int speed) : base(name, speed) { }
        public override bool CanHandle(string incidentType) => incidentType == "Flood";
        public override void RespondToIncident(Incident incident, int responseTime)
        {
            Console.WriteLine($"{Name} is rescuing people from a Flood at {incident.Location} (Time: {responseTime}s).");
        }
    }

    class DisasterResponseTeam : EmergencyUnit
    {
        public DisasterResponseTeam(string name, int speed) : base(name, speed) { }
        public override bool CanHandle(string incidentType) => incidentType == "Earthquake";
        public override void RespondToIncident(Incident incident, int responseTime)
        {
            Console.WriteLine($"{Name} is providing disaster relief after an Earthquake at {incident.Location} (Time: {responseTime}s).");
        }
    }

    class StructuralRescueTeam : EmergencyUnit
    {
        public StructuralRescueTeam(string name, int speed) : base(name, speed) { }
        public override bool CanHandle(string incidentType) => incidentType == "Building Collapse";
        public override void RespondToIncident(Incident incident, int responseTime)
        {
            Console.WriteLine($"{Name} is rescuing victims from a Building Collapse at {incident.Location} (Time: {responseTime}s).");
        }
    }

    // Incident class
    class Incident
    {
        public string Type { get; set; }
        public string Location { get; set; }
        public string Difficulty { get; set; }

        public Incident(string type, string location, string difficulty)
        {
            Type = type;
            Location = location;
            Difficulty = difficulty;
        }
    }


    class Program
    {
        static void Main()
        {
            var random = new Random();
            var units = new List<EmergencyUnit>
            {
                new Police("Police Unit", 80),
                new Firefighter("Fire Team", 70),
                new Ambulance("Ambulance Squad", 90),
                new HazmatTeam("Hazmat Response Team", 60),
                new RescueTeam("Flood Rescue Team", 65),
                new DisasterResponseTeam("Earthquake Disaster Team", 55),
                new StructuralRescueTeam("Building Collapse Rescue Team", 60)
            };

            string[] incidentTypes = { "Crime", "Fire", "Medical", "Hazardous Material", "Flood", "Earthquake", "Building Collapse" };
            string[] locations = { "Downtown", "Airport", "Suburbs", "Chemical Plant", "Harbor", "City Center", "Suburbs Region", "Old Town" };
            string[] difficulties = { "Easy", "Medium", "Hard" };

            int score = 0;

            Console.WriteLine("=== Emergency Simulation Game ===\n");


            string? mode = "";
            while (true)
            {
                Console.Write("Select mode: (A)utomatic or (M)anual? ");
                mode = Console.ReadLine()?.Trim().ToLower();

                if (mode == "a" || mode == "m")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("❌ Invalid selection. Please select 'A' for Automatic or 'M' for Manual.");
                }
            }

            bool isManual = mode == "m";

            for (int round = 1; round <= 5; round++)
            {
                Console.WriteLine($"\n--- Round {round} ---");

                string type = incidentTypes[random.Next(incidentTypes.Length)];
                string location = locations[random.Next(locations.Length)];
                string difficulty = difficulties[random.Next(difficulties.Length)];

                Incident incident = new Incident(type, location, difficulty);
                Console.WriteLine($"Incident: {type} at {location} | Difficulty: {difficulty}");

                EmergencyUnit? selectedUnit = null;

                if (isManual)
                {
                    Console.WriteLine("\nAvailable Units:");
                    for (int i = 0; i < units.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {units[i].Name} (Speed: {units[i].Speed})");
                    }

                    Console.Write("Enter unit number: ");
                    if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= units.Count)
                    {
                        selectedUnit = units[index - 1];
                        if (!selectedUnit.CanHandle(incident.Type))
                        {
                            Console.WriteLine("❌ Wrong unit selected. -5 points.");
                            score -= 5;
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. -5 points.");
                        score -= 5;
                        continue;
                    }
                }
                else // Automatic Mode
                {
                    foreach (var unit in units)
                    {
                        if (unit.CanHandle(incident.Type))
                        {
                            selectedUnit = unit;
                            break;
                        }
                    }

                    if (selectedUnit == null)
                    {
                        Console.WriteLine("❌ No unit available to handle this incident. -5 points.");
                        score -= 5;
                        continue;
                    }
                }

                int basePoints = difficulty switch
                {
                    "Easy" => 10,
                    "Medium" => 15,
                    "Hard" => 20,
                    _ => 10
                };

                int responseTime = 100 / selectedUnit.Speed;
                Thread.Sleep(responseTime * 100); // simulate time delay

                selectedUnit.RespondToIncident(incident, responseTime);
                Console.WriteLine($"✅ Response successful. +{basePoints} points.");
                score += basePoints;
            }

            Console.WriteLine($"\n=== Simulation Complete ===\nFinal Score: {score}\nThanks for playing!");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();

        }
    }
}
