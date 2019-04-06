# --- PORTABLE ---

# Get files
$files = @()
$files += Get-Item -Path "$PSScriptRoot\..\License.txt"
$files += Get-ChildItem -Path "$PSScriptRoot\..\OsuHelper\bin\Release\*" -Include "*.exe", "*.dll", "*.config"

# Pack into archive
New-Item "$PSScriptRoot\Portable\bin" -ItemType Directory -Force
$files | Compress-Archive -DestinationPath "$PSScriptRoot\Portable\bin\OsuHelper.zip" -Force

# --- CHOCOLATEY ---

# Create package
New-Item "$PSScriptRoot\Choco\bin\" -ItemType Directory -Force
choco pack $PSScriptRoot\Choco\osuhelper.nuspec --out $PSScriptRoot\Choco\bin\