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

        public async Task<List<Walk>> GetAllAsync()
        {
            var walksList = await dbContext.Walks.ToListAsync();
            return walksList;
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var walk = await dbContext.Walks.FirstOrDefaultAsync(x=>sbyte.Equals(x.Id, id));
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
