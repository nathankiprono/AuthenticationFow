# AuthenticationFlow

A modern ASP.NET Core MVC authentication system with email OTP verification, JWT login, and a beautiful Bootstrap/jQuery UI.

## Tech Choices
- **ASP.NET Core MVC (C#)**: Chosen for its robust, scalable, and secure web application framework.
- **Entity Framework Core**: For ORM and database migrations.
- **SQL Server**: As the backend database.
- **JWT (JSON Web Token)**: For secure, stateless authentication.
- **Bootstrap 5 & Bootstrap Icons**: For responsive, modern UI.
- **jQuery**: For simple AJAX and UI interactivity.
- **SMTP (Gmail)**: For sending OTP emails.

## Folder Structure
- `Controllers/` - MVC controllers for API and web endpoints
- `DTOs/` - Data Transfer Objects for API requests/responses
- `Models/` - Entity models (User, etc.)
- `Services/` - Business logic, user management, OTP/email
- `Data/` - EF Core DbContext and migrations
- `Views/` - Razor views for UI (SignUp, SignIn, VerifyOtp, etc.)
- `wwwroot/` - Static assets (CSS, JS, Bootstrap, icons)
- `appsettings.json` - Configuration (DB, JWT, SMTP)

## Setup & Run
1. Clone the repo: `git clone https://github.com/nathankiprono/AuthenticationFows.git`
2. Update `appsettings.json` with your DB, JWT, and SMTP credentials
3. Run migrations: `dotnet ef database update`
4. Start the app: `dotnet run` or via Visual Studio
5. Visit `https://localhost:<port>`

## Challenges Faced
- **Email deliverability**: Gmail SMTP sometimes blocks sign-in from less secure apps; use app passwords and enable 2FA.
- **OTP security**: For demo, OTP is stored in DB. In production, use expiry and rate limiting.
- **UI/UX**: Making forms both beautiful and functional for all devices.
- **JWT integration**: Ensuring secure token generation and validation.



