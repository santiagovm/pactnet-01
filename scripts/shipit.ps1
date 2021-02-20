$ErrorActionPreference = "Stop"

# consumer contract tests

dotnet test .\consumer\consumer.test.contract\
if ($LastExitCode -ne 0) { Exit $LastExitCode }

# provider contract tests

dotnet test .\provider\provider.test.contract\
if ($LastExitCode -ne 0) { Exit $LastExitCode }
