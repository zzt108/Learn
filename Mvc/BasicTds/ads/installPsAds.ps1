iex ((new-object net.webclient).DownloadString('https://chocolatey.org/install.ps1'))
cinst ps.ads  -Source http://nuget1dk1:8181/nuget/Packages

#choco install ps.ads -Version 1.5.729 -Source http://nuget1dk1:8181/nuget/Packages