$ErrorActionPreference = "Stop"

# consumer contract tests

dotnet test .\ConsumerApp\ConsumerApp.Test\
if ($LastExitCode -ne 0) { Exit $LastExitCode }

# provider contract tests

dotnet test .\Provider\Provider.Test\
if ($LastExitCode -ne 0) { Exit $LastExitCode }
