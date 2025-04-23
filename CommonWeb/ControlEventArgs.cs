using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace CommonWeb
{
    public class ControlEventArgs : EventArgs
    {
        private string error = "";

        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        public ControlEventArgs()
            : base()
        { }

        public ControlEventArgs(string error)
            : base()
        {
            this.Error = error;
        }
    }

    public delegate void ControlEventHandler(object sender, ControlEventArgs e);
}
