//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccountingSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            this.StudentStatusOnLessons = new HashSet<StudentStatusOnLesson>();
        }
    
        public int StudentId { get; set; }
        public int GroupId { get; set; }
        public int AddressId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> StudentStateId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }
        public System.DateTime DateBirth { get; set; }
        public int PassportSeries { get; set; }
        public int PassportNumber { get; set; }
        public string PassportSource { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual Group Group { get; set; }
        public virtual StudentState StudentState { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentStatusOnLesson> StudentStatusOnLessons { get; set; }
    }
}