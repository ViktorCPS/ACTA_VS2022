using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Drawing;

namespace System.Drawing.PieChart.WebControl
{
    [ToolboxData("<{0}:PieChart runat=server />")]
    public class PieChart3D : System.Web.UI.UserControl
    {
        protected System.Web.UI.HtmlControls.HtmlGenericControl m_Mapper;
        protected System.Web.UI.HtmlControls.HtmlImage m_ChartImg;

        private int m_Width = 400;
        private int m_Height = 200;

        private decimal[] m_Values;
        private string[] m_Texts;
        private string[] m_Links;
        private string[] m_Colors;
        private float[] m_SliceDisplacements;

        private int m_Alpha = 160;
        private ShadowStyle m_ShadowStyle = ShadowStyle.GradualShadow;
        private EdgeColorType m_EdgeColorType = EdgeColorType.DarkerThanSurface;

        private string m_FontFamily = "Verdana";
        private float m_FontSize = 10;
        private Drawing.FontStyle m_FontStyle = Drawing.FontStyle.Regular;
        private string m_ForeColor = "Black";

        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        public string Values
        {
            get
            {
                return string.Join(",", Array.ConvertAll<Decimal, String>(m_Values,
                new Converter<Decimal, String>(Convert.ToString)));
            }
            set
            {
                m_Values = Array.ConvertAll<String, Decimal>(value.Split(",".ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries),
                    new Converter<String, Decimal>(Convert.ToDecimal));
            }
        }

        public string Texts
        {
            get { return String.Join("|", m_Texts); }
            set { m_Texts = value.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries); }
        }

        public string Links
        {
            get { return String.Join(",", m_Links); }
            set { m_Links = value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries); }
        }

        public string Colors
        {
            get { return String.Join(",", m_Colors); }
            set { m_Colors = value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries); }
        }

        public string SliceDisplacments
        {
            get
            {
                return String.Join(",", Array.ConvertAll<Single, String>(m_SliceDisplacements,
                   new Converter<Single, String>(Convert.ToString)));
            }
            set
            {
                m_SliceDisplacements = Array.ConvertAll<String, Single>(value.Split(",".ToCharArray(),
        StringSplitOptions.RemoveEmptyEntries),
        new Converter<String, Single>(Convert.ToSingle));
            }
        }

        public int Opacity
        {
            get { return m_Alpha; }
            set { m_Alpha = value; }
        }

        public ShadowStyle ShadowStyle
        {
            get { return m_ShadowStyle; }
            set { m_ShadowStyle = value; }
        }

        public EdgeColorType EdgeColorType
        {
            get { return m_EdgeColorType; }
            set { m_EdgeColorType = value; }
        }

        public string FontFamily
        {
            get { return m_FontFamily; }
            set { m_FontFamily = value; }
        }

        public float FontSize
        {
            get { return m_FontSize; }
            set { m_FontSize = value; }
        }

        public Drawing.FontStyle FontStyle
        {
            get { return m_FontStyle; }
            set { m_FontStyle = value; }
        }

        public string ForeColor
        {
            get { return m_ForeColor; }
            set { m_ForeColor = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!this.Visible || Values.Length <= 0) return;

            //Init Controls
            m_Mapper = new System.Web.UI.HtmlControls.HtmlGenericControl("map");
            m_ChartImg = new System.Web.UI.HtmlControls.HtmlImage();
            this.Controls.Add(m_Mapper);
            this.Controls.Add(m_ChartImg);

            PieChartControl oPie = new PieChartControl();
            Collections.Generic.Dictionary<string, Pair> oLinksList = new Dictionary<string, Pair>();
            if (m_Texts.Length == m_Links.Length)
            {
                for (int iCnt = 0; iCnt < m_Texts.Length; iCnt++)
                    oLinksList.Add(m_Texts[iCnt], new Pair(m_Links[iCnt], m_Values[iCnt]));
            }

            oPie.ClientSize = new SizeF(Width, Height);
            oPie.Values = m_Values;
            oPie.Texts = m_Texts;

            Collections.Generic.Queue<Color> oColors = new Queue<Color>();
            foreach (string sHtmlColor in m_Colors)
                oColors.Enqueue(Color.FromArgb(Opacity, ColorTranslator.FromHtml(sHtmlColor)));
            oPie.Colors = oColors.ToArray();

            if (m_SliceDisplacements == null)
            {
                m_SliceDisplacements = new float[m_Values.Length];
                for (int iValIndex = 0; iValIndex < m_Values.Length; iValIndex++)
                    m_SliceDisplacements[iValIndex] = 0.05F;
            }

            oPie.SliceRelativeDisplacements = m_SliceDisplacements;
            oPie.ShadowStyle = ShadowStyle;
            oPie.EdgeColorType = EdgeColorType;
            oPie.Font = new Font(FontFamily, FontSize, FontStyle);
            oPie.ForeColor = ColorTranslator.FromHtml(ForeColor);

            System.Drawing.Image oBitmap = oPie.GetChart();
            using (System.IO.MemoryStream oMem = new System.IO.MemoryStream())
            {
                oBitmap.Save(oMem, System.Drawing.Imaging.ImageFormat.Png);
                //if (Page.Request.Browser.IsBrowser("IE"))
                //{
                //    Context.Session[m_ChartImg.ClientID] = oMem;
                //    m_ChartImg.Src = "_getchart.aspx?imgId=" + m_ChartImg.ClientID;
                //}
                //else
                m_ChartImg.Src = "data:image/png;base64," + Convert.ToBase64String(oMem.ToArray(),
                                Base64FormattingOptions.None);
            }

            foreach (PieSlice oSlice in oPie.PieSlices)
            {
                System.Text.StringBuilder oSB = new System.Text.StringBuilder();
                foreach (PointF p in oSlice.ArcPath.PathPoints)
                    oSB.Append(",").Append(p.X).Append(",").Append(p.Y);

                System.Web.UI.HtmlControls.HtmlGenericControl oArea =
                    new System.Web.UI.HtmlControls.HtmlGenericControl("area");
                oArea.Attributes.Add("coords", oSB.ToString().Substring(1));
                // oArea.Attributes.Add("href", oLinksList[oSlice.Text].First.ToString());
                oArea.Attributes.Add("title", oSlice.Text + " (" +
                    oLinksList[oSlice.Text].Second + "%)");
                oArea.Attributes.Add("shape", "poly");
                m_Mapper.Controls.Add(oArea);
            }

            m_Mapper.Attributes.Add("name", m_Mapper.ClientID);
            m_ChartImg.Attributes.Add("usemap", "#" + m_Mapper.ClientID);
            m_ChartImg.Attributes.Add("border", "0");
        }
    }
}
