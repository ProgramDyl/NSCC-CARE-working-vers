using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NursingEducationalBackend.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NursingEducationalBackend.DTOs;
using NursingEducationalBackend.Utilities;
using Azure;

namespace NursingEducationalBackend.Controllers
{
    [Route("api/patients")]
    [ApiController]
    //[Authorize]
    public class PatientsWriteController : ControllerBase
    {
        private readonly NursingDbContext _context;
        
        public PatientsWriteController(NursingDbContext context)
        {
            _context = context;
        }
        
        //Create patient
        [HttpPost("create")]
        //[Authorize]
        public async Task<ActionResult> CreatePatient([FromBody] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
                _context.Records.Add(new Record { PatientId = patient.PatientId });
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("Unable to create patient");
            }
        }
        
        //Assign nurseId to patient
        [HttpPost("{id}/assign-nurse/{nurseId}")]
        //[Authorize]
        public async Task<ActionResult> AssignNurseToPatient(int id, int nurseId)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
            
            if (patient != null)
            {
                patient.NurseId = nurseId;
                _context.Update(patient);
                await _context.SaveChangesAsync();
                return Ok(new { patient.NurseId });
            }
            else
            {
                return BadRequest("Nurse id unable to be assigned");
            }
        }
        
        [HttpPost("{id}/submit-data")]
        public async Task<ActionResult> SubmitData(int id, [FromBody] Dictionary<string, object> patientData)
        {
            var mostRecentlyChangedData = new Dictionary<string, object>();
            try
            {
                PatientDataSubmissionHandler handler = new PatientDataSubmissionHandler();
                
                // Get patient data once outside the loop
                var patient = await _context.Patients
                    .Include(p => p.Records)
                    .FirstOrDefaultAsync(p => p.PatientId == id);
                    
                if (patient == null)
                {
                    return NotFound("Patient not found");
                }
                    
                var record = patient.Records.FirstOrDefault();
                if (record == null)
                {
                    record = new Record { PatientId = patient.PatientId };
                    _context.Records.Add(record);
                    await _context.SaveChangesAsync();
                }
                
                foreach (var entry in patientData)
                {
                    var key = entry.Key;
                    var value = entry.Value;
                    
                    if (value == null) continue;
                    
                    string[] keyParts = key.Split('-');
                    if (keyParts.Length < 2) continue;
                    
                    var tableType = keyParts[1].ToLower();
                    var patientIdFromTitle = keyParts.Length > 2 && int.TryParse(keyParts[2], out int patientId) 
                        ? patientId 
                        : id;
                    
                    switch (tableType)
                    {
                        case "elimination":
                            var eliminationResult = await handler.SubmitEliminationData(_context, value, record, patientIdFromTitle);

                            if (eliminationResult is OkObjectResult okResult)
                            {
                                mostRecentlyChangedData["elimination"] = okResult.Value;
                            }
                            break;
                        case "mobility":
                            await handler.SubmitMobilityData(_context, value, record, patientIdFromTitle);
                            break;
                        case "nutrition":
                            await handler.SubmitNutritionData(_context, value, record, patientIdFromTitle);
                            break;
                        case "cognitive":
                            await handler.SubmitCognitiveData(_context, value, record, patientIdFromTitle);
                            break;
                        case "safety":
                            await handler.SubmitSafetyData(_context, value, record, patientIdFromTitle);
                            break;
                        case "adl":
                            await handler.SubmitAdlData(_context, value, record, patientIdFromTitle);
                            break;
                        case "behaviour":
                            await handler.SubmitBehaviourData(_context, value, record, patientIdFromTitle);
                            break;
                        case "progressnote":
                            await handler.SubmitProgressNoteData(_context, value, record, patientIdFromTitle);
                            break;
                        case "skinsensoryaid":
                            await handler.SubmitSkinAndSensoryAidData(_context, value, record, patientIdFromTitle);
                            break;
                        case "profile":
                            await handler.SubmitProfileData(_context, value, patient);
                            break;
                    }
                }

                var responseData = new
                {
                    message = "Data submitted successfully",
                    data = mostRecentlyChangedData
                };

                return Ok(responseData);

                //return Ok("Data submitted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error submitting data: {ex.Message}");
            }
        }
    }
}