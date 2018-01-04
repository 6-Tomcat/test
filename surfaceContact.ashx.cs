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
    /// surfaceContact 的摘要说明
    /// </summary>
    public class surfaceContact : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            if (context.Session["User"] == null || context.Session["User"].ToString() == "")
            {
                context.Response.Redirect("/html/loginContact.html");
            }
            string User_ID = context.Session["Id"].ToString();
            string action = context.Request["Action"];
            /*
            context.Response.ContentType = "text/html";
            if (context.Session["Admin"] == null || context.Session["Admin"].ToString() == "")
            {
                context.Response.Redirect("/html/adminLogin.html");
            }

            string action = context.Request["Action"];
             */
            /*
            string adminName = context.Session["Admin"].ToString();
            string adminId = "";
            DataTable dts =
            SqlHelper.ExecuteDataTable("select *  from Admin where Account = " + adminName);
            foreach (DataRow dr in dts.Rows)
            {
                adminId = dr["Admin_ID"].ToString();
            }
            */
             //查询学生
            if (action == "Search")
            {
                string requestNum = context.Request["PageNumber"];
                string html = FenYe.FY("Contact.ashx", "searchContact.html", "V_C", requestNum, "Contact_ID", "");
                context.Response.Write(html);
            }
            else if (action == "add")
            {
                string html = CommonHelper.RenderHtml("addContact.html", "");
                context.Response.Write(html);
            }
            else if (action == "Add")
            {
                /*
                 id="User_ID" name="User_ID"
                 id="Admin_ID" name="Admin_ID
                 id="C_Name" name="C_Name"
                 id="C_Email" name="C_Email"
                 id="C_Subject" name="C_Subject"
                 id="Message" name="Message" 
                 */
                string uid = context.Request["User_ID"];
                string aid = context.Request["Admin_ID"];
                string cname = context.Request["C_Name"];
                string cemail = context.Request["C_Email"];
                string csubject = context.Request["C_Subject"];
                string message = context.Request["Message"];

                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("Insert into Contact(User_ID,Admin_ID,C_Name,C_Email,C_Subject,Message,IS_DELETE) values(@User_ID,@Admin_ID,@C_Name,@C_Email,@C_Subject,@Message,@IS_DELETE)",
                         new SqlParameter("@User_ID", uid)
                         , new SqlParameter("@Admin_ID", aid)
                         , new SqlParameter("@C_Name", cname)
                         , new SqlParameter("@C_Email", cemail)
                         , new SqlParameter("@C_Subject", csubject)
                         , new SqlParameter("@Message", message)
                        , new SqlParameter("@IS_DELETE", is_delete)
                        );

                context.Response.Redirect("http://localhost:54436/Contact.ashx?Action=Search");
            }
            else if (action == "Adda")
            {
                /*
                 id="User_ID" name="User_ID"
                 id="Admin_ID" name="Admin_ID
                 id="C_Name" name="C_Name"
                 id="C_Email" name="C_Email"
                 id="C_Subject" name="C_Subject"
                 id="Message" name="Message" 
                 */
                string uid = "1";//context.Request["User_ID"];
                string aid = "1";//context.Request["Admin_ID"];
                string cname = context.Request["author"];
                string cemail = context.Request["email"];
                string csubject = context.Request["subject"];
                string message = context.Request["text"];

                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("Insert into Contact(User_ID,Admin_ID,C_Name,C_Email,C_Subject,Message,IS_DELETE) values(@User_ID,@Admin_ID,@C_Name,@C_Email,@C_Subject,@Message,@IS_DELETE)",
                         new SqlParameter("@User_ID", uid)
                         , new SqlParameter("@Admin_ID", aid)
                         , new SqlParameter("@C_Name", cname)
                         , new SqlParameter("@C_Email", cemail)
                         , new SqlParameter("@C_Subject", csubject)
                         , new SqlParameter("@Message", message)
                        , new SqlParameter("@IS_DELETE", is_delete)
                        );
                context.Response.Redirect("http://localhost:54436/html/contact.html");
                //context.Response.Redirect("http://localhost:54436/Contact.ashx?Action=Search");
            }
            else if (action == "Edit")
            {
                string id = context.Request["Id"];
                DataTable dt =
                SqlHelper.ExecuteDataTable("select *  from Contact where Contact_ID = " + id);
                string html = CommonHelper.RenderHtml("editContact.html", dt.Rows);
                context.Response.Write(html);
            }
            else if (action == "edit")
            {
                string id = context.Request["Contact_ID"];
                string name = context.Request["Name"];
                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("update Category set C_Name=@C_Name,IS_DELETE=@IS_DELETE where Category_ID=@Category_ID"
                        , new SqlParameter("@C_Name", name)
                        , new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@Category_ID", id)
                        );
                context.Response.Redirect("http://localhost:54436/category.ashx?Action=Search");
            }
            else if (action == "Delete")
            {
                string id = context.Request["Id"];
                int is_delete = 1;
                SqlHelper.ExecuteNonQuery("update Category set IS_DELETE=@IS_DELETE where Category_ID=@Category_ID",
                         new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@Category_ID", id)
                        );
                context.Response.Redirect("http://localhost:54436/category.ashx?Action=Search");
            }

            else
            {
                context.Response.Redirect("http://localhost:54436/category.ashx?Action=Search");
            }
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