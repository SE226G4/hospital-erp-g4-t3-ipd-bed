using System;

public class HospitalSystem
{
    public void AssignPatientToBed(int patientId, Bed bed)
    {
        if (bed.Status != BedStatus.Available)
        {
            Console.WriteLine("Bed is not available");
            return;
        }

        bed.Status = BedStatus.Busy;

        Console.WriteLine(
            $"Patient {patientId} assigned to Bed {bed.Id}"
        );
    }

    public void TransferPatient(
        int patientId,
        Bed currentBed,
        Bed newBed)
    {
        if (newBed.Status != BedStatus.Available)
        {
            Console.WriteLine("New bed is not available");
            return;
        }

        currentBed.Status = BedStatus.Cleaning;

        newBed.Status = BedStatus.Busy;

        Console.WriteLine(
            $"Patient {patientId} transferred " +
            $"from Bed {currentBed.Id} to Bed {newBed.Id}"
        );
    }
}