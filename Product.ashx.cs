using System.Web;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.SessionState;
using System.IO;
using System;

namespace ykx
{
    /// <summary>
    /// Student 的摘要说明
    /// </summary>
    public class Product : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            if (context.Session["Admin"] == null || context.Session["Admin"].ToString() == "")
            {
                context.Response.Redirect("/html/adminLogin.html");
            }
            string action = context.Request["Action"];
            string adminName = context.Session["Admin"].ToString();
            string adminId = "";
            DataTable dts =
            SqlHelper.ExecuteDataTable("select *  from Admin where Account = " + adminName);
            foreach (DataRow dr in dts.Rows)
            {
                adminId = dr["Admin_ID"].ToString();
            }
            //查询学生
            if (action == "Search")
            {
                string requestNum = context.Request["PageNumber"];
                string html = FenYe.FY("Product.ashx", "product.html", "V_P", requestNum, "Product_ID", adminId);
                context.Response.Write(html);
            }
            else if (action == "add")
            {

                DataTable dt =
                SqlHelper.ExecuteDataTable("select *  from Category where IS_DELETE = 0 and Admin_ID = "+adminId+"");
                string html = CommonHelper.RenderHtml("addProduct.html", dt.Rows);
                context.Response.Write(html);
            }
            else if (action == "Add")
            {
                HttpServerUtility server = context.Server;
                HttpRequest request = context.Request;
                HttpResponse response = context.Response;

                HttpPostedFile file = context.Request.Files[0];
                string fullName = "";
                if (file.ContentLength > 0)
                {
                    string extName = Path.GetExtension(file.FileName);
                    string fileName = Guid.NewGuid().ToString();
                    fullName = fileName + extName;

                    string imageFilter = ".jpg|.png|.gif|.ico";// 随便模拟几个图片类型
                    if (imageFilter.Contains(extName.ToLower()))
                    {
                        string phyFilePath = server.MapPath("~/Upload/Image/") + fullName;
                        file.SaveAs(phyFilePath);
                        //response.Write("上传成功！文件名：" + fullName + "<br />");
                        //response.Write(string.Format("<img src='Upload/Image/{0}'/>", fullName));
                    }
                }
                string P_Name = context.Request["P_Name"];
                string Category_ID = context.Request["Category_ID"];
                string Availability = context.Request["Availability"];
                string P_Price = context.Request["P_Price"];
                string Model = context.Request["Model"];
                string Manufacturer = context.Request["Manufacturer"];
                string Quantity = context.Request["Quantity"];
                string P_Detail = context.Request["P_Detail"];
                string IS_TJ = context.Request["IS_TJ"];
                string P_Pic = string.Format("http://localhost:54436/Upload/Image/{0}", fullName);
                int IS_DELETE = 0;
                SqlHelper.ExecuteNonQuery("Insert into Product(Category_ID,"+
                 "Admin_ID,P_Name,P_Price,P_Pic,Availability,Model,Manufacturer,"+
                 "P_Detail,IS_DELETE,Quantity,IS_TJ) values(@Category_ID,"+
                 "@Admin_ID,@P_Name,@P_Price,@P_Pic,@Availability,@Model,"+
                 "@Manufacturer,@P_Detail,@IS_DELETE,@Quantity,@IS_TJ)",
                        new SqlParameter("@Category_ID", Category_ID)
                        , new SqlParameter("@Admin_ID", adminId)
                        , new SqlParameter("@P_Name", P_Name)
                        , new SqlParameter("@P_Price", P_Price)
                        , new SqlParameter("@P_Pic", P_Pic)
                        , new SqlParameter("@Availability", Availability)
                        , new SqlParameter("@Model", Model)
                        , new SqlParameter("@Manufacturer", Manufacturer)
                        , new SqlParameter("@P_Detail", P_Detail)
                        , new SqlParameter("@IS_DELETE", IS_DELETE)
                        , new SqlParameter("@Quantity", Quantity)
                        , new SqlParameter("@IS_TJ", IS_TJ)
                        );
                context.Response.Redirect("http://localhost:54436/product.ashx?Action=Search");
            }
            else if (action == "Edit")
            {
                string id = context.Request["Id"];
                DataTable dt =
                SqlHelper.ExecuteDataTable("select *  from V_P where Product_ID = " + id);
                DataTable dt2 =
                SqlHelper.ExecuteDataTable("select *  from Category where IS_DELETE = 0 and Admin_ID = "+ adminId);
                var Data = new {Product = dt.Rows,Category = dt2.Rows};
                string html = CommonHelper.RenderHtml("editProduct.html", Data);
                context.Response.Write(html);
            }
            else if (action == "edit")
            {
                HttpServerUtility server = context.Server;
                HttpRequest request = context.Request;
                HttpResponse response = context.Response;

                HttpPostedFile file = context.Request.Files[0];
                string fullName = "";
                string P_Pic = "";
                if (file.ContentLength > 0)
                {
                    string extName = Path.GetExtension(file.FileName);
                    string fileName = Guid.NewGuid().ToString();
                    fullName = fileName + extName;

                    string imageFilter = ".jpg|.png|.gif|.ico";// 随便模拟几个图片类型
                    if (imageFilter.Contains(extName.ToLower()))
                    {
                        string phyFilePath = server.MapPath("~/Upload/Image/") + fullName;
                        file.SaveAs(phyFilePath);
                        P_Pic = string.Format("http://localhost:54436/Upload/Image/{0}", fullName);
                        //response.Write("上传成功！文件名：" + fullName + "<br />");
                        //response.Write(string.Format("<img src='Upload/Image/{0}'/>", fullName));
                    }
                }else
                {
                    string id = context.Request["Product_ID"];
                    DataTable dt =
                SqlHelper.ExecuteDataTable("select *  from V_P where Product_ID = " + id);
                    foreach (DataRow dr in dt.Rows)
                    {
                        P_Pic = dr["P_Pic"].ToString();
                    }
                }
                string Product_ID = context.Request["Product_ID"];
                string P_Name = context.Request["P_Name"];
                string Category_ID = context.Request["Category_ID"];
                string Availability = context.Request["Availability"];
                string P_Price = context.Request["P_Price"];
                string Model = context.Request["Model"];
                string Manufacturer = context.Request["Manufacturer"];
                string Quantity = context.Request["Quantity"];
                string P_Detail = context.Request["P_Detail"];
                string IS_TJ = context.Request["IS_TJ"];
                int IS_DELETE = 0;
                SqlHelper.ExecuteNonQuery("update Product set Category_ID=@Category_ID," +
                 "Admin_ID=@Admin_ID,P_Name=@P_Name,P_Price=@P_Price,P_Pic=@P_Pic"+
                 ",Availability=@Availability,Model=@Model,Manufacturer=@Manufacturer," +
                 "P_Detail=@P_Detail,IS_DELETE=@IS_DELETE,Quantity=@Quantity,IS_TJ=@IS_TJ "+
                 " where Product_ID=@Product_ID",
                        new SqlParameter("@Category_ID", Category_ID)
                        , new SqlParameter("@Admin_ID", adminId)
                        , new SqlParameter("@P_Name", P_Name)
                        , new SqlParameter("@P_Price", P_Price)
                        , new SqlParameter("@P_Pic", P_Pic)
                        , new SqlParameter("@Availability", Availability)
                        , new SqlParameter("@Model", Model)
                        , new SqlParameter("@Manufacturer", Manufacturer)
                        , new SqlParameter("@P_Detail", P_Detail)
                        , new SqlParameter("@IS_DELETE", IS_DELETE)
                        , new SqlParameter("@Quantity", Quantity)
                        , new SqlParameter("@IS_TJ", IS_TJ)
                        , new SqlParameter("@Product_ID", Product_ID)
                        );
                context.Response.Redirect("http://localhost:54436/product.ashx?Action=Search");
            }
            else if (action == "Delete")
            {
                string id = context.Request["Id"];
                int is_delete = 1;
                SqlHelper.ExecuteNonQuery("update Product set IS_DELETE=@IS_DELETE where Product_ID=@Product_ID",
                         new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@Product_ID", id)
                        );
                context.Response.Redirect("http://localhost:54436/product.ashx?Action=Search");
            }
            else
            {
                context.Response.Redirect("http://localhost:54436/product.ashx?Action=Search");
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