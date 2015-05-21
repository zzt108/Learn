<# This file contains the functions for tests execution #>
$ErrorActionPreference = "stop"

Function SVNUpdate(
[Parameter(Mandatory=$true )]	[string]    $tortoiseSVNPath,
[Parameter(Mandatory=$true )]	[string]	$testsSolutionFolder
)
{
	Write-Host "Updating project folder from SVN" -foregroundcolor "green"
	& $tortoiseSVNPath /command:update /path:$testsSolutionFolder /closeonend:2
}

Function BuildTestSolution(
[Parameter(Mandatory=$true )]	[string]	$msBuildPath,
[Parameter(Mandatory=$true )]	[string]	$testsSolutionFolder 
)
{
	Write-Host "Building Helper Service project" -foregroundcolor "green"
	& "$msBuildPath\msbuild.exe" "$testsSolutionFolder\HelperWebService\HelperWebService.csproj"

	Write-Host "Building UTF project" -foregroundcolor "green"
	& "$msBuildPath\msbuild.exe" "$testsSolutionFolder\UTF\UTF.csproj"

	Write-Host "Building UTF.EE project" -foregroundcolor "green"
	& "$msBuildPath\msbuild.exe" "$testsSolutionFolder\UTF.EE\UTF.ExperienceEditor.csproj"

	Write-Host "Building EE Smoke Tests project" -foregroundcolor "green"
	& "$msBuildPath\msbuild.exe" "$testsSolutionFolder\Experience Editor Smoke Tests\Experience Editor Smoke Tests.csproj"
}

Function ExecuteTests(
[Parameter(Mandatory=$true )]	[string]    $nUnitPath,
[Parameter(Mandatory=$true )]	[string]	$testsSolutionFolder,
[Parameter(Mandatory=$true )]	[string]    $nUnitXSLTPath,
[Parameter(Mandatory=$true )]	[string]    $nUnitReportXMLPath,
[Parameter(Mandatory=$true )]	[string]    $nUnitReportHTMLPath,
[Parameter(Mandatory=$false )]	[int]    $timeout=300000
)
{
	 & $nUnitPath  "$testsSolutionFolder\Experience Editor Smoke Tests\bin\Debug\Experience Editor Smoke Tests.dll" /xml $nUnitReportXMLPath /timeout $timeout
	 $settings = new-object system.xml.xsl.xsltsettings($TRUE, $TRUE)
	 $resolver = new-object system.xml.xmlurlresolver
	 $xslt = new-object system.xml.xsl.xslcompiledtransform
	 $xslt.load($nUnitXSLTPath, $settings, $resolver)
	 $xslt.Transform($nUnitReportXMLPath, $nUnitReportHTMLPath)
}

Function UpdateAppConfig(

[Parameter(Mandatory=$true )]	[string]	$testsSolutionFolder,
[Parameter(Mandatory=$true )]	[string]    $instanceName
)
{
Write-Host "Updating app.config file" -foregroundcolor "green"
	 $doc = new-object system.xml.xmldocument
	 $doc.Load("$testsSolutionFolder\Experience Editor Smoke Tests\App.config")
	 $node = $doc.SelectSingleNode("//add[@key='Instance']")
	 $node.value = $instanceName
	 $doc.Save("$testsSolutionFolder\Experience Editor Smoke Tests\App.config")
}
