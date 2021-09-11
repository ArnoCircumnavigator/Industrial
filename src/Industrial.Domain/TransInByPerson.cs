using Industrial.Infra.Database;
using Microsoft.EntityFrameworkCore;
using System;

namespace Industrial.Domain
{
    /// <summary>
    /// 人工入库
    /// </summary>
    public class TransInByPerson
    {
        /*
         * 按照DDD思想，这里应该只有业务逻辑
         */
        BusinessDbContext _dbContext;//业务数据库
        int _stnID;//站台号

        public TransInByPerson(BusinessDbContext dbContext, int stnID)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _stnID = stnID;
        }
        public void Foo()
        {
            //_dbContext.Items
        }
    }
}
