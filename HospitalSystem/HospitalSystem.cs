using System;

public class HospitalSystem
{
    // Assign patient to bed
public bool AssignPatientToBed(Patient patient, Bed bed)
{
    if (!CanAssignPatient(patient, bed))
    {
        Console.WriteLine("Assignment failed");
        return false;
    }

    bed.Status = BedStatus.Busy;

    Console.WriteLine(
        $"Patient {patient.Id} assigned to Bed {bed.Id}");

    return true;
}

    private bool CanAssignPatient(
        Patient patient,
        Bed bed)
    {
        return bed.Status == BedStatus.Available
            && bed.Type == patient.RequiredBedType;
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