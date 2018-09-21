#Requires -Version 3.0

Param(
 [Parameter(Mandatory=$True)]
 [string]
 $DockerRegistryUrl
)

# stop the script on first error
$ErrorActionPreference = 'Stop'

$NotificationProducerImageName = "$DockerRegistryUrl/notification-producer"

docker build -f ./NotificationProducer/Dockerfile -t $NotificationProducerImageName ./NotificationProducer
docker push $DockerRegistryUrl/notification-producer