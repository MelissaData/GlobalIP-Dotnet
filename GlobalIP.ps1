# Name:    GlobalIPCloudAPI
# Purpose: Execute the GlobalIPCloudAPI program

######################### Parameters ##########################
param(
  $ip = '',
  $license = '',
  [switch]$quiet = $false
)

# Uses the location of the .ps1 file 
# Modify this if you want to use 
$CurrentPath = $PSScriptRoot
Set-Location $CurrentPath
$ProjectPath = "$CurrentPath\GlobalIP"
$BuildPath = "$ProjectPath\Build"

if (!(Test-Path $BuildPath)) {
  New-Item -Path $ProjectPath -Name 'Build' -ItemType "directory"
}

########################## Main ############################
Write-Host "`n==================== Melissa Global Ip Cloud API =====================`n"

# Get license (either from parameters or user input)
if ([string]::IsNullOrEmpty($license) ) {
  $license = Read-Host "Please enter your license string"
}

# Check for License from Environment Variables 
if ([string]::IsNullOrEmpty($license) ) {
  $license = $env:MD_LICENSE 
}

if ([string]::IsNullOrEmpty($license)) {
  Write-Host "`nLicense String is invalid!"
  Exit
}

# Start program
# Build project
Write-Host "`n========================== BUILD PROJECT ============================="

dotnet publish -f="net7.0" -c Release -o $BuildPath GlobalIP\GlobalIP.csproj

# Run project
if ([string]::IsNullOrEmpty($ip)) {
  dotnet $BuildPath\GlobalIP.dll --license $license 
}
else {
  dotnet $BuildPath\GlobalIP.dll --license $license --ip $ip
}
