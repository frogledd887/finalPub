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
    /// EIP應用系統清單
    /// </summary>
    
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EipController : ApiController
    {
        private readonly OracleConnectionFactory _conn;
        private EipController()
        {
            _conn = new OracleConnectionFactory();
        }

        /// <summary>
        /// 取得所有 EIP 應用系統清單
        /// </summary>
        /// <returns>EIP 應用系統清單</returns>
        [HttpGet]
        [Route("Eip/GetApplst")]
        public IHttpActionResult Get()
        {
            using (var cn = _conn.CreateConnection("PL"))
            {
                string sql = $@"SELECT  A.APPL_ID, A.APPL_NAME, A.DEVP_ID, A.DEVP_NAME, B.E_MAIL_ADDR, B.OFFI_TELE_NMBR_EXTE
                                       FROM  PL.V_EIP_APPL_LIST A, PP.V_EMPL_OPEN B 
                                       WHERE (A.DEVP_ID = B.EMPL_SERI_NMBR) AND (A.FG_WEBF_LIST = 'Y') AND (A.FG_WEBF_LIST = 'Y') AND (INSTR(APPL_ID, '_') = 0) AND (INSTR(APPL_NAME, '測') = 0)
                                       ORDER BY A.APPL_ID";
                //AND(A.FG_WEBF_LIST = 'Y') AND(NOT(A.OPEN_CODE IS NULL))
                var result = cn.Query(sql);
                return Json(JArray.FromObject(result));
            }
        }

        /// <summary>
        /// 依據「系統代號」取得 EIP 應用系統清單
        /// </summary>
        /// <param name="id">系統代號</param>
        /// <returns>EIP 應用系統清單</returns>
        [HttpGet]
        [Route("Eip/GetApplstById")]
        public IHttpActionResult Get(string id)
        {
            using (var cn = _conn.CreateConnection("PL"))
            {
                string sql = $@"SELECT  A.APPL_ID, A.APPL_NAME, A.DEVP_ID, A.DEVP_NAME, B.E_MAIL_ADDR, B.OFFI_TELE_NMBR_EXTE
                                       FROM  PL.V_EIP_APPL_LIST A, PP.V_EMPL_OPEN B 
                                       WHERE (A.DEVP_ID = B.EMPL_SERI_NMBR) AND (A.FG_WEBF_LIST = 'Y') AND (A.APPL_ID = :APPL_ID)";
                var result = cn.Query(sql, new { APPL_ID = id.ToUpper() });
                return Json(JArray.FromObject(result));
            }
        }
    }
}
