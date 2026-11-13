using System;

//using Dynastream.Fit;

namespace FitInterpreterSource
{
    using FitInterpreterExtensions;
    using PaceCutter;
    //using FitGrapher;

    using static ListShenanigans.ListSharingan;

    public class FitInterpreter
    {
        private List<List<string>> rawData = new List<List<string>>();
        public FitInfo info;
        public FitScore score;
        public FitGrapher graph = new FitGrapher(1000, 500);

        // public variables to show: 
        //   FitInfo, FitScore, FitGrapher (for now not because its just gonna export an image)

        public FitInterpreter(List<List<string>> data)
        {
            Console.WriteLine("FIT Interpreter");

            rawData = data;

            //StreamReader reader = null;

            List<string> activityInfo = null;
            List<string> activityLine = null;
            List<List<string>> activityData = new List<List<string>>(); // list of lists to save each line of csv file

            ActivityType activityType = ActivityType.Unknown;

            try
            {
                Console.WriteLine("opening file");
                //using StreamReader reader = new(args[0]);
                //int n = 0;
                //bool gotInfo = false;

                // rewrite this shit because i now pass listlist
                // what do we get from this loop:
                // activityType, activityInfo (column names), activityData (data)
                /*foreach(var line in rawData)
                {
                    var line = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) // if line is empty
                    {
                        continue;
                    }

                    if (line.StartsWith("#")) // file info, also maybe well get distance from here?
                    {
                        activityType = line.GetActivityType();
                        continue;
                    }

                    if (!gotInfo)
                    {
                        activityInfo = new List<string>(line.Split(',')); // this list contains collumns of the info on each line
                                                                          //activityInfo.Insert(0, n);
                        gotInfo = true;
                        n = 0;

                        /*foreach (string item in activityInfo)
                        {
                            Console.WriteLine(item);
                        }

                        continue;
                    }
                    else
                    {
                        activityLine = new List<string>(line.Split(','));
                        activityData.Add(activityLine);
                    }
                    // I could right there calculate the sum of all distances, speeds etc but i wont because i want to isolate the calcs 
                    n++;
                }

                reader.Close();*/
                
                // since we now that everytime the table the same, we can get use the exact position of the items we need.
                activityType = rawData[0][1].GetActivityType();
                Console.WriteLine(rawData[0][1]);
                activityInfo = rawData[1];
                activityData = rawData.GetRange(2, rawData.Count - 2);

				// now comes the calc part :=) ig? or maybe this is a part where we organize the data?
				//double hrSum = 0;
				//double speedSum = 0;
				//int nHr = 0;
				//int nSpeed = 0;

				int rawTime = 0;
				double speed = 0.0;
                double avrgHr = 0.0;
                double distance = 0.0;
                double avrgPower = 0.0;

                string avrgSpeed = "0'";
                string time = "0'";

                List<double> timeGraph = new List<double>();
                List<double> paceGraph = new List<double>();
                List<double> powerGraph = new List<double>();

				List<double> paceList = new List<double>();
                List<double> powerList = new List<double>();
                //List<double> cadenceList = new List<double>();
                /*foreach (List<string> line in activityData) // change this into a get something list functions
                {   
                    // heartrate
                    if (string.IsNullOrWhiteSpace(line[heartRateIndex]) || Convert.ToDouble(line[heartRateIndex]) <= 0.0)
                        continue;

                    hrSum += Convert.ToDouble(line[heartRateIndex]);
                    nHr++;

                    // pace
                    if (string.IsNullOrWhiteSpace(line[speedIndex]) || Convert.ToDouble(line[speedIndex]) <= 0.0)
                    {
                        paceGraph.Add(0);
                        continue;
                    }

                    speed = Convert.ToDouble(line[speedIndex]);
                    speedSum += speed;
                    nSpeed++;

                    paceGraph.Add(speed);
                    timeGraph.Add(Convert.ToDouble(line[secondsIndex]));

                    // power
                    if (string.IsNullOrWhiteSpace(line[powerIndex]) || Convert.ToDouble(line[powerIndex]) <= 0.0)
                        continue;

                }*/

                // get relative position to important infos
                // do things if we find the indexes, if not ignore.
                // like if we find distance we take distance info, pace, make avrg pace etc.

                // are those comments even needed???? ^^^^^^


                if (activityInfo is null || activityInfo.Count == 0)
                {
                    Console.WriteLine("The file contains no activity header.");
                    return;
                }

                int distanceIndex = FindColumn(activityInfo, "Distance");
                int speedIndex = FindColumn(activityInfo, "Speed");
                int cadenceIndex = FindColumn(activityInfo, "Cadence");
                int powerIndex = FindColumn(activityInfo, "Power");
                int altitudeIndex = FindColumn(activityInfo, "Altitude");
                int heartRateIndex = FindColumn(activityInfo, "HeartRate");

                int secondsIndex = FindColumn(activityInfo, "Seconds");
                int timerEventIndex = FindColumn(activityInfo, "TimerEvent");
                int lapIndex = FindColumn(activityInfo, "Lap");

                if (activityData.Count == 0)
                {
                    Console.WriteLine("The file contains no activity data.");
                    return;
                }

                List<string> lastRecord = activityData[^1];

                // distace
                if (distanceIndex >= 0)
                    distance = Math.Round(Convert.ToDouble(lastRecord[distanceIndex]));

                // speed
                PaceCutterClass cut = new PaceCutterClass();
                if (speedIndex >= 0)
                {
                    double limit = cut.PaceCut(GetListFromActivityData(activityData, speedIndex));
                    graph.limit = limit;
                    paceList = GetFilteredByValueListFromActivityData(activityData, speedIndex, limit, true);
                    //Console.Write(paceList[^1]);
                    avrgSpeed = FitIntExt.GetSpeedByActivityType(GetAverageFromList(paceList), activityType);
                    graph.AddPacePoints(GetListFromActivityData(activityData, speedIndex));

                }

                int o = 0;
                // cadence
                if (cadenceIndex >= 0)
                    o = 0;

                // power
                if (powerIndex >= 0)
                {
                    powerList = GetFilteredByValueListFromActivityData(activityData, powerIndex, 0, true);
                    avrgPower = GetAverageFromList(powerList);
					graph.AddPowerPoints(powerList);
				}
                //Console.Write(" " + powerList[^1]);

                // altitude
                if (altitudeIndex >= 0)
                    o = 0;

                // heartrate
                if (heartRateIndex >= 0)
                    avrgHr = Math.Round(GetFilteredAverageFromActivityData(activityData, heartRateIndex, 30, true));

                // time
                if (secondsIndex >= 0)
                {
                    rawTime = Convert.ToInt32(lastRecord[secondsIndex]);
                    time = rawTime.GetTimeHMS();
                    graph.AddTimePoints(GetListFromActivityData(activityData, secondsIndex));
                }


                /*switch (activityType)
                {
                    case "running":
                        avrgSpeed = (Math.Round(speedSum / nSpeed, 3)).ToMinKm();
                        break;
                    case "cycling":
                        avrgSpeed = (Math.Round(speedSum / nSpeed, 3)).ToKmH();
                        break;
                    case "swimming":
                        avrgSpeed = (Math.Round(speedSum / nSpeed, 3)).ToMinHM();
                        break;
                    default:
                        avrgSpeed = null;
                        break;
                }*/

                // here we give things to the all the classes.
                /*Console.Write("Activity Type: " + activityType);
                Console.Write("\nAverage Heart Rate: " + avrgHr);
                Console.Write("\nDistance: " + distance);
                Console.Write("\nTime: " + time);
                Console.Write("\nAverage Speed: " + avrgSpeed);
                Console.Write("\n" + powerIndex + " " + speedIndex);

                //Console.Write("\n" + timeGraph.Count);
                //Console.Write("\n" + paceGraph.Count);*/

                info = new FitInfo(activityType, distance, time, avrgSpeed, avrgHr, avrgPower);
                score = new FitScore(activityType, paceList, powerList, avrgHr, 7, rawTime);

                /*Console.Write("\nIF: " + score.IntensityFactor);
                Console.Write("\nTSS: " + score.TrainingSessionScore);

                Console.Write("\npaceIF: " + score.paceIntensityFactor);
                Console.Write("\npaceTSS: " + score.paceTrainingSessionScore);

                Console.Write("\nhrIF: " + score.hrIntensityFactor);
                Console.Write("\nhrTSS: " + score.hrTrainingSessionScore);

                Console.Write("\nrpeIF: " + score.rpeIntensityFactor);
                Console.Write("\nrpeTSS: " + score.rpeTrainingSessionScore);

                //Console.Write("\nrpeIF: " + score.paceList[^1]);
                //Console.Write("\nrpeTSS: " + score.powerList[^1]);*/

                /*try
                {
                    graph.ExportGraph(timeGraph, paceGraph, "pace_analysis.png");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to generate graph: {ex.Message}");
                }*/
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    } 
}
