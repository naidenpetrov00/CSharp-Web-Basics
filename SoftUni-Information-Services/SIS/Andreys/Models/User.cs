﻿namespace Andreys.Models
{
	using System.ComponentModel.DataAnnotations;

	public class User
	{
		public User()
		{
			this.Products = new HashSet<Product>();
			this.Id = Guid.NewGuid().ToString();
		}

		public string Id { get; set; }

		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public string Email { get; set; }

		public ICollection<Product> Products { get; set; }
	}
}
