<# First, delete any existing package or nuspec file #>
del *.nuspec
del *.nupkg

<# Next up - create the default spec file from metadata #>
nuget spec

<# Transform the spec file to replace default values with suitable alternatives #>
(Get-Content miiCard.Providers.ASPNet.nuspec) |
Foreach-Object {
	$_ -replace "<licenseUrl>.*<\/licenseUrl>", "<licenseUrl>http://www.miicard.com/developers</licenseUrl>" `
	   -replace "<projectUrl>.*<\/projectUrl>", "<projectUrl>http://www.miicard.com/developers/libraries-components/aspnet-provider</projectUrl>" `
	   -replace "<iconUrl>.*<\/iconUrl>", "<iconUrl>http://www.miicard.com/sites/default/files/dev/nuget_32x32.png</iconUrl>" `
	   -replace "<releaseNotes>.*<\/releaseNotes>", "<releaseNotes></releaseNotes>" `
	   -replace "<tags>.*<\/tags>", "<tags>miiCard identity assurance security miicard.com ASP.NET provider</tags>"
} | Set-Content miiCard.Providers.ASPNet.nuspec 

<# Finally, package the thing #>
nuget pack miiCard.Providers.ASPNet.csproj -Prop Configuration="Debug"