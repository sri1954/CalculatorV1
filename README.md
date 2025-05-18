# Calculator Solution
A simple and functional calculator application built with C# and .NET, designed to perform basic arithmetic operations with a clean and intuitive interface.

# 🔍 Project Overview
Calculator Solution is a multi-layered .NET solution that demonstrates the integration of various project templates and C# development skills. 
It combines a Web API, a Class Library, and Unit Testing using NUnit, all orchestrated to follow clean architecture principles and best practices in modern software development.

🧠 C# Concepts Demonstrated
This solution showcases key C# and .NET development practices, structured for scalability, testability, and maintainability.

# 🌐 Web API (ASP.NET Core)
RESTful API Design: The project includes a Web API layer that exposes endpoints for calculator operations.
Routing & Controllers: Uses attribute-based routing to define clear and concise endpoints.
Dependency Injection: Promotes loose coupling and testability by injecting services into controllers.
Model Binding & Validation: Ensures clean data handling and error management.

# 📦 Class Library
Separation of Concerns: Core business logic (e.g., arithmetic operations) is encapsulated in a reusable class library.
Reusability: The library can be referenced by both the Web API and test projects.
Maintainability: Changes to logic are isolated from UI or API layers, making updates safer and easier.

# 🧪 Unit Testing with NUnit
Test-Driven Development (TDD): NUnit is used to write unit tests for the class library.
Assertions & Test Cases: Covers various scenarios including edge cases and invalid inputs.
Test Project Structure: Organized test classes mirror the structure of the class library for clarity.
CI/CD Ready: Tests can be easily integrated into build pipelines for automated validation.

# 🛠️ Skills Demonstrated
Object-Oriented Programming (OOP): Encapsulation, abstraction, and modular design.
API Development: REST principles, controller design, and HTTP communication.
Dependency Injection: Decouples components for better testability and flexibility.
Unit Testing: Test-driven development using NUnit and mocking strategies.
Solution Architecture: Layered structure for scalability and maintainability.
Version Control: Git and GitHub for collaborative development and code management.

# 🔗 Clone the Repository
git clone https://github.com/sri1954/CalculatorV1.git

# 🛠️ Open in Visual Studio
Launch Visual Studio (2022 or later recommended).
Click on "Open a project or solution".
Navigate to the cloned folder and open the .sln file (e.g., CalculatorV1.sln).

# ▶️ Build and Run
Press F5 or click "Start" to build and run the application.
The calculator UI will launch, ready for use.

# ✅ Testing the Application
Try basic operations: addition, subtraction, multiplication, and division.
Validate input handling and error messages.
Explore the code to understand event handling and UI logic.
