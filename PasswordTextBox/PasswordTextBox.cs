using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;

namespace opp
{
    public class PasswordTextBox : TextBox
    {
        public PasswordTextBox()
        {
            TextMode = TextBoxMode.Password;
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;

                Attributes["value"] = value;    
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            Attributes["value"] = Text;
        }
    }
}
