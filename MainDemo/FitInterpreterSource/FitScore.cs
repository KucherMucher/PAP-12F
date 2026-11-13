using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using FitInterpreterSource.FitInterpreterExtensions;

using static FitInterpreterSource.ListShenanigans.ListSharingan;

namespace FitInterpreterSource
{
	public class FitScore
	{
		// varibales in future from other class : (Athlete Class)

		public double cFTP = 230;
		public double prFTP = 230;
		public double rFTPw = 3.33;
		public double sFTP = 0.91;

		public double cLTHR = 161;
		public double rLTHR = 171;
		public double sLTHR = 151;

		//public ScoreMethod prefferedMethod;
		//public double prefferedMethodValue;

		public ActivityType activityType { get; private set; }
		public ScoreMethod scoreMethod { get; private set; }

		//public double NormalizedP			{ get; private set; } // cuz its either Normalized Power or Normalized Graded Pace

		// power
		public double IntensityFactor { get; private set; }
		public double TrainingSessionScore { get; private set; }

		// pace
		public double paceIntensityFactor { get; private set; }
		public double paceTrainingSessionScore { get; private set; }

		// heartrate
		public double hrIntensityFactor { get; private set; }
		public double hrTrainingSessionScore { get; private set; }

		// session
		public double rpeIntensityFactor { get; private set; }
		public double rpeTrainingSessionScore { get; private set; }

		public List<double> paceList = new List<double>();
		public List<double> powerList = new List<double>();
		public double averageHeartRate;
		public int RPE;
		public int totalTime;


		//private bool isSwimming = false;

		public FitScore(ActivityType activity, List<double> paces, List<double> powers, double avrgHr, int rpe, int time) // for list... it will depend on the method used...
																																			  // fist method: pace - a list of paces as doubles would be given to then calculate the score
																																			  // second method: heartrate - a double of an avarage heartrate would be given to use 
																																			  // third method: power - a list of powers as doubles would be given
																																			  // forth method: session - user's subjective rpe value would be given to make the calculus
																																			  // if none is given, return every value as 0.

		// silly idea: just pass a list based on the method, with avarage heartrate being [value] :P // NO

		// make so that theres a preffered method.

		// brother gotta transform this shit into functions. I guess we can make 2 ways of calculating score : if alot of ways possible calc with medium ; if has preferences calc by preference only.

		/*So we have preffered way and brute force way.

		If preffered way, indicate preffered way OR just use preffered values? Maybe no because we gotta indicate maybe what tss was that? Ill look into that.

		If brute force way, give the lists of paces and powers, give average heart rate and subjective / average rpe. 
		After giving all the values, even if they are null/zero, try calculating tss for each one (ofc, dont calculate if its 0 or null). Then make and avarage tss. If theres no values except for rpe , calc by rpe. 


		Also another notice. If the athlete doesnt have manual ftp or lthr values, maybe calculate them by height and weight?*/
		{
			activityType = activity;
			//scoreMethod = method;
			paceList = paces;
			powerList = powers;
			averageHeartRate = avrgHr;
			RPE = rpe;
			totalTime = time;



			(IntensityFactor, TrainingSessionScore) = CalcPowerTSS(activityType, powerList, totalTime);
			(paceIntensityFactor, paceTrainingSessionScore) = CalcPaceTSS(activityType, paceList, totalTime);
			(hrIntensityFactor, hrTrainingSessionScore) = CalcHrTSS(activityType, averageHeartRate, totalTime);
			(rpeIntensityFactor, rpeTrainingSessionScore) = CalcRpeTSS(RPE, totalTime);

			/*if (TrainingSessionScore == paceTrainingSessionScore)
			{
				throw new Exception("FSSSSGG");
			}*/


		}
		/*double denominant = 0;
		double denominator = 1;

		switch (activityType, scoreMethod)
		{
			case (ActivityType.Running, ScoreMethod.Pace):
				denominant = CalcNP(list, totalTime);
				denominator = rFTPw;
				break;

            case (ActivityType.Running, ScoreMethod.Power):
                denominant = CalcNP(list, totalTime);
                denominator = rFTPw;
                break;

            case (ActivityType.Running, ScoreMethod.Heartrate):
				denominant = list[0];
				denominator = rLTHR;
                break;

            case (ActivityType.Swimming, ScoreMethod.Pace):
				denominant = list[0] / totalTime;
				denominator = 100 / sFTP;
				isSwimming = true;
                break;

            case (ActivityType.Swimming, ScoreMethod.Heartrate):
				denominant = list[0];
				denominator = sLTHR;
                break;

            case (ActivityType.Cycling, ScoreMethod.Power):
				denominant = CalcNP(list, totalTime);
				denominator = cFTP;
                break;

            case (ActivityType.Cycling, ScoreMethod.Heartrate):
				denominant = list[0];
				denominator = cLTHR;
                break;

            case (ActivityType.Cycling, ScoreMethod.Session):
            case (ActivityType.Running, ScoreMethod.Session):
            case (ActivityType.Swimming, ScoreMethod.Session):
                double rpe = list[0]; // in this case, the list passed was [rpe]
				IntensityFactor = 0.50 + (rpe * 0.06);
				break;

			default:
				NormalizedP = 0;
				IntensityFactor = 0;
				TrainingSessionScore = 0;
				break;

		}
		IntensityFactor = (scoreMethod == ScoreMethod.Session) ? IntensityFactor : CalcIF(denominant, denominator);

        TrainingSessionScore = CalcTSS(IntensityFactor, totalTime, isSwimming);

	}*/

		// just for future - we need to account for the paces that are really low (like 0 or 10 min/km)
		// so well send a filtered list with work paces that are above avarage + 1 below avarage (for running)
		// for cycling that would be just 0 or below 5 (5 is unrealistic)

		private double CalcNP(List<double> list, int totalTime) // in case of needing this function outside of class, set as public and make it return a tuple 
		{
			if (list is null || list.Count <= 1 || totalTime <= 0)
				return 0;

			// first, get the sum of s30^4
			double avarageRollingSum = 0; // S 30^4 sum
			double windowSum = 0;
			int windowCount = 0;

			for (int i = 0; i < list.Count; i++)
			{
				windowSum += list[i];

				if (i >= 30)
					windowSum -= list[i - 30];

				int currentWindowSize = Math.Min(i + 1, 30); // Calculate the current window size (up to 30)
				double rollingAvarage = windowSum / currentWindowSize; // Calculate the rolling average for the current window

				avarageRollingSum += Math.Pow(rollingAvarage, 4);
				windowCount++;
			}

			// second, get its avarage

			double avarageRolling = avarageRollingSum / windowCount;

			// third, calculate NGP by taking the 4th root

			double NP = Math.Pow(avarageRolling, 1.0 / 4.0);

			return NP;

		}

		private double CalcIF(double x, double y)
		{
			if (y <= 0) // in case of division by zero or by null go fuck yourself
				return 0;

			double IF = x / y;
			return IF;
		}

		private double CalcTSS(double IF, int totalTime, bool swimming = false)
		{
			double TSS = (Convert.ToDouble(totalTime) / 3600.0) * Math.Pow(IF, swimming ? 3 : 2) * 100.0;
			return TSS;
		}

		private (double, double) CalcPowerTSS(ActivityType activity, List<double> powerList, int totalTime)
		{
			if (powerList is null || powerList.Count <= 1 || totalTime <= 0)
				return (0, 0);

			double NP = CalcNP(powerList, totalTime);
			double denator = 1;

			switch (activity)
			{
				case ActivityType.Running:
					denator = prFTP;
					break;

				case ActivityType.Cycling:
					denator = cFTP;
					break;

				default:
					return (0, 0);
			}

			double IF = CalcIF(NP, denator);
			double TSS = CalcTSS(IF, totalTime);

			return (IF, TSS);
		}

		private (double, double) CalcPaceTSS(ActivityType activity, List<double> paceList, int totalTime)
		{
			if (paceList is null || paceList.Count <= 1 || totalTime <= 0)
				return (0, 0);

			double IF = 0;
			double TSS = 0;

			switch (activity)
			{
				case ActivityType.Running:
					double NP = CalcNP(paceList, totalTime);

					IF = CalcIF(NP, rFTPw);
					TSS = CalcTSS(IF, totalTime);

					return (IF, TSS);

				//case ActivityType.Cycling:
				//    break;

				case ActivityType.Swimming:
					double sWorkout = GetFilteredAverageFromList(paceList, 0, true);
					double sThreshold = 100 / sFTP;

					IF = CalcIF(sWorkout, sThreshold);
					TSS = CalcTSS(IF, totalTime, true);

					return (IF, TSS);

				default:
					return (0, 0);
			}
		}

		private (double, double) CalcHrTSS(ActivityType activity, double avrgHr, int totalTime)
		{
			if (avrgHr <= 0 || totalTime <= 0)
				return (0, 0);

			double denator = 1;
			switch (activity)
			{
				case ActivityType.Running:
					denator = rLTHR;
					break;

				case ActivityType.Cycling:
					denator = cLTHR;
					break;

				case ActivityType.Swimming:
					denator = sLTHR;
					break;

				default:
					return (0, 0);
			}

			double IF = CalcIF(avrgHr, denator);
			double TSS = CalcTSS(IF, totalTime);

			return (IF, TSS);

		}

		private (double, double) CalcRpeTSS(int rpe, int totalTime)
		{
			if (rpe <= 0 || totalTime <= 0)
				return (0, 0);

			rpe = Math.Clamp(rpe, 1, 10);

			double IF = 0.50 + (rpe * 0.06);
			double TSS = CalcTSS(IF, totalTime);

			return (IF, TSS);
		}

		/*private double CalcAvrgSpeed(List<double> list)
		{
			if (list.Count == 0 || list is null)
				return 0;

			double speedSum = 0;
			for (int i = 0; i < list.Count; i++)
			{
				speedSum += list[i];
			}

			return speedSum / list.Count;
		}*/

		public void ShowFitScore()
        {
            Console.WriteLine($"Activity Type: {activityType}");
            Console.WriteLine($"Intensity Factor: {IntensityFactor}");
            Console.WriteLine($"Training Session Score: {TrainingSessionScore}");
            Console.WriteLine($"Pace Intensity Factor: {paceIntensityFactor}");
            Console.WriteLine($"Pace Training Session Score: {paceTrainingSessionScore}");
            Console.WriteLine($"Heart Rate Intensity Factor: {hrIntensityFactor}");
            Console.WriteLine($"Heart Rate Training Session Score: {hrTrainingSessionScore}");
            Console.WriteLine($"RPE Intensity Factor: {rpeIntensityFactor}");
            Console.WriteLine($"RPE Training Session Score: {rpeTrainingSessionScore}");
        }
    } 
}


/*

The purpose of this file is to return IF and TSS of the workout. Independenetally of 

Methods for calculating per workout metrics

Running:
	Power:
		rIF with rNP and rFTPw
		rTSS with rIF
	Pace:
		rIF  with SNGP and SrFTPw
		rTSS with rIF

	HeartRate:
		HrIF  with rLTHR
		HrTSS def
	
Swimming:
	Pace:
		swIF with Sworkout and Sthreshold
		swTSS with swIF^3

	heartrate:
		HrIF with swLTHR
		HrTSS def

Cycling:
	Power:
		default IF
		default TSS

	No power:
		HeartRate:
			HrIF with cLTHR
			HrTSS def

			need: cLTHR (seconds), HR avrg

Method for everything:
	session rpe:
		sTSS

Calculus:
	IF = NP/FTP
	TSS = (t/3600) * IF^2 * 100

	HrIF = Hravrg/LTHR
	sTSS = (tmin/60) * (IFrpe)^2 * 100

	NP  = root4/avrg sum (P30)^4
	NGP = root4/avrg sum (S30)^4
	S30 = 

    Sworkout = totaldistance / totaltime
    Sthreshold = 100 / sFTP

	1 Step: Get each rolling 30-second avarage
	2 Step: Elevade each fo them to 4
	3 Step: Avarage the sum
	4 Step: take 4th root

Avarage values for avarage thriathlete:
	cFTP
	rFTPw
	swFTP

	cLTHR
	rLTHR
	swLTHR

References:

	NP (W)           - Normalized Power
	NGP (min/km)     - Normalized Ground Power

	IF               - Intensity Factor
	rIF              - running intensity factor
	swIF             - swimming intensity factor
	cIF              - cycling intensity factor
	HrIF             - heartrate intensity facotr
	IFrpe            - subjective effort multiplier

	TSS              - Training stress score
	rTSS             - running training stress score
	swTSS            - swimming training stress score
	cTSS             - cycling training stress score
	hrTSS            - heartrate training stress score
	sTSS             - session training stress score

	FTP              - Functional threshold power
	cFTP  (W)        - cycling functional threshold power
	rFTPw (min/km)   - Functional threshold run pace
	sFTP  (min/100m) - functional threshold swimming pace

	LTHR             - Lactate threshold heart rate
	cLTHR  (bpm)     - cycling lactate threshold heart rate
	rLTHR  (bpm)     - running lactate threshold heart rate
	swLTHR (bpm)     - swimming lactate threshold heart rate

	SNGP       (m/s) - NGP converted to speed 
	SrFTPw     (m/s) - rFTPw converted to speed
	Sworkout   (m/s) - Avarage workout speed
	Sthreshold (m/s) - Threshold Swim Speed

	P30 (W)			 - 30-Second Rolling Average Power
	S30 (km/h / m/s) - 30-Second Rolling Average Speed
	

 */