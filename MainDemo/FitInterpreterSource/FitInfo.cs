using System;
using System.Collections.Generic;
using System.Text;



namespace FitInterpreterSource
{
    using FitInterpreterExtensions;
    public class FitInfo
    {
        public ActivityType activityType;
        public double distance;
        public string totalTime;
        public string avgSpeed;
        public double avgHr;
        public double avgPower;
        public FitInfo(ActivityType activity, double dist, string time, string speed, double hr, double power)
        {
            activityType = activity;
            distance = dist;
            totalTime = time;
            avgSpeed = speed;
            avgHr = hr;
            avgPower = power;
        }

        public void ShowFitInfo()
        {
            Console.WriteLine($"Activity Type: {activityType}");
            Console.WriteLine($"Distance: {distance}");
            Console.WriteLine($"Total Time: {totalTime}");
            Console.WriteLine($"Average Speed: {avgSpeed}");
            Console.WriteLine($"Average Heart Rate: {avgHr}");
            Console.WriteLine($"Average Power: {avgPower}");
        }
    }

}
