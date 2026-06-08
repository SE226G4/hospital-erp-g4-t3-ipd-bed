using Xunit;
using HospitalAssignment.Core;

namespace HospitalAssignment.Tests
{
    public class InpatientManagerTests
    {
        private readonly InpatientManager _manager;

        public InpatientManagerTests()
        {
            _manager = new InpatientManager();
        }

        [Fact]
        public void EvaluateBedAssignment_ShouldReturnRejected_WhenBedIsOccupied()
        {
            // Arrange 
            var bed = new Bed { Id = "B1", Status = BedStatus.Occupied, BedType = "ICU", IsRoomReady = true };
            var patient = new Patient { Name = "Sami", RequiredBedType = "ICU" };

            // Act 
            string result = _manager.EvaluateBedAssignment(bed, patient, isTransferAction: false);

            // Assert 
            Assert.Equal("Rejected: Bed is occupied", result);
        }

        [Fact]
        public void EvaluateBedAssignment_ShouldReturnRejected_WhenBedIsCleaning()
        {
            // Arrange
            var bed = new Bed { Id = "B2", Status = BedStatus.Cleaning, BedType = "Regular", IsRoomReady = true };
            var patient = new Patient { Name = "Ali", RequiredBedType = "Regular" };

            // Act
            string result = _manager.EvaluateBedAssignment(bed, patient, isTransferAction: false);

            // Assert
            Assert.Equal("Rejected: Bed is currently cleaning", result);
        }

        [Fact]
        public void EvaluateBedAssignment_ShouldReturnRejected_WhenTransferActionAndRoomNotReady()
        {
            // Arrange
            var bed = new Bed { Id = "B3", Status = BedStatus.Available, BedType = "ICU", IsRoomReady = false };
            var patient = new Patient { Name = "Omar", RequiredBedType = "ICU" };

            // Act
            string result = _manager.EvaluateBedAssignment(bed, patient, isTransferAction: true);

            // Assert
            Assert.Equal("Rejected: New room is not ready", result);
        }

        [Fact]
        public void EvaluateBedAssignment_ShouldReturnRejected_WhenBedTypeDoesNotMatchPatientCondition()
        {
            // Arrange
            var bed = new Bed { Id = "B4", Status = BedStatus.Available, BedType = "Regular", IsRoomReady = true };
            var patient = new Patient { Name = "Khaled", RequiredBedType = "ICU" };

            // Act
            string result = _manager.EvaluateBedAssignment(bed, patient, isTransferAction: false);

            // Assert
            Assert.Equal("Rejected: Bed type does not match patient condition", result);
        }

        [Fact]
        public void EvaluateBedAssignment_ShouldReturnApproved_WhenAllConditionsAreValid()
        {
            // Arrange
            var bed = new Bed { Id = "B5", Status = BedStatus.Available, BedType = "ICU", IsRoomReady = true };
            var patient = new Patient { Name = "Sami", RequiredBedType = "ICU" };

            // Act
            string result = _manager.EvaluateBedAssignment(bed, patient, isTransferAction: true);

            // Assert
            Assert.Equal("Approved: Bed Assigned Successfully", result);
        }
    }
}