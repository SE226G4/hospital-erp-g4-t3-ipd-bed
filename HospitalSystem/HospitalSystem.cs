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


public bool IsBedAvailable(Bed bed)
{
    return bed.Status == BedStatus.Available;
}

public bool IsBedSuitable(Patient patient, Bed bed)
{
    return bed.Type == patient.RequiredBedType;
}

public bool IsValidPatient(Patient patient)
{
    return patient != null;
}

public bool IsValidBed(Bed bed)
{
    return bed != null;
}

public void TransferPatient(
    Patient patient,
    Bed currentBed,
    Bed newBed)
{
    if (!IsValidPatient(patient) ||
        !IsValidBed(currentBed) ||
        !IsBedAvailable(newBed) ||
        !IsBedSuitable(patient, newBed))
    {
        Console.WriteLine("Transfer failed");
        return;
    }

    currentBed.Status = BedStatus.Cleaning;
    newBed.Status = BedStatus.Busy;

    Console.WriteLine(
        $"Patient {patient.Name} transferred from Bed {currentBed.Id} to Bed {newBed.Id}");
}
}