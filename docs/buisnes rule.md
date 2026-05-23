# IPD-BED Business Rules & Validation

## 1. Bed Availability Validation

- A patient cannot be assigned to a bed unless the bed status is Available.
- Beds with status Busy or Cleaning cannot accept new patients.

---

## 2. Bed Suitability Validation

- The assigned bed must match the patient medical condition.
- ICU patients must be assigned to ICU beds.
- Isolation patients must be assigned to isolation rooms.
- Pediatric patients must be assigned to pediatric beds.

---

## 3. Room Readiness Validation

- A patient cannot be assigned or transferred to a room unless the room is marked as ready.
- Rooms under cleaning or maintenance are considered unavailable.

---

## 4. Patient Transfer Validation

- Patient transfer is allowed only if:
  - The new bed is available
  - The new bed is suitable
  - The new room is ready

---

## 5. Bed Status Update Rules

- When a patient is assigned:
  - Bed status changes from Available to Busy

- When a patient is transferred:
  - Old bed status changes to Cleaning
  - New bed status changes to Busy

- After room cleaning:
  - Bed status changes from Cleaning to Available

---

## 6. Discharge Notification Rule

- The system must send a notification to the finance system 24 hours before expected patient discharge.

---

## 7. Admission Rejection Rules

Admission request must be rejected if:
- No available bed exists
- Bed is not suitable
- Room is not ready

---

## 8. Transfer Rejection Rules

Transfer request must be rejected if:
- New bed is unavailable
- New bed is unsuitable
- New room is not ready