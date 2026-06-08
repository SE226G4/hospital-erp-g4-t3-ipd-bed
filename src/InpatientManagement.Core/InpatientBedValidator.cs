namespace HospitalAssignment.Core
{
    public enum BedStatus { Available, Occupied, Cleaning }

    public class Patient
    {
        public string Name { get; set; }
        public string RequiredBedType { get; set; } 
    }

    public class Bed
    {
        public string Id { get; set; }
        public BedStatus Status { get; set; } 
        public string BedType { get; set; }
        public bool IsRoomReady { get; set; } 
    }

    public class InpatientManager
    {
        public string EvaluateBedAssignment(Bed bed, Patient patient, bool isTransferAction)
        {
            switch (bed.Status)
            {
                case BedStatus.Occupied:
                    return "Rejected: Bed is occupied";
                
                case BedStatus.Cleaning:
                    return "Rejected: Bed is currently cleaning";

                case BedStatus.Available:
                    if (isTransferAction && !bed.IsRoomReady)
                    {
                        return "Rejected: New room is not ready";
                    }

                    if (bed.BedType != patient.RequiredBedType)
                    {
                        return "Rejected: Bed type does not match patient condition";
                    }

                    return "Approved: Bed Assigned Successfully";

                default:
                    return "Rejected: Process failed";
            }
        }
    }
}