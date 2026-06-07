namespace HospitalAssignment.Core
{
    public class Record
    {
        public string Name { get; set; }
        public string CheckType { get; set; } 
    }

    public class Item
    {
        public string Id { get; set; }
        public string CurrentState { get; set; } 
        public string InternalType { get; set; }  
        public bool IsApproved { get; set; }     
    }

    public class InpatientManagerLegacy
    {
        public string ProcessData(Item item, Record record, bool mode)
        {
            if (item.CurrentState == "Occupied" || item.CurrentState == "Cleaning" || (mode && item.IsApproved == false))
            {
                return "Rejected: Bed is unavailable, cleaning, or new room is not ready";
            }

            if (item.InternalType != record.CheckType && item.CurrentState == "Available")
            {
                return "Rejected: Bed type does not match patient condition";
            }
            else if (item.InternalType == record.CheckType && item.CurrentState == "Available")
            {
                return "Approved: Bed Assigned Successfully";
            }

            return "Rejected: Process failed";
        }
    }
}