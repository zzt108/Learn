#Deploys UTF
Function Deploy-UTF(	
							[Parameter(Mandatory=$true )][string] $solutionFolder, # test solution root
	                        [Parameter(Mandatory=$true )][string] $projectFolder = 'Experience Editor Smoke Tests',
	                        [Parameter(Mandatory=$true )][string] $seleniumFolder, # = "$projectFolder\Components\Selenium\2.40x64",
                            [Parameter(Mandatory=$true )][string] $targetDir,
							[Parameter(Mandatory=$true )][string] $instanceName,  
							[Parameter(Mandatory=$true )][string] $helperWebServiceProject
						)
{
<#
.SYNOPSIS 
Performs Sitecore UTF and Selenium deploy on specified instance.

.DESCRIPTION
Function performs several actions:
- Copy HelperWebService to target instance
- Copy Selenium dlls to target instance

.PARAMETER solutionFolder
Specifies the folder of UTF solution.

.PARAMETER instanceName
Specify path to folder where Sitecore site is placed.

.EXAMPLE
to be added
#>

    #$targetDir = Join-Path -Path $instanceRoot -ChildPath $instanceName

    $helperWebServiceFolder = Join-Path -Path $solutionFolder -ChildPath $helperWebServiceProject
    $helperWebServiceAsmx = Join-Path -Path $helperWebServiceFolder -ChildPath "\HelperWebService.asmx"
    $helperWebServiceDll = Join-Path -Path $helperWebServiceFolder -ChildPath "bin\HelperWebService.dll"
    $seleniumFolder = Join-Path -Path $solutionFolder -ChildPath $seleniumFolder    
    $seleniumWebDriverDlls = Join-Path -Path $seleniumFolder -ChildPath "\WebDriver*.dll"

    Deploy-UTF-Absolute -instanceName $instanceName -targetDir $targetDir -helperWebServiceAsmx $helperWebServiceAsmx -helperWebServiceDll $helperWebServiceDll -seleniumWebDriverDlls $seleniumWebDriverDlls
    Deploy-SeleniumDriverExe -solutionFolder $solutionFolder -projectFolder $projectFolder -seleniumFolder $seleniumFolder
}

Function Deploy-UTF-Absolute(	
                            [Parameter(Mandatory=$true )][string]$targetDir,
							[Parameter(Mandatory=$true )][string]$instanceName,
							[Parameter(Mandatory=$true )][string]$helperWebServiceAsmx,
							[Parameter(Mandatory=$true )][string]$helperWebServiceDll,
							[Parameter(Mandatory=$true )][string]$seleniumWebDriverDlls
                            
						)
{
<#
.SYNOPSIS 
Performs Sitecore UTF and Selenium deploy on specified instance with absolute paths.

.DESCRIPTION
Function performs several actions:
- Copy HelperWebService to target instance
- Copy Selenium dlls to target instance

.PARAMETER helperWebServiceAsmx
"<project folder>\HelperWebService\HelperWebService.asmx"

.PARAMETER helperWebServiceDll
"<project folder>\HelperWebService\bin\HelperWebService.dll"

.PARAMETER seleniumWebDriverDlls
"<project folder>\Components\WebDriver*.dll"

.EXAMPLE
to be added
#>
    #$targetDir = Join-Path -Path $instanceRoot -ChildPath $instanceName

	Write-Host "+++Deploying UTF..." -foregroundcolor "green"
	Copy-Item $helperWebServiceAsmx "$targetDir\Website"
	Copy-Item $helperWebServiceDll "$targetDir\Website\bin"
	Copy-Item $seleniumWebDriverDlls "$targetDir\Website\bin"
	Write-Host "UTF has been successfully deployed into $instanceName"
}

<#
.SYNOPSIS 
Performs Selenium driver exe files deploy into specified project.

.DESCRIPTION
Function performs several actions:
- Copy HelperWebService to target instance
- Copy Selenium dlls to target instance

.PARAMETER solutionFolder
Specifies the folder of UTF solution.


.EXAMPLE
to be added
#>
Function Deploy-SeleniumDriverExe(
		[Parameter(Mandatory=$true )][string] $solutionFolder, # test solution root
	    [Parameter(Mandatory=$true )][string] $projectFolder,
	    [Parameter(Mandatory=$true )][string] $seleniumFolder 
)
{
    $seleniumWebDriverDlls = Join-Path -Path $seleniumFolder -ChildPath "\WebDriver*.dll"
    $seleniumWebDriverExes = Join-Path -Path $seleniumFolder -ChildPath "\*.exe"
    $destination = Join-Path -Path $solutionFolder -ChildPath $projectFolder;
    $destination = Join-Path -Path $destination -ChildPath "Bin\Debug";

	Copy-Item $seleniumWebDriverExes $destination -verbose
	Copy-Item $seleniumWebDriverDlls $destination -verbose
	Write-Host "Selenium drivers has been successfully deployed into $projectFolder"

}

