using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilter;
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
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            
            //Map dto to domain model

            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            //Save to database
            await walkRepository.CreateAsync(walkDomainModel);

            //Map doamin model to DTO

            return Ok(mapper.Map<WalkDto>(walkDomainModel));
            

        }
        //Get the walks
        //GET: /api/Walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1*pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber =1,[FromQuery] int pageSize =1000)
        {
            //Get all from database
            var walkDomainModel = await walkRepository.GetAllAsync(filterOn,filterQuery,
                sortBy,isAscending ?? true,pageNumber,pageSize);

            //Map domain model to dto model
            
            return Ok(mapper.Map<List<WalkDto>>(walkDomainModel));
        }

        //Get walk by id
        //GET : /api/walks/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Get walk by id from repository 
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null) 
            {
                return NotFound();
            }

            //Map domain model to dto
            return Ok(mapper.Map<WalkDto>(walkDomainModel));

        }

        //Update walk
        //PUT: api/Walks/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            
                //map dto to domain model
                var walkDomain = mapper.Map<Walk>(updateWalkRequestDto);

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
        //Delete : api/Walks/{id}

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
