$ErrorActionPreference = 'Stop'
$packageName = $env:ChocolateyPackageName
$installDirPath = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

# Install package
$packageArgs = @{
    packageName   = $packageName
    unzipLocation = $installDirPath
    url           = 'https://github.com/Tyrrrz/OsuHelper/releases/download/2.0.3/OsuHelper.zip'
}
Install-ChocolateyZipPackage @packageArgs

# Mark the executable as GUI
New-Item (Join-Path $installDirPath "osu!helper.exe.gui") -ItemType File -Force