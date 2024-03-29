﻿using System.ComponentModel.DataAnnotations;

namespace DemoFBOS.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public int RoleId { get; set; }
		public Role Role { get; set; }
	}
}
