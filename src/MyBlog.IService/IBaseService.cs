using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.IService
{
    public interface IBaseService<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 创建一个<see cref="TEntity"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> CreateAsync(TEntity entity);

        /// <summary>
        /// 通过<see cref="TEntity"/>的ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// 编辑<see cref="TEntity"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> EditAsync(TEntity entity);

        /// <summary>
        /// 查找<see cref="TEntity"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(int id);

        /// <summary>
        /// 查找<see cref="TEntity"/>
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> func);

        /// <summary>
        /// 查询全部的数据
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync();

        /// <summary>
        /// 自定义条件查询
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(int page, int size, RefAsync<int> total);

        /// <summary>
        /// 自定义条件分页查询
        /// </summary>
        /// <param name="func"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func, int page, int size, RefAsync<int> total);
    }
}
