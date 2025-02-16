using MathNet.Numerics;
using CurveFittingApp.Models;

namespace CurveFittingApp.Services
{
    /// <summary>
    /// Provides methods for fitting data points to different types of models.
    /// </summary>
    public class CurveFittingService : ICurveFittingService
    {
        /// <summary>
        /// Fits the given data points to the specified model type.
        /// </summary>
        /// <param name="data">The list of data points to fit.</param>
        /// <param name="modelType">The type of model to fit the data to.</param>
        /// <returns>A tuple containing the parameters of the fitted model.</returns>
        /// <exception cref="ArgumentException">Thrown when an invalid fitting model is specified.</exception>
        public (double a, double b) FitData(List<DataPointModel> data, FitModelType modelType)
        {
            // Extract X and Y data from the data points
            var xData = data.ConvertAll(d => d.X).ToArray();
            var yData = data.ConvertAll(d => d.Y).ToArray();

            switch (modelType)
            {
                case FitModelType.Linear:
                    // Perform linear fit
                    var linearFit = Fit.Line(xData, yData);
                    return (linearFit.Item2, linearFit.Item1); // a, b

                case FitModelType.Exponential:
                    // Perform exponential fit by taking the log of Y data
                    var logY = new double[yData.Length];
                    for (int i = 0; i < yData.Length; i++)
                        logY[i] = Math.Log(yData[i]);

                    var expFit = Fit.Line(xData, logY);
                    return (Math.Exp(expFit.Item2), expFit.Item1);

                case FitModelType.Power:
                    // Perform power fit by taking the log of both X and Y data
                    var logX = new double[xData.Length];
                    var logYPower = new double[yData.Length];
                    for (int i = 0; i < xData.Length; i++)
                    {
                        logX[i] = Math.Log(xData[i]);
                        logYPower[i] = Math.Log(yData[i]);
                    }

                    var powerFit = Fit.Line(logX, logYPower);
                    return (Math.Exp(powerFit.Item2), powerFit.Item1);

                default:
                    throw new ArgumentException("Invalid fitting model.");
            }
        }
    }
}
