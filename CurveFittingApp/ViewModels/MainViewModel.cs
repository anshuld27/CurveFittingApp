using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Win32;
using CurveFittingApp.Models;
using CurveFittingApp.Services;
using OxyPlot;
using OxyPlot.Series;

namespace CurveFittingApp.ViewModels
{
    /// <summary>
    /// ViewModel for the main window, handling data loading, fitting, and plotting.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IFileService _fileService;
        private readonly ICurveFittingService _curveFittingService;
        private List<DataPointModel> _dataPoints;
        private FitModelType _selectedModel;
        private PlotModel _plotModel;
        private double _a, _b;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// <param name="fileService">The file service for loading data.</param>
        /// <param name="curveFittingService">The curve fitting service for fitting data.</param>
        public MainViewModel(IFileService fileService, ICurveFittingService curveFittingService)
        {
            _fileService = fileService;
            _curveFittingService = curveFittingService;
            LoadDataCommand = new RelayCommand(LoadData);
            FitDataCommand = new RelayCommand(FitData, CanFitData);
            PlotModel = new PlotModel { Title = "Curve Fitting" };
        }

        /// <summary>
        /// Command to load data from a file.
        /// </summary>
        public ICommand LoadDataCommand { get; }

        /// <summary>
        /// Command to fit the loaded data to the selected model.
        /// </summary>
        public ICommand FitDataCommand { get; }

        /// <summary>
        /// Gets the available fit model types.
        /// </summary>
        public IEnumerable<FitModelType> FitModelTypes => Enum.GetValues(typeof(FitModelType)).Cast<FitModelType>();

        /// <summary>
        /// Gets or sets the selected fit model type.
        /// </summary>
        public FitModelType SelectedModel
        {
            get => _selectedModel;
            set { _selectedModel = value; OnPropertyChanged(nameof(SelectedModel)); }
        }

        /// <summary>
        /// Gets or sets the plot model for displaying data and fitted curves.
        /// </summary>
        public PlotModel PlotModel
        {
            get => _plotModel;
            set { _plotModel = value; OnPropertyChanged(nameof(PlotModel)); }
        }

        /// <summary>
        /// Gets or sets the parameter 'a' of the fitted model.
        /// </summary>
        public double A
        {
            get => _a;
            set { _a = value; OnPropertyChanged(nameof(A)); }
        }

        /// <summary>
        /// Gets or sets the parameter 'b' of the fitted model.
        /// </summary>
        public double B
        {
            get => _b;
            set { _b = value; OnPropertyChanged(nameof(B)); }
        }

        /// <summary>
        /// Loads data from a file selected by the user.
        /// </summary>
        /// <param name="parameter">The command parameter (not used).</param>
        public void LoadData(object parameter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select a Data File",
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _dataPoints = _fileService.LoadData(openFileDialog.FileName);
                    if (_dataPoints.Count == 0)
                    {
                        System.Windows.MessageBox.Show("The file is empty or incorrectly formatted.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                        return;
                    }

                    UpdatePlotWithDataPoints();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error loading file: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Determines whether the FitData command can be executed.
        /// </summary>
        /// <param name="parameter">The command parameter (not used).</param>
        /// <returns>True if data points are loaded; otherwise, false.</returns>
        private bool CanFitData(object parameter) => _dataPoints != null && _dataPoints.Count > 0;

        /// <summary>
        /// Fits the loaded data to the selected model and updates the plot.
        /// </summary>
        /// <param name="parameter">The command parameter (not used).</param>
        private void FitData(object parameter)
        {
            try
            {
                (A, B) = _curveFittingService.FitData(_dataPoints, SelectedModel);
                PlotFittedCurve();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error fitting data: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Updates the plot with the loaded data points.
        /// </summary>
        private void UpdatePlotWithDataPoints()
        {
            PlotModel.Series.Clear();
            var scatterSeries = new ScatterSeries { Title = "Data Points", MarkerType = MarkerType.Circle };

            foreach (var point in _dataPoints)
                scatterSeries.Points.Add(new ScatterPoint(point.X, point.Y));

            PlotModel.Series.Add(scatterSeries);
            PlotModel.InvalidatePlot(true);
        }

        /// <summary>
        /// Plots the fitted curve based on the selected model and fitted parameters.
        /// </summary>
        private void PlotFittedCurve()
        {
            if (_dataPoints == null || _dataPoints.Count == 0) return;

            var fittedSeries = new LineSeries { Title = "Fitted Curve", Color = OxyColors.Red };

            foreach (var point in _dataPoints.OrderBy(p => p.X))
            {
                double y = SelectedModel switch
                {
                    FitModelType.Linear => A * point.X + B,
                    FitModelType.Exponential => A * Math.Exp(B * point.X),
                    FitModelType.Power => A * Math.Pow(point.X, B),
                    _ => 0
                };
                fittedSeries.Points.Add(new DataPoint(point.X, y));
            }

            if (PlotModel.Series.Count > 1)
                PlotModel.Series.RemoveAt(1); // Remove previous fitted curve if exists

            PlotModel.Series.Add(fittedSeries);
            PlotModel.InvalidatePlot(true);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    /// <summary>
    /// A command that relays its execution to a specified action.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        /// <param name="canExecute">The predicate to determine if the command can execute.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determines whether the command can execute.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>True if the command can execute; otherwise, false.</returns>
        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}


