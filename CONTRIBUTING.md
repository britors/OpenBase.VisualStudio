# Contributing to OpenBase for Visual Studio

Thank you for your interest in contributing! We aim to make this the best developer tool for Visual Studio.

## 🛠️ Development Standards

### 1. Modern C# Patterns
We strictly use the latest C# features. All new services should use:
*   **Primary Constructors** for dependency injection.
*   **File-scoped Namespaces** to reduce indentation.
*   **Expression-bodied members** where appropriate.

### 2. Async-First
All IO-bound operations (CLI calls, WebView communication, File access) **must** be asynchronous. Always use `AsyncPackage` and support `CancellationToken` where possible.

### 3. CLI Integration
This extension is an orchestrator. Logic that interacts with databases or networks should ideally live in the `openbase` CLI. The extension's role is to provide a rich UI wrapper around these commands.

## 🌿 Branching Strategy
*   `main`: Stable releases.
*   `develop`: Integration branch for new features.
*   `feature/*`: New features or bug fixes.

## 🧪 Testing
*   Ensure the project compiles with **0 warnings**.
*   Verify changes in the **Experimental Instance** of Visual Studio.
*   Add unit tests in the `tests/` directory for any new logic.

## 📝 Commit Messages
We follow conventional commits:
*   `feat:` for new features.
*   `fix:` for bug fixes.
*   `docs:` for documentation changes.
*   `refactor:` for code changes that neither fix a bug nor add a feature.

---
By contributing, you agree that your contributions will be licensed under the project's MIT License.
