using System.Web;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.SessionState;
namespace ykx
{
    /// <summary>
    /// Register 的摘要说明
    /// </summary>
    public class Register : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string U_Name = context.Request["U_Name"];
            string U_ACCOUNT = context.Request["U_ACCOUNT"];
            string U_PSW = context.Request["U_PSW"];
            string U_Phone = context.Request["U_Phone"];
            string U_Email = context.Request["U_Email"];
            int IS_DELETE = 0;
            SqlHelper.ExecuteNonQuery("Insert into HY(U_Name,U_ACCOUNT,"+
            "U_PSW,U_Phone,U_Email,IS_DELETE) values(@U_Name,@U_ACCOUNT,"+
            "@U_PSW,@U_Phone,@U_Email,@IS_DELETE)",
                    new SqlParameter("@U_Name", U_Name)
                    , new SqlParameter("@U_ACCOUNT", U_ACCOUNT)
                    , new SqlParameter("@U_PSW", U_PSW)
                    , new SqlParameter("@U_Phone", U_Phone)
                    , new SqlParameter("@U_Email", U_Email)
                    , new SqlParameter("@IS_DELETE", IS_DELETE)
                    );
            context.Response.Redirect("http://localhost:54436/student.ashx?Action=Search");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}