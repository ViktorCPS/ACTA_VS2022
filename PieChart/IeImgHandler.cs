using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace WebControl
{
    public class IeImgHandler : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(System.Web.HttpContext context)
        {
            context.Response.Clear();

            //context.Response.Cache.SetExpires(DateTime.Now.AddDays(1));
            //context.Response.Cache.SetValidUntilExpires(true);
            //context.Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
            //context.Response.Cache.AppendCacheExtension("post-check=900, pre-check=3600");
            context.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            context.Response.ContentType = "image/png";
            MemoryStream memStream = (MemoryStream)System.Web.HttpContext.Current.Session[context.Request["imgId"]];
           
            
            
           context.Response.BinaryWrite(memStream.ToArray());
        }

        #endregion
    }
}
