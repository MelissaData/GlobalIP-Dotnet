# Melissa - Global IP Cloud API

## Purpose
This code showcases the Melissa Global IP Cloud API using C#.

Please feel free to copy or embed this code to your own project. Happy coding!

For the latest Melissa Global IP release notes, please visit: https://releasenotes.melissa.com/cloud-api/global-ip/

For further documentation, please visit: https://www.melissa.com/quickstart-guides/ip-locator

The console will ask the user for:

- IP

And return information of the IP such as:

- City
- ConnectionSpeed
- ConnectionType
- Continent
- CountryAbbreviation
- CountryName
- DomainName
- DST
- IPAddress
- ISPName
- Latitude and Longitude
- PostalCode
- ProxyDescription
- ProxyType
- Region
- Result
- TimeZoneCode and TimeZoneName
- UTC

## Tested Environments
- Windows 64-bit .NET 7.0
- Global IP Cloud API Version 6.2.0.1122

## Getting Started
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Install the Dotnet Core SDK

Before starting, make sure that the .NET 7.0 SDK has been correctly installed on your machine (If you have Visual Studio installed, you most likely have it already). If you are unsure, you can check by opening a command prompt window and typing the following:

`dotnet --list-sdks`

If the .NET 7.0 SDK is already installed, you should see it in the following list:

![alt text](/screenshots/dotnet_output.png)

As long as the above list contains version `7.0.xxx` (underlined in red), then you can skip to the next step. If your list does not contain version 7.0, or you get any kind of error message, then you will need to download and install the .NET 7.0 SDK from the Microsoft website.

To download, follow this link: https://dotnet.microsoft.com/en-us/download/dotnet

Select `.NET 7.0` and you will be navigated to the download page.

Click and download the `x64` SDK installer for your operating system.

(IMPORTANT: Make sure you download the SDK, NOT the runtime. the SDK contains both the runtime as well as the tools needed to build the project.)

![alt text](/screenshots/net7.png)

Once clicked, your web browser will begin downloading an installer for the SDK. Run the installer and follow all of the prompts to complete the installation (your computer may ask you to restart before you can continue). Once all of that is done, you should be able to verify that the SDK is installed with the `dotnet --list-sdks` command.

### Download this project
```
$ git clone https://github.com/MelissaData/GlobalIP-Dotnet.git
$ cd GlobalIP-Dotnet
```

## Run Powershell Script
Parameters:
- -ip: an input IP 
 	
  This is convenient when you want to get results for a specific IP in one run instead of testing multiple records in interactive mode.  

- -license (optional): a license string to test the Cloud API

There are two modes:

- Interactive 

	The script will prompt the user for input(s), then use the provided input(s) to call the Cloud API. For example:
	```
	$ .\GlobalIP.ps1
	```

- Command Line 

	You can pass an IP and license string into `-ip`, and `-license` parameters respectively to test the Cloud API. For example:
	```
    $ .\GlobalIP.ps1 -ip "12.203.219.6"
    $ .\GlobalIP.ps1 -ip "12.203.219.6" -license "<your_license_string>"
    ```
	
This is the expected output from a successful setup for interactive mode:

![alt text](/screenshots/output.png)

## Result Codes
For details about the result codes please refer to https://wiki.melissadata.com/index.php?title=Result_Code_Details#IP_Locator

## Contact Us
For free technical support, please call us at 800-MELISSA ext. 4 (800-635-4772 ext. 4) or email us at tech@melissa.com.

To purchase this product, contact the Melissa sales department at 800-MELISSA ext. 3 (800-635-4772 ext. 3).
