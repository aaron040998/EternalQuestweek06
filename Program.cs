// Entry point for Eternal Quest goal tracking application

/*
 * EXCEEDS REQUIREMENTS:
 * Implemented streak tracking system to reward consistent engagement.
 * - Tracks consecutive days with recorded events
 * - Persists streak data across sessions
 * - Displays current streak in player info
 * This enhances gamification by adding behavioral psychology elements.
 */
using System;

namespace EternalQuest
{
    class Program
    {
        // Main application entry point
        static void Main(string[] args)
        {
            // Initialize goal manager with streak tracking feature
            var manager = new GoalManager();
            
            // Start main program loop
            manager.Start();
            
            // Note: Exceeds core requirements by implementing streak tracking
            // Creative Feature: Tracks consecutive-day streaks to encourage daily engagement
        }
    }
}