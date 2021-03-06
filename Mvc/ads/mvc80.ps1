param
(
	[Parameter(Mandatory=$false )]	[string]	$instanceName = 'mvc80',
	[Parameter(Mandatory=$false )]	[string]	$psAdsRoot = "C:\sc\psAds",
	[Parameter(Mandatory=$false )]	[string]	$toolsRoot = "$psAdsRoot\ps.ads\tools",
	[Parameter(Mandatory=$false )]	[string]	$dbServer = [System.Net.Dns]::GetHostName()+"\GERZSON",
	[Parameter(Mandatory=$false )]	[string]	$dbUser = "sa5",
	[Parameter(Mandatory=$false )]	[string]	$dbPass = "12345",
	[Parameter(Mandatory=$false )]	[string]	$targetDir = "c:\sc\inst\$instanceName",
	[Parameter(Mandatory=$false )]	[string]	$testsSolutionFolder = 'C:\Sitecore Autodeployment\AutoTests\Experience Editor Smoke Tests',
	[Parameter(Mandatory=$false )]	[string]	$testsProjectFolder = 'Experience Editor Smoke Tests',
	[Parameter(Mandatory=$false )]	[string]	$msBuildPath = 'C:\Windows\Microsoft.NET\Framework\v4.0.30319',	
	[Parameter(Mandatory=$false )]	[string]    $tortoiseSVNPath = 'C:\Program Files\TortoiseSVN\bin\TortoiseProc.exe',
	[Parameter(Mandatory=$false )]	[string]    $nUnitPath = 'C:\123\Applications\Installs\NUnit\bin\Nunit 2.5.10.11092\net-2.0\nunit-console.exe',
	[Parameter(Mandatory=$false )]	[string]    $nUnitXSLTPath = "$testsSolutionFolder\ads\nUnitXSL\noframe.xsl",
	[Parameter(Mandatory=$false )]	[string]    $nUnitReportXMLPath = 'C:\NUnit Reports\Report.xml',
	[Parameter(Mandatory=$false )]	[string]    $nUnitReportHTMLPath = 'C:\NUnit Reports\Report.html',
	[Parameter(Mandatory=$false )]	[boolean]   $updateTestsAppConfig = $TRUE,
	[Parameter(Mandatory=$false )]	[string]	$seleniumFolder = "$testsProjectFolder\Components\Selenium\2.41x64",
    [Parameter(Mandatory=$false )]	[int]   $svnUpdateDelay = 15
	,[Parameter(Mandatory=$false )]	[switch]	$updateOnly #do not install Sitecore and the EE package
	,[Parameter(Mandatory=$false )]	[switch]	$noUnitTests #do not run unit tests
	,[Parameter(Mandatory=$false )]	[string]	$mongoPath = "c:\Program Files\MongoDB 2.6 Standard\bin\mongo.exe"

)

# Constants
$ErrorActionPreference = "Continue"
$revision = "150121"
$instPath = "\\mars\QA\8.0\Updates\Update-1\Sitecore 8.0 rev. $revision";
$cmsPackage = "$instPath\Sitecore 8.0 rev. $revision.zip"

#$cmsPath = "\\mars\QA\8.0\Updates\Update-1\Sitecore 8.0 rev. 150101"
#$cmsPath = "\\build10ua1\Builds\CMS_80_Update1_Nightly\"
#$cmsPackage1 = Get-LastItem -TargetPath $cmsPath -Extension ".zip" -NamePattern "Sitecore 8.0 rev. "
#$cmsPackage = $cmsPackage1.Replace("_ja-JP", "")

#$ItemBucketsPackage = Get-LastItem -TargetPath "\\build13ua1\Builds\Component_ItemBuckets80_Ondemand_Canaveral_Nightly\" -Extension ".zip" -NamePattern "Sitecore Buckets 1.0.0 rev."
#$SolRSupportPackage = Get-LastItem -TargetPath "\\build13ua1\Builds\Component_ItemBuckets80_Ondemand_Canaveral_Nightly\" -Extension ".zip" -NamePattern "Sitecore.Solr.Support 1.0.0 rev."
#$ItemBucketsPackage = Get-LastItem -TargetPath "\\mars\QA\8.0\Components\Item Buckets\Update-1\" -Extension ".zip" -NamePattern "Sitecore Buckets 1.0.0 rev."
#$SolRSupportPackage = Get-LastItem -TargetPath "\\mars\QA\8.0\Components\Item Buckets\Update-1\" -Extension ".zip" -NamePattern "Sitecore.Solr.Support 1.0.0 rev."

#$LaunchPadPackage = Get-LastItem -TargetPath "\\build7ua1\Builds\launchpad_Nightly\" -Extension ".zip" -NamePattern "Sitecore LaunchPad 1.0 rev."

$license = "\\mars\installs\Licenses\Sitecore Partner License\license.xml"

Write-Host "CMS package: $cmsPackage" -foregroundcolor "green"
Write-Host "Item buckets package: $ItemBucketsPackage" -foregroundcolor "green"
Write-Host "Launchpat package: $LaunchPadPackage" -foregroundcolor "green"

Import-Module DistributedDeploy  -DisableNameChecking
Import-Module FileSystem  -DisableNameChecking

$invocation = (Get-Variable MyInvocation).Value
$currentPath = Split-Path $invocation.MyCommand.Path

Write-Host "+++Import module $currentPath\UTF" -foregroundcolor "green"
Import-Module "$currentPath\UTF" -Force -DisableNameChecking
Write-Host "+++Import module $currentPath\Execute-Tests" -foregroundcolor "green"
Import-Module "$currentPath\Execute-Tests" -Force -DisableNameChecking

Function InstallSitecore(){
    #Clean Instances
    Write-Host "+++Clean Instance $instanceName ..." -foregroundcolor "green"
    Clean-IIS -Instance $instanceName 
    Clean-DB -Instance $instanceName -SqlServer $dbServer -MongoPath $mongoPath
    Remove-Files $targetDir

    #Upload package
    Write-Host "+++Uploading Sitecore package on Server..." -foregroundcolor "green"
    Extract-Files $cmsPackage $targetDir -omitFirstLevelFolder -SuppressOutput -Verbose
    #Write-Host "+++Uploading Analytics package..." -foregroundcolor "green"
    #Extract-Files $analyticsPackage "$targetDir\Databases" -SuppressOutput -Verbose

    # Copy translations
	
    # copy "\\fil1dk1.dk.sitecore.net\data\users\zzt\SC\_SampleData\SC8Translations\*" $targetDir -Force -Verbose

    # Deploy DB
    Write-Host "+++Starting DB deploy" -foregroundcolor "green"
    Deploy-Sitecore-DB -instanceName $instanceName -dbRoot $targetDir -SqlServerName $dbServer -Verbose
    Deploy-Analytics-DB -instanceName $instanceName -dbRoot $targetDir -SqlServerName $dbServer -Verbose

    # Deploy IIS site
    Write-Host "+++Starting IIS deploy" -foregroundcolor "green"
    Deploy-Sitecore -instanceName $instanceName -licenseFile $license -toolsRoot $toolsRoot -sqlServer $dbServer -dbUser $dbUser -dbPass $dbPass -SitecorePath $targetDir
    
    #Deploy-Analytics -instanceName $instanceName -sqlServer $dbServer  -dbUser $dbUser -dbPass $dbPass -SitecorePath $targetDir
    Write-Host "+++Check site availability" -foregroundcolor "green"
    Invoke-WebRequest -Uri "http://$instanceName/" | Select-Object StatusDescription
    Write-Host "+++Done!" -foregroundcolor "green"
}

Function InstallPackages(){

    Write-Host "+++Installing package: ItemBuckets"
    Install-Package -instanceName $instanceName -packagePath $ItemBucketsPackage -pkgFolderPath $targetDir + "\Data\packages" -runPostStep 

    Write-Host "+++Installing package: SolR support"
    Install-Package -instanceName $instanceName -packagePath $SolRSupportPackage -pkgFolderPath $targetDir + "\Data\packages" -runPostStep 

    #Write-Host "+++Installing package: Launch pad "
    #Install-Package -instanceName $instanceName -packagePath $LaunchPadPackage -pkgFolderPath $targetDir + "\Data\packages" -runPostStep 

    Write-Host "+++Check site availability" -foregroundcolor "green"
    Invoke-WebRequest -Uri "http://$instanceName/" | Select-Object StatusDescription
}

Function InstallQA(){
    Write-Host "+++Installing package: All fields"
    Install-Package -instanceName $instanceName -packagePath "\\fil1dk1.dk.sitecore.net\data\users\zzt\SC\_SampleData\QApackages\All fields Template.zip" -pkgFolderPath $targetDir + "\Data\packages" -runPostStep 
}


if(!$updateOnly){
    InstallSitecore
    #InstallPackages
    InstallQA
}

Write-Host "$cmsPackage installed" -foregroundcolor "green"



