# CurveFittingApp

CurveFittingApp is a WPF application that allows users to load data points from a file, fit the data to various mathematical models, and visualize the results. The application supports linear, exponential, and power models for curve fitting.

## Features

- Load data points from CSV files.
- Fit data to linear, exponential, and power models.
- Visualize data points and fitted curves using OxyPlot.
- Unit tests for core services.

## Installation

1. Clone the repository:
    git clone https://github.com/anshuld27/CurveFittingApp.git

2. Open the solution in Visual Studio 2022.

3. Restore the NuGet packages:
    dotnet restore

4. Build the solution:
    dotnet build


## Usage

1. Run the application:
    dotnet run --project CurveFittingApp

2. Use the "Load Data" button to select a CSV file containing data points.
3. Select a model type (Linear, Exponential, or Power) from the dropdown.
4. Click the "Fit Data" button to fit the data to the selected model and visualize the results.

## Project Structure

- `CurveFittingApp/`: Main application project.
  - `App.xaml.cs`: Application startup and dependency injection configuration.
  - `MainWindow.xaml.cs`: Main window logic.
  - `ViewModels/`: Contains the `MainViewModel` class for data binding and command handling.
  - `Services/`: Contains service classes for file loading and curve fitting.
  - `Models/`: Contains data models such as `DataPointModel` and `FitModelType`.

- `CurveFittingApp.Tests/`: Unit test project.
  - `CurveFittingServiceTests.cs`: Unit tests for the `CurveFittingService` class.
  - `FileServiceTests.cs`: Unit tests for the `FileService` class.
  - `MainViewModelTests.cs`: Unit tests for the `MainViewModel` class.

## Dependencies

- [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MathNet.Numerics](https://www.nuget.org/packages/MathNet.Numerics/)
- [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/)
- [OxyPlot](https://www.nuget.org/packages/OxyPlot.Core/)
- [Moq](https://www.nuget.org/packages/Moq/)
- [xUnit](https://www.nuget.org/packages/xunit/)

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
