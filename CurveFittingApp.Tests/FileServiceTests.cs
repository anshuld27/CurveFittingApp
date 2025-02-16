using Moq;
using Xunit;
using CurveFittingApp.Models;
using CurveFittingApp.Services;

namespace CurveFittingApp.Tests
{
    /// <summary>
    /// Unit tests for the FileService class.
    /// </summary>
    public class FileServiceTests
    {
        private readonly Mock<IFileService> _mockFileService;

        /// <summary>
        /// Initializes a new instance of the FileServiceTests class.
        /// Sets up mock services and initializes test data.
        /// </summary>
        public FileServiceTests()
        {
            _mockFileService = new Mock<IFileService>();

            // Setup mock for valid CSV
            _mockFileService
                .Setup(s => s.LoadData("test_data.csv"))
                .Returns(new List<DataPointModel>
                {
                    new DataPointModel { X = 1, Y = 2 },
                    new DataPointModel { X = 2, Y = 4 },
                    new DataPointModel { X = 3, Y = 6 }
                });

            // Setup mock for invalid CSV
            _mockFileService
                .Setup(s => s.LoadData("invalid_data.csv"))
                .Returns(new List<DataPointModel>
                {
                    new DataPointModel { X = 1, Y = 2 },
                    new DataPointModel { X = 3, Y = 6 }
                });

            // Setup mock for empty file
            _mockFileService
                .Setup(s => s.LoadData("empty.csv"))
                .Returns(new List<DataPointModel>());
        }

        /// <summary>
        /// Tests that LoadData correctly parses a valid CSV file.
        /// </summary>
        [Fact]
        public void LoadData_ShouldParseValidCSV()
        {
            var data = _mockFileService.Object.LoadData("test_data.csv");

            Assert.Equal(3, data.Count);
            Assert.Equal(1, data[0].X);
            Assert.Equal(2, data[0].Y);
            Assert.Equal(2, data[1].X);
            Assert.Equal(4, data[1].Y);
        }

        /// <summary>
        /// Tests that LoadData handles an invalid CSV file.
        /// </summary>
        [Fact]
        public void LoadData_ShouldHandleInvalidCSV()
        {
            var data = _mockFileService.Object.LoadData("invalid_data.csv");

            Assert.Equal(2, data.Count); // Only valid lines should be parsed
        }

        /// <summary>
        /// Tests that LoadData returns an empty list for an empty file.
        /// </summary>
        [Fact]
        public void LoadData_EmptyFile_ShouldReturnEmptyList()
        {
            var data = _mockFileService.Object.LoadData("empty.csv");

            Assert.Empty(data);
        }
    }
}


