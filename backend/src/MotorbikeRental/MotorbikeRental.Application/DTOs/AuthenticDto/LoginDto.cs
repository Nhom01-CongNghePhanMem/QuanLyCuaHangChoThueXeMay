﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.AuthenticDto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "UserName is required")]
        [StringLength(50, ErrorMessage = "UserName cannot exceed 50 characters")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be 6–50 characters")]
        public string Password { get; set; }
    }
}
