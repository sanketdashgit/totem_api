using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Totem.Database.StoreProcedure;

namespace Totem.Database.Models
{
    public partial class TotemDBContext 
    {
        
        public virtual DbSet<Sp_GetEvent> Sp_EventByID { get; set; }

        public virtual DbSet<Sp_GetUserDetails> Sp_GetUserDetails { get; set; }
    }
}
