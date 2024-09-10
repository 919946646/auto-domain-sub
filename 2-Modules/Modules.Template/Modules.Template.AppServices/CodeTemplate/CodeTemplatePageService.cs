using MapsterMapper;
using Modules.Core.AppServices.Authentication;
using Modules.Core.AppServices.Base_IdServer;
using Modules.Core.AppServices.Base_log;
using Modules.Core.AppServices.Base_user_columns;
using Modules.Core.AppServices.ObjectServices;
using Modules.Core.Domain.DomainServices.ColumnHeader;
using Modules.Core.Domain.ObjectDomain;
using Modules.Core.Domain.TableDataModel;
using Modules.Core.Shared;
using Modules.Template.Domain.CodeTemplate;
using Modules.Template.Domain.DbEntity;
using Modules.Template.Shared.CodeTemplate;
using System.Text.Json;

namespace Modules.Template.AppServices.CodeTemplate
{
    public class CodeTemplatePageService : ICodeTemplatePageService
    {
        private readonly ICodeTemplateRepository _repository;
        private ICurrentUserService _currentUserService;
        private IBase_user_columnsService user_ColumnsService;
        private IBase_logService logService;
        private IObjectService objectService;
        private IMapper _mapper;
        public TableDataModel<CodeTemplateVM> TableDataModel { get; private set; }
        public ObjectModel ObjectModel { get; private set; } = new ObjectModel();
        public CodeTemplatePageService(IMapper _mapper, IObjectService objectService, ICodeTemplateRepository Repository, IBase_logService logService, IBase_user_columnsService user_ColumnsService, ICurrentUserService CurrentUser)
        {
            this.objectService = objectService;
            this._currentUserService = CurrentUser;
            this.user_ColumnsService = user_ColumnsService;
            this.logService = logService;
            _repository = Repository;
            this._mapper = _mapper;
        }

        private string Old_Id = string.Empty;
        public async Task<Result> InitAsync(string IdOrTablename)
        {
            if (Old_Id != IdOrTablename)
            {
                Old_Id = IdOrTablename;//避免同一个Id被多次初始化

                this.ObjectModel = await objectService.GetObjectModel(IdOrTablename);
                var ObjectId = this.ObjectModel.IsValid() ? ObjectModel.ObjectInfo.Id : IdOrTablename;

                SqlSugar.DbType dbType = _repository.DbClient.CurrentConnectionConfig.DbType;
                this.TableDataModel = new TableDataModel<CodeTemplateVM>(ObjectId, dbType);

                //下面可选必须由对应的对象ID确定权限
                //if (!this.ObjectModel.IsValid())
                //{
                //    return Core.Shared.Result.Error("初始化失败。" + this.ObjectModel.Errors());
                //}

                if (this.ObjectModel.IsValid())
                {
                    //可选解决方案管理员可编辑
                    //this.TableDataModel.TableOptions.SetSolutionAdminRole(_currentUserService.CurrentUser, ObjectModel.SolutionInfo.Id);

                    //如果是对象ID，合并属性定义的角色权限
                    this.TableDataModel.ColumnHeaderModels = ColumnHeaderService.ProcessColumnHeaderModels(ObjectModel.ObjectInfo, ObjectModel.PropertyList, _currentUserService.CurrentUser.UserRoles);
                    this.TableDataModel.TableOptions.SetRoleByObject(ObjectModel.ObjectInfo, this.TableDataModel.ColumnHeaderModels, _currentUserService.CurrentUser);
                }
                else
                {
                    //ColumnHeaderModel需要从数据库实体获得，，或者从sql查询结果获得，这样才能与数据表一一对应
                    this.TableDataModel.ColumnHeaderModels = ColumnHeaderService.GetColumnHeaderModelFromVM<CodeTemplateEntity>();
                    //不绑定对象，需要自己确定权限
                    this.TableDataModel.TableOptions.IsEdit = true;
                    this.TableDataModel.TableOptions.IsShow = true;
                }

                //合并用户自定义列
                var user_ColumnsVMs = await user_ColumnsService.GetUerTableColumn(ObjectId);
                this.TableDataModel.ColumnHeaderModels = ColumnHeaderService.MergeUserColumnHeaderModel(this.TableDataModel.ColumnHeaderModels, user_ColumnsVMs);
            }
            if (TableDataModel.IsValid())
                return Core.Shared.Result.Success("初始化成功。");
            else
                return Core.Shared.Result.Error("初始化失败。" + TableDataModel.Errors());
        }

        /// <summary>
        /// 设置服务的数据源，方便页面引入的组件使用同一个数据源
        /// </summary>
        /// <param name="list"></param>
        public void SetTableModelDataSource(List<CodeTemplateVM> list)
        {
            if (this.TableDataModel == null) this.TableDataModel = new TableDataModel<CodeTemplateVM>();
            this.TableDataModel.TableModel.DataSource = list;
        }
        /*代码生成器会覆盖标记之间的内容。*/
        //CodeGenerator start

        #region EXCEL导入导出
        public async Task<List<Dictionary<string, object>>> QueryCurrentPageDataAsync()
        {
            var sql = this.TableDataModel.GetPageSql("CodeTemplate"); //取得当前页
            return await _repository.QueryPageDictDataAsync(sql.sql, sql.SqlParameters);
        }
        public async Task<List<Dictionary<string, object>>> QueryTopPageDataAsync()
        {
            var sql = this.TableDataModel.GetTopPageSql("CodeTemplate", 30000); //取得当前筛选条件下的top数据
            return await _repository.QueryPageDictDataAsync(sql.sql, sql.SqlParameters);
        }
        #endregion

        #region 分页查询(sql方式)
        public async Task QueryPageDataAsync()
        {
            var sql = this.TableDataModel.GetPageSql("CodeTemplate");
            var data = await _repository.QueryPageDataAsync(sql.sql, sql.SqlParameters);
            this.TableDataModel.TableModel.DataSource = _mapper.Map<List<CodeTemplateVM>>(data);
        }
        public async Task QueryPageTotalCountAsync()
        {
            var sql = this.TableDataModel.GetPageCountSql("CodeTemplate");
            int total = await _repository.QueryPageTotalCountAsync(sql.sql, sql.SqlParameters);
            this.TableDataModel.TableModel.TotalCount = total;
        }
        #endregion
        #region 分页查询(类方式)
        public async Task QueryPageModelDataAsync()
        {
            var ret = await _repository.QueryPageAsync(TableDataModel.TableModel.PageIndex, TableDataModel.TableModel.PageSize, TableDataModel.TableModel.ConditionalModels, TableDataModel.TableModel.OrderByModel);
            this.TableDataModel.TableModel.DataSource = _mapper.Map<List<CodeTemplateVM>>(ret.Data);
            this.TableDataModel.TableModel.TotalCount = ret.TotalCount;
        }
        #endregion

        #region 添加，删除，修改
        public async Task<bool> RemoveRowDataAsync(CodeTemplateVM row)
        {
            //记录删除日志
            CodeTemplateEntity beforeData = null;
            if (ObjectModel != null && ObjectModel.ObjectInfo != null && ObjectModel.ObjectInfo?.Is_delete_log > 0) beforeData = await _repository.QueryByIdAsync(row.Id);

            var ret = await _repository.DeleteByIdAsync(row.Id);
            if (ret && this.TableDataModel != null)
            {
                this.TableDataModel.TableModel.RemoveRow(row);

                if (ObjectModel != null && ObjectModel.ObjectInfo != null && ObjectModel.ObjectInfo?.Is_delete_log > 0)
                {
                    await logService.AddDeleteLog(JsonSerializer.Serialize(beforeData), ObjectModel.ObjectInfo.Solution_id, ObjectModel.ObjectInfo.Id, ObjectModel.ObjectInfo.Name_short);
                }
            }
            return ret;
        }
        public async Task<bool> RemoveRowDataAsync(List<CodeTemplateVM> rows)
        {
            var IDs = rows.Select(s => s.Id).ToArray();
            //记录删除日志
            List<CodeTemplateEntity> beforeData = null;
            if (ObjectModel != null && ObjectModel.ObjectInfo != null && ObjectModel.ObjectInfo?.Is_delete_log > 0) beforeData = await _repository.QueryByIDsAsync(IDs);

            var ret = await _repository.DeleteByIdsAsync(IDs);
            if (ret && this.TableDataModel != null)
            {
                this.TableDataModel.TableModel.RemoveRow(rows);
                if (ObjectModel != null && ObjectModel.ObjectInfo != null && ObjectModel.ObjectInfo?.Is_delete_log > 0)
                {
                    //记录删除日志
                    await logService.AddDeleteLog(JsonSerializer.Serialize(beforeData), ObjectModel.ObjectInfo.Solution_id, ObjectModel.ObjectInfo.Id, ObjectModel.ObjectInfo.Name_short);
                }
            }
            return ret;
        }
        public async Task<CodeTemplateVM?> AddRowDataAsync(CodeTemplateVM row)
        {
            row.Id = YitIdHelper.NextId().ToString();
            row.Createtime = DateTime.Now;
            row.Createuid = _currentUserService.CurrentUser.Authname;
            var ret = await _repository.InsertAsync(_mapper.Map<CodeTemplateEntity>(row));
            if (ret > 0 && this.TableDataModel != null)
            {
                this.TableDataModel.TableModel.AddRow(row);
                return row;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> AddRowDataAsync(List<CodeTemplateVM> rows)
        {
            //如果没有主键，添加主键值
            NullIdServer<CodeTemplateVM>.ProcessNullId(rows);

            foreach (var row in rows)
            {
                row.Id = YitIdHelper.NextId().ToString();
                row.Createtime = DateTime.Now;
                row.Createuid = _currentUserService.CurrentUser.Authname;
            }

            var ret = await _repository.InsertAsync(_mapper.Map<List<CodeTemplateEntity>>(rows));
            if (ret > 0 && this.TableDataModel != null)
            {
                this.TableDataModel.TableModel.AddRow(rows);
            }
            return ret > 0;
        }
        public async Task<bool> UpdateRowDataAsync(CodeTemplateVM row)
        {
            row.Updatetime = DateTime.Now;
            row.Updateuid = _currentUserService.CurrentUser.Authname;
            //记录修改日志
            CodeTemplateEntity beforeData = null;
            if (ObjectModel.ObjectInfo != null && ObjectModel.ObjectInfo?.Is_edit_log > 0) beforeData = await _repository.QueryByIdAsync(row.Id);

            bool ret = await _repository.UpdateAsync(_mapper.Map<CodeTemplateEntity>(row));
            if (ret && this.TableDataModel != null)
            {
                this.TableDataModel.TableModel.UpdateRow(row);

                if (ObjectModel.ObjectInfo != null && ObjectModel.ObjectInfo?.Is_edit_log > 0) await logService.AddEditLog(JsonSerializer.Serialize(beforeData), JsonSerializer.Serialize(row), ObjectModel.ObjectInfo.Solution_id, ObjectModel.ObjectInfo.Id, ObjectModel.ObjectInfo.Name_short);
            }
            return ret;
        }
        public async Task<bool> UpdateRowDataAsync(List<CodeTemplateVM> rows)
        {
            foreach (var row in rows)
            {
                row.Updatetime = DateTime.Now;
                row.Updateuid = _currentUserService.CurrentUser.Authname;
            }

            //记录修改日志
            var IDs = rows.Select(s => s.Id).ToArray();
            List<CodeTemplateEntity> beforeData = null;
            if (ObjectModel.ObjectInfo != null && ObjectModel.ObjectInfo?.Is_edit_log > 0) beforeData = await _repository.QueryByIDsAsync(IDs);

            bool ret = await _repository.UpdateAsync(_mapper.Map<List<CodeTemplateEntity>>(rows));
            if (ret && this.TableDataModel != null)
            {
                this.TableDataModel.TableModel.RemoveRow(rows);

                if (ObjectModel.ObjectInfo != null && ObjectModel.ObjectInfo?.Is_edit_log > 0) await logService.AddEditLog(JsonSerializer.Serialize(beforeData), JsonSerializer.Serialize(rows), ObjectModel.ObjectInfo.Solution_id, ObjectModel.ObjectInfo.Id, ObjectModel.ObjectInfo.Name_short);
            }
            return ret;
        }
        #endregion
        //CodeGenerator end
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
