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
    
    public partial class warehouse
    {
        public string id { get; set; }
        public string parent_id { get; set; }
        public string name { get; set; }
        public short position { get; set; }
        public Nullable<System.DateTime> added_date { get; set; }
        public string added_user { get; set; }
        public Nullable<System.DateTime> modification_date { get; set; }
        public string modification_user { get; set; }
    }
}
