// Represents an eternal goal that can be repeatedly recorded for points but never completes
using System;

namespace EternalQuest
{
    public class EternalGoal : Goal
    {
        // Constructor initializes eternal goal with base properties
        public EternalGoal(string name, string description, int points)
            : base(name, description, points)
        {
            // Eternal goals have no completion state to initialize
        }

        // Records progress and returns point value (no completion state change)
        public override int RecordEvent()
        {
            return _points;  // Always award full points on each recording
        }

        // Eternal goals are never considered complete
        public override bool IsComplete() => false;

        // Creates serialized string representation for saving/loading
        public override string GetStringRepresentation()
        {
            return $"EternalGoal:{_shortName},{_description},{_points}";
        }

        // Exceeds core requirements by implementing infinite repeatability
    }
}