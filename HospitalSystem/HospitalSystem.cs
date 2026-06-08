using System;

public class HospitalSystem
{
    // Assign patient to bed
public void AssignPatientToBed(Patient patient, Bed bed, Room room)
{
    if (bed.Status != BedStatus.Available)
    {
        Console.WriteLine("Bed is not available");
        return;
    }

    if (bed.Type != patient.RequiredBedType)
    {
        Console.WriteLine("Bed is not suitable");
        return;
    }

    if (!room.IsReady)
    {
        Console.WriteLine("Room is not ready");
        return;
    }

    bed.Status = BedStatus.Busy;

    Console.WriteLine(
        $"Patient {patient.Id} assigned to Bed {bed.Id}"
    );
}

    public void TransferPatient(
        Patient patient,
        Bed currentBed,
        Bed newBed)
    {
        if (newBed.Status != BedStatus.Available)
        {
            Console.WriteLine(
                "New bed is not available"
            );
            return;
        }

        // Check new bed suitability
        if (newBed.Type != patient.RequiredBedType)
        {
            Console.WriteLine(
                "New bed is not suitable"
            );
            return;
        }

        currentBed.Status = BedStatus.Cleaning;

        // Update new bed
        newBed.Status = BedStatus.Busy;

        Console.WriteLine(
            $"Patient {patient.Name} transferred " +
            $"from Bed {currentBed.Id} to Bed {newBed.Id}"
        );
    }
}