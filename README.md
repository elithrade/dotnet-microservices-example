# A example implementation of Microservices using .NET and Kubernetes

## Notes

### Environments
Environments are configured in `appsettings|{.Env}.json` file. For more details refer to https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-6.0 for more details.

### What's the difference between `ClusterIP`, `NodePort` and `LoadBalancer` service types in Kubernetes?
- `ClusterIP`: Expose service through k8s cluster with `ip/name:port`.
- `NodePort`: Expose service through internal network also to k8s `ip/name:port`.
- `LoadBalancer`: Expose service through external world or whatever you defined in your load balancer.