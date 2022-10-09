using Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure.Sql.Unit
{
    /// <summary>
    /// Helper class to provide sets as mocks
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DbSetMocker<T> where T:class
    {
        public static Mock<DbSet<T>> GetMockSet(IQueryable<T> fakeSet)
        {
            var moviesMockSet = new Mock<DbSet<T>>();
            moviesMockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(fakeSet.Provider);
            moviesMockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(fakeSet.Expression);
            moviesMockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(fakeSet.ElementType);
            moviesMockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(fakeSet.GetEnumerator());

            return moviesMockSet;
        }
    }
}
