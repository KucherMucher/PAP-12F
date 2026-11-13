using System;
using static FitInterpreterSource.ListShenanigans.ListSharingan;


// old idea - use pace cutter to filter out the paces
// new idea - use pace cutter to set a limit for a filter inside FitInterpreter
namespace FitInterpreterSource.PaceCutter
{
    public class PaceCutterClass
    {
        //public double limit = 0.0;
        public PaceCutterClass()
        {

        }
        public double PaceCut(List<double> list)
        {
            if (list == null || list.Count == 0)
                return 0.0;

            double firstAverage = GetFilteredAverageFromList(list, 0, true);
            List<double> firstFilter = SpecificLoop(list, firstAverage);

            if (firstFilter.Count == 0)
                return 0.0;

            double secondAverage = GetFilteredAverageFromList(firstFilter, 0, true);
            /*List<double> secondFilter = SpecificLoop(firstFilter, secondAverage);

            List<double> finalList = new List<double>();

            foreach (double value in list)
            {
                if (!secondFilter.Contains(value))
                {
                    finalList.Add(value);
                }
            }

            return finalList;*/

            return secondAverage;
        }

        private static List<double> SpecificLoop(List<double> list, double value)
        {
            List<double> newList = new List<double>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] < value)
                {
                    newList.Add(list[i]);
                }
            }

            return newList;
        }
    }
}



/*
 The ideaa:

	grab a list and get its avarage.
	Take items from a list that are below avarage
	then, make and avarage of thos items and cut the items that are below the second avarage.
	for swimming, maybve even make 3 or 4 layers instead of 2.
	for cycling, we can make a limit for how gihn the pace can bu cut.

second idea:

	already kinda implemented, filter the list by not including minimum valuess (like dont include paces below 7 min / km or 5 km / h)

im gonna try both ideais and see which ine suits the interpreter better.
 */
