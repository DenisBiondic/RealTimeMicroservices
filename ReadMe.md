## Getting started

- prerequisite: k8s cluster with access to a registry

Run `BuildAndPush-Containers.ps1` and `Deploy-Application.ps1` (you need the URL of your registry)
To re-deploy, delete the k8s realtime-microservices namespace


## Side notes

Check redis: `kubectl run redis-cli-image -i --tty --rm --image redis -- redis-cli -h redis`