using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public PlatformsController(IPlatformRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
        public ActionResult<PlatformReadDto> Create(PlatformCreateDto dto)
        {
            var model = _mapper.Map<Platform>(dto);

            _repository.Create(model);
            _repository.Commit();

            var readDto = _mapper.Map<PlatformReadDto>(model);
            // Tells client the created resource location.
            return CreatedAtRoute(nameof(Get), new { Id = readDto.Id }, readDto);
        }
    }
}