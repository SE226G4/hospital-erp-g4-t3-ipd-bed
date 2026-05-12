# Software Requirements Specification (SRS)
## Project: Hospital ERP System
## Module/Subsystem: In-Patient Department & Bed Management
**Version:** 1.0  
**Date:** 2026-05-12

---

## 1. Introduction
### 1.1 Purpose
The purpose of this document is to provide a comprehensive description of the In-Patient Department & Bed Management module. It specifies the functional and non-functional requirements for developers and collaborating teams (Finance, Admission, and Surgery). This document serves as a blueprint for managing hospital bed capacity and patient stays within the ERP system.

### 1.2 Scope
The IPD-BED module manages the lifecycle of patient hospitalization, from bed assignment to discharge notification.

Core Goals: To optimize bed utilization, ensure patient-to-ward suitability, and automate stay-cost reporting.

The system WILL:

Monitor real-time bed status (Available, Occupied, Under Cleaning).
Validate patient eligibility based on data from Module 1 (Admission).
Manage internal patient transfers between wards.
Notify Module 2 (Finance) 24 hours before the expected discharge.

The system WILL NOT:
Register new patient identities.
Process financial payments or insurance claims .
Manage pharmacy prescriptions or inventory .

### 1.3 Definitions, Acronyms, and Abbreviations
ERP : Enterprise Resource Planning – The integrated hospital management system.
IPD : In-Patient Department – The department for patients who require an overnight stay.
Bed Status : A variable indicating whether a bed is ready for use, occupied, or needs sanitation.

### 1.4 References
IEEE 830-1998 Standard for Software Requirements Specifications.
Hospital ERP System Project Document (Module 3 Specification).

### 1.5 Overview
This document is organized into four main sections. Section 2 describes the overall product perspective and interfaces. Section 3 details the specific requirements and User Stories. Section 4 provides technical diagrams and the GitHub traceability checklist.

---

## 2. Overall Description
### 2.1 Product Perspective
The IPD-BED module is a core component of the Hospital ERP System. It acts as the intermediate layer between patient admission and financial clearance. The module does not operate in isolation; it consumes patient identity data from the Admission team (team 1) and provides occupancy and stay-duration data to the Finance team (team 2). It also coordinates with the Surgery (Module 5) and Emergency (team 6) modules to ensure bed availability for critical cases

*   **2.1.1 System Interfaces:
Inbound: Consumes Patient ID and Medical Risk profiles from Module 1 (ADM-MC) via REST API.
Outbound: Exposes "Bed Availability Status" to Module 5 (Surgery) and Module 6 (ER).
Reporting: Sends "Stay Duration & Room Grade" data to Module 2 (FIN-INS) for billing.

*   **2.1.2 User Interfaces:
The UI will feature a Dashboard/Grid View representing the hospital wards.
A Color-Coding System will be used (Green: Available, Red: Occupied, Yellow: Under Cleaning).
The design follows a Minimalist Design System with high contrast to ensure readability for nursing staff in high-pressure environments.

*   **2.1.3 Hardware Interfaces:** None (The system is purely web-based).
*   **2.1.4 Software Interfaces:** 
Operating System: Platform-independent (Web-based).
Database: Centralized SQL Database (managed by the Integration Team).
Libraries: React.js or Flutter for the frontend; Spring Boot or Flask for API management.

*   **2.1.5 Communications Interfaces:** 
Protocol: HTTP/HTTPS using RESTful architectural style.
Data Format: JSON (JavaScript Object Notation) for all cross-module communication.

*   **2.1.6 Memory & Operational Constraints:** 
Client Side: Minimum 4GB RAM and a modern web browser (Chrome/Edge).
Operational: The system must support up to 50 concurrent users (nurses/receptionists) and maintain 99.9% uptime for bed status accuracy.assumptions].

### 2.2 Product Functions
Bed Inventory Management: Tracking the real-time status of all hospital beds (Available, Occupied, Cleaning).
Admission Validation: Checking patient eligibility and medical risks before ward assignment.
Patient Placement & Transfer: Handling the digital movement of patients between rooms and wards.
Notification System: Generating automated alerts for the Finance module regarding discharge and stay costs.
Reporting: Providing occupancy and vacancy rate reports for hospital management.

### 2.3 User Characteristics
Receptionist/Admission Officer: (Medium technical expertise) Responsible for assigning beds to new patients.
Ward Nurse: (Medium technical expertise) Responsible for updating bed cleaning status and patient transfers.
System Administrator: (High technical expertise) Manages system configurations, room categories, and user permissions.

### 2.4 Constraints, Assumptions, and Dependencies
Constraints: Data privacy (HIPAA-like standards) for patient locations; system must be web-accessible as per session requirements.

Assumptions: Users have basic training on the web interface; the hospital network is stable.
Dependencies: * Module 1 (Admission): Our module cannot function without the Patient ID and Risk Profile APIs.

Integration Team: Dependency on the central database schema and API gateway provided by the G4-Integration team.

---

## 3. Specific Requirements (Agile Approach)
* **Instruction:** This section translates traditional functional requirements into Agile User Stories. Every feature must be traceable to the project management board.

### 3.1 External Interface Requirements
Data Formats: All inter-module communication will use JSON via RESTful APIs.

API Endpoints:

GET /api/v1/admission/patient/{id}: Consumes patient's medical profile and allergy data from Module 1.
POST /api/v1/finance/billing/update: Sends stay duration, room grade, and base costs to Module 2.
PATCH /api/v1/ipd/bed-status: Exposes current bed availability to Module 5 (Surgery) and Module 6 (ER).
UI Layouts: A dashboard featuring a Grid/Map View of hospital wards. Wards are categorized by type (General, ICU, Private). Visual indicators (Icons/Colors) represent the current status of each bed.

### 3.2 System Features & User Stories

#### 3.2.1 Feature: Bed Inventory & Status Management
*   **Description:** A real-time tracking system for hospital bed availability and sanitation cycles.
*   **Priority:** High.
*   **User Stories:**
    *   **Story 1:** 
    As the IPD System, I want to send a discharge notification to the Finance module 24 hours in 
    advance so that the billing department can prepare the final invoice.
    
        * *Acceptance Criteria:* 
        An automated API call is triggered based on the "Expected Discharge Date"; Finance module 
        receives a confirmation response.
        
        * *GitHub Issue:* #12


#### 3.2.2 Feature: Patient Admission & Placement Validation
 **Description:** A real-time tracking system for hospital bed availability and sanitation cycles.
*   **Priority:** High.
*   **User Stories:**
    *   **Story 1:**
    As the IPD System, I want to send a discharge notification to the Finance module 24 hours in 
    advance so that the billing department can prepare the final invoice.
    
        * *Acceptance Criteria:* 
        An automated API call is triggered based on the "Expected Discharge Date"; Finance module 
        receives a confirmation response.
        
        * *GitHub Issue:* #12
### 3.3 Performance Requirements
Response Time: The system must return bed search and filtering results in under 1.5 seconds.

Concurrency: The module must support up to 30 concurrent users (nurses and receptionists) without performance degradation.

Availability: The bed status dashboard must maintain 99.9% uptime to ensure real-time accuracy for emergency placements.

### 3.4 Logical Database Requirements
Our module is responsible for the following entities within the shared database:

Beds Table: Stores Bed_ID, Ward_ID, Status (Available/Occupied/Cleaning), and Bed_Type.

Wards Table: Stores Ward_Name, Department, and Total_Capacity.

Stay_Records Table: A junction table linking Patient_ID (from Module 1) to Bed_ID, including Check_In_Date and Expected_Discharge_Date.

### 3.5 Software System Attributes
* **Instruction:** Define the Non-Functional Requirements (NFRs) for your module:
  * **Reliability:**
     The system shall maintain an uptime of 99.9% during hospital operating hours. The maximum
     acceptable downtime for the bed-status dashboard is 5 minutes per month to ensure emergency cases
     always find a location
     
  * **Security:** All users must authenticate via JWT (JSON Web Tokens)
  
  * **Maintainability & Portability:** 
  Code must follow the Clean Code principles and the Google Style Guide for Java/JavaScript.

  The system must be platform-independent, supporting all modern web browsers (Chrome, Firefox, Edge).

  APIs must be documented using Swagger/OpenAPI 3.0 to facilitate integration with other teams.

---

## 4. Appendices
### Appendix A: Glossary & Models
ossary: * Bed Assignment: The process of linking a patient to a specific physical bed.

Ward: A specific section of the hospital (e.g., ICU, Pediatric, General).

ERD: (Entity-Relationship Diagram) showing the links between Beds, Wards, and Stay_Records (referencing Patient_ID from Module 1).

DFD: (Data Flow Diagram) illustrating how patient data flows from Admission (Module 1) to Bed Management, then to Finance (Module 2).

### Appendix B: GitHub Traceability Checklist
* **Instruction for Team Members:** Before submitting this SRS, ensure that:
  * [ ] Every User Story in Section 3.2 has a corresponding GitHub Issue.
  * [ ] Every GitHub Issue has an appropriate label (e.g., `enhancement`, `requirement`).
  * [ ] Pull Requests reference the Issue IDs (e.g., `Closes #12`). 
