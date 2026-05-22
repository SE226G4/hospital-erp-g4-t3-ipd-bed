class Program
{
    static void Main()
    {
        HospitalSystem system = new HospitalSystem();

        Patient patient = new Patient
        {
            Id = 1,
            Name = "Mahmoud",
            RequiredBedType = BedType.ICU
        };

        Bed bed1 = new Bed
        {
            Id = 101,
            Status = BedStatus.Available,
            Type = BedType.ICU
        };

        Bed bed2 = new Bed
        {
            Id = 102,
            Status = BedStatus.Available,
            Type = BedType.ICU
        };

        // Assign patient
        system.AssignPatientToBed(
            patient,
            bed1
        );

        // Transfer patient
        system.TransferPatient(
            patient,
            bed1,
            bed2
        );
    }
}