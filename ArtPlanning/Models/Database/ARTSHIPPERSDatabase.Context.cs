﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArtPlanning.Models.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ARTSHIPPERSDatabase : DbContext
    {
        public ARTSHIPPERSDatabase()
            : base("name=ARTSHIPPERSDatabase")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<address> address { get; set; }
        public virtual DbSet<folder> folder { get; set; }
        public virtual DbSet<language> language { get; set; }
        public virtual DbSet<material> material { get; set; }
        public virtual DbSet<mission> mission { get; set; }
        public virtual DbSet<mission_type> mission_type { get; set; }
        public virtual DbSet<service> service { get; set; }
        public virtual DbSet<task_material> task_material { get; set; }
        public virtual DbSet<task_service> task_service { get; set; }
        public virtual DbSet<task_service_type> task_service_type { get; set; }
        public virtual DbSet<task_staff> task_staff { get; set; }
        public virtual DbSet<task_type> task_type { get; set; }
        public virtual DbSet<task_vehicle> task_vehicle { get; set; }
        public virtual DbSet<translate_material> translate_material { get; set; }
        public virtual DbSet<translate_service> translate_service { get; set; }
        public virtual DbSet<translate_task_type> translate_task_type { get; set; }
        public virtual DbSet<translate_vehicle> translate_vehicle { get; set; }
        public virtual DbSet<vehicle> vehicle { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<task> task { get; set; }
        public virtual DbSet<warehouse> warehouse { get; set; }
        public virtual DbSet<country> country { get; set; }
    }
}
