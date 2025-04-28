// --- In Models/Record.cs ---
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization; // Keep if needed, but EF Core Nav props usually aren't serialized directly like this
using System.ComponentModel.DataAnnotations; // Add if using attributes like [Key]
using System.ComponentModel.DataAnnotations.Schema; // Add if using attributes like [ForeignKey]


namespace NursingEducationalBackend.Models;

public partial class Record
{
    [Key] // Assuming RecordId is the primary key
    public int RecordId { get; set; }

    [ForeignKey("Patient")] // Links PatientId to the Patient navigation property
    public int? PatientId { get; set; }

    // Foreign Keys (Already present)
    public int? CognitiveId { get; set; }
    public int? NutritionId { get; set; }
    public int? EliminationId { get; set; }
    public int? MobilityId { get; set; }
    public int? SafetyId { get; set; }
    public int? AdlsId { get; set; } // Note: Name mismatch with 'Adl' model?
    public int? SkinId { get; set; } // Note: Name mismatch with 'SkinAndSensoryAid' model?
    public int? BehaviourId { get; set; }
    public int? ProgressNoteId { get; set; }

    // --- Navigation Properties ---

    // Existing Patient Navigation Property
    [JsonIgnore] // Keep JsonIgnore if you don't want it in API responses automatically
    public virtual Patient? Patient { get; set; }

    // *** ADD THESE MISSING NAVIGATION PROPERTIES ***
    // Make them nullable '?' to match the nullable foreign keys 'int?'
    // Use 'virtual' for lazy loading or change tracking proxies (standard practice)

    [ForeignKey("CognitiveId")] // Optional: Explicitly link FK to Navigation Property
    public virtual Cognitive? Cognitive { get; set; }

    [ForeignKey("NutritionId")]
    public virtual Nutrition? Nutrition { get; set; }

    [ForeignKey("EliminationId")]
    public virtual Elimination? Elimination { get; set; }

    [ForeignKey("MobilityId")]
    public virtual Mobility? Mobility { get; set; }

    [ForeignKey("SafetyId")]
    public virtual Safety? Safety { get; set; }

    [ForeignKey("AdlsId")] // Does this link to an 'Adl' entity?
    public virtual Adl? Adl { get; set; } // Naming: Nav prop 'Adl' vs FK 'AdlsId'

    [ForeignKey("SkinId")] // Does this link to a 'SkinAndSensoryAid' entity?
    public virtual SkinAndSensoryAid? SkinAndSensoryAid { get; set; } // Naming: Nav prop 'SkinAndSensoryAid' vs FK 'SkinId'

    [ForeignKey("BehaviourId")]
    public virtual Behaviour? Behaviour { get; set; }

    [ForeignKey("ProgressNoteId")] // Assuming 1-to-1 relationship here
    public virtual ProgressNote? ProgressNote { get; set; }
    // If it's 1-to-Many (Record has many ProgressNotes), it would be:
    // public virtual ICollection<ProgressNote> ProgressNotes { get; set; } = new List<ProgressNote>();


    // --- End Navigation Properties ---
}