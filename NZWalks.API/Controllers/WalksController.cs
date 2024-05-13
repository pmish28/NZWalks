using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Mappings;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper,IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }


        //Create walk
        //POST : /api/walks

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map dto to domain model

            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            //Save to database
            await walkRepository.CreateAsync(walkDomainModel);

            //Map doamin model to DTO
            
            return Ok(mapper.Map<WalkDto>(walkDomainModel));

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get all from database
            var walkDomainModel = await walkRepository.GetAllAsync();

            //Map data model to domain model
            
            return Ok(mapper.Map<List<WalkDto>>(walkDomainModel));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Get walk by id from repository 
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            //Map domain model to dto
            return Ok(mapper.Map<WalkDto>(walkDomainModel));

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        { 
            //map dto to domain model
            var walkDomain =  mapper.Map<Walk>(updateWalkRequestDto);

            // Update database using repository
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            if (walkDomain == null)
            {
                return NotFound();
            }
            //Map walk domain to dto
            return Ok(mapper.Map<WalkDto>(walkDomain));
        
        }
        //Delete Walk 
        //Delete : api/Walk/{id}

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.DeleteAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            var walkDto = mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }
    }
}
