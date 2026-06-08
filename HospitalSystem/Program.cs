class Program
{
    static void Main(string[] args)
    {
        HospitalSystem hospital = new HospitalSystem();

        Console.WriteLine("=== Test Case 1 ===");

        Patient patient1 = new Patient
        {
            Name = "Ali",
            RequiredBedType = BedType.ICU
        };

        Bed currentBed1 = new Bed
        {
            Id = 1,
            Type = BedType.ICU,
            Status = BedStatus.Busy
        };

        Bed newBed1 = new Bed
        {
            Id = 2,
            Type = BedType.ICU,
            Status = BedStatus.Busy
        };

        hospital.TransferPatient(patient1, currentBed1, newBed1);

        Console.WriteLine();


        Console.WriteLine("=== Test Case 2 ===");

        Patient patient2 = new Patient
        {
            Name = "Ahmad",
            RequiredBedType = BedType.ICU
        };

        Bed currentBed2 = new Bed
        {
            Id = 3,
            Type = BedType.ICU,
            Status = BedStatus.Busy
        };

        Bed newBed2 = new Bed
        {
            Id = 4,
            Type = BedType.Standard,
            Status = BedStatus.Available
        };

        hospital.TransferPatient(patient2, currentBed2, newBed2);

        Console.WriteLine();


        Console.WriteLine("=== Test Case 3 ===");

        Patient patient3 = new Patient
        {
            Name = "Saleh",
            RequiredBedType = BedType.ICU
        };

        Bed currentBed3 = new Bed
        {
            Id = 5,
            Type = BedType.ICU,
            Status = BedStatus.Busy
        };

        Bed newBed3 = new Bed
        {
            Id = 6,
            Type = BedType.ICU,
            Status = BedStatus.Available
        };

        hospital.TransferPatient(patient3, currentBed3, newBed3);

        Console.ReadKey();
    }
}