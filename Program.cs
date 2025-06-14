// Entry point for Eternal Quest goal tracking application
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