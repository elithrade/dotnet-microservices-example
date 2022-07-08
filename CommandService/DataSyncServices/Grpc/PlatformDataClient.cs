using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.DataSyncServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private IConfiguration _config;
        private IMapper _mapper;

        public PlatformDataClient(IConfiguration config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
        }

        public IEnumerable<Platform>? ReturnAllPlatforms()
        {
            Console.WriteLine($"Calling Grpc service {_config["GrpcPlatform"]}");

            // Bypass PartialChain cert error.
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var channel = GrpcChannel.ForAddress(_config["GrpcPlatform"], new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while calling Grpc server {ex.Message}");
                return null;
            }
        }
    }
}