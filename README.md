# Employee Administration REST API

## Project Overview

This project is a REST API designed to manage employee records and their associated tasks and projects. The system is built with a focus on two user roles: **Employee** and **Administrator**, offering different levels of access and functionality based on the user's role. The primary features of the system include user authentication, task assignment, and project management.

### Key Features:

- **User Authentication**: 
  - Simple login system using JWT authentication.
  - No registration page; user accounts are created by the administrator.
  
- **User Roles**:
  - **Employee**: 
    - Update personal profile information and upload a profile picture.
    - Create tasks that belong to projects they are a part of.
    - Assign tasks to other employees within the same project.
    - Mark tasks as completed.
    - View all tasks related to their projects (read-only for tasks not assigned to them).
  - **Administrator**:
    - Create, update, and remove users, projects, and tasks.
    - Add or remove employees from projects.
    - Assign tasks to employees and manage their completion.
    - Administrators cannot remove projects that have ongoing tasks.

### Technologies Used:

- **.NET SDK 5.0** (v5.0.102)
- **Entity Framework Core** (EFCore & EFCore Tools)
- **Dapper** (Optional, for more flexible query writing)
- **Microsoft SQL Server 2017/2019** 
- **Microsoft SQL Server Management Studio 18**
- **Microsoft Visual Studio 2019**

### Project Structure:

- **Models**: Models are separated into a class library for better organization and are referenced in the Web API project. Model properties are annotated for validation and documentation.
- **Controllers**: API methods are documented and grouped into separate controllers for better clarity. 
- **Services**: Service classes have corresponding interfaces, and dependency injection is used to inject these services into the controllers.
- **Event Handling**: Implemented using the `Abp.Events.Bus` library.

### Project Functionality:

- **Employee**:
  - Can update profile information and upload a profile picture.
  - Can create tasks and assign them to other employees within the same project.
  - Can mark tasks as completed.
  - Can view all tasks related to projects they are involved in (read-only for unassigned tasks).

- **Administrator**:
  - Can create, update, and delete users, projects, and tasks.
  - Can assign employees to projects and manage tasks across the organization.
  - Cannot remove projects if they contain open tasks.

### Setup Instructions:

1. Clone the repository:
   ```bash
   git clone <repository-url>
2. Open the solution in Microsoft Visual Studio.

3. Update the connection string in appsettings.json with your SQL Server configuration.

4. Run database migrations:
   ```bash
   dotnet ef database update
3. Build and run the API.

Documentation:
Each controller contains detailed documentation for its methods using the GroupName annotation to group the methods. Documentation can be accessed via Swagger or similar tools integrated into the project.