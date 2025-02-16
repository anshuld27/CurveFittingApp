using CurveFittingApp.Models;

namespace CurveFittingApp.Services
{
    /// <summary>
    /// Defines methods for loading data points from a file.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Loads data points from a specified file.
        /// </summary>
        /// <param name="filePath">The path to the file containing the data points.</param>
        /// <returns>A list of data points loaded from the file.</returns>
        List<DataPointModel> LoadData(string filePath);
    }
}

