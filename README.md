# A example implementation of Microservices using .NET and Kubernetes

## Commands

### Docker
- `docker build -t elithrade/platformservice .` will build the local docker image using `latest` tag.
- `docker push elithrade/platformservice` will push the image to Docker Hub.

### Kubernetes
- `kubectl get deployments` will get the current deployments.
- `kubectl get pods` will get the current pods.
- `kubectl get services` will get the current services.
- `kubectl apply -f ./k8s/{.yml}` will apply settings in `.yml` file and create resources accordingly.
- `kubectl rollout restart deployment platforms-depl` will refetch the latest image in Docker Hub and restart the service.

## Notes

### Environments
Environments are configured in `appsettings|{.Env}.json` file. For more details refer to https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-6.0 for more details.

### What's the difference between `ClusterIP`, `NodePort` and `LoadBalancer` service types in Kubernetes?
- `ClusterIP`: Expose service through k8s cluster with `ip/name:port`.
- `NodePort`: Expose service through internal network also to k8s `ip/name:port`.
- `LoadBalancer`: Expose service through external world or whatever you defined in your load balancer.