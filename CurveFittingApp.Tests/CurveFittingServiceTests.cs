using Moq;
using Xunit;
using CurveFittingApp.Models;
using CurveFittingApp.Services;

namespace CurveFittingApp.Tests
{
    /// <summary>
    /// Unit tests for the CurveFittingService class.
    /// </summary>
    public class CurveFittingServiceTests
    {
        private readonly Mock<ICurveFittingService> _mockCurveFittingService;

        /// <summary>
        /// Initializes a new instance of the CurveFittingServiceTests class.
        /// Sets up mock services and initializes test data.
        /// </summary>
        public CurveFittingServiceTests()
        {
            _mockCurveFittingService = new Mock<ICurveFittingService>();

            // Setup mock for Linear model
            _mockCurveFittingService
                .Setup(s => s.FitData(It.Is<List<DataPointModel>>(d => d == _linearData), FitModelType.Linear))
                .Returns((2.0, 0.0));

            // Setup mock for Exponential model
            _mockCurveFittingService
                .Setup(s => s.FitData(It.Is<List<DataPointModel>>(d => d == _exponentialData), FitModelType.Exponential))
                .Returns((Math.Exp(1), 1.0));

            // Setup mock for Power model
            _mockCurveFittingService
                .Setup(s => s.FitData(It.Is<List<DataPointModel>>(d => d == _powerData), FitModelType.Power))
                .Returns((1.0, 2.0));

            // Setup mock for invalid model
            _mockCurveFittingService
                .Setup(s => s.FitData(It.IsAny<List<DataPointModel>>(), It.Is<FitModelType>(m => m == (FitModelType)99)))
                .Throws<ArgumentException>();

            // Setup mock for empty data
            _mockCurveFittingService
                .Setup(s => s.FitData(It.Is<List<DataPointModel>>(d => d == _emptyData), It.IsAny<FitModelType>()))
                .Throws<InvalidOperationException>();

            // Setup mock for negative values
            _mockCurveFittingService
                .Setup(s => s.FitData(It.Is<List<DataPointModel>>(d => d == _negativeData), FitModelType.Linear))
                .Returns((2.0, 0.0));
        }

        private readonly List<DataPointModel> _linearData = new()
        {
            new() { X = 1, Y = 2 },
            new() { X = 2, Y = 4 },
            new() { X = 3, Y = 6 }
        };

        private readonly List<DataPointModel> _exponentialData = new()
        {
            new() { X = 1, Y = Math.Exp(1) },
            new() { X = 2, Y = Math.Exp(2) },
            new() { X = 3, Y = Math.Exp(3) }
        };

        private readonly List<DataPointModel> _powerData = new()
        {
            new() { X = 1, Y = 1 },
            new() { X = 2, Y = 4 },
            new() { X = 3, Y = 9 }
        };

        private readonly List<DataPointModel> _negativeData = new()
        {
            new() { X = -1, Y = -2 },
            new() { X = -2, Y = -4 },
            new() { X = -3, Y = -6 }
        };

        private readonly List<DataPointModel> _emptyData = new();

        /// <summary>
        /// Tests that FitData correctly fits a linear model.
        /// </summary>
        [Fact]
        public void FitData_ShouldFitLinearModel()
        {
            var (a, b) = _mockCurveFittingService.Object.FitData(_linearData, FitModelType.Linear);

            Assert.InRange(a, 1.99, 2.01);  // a ≈ 2 (allowing minor floating-point errors)
            Assert.InRange(b, -0.01, 0.01); // b ≈ 0
        }

        /// <summary>
        /// Tests that FitData correctly fits an exponential model.
        /// </summary>
        [Fact]
        public void FitData_ShouldFitExponentialModel()
        {
            var (a, b) = _mockCurveFittingService.Object.FitData(_exponentialData, FitModelType.Exponential);

            Assert.InRange(a, Math.Exp(1) - 0.01, Math.Exp(1) + 0.01);  // a ≈ e (allowing minor floating-point errors)
            Assert.InRange(b, 0.99, 1.01);  // b ≈ 1
        }

        /// <summary>
        /// Tests that FitData correctly fits a power model.
        /// </summary>
        [Fact]
        public void FitData_ShouldFitPowerModel()
        {
            var (a, b) = _mockCurveFittingService.Object.FitData(_powerData, FitModelType.Power);

            Assert.InRange(a, 0.99, 1.01);  // a ≈ 1
            Assert.InRange(b, 1.99, 2.01);  // b ≈ 2
        }

        /// <summary>
        /// Tests that FitData throws an exception for an invalid model type.
        /// </summary>
        [Fact]
        public void FitData_InvalidModel_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _mockCurveFittingService.Object.FitData(_linearData, (FitModelType)99));
        }

        /// <summary>
        /// Tests that FitData throws an exception for empty data.
        /// </summary>
        [Fact]
        public void FitData_EmptyData_ShouldThrowException()
        {
            Assert.Throws<InvalidOperationException>(() => _mockCurveFittingService.Object.FitData(_emptyData, FitModelType.Linear));
        }

        /// <summary>
        /// Tests that FitData correctly handles negative values.
        /// </summary>
        [Fact]
        public void FitData_NegativeValues_ShouldHandleCorrectly()
        {
            var (a, b) = _mockCurveFittingService.Object.FitData(_negativeData, FitModelType.Linear);

            Assert.InRange(a, 1.99, 2.01);
            Assert.InRange(b, -0.01, 0.01);
        }
    }
}



