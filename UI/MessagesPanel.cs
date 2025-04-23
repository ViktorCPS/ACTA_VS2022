using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

namespace UI
{
    public partial class MessagesPanel : Form
    {
        public string Message = "";

        private CultureInfo culture;
        private ResourceManager rm;
        
        public MessagesPanel(List<string> msgList)
        {
            try
            {
                InitializeComponent();

                culture = CultureInfo.CreateSpecificCulture(Common.NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(MessagesPanel).Assembly);
                
                setLanguage();
                
                InitializePanel(msgList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("MessagesPanel", culture);

                //label's text                
                this.label1.Text = rm.GetString("lblSelectMsgInfo", culture);
                
                //button's text
                this.btnCancel.Text = rm.GetString("btnCancel", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializePanel(List<string> msgList)
        {
            try
            {
                int yGap = 10;

                int xPos = 0;
                int yPos = 0;

                int width = 405;
                int height = 110;

                foreach (string msg in msgList)
                {
                    TextBox tb = new TextBox();
                    tb.Location = new Point(xPos, yPos);
                    tb.Multiline = true;
                    tb.Size = new Size(width, height);
                    tb.ReadOnly = true;
                    tb.Text = msg.Trim();
                    tb.BackColor = SystemColors.Info;
                    tb.DoubleClick += new EventHandler(msg_DoubleClick);

                    msgPanel.Controls.Add(tb);
                    yPos += height + yGap;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void msg_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (sender is TextBox)
                    this.Message = ((TextBox)sender).Text.Trim();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
