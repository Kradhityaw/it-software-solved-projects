﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimulasiDesktopW4
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QuizinAjaEntities : DbContext
    {
        public QuizinAjaEntities()
            : base("name=QuizinAjaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Participant> Participant { get; set; }
        public virtual DbSet<ParticipantAnswer> ParticipantAnswer { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<Quiz> Quiz { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
