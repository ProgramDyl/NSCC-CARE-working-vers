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
                            var eliminationChanges = await handler.SubmitEliminationData(_context, value, record, patientIdFromTitle);

                            if (eliminationChanges is OkObjectResult elimChanges && eliminationChanges != null)
                            {
                                mostRecentlyChangedData["elimination"] = elimChanges.Value;
                            }
                            break;

                        case "mobility":
                            var mobilityChanges = await handler.SubmitMobilityData(_context, value, record, patientIdFromTitle);
                            if (mobilityChanges is OkObjectResult mobilChanges && mobilityChanges != null)
                            {
                                mostRecentlyChangedData["mobility"] = mobilChanges.Value;
                            }
                            break;
                        case "nutrition":
                            var nutritionChanges = await handler.SubmitNutritionData(_context, value, record, patientIdFromTitle);
                            if (nutritionChanges is OkObjectResult nutrChanges && nutritionChanges != null)
                            {
                                mostRecentlyChangedData["nutrition"] = nutrChanges.Value;
                            }
                            break;

                        case "cognitive":
                            var cognitiveChanges = await handler.SubmitCognitiveData(_context, value, record, patientIdFromTitle);
                            if (cognitiveChanges is OkObjectResult cogChanges && cognitiveChanges != null)
                            {
                                mostRecentlyChangedData["cognitive"] = cogChanges.Value;
                            }
                            break;

                        case "safety":
                            var safetyChanges = await handler.SubmitSafetyData(_context, value, record, patientIdFromTitle);
                            if (safetyChanges is OkObjectResult safeChanges && safetyChanges != null)
                            {
                                mostRecentlyChangedData["safety"] = safeChanges.Value;
                            }
                            break;

                        case "adl":
                            var adlChanges = await handler.SubmitAdlData(_context, value, record, patientIdFromTitle);
                            if (adlChanges is OkObjectResult changesToAdls && adlChanges != null)
                            {
                                mostRecentlyChangedData["adl"] = changesToAdls.Value;
                            }
                            break;

                        case "behaviour":
                            var behaviourChanges = await handler.SubmitBehaviourData(_context, value, record, patientIdFromTitle);
                            if (behaviourChanges is OkObjectResult behaveChanges && behaviourChanges != null)
                            {
                                mostRecentlyChangedData["behaviour"] = behaveChanges.Value;
                            }
                            break;

                        case "progressnote":
                            var progressNoteChanges = await handler.SubmitProgressNoteData(_context, value, record, patientIdFromTitle);
                            if (progressNoteChanges is OkObjectResult progNoteChanges && progressNoteChanges != null)
                            {
                                mostRecentlyChangedData["progressnote"] = progNoteChanges.Value;
                            }
                            break;

                        case "skinsensoryaid":
                            var skinChanges = await handler.SubmitSkinAndSensoryAidData(_context, value, record, patientIdFromTitle);
                            if (skinChanges is OkObjectResult skChanges && skinChanges != null)
                            {
                                mostRecentlyChangedData["skinsensoryaid"] = skChanges.Value;
                            }
                            break;

                        case "profile":
                            var profileChanges = await handler.SubmitProfileData(_context, value, patient);
                            if (profileChanges is OkObjectResult profChanges && profileChanges != null)
                            {
                                mostRecentlyChangedData["profile"] = profChanges.Value;
                            }
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