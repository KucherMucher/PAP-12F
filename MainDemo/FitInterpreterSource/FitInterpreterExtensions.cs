using System;
using System.Collections.Generic;

namespace FitInterpreterSource.FitInterpreterExtensions
{
	public static class FitIntExt
	{
		public static string GetTimeHMS(this int totalSeconds)
		{
			int hours = totalSeconds / 3600;
			int minutes = (totalSeconds % 3600) / 60;
			int seconds = totalSeconds % 60;
			return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
		}

        public static string GetTimeMS(int totalSeconds)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        public static string ToMinHM(this double speedMetersPerSecond) // to minutes / 100m
        {
            if (speedMetersPerSecond <= 0) return "0:00";

            // Convert speed (m/s) to seconds per 100m
            int totalSeconds = (int)Math.Round(100.0 / speedMetersPerSecond);

            return GetTimeMS(totalSeconds);
        }

        public static string ToMinKm(this double speedMetersPerSecond) // to minutes / km
        {
            if (speedMetersPerSecond <= 0) return "0:00";

            // Convert speed (m/s) to minutes per km
            int totalSeconds = (int)Math.Round(1000.0 / speedMetersPerSecond);

            return GetTimeMS(totalSeconds);
        }

        public static string ToKmH(this double speedMetersPerSecond) // to km/h
        {
            if (speedMetersPerSecond <= 0) return "0:00";

            // Convert speed (m/s) to km/h
            int pace = (int)Math.Round(speedMetersPerSecond * 3.6);

            return $"{pace}";
        }



        public static ActivityType GetActivityType(this string str)
        {
            ActivityType activityType = ActivityType.Unknown;

            if (string.IsNullOrWhiteSpace(str)) return activityType;
            
            //var parts = line.Split(',');
            
            //if (parts.Length < 2) return activityType;

            if (Enum.TryParse<ActivityType>(str.Trim(), ignoreCase: true, out var result))
            {
                return result;
            }

            return activityType;
        }

        public static string GetSpeedByActivityType(double speed, ActivityType activityType) {
            switch (activityType)
            {
                case ActivityType.Running:
                    return (Math.Round(speed, 3)).ToMinKm();

                case ActivityType.Cycling:
                    return (Math.Round(speed, 3)).ToKmH();

                case ActivityType.Swimming:
                    return (Math.Round(speed, 3)).ToMinHM();

                default:
                    return $"0";
            }
        }

    }

    public enum ActivityType
    {
        Running,
        Cycling,
        Swimming,
        Unknown
    }

    public enum ScoreMethod
    {
        Pace,           // based on pace
        Heartrate,      // based on heartrate
        Power,          // based on power values from activity
        Session,        // based on subjective rpe avaluation
        None
    }

    //namespace FitInterpreterExtensions.Enums
}
