
mklink /D C:\inetpub\wwwroot\TripXML C:\temp


if (Get-Item -Path C:\inetpub\wwwroot\TripXML -ErrorAction Ignore)
{
	(Get-Item C:\inetpub\wwwroot\TripXML).Delete()
}

$path = $OctopusParameters["Octopus.Action.Package.InstallationDirectoryPath"]
Write-Host "TripXML Path is: $path"
New-Item -ItemType SymbolicLink -Path C:\inetpub\wwwroot\TripXML -Target $path