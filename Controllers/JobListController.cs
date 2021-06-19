using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using PubApi.Models;
using PubApi.Connection;
using Dapper;
using Newtonsoft.Json.Linq;
using System.Web.Http.Cors;

namespace PubApi.Controllers
{
    /// <summary>
    /// 工作代號清單
    /// </summary>

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class JobListController : ApiController
    {
        private readonly OracleConnectionFactory _conn;
        private JobListController()
        {
            _conn = new OracleConnectionFactory();
        }

        /// <summary>
        /// 取得所有工作代號清單
        /// </summary>
        /// <returns>工作代號清單</returns>
        [HttpGet]
        [Route("JobList")]
        public IEnumerable<JOB_NO_LIST> Get()
        {
            var cn = _conn.CreateConnection("public");
            string sql = @"SELECT * FROM PC.JOB_NO_LIST A,PC.T_LINE_NAME B WHERE A.LINE_ID=B.LINE_CD
                                    ORDER BY A.PROJ_ID";
            var joblist = cn.Query<JOB_NO_LIST>(sql).ToList();
            return joblist;
        }

        /// <summary>
        /// 依據「工作代號(或工程標號)」取得專案項目內容
        /// </summary>
        /// <param name="id">工作代號(工程標號)</param>
        /// <returns>工作項目內容</returns>
        [HttpGet]
        [Route("JobList/ProjbyId")]
        public IEnumerable<JOB_NO_LIST> ProjbyId(string id)
        {
            var cn = _conn.CreateConnection("public");
            string sql = @"SELECT * FROM PC.JOB_NO_LIST A,PC.T_LINE_NAME B 
                                WHERE A.LINE_ID=B.LINE_CD AND A.PROJ_ID = :PROJ_ID
                                ORDER BY A.LINE_ID, A.PROJ_ID";
            var joblist = cn.Query<JOB_NO_LIST>(sql, new { PROJ_ID = id.ToUpper() }).ToList();
            return joblist;
        }

        /// <summary>
        /// 依據「線別代碼」取得工作項目
        /// </summary>
        /// <param name="id">線別代碼</param>
        /// <returns>工作項目清單</returns>
        [HttpGet]
        [Route("JobList/ProjbyLine")]
        public IEnumerable<JOB_NO_LIST> ProjbyLine(string id)
        {
            var cn = _conn.CreateConnection("public");
            string sql = @"SELECT * FROM PC.JOB_NO_LIST A,PC.T_LINE_NAME B 
                                WHERE A.LINE_ID=B.LINE_CD AND A.LINE_ID = :LINE_ID
                                ORDER BY A.PROJ_ID";
            var joblist = cn.Query<JOB_NO_LIST>(sql, new { LINE_ID = id.ToUpper() }).ToList();
            return joblist;
        }
    }
}
