using System.ComponentModel.DataAnnotations;

namespace DemoFBOS.Models
{
	public class Role
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public List<PermissionRole> PermissionRoles { get; set; }
	}
}
