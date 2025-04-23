using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.Caching;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections.Specialized; 
using System.Web.UI.Design;
using System.ComponentModel;

[assembly: TagPrefix ("Tittle.Controls" , "Tittle") ]
namespace Tittle.Controls
{
	/// <summary>	
	/// Notes Tooltip by Tittle Joseph
	/// http://www.codeproject.com/script/articles/list_articles.asp?userid=1654009
	/// tooltip with the help of wz_tooltip.js supplied by http://www.walterzorn.com
	/// </summary>
	[Designer(typeof(NotesDesigner))]
	public class Notes : WebControl, INamingContainer, IPostBackDataHandler, IPostBackEventHandler 
	{																
		#region Private variables
		private string text;				
		private string displayText="";
		private string enableImage = "notes.gif";
		private string disableImage = "notesblank.gif";
		
		#region Notes Property
		private bool sticky=false;
		private string notesWidth="300";
		private bool above=false;
		private string bgColor="#e6ecff";
		private string bgImg="";
		private int borderWidth=1;
		private string borderColor="#003399";
		private int delay=500;
		private string fix="";
		private string fontColor="#000066";
		private string fontFace="arial,helvetica,sans-serif";
		private string fontSize="11px";
		private string fontWeight="normal";
		private bool left=false;
		private int offSetX=12;
		private int offSetY=15;
		private int opaCity=100;
		private int padding=3;
		private string shadowColor="";
		private int shadowWidth=0;
		private bool tstatic=true;
		private int temp=0;
		private string textAlign="left";
		private string title="";
		private string titleColor="#ffffff";
		#endregion
		
		#region Notes attribute check if set then only render them, otherwise let pick default from wz_tooltip.js
		private bool isStickySet=false;
		private bool isNotesWidthSet=false;
		private bool isAboveSet=false;
		private bool isBgColorSet=false;
		private bool isBgImgSet=false;
		private bool isBorderWidthSet=false;
		private bool isBorderColorSet=false;
		private bool isDelaySet=false;
		private bool isFixSet=false;
		private bool isFontColorSet=false;
		private bool isFontFaceSet=false;
		private bool isFontSizeSet=false;
		private bool isFontWeightSet=false;
		private bool isLeftSet=false;
		private bool isOffSetXSet=false;
		private bool isOffSetYSet=false;
		private bool isOpaCitySet=false;
		private bool isPaddingSet=false;
		private bool isShadowColorSet=false;
		private bool isShadowWidthSet=false;
		private bool isTstaticSet=false;
		private bool isTempSet=false;
		private bool isTextAlignSet=false;
		private bool isTitleSet=false;
		private bool isTitleColorSet=false;
		#endregion

		#endregion
		
		#region Constructor
		public Notes()
		{
		}
		#endregion
		
		#region Exposed Attributes
		/// <summary>
		/// Tooltip text
		/// </summary>
		public string Text
		{
			get { return text; }
			set { text = value; }
		}

		/// <summary>
		/// Display Text instead of image icon.
		/// </summary>
		public string DisplayText
		{
			get { return displayText; }
			set { displayText = value; }
		}
		
		/// <summary>
		/// Image to display when tooltip text has some data.
		/// 
		/// If DisplayText is not empty, this is ignored.
		/// 
		/// Default: notes.gif
		/// </summary>
		public string EnableImage
		{
			get { return enableImage; }
			set { enableImage = value; }
		}
		/// <summary>
		/// Image to display when tooltip text is empty.
		/// 
		/// If DisplayText is not empty, this is ignored.
		/// 
		/// Default: notesblank.gif
		/// </summary>
		public string DisableImage
		{
			get { return disableImage; }
			set { disableImage = value; }
		}
		/// <summary>
		/// The tooltip stays fixed on its initial position until another tooltip is activated, or the user clicks on the document. Value: true. To enforce the tooltip to disappear after a certain time span, however, you might additionally apply the this.Temp command. 
		/// 
		/// Default: false
		/// </summary>
		public bool Sticky
		{
			get { return sticky; }
			set 
			{ 
				sticky = value; 
				isStickySet = true;
			}
		}

		/// <summary>
		/// Notes Tooltip Width
		/// 
		/// Default: 300
		/// </summary>
		public string NotesWidth
		{
			get { return notesWidth; }
			set 
			{ 
				notesWidth = value; 
				isNotesWidthSet = true;
			}
		}

		/// <summary>
		/// Places the tooltip above the mousepointer
		/// 
		/// Additionally applying the this.OffSetY command allows to set the vertical distance from the mousepointer
		/// 
		/// Default: false
		/// </summary>
		public bool Above
		{
			get { return above; }
			set 
			{ 
				above = value; 
				isAboveSet = true;
			}
		}

		/// <summary>
		/// Background color of the tooltip.
		/// 
		/// Default: #e6ecff
		/// </summary>
		public string BgColor
		{
			get { return bgColor; }
			set 
			{ 
				bgColor = value; 
				isBgColorSet = true;
			}
		}

		/// <summary>
		/// Background image.
		/// 
		/// Default: Empty
		/// </summary>
		public string BgImg
		{
			get { return bgImg; }
			set 
			{ 
				bgImg = value; 
				isBgImgSet = true;
			}
		}

		/// <summary>
		/// Width of tooltip border.
		/// 
		/// Default: 1
		/// </summary>
		public int TBorderWidth
		{
			get { return borderWidth; }
			set 
			{ 
				borderWidth = value; 
				isBorderWidthSet = true;
			}			
		}

		/// <summary>
		/// Border color.
		/// 
		/// Default: #003399
		/// </summary>
		public string TBorderColor
		{
			get { return borderColor; }
			set 
			{ 
				borderColor = value; 
				isBorderColorSet = true;
			}
		}

		/// <summary>
		/// Tooltip shows up after the specified timeout (milliseconds). A behavior similar to that of OS based tooltips. 
		/// 
		/// Default: 500
		/// </summary>
		public int Delay
		{
			get { return delay; }
			set 
			{ 
				delay = value; 
				isDelaySet = true;
			}
		}

		/// <summary>
		/// Fixes the tooltip to the co-ordinates specified within the square brackets. Useful, for example, if combined with the this.Sticky command. 
		/// 
		/// E.g. [200, 400]
		/// Default: Empty
		/// </summary>
		public string Fix
		{
			get { return fix; }
			set 
			{ 
				fix = value; 
				isFixSet = true;
			}
		}

		/// <summary>
		/// Font Color.
		/// 
		/// Default: #000066
		/// </summary>
		public string FontColor
		{
			get { return fontColor; }
			set 
			{ 
				fontColor = value; 
				isFontColorSet = true;
			}
		}

		/// <summary>
		/// Font face/family.
		/// 
		/// Default: arial,helvetica,sans-serif
		/// </summary>
		public string FontFace
		{
			get { return fontFace; }
			set 
			{ 
				fontFace = value; 
				isFontFaceSet = true;
			}
		}

		/// <summary>
		/// Font size + unit.
		/// 
		/// Default: 11px
		/// </summary>
		public string FontSize
		{
			get { return fontSize; }
			set 
			{ 
				fontSize = value; 
				isFontSizeSet = true;
			}
		}

		/// <summary>
		/// Font Weight. normal or bold.
		/// 
		/// Default: normal
		/// </summary>
		public string FontWeight
		{
			get { return fontWeight; }
			set 
			{ 
				fontWeight = value; 
				isFontWeightSet = true;
			}
		}

		/// <summary>
		/// Tooltip positioned on the left side of the mousepointer.
		/// 
		/// Default: false
		/// </summary>
		public bool Left
		{
			get { return left; }
			set 
			{ 
				left = value; 
				isLeftSet = true;
			}
		}

		/// <summary>
		/// Horizontal offset from mouse-pointer
		/// 
		/// Default: 12
		/// </summary>
		public int OffSetX
		{
			get { return offSetX; }
			set 
			{ 
				offSetX = value; 
				isOffSetXSet = true;
			}
		}

		/// <summary>
		/// Vertical offset from mouse-pointer
		/// 
		/// Default: 15
		/// </summary>
		public int OffSetY
		{
			get { return offSetY; }
			set 
			{ 
				offSetY = value; 
				isOffSetYSet = true;
			}
		}

		/// <summary>
		/// Transparency of tooltip. Opacity is the opposite of transparency. Value must be a number between 0 (fully transparent) and 100 (opaque, no transparency). Not (yet) supported by Opera.
		/// 
		/// Default: 100
		/// </summary>
		public int OpaCity
		{
			get { return opaCity; }
			set 
			{ 
				opaCity = value; 
				isOpaCitySet = true;
			}
		}

		/// <summary>
		/// Inner spacing, i.e. the spacing between border and content, for instance text or image(s). 
		/// 
		/// Default: 3
		/// </summary>
		public int Padding
		{
			get { return padding; }
			set 
			{ 
				padding = value; 
				isPaddingSet = true;
			}
		}

		/// <summary>
		/// Creates shadow with the specified color.
		/// 
		/// Default: Empty
		/// </summary>
		public string ShadowColor
		{
			get { return shadowColor; }
			set 
			{ 
				shadowColor = value; 
				isShadowColorSet = true;
			}
		}

		/// <summary>
		/// Creates shadow with the specified width (offset). Shadow color is automatically set to '#cccccc' (light grey)
		/// 
		/// Default: 0
		/// </summary>
		public int ShadowWidth
		{
			get { return shadowWidth; }
			set 
			{ 
				shadowWidth = value; 
				isShadowWidthSet = true;
			}
		}

		/// <summary>
		/// Like OS-based tooltips, the tooltip doesn't follow the movements of the mouse-pointer.
		/// 
		/// Default: true
		/// </summary>
		public bool Tstatic
		{
			get { return tstatic; }
			set 
			{ 
				tstatic = value; 
				isTstaticSet = true;			
			}
		}

		/// <summary>
		/// Specifies a time span in milliseconds after which the tooltip disappears, even if the mousepointer is still on the concerned HTML element, or if the this.T_STICKY command has been applied. Values less than or equal to 0 make the tooltip behave normally as if no time span had been specified. 
		/// 
		/// Default: 0
		/// </summary>
		public int Temp
		{
			get { return temp; }
			set 
			{ 
				temp = value; 
				isTempSet = true;
			}
		}

		/// <summary>
		/// Aligns the text of both the title and the body of the tooltip. Values must be included in single quotes and can be either 'right', 'justify' or 'left', the latter being unnecessary since it is the preset default value.
		/// 
		/// Default: left
		/// </summary>
		public string TextAlign
		{
			get { return textAlign; }
			set 
			{ 
				textAlign = value; 
				isTextAlignSet = true;			
			}
		}

		/// <summary>
		/// Title. Text in single quotes. Background color is automatically the same as the border color. 
		/// 
		/// Default: Empty
		/// </summary>
		public string Title
		{
			get { return title; }
			set 
			{ 
				title = value; 
				isTitleSet = true;
			}
		}

		/// <summary>
		/// Color of title text. 
		/// 
		/// Default: #ffffff
		/// </summary>
		public string TitleColor
		{
			get { return titleColor; }
			set 
			{ 
				titleColor = value; 
				isTitleColorSet = true;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Onload event
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if (Page != null)
			{
				// Register the control with the page's postback mechanism, or
				// the control will not update on Postback
				/* Page.RegisterRequiresPostBack(this);				 */
			}

			if ( !Page.IsPostBack )
			{
				//Create child controls
				EnsureChildControls();
			}			
		}		
		
		/// <summary>
		/// Create Child Controls
		/// </summary>
		protected override void CreateChildControls() 
		{			
			TextBox txtNotes = new TextBox();
			txtNotes.ID = "txtNotes";
			txtNotes.Text = this.text;
			txtNotes.Style.Add("display","none");			

			HyperLink hyper = new HyperLink();
			
			hyper.NavigateUrl = "javascript:void(0)";
			if ( text != "" )
			{				
				#region Attach Tooltip
				string onOverScript="";
				onOverScript += (isStickySet==true)?"this.T_STICKY=1;":"";
				onOverScript += (isNotesWidthSet==true)?"this.T_WIDTH=" + this.notesWidth + ";":"";
				onOverScript += (isShadowWidthSet==true)?"this.T_SHADOWWIDTH=" + this.shadowWidth.ToString() + ";":"";
				onOverScript += (isAboveSet==true)?"this.T_ABOVE=" + this.above.ToString().ToLower() + ";":"";
				onOverScript += (isBgColorSet==true)?"this.T_BGCOLOR='" + this.bgColor + "';":"";
				onOverScript += (isBgImgSet==true)?"this.T_BGIMG='" + this.bgImg + "';":"";
				onOverScript += (isBorderWidthSet==true)?"this.T_BORDERWIDTH=" + this.borderWidth.ToString() + ";":"";
				onOverScript += (isBorderColorSet==true)?"this.T_BORDERCOLOR='" + this.borderColor + "';":"";
				onOverScript += (isDelaySet==true)?"this.T_DELAY=" + this.delay.ToString() + ";":"";
				onOverScript += (isFixSet==true)?"this.T_FIX=" + this.fix + ";":"";
				onOverScript += (isFontColorSet==true)?"this.T_FONTCOLOR='" + this.fontColor + "';":"";
				onOverScript += (isFontFaceSet==true)?"this.T_FONTFACE='" + this.fontFace + "';":"";
				onOverScript += (isFontSizeSet==true)?"this.T_FONTSIZE='" + this.fontSize + "';":"";
				onOverScript += (isFontWeightSet==true)?"this.T_FONTWEIGHT='" + this.fontWeight + "';":"";
				onOverScript += (isLeftSet==true)?"this.T_LEFT=" + this.left.ToString().ToLower() + ";":"";
				onOverScript += (isOffSetXSet==true)?"this.T_OFFSETX=" + this.offSetX.ToString() + ";":"";
				onOverScript += (isOffSetYSet==true)?"this.T_OFFSETY=" + this.offSetY.ToString() + ";":"";
				onOverScript += (isOpaCitySet==true)?"this.T_OPACITY=" + this.opaCity.ToString() + ";":"";
				onOverScript += (isPaddingSet==true)?"this.T_PADDING=" + this.padding.ToString() + ";":"";
				onOverScript += (isShadowColorSet==true)?"this.T_SHADOWCOLOR='" + this.shadowColor + "';":"";
				onOverScript += (isShadowWidthSet==true)?"this.T_SHADOWWIDTH=" + this.shadowWidth.ToString() + ";":"";
				onOverScript += (isTstaticSet==true)?"this.T_STATIC=" + this.tstatic.ToString().ToLower() + ";":"";
				onOverScript += (isTempSet==true)?"this.T_TEMP=" + this.temp.ToString() + ";":"";
				onOverScript += (isTextAlignSet==true)?"this.T_TEXTALIGN='" + this.textAlign + "';":"";
				onOverScript += (isTitleSet==true)?"this.T_TITLE='" + this.title + "';":"";
				onOverScript += (isTitleColorSet==true)?"this.T_TITLECOLOR='" + this.titleColor + "';":"";

				hyper.Attributes.Add("onmouseover",onOverScript+"return escape(this.previousSibling.value)");
				#endregion
			}
			
			if ( displayText != "" )
			{
				Literal lit = new Literal();
				lit.Text = displayText;
				hyper.Controls.Add(lit);
			}
			else
			{			
				Image img = new Image();
				img.ID = "imgNotes";			
				img.BorderWidth = 0;
				if ( text != "" )
					img.ImageUrl = Config.ImagePath + enableImage;
				else
					img.ImageUrl = Config.ImagePath + disableImage;

				hyper.Controls.Add(img);
			}

			this.Controls.Add(txtNotes);
			this.Controls.Add(hyper);
		}		
				
		/// <summary>
		/// Load Post Data
		/// </summary>
		/// <param name="postDatakey">notes</param>
		/// <param name="postCollection"></param>
		/// <returns></returns>
		public bool LoadPostData(string postDatakey, NameValueCollection postCollection)
		{
			text = postCollection[postDatakey + ":txtNotes"];

			return true;
		}				
	
		/// <summary>
		/// Need to be implemented when using interface IPostBack..
		/// </summary>
		public void RaisePostDataChangedEvent()
		{
		}

		/// <summary>
		/// Based on Add Button clicked or Remove Icon clicked some series of steps are run here.
		/// </summary>
		/// <param name="argument"></param>
		public void RaisePostBackEvent(string argument)
		{						
		}

		/// <summary>
		/// Print tooltip .JS file here.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{			
			//As per the guidelines given by wztooltip.js, js file must be included after all the tooltip used.
			if ( !Page.ClientScript.IsStartupScriptRegistered("wztooltip") )
			{
                Page.ClientScript.RegisterStartupScript(GetType(), "wztooltip", "<script language=javascript src='" + Config.JSPath + "wz_tooltip.js' type='text/javascript'></script>");
			}			
		}
		#endregion
	};	      	

	#region Notes in Design View
	public sealed class NotesDesigner : ControlDesigner
	{		
		public override string GetDesignTimeHtml()
		{
			try
			{				
				Notes notes = (Notes)Component;
				
				return "<img src='" + Config.ImagePath + notes.EnableImage +"' />";
			}
			catch(Exception ex)
			{
				return GetErrorDesignTimeHtml(ex);
			}
		}

		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return CreatePlaceHolderDesignTimeHtml(e.Message);
		}
	}
	#endregion
}