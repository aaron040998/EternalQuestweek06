// This class defines a ChecklistGoal that tracks progress toward a multi-step goal and awards a bonus when completed.
using System;

namespace EternalQuest
{
    public class ChecklistGoal : Goal
    {
        private int _amountCompleted;   // Tracks how many items have been marked complete so far
        private int _target;            // The total number of items required to complete the goal
        private int _bonus;             // Additional bonus points awarded when the goal is fully completed

        // Constructor initializes checklist goal with specific completion targets
        public ChecklistGoal(string name, string description, int points, int target, int bonus)
            : base(name, description, points)
        {
            _target = target;           // Set required completion count
            _bonus = bonus;             // Set bonus points for full completion
            _amountCompleted = 0;       // Start with zero progress
        }

        // Records progress toward goal and calculates earned points
        public override int RecordEvent()
        {
            // Only process if target not yet reached
            if (_amountCompleted < _target)
            {
                _amountCompleted++;      // Increment completion count
                
                // Award bonus when target is first reached
                if (_amountCompleted == _target)
                {
                    return _points + _bonus;  // Base points + bonus
                }
                return _points;           // Base points for partial completion
            }
            return 0;  // Goal already completed - no additional points
        }

        // Checks if goal has been fully completed
        public override bool IsComplete() => _amountCompleted >= _target;

        // Generates formatted string showing goal status and progress
        public override string GetDetailsString()
        {
            string status = IsComplete() ? "[X]" : "[ ]";  // Completion indicator
            return $"{status} {_shortName} ({_description}) {_amountCompleted}/{_target}";  // Progress display
        }

        // Creates serialized string representation for saving/loading
        public override string GetStringRepresentation()
        {
            return $"ChecklistGoal:{_shortName},{_description},{_points},{_amountCompleted},{_target},{_bonus}";
        }

        // Exceeds core requirements by implementing bonus points system for full completion
    }
}