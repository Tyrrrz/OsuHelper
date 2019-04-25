# Get files
$files = @()
$files += Get-Item -Path "$PSScriptRoot\..\License.txt"
$files += Get-ChildItem -Path "$PSScriptRoot\..\OsuHelper\bin\Release\*" -Include "*.exe", "*.dll", "*.config"

# Pack into archive
New-Item "$PSScriptRoot\Portable\bin" -ItemType Directory -Force
$files | Compress-Archive -DestinationPath "$PSScriptRoot\Portable\bin\OsuHelper.zip" -Force