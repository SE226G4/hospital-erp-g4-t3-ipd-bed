class Program
{
    static void Main()
    {
        HospitalSystem system = new HospitalSystem();

        Patient patient = new Patient
        {
            Id = 1,
            RequiredBedType = BedType.ICU
        };

        Room room = new Room
        {
            Id = 201,
            IsReady = true
        };

        // TC01
        Console.WriteLine("===== TC01 =====");

        Bed bed1 = new Bed
        {
            Id = 101,
            Status = BedStatus.Available,
            Type = BedType.ICU
        };

        system.AssignPatientToBed(patient, bed1, room);

        // TC02
        Console.WriteLine("\n===== TC02 =====");

        Bed bed2 = new Bed
        {
            Id = 102,
            Status = BedStatus.Busy,
            Type = BedType.ICU
        };

        system.AssignPatientToBed(patient, bed2, room);

        // TC03
        Console.WriteLine("\n===== TC03 =====");

        Bed bed3 = new Bed
        {
            Id = 103,
            Status = BedStatus.Cleaning,
            Type = BedType.ICU
        };

        system.AssignPatientToBed(patient, bed3, room);

        // TC04
        Console.WriteLine("\n===== TC04 =====");

        Bed bed4 = new Bed
        {
            Id = 104,
            Status = BedStatus.Available,
            Type = BedType.Standard
        };

        system.AssignPatientToBed(patient, bed4, room);
    }
}