using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Dapper;
using MySql.Data.MySqlClient;

namespace School
{
   public static class DapperHelper
   {
       private static string Host = "Data Source=gxcm1234.mysql.huhehaote.rds.aliyuncs.com;port=3306;Initial Catalog=back_db;uid=yuanren;password=J3BdtwFAHHW1S1vZo8bn;Charset=utf8;";
       public static async Task<IEnumerable<T>> Execute<T>() where T:class ,new ()
       {
           var name = nameof(T);
           var sql = $"select * from {name}";
           using (MySqlConnection conn=new MySqlConnection(Host))
           {
               var res =await conn.QueryAsync<T>(sql);
               return res;
           }
       }
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static async Task<PagedResultDto<T>> GetPagedResult<T>(string where="", int skip=0, int take=10) where T : class, new()
       {
           var name = nameof(T);
           var sql = $"select  * from {name}  {where} order by id  limit {skip},{take}";
           var countsql = $"select count(1) from {name}  {where}";
            using (MySqlConnection conn = new MySqlConnection(Host))
           {
               var res = await conn.QueryAsync<T>(sql);
               var count = await conn.QueryFirst(countsql);
               return new PagedResultDto<T>(count,res.ToList());
           }
        }
   }
}
