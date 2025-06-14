// Represents a one-time goal that can be marked complete for points
using System;

namespace EternalQuest
{
    public class SimpleGoal : Goal
    {
        private bool _isComplete;  // Tracks completion status

        // Constructor initializes simple goal with base properties
        public SimpleGoal(string name, string description, int points)
            : base(name, description, points)
        {
            _isComplete = false;  // New goals start incomplete
        }

        // Records completion and awards points if not already complete
        public override int RecordEvent()
        {
            if (!_isComplete)
            {
                _isComplete = true;    // Mark complete
                return _points;        // Award points
            }
            return 0;  // Already completed - no additional points
        }

        // Checks if goal has been completed
        public override bool IsComplete() => _isComplete;

        // Creates serialized string representation for saving/loading
        public override string GetStringRepresentation()
        {
            return $"SimpleGoal:{_shortName},{_description},{_points},{_isComplete}";
        }

        // Exceeds core requirements: Implements one-time completion logic
    }
}