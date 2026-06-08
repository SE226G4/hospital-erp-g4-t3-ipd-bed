using System;

public class HospitalSystem
{
    // Assign patient to bed
    public void AssignPatientToBed(
        Patient patient,
        Bed bed)
    {
        if (bed.Status != BedStatus.Available)
        {
            Console.WriteLine(
                "Bed is not available"
            );
            return;
        }

        // Check bed suitability
        if (bed.Type != patient.RequiredBedType)
        {
            Console.WriteLine(
                "Bed is not suitable for patient condition"
            );
            return;
        }

        // Assign patient
        bed.Status = BedStatus.Busy;

        Console.WriteLine(
            $"Patient {patient.Name} assigned to Bed {bed.Id}"
        );
    }

 public void TransferPatient(
    Patient patient,
    Bed currentBed,
    Bed newBed)
{
    if (newBed.Status != BedStatus.Available)
    {
        Console.WriteLine("New bed is not available");
        return;
    }

    if (newBed.Type != patient.RequiredBedType)
    {
        Console.WriteLine("New bed is not suitable");
        return;
    }

    if (patient == null)
    {
        Console.WriteLine("Patient not found");
        return;
    }

    if (currentBed == null)
    {
        Console.WriteLine("Current bed not found");
        return;
    }

    currentBed.Status = BedStatus.Cleaning;
    newBed.Status = BedStatus.Busy;

    Console.WriteLine(
        $"Patient {patient.Name} transferred " +
        $"from Bed {currentBed.Id} to Bed {newBed.Id}");
}
}