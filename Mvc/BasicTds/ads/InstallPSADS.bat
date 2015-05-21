@echo off
echo Consider downloading Nuget.exe from http://nuget.codeplex.com
echo you might specify -nugetPath parameter (powershell "&{Install-Ads.ps1 -nugetPath 'xxxxx'}")
echo -psAdsRoot folder will be erased
pause
powershell "&{InstallPsAds.ps1}"
pause