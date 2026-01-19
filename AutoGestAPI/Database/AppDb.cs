using Microsoft.EntityFrameworkCore;

namespace AutoGestAPI.Database
{
    public class AppDb : DbContext
    {
        public AppDb(DbContextOptions<AppDb> options) : base(options) { }
    }
}
