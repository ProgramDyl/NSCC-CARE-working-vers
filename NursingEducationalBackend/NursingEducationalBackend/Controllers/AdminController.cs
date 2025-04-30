using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NursingEducationalBackend.DTOs;
using NursingEducationalBackend.Models;
using System;
using System.Threading.Tasks;

namespace NursingEducationalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Ensure only admins can access these endpoints
    public class AdminController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly NursingDbContext _context;

        public AdminController(
            UserManager<IdentityUser> userManager,
            NursingDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Deletes a user by email
        /// </summary>
        [HttpDelete("users/{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound(new { Success = false, Message = "User not found." });

            // Find the associated nurse record
            var nurse = await _context.Nurses.FirstOrDefaultAsync(n => n.Email == email);
            
            // Begin a transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Delete the nurse record if it exists
                if (nurse != null)
                {
                    _context.Nurses.Remove(nurse);
                    await _context.SaveChangesAsync();
                }

                // Delete the identity user
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new { Success = false, Message = "Failed to delete user.", Errors = result.Errors });
                }

                await transaction.CommitAsync();
                return Ok(new { Success = true, Message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Success = false, Message = "An error occurred while deleting the user.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Resets a user's password
        /// </summary>
        [HttpPost("users/{email}/reset-password")]
        public async Task<IActionResult> ResetUserPassword(string email, [FromBody] ResetPasswordRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound(new { Success = false, Message = "User not found." });

            // Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            // Reset the password
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (!result.Succeeded)
                return BadRequest(new { Success = false, Message = "Password reset failed.", Errors = result.Errors });

            return Ok(new { Success = true, Message = "Password reset successfully." });
        }

        /// <summary>
        /// Resets the admin's own password
        /// </summary>
        [HttpPost("reset-my-password")]
        public async Task<IActionResult> ResetAdminPassword([FromBody] ChangePasswordRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get the current user
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser == null)
                return NotFound(new { Success = false, Message = "User not found." });

            // Change the password
            var result = await _userManager.ChangePasswordAsync(currentUser, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
                return BadRequest(new { Success = false, Message = "Password change failed.", Errors = result.Errors });

            return Ok(new { Success = true, Message = "Password changed successfully." });
        }

        /// <summary>
        /// Updates a patient's information except for PatientId
        /// </summary>
        [HttpPut("patients/{patientId}")]
        public async Task<IActionResult> UpdatePatient(int patientId, [FromBody] UpdatePatientRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find the patient
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null)
                return NotFound(new { Success = false, Message = "Patient not found." });

            // Update patient properties except for PatientId
            patient.NurseId = model.NurseId;
            patient.ImageFilename = model.ImageFilename;
            patient.BedNumber = model.BedNumber;
            patient.NextOfKin = model.NextOfKin;
            patient.NextOfKinPhone = model.NextOfKinPhone;
            patient.FullName = model.FullName;
            patient.Sex = model.Sex;
            patient.PatientWristId = model.PatientWristId;
            patient.Dob = model.Dob;
            patient.AdmissionDate = model.AdmissionDate;
            patient.DischargeDate = model.DischargeDate;
            patient.MaritalStatus = model.MaritalStatus;
            patient.MedicalHistory = model.MedicalHistory;
            patient.Weight = model.Weight;
            patient.Height = model.Height;
            patient.Allergies = model.Allergies;
            patient.IsolationPrecautions = model.IsolationPrecautions;
            patient.Unit = model.Unit;
            patient.RoamAlertBracelet = model.RoamAlertBracelet;

            try
            {
                // Save changes
                await _context.SaveChangesAsync();
                return Ok(new { Success = true, Message = "Patient updated successfully.", Patient = patient });
            }
            catch (DbUpdateException ex)
            {
                // Handle unique constraint violation for PatientWristId
                if (ex.InnerException?.Message.Contains("IX_Patients_PatientWristId") == true)
                {
                    return BadRequest(new { Success = false, Message = "Patient wrist ID already exists." });
                }
                
                return StatusCode(500, new { Success = false, Message = "An error occurred while updating the patient.", Error = ex.Message });
            }
        }
    }
}