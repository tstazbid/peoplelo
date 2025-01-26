# Peoplelo

Peoplelo is a web application that allows users to login, register, verify their email, and forget password. The platform includes user management features such as blocking, unblocking, and deleting users, as well as a search functionality for user lists. This project utilizes ASP.NET Core and Bootstrap for the frontend.

## Features

- **User Registration**: Users can create an account by providing a name, email, and password.
- **Email Verification**: Users can verify their email address after registration.
- **User Management**: Any user can block, unblock, or delete others.
- **User Search**: Uesr can filter by name using the search bar.
- **Responsive Design**: Built with Bootstrap for mobile-friendly and responsive UI.
- **Password Visibility Toggle**: Users can toggle the visibility of their password and confirmation password during registration.
- **User Activity**: Displays last seen information for users with relative time display (e.g., "Less than a minute ago", "2 hours ago").

## Prerequisites

Before running the application, ensure you have the following installed:

- [ASP.NET Core SDK](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/) (or any other IDE for .NET development)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or any compatible database)

## Installation

### Clone the Repository

Clone this repository to your local machine:

```bash
git clone https://github.com/tstazbid/peoplelo.git
```

### Set Up the Database

Make sure to configure the connection string in `appsettings.json` to match your database setup.

Example:

```json
{
  "ConnectionStrings": {
    "Default": "Server=(localdb)\\mssqllocaldb;Database=PeopleloDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

Run the migrations to set up the database:

```bash
dotnet ef database update
```

### Install Dependencies

Restore the NuGet packages:

```bash
dotnet restore
```

### Run the Application

After restoring the dependencies, you can run the application:

```bash
dotnet run
```

The application will be available at `https://localhost`.

## Frontend

The frontend of the application is built using Bootstrap for responsiveness. Additionally, Font Awesome icons are used for buttons such as Block, Unblock, and Delete, as well as to toggle the visibility of passwords.

- **Bootstrap**: The application uses Bootstrap's grid system, components, and utilities.
- **FontAwesome**: Icons are used to represent different user management actions like block, unblock, and delete.

## Configuration

- The app uses ASP.NET Core Identity for user authentication and role management.
- Password policies are configured for simplicity (you can adjust this as per your security needs).
- Email verification is required to confirm the user’s identity after registration (under development).

## Scripts

- **Validation Scripts**: The project uses `_ValidationScriptsPartial.cshtml` to include client-side validation scripts that ensure correct input during registration and email verification.
- **Password Visibility Toggle**: The toggle function is implemented using JavaScript to show or hide passwords during registration.

## User Management Actions

- **Block User**: User can block users by clicking on the block icon.
- **Unblock User**: User can unblock users by clicking on the unblock icon.
- **Delete User**: User can delete users by clicking on the trash icon.
- **Select Users**: User can select multiple users to perform actions on them.

## Customization

- Modify the `AccountController` to add or modify the logic for handling user registration, login, and verification.