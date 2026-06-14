# OpenBase for Visual Studio

OpenBase is a powerful, all-in-one extension for Visual Studio designed to streamline database management, API testing, and system monitoring. This project is a functional port of the popular OpenBase extension for VS Code, rebuilt specifically for the Visual Studio ecosystem using WPF and .NET 10.

## 🚀 Key Features

*   **SQL Runner:** A full-featured SQL editor (powered by Monaco) with connection management and a high-performance DataGrid for results.
*   **HTTP Runner:** A complete REST client integrated directly into your IDE, supporting variables and environment synchronization.
*   **ER Diagram:** Instant database visualization using Mermaid.js rendering via WebView2.
*   **Migration Runner:** Interface for EF Core and Fluent Migrator orchestration.
*   **System Monitor:** Real-time view of system metrics and .NET process logs.

## 🛠️ Tech Stack

*   **Language:** C# 13 (.NET 10)
*   **UI:** WPF (Windows Presentation Foundation)
*   **Web Integration:** Microsoft Edge WebView2
*   **CLI Orchestration:** Powered by the `openbase` CLI tool.
*   **Architecture:** Clean, Async-first with Dependency Injection.

## 📦 Getting Started

### Prerequisites
1.  **Visual Studio 2022/2025/2026** with the "Visual Studio extension development" workload.
2.  **.NET 10 SDK**.
3.  **OpenBase CLI** installed and available in your system PATH.

### Installation
Currently, the project is in early development. To run it:
1.  Clone this repository.
2.  Open `OpenBase.VisualStudio.slnx`.
3.  Press `F5` to start the Experimental Instance of Visual Studio.

## 📜 Roadmap
- [x] Phase 1: Foundation (DI, Logging, CLI Integration)
- [x] Phase 2: WebView2 Infrastructure
- [x] Phase 3: SQL Runner (Core Logic)
- [x] Phase 4: HTTP Runner (Core Logic)
- [x] Phase 5: ER Diagram (Mermaid Rendering)
- [ ] Phase 6: Monaco Editor Integration & Themes
- [ ] Phase 7: Migration & Scaffold Tools
- [ ] Phase 8: Options & Settings

## 📄 License
This project is licensed under the MIT License - see the LICENSE file for details.
