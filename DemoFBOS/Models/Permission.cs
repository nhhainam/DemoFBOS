﻿using System.ComponentModel.DataAnnotations;

namespace DemoFBOS.Models
{
	public class Permission
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
