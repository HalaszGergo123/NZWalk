using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.API.Data;
using NZWalk.API.Models.Domain;
using NZWalk.API.Models.DTO;

namespace NZWalk.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegionsController : ControllerBase
	{
		private readonly NZWalksDbContext dbContext;
		private readonly IMapper mapper;

		public RegionsController(NZWalksDbContext dbContext, IMapper mapper)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var regionsDomain = dbContext.Regions.ToList();

			var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

			return Ok(regionsDto);
		}

		[HttpGet]
		[Route("{id:Guid}")]
		public IActionResult GetById([FromRoute] Guid id)
		{
			var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

			var regionDto = mapper.Map<RegionDto>(regionDomain);

			if (regionDomain == null)
			{
				return NotFound();
			}

			return Ok(regionDto);
		}

		[HttpPost]
		public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
		{
			var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

			dbContext.Regions.Add(regionDomainModel);
			dbContext.SaveChanges();

			var regionDto = mapper.Map<RegionDto>(regionDomainModel);

			return CreatedAtAction(nameof(GetById), new {id = regionDomainModel.Id}, regionDto);
		}
	}
}
