﻿using Modules.Core.Repository.Base;
using Modules.Template.Domain.Base;
using SqlSugar;

namespace Modules.Template.Repository.Base
{
    public class BaseRepository<TEntity> : CoreRepository<TEntity>, IBaseRepository<TEntity> where TEntity : class, new()
    {
        public BaseRepository(ISqlSugarClient _DbClient) : base(_DbClient)
        {
            //指定模块使用的数据库
            //base.SetDbClient("数据库ID");
        }
    }
}
