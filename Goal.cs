// Abstract base class defining common structure and behavior for all goal types
using System;

namespace EternalQuest
{
    public abstract class Goal
    {
        protected string _shortName;     // Short identifier for the goal
        protected string _description;    // Detailed description of the goal
        protected int _points;            // Point value awarded for progress

        // Public read-only accessor for short name
        public string ShortName => _shortName;

        // Constructor initializes common goal properties
        protected Goal(string name, string description, int points)
        {
            _shortName = name;
            _description = description;
            _points = points;
        }

        // Abstract method to record goal progress (implemented in derived classes)
        public abstract int RecordEvent();

        // Abstract method to check completion status (implemented in derived classes)
        public abstract bool IsComplete();

        // Virtual method provides default display format for goals
        public virtual string GetDetailsString()
        {
            string status = IsComplete() ? "[X]" : "[ ]";  // Completion indicator
            return $"{status} {_shortName} ({_description})";  // Basic goal display
        }

        // Abstract method for serialization (implemented in derived classes)
        public abstract string GetStringRepresentation();

        // Demonstrates polymorphism - each goal type implements unique behavior
    }
}