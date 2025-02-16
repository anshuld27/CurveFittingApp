using Moq;
using Xunit;
using CurveFittingApp.ViewModels;
using CurveFittingApp.Services;
using CurveFittingApp.Models;

namespace CurveFittingApp.Tests
{
    /// <summary>
    /// Unit tests for the MainViewModel class.
    /// </summary>
    public class MainViewModelTests
    {
        private readonly Mock<IFileService> _mockFileService;
        private readonly Mock<ICurveFittingService> _mockCurveFittingService;
        private readonly MainViewModel _viewModel;
        private readonly List<DataPointModel> _testData = new()
        {
            new() { X = 1, Y = 2 },
            new() { X = 2, Y = 4 }
        };

        /// <summary>
        /// Initializes a new instance of the MainViewModelTests class.
        /// Sets up mock services and initializes the MainViewModel.
        /// </summary>
        public MainViewModelTests()
        {
            _mockFileService = new Mock<IFileService>();
            _mockFileService.Setup(s => s.LoadData(It.IsAny<string>())).Returns(_testData);

            _mockCurveFittingService = new Mock<ICurveFittingService>();
            _mockCurveFittingService.Setup(s => s.FitData(It.IsAny<List<DataPointModel>>(), FitModelType.Linear))
                .Returns((2.0, 0.0));

            _viewModel = new MainViewModel(_mockFileService.Object, _mockCurveFittingService.Object);
        }

        /// <summary>
        /// Tests that the FitDataCommand cannot execute when no data is loaded.
        /// </summary>
        [Fact]
        public void FitDataCommand_ShouldNotExecuteWhenNoData()
        {
            var canExecute = _viewModel.FitDataCommand.CanExecute(null);
            Assert.False(canExecute);
        }
    }
}


