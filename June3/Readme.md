# FirstAPI Project – Working Flow & Architecture

This document explains the **end-to-end flow** of your ASP.NET Core API project (`FirstAPI` under `June3`), focusing on how data moves through the system, with code references and explanations for each major feature. All flows are described as they happen when using **Swagger**.

---

## 1. Authentication & Authorization

### **Flow Overview**
- **User submits login credentials** via Swagger.
- **Controller** receives the request and calls `AuthenticationService`.
- **AuthenticationService**:
  - Fetches user from the database.
  - Encrypts the provided password using `EncryptionService`.
  - Compares the encrypted password with the stored hash.
  - If valid, generates a JWT token using `TokenService`.
- **JWT token** is returned to the user.
- For protected endpoints, **Authorization Middleware** checks the JWT and applies policies (e.g., `"ExperiencedDoctorOnly"`).

### **Key Code Involved**
- `Controllers/AuthController.cs` (assumed for login endpoint)
- `Services/AuthenticationService.cs`
- `Services/EncryptionService.cs`
- `Services/TokenService.cs`
- `Year-Authorization/ExperienceHandler.cs` (custom policy)
- `Program.cs` (policy registration)

---

## 2. Authentication, Encryption, Token Services

### **AuthenticationService**
- Handles login logic.
- Uses `_userRepository` to fetch user.
- Uses `_encryptionService` to hash the password.
- Uses `_tokenService` to generate JWT.

### **EncryptionService**
- Hashes passwords using HMACSHA256.
- Uses a hash key for both registration and login.
- Returns both the hashed password and the key.

### **TokenService**
- Reads secret key from config.
- Builds claims (username, role).
- Creates and signs a JWT token.
- Returns the token string.

---

## 3. Flow of Adding & Deleting an Appointment

### **Adding an Appointment**
1. **User submits appointment request** (with patient email, password, doctor name, time).
2. `AppointmentService.AddAppointment`:
   - Fetches patient and user by email.
   - Encrypts and verifies password.
   - Fetches doctor by name.
   - Checks for existing appointment at the same time.
   - If all checks pass, creates and saves a new appointment.
3. **Returns** the new appointment details.

### **Deleting an Appointment (Soft Delete)**
1. **Doctor calls DELETE endpoint** in Swagger with appointment number and JWT token.
2. `DoctorController.DeleteAppointment`:
   - Checks authorization policy (`ExperiencedDoctorOnly`).
   - Fetches appointment by number.
   - Sets `Status = "Cancelled"` (soft delete).
   - Updates the appointment in the repository.
3. **Returns** success or error message.

---

## 4. How a User is Authenticated

- User provides username and password.
- `AuthenticationService` fetches user, hashes the input password with the stored hash key, and compares.
- If matched, a JWT token is generated and returned.
- JWT is used for all subsequent requests to protected endpoints.

---

## 5. How Doctor Speciality is Added

- **Doctor, Speciality, and DoctorSpeciality** tables/entities are involved.
- When adding a doctor with a speciality:
  - The doctor is created (if not exists).
  - The speciality is created (if not exists).
  - An entry is added to the DoctorSpeciality join table.
- This is typically handled in `DoctorService.AddDoctor` (not shown, but inferred).

---

## 6. Flow of Mapper Files (Custom & AutoMapper)

- **AutoMapper** is used for mapping between DTOs and entities (e.g., `DoctorAddRequestDto` to `Doctor`).
- **Custom mapping** is used where logic is too complex for AutoMapper or needs manual intervention.
- **Efficiency**: AutoMapper is efficient for simple property-to-property mapping; custom mapping is used for special cases (e.g., combining fields, conditional logic).

---

## 7. DTOs

- **DTOs (Data Transfer Objects)** are used to:
  - Receive data from the client (e.g., `UserLoginRequest`, `AppointmentRequestDTO`).
  - Send data to the client (e.g., `UserLoginResponse`, `DoctorsBySpecialityResponseDto`).
- DTOs help decouple the API contract from the database models.

---

## 8. Flow of Repository, Services, Controllers, Contexts

- **Controller** receives HTTP request and calls the appropriate **Service**.
- **Service** contains business logic and interacts with **Repositories**.
- **Repository** handles data access (CRUD) with the **DbContext** (`ClinicContext`).
- **DbContext** manages database connections and entity tracking.

**Example:**
```
Swagger → Controller → Service → Repository → DbContext → Database
```

---

## 9. Dependency Injections

- All services, repositories, and context are registered in **DI container** (in `Program.cs`).
- They are injected into constructors where needed.
- Promotes loose coupling and testability.

---

## 10. Program.cs

- Configures services, repositories, AutoMapper, authentication, and authorization policies.
- Sets up middleware for routing, authentication, and Swagger.
- Registers custom authorization handlers (e.g., `ExperienceHandler`).

---

## 11. Patient Registration & Management

- **Patient registration** is handled via a dedicated endpoint (e.g., `PatientController`).
- The controller receives patient registration data (DTO), calls the service, which:
  - Validates and creates a new patient entity.
  - Hashes the password using `EncryptionService`.
  - Stores patient and user data in the database.
- **Updating patients** follows a similar flow, using DTOs for updates.

---

## 12. Doctor Registration & Management

- **Doctor registration** is managed via `DoctorController` and `DoctorService`.
- The controller receives a `DoctorAddRequestDto`, calls the service, which:
  - Creates a new doctor entity.
  - Handles speciality assignment (see section 5).
- **Updating doctors** is similar, using update DTOs and service methods.

---

## 13. Appointment Listing/Querying

- Endpoints exist to **list appointments** by patient, doctor, or date.
- The controller receives query parameters, calls the service, which:
  - Fetches appointments from the repository using filters.
  - Maps entities to DTOs for the response.

---

## 14. Role Management

- Multiple roles are supported (e.g., Admin, Doctor, Patient).
- Roles are assigned as claims in JWT tokens.
- `[Authorize(Roles = "Doctor")]` or custom policies restrict endpoint access.
- Role checks are enforced in controllers and policies.

---
## 15. Error Handling & Logging

- Errors are logged using `ILogger<T>` in services and controllers.
- Exceptions are caught and returned as `BadRequest` or `NotFound` responses.
- For global error handling, middleware can be used (not shown, but recommended).

---

## 16. Validation

- Input validation is handled using **DataAnnotations** on DTOs (e.g., `[Required]`, `[EmailAddress]`).
- Controllers automatically validate incoming models and return errors if invalid.
- For advanced scenarios, libraries like **FluentValidation** can be integrated.

---

## 17. Swagger Customization

- **Swagger** is configured in `Program.cs` for API documentation and testing.
- Security schemes are set up to allow JWT token input for authorized endpoints.
- Endpoints are grouped and described for clarity.

---

## 18. JWT Token Generation
- **Token generation** is handled by `TokenService`.
- Tokens are created with claims (e.g., roles, user ID).
- Tokens are stored in memory for demonstration purposes; consider a secure storage solution.
- Tokens are validated on each request to ensure authenticity.

---

## 19. JWT Token Validation
- **Token validation** is performed in `TokenService`.
- Tokens are checked for expiration, signature, and claims.
- Invalid tokens result in a `401 Unauthorized` response.
- Valid tokens are used to authenticate requests.

---


## **Summary Table**

| Layer         | Example File/Type                | Responsibility                              |
|---------------|----------------------------------|---------------------------------------------|
| Controller    | `DoctorController.cs`            | Handles HTTP requests/responses             |
| Service       | `AppointmentService.cs`          | Business logic, validation                  |
| Repository    | `IRepository<T>`                 | Data access (CRUD)                          |
| Context       | `ClinicContext.cs`               | Database context (EF Core)                  |
| DTO           | `AppointmentRequestDTO.cs`       | Data transfer between client and server     |
| Mapper        | `AutoMapperProfile.cs`           | Object-object mapping                       |
| Auth Service  | `AuthenticationService.cs`       | Login, password check, token generation     |
| Encryption    | `EncryptionService.cs`           | Password hashing                            |
| Token         | `TokenService.cs`                | JWT creation                                |
| Authorization | `ExperienceHandler.cs`           | Custom policy for doctor experience         |
| Program       | `Program.cs`                     | App configuration, DI, middleware           |

---

## **Typical Request Flow Example (Add Appointment)**

1. **Swagger**: User submits appointment request.
2. **Controller**: Receives and validates request.
3. **AppointmentService**: Handles business logic (patient/doctor lookup, password check, slot check).
4. **Repository**: Adds appointment to database.
5. **DbContext**: Saves changes.
6. **Controller**: Returns response to Swagger.

---

## **Typical Request Flow Example (Delete Appointment)**

1. **Swagger**: Doctor sends DELETE request with JWT.
2. **Authorization Middleware**: Validates token and experience policy.
3. **Controller**: Fetches and soft-deletes appointment.
4. **Repository**: Updates appointment status.
5. **Controller**: Returns response to Swagger.

---



---