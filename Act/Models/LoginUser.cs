using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace Act.Models {
public class LoginUser
{
    
    [Required]
    public string Email {get; set;}
    [Required]
    public string Password { get; set; }
}

}