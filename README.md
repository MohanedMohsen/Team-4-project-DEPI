# Educational Management System (EMS)

**A modular educational management application built with C# and .NET, designed to organize academic workflows through object-oriented design and structured data management.**

[Overview](#overview) · [Features](#planned-features) · [Architecture](#architecture) · [Tech Stack](#technology-stack) · [Project Structure](#project-structure) · [Getting Started](#getting-started) · [Contributing](#development-workflow)

---

## Overview

The **Educational Management System (EMS)** is a team software engineering project developed as part of the **Digital Egypt Pioneers Initiative (DEPI)**.

The system aims to provide a centralized environment for managing academic activities and connecting students, doctors (instructors), and teaching assistants through organized educational workflows.

It is designed to bring course management, student enrollment, assignments, submissions, grading, and academic communication into one application, with access to relevant features determined by each user's role and permissions.

The project emphasizes maintainable software design, object-oriented programming, relational database management, and collaborative development practices.

## Planned Features

### User Management and Authentication
- Manage user accounts and identity.
- Support student, doctor, and teaching assistant roles.
- Apply authentication and authorization rules to protected operations.

### Courses and Enrollment
- Manage courses and their responsible instructors.
- Organize student enrollment records.
- Maintain course-related information and relationships.

### Assignments and Submissions
- Create and manage course assignments.
- Track assignment deadlines and publication status.
- Manage student submissions and their status.

### Grading
- Record grades and instructor feedback.
- Associate grading records with the relevant submissions.
- Keep grading rules separate from unrelated application logic.

### Communication
- Publish academic announcements.
- Support messages and notifications where required.
- Associate communication with the relevant users and courses.

> **Note:** These are planned capabilities. Each feature will be marked as implemented only after development and verification.

## User Roles

| Role | Intended Responsibilities |
|---|---|
| Student | Access enrolled courses, view assignments, submit work, and view published grades and feedback. |
| Doctor (Instructor) | Manage assigned courses, create assignments, grade submissions, and publish course announcements. |
| Teaching Assistant (TA) | Support assigned academic activities within the permissions defined by the project requirements. |

The final permissions and workflows will be determined by the approved requirements.

## Architecture

The application is planned as a **modular monolith** with separated responsibilities. This keeps the project manageable for a team while allowing its components to evolve independently.

### Planned Layers

- **Domain** — Core entities, enums, and business rules.
- **Application** — Use cases, application services, and abstractions required by the system.
- **Infrastructure** — SQL Server integration, ADO.NET data access, and repository implementations.
- **Presentation** — Console menus, user input, and output.

### Dependency Direction

The intended dependency structure is:

- Presentation → Application
- Application → Domain
- Infrastructure → Application abstractions and Domain types where required

The Domain layer will not depend on database access, console interaction, or Infrastructure implementations.

### Design Principles

The design aims to apply:

- **Encapsulation** — Protect object state and enforce valid operations.
- **Abstraction** — Define meaningful contracts and hide implementation details.
- **Inheritance and Polymorphism** — Reuse shared behavior and support specialized behavior where appropriate.
- **SOLID** — Improve maintainability, extensibility, and separation of responsibilities.
- **Separation of Concerns** — Keep business rules, application workflows, persistence, and presentation independent.

The implementation will follow the finalized UML Class Diagram and System Design, subject to validated requirements.

## Technology Stack

| Technology | Purpose |
|---|---|
| C# | Application development |
| .NET | Application runtime and development platform |
| Microsoft SQL Server | Relational data storage |
| ADO.NET | Database connectivity and data access |
| Git | Version control |
| GitHub | Repository hosting, collaboration, and code review |

The exact .NET SDK version and development environment will be documented after the team finalizes the setup.

## Project Structure

The planned solution structure is:

```text
EducationalManagementSystem/
├── EducationalManagementSystem.sln
├── README.md
├── .gitignore
├── .editorconfig
│
├── docs/
│   ├── diagrams/
│   ├── database/
│   └── decisions/
│
├── database/
│   ├── scripts/
│   └── README.md
│
├── src/
│   ├── EMS.Domain/
│   ├── EMS.Application/
│   ├── EMS.Infrastructure/
│   └── EMS.Presentation.Console/
│
└── tests/
    ├── EMS.Domain.Tests/
    └── EMS.Application.Tests/
```

This structure is the planned organization. The actual repository will be updated to reflect the projects and files that have been created.

## Database Design

Microsoft SQL Server is planned to store the system's persistent data, including users, courses, enrollments, assignments, submissions, grades, and communication records.

Database design will define:

- Primary and foreign keys.
- Relationships and cardinalities.
- Unique constraints and data integrity rules.
- Appropriate validation and transaction boundaries.

ADO.NET database operations will be isolated in the Infrastructure layer. Credentials and sensitive connection details must not be committed to the repository.

Database scripts will be documented before they are used. Any script that drops, recreates, or deletes data must be clearly identified and used only in an appropriate development or test environment.

## Getting Started

The project setup is currently being organized. Verified installation, configuration, database initialization, and execution instructions will be added once the initial solution is available.

The intended setup process is:

1. Install the required .NET SDK.
2. Clone the repository.
3. Open the solution in Visual Studio or a compatible .NET development environment.
4. Configure the SQL Server connection using local configuration that is not committed to Git.
5. Initialize the development database using the documented scripts.
6. Build and run the console application.

**The exact commands and database configuration will be documented after they have been tested against the actual solution.**

## Documentation

Design and development documentation will be maintained under `docs/`.

- **UML Class Diagram** — Domain classes, attributes, operations, and relationships.
- **System Design** — Architectural components and their interactions.
- **Database Design** — Tables, relationships, and integrity constraints.
- **Architecture Decisions** — Important design choices, assumptions, and their rationale.

Editable source files will be preserved alongside exported diagrams where applicable.

## Development Workflow

The team follows a feature-branch workflow to keep `main` stable.

1. Update the local `main` branch.
2. Create a branch for a focused feature or task.
3. Implement and test the changes.
4. Commit with a meaningful message.
5. Push the branch to GitHub.
6. Open a Pull Request targeting `main`.
7. Review the changes and resolve feedback before merging.

Direct pushes to `main` are not part of the intended workflow.

Branch protection and required review settings will be configured according to the team's GitHub permissions and workflow.

## Testing and Quality

Testing will be introduced alongside implementation to verify domain behavior and application workflows.

The team aims to maintain:

- Consistent coding conventions.
- Clear separation of responsibilities.
- Meaningful validation and error handling.
- Reviewable, focused commits.
- Automated tests for appropriate business rules and use cases.

Test coverage and build status will be reported only when supported by actual test and build results.

## Project Status

**Current Phase:** Repository Setup and System Design

The team is preparing the repository, validating the design, and establishing the development workflow before implementing the planned modules.

## Team

Developed collaboratively by the DEPI project team.

Team member names, responsibilities, and contribution details will be added as the work is assigned and confirmed.

---

*This project is intended for educational and software engineering practice as part of the Digital Egypt Pioneers Initiative (DEPI).*
