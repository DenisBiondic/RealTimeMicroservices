#Requires -Version 3.0

Param(
 [Parameter(Mandatory=$True)]
 [string]
 $DockerRegistryUrl
)

# stop the script on first error
$ErrorActionPreference = 'Stop'

Write-Host "-------------------------------------------------------"
Write-Host "Building and pushing the notification producer image..."
$NotificationProducerImageName = "$DockerRegistryUrl/notification-producer"

docker build -f ./NotificationProducer/Dockerfile -t $NotificationProducerImageName ./NotificationProducer
docker push $NotificationProducerImageName
    
Write-Host "------------------------------------------------------"
Write-Host "Building and pushing the backend for frontend image..."
$BffImageName = "$DockerRegistryUrl/backend-for-frontend"

docker build -f ./BackendForFrontend/Dockerfile -t $BffImageName ./BackendForFrontend
docker push $BffImageName

Write-Host "------------------------------------------------------"
Write-Host "Building and pushing the frontend image..."
$FrontendImageName = "$DockerRegistryUrl/frontend"

docker build -f ./Frontend/Dockerfile -t $FrontendImageName ./Frontend
docker push $FrontendImageName