<#
.SYNOPSIS
    Builds and runs the Melissa Global IP Cloud API .NET sample.

.DESCRIPTION
    This script builds GlobalIPDotnet with dotnet publish, then runs the
    resulting executable, passing along the license and (if supplied) the IP address.

    Overall flow:
      1. Resolve the license (parameter, prompt, or MD_LICENSE environment variable).
      2. Publish GlobalIPDotnet in Release configuration to
         .\GlobalIPDotnet\Build.
      3. Run the built executable: one-shot mode if -ip was supplied,
         otherwise interactive mode (the .NET program prompts for the IP).

.PARAMETER ip
    IP address to look up in one-shot mode.

.PARAMETER license
    License string. Resolved in this order:
      1. This parameter.
      2. An interactive prompt, if the parameter was not supplied.
      3. The MD_LICENSE environment variable, if the prompt was left blank.
    Note that the environment variable is the last resort, not the first: running
    without -license always prompts, even when MD_LICENSE is set.

.PARAMETER quiet
    Accepted for parity with other sample scripts; not currently used to suppress output.

.EXAMPLE
    .\GlobalIPDotnet.ps1 -license "your-license"

.EXAMPLE
    .\GlobalIPDotnet.ps1 -ip "12.203.219.6" -license "your-license"
#>

######################### Parameters ##########################
param(
  $ip = '',
  $license = '',
  [switch]$quiet = $false
)

# Uses the location of the .ps1 file
$CurrentPath = $PSScriptRoot
Set-Location $CurrentPath
$ProjectPath = "$CurrentPath\GlobalIPDotnet"
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

dotnet publish -f="net7.0" -c Release -o $BuildPath GlobalIPDotnet\GlobalIPDotnet.csproj

# Run project
# No IP supplied -> run interactively; otherwise pass it through for one-shot mode.
if ([string]::IsNullOrEmpty($ip)) {
  dotnet $BuildPath\GlobalIPDotnet.dll --license $license 
}
else {
  dotnet $BuildPath\GlobalIPDotnet.dll --license $license --ip $ip
}
