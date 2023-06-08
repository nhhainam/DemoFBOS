using System.ComponentModel.DataAnnotations.Schema;

namespace DemoFBOS.Models
{
	public class PermissionRole
	{
		[ForeignKey("Permission")]
		public int PermissionId { get; set; }
		public Permission Permission { get; set; }

		[ForeignKey("Role")]
		public int RoleId { get; set; }
		public Role Role { get; set; }
	}
}
