﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NursingEducationalBackend.Models;

[Table("ADLs")]  // Specify the exact table name in the database
public partial class Adl
{
    [Column("ADLsID")]  // Specify the exact column name in the database
    public int AdlsId { get; set; }
    
    [Range(typeof(DateTime), "1900-01-01 00:00:00", "3000-12-31 00:00:00")]
    public DateTime? BathDate { get; set; }
    public string? TubShowerOther { get; set; }
    public string? TypeOfCare { get; set; }
    public string? TurningSchedule { get; set; }
    public string? Teeth { get; set; }
    public string? FootCare { get; set; }
    public string? HairCare { get; set; }
}