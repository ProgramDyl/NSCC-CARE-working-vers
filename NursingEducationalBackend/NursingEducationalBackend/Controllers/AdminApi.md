# Admin API Documentation

This document outlines the Admin API endpoints for the Nursing Educational Backend application. These endpoints are secured and can only be accessed by users with the `Admin` role.

## Base URL

```
/api/Admin
```

## Authorization

All endpoints require a valid JWT token with Admin role claims.

```
Authorization: Bearer <your_token>
```

## Endpoints

### 1. Delete User

Removes a user and their associated nurse record from the system.

- **URL**: `/users/{email}`
- **Method**: `DELETE`
- **URL Parameters**:
  - `email`: The email address of the user to delete

#### Success Response

- **Code**: 200 OK
- **Content**:
  ```json
  {
    "success": true,
    "message": "User deleted successfully."
  }
  ```

#### Error Responses

- **Code**: 404 Not Found
  ```json
  {
    "success": false,
    "message": "User not found."
  }
  ```

- **Code**: 400 Bad Request
  ```json
  {
    "success": false,
    "message": "Failed to delete user.",
    "errors": [
      {
        "code": "UserDeletionError",
        "description": "Error details..."
      }
    ]
  }
  ```

- **Code**: 500 Internal Server Error
  ```json
  {
    "success": false,
    "message": "An error occurred while deleting the user.",
    "error": "Error details..."
  }
  ```

### 2. Reset User Password

Resets the password for a specified user.

- **URL**: `/users/{email}/reset-password`
- **Method**: `POST`
- **URL Parameters**:
  - `email`: The email address of the user
- **Request Body**:
  ```json
  {
    "newPassword": "NewSecurePassword123"
  }
  ```

#### Success Response

- **Code**: 200 OK
- **Content**:
  ```json
  {
    "success": true,
    "message": "Password reset successfully."
  }
  ```

#### Error Responses

- **Code**: 404 Not Found
  ```json
  {
    "success": false,
    "message": "User not found."
  }
  ```

- **Code**: 400 Bad Request
  ```json
  {
    "success": false,
    "message": "Password reset failed.",
    "errors": [
      {
        "code": "PasswordRequiresDigit",
        "description": "Error details..."
      }
    ]
  }
  ```

### 3. Reset Admin's Own Password

Allows an admin to change their own password.

- **URL**: `/reset-my-password`
- **Method**: `POST`
- **Request Body**:
  ```json
  {
    "currentPassword": "CurrentSecurePassword123",
    "newPassword": "NewSecurePassword456"
  }
  ```

#### Success Response

- **Code**: 200 OK
- **Content**:
  ```json
  {
    "success": true,
    "message": "Password changed successfully."
  }
  ```

#### Error Responses

- **Code**: 404 Not Found
  ```json
  {
    "success": false,
    "message": "User not found."
  }
  ```

- **Code**: 400 Bad Request
  ```json
  {
    "success": false,
    "message": "Password change failed.",
    "errors": [
      {
        "code": "PasswordMismatch",
        "description": "Error details..."
      }
    ]
  }
  ```

### 4. Update Patient Information

Updates a patient's information, except for the PatientId.

- **URL**: `/patients/{patientId}`
- **Method**: `PUT`
- **URL Parameters**:
  - `patientId`: The ID of the patient to update
- **Request Body**:
  ```json
  {
    "nurseId": 5,
    "imageFilename": "patient123.jpg",
    "bedNumber": 42,
    "nextOfKin": "Jane Doe",
    "nextOfKinPhone": "555-123-4567",
    "fullName": "John Doe",
    "sex": "Male",
    "patientWristId": "WID123456",
    "dob": "1980-01-15",
    "admissionDate": "2023-05-10",
    "dischargeDate": null,
    "maritalStatus": "Married",
    "medicalHistory": "Hypertension, Diabetes",
    "weight": 75,
    "height": "180cm",
    "allergies": "Penicillin",
    "isolationPrecautions": "None",
    "unit": "Cardiology",
    "roamAlertBracelet": "No"
  }
  ```

#### Success Response

- **Code**: 200 OK
- **Content**:
  ```json
  {
    "success": true,
    "message": "Patient updated successfully.",
    "patient": {
      "patientId": 1,
      "nurseId": 5,
      "imageFilename": "patient123.jpg",
      "bedNumber": 42,
      "nextOfKin": "Jane Doe",
      "nextOfKinPhone": "555-123-4567",
      "fullName": "John Doe",
      "sex": "Male",
      "patientWristId": "WID123456",
      "dob": "1980-01-15",
      "admissionDate": "2023-05-10",
      "dischargeDate": null,
      "maritalStatus": "Married",
      "medicalHistory": "Hypertension, Diabetes",
      "weight": 75,
      "height": "180cm",
      "allergies": "Penicillin",
      "isolationPrecautions": "None",
      "unit": "Cardiology",
      "roamAlertBracelet": "No"
    }
  }
  ```

#### Error Responses

- **Code**: 404 Not Found
  ```json
  {
    "success": false,
    "message": "Patient not found."
  }
  ```

- **Code**: 400 Bad Request
  ```json
  {
    "success": false,
    "message": "Patient wrist ID already exists."
  }
  ```
  or
  ```json
  {
    "fullName": ["The FullName field is required."],
    "weight": ["The field Weight must be between 1 and 1000."]
  }
  ```

- **Code**: 500 Internal Server Error
  ```json
  {
    "success": false,
    "message": "An error occurred while updating the patient.",
    "error": "Error details..."
  }
  ```