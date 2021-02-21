$ErrorActionPreference = "Stop"

# consumer contract tests

dotnet test .\consumer\consumer.test.contract\
if ($LastExitCode -ne 0) { Exit $LastExitCode }

# consumer contract tests nunit

dotnet test .\consumer\consumer.test.contract.nunit\
if ($LastExitCode -ne 0) { Exit $LastExitCode }

# provider contract tests

dotnet test .\provider\Provider.Test\
if ($LastExitCode -ne 0) { Exit $LastExitCode }
