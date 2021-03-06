param
(
	[Parameter(Mandatory=$false )]	[string]	$instanceName = 'lsc80',
	[Parameter(Mandatory=$false )]	[string]	$psAdsRoot = "c:\SitecorePowerShell\",
	[Parameter(Mandatory=$false )]	[string]	$toolsRoot = "$psAdsRoot\ps.ads\tools",
	[Parameter(Mandatory=$false )]	[string]	$dbServer = [System.Net.Dns]::GetHostName(),
	[Parameter(Mandatory=$false )]	[string]	$dbUser = "sa",
	[Parameter(Mandatory=$false )]	[string]	$dbPass = "12345",
	[Parameter(Mandatory=$false )]	[string]	$targetDir = "c:\sc\inst\$instanceName",
	[Parameter(Mandatory=$false )]	[string]	$testsSolutionFolder = 'c:\SVN\MVC\MVC BDD Tests',
	[Parameter(Mandatory=$false )]	[string]	$testsProjectFolder = 'Tests',
	[Parameter(Mandatory=$false )]	[boolean]   $updateTestsAppConfig = $TRUE,
	[Parameter(Mandatory=$false )]	[string]	$seleniumFolder = "$testsProjectFolder\Components\Selenium\2.44",
    [Parameter(Mandatory=$false )]	[int]   $svnUpdateDelay = 15
	,[Parameter(Mandatory=$false )]	[switch]	$updateOnly #do not install Sitecore and the EE package
	,[Parameter(Mandatory=$false )]	[switch]	$noUnitTests #do not run unit tests
	,[Parameter(Mandatory=$false )]	[string]	$mongoPath = "c:\Program Files\MongoDB\Server\3.0\bin\mongo.exe"
	,[Parameter(Mandatory=$false )]	[string]	$launchSitecoreZipPath = "c:\SVN\MVC\MVC BDD Tests\Tests\Components\TestSites\LaunchSitecoreMVC8011.zip"
	,[Parameter(Mandatory=$false )]	[string]	$sitecoreMvcBuildPath = "\\build13ua1\Builds\GIT_AutoNightly_Sitecore_MVC_Build\"
	,[Parameter(Mandatory=$false )]	[string]	$mvcQaZipPath = "c:\SVN\MVC\MVC BDD Tests\Tests\Components\TestSites\MvcQa.zip"
	,[Parameter(Mandatory=$false )]	[string]	$adsVersionSpecificFolder = $instanceName
	,[Parameter(Mandatory=$false )]	[string]	$scVersion = "8.0"

)

cd "c:\SVN\MVC\MVC BDD Tests\ads\"

# Constants
$ErrorActionPreference = "Continue"
$cmsPackage = Get-Archive -name "Sitecore" -ver $scVersion
#$mvcPackage = Get-Archive -ArchiveName "Sitecore Mvc" -FilePath $sitecoreMvcBuildPath
$lastBuildFolder = Get-LastFolder -TargetPath $sitecoreMvcBuildPath
$lastBuildPackage = Get-LastItem -TargetPath $lastBuildFolder -ArchiveName "Sitecore Mvc" 

$license = "\\mars\installs\Licenses\Sitecore Partner License\license.xml"

Write-Host "CMS package: $cmsPackage" -foregroundcolor "green"
Write-Host "Sitecore MVC folder: $lastBuildFolder" -foregroundcolor "green"
Write-Host "Sitecore MVC package: $lastBuildPackage" -foregroundcolor "green"

Import-Module DistributedDeploy  -DisableNameChecking
Import-Module FileSystem  -DisableNameChecking

$invocation = (Get-Variable MyInvocation).Value
$currentPath = Split-Path $invocation.MyCommand.Path

Write-Host "+++Import module $currentPath\UTF" -foregroundcolor "green"
Import-Module "$currentPath\UTF" -Force -DisableNameChecking
Write-Host "+++Import module $currentPath\Execute-Tests" -foregroundcolor "green"
Import-Module "$currentPath\Execute-Tests" -Force -DisableNameChecking

Function InstallSitecore(){

    $ErrorActionPreference = "Continue"
    Write-Log "Installing Sitecore, site name $instanceName"
    Write-Log "Sitecore: $cmsPackage"

    Deploy-All -InstanceName $instanceName -cmsPackage $cmsPackage -SqlServerName $dbServer -dbUser $dbUser -dbPass $dbPass -MongoPath $mongoPath -SitecorePath $targetDir -SuppressOutput

    Write-Host "+++Check site availability" -foregroundcolor "green"
    Invoke-WebRequest -Uri "http://$instanceName/" | Select-Object StatusDescription

    Write-Host "+++Done!" -foregroundcolor "green"
}

Function InstallQA(){
    Write-Host "+++Installing package: All fields"

    Install-Package -instanceName $instanceName -packagePath $lastBuildPackage -pkgFolderPath ($targetDir + "\Data\packages") #-runPostStep 
    #copy "c:\SVN\MVC\MVC BDD Tests\ads\lsc81\web.config" "$targetDir\WebSite" -force -Verbose
    copy "$adsVersionSpecificFolder\web.config" "$targetDir\WebSite" -force -Verbose

    #Install-Package -instanceName $instanceName -packagePath "\\fil1dk1.dk.sitecore.net\data\users\zzt\SC\_SampleData\QApackages\All fields Template.zip" -pkgFolderPath ($targetDir + "\Data\packages") -runPostStep 

    #MVC test site
	Install-Package -instanceName $instanceName -packagePath $launchSitecoreZipPath -pkgFolderPath ($targetDir + "\Data\packages") 
	Install-Package -instanceName $instanceName -packagePath $mvcQaZipPath -pkgFolderPath ($targetDir + "\Data\packages") 
}

Function DeployUTF()
{
    Write-Host "Deploy UTF"
    Deploy-UTF -instanceName $instanceName -targetDir $targetDir -solutionFolder $testsSolutionFolder -helperWebServiceProject "HelperWebService" -projectFolder $testsProjectFolder -seleniumFolder $seleniumFolder 

    Write-Host "UTF $instanceName Installed"
}

#$updateOnly=$TRUE;


if(!$updateOnly){
    InstallSitecore
    InstallQA
}


DeployUTF
Publish-Smart -instanceName $instanceName
Rebuild-Indexes -instanceName $instanceName

Write-Host "$cmsPackage installed" -foregroundcolor "green"



