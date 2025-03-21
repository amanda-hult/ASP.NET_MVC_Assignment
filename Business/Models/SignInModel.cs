﻿using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class SignInModel
{
    [Required]
    [Display(Name = "Email", Prompt = "Your email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;


    [Required]
    [Display(Name = "Password", Prompt = "Enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
