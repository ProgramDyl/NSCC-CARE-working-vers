﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NursingEducationalBackend.DTOs;
using NursingEducationalBackend.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NursingEducationalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly NursingDbContext _context;

        public AuthController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            NursingDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if email already exists
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return BadRequest(new { Success = false, Message = "Email already exists!" });

            // Create Identity user
            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(new { Success = false, Message = "User creation failed! Please check user details and try again.", Errors = result.Errors });

            // Create Nurse record
            var nurse = new Nurse
            {
                Email = model.Email,
                FullName = model.FullName,
                StudentNumber = model.StudentNumber,
                Campus = model.Campus  // Add the new Campus field
            };

            await _context.Nurses.AddAsync(nurse);
            await _context.SaveChangesAsync();

            // Update the user with NurseId claim
            await _userManager.AddClaimAsync(user, new Claim("NurseId", nurse.NurseId.ToString()));

            // Add Campus as a claim
            await _userManager.AddClaimAsync(user, new Claim("Campus", nurse.Campus));

            return Ok(new { Success = true, Message = "User registered successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] NurseLoginRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find user by email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new LoginResponse { Success = false, Message = "Invalid email or password." });

            // Check password
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new LoginResponse { Success = false, Message = "Invalid email or password." });

            // Find corresponding nurse
            var nurse = await _context.Nurses.FirstOrDefaultAsync(n => n.Email == model.Email);
            if (nurse == null)
                return Unauthorized(new LoginResponse { Success = false, Message = "Nurse record not found." });

            // Get user roles
            var userRoles = await _userManager.GetRolesAsync(user);

            // Get user claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            var campusClaim = userClaims.FirstOrDefault(c => c.Type == "Campus");

            // Create claims
            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("NurseId", nurse.NurseId.ToString())
    };

            // Add Campus claim to token if it exists
            if (campusClaim != null)
            {
                authClaims.Add(new Claim("Campus", campusClaim.Value));
            }
            else if (!string.IsNullOrEmpty(nurse.Campus))
            {
                // Fallback to nurse record if claim doesn't exist (for existing users)
                authClaims.Add(new Claim("Campus", nurse.Campus));

                // Optionally add the claim to the user for future logins
                await _userManager.AddClaimAsync(user, new Claim("Campus", nurse.Campus));
            }

            // Add role claims
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            // Generate token
            var token = GenerateJwtToken(authClaims);

            return Ok(new LoginResponse
            {
                Success = true,
                Token = token,
                NurseId = nurse.NurseId,
                FullName = nurse.FullName,
                Email = nurse.Email,
                Campus = nurse.Campus,  // Add the Campus to the login response
                Roles = userRoles.ToList()
            });
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // For JWT, logout is typically handled client-side by removing the token
            // This endpoint mainly exists for completeness and future extensions
            await _signInManager.SignOutAsync();
            return Ok(new { Success = true, Message = "Logged out successfully" });
        }

        private string GenerateJwtToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: signIn
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}