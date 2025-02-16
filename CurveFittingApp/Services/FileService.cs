using System.IO;
using CurveFittingApp.Models;

namespace CurveFittingApp.Services
{
    /// <summary>
    /// Provides methods for loading data points from a file.
    /// </summary>
    public class FileService : IFileService
    {
        /// <summary>
        /// Loads data points from a specified file.
        /// </summary>
        /// <param name="filePath">The path to the file containing the data points.</param>
        /// <returns>A list of data points loaded from the file.</returns>
        public List<DataPointModel> LoadData(string filePath)
        {
            var dataPoints = new List<DataPointModel>();
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length != 2) continue;

                if (double.TryParse(parts[0], out double x) &&
                    double.TryParse(parts[1], out double y))
                {
                    dataPoints.Add(new DataPointModel { X = x, Y = y });
                }
            }

            return dataPoints;
        }
    }
}