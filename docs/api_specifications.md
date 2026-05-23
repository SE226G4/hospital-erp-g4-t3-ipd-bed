# IPD-BED API Specification

## Module Description

The IPD-BED module is responsible for:
- Managing hospital beds
- Assigning patients to suitable beds
- Transferring patients between beds and rooms
- Updating bed status
- Displaying bed and room distribution
- Sending discharge notifications to the finance system

---

# 1. Assign Patient To Bed

## Endpoint

```http
POST /api/beds/assign
```

## Description

Assign a patient to an available and suitable bed.

## Request Body

```json
{
  "patientId": 1,
  "bedId": 101
}
```

## Validation Rules

- Bed must be available
- Bed must be suitable for patient condition
- Room must be ready

## Success Response

```json
{
  "message": "Patient assigned successfully"
}
```

## Error Responses

### Bed Not Available

```json
{
  "error": "Bed is not available"
}
```

### Bed Not Suitable

```json
{
  "error": "Bed is not suitable for patient condition"
}
```

### Room Not Ready

```json
{
  "error": "Room is not ready"
}
```

---

# 2. Transfer Patient

## Endpoint

```http
POST /api/beds/transfer
```

## Description

Transfer patient from current bed to another suitable bed.

## Request Body

```json
{
  "patientId": 1,
  "currentBedId": 101,
  "newBedId": 102
}
```

## Validation Rules

- New bed must be available
- New bed must be suitable
- New room must be ready

## Success Response

```json
{
  "message": "Patient transferred successfully"
}
```

## Error Responses

### New Bed Not Available

```json
{
  "error": "New bed is not available"
}
```

### New Bed Not Suitable

```json
{
  "error": "New bed is not suitable"
}
```

### Room Not Ready

```json
{
  "error": "New room is not ready"
}
```

---

# 3. Send Discharge Notification

## Endpoint

```http
POST /api/discharge/notify
```

## Description

Send discharge notification to finance system 24 hours before expected discharge.

## Request Body

```json
{
  "patientId": 1
}
```

## Success Response

```json
{
  "message": "Finance system notified successfully"
}
```

---

# 4. Get Beds Status

## Endpoint

```http
GET /api/beds
```

## Description

Retrieve all beds with their current status and assigned rooms.

## Success Response

```json
[
  {
    "bedId": 101,
    "status": "Busy",
    "roomId": 12
  },
  {
    "bedId": 102,
    "status": "Available",
    "roomId": 12
  }
]
```