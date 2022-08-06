using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLogic.DataAccess.Services.Interfaces
{
    public interface IDataService
    {
        public void DeleteSource();

        public void Dispose();
    }
}
