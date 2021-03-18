using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace frontend.Shared
{
    public class PersonResource
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public IEnumerable<TodoItemResource> TodoItems { get; set; }
    }
}
