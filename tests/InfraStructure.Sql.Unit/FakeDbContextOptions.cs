using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure.Sql.Unit
{
    /// <summary>
    /// Used this instead of Mock because I was keep getting Null Reference on dbContext creation
    /// </summary>
    internal class FakeDbContextOptions : DbContextOptions
    {
        public override Type ContextType => typeof(object);

        public override DbContextOptions WithExtension<TExtension>(TExtension extension)
        {
            throw new NotImplementedException();
        }
    }
}
