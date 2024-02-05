# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/hifipc.nopillarbox/*" -Force -Recurse
dotnet publish "./hifipc.nopillarbox.csproj" -c Release -o "$env:RELOADEDIIMODS/hifipc.nopillarbox" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location