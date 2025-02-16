using CurveFittingApp.Models;

namespace CurveFittingApp.Services
{
    /// <summary>
    /// Defines methods for fitting data points to different types of models.
    /// </summary>
    public interface ICurveFittingService
    {
        /// <summary>
        /// Fits the given data points to the specified model type.
        /// </summary>
        /// <param name="data">The list of data points to fit.</param>
        /// <param name="modelType">The type of model to fit the data to.</param>
        /// <returns>A tuple containing the parameters of the fitted model.</returns>
        (double a, double b) FitData(List<DataPointModel> data, FitModelType modelType);
    }
}

