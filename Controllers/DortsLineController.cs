using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using PubApi.Models;
using PubApi.Connection;
using Dapper;
using Newtonsoft.Json.Linq;
using System.Web.Http.Cors;

namespace PubApi.Controllers
{
    /// <summary>
    /// 捷運線別清單
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DortsLineController : ApiController
    {
        private readonly OracleConnectionFactory _conn;
        private DortsLineController()
        {
            _conn = new OracleConnectionFactory();
        }

        /// <summary>
        /// 取得所有捷運線別清單
        /// </summary>
        /// <returns>捷運線別清單</returns>
        // PubApi/Pub/JobList
        [HttpGet]
        [Route("DortsLine/GetLine")]
        public IEnumerable<LINE_NAME> Get()
        {
            var cn = _conn.CreateConnection("public");
            string sql = $@"SELECT * FROM PC.T_LINE_NAME
                                ORDER BY LINE_CD";
            var linelist = cn.Query<LINE_NAME>(sql).ToList();
            return linelist;
        }

        /// <summary>
        /// 依據「線別代碼」取得捷運線別清單
        /// </summary>
        /// <param name="id">線別代碼</param>
        /// <returns>捷運線別資料列</returns>
        // PubApi/Pub/Line/Q
        [HttpGet]
        [Route("DortsLine/GetLineById")]
        public IEnumerable<LINE_NAME> Get(string id)
        {
            var cn = _conn.CreateConnection("public");
            string sql = $@"SELECT * FROM PC.T_LINE_NAME 
                                WHERE LINE_CD = :LINE_CD";
            var linelist = cn.Query<LINE_NAME>(sql, new { LINE_CD = id.ToUpper() }).ToList();
            return linelist;
        }

    }
}
