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
    /// 廠商基本資料
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustController : ApiController
    {
        private readonly OracleConnectionFactory _conn;
        private CustController()
        {
            _conn = new OracleConnectionFactory();
        }

        /// <summary>
        /// 取得特定廠商資料
        /// </summary>
        /// <param name="id">廠商統一編號</param>
        /// <returns>廠商資料</returns>
        [HttpGet]
        [Route("Cust/GetCustById")]
        public IHttpActionResult Get(string id)
        {
            using (var cn = _conn.CreateConnection("PL"))
            {
                string sql = @"SELECT * FROM EM.T_CUST_BASI WHERE CUST_UNIF_NO = :CUST_UNIF_NO";
                var result = cn.Query(sql, new { CUST_UNIF_NO = id });
                return Json(JArray.FromObject(result));
            }
        }

    }
}
