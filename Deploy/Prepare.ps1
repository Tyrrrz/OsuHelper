New-Item "$PSScriptRoot\bin" -ItemType Directory -Force
$files = @()
$files += Get-ChildItem -Path "$PSScriptRoot\..\OsuHelper\bin\Release\*" -Include "*.exe", "*.dll", "*.config"
$files | Compress-Archive -DestinationPath "$PSScriptRoot\bin\osu.helper.zip" -Force