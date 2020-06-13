using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerApi.Models
{
    public class CustomerModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}