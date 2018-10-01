#Requires -Version 3.0

Param(
 [Parameter(Mandatory=$True)]
 [string]
 $DockerRegistryUrl
)

# stop the script on first error
$ErrorActionPreference = 'Stop'

Remove-Item ./.generated -Recurse -ErrorAction Ignore
New-Item -ItemType Directory -Force -Path ./.generated

# namespace (will fail if exists, therefore should be manually delete if re-deploying)
kubectl create ns realtime-microservices 

# redis
Copy-Item -Path ./Redis/deployment.yaml -Destination ./.generated/redis-deployment.yaml

# notification producer
$deploymentFilePath = "./.generated/notification-producer-deployment.yaml"
Copy-Item -Path ./NotificationProducer/deployment.yaml -Destination $deploymentFilePath
(Get-Content $deploymentFilePath).replace('${DockerRegistryUrl}', $DockerRegistryUrl) | Set-Content $deploymentFilePath

# backend for frontend
$deploymentFilePath = "./.generated/bff-deployment.yaml"
Copy-Item -Path ./BackendForFrontend/deployment.yaml -Destination $deploymentFilePath
(Get-Content $deploymentFilePath).replace('${DockerRegistryUrl}', $DockerRegistryUrl) | Set-Content $deploymentFilePath

# frontend
$deploymentFilePath = "./.generated/frontend-deployment.yaml"
Copy-Item -Path ./Frontend/deployment.yaml -Destination $deploymentFilePath
(Get-Content $deploymentFilePath).replace('${DockerRegistryUrl}', $DockerRegistryUrl) | Set-Content $deploymentFilePath

# deploy everything
kubectl apply -f .generated