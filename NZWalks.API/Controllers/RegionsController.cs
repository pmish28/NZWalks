using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    //https:localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        //GET ALL REGIONS
        // https:localhost:portnumber/api/regions/
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from database  - domain models
            var regions = await regionRepository.GetAllAsync();

            //map domain models to DTO
                        
            //var regionsDto = new List<RegionDTO>();
            //foreach (var region in regions)
            //{
            //    regionsDto.Add(new RegionDTO()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        RegionImageUrl = region.RegionImageUrl
            //    });
            //}
                
            //Return DTO back to client

            return Ok(mapper.Map<List<RegionDTO>>(regions));
        }

        // Get region by ID
        // https:localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            //Get region domain model from 
            var regionDomain = await regionRepository.GetByIdAsync(id);
            
            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map region domain model to region DTO

            //Return DTO back to client
            return Ok(mapper.Map<RegionDTO>(regionDomain));
            
        }

        //POST to create new region 
        //https:localhost:portnumber/api/regions/
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO) 
        {
            //Map or convert DTO to Domain model
            var regionDomain = mapper.Map<Region>(addRegionRequestDTO);
            //Use domain model to create region
            regionDomain =  await regionRepository.CreateAsync(regionDomain);

            //Map domain model back to DTO
            var regionDTO = mapper.Map<RegionDTO>(regionDomain);

            return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);

    }

        //Update region
        //PIT: https:localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO) 
        {
            //Map DTO to domain model

            var regionDomain = mapper.Map<Region>(updateRegionRequestDTO);
            //Find region in db and see if it exists
            regionDomain = await regionRepository.UpdateAsync(id, regionDomain);
            if (regionDomain == null)
            {
                return NotFound();
            }

            //Convert domain model to DTO
            var regionDTO = mapper.Map<RegionDTO> (regionDomain);
            return Ok(regionDTO);
            
        }

        //Delete region
        //https:localhost:portnumber/api/regions/{id}

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            //Find region if exists
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            //delete region
            //Map domain model to dto
            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);
            return Ok(regionDTO);

        }

    }
}
