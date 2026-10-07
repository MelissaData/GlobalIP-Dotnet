#!/bin/bash

# Builds and runs the Melissa Global IP Cloud API .NET sample.
#
# This script builds GlobalIPDotnet with dotnet publish, then runs the resulting
# executable, passing along the license and (if supplied) the IP address.
#
# Overall flow:
#   1. Parse the command-line options below.
#   2. Resolve the license (--license, then a prompt, then the MD_LICENSE environment variable).
#   3. Publish GlobalIPDotnet in Release configuration to ./GlobalIPDotnet/Build.
#   4. Run the built executable: one-shot mode if any IP addre was supplied,
#      otherwise interactive mode (the .NET program prompts for each field).
#
# Options (each takes a value):
#   --ip        IP address to locate.
#   --license   License string. If omitted, the script prompts for it; if the prompt
#               is left blank, it falls back to MD_LICENSE. Running without --license
#               always prompts, even when MD_LICENSE is set.
#
# Paths are relative to the current directory, so run the script from its own folder.
#
# Examples:
#   ./GlobalIPDotnet.sh --license "your-license"
#   ./GlobalIPDotnet.sh --ip "12.203.219.6" --license "your-license"

######################### Constants ##########################

RED='\033[0;31m' #RED
NC='\033[0m' # No Color

######################### Parameters ##########################

ip=""
license=""

# Read each --flag and its value. A flag with no value, or whose value starts
# with "-", is an error. Unrecognized options are ignored.
while [ $# -gt 0 ] ; do
  case $1 in
    --ip) 
        if [ -z "$2" ] || [[ $2 == -* ]];
        then
            printf "${RED}Error: Missing an argument for parameter \'ip\'.${NC}\n"  
            exit 1
        fi 

        ip="$2"
        shift
        ;;
    --license) 
        if [ -z "$2" ] || [[ $2 == -* ]];
        then
            printf "${RED}Error: Missing an argument for parameter \'license\'.${NC}\n"  
            exit 1
        fi 

        license="$2"
        shift 
        ;;
  esac
  shift
done

# Build paths are relative to the current directory (not the script's location)
CurrentPath="$(pwd)"
ProjectPath="$CurrentPath/GlobalIPDotnet"
BuildPath="$ProjectPath/Build"

if [ ! -d "$BuildPath" ];
then
    mkdir $BuildPath
fi

########################## Main ############################
printf "\n==================== Melissa Global Ip Cloud API =====================\n"

# Get license (either from parameters or user input)
if [ -z "$license" ];
then
  printf "Please enter your license string: "
  read license
fi

# Check for License from Environment Variables 
if [ -z "$license" ];
then
  license=`echo $MD_LICENSE` 
fi

if [ -z "$license" ];
then
  printf "\nLicense String is invalid!\n"
  exit 1
fi

# Start program
# Build project
printf "\n============================ BUILD PROJECT ===========================\n"

dotnet publish -f="net7.0" -c Release -o "$BuildPath" GlobalIPDotnet/GlobalIPDotnet.csproj

# Run project
# No IP address supplied -> run interactively; otherwise pass it through for one-shot mode.
if [ -z "$ip" ];
then
    dotnet "$BuildPath"/GlobalIPDotnet.dll --license "$license"
else
    dotnet "$BuildPath"/GlobalIPDotnet.dll --license "$license" --ip "$ip"
fi


