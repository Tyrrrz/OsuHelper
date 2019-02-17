New-Item "$PSScriptRoot\bin" -ItemType Directory -Force
$files = @()
$files += Get-Item -Path "$PSScriptRoot\..\License.txt"
$files += Get-ChildItem -Path "$PSScriptRoot\..\OsuHelper\bin\Release\*" -Include "*.exe", "*.dll", "*.config"
$files | Compress-Archive -DestinationPath "$PSScriptRoot\bin\osu.helper.zip" -Force