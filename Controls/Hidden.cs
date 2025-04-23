using System;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix ("Tittle.Controls" , "Tittle") ]
namespace Tittle.Controls
{
	/// <summary>
	/// Summary description for Hidden.
	/// 
	/// I prefer to use TextBox instead of HtmlInputHidden because I can use it the way I use text box then (i.e. hdn.Text NOT hdn.Value), and 
	/// making visibility on here I can see all hidden textbox values on all the pages while debugging.
	/// </summary>
	public class Hidden : TextBox
	{
		public Hidden() : base()
		{
			this.Style.Add("display","none");
		}		
	}
}
