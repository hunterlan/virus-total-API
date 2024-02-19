# How to Contribute:

You can contribute to VirusTotalCore app by:
- Report issues and bugs.
- Submit feature requests.
- Creating a pull request.

# How to Build and Run VirusTotalCore from source:

* Make sure your machine is satisfying [.NET 8 requirements](https://github.com/dotnet/core/blob/main/release-notes/8.0/supported-os.md).
* Open `src/VirusTotalCore.sln` with your favourite IDE and set Solution Platform to x64*.
* Create `appsettings.json` and `VirusTotalCore.Tests` and key named `apiKey` and insert your VirusTotal API key as value.
* Try to build and run unit tests.

**If x64 doesn't work, use the architecture of your system*

# Coding

## Code Style

1. DO use `PascalCase`:
- class names
- method names
- const names

2. DO use `camelCase`:
- method arguments
- local variables
- private fields

4. DO NOT use Hungarian notation.

5. DO NOT use underscores, hyphens, or any other non-alphanumeric characters.

6. DO NOT use Caps for any names.

7. DO use predefined type names like `int`, `string` etc. instead of `Int32`, `String`.

8. DO use `_` prefix for private field names.

9. DO use the `I` prefix for Interface names.

10. DO vertically align curly brackets.

11. DO NOT use `Enum` or `Flag(s)` suffix for Enum names.

12. DO use prefix `Is`, `Has`, `Have`, `Any`, `Can` or similar keywords for Boolean names.

13. DO use curly brackets for single line `if`, `for` and `foreach` statements.
