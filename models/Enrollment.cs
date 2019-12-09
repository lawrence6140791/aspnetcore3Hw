﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspcore3hw.models
{
    public partial class Enrollment
    {
        [Key]
        [Column("EnrollmentID")]
        public int EnrollmentId { get; set; }
        [Column("CourseID")]
        public int CourseId { get; set; }
        [Column("StudentID")]
        public int StudentId { get; set; }
        public int? Grade { get; set; }

        [ForeignKey(nameof(CourseId))]
        [InverseProperty("Enrollment")]
        public virtual Course Course { get; set; }
        [ForeignKey(nameof(StudentId))]
        [InverseProperty(nameof(Person.Enrollment))]
        public virtual Person Student { get; set; }
    }
}