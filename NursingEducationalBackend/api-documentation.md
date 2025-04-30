# Nursing Educational Backend API Documentation

## Overview

This API documentation covers the endpoints for the Nursing Educational Backend system. The API allows for authentication, patient management, and medical record creation/retrieval. All secure endpoints require a valid JWT token obtained through the login process.

This documentation is updated as of April 30, 2025 and reflects the current state of the controllers including PatientsController and PatientsWriteController.

## Authentication Endpoints

### Register
- **URL**: `/api/Auth/register`
- **Method**: `POST`
- **Auth Required**: No
- **Description**: Registers a new nurse user in the system
- **Request Body**:
  ```json
  {
    "email": "user@example.com",
    "password": "Password123!",
    "confirmPassword": "Password123!",
    "fullName": "User Name",
    "studentNumber": "12345",
    "campus": "Main Campus"
  }
  ```
- **Example Request**:
  ```bash
  curl -X POST \
    "http://localhost:5232/api/Auth/register" \
    -H "Content-Type: application/json" \
    -d '{
      "email": "user@example.com",
      "password": "Password123!",
      "confirmPassword": "Password123!",
      "fullName": "User Name",
      "studentNumber": "12345",
      "campus": "Main Campus"
    }'
  ```
- **Success Response**: 
  ```json
  {
    "success": true,
    "message": "User registered successfully!"
  }
  ```
- **Error Responses**:
  - **400 Bad Request**: If the email already exists or validation fails
    ```json
    {
      "success": false,
      "message": "Email already exists!"
    }
    ```
    Or
    ```json
    {
      "success": false,
      "message": "User creation failed! Please check user details and try again.",
      "errors": [
        { "code": "PasswordTooShort", "description": "Passwords must be at least 6 characters." },
        // Other potential validation errors
      ]
    }
    ```

### Login
- **URL**: `/api/Auth/login`
- **Method**: `POST`
- **Auth Required**: No
- **Description**: Authenticates a nurse user and returns a JWT token
- **Request Body**:
  ```json
  {
    "email": "user@example.com",
    "password": "Password123!"
  }
  ```
- **Example Request**:
  ```bash
  curl -X POST \
    "http://localhost:5232/api/Auth/login" \
    -H "Content-Type: application/json" \
    -d '{
      "email": "user@example.com",
      "password": "Password123!"
    }'
  ```
- **Success Response**:
  ```json
  {
    "success": true,
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlckBleGFtcGxlLmNvbSIsImp0aSI6ImI0YWM3Y2FkLTA1YTUtNDdhNy04MGQxLWE4NzYzZDdhMGQzMCIsIk51cnNlSWQiOiIxIiwiZXhwIjoxNzQyNjE5MjczLCJpc3MiOiJOdXJzaW5nRWR1Y2F0aW9uYWxCYWNrZW5kIiwiYXVkIjoiTnVyc2luZ0VkdWNhdGlvbmFsQXBwIn0.X_3KpjeKyvXUlA9y7H_OTI9MWYEL0-x4pxu4jORIk2Q",
    "nurseId": 1,
    "fullName": "User Name",
    "email": "user@example.com",
    "campus": "Main Campus",
    "roles": ["Admin"]  // For admin users, or [] for regular nurses
  }
  ```
- **Error Responses**:
  - **401 Unauthorized**: If authentication fails
    ```json
    {
      "success": false,
      "message": "Invalid email or password."
    }
    ```
  - **401 Unauthorized**: If nurse record not found
    ```json
    {
      "success": false,
      "message": "Nurse record not found."
    }
    ```

### Logout
- **URL**: `/api/Auth/logout`
- **Method**: `POST`
- **Auth Required**: Yes
- **Description**: Logs out the current user (primarily handled client-side by removing JWT)
- **Example Request**:
  ```bash
  curl -X POST \
    "http://localhost:5232/api/Auth/logout" \
    -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWJjZEBnbWFpbC5jb20iLCJqdGkiOiJiNGFjN2NhZC0wNWE1LTQ3YTctODBkMS1hODc2M2Q3YTBkMzAiLCJOdXJzZUlkIjoiMSIsImV4cCI6MTc0MjYxOTI3MywiaXNzIjoiTnVyc2luZ0VkdWNhdGlvbmFsQmFja2VuZCIsImF1ZCI6Ik51cnNpbmdFZHVjYXRpb25hbEFwcCJ9.5qCT3VhwvjrjWlqI1BI_GeyGLXj4lWl76Jg3ov6Xn3U" \
    -H "Content-Type: application/json"
  ```
- **Success Response**:
  ```json
  {
    "success": true,
    "message": "Logged out successfully"
  }
  ```

## JWT Token Information

The JWT token returned by the login endpoint contains the following claims:

- **Name**: The user's email address
- **Jti**: A unique token identifier
- **NurseId**: The nurse's ID in the system
- **Campus**: The campus the nurse is associated with
- **Roles**: (If assigned) User roles such as "Admin"
- **Expiration**: Token expiration time (configured in minutes via JwtSettings:DurationInMinutes)
- **Issuer**: The issuing authority (configured via JwtSettings:Issuer)
- **Audience**: The intended audience (configured via JwtSettings:Audience)

## Basic Patient Endpoints

### Get All Patients
- **URL**: `/api/Patients`
- **Method**: `GET`
- **Auth Required**: No (Currently commented out in code)
- **Description**: Returns a list of all patients in the system
- **Example Request**:
  ```bash
  curl -X GET "http://localhost:5232/api/Patients"
  ```
- **Success Response**: 
  ```json
  [
    {
      "patientId": 1,
      "patientWristId": "W-0001",
      "fullName": "John Doe",
      "dob": "1975-07-04",
      "nurseId": 1,
      "gender": "Male",
      "roomNumber": "101",
      "admitDate": "2023-05-15",
      "diagnosis": "Hypertension",
      "allergies": "Penicillin",
      "code": "Full Code"
    },
    // More patients...
  ]
  ```

### Get Patient By ID
- **URL**: `/api/Patients/{id}`
- **Method**: `GET`
- **Auth Required**: No (Currently commented out in code)
- **Description**: Returns a specific patient by ID
- **URL Parameters**: `id=[integer]` where `id` is the PatientId
- **Example Request**:
  ```bash
  curl -X GET "http://localhost:5232/api/Patients/1"
  ```
- **Success Response**: 
  ```json
  {
    "patientId": 1,
    "patientWristId": "W-0001",
    "fullName": "John Doe",
    "dob": "1975-07-04",
    "nurseId": 1,
    "gender": "Male",
    "roomNumber": "101",
    "admitDate": "2023-05-15",
    "diagnosis": "Hypertension",
    "allergies": "Penicillin",
    "code": "Full Code"
  }
  ```

### Create Patient (PatientsController)
- **URL**: `/api/Patients`
- **Method**: `POST`
- **Auth Required**: No (Currently commented out in code)
- **Description**: Creates a new patient record
- **Request Body**: JSON object with Patient properties
- **Example Request**:
  ```bash
  curl -X POST \
    "http://localhost:5232/api/Patients" \
    -H "Content-Type: application/json" \
    -d '{
      "fullName": "Jane Smith",
      "patientWristId": "W-0002",
      "gender": "Female",
      "dob": "1980-03-22",
      "unit": "A",
      "bedNumber": 5,
      "allergies": "None"
    }'
  ```
- **Success Response**: 
  - **Code**: 201 Created
  - **Content**: The created patient object
- **Error Responses**:
  - **400 Bad Request**: If validation fails or if Unit/Bed combination already exists
    ```json
    {
      "message": "A patient is already assigned to Unit A, Bed 5. This combination must be unique."
    }
    ```
  - **500 Internal Server Error**: If server error occurs
    ```json
    {
      "message": "Error creating patient",
      "error": "Error message"
    }
    ```

### Update Patient
- **URL**: `/api/Patients/{id}`
- **Method**: `PUT`
- **Auth Required**: No (Currently commented out in code)
- **Description**: Updates an existing patient record
- **URL Parameters**: `id=[integer]` where `id` is the PatientId
- **Request Body**: JSON object with updated Patient properties
- **Example Request**:
  ```bash
  curl -X PUT \
    "http://localhost:5232/api/Patients/1" \
    -H "Content-Type: application/json" \
    -d '{
      "patientId": 1,
      "fullName": "John Doe Updated",
      "patientWristId": "W-0001",
      "gender": "Male",
      "dob": "1975-07-04",
      "unit": "B",
      "bedNumber": 3,
      "allergies": "Penicillin, Sulfa"
    }'
  ```
- **Success Response**: 
  - **Code**: 204 No Content
- **Error Responses**:
  - **400 Bad Request**: If validation fails or if Unit/Bed combination already exists
    ```json
    {
      "message": "A patient is already assigned to Unit B, Bed 3. This combination must be unique."
    }
    ```
  - **404 Not Found**: If patient with given ID doesn't exist
    ```json
    {
      "message": "Patient with ID 1 not found"
    }
    ```
  - **500 Internal Server Error**: If server error occurs
    ```json
    {
      "message": "Error updating patient",
      "error": "Error message"
    }
    ```

## Patient Write Endpoints (PatientsWriteController)

### Create Patient
- **URL**: `/api/patients/create`
- **Method**: `POST`
- **Auth Required**: No (Currently commented out in code)
- **Description**: Creates a new patient record
- **Request Body**: JSON object with Patient properties
- **Example Request**:
  ```bash
  curl -X POST \
    "http://localhost:5232/api/patients/create" \
    -H "Content-Type: application/json" \
    -d '{
      "fullName": "John Doe",
      "sex": "Male",
      "patientWristId": "P12345",
      "nextOfKin": "Jane Doe (Wife)",
      "height": "5'\''10\"",
      "weight": 180,
      "dateOfBirth": "1975-05-15",
      "allergies": "Penicillin, Shellfish",
      "isolationPrecautions": "None",
      "diagnosis": "Type 2 Diabetes",
      "admissionDate": "2025-03-20",
      "unit": "A",
      "bedNumber": 5
    }'
  ```
- **Success Response**: 
  - **Code**: 200 OK
- **Error Responses**:
  - **400 Bad Request**: If validation fails or if Unit/Bed combination already exists
    ```json
    "A patient is already assigned to Unit A, Bed 5. This combination must be unique."
    ```
    Or
    ```json
    "Unable to create patient: Invalid model state"
    ```
  - **500 Internal Server Error**: If server error occurs
    ```json
    "Unable to create patient: [error message]"
    ```

### Assign Nurse to Patient
- **URL**: `/api/patients/{id}/assign-nurse/{nurseId}`
- **Method**: `POST`
- **Auth Required**: No (Currently commented out in code)
- **URL Parameters**:
  - `id=[integer]` where `id` is the PatientId
  - `nurseId=[integer]` where `nurseId` is the ID of the nurse to assign
- **Description**: Assigns a nurse to a patient
- **Example Request**:
  ```bash
  curl -X POST \
    "http://localhost:5232/api/patients/1/assign-nurse/2" \
    -H "Content-Type: application/json"
  ```
- **Success Response**: 
  ```json
  {
    "nurseId": 2
  }
  ```
- **Error Responses**:
  - **400 Bad Request**: If operation fails
    ```json
    "Nurse id unable to be assigned"
    ```

### Submit Patient Data
- **URL**: `/api/patients/{id}/submit-data`
- **Method**: `POST`
- **Auth Required**: No
- **URL Parameters**: `id=[integer]` where `id` is the PatientId
- **Description**: Submits or updates various types of patient assessment data
- **Request Body**: Dictionary of key-value pairs where keys follow the format `table-{tableType}-{patientId}`
- **Supported Table Types**:
  - elimination
  - mobility
  - nutrition
  - cognitive
  - safety
  - adl
  - behaviour
  - progressnote
  - skinsensoryaid
  - profile
- **Example Request**:
  ```bash
  curl -X POST \
    "http://localhost:5232/api/patients/1/submit-data" \
    -H "Content-Type: application/json" \
    -d '{
      "table-cognitive-1": {
        "speech": "Clear and coherent",
        "loc": "Alert and oriented x4",
        "mmse": "28/30",
        "confusion": "No signs of confusion"
      },
      "table-progressnote-1": {
        "note": "Patient reports feeling well this morning. Vitals stable.",
        "timestamp": "2025-04-13T10:30:00"
      }
    }'
  ```
- **Success Response**: 
  - **Code**: 200 OK
  - **Content**: `"Data submitted successfully"`
- **Error Responses**:
  - **400 Bad Request**: If validation fails
    ```json
    "Error submitting data: [error message]"
    ```
  - **404 Not Found**: If patient not found
    ```json
    "Patient not found"
    ```

## Nurse and Admin Endpoints

### Get Assigned Patients (Nurse)
- **URL**: `/api/Patients/nurse/ids`
- **Method**: `GET`
- **Auth Required**: Yes
- **Required Role**: Any authenticated user
- **Description**: Returns patients assigned to the authenticated nurse or patients with no nurse assignment
- **Example Request**:
  ```bash
  curl -X GET \
    "http://localhost:5232/api/Patients/nurse/ids" \
    -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWJjZEBnbWFpbC5jb20iLCJqdGkiOiJiNGFjN2NhZC0wNWE1LTQ3YTctODBkMS1hODc2M2Q3YTBkMzAiLCJOdXJzZUlkIjoiMSIsImV4cCI6MTc0MjYxOTI3MywiaXNzIjoiTnVyc2luZ0VkdWNhdGlvbmFsQmFja2VuZCIsImF1ZCI6Ik51cnNpbmdFZHVjYXRpb25hbEFwcCJ9.5qCT3VhwvjrjWlqI1BI_GeyGLXj4lWl76Jg3ov6Xn3U" \
    -H "Content-Type: application/json"
  ```
- **Success Response**: 
  ```json
  [
    {
      "patientId": 1,
      "patientWristId": "W-0001", 
      "fullName": "John Doe",
      "dob": "1975-07-04",
      "nurseId": 1,
      "gender": "Male",
      "roomNumber": "101",
      "admitDate": "2023-05-15",
      "diagnosis": "Hypertension",
      "allergies": "Penicillin",
      "code": "Full Code"
    }
  ]
  ```
- **Error Responses**:
  - **401 Unauthorized**: If user is not authenticated
    ```json
    {
      "message": "Invalid token or missing NurseId claim"
    }
    ```
  - **400 Bad Request**: If NurseId format is invalid
    ```json
    {
      "message": "Invalid NurseId format"
    }
    ```
  - **500 Internal Server Error**: If server error occurs
    ```json
    {
      "message": "Error retrieving nurse patients",
      "error": "Error message"
    }
    ```

### Get Patient Assessment By Type (Nurse)
- **URL**: `/api/Patients/nurse/patient/{id}/{tableType}`
- **Method**: `GET`
- **Auth Required**: Yes (Currently commented out in code)
- **Description**: Retrieves a specific assessment type for a patient
- **URL Parameters**: 
  - `id=[integer]` where `id` is the PatientId
  - `tableType=[string]` where `tableType` is one of: "adl", "behaviour", "cognitive", "elimination", "mobility", "nutrition", "progressnote", "safety", "skinandsensoryaid"
- **Example Request**:
  ```bash
  curl -X GET \
    "http://localhost:5232/api/Patients/nurse/patient/1/cognitive" \
    -H "Authorization: Bearer {jwt_token}"
  ```
- **Success Response**: 
  ```json
  {
    "cognitiveId": 1,
    "speech": "Clear and coherent",
    "loc": "Alert and oriented x4",
    "mmse": "28/30",
    "confusion": "No signs of confusion"
  }
  ```
- **Error Responses**:
  - **400 Bad Request**: If invalid table type is provided
    ```json
    { "message": "Invalid table type" }
    ```
  - **404 Not Found**: If patient or table data not found
    ```json
    "Table not found"
    ```
  - **500 Internal Server Error**: If server error occurs
    ```json
    {
      "message": "Error retrieving data",
      "error": "Error message"
    }
    ```

### Get All Patients (Admin)
- **URL**: `/api/Patients/admin/ids`
- **Method**: `GET`
- **Auth Required**: Yes
- **Required Role**: Admin
- **Description**: Returns a list of all patients in the system (admin access only)
- **Example Request**:
  ```bash
  curl -X GET \
    "http://localhost:5232/api/Patients/admin/ids" \
    -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW5AbnVyc2luZy5lZHUiLCJqdGkiOiJjZDZhNWE2OC00NzM5LTQ4YmItYWNmMC1lNjQyMDNiZTZkNDciLCJOdXJzZUlkIjoiMiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzQyNjExOTY1LCJpc3MiOiJOdXJzaW5nRWR1Y2F0aW9uYWxCYWNrZW5kIiwiYXVkIjoiTnVyc2luZ0VkdWNhdGlvbmFsQXBwIn0.7vR4EE9vzc8vC9XOgQ9XlZQZimzp8s9-hQdOSVAX9Hc" \
    -H "Content-Type: application/json"
  ```
- **Success Response**: 
  ```json
  [
    {
      "patientId": 1,
      "patientWristId": "W-0001", 
      "fullName": "John Doe",
      "dob": "1975-07-04",
      "nurseId": 1,
      "gender": "Male",
      "roomNumber": "101",
      "admitDate": "2023-05-15",
      "diagnosis": "Hypertension",
      "allergies": "Penicillin",
      "code": "Full Code"
    }
  ]
  ```
- **Error Responses**:
  - **401 Unauthorized**: If user is not authenticated
  - **403 Forbidden**: If user doesn't have Admin role
  - **500 Internal Server Error**: If server error occurs
    ```json
    {
      "message": "Error retrieving patients",
      "error": "Error message"
    }
    ```

### Debug: List All Database Tables (Admin)
- **URL**: `/api/Patients/debug/tables`
- **Method**: `GET`
- **Auth Required**: Yes
- **Required Role**: Admin
- **Description**: Returns database information for debugging purposes
- **Example Request**:
  ```bash
  curl -X GET \
    "http://localhost:5232/api/Patients/debug/tables" \
    -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW5AbnVyc2luZy5lZHUiLCJqdGkiOiJjZDZhNWE2OC00NzM5LTQ4YmItYWNmMC1lNjQyMDNiZTZkNDciLCJOdXJzZUlkIjoiMiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzQyNjExOTY1LCJpc3MiOiJOdXJzaW5nRWR1Y2F0aW9uYWxCYWNrZW5kIiwiYXVkIjoiTnVyc2luZ0VkdWNhdGlvbmFsQXBwIn0.7vR4EE9vzc8vC9XOgQ9XlZQZimzp8s9-hQdOSVAX9Hc" \
    -H "Content-Type: application/json"
  ```
- **Success Response**: 
  ```json
  {
    "databaseProvider": "Microsoft.EntityFrameworkCore.Sqlite",
    "availableTables": [
      "AspNetRoleClaims",
      "AspNetRoles",
      "AspNetUserClaims",
      "AspNetUserLogins",
      "AspNetUserRoles",
      "AspNetUsers",
      "AspNetUserTokens",
      "Patients",
      "Nurses",
      "Cognitives",
      "Nutritions",
      "Eliminations",
      "Mobilities",
      "Safeties",
      "Adls",
      "SkinAndSensoryAids",
      "Behaviours",
      "ProgressNotes",
      "Records"
    ],
    "connectionInfo": "Data Source=nursing.db",
    "dbContextType": "NursingEducationalBackend.Models.NursingDbContext",
    "models": {
      "patientsDbSet": "Registered",
      "patientEntityType": "Patients"
    }
  }
  ```
- **Error Responses**:
  - **401 Unauthorized**: If user is not authenticated
  - **403 Forbidden**: If user doesn't have Admin role
  - **500 Internal Server Error**: If server error occurs
    ```json
    {
      "message": "Error listing tables",
      "error": "Error message",
      "stackTrace": "Stack trace details",
      "innerException": "Inner exception message"
    }
    ```

## API Security

All endpoints except for registration and login require JWT token authentication. The token must be included in the Authorization header as a Bearer token.

### Role-Based Access Control
- **Admin-only endpoints**: Paths containing `/admin/` or endpoints like `/api/PatientCreate` require the "Admin" role
- **Nurse endpoints**: Paths containing `/nurse/` can be accessed by any authenticated user, but they will only show patients assigned to that nurse or patients with no nurse assignment

### JWT Token Claims
- The token includes the nurse's ID in the "NurseId" claim
- Admin users have the "Admin" role claim
- The token expiration is set by the server configuration