using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.DataSyncServices;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.Repo;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private IMapper _mapper;
        private readonly IDataSyncService _dataSyncService;

        public PlatformsController(
            IPlatformRepo repository,
            IMapper mapper,
            IDataSyncService dataSyncService)
        {
            _repository = repository;
            _mapper = mapper;
            _dataSyncService = dataSyncService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAll()
        {
            var platforms = _repository.GetAll();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id}", Name = "Get")]
        public ActionResult<IEnumerable<PlatformReadDto>> Get(int id)
        {
            var platform = _repository.Get(id);
            if (platform != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platform));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> Create(PlatformCreateDto dto)
        {
            var model = _mapper.Map<Platform>(dto);

            _repository.Create(model);
            _repository.Commit();

            var readDto = _mapper.Map<PlatformReadDto>(model);

            try
            {
                await _dataSyncService.SendPlatform(readDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while calling data sync service: {ex.InnerException}");
            }

            // Tells client the created resource location.
            return CreatedAtRoute(nameof(Get), new { Id = readDto.Id }, readDto);
        }
    }
}