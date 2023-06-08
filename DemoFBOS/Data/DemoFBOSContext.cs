using Microsoft.EntityFrameworkCore;

namespace DemoFBOS.Data
{
	public class DemoFBOSContext : DbContext
	{
		public DemoFBOSContext(DbContextOptions<DemoFBOSContext> options)
			: base(options)
		{
		}

		public DbSet<Models.Permission> Permissions { get; set; }
		public DbSet<Models.Role> Roles { get; set; }
		public DbSet<Models.PermissionRole> PermissionRoles { get; set; }
		public DbSet<Models.User> Users { get; set; }
	}
}
