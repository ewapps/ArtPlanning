//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class task_service
    {
        public int id { get; set; }
        public int task_id { get; set; }
        public int service_id { get; set; }
        public int service_type_id { get; set; }
        public string driver_id { get; set; }
        public Nullable<System.DateTime> added_date { get; set; }
        public string added_user { get; set; }
        public Nullable<System.DateTime> modification_date { get; set; }
        public string modification_user { get; set; }
    
        public virtual service service { get; set; }
        public virtual task_service_type task_service_type { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual task task { get; set; }
    }
}