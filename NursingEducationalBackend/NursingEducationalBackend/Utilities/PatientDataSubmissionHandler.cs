using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NursingEducationalBackend.DTOs;
using NursingEducationalBackend.Models;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace NursingEducationalBackend.Utilities
{
    public class PatientDataSubmissionHandler : ControllerBase
    {
        public async Task<ActionResult> SubmitEliminationData(NursingDbContext _context, object value, Record record, int patientId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var eliminationData = JsonConvert.DeserializeObject<PatientEliminationDTO>(value.ToString());
                var existingEntry = await _context.Eliminations.FindAsync(patientId);
                var changedFields = new Dictionary<string, object>();

                if (existingEntry != null)
                {
                    foreach (var prop in typeof(PatientEliminationDTO).GetProperties())
                    {
                        var entityProp = typeof(Elimination).GetProperty(prop.Name);
                        if (entityProp != null)
                        {
                            var oldVal = entityProp.GetValue(existingEntry);
                            var newVal = prop.GetValue(eliminationData);

                            if (!Equals(oldVal, newVal))
                            {
                                changedFields[prop.Name] = newVal;
                            }
                        }
                    }

                    _context.Entry(existingEntry).CurrentValues.SetValues(eliminationData);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var eliminationEntity = new Elimination
                    {
                        // Let DB auto-generate the ID
                        IncontinentOfBladder = eliminationData.IncontinentOfBladder,
                        IncontinentOfBowel = eliminationData.IncontinentOfBowel,
                        DayOrNightProduct = eliminationData.DayOrNightProduct,
                        LastBowelMovement = eliminationData.LastBowelMovement,
                        BowelRoutine = eliminationData.BowelRoutine,
                        BladderRoutine = eliminationData.BladderRoutine,
                        CatheterInsertion = eliminationData.CatheterInsertion,
                        CatheterInsertionDate = eliminationData.CatheterInsertionDate
                    };

                    foreach (var prop in typeof(Elimination).GetProperties())
                    {
                        if(prop.Name != "EliminationId")
                        {
                            var val = prop.GetValue(eliminationEntity);
                            if (val != null)
                            {
                                changedFields[prop.Name] = val;
                            }
                        }
                    }

                    _context.Eliminations.Add(eliminationEntity);
                    await _context.SaveChangesAsync();
                    
                    record.EliminationId = eliminationEntity.EliminationId;
                    _context.Update(record);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();            

                return Ok(changedFields);
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Operation failed: {ex.Message}");
            }
        }

        public async Task<ActionResult> SubmitMobilityData(NursingDbContext _context, object value, Record record, int patientId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var mobilityData = JsonConvert.DeserializeObject<PatientMobilityDTO>(value.ToString());            
                var existingEntry = await _context.Mobilities.FindAsync(patientId);
                var changedFields = new Dictionary<string, object>();


                if (existingEntry != null)
                {
                    foreach (var prop in typeof(PatientMobilityDTO).GetProperties())
                    {
                        var entityProp = typeof(Mobility).GetProperty(prop.Name);
                        if (entityProp != null)
                        {
                            var oldVal = entityProp.GetValue(existingEntry);
                            var newVal = prop.GetValue(mobilityData);

                            if (!Equals(oldVal, newVal))
                            {
                                changedFields[prop.Name] = newVal;
                            }
                        }
                    }
                    _context.Entry(existingEntry).CurrentValues.SetValues(mobilityData);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var mobilityEntity = new Mobility
                    {
                        // Let DB auto-generate the ID
                        Transfer = mobilityData.Transfer,
                        Aids = mobilityData.Aids
                    };

                    foreach (var prop in typeof(Mobility).GetProperties())
                    {
                        if (prop.Name != "MobilityId")
                        {
                            var val = prop.GetValue(mobilityEntity);
                            if (val != null)
                            {
                                changedFields[prop.Name] = val;
                            }
                        }
                    }

                    _context.Mobilities.Add(mobilityEntity);
                    await _context.SaveChangesAsync();
                    
                    record.MobilityId = mobilityEntity.MobilityId;
                    _context.Update(record);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return Ok(changedFields);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Operation failed: {ex.Message}");
            }
        }

        public async Task<ActionResult> SubmitNutritionData(NursingDbContext _context, object value, Record record, int patientId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var nutritionData = JsonConvert.DeserializeObject<PatientNutritionDTO>(value.ToString());            
                var existingEntry = await _context.Nutritions.FindAsync(patientId);
                var changedFields = new Dictionary<string, object>();

                if (existingEntry != null)
                {
                    foreach (var prop in typeof(PatientNutritionDTO).GetProperties())
                    {
                        var entityProp = typeof(Nutrition).GetProperty(prop.Name);
                        if (entityProp != null)
                        {
                            var oldVal = entityProp.GetValue(existingEntry);
                            var newVal = prop.GetValue(nutritionData);

                            if (!Equals(oldVal, newVal))
                            {
                                changedFields[prop.Name] = newVal;
                            }
                        }
                    }
                    _context.Entry(existingEntry).CurrentValues.SetValues(nutritionData);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var nutritionEntity = new Nutrition
                    {
                        // Let DB auto-generate the ID
                        Diet = nutritionData.Diet,
                        Assist = nutritionData.Assist,
                        Intake = nutritionData.Intake,
                        Time = nutritionData.Time,
                        DietarySupplementInfo = nutritionData.DietarySupplementInfo,
                        Weight = nutritionData.Weight,
                        Date = nutritionData.Date,
                        Method = nutritionData.Method,
                        IvSolutionRate = nutritionData.IvSolutionRate,
                        SpecialNeeds = nutritionData.SpecialNeeds
                    };

                    foreach (var prop in typeof(Nutrition).GetProperties())
                    {
                        if (prop.Name != "NutritionId")
                        {
                            var val = prop.GetValue(nutritionEntity);
                            if (val != null)
                            {
                                changedFields[prop.Name] = val;
                            }
                        }
                    }

                    _context.Nutritions.Add(nutritionEntity);
                    await _context.SaveChangesAsync();
                    
                    record.NutritionId = nutritionEntity.NutritionId;
                    _context.Update(record);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return Ok(changedFields);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Operation failed: {ex.Message}");
            }
        }

        public async Task<ActionResult> SubmitCognitiveData(NursingDbContext _context, object value, Record record, int patientId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var cognitiveData = JsonConvert.DeserializeObject<PatientCognitiveDTO>(value.ToString());   
                var existingEntry = await _context.Cognitives.FindAsync(patientId);
                var changedFields = new Dictionary<string, object>();

                if (existingEntry != null)
                {
                    foreach (var prop in typeof(PatientCognitiveDTO).GetProperties())
                    {
                        var entityProp = typeof(Cognitive).GetProperty(prop.Name);
                        if (entityProp != null)
                        {
                            var oldVal = entityProp.GetValue(existingEntry);
                            var newVal = prop.GetValue(cognitiveData);

                            if (!Equals(oldVal, newVal))
                            {
                                changedFields[prop.Name] = newVal;
                            }
                        }
                    }
                    _context.Entry(existingEntry).CurrentValues.SetValues(cognitiveData);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var cognitiveEntity = new Cognitive
                    {
                        // Let DB auto-generate the ID
                        Speech = cognitiveData.Speech,
                        Loc = cognitiveData.Loc,
                        Mmse = cognitiveData.Mmse,
                        Confusion = cognitiveData.Confusion                      
                    };

                    foreach (var prop in typeof(Cognitive).GetProperties())
                    {
                        if (prop.Name != "CognitiveId")
                        {
                            var val = prop.GetValue(cognitiveEntity);
                            if (val != null)
                            {
                                changedFields[prop.Name] = val;
                            }
                        }
                    }

                    _context.Cognitives.Add(cognitiveEntity);
                    await _context.SaveChangesAsync();
                    
                    record.CognitiveId = cognitiveEntity.CognitiveId;
                    _context.Update(record);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return Ok(changedFields);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Operation failed: {ex.Message}");
            }
        }

        public async Task<ActionResult> SubmitSafetyData(NursingDbContext _context, object value, Record record, int patientId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var safetyData = JsonConvert.DeserializeObject<PatientSafetyDTO>(value.ToString());            
                var existingEntry = await _context.Safeties.FindAsync(patientId);
                var changedFields = new Dictionary<string, object>();

                if (existingEntry != null)
                {
                    foreach (var prop in typeof(PatientSafetyDTO).GetProperties())
                    {
                        var entityProp = typeof(Safety).GetProperty(prop.Name);
                        if (entityProp != null)
                        {
                            var oldVal = entityProp.GetValue(existingEntry);
                            var newVal = prop.GetValue(safetyData);

                            if (!Equals(oldVal, newVal))
                            {
                                changedFields[prop.Name] = newVal;
                            }
                        }
                    }
                    _context.Entry(existingEntry).CurrentValues.SetValues(safetyData);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var safetyEntity = new Safety
                    {
                        // Let DB auto-generate the ID
                        HipProtectors = safetyData.HipProtectors,
                        SideRails = safetyData.SideRails,
                        FallRiskScale = safetyData.FallRiskScale,
                        CrashMats = safetyData.CrashMats,
                        BedAlarm = safetyData.BedAlarm
                    };

                    foreach (var prop in typeof(Safety).GetProperties())
                    {
                        if (prop.Name != "SafetyId")
                        {
                            var val = prop.GetValue(safetyEntity);
                            if (val != null)
                            {
                                changedFields[prop.Name] = val;
                            }
                        }
                    }

                    _context.Safeties.Add(safetyEntity);
                    await _context.SaveChangesAsync();
                    
                    record.SafetyId = safetyEntity.SafetyId;
                    _context.Update(record);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return Ok(changedFields);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Operation failed: {ex.Message}");
            }
        }

        public async Task<ActionResult> SubmitAdlData(NursingDbContext _context, object value, Record record, int patientId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var adlData = JsonConvert.DeserializeObject<PatientAdlDTO>(value.ToString());            
                var existingEntry = await _context.Adls.FindAsync(patientId);
                var changedFields = new Dictionary<string, object>();

                if (existingEntry != null)
                {
                    foreach (var prop in typeof(PatientAdlDTO).GetProperties())
                    {
                        var entityProp = typeof(Adl).GetProperty(prop.Name);
                        if (entityProp != null)
                        {
                            var oldVal = entityProp.GetValue(existingEntry);
                            var newVal = prop.GetValue(adlData);

                            if (!Equals(oldVal, newVal))
                            {
                                changedFields[prop.Name] = newVal;
                            }
                        }
                    }
                    _context.Entry(existingEntry).CurrentValues.SetValues(adlData);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var adlEntity = new Adl
                    {
                        // Let DB auto-generate the ID
                        BathDate = adlData.BathDate,
                        TubShowerOther = adlData.TubShowerOther,
                        TypeOfCare = adlData.TypeOfCare,
                        TurningSchedule = adlData.TurningSchedule,
                        Teeth = adlData.Teeth,
                        FootCare = adlData.FootCare,
                        HairCare = adlData.HairCare
                    };

                    foreach (var prop in typeof(Adl).GetProperties())
                    {
                        if (prop.Name != "AdlsId")
                        {
                            var val = prop.GetValue(adlEntity);
                            if (val != null)
                            {
                                changedFields[prop.Name] = val;
                            }
                        }
                    }

                    _context.Adls.Add(adlEntity);
                    await _context.SaveChangesAsync();
                    
                    record.AdlsId = adlEntity.AdlsId;
                    _context.Update(record);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return Ok(changedFields);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Operation failed: {ex.Message}");
            }
        }

        public async Task<ActionResult> SubmitBehaviourData(NursingDbContext _context, object value, Record record, int patientId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var behaviourData = JsonConvert.DeserializeObject<PatientBehaviourDTO>(value.ToString());            
                var existingEntry = await _context.Behaviours.FindAsync(patientId);
                var changedFields = new Dictionary<string, object>();

                if (existingEntry != null)
                {
                    foreach (var prop in typeof(PatientBehaviourDTO).GetProperties())
                    {
                        var entityProp = typeof(Behaviour).GetProperty(prop.Name);
                        if (entityProp != null)
                        {
                            var oldVal = entityProp.GetValue(existingEntry);
                            var newVal = prop.GetValue(behaviourData);

                            if (!Equals(oldVal, newVal))
                            {
                                changedFields[prop.Name] = newVal;
                            }
                        }
                    }
                    _context.Entry(existingEntry).CurrentValues.SetValues(behaviourData);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var behaviourEntity = new Behaviour
                    {
                        // Let DB auto-generate the ID
                        Report = behaviourData.Report
                    };

                    foreach (var prop in typeof(Behaviour).GetProperties())
                    {
                        if (prop.Name != "BehaviourId")
                        {
                            var val = prop.GetValue(behaviourEntity);
                            if (val != null)
                            {
                                changedFields[prop.Name] = val;
                            }
                        }
                    }

                    _context.Behaviours.Add(behaviourEntity);
                    await _context.SaveChangesAsync();
                    
                    record.BehaviourId = behaviourEntity.BehaviourId;
                    _context.Update(record);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return Ok(changedFields);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Operation failed: {ex.Message}");
            }
        }

        public async Task<ActionResult> SubmitSkinAndSensoryAidData(NursingDbContext _context, object value, Record record, int patientId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var skinData = JsonConvert.DeserializeObject<PatientSkinDTO>(value.ToString());            
                var existingEntry = await _context.SkinAndSensoryAids.FindAsync(patientId);
                var changedFields = new Dictionary<string, object>();

                if (existingEntry != null)
                {
                    foreach (var prop in typeof(PatientSkinDTO).GetProperties())
                    {
                        var entityProp = typeof(SkinAndSensoryAid).GetProperty(prop.Name);
                        if (entityProp != null)
                        {
                            var oldVal = entityProp.GetValue(existingEntry);
                            var newVal = prop.GetValue(skinData);

                            if (!Equals(oldVal, newVal))
                            {
                                changedFields[prop.Name] = newVal;
                            }
                        }
                    }
                    _context.Entry(existingEntry).CurrentValues.SetValues(skinData);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var skinAndSensoryAidsEntity = new SkinAndSensoryAid
                    {
                        // Let DB auto-generate the ID
                        Glasses = skinData.Glasses,
                        Hearing = skinData.Hearing,
                        SkinIntegrityPressureUlcerRisk = skinData.SkinIntegrityPressureUlcerRisk,
                        SkinIntegrityTurningSchedule = skinData.SkinIntegrityTurningSchedule,
                        SkinIntegrityBradenScale = skinData.SkinIntegrityBradenScale,
                        SkinIntegrityDressings = skinData.SkinIntegrityDressings
                    };

                    foreach (var prop in typeof(SkinAndSensoryAid).GetProperties())
                    {
                        if (prop.Name != "SkinAndSensoryAidsId")
                        {
                            var val = prop.GetValue(skinAndSensoryAidsEntity);
                            if (val != null)
                            {
                                changedFields[prop.Name] = val;
                            }
                        }
                    }

                    _context.SkinAndSensoryAids.Add(skinAndSensoryAidsEntity);
                    await _context.SaveChangesAsync();
                    
                    record.SkinId = skinAndSensoryAidsEntity.SkinAndSensoryAidsId;
                    _context.Update(record);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return Ok(changedFields);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Operation failed: {ex.Message}");
            }
        }

        public async Task<ActionResult> SubmitProgressNoteData(NursingDbContext _context, object value, Record record, int patientId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var progressNoteData = JsonConvert.DeserializeObject<PatientProgressNoteDTO>(value.ToString());
                var changedFields = new Dictionary<string, object>();

                // Create a new progress note without specifying the ID
                var progressNoteEntity = new ProgressNote
                {
                    // Don't set ProgressNoteId - let the database generate it
                    Timestamp = progressNoteData.Timestamp,
                    Note = progressNoteData.Note
                };

                foreach (var prop in typeof(ProgressNote).GetProperties())
                {
                    if (prop.Name != "ProgressNoteId")
                    {
                        var val = prop.GetValue(progressNoteEntity);
                        if (val != null)
                        {
                            changedFields[prop.Name] = val;
                        }
                    }
                }

                // Add to database first to get the auto-generated ID
                _context.ProgressNotes.Add(progressNoteEntity);
                await _context.SaveChangesAsync();
                
                // Now update the record with the generated ID
                record.ProgressNoteId = progressNoteEntity.ProgressNoteId;
                _context.Update(record);
                await _context.SaveChangesAsync();
                
                await transaction.CommitAsync();
                return Ok(changedFields);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Operation failed: {ex.Message}");
            }
        }

        public async Task<ActionResult> SubmitProfileData(NursingDbContext _context, object value, Patient patient)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var profileData = JsonConvert.DeserializeObject<PatientProfileDTO>(value.ToString());           
                var existingEntry = await _context.Patients.FindAsync(patient.PatientId);
                var changedFields = new Dictionary<string, object>();

                if (existingEntry != null)
                {
                    foreach (var prop in typeof(PatientProfileDTO).GetProperties())
                    {
                        var entityProp = typeof(Patient).GetProperty(prop.Name);
                        if (entityProp != null)
                        {
                            var oldVal = entityProp.GetValue(existingEntry);
                            var newVal = prop.GetValue(profileData);

                            if (!Equals(oldVal, newVal))
                            {
                                changedFields[prop.Name] = newVal;
                            }
                        }
                    }
                    _context.Entry(existingEntry).CurrentValues.SetValues(profileData);
                    await _context.SaveChangesAsync();
                }
                
                await transaction.CommitAsync();
                return Ok(changedFields);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Operation failed: {ex.Message}");
            }
        }
    }
}