$ErrorActionPreference = 'Stop';

# Install package
$packageArgs = @{
    packageName   = $env:ChocolateyPackageName
    unzipLocation = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
    url           = 'https://github.com/Tyrrrz/OsuHelper/releases/download/2.0.2/osu.helper.zip'
}
Install-ChocolateyZipPackage @packageArgs

# Mark the executable as GUI
New-Item (Join-Path unzipLocation "osu!helper.exe.gui") -ItemType File -Force