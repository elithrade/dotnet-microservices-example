using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/commands/platforms/{platformId}/[controller]")]
    public class CommandsController : ControllerBase
    {
        private ICommandRepo _repo;

        private IMapper _mapper;

        public CommandsController(ICommandRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAll(int platformId)
        {
            if (!_repo.PlatformExists(platformId)) return NotFound($"Platform with id {platformId} does not exist");

            var commands = _repo.GetCommands(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}")]
        public ActionResult<CommandReadDto> Get(int platformId, int commandId)
        {
            if (!_repo.PlatformExists(platformId)) return NotFound($"Platform with id {platformId} does not exist");

            var command = _repo.GetCommand(platformId, commandId);
            if (command == null) return NotFound($"Command with id {commandId} does not exist for platform {platformId}");

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> Create(int platformId, CommandCreateDto commandCreateDto)
        {
            if (!_repo.PlatformExists(platformId)) return NotFound($"Platform with id {platformId} does not exist");

            var command = _mapper.Map<Command>(commandCreateDto);
            _repo.Create(platformId, command);
            _repo.Commit();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(Get), new
            {
                platformId = platformId,
                commandId = commandReadDto.Id
            }, commandReadDto);
        }
    }
}