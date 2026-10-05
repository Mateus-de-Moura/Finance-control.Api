using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundService.Entity
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [BindProperty]
        public bool Active { get; set; }

        public string? Name { get; set; }
        public string? Type { get; set; }

        public List<Expenses> Expenses { get; set; } 
    }
}
