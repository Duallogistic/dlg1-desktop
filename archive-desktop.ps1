
$scriptDirectory = $PSScriptRoot

$deliverPath = "C:\Users\achebv\Dropbox\planner-app"

$sourceFolder = Join-Path $scriptDirectory "AmazonDeliveryPlanner\bin\Debug"
$archiveFolder = Join-Path $deliverPath "planner.zip"

$configFileSource = Join-Path $scriptDirectory "AmazonDeliveryPlanner\\ConfigurationTemplate\conf.prod.json"
$configFileDestination = Join-Path $sourceFolder "conf.json"

$installFileSrc = Join-Path $scriptDirectory "install-desktop.ps1"
$installFileDest = Join-Path $deliverPath "install-desktop.ps1"

# Check if the source folder exists
if (Test-Path $sourceFolder -PathType Container) {
    # Create the archive folder if it doesn't exist
    $null = New-Item -Path (Split-Path $archiveFolder) -ItemType Directory -Force
	
    # Copy the config-prod.json file to the project folder
    Copy-Item -Path $configFileSource -Destination $configFileDestination -Force

    # Copy install file
    Copy-Item -Path $installFileSrc -Destination $installFileDest -Force

    # Archive the source folder
    Compress-Archive -Path $sourceFolder -DestinationPath $archiveFolder -Force

    Write-Host "Folder '$sourceFolder' has been archived to '$archiveFolder'."
} else {
    Write-Host "Source folder '$sourceFolder' does not exist."
}

