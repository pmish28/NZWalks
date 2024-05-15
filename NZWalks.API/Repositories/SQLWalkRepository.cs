using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
       
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk ;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk != null) 
            {
                dbContext.Walks.Remove(existingWalk);
                await dbContext.SaveChangesAsync();
            }
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, 
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pagesize = 1000)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
            //Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && (string.IsNullOrWhiteSpace(filterQuery) == false))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //Sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase)) 
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) :walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthinKm) : walks.OrderByDescending(x => x.LengthinKm);
                }
            }

            //Pagination
            var skipResults = (pageNumber-1)*pagesize;


            return await walks.Skip(skipResults).Take(pagesize).ToListAsync();
            //var walksList = await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
            //return walksList;
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var walk = await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x=>sbyte.Equals(x.Id, id));
            return walk;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = dbContext.Walks.FirstOrDefault(x => x.Id == id);
            if (existingWalk != null) 
            {
                existingWalk.Name = walk.Name;
                existingWalk.Description = walk.Description;
                existingWalk.RegionId = walk.RegionId;
                existingWalk.DifficultyId = walk.DifficultyId;
                existingWalk.LengthinKm = walk.LengthinKm;
                existingWalk.WalkImageUrl = walk.WalkImageUrl;
            
            }
            await dbContext.SaveChangesAsync();
            return existingWalk;
                
        }
    }
}
