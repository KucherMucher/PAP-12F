using System;
using ScottPlot;

namespace FitInterpreterSource
{
    public class FitGrapher
    {
        public int imgW { get; set; }
        public int imgH { get; set; }

        public List<double> timePoints = new List<double>();
        public List<double> pacePoints = new List<double>();
        public List<double> powerPoints = new List<double>();
        public List<double> cadencePoints = new List<double>();
        public double limit = 0.0;

        private string projectDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..");

        public FitGrapher(int width = 800, int height = 400)
        {
            imgW = width;
            imgH = height;
        }

        public void ExportGraph(string path)
        {
            if (timePoints == null || pacePoints == null)
                throw new ArgumentNullException("Data list cannot be null.");

            if (timePoints.Count == 0 || timePoints.Count != timePoints.Count)
                throw new ArgumentException("Time and pace lists must contain data and be of equal length.");

            Plot plot = new Plot();

            var scatter = plot.Add.Scatter(timePoints.ToArray(), pacePoints.ToArray());
            scatter.LineWidth = 2;
            scatter.MarkerSize = 4;

            var limitLine = plot.Add.HorizontalLine(limit);

            limitLine.Color = Colors.Red;
            limitLine.LineWidth = 2;

            plot.Title("Graph");
            plot.XLabel("seconds");
            plot.YLabel("pace");

            string outputPath = Path.Combine(projectDir, path);

            plot.SavePng(outputPath, imgW, imgH);
        }

        public List<double> AddPointsToGraph(List<double> dataList)
        {
            List<double> graphList = new List<double>();

            if (dataList == null)
                return [0];
            foreach (var data in dataList)
            {
                if (data <= 0.0)
                {
                    graphList.Add(0);
                    continue;
                }

                graphList.Add(data);
            }

            return graphList;
        }

        public void AddTimePoints(List<double> timeList)
        {
            if (timeList == null)
                throw new ArgumentNullException("Time list cannot be null.");
            timePoints = AddPointsToGraph(timeList);
        }

        public void AddPacePoints(List<double> paceList)
        {
            if (paceList == null)
                throw new ArgumentNullException("Pace list cannot be null.");
            pacePoints = AddPointsToGraph(paceList);
        }

        public void AddPowerPoints(List<double> powerList)
        {
            if (powerList == null)
                throw new ArgumentNullException("Power list cannot be null.");
            powerPoints = AddPointsToGraph(powerList);
        }

        public void AddCadencePoints(List<double> cadenceList)
        {
            if (cadenceList == null)
                throw new ArgumentNullException("Cadence list cannot be null.");
            cadencePoints = AddPointsToGraph(cadenceList);
        }
    } 
}