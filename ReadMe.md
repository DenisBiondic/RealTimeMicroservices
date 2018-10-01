## Getting started

### Locally with docker & compose

Simply run docker-compose up from the source code root.

### With a Kubernetes cluster

**prerequisite:** 
k8s cluster with 
- an access to a registry
- ingress controller (or ignore the ingress and modify the services to expose them directly over LB or node port). Beware: Ingress is written with path based routing only, meaning the complete host is presumed to be free (/ is the app, /app & /hubs is pointed to bff).

Run `BuildAndPush-Containers.ps1` and `Deploy-Application.ps1` (you need the URL of your registry as a mandatory parameter for the scripts)
To re-deploy, delete the k8s realtime-microservices namespace


## Side notes

### Websockets

For websockets to work through ingress, sticky session has to be used, which is implemented using affinity cookies at the moment (check the ingress definition of the BFF) 

### Redis
Check redis: `kubectl run redis-cli-image -i --tty --rm --image redis -n realtime-microservices -- /bin/sh`
You can then run something like `redis-cli -h redis` and `SUBSCRIBE Notification-Channel`