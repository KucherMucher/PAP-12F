using System;
using System.Collections.Generic;

// how to fix bugs here - so like the list that is passed is a type of list list string, so change it. 
// also - because its a string, use strng.isnullorwhitespace to check if empty

// type shit

namespace FitInterpreterSource.ListShenanigans
{
    public static class ListSharingan
    {
        public static List<double> GetListFromActivityData(List<List<string>> activityData, int index)
        {
            List<double> list = new List<double>();
            /*foreach(line in listList)
            {
                if (string.IsNullOrWhiteSpace(line[index]))
                    continue;

                list.Add(line[index]);
            }*/

            for(int i = 0; i < activityData.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(activityData[i][index]))
                    continue;

                list.Add(Convert.ToDouble(activityData[i][index]));
            }

            return list;
        }

        public static List<double> FilterListByValue(List<double> list, double referenceValue, bool greater)
        {
            List<double> newList = new List<double>();
            for (int i = 0; i < list.Count; i++)
            {
                bool match = greater ? list[i].CompareTo(referenceValue) > 0 : list[i].CompareTo(referenceValue) < 0;
                if (match)
                    newList.Add(list[i]);
            }

            return newList;
        }

        public static List<double> GetFilteredByValueListFromActivityData(List<List<string>> activityData, int index, double referenceValue, bool greater)
        {
            List<double> list = new List<double>();
            list = GetListFromActivityData(activityData, index);

            list = FilterListByValue(list, referenceValue, greater);

            return list;

        }

        public static double GetAverageFromList(List<double> list)
        {
            double sum = 0;
            /*foreach(element in list)
            {
                sum += Convert.ToDouble(element);
            }*/

            for (int i = 0; i < list.Count; i++)
            {
                sum += Convert.ToDouble(list[i]);
            }

            return sum / list.Count;

        }

        public static double GetFilteredAverageFromList(List<double> list, double referenceValue, bool greater)
        {
            List<double> filteredList = new List<double>();

            filteredList = FilterListByValue(list, referenceValue, greater);

            return GetAverageFromList(filteredList);
        }

        public static double GetAverageFromActivityData(List<List<string>> activityData, int index)
        {
            List<double> list = new List<double>();

            list = GetListFromActivityData(activityData, index);

            return GetAverageFromList(list);
        }

        public static double GetFilteredAverageFromActivityData(List<List<string>> activityData, int index, double referenceValue, bool greater)
        {
            List<double> list = new List<double>();

            list = GetFilteredByValueListFromActivityData(activityData, index, referenceValue, greater);

            return GetAverageFromList(list);
        }

        public static int FindColumn(List<string> headers, string columnName)
        {
            return headers.FindIndex(
                header => string.Equals(
                    header?.Trim(),
                    columnName,
                    StringComparison.OrdinalIgnoreCase));
        }

        /*public static List<List<string>> ListToListList(this List<string> list)
        {
            List<List<string>> listList = new List<List<string>>();
            foreach(string element in list)
            {
                listList.Add(element.Split(","))
            }
        }*/
    }
}

