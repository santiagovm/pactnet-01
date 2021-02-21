$ErrorActionPreference = "Stop"

# consumer contract tests

dotnet test .\Consumer\Consumer.Test.XUnit\
if ($LastExitCode -ne 0) { Exit $LastExitCode }

# consumer contract tests nunit

dotnet test .\Consumer\Consumer.Test.NUnit\
if ($LastExitCode -ne 0) { Exit $LastExitCode }

# provider contract tests

dotnet test .\Provider\Provider.Test\
if ($LastExitCode -ne 0) { Exit $LastExitCode }
