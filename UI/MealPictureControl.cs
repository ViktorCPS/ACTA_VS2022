using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using Util;
using TransferObjects;
using Common;
using System.IO;

namespace UI
{
    public partial class MealPictureControl : UserControl
    {
        private CultureInfo culture;
        private ResourceManager rm;
        private DebugLog log;
        public int typeID;
        private Form parent;
        public DayOfWeek day;

        public MealPictureControl()
        {
            try
            {
                InitializeComponent();

                typeID = 0;
                day = DayOfWeek.Monday;
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                rm = new ResourceManager("UI.Resource", typeof(MealTypesAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsPictureControl.MealsPictureControl(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public MealPictureControl(int mealTypeID, DayOfWeek dayOfWeek, Form form)
        {
            try
            {
                InitializeComponent();

                typeID = mealTypeID;

                day = dayOfWeek;

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                rm = new ResourceManager("UI.Resource", typeof(MealTypesAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                parent = form;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsPictureControl.MealsPictureControl(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }

        private void MealPictureControl_Load(object sender, EventArgs e)
        {
            try
            {
                MealTypeTO type = new MealType().GetMealType(typeID);
                MealTypeAdditionalDataTO data = new MealTypeAdditionalData().GetAdditionalData(typeID);

                byte[] byteArrayIn = null;
                if (data.Picture == null) //if there is no picture in database, take default
                {
                    Image image = Image.FromFile(Constants.DefaultMealImagePath);
                    pb.Image = image;
                }
                else
                {
                    byteArrayIn = data.Picture;
                    MemoryStream ms = new MemoryStream(byteArrayIn);
                    Image image = Image.FromStream(ms);
                    pb.Image = image;
                }

                string nameOfBox = "pb" + data.MealTypeID + "-" + day;
                pb.Name = nameOfBox;
                pb.Visible = true;
                pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
                pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                pb.Tag = typeID;
                lbl.Text = type.Name;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsPictureControl.MealsPictureControl_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void pb_Click(object sender, EventArgs e)
        {
            try
            {
                PictureBox pictureBox = sender as PictureBox;

                int typeID = Convert.ToInt32(pictureBox.Tag.ToString());
                MealTypeAdditionalDataTO additionalData = new MealTypeAdditionalData().GetAdditionalData(typeID);
                string day = pictureBox.Name.Substring(pictureBox.Name.IndexOf("-") + 1);

                if (parent.GetType().ToString().Equals("UI.MealsOrder"))
                    ((MealsOrder)parent).setOrders(day, typeID, pictureBox.Image);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsPictureControl.pb_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
           
        }
    }
}
