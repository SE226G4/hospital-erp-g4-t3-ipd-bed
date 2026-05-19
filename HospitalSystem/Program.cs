class Program
{
    static void Main()
    {
        HospitalSystem system = new HospitalSystem();

        Bed bed1 = new Bed
        {
            Id = 101,
            Status = BedStatus.Available
        };

        Bed bed2 = new Bed
        {
            Id = 102,
            Status = BedStatus.Available
        };

        system.AssignPatientToBed(1, bed1);

        system.TransferPatient(1, bed1, bed2);
    }
}