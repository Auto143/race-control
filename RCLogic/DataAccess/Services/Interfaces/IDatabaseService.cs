using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLogic.DataAccess.Services.Interfaces
{
    public interface IDatabaseService
    {
        public DbContext DBContext { get; }

        public void Dispose();
    }
}
