$ErrorActionPreference = 'Stop';
$packageArgs = @{
  packageName = $env:ChocolateyPackageName
  unzipLocation = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
  url = 'https://github.com/Tyrrrz/OsuHelper/releases/download/2.0.2/osu.helper.zip'
}
Install-ChocolateyZipPackage @packageArgs