using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;
using System.Collections;

namespace UI
{
    public partial class MealTypesAdditionalData : Form
    {
        string mealID = "";
        string mealName = "";
        string mealDesc = "";
        string picturePath = "";
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        public MealTypeAdditionalData currentMealTypeAddData;
        bool pictureChanged = false;

        public MealTypesAdditionalData()
        {
            InitializeComponent();

            this.CenterToScreen();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            currentMealTypeAddData = new MealTypeAdditionalData();
            logInUser = NotificationController.GetLogInUser();

            rm = new ResourceManager("UI.Resource", typeof(MealTypesAdd).Assembly);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            setLanguage();

            this.btnUpdate.Visible = false;
        }

        public void setMealID(string id)
        {
            this.mealID = id;
        }

        public void setMealName(string name)
        {
            this.mealName = name;
        }

        private void setLanguage()
        {
            this.lblMealTypeID.Text = rm.GetString("lblMealTypeID", culture);
            this.lblDescription.Text = rm.GetString("lblDescription", culture);
            this.lblName.Text = rm.GetString("lblName", culture);
            this.lblPath.Text = rm.GetString("lblPath", culture);

            this.btnCancel.Text = rm.GetString("btnCancel", culture);
            this.btnSave.Text = rm.GetString("btnSave", culture);
            this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
            this.btnBrowse.Text = rm.GetString("btnBrowse", culture);

            // Form name
            this.Text = rm.GetString("frmAdditionalData", culture);
        }

        private MealTypeAdditionalDataTO additionalData(int mealID) //return object MealTypeAdditionalData for mealID
        {
            MealTypeAdditionalDataTO data = new MealTypeAdditionalDataTO();
            try
            {
                data = currentMealTypeAddData.GetAdditionalData(mealID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesAdditionalData.additionalData(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return data;

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = rm.GetString("browsePhoto", culture);
                dlg.Filter = "bmp files (*.bmp)|*.bmp"
                    + "|gif files (*.gif)|*.gif"
                    + "|jpg files (*.jpg)|*.jpg"
                    + "|jpeg files (*.jpeg)|*.jpeg";
                dlg.FilterIndex = 3;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(dlg.FileName);
                    if (fi.Length > 1043576)
                    {
                        MessageBox.Show(rm.GetString("imgLarge", culture));
                    }
                    else
                    {
                        this.tbPicture.Text = dlg.FileName;
                        if (pbMeal.Image != null)
                            pbMeal.Image.Dispose();
                        this.pbMeal.Image = new Bitmap(dlg.FileName);
                    }
                }

                dlg.Dispose();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.btnBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                currentMealTypeAddData.MealTypeAddDataTO = null;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesAdditionalData.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool updated = false;

                int idOfMeal = Int32.Parse(tbMealTypeID.Text.Trim());
                mealDesc = tbDescription.Text.Trim();
                byte[] imgbyte = null;

                if (!tbPicture.Text.Equals("") && tbPicture.Text != null)
                {
                    picturePath = tbPicture.Text.Trim();
                    imgbyte = File.ReadAllBytes(picturePath);
                }
                else
                {
                    imgbyte = imageToByteArray(pbMeal.Image);
                }
                updated = currentMealTypeAddData.Update(idOfMeal, mealDesc, imgbyte);

                if (updated)
                {
                    MessageBox.Show(rm.GetString("mealTypeAdditionalDataUpdated", culture));
                }
                else
                {
                    MessageBox.Show(rm.GetString("mealTypeAdditionalDataNotUpdated", culture));
                }

                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void gbMealType_Enter(object sender, EventArgs e)
        {
            tbMealTypeID.Text = mealID;
            tbName.Text = mealName;
            MealTypeAdditionalDataTO mealType = additionalData(Convert.ToInt32(mealID));
            if (mealType.MealTypeID != -1) //postoji u bazi
            {
                tbDescription.Text = mealType.DescriptionAdditional.ToString().Trim();
                if (mealType.Picture != null)
                {
                    byte[] byteArrayIn = mealType.Picture;
                    MemoryStream ms = new MemoryStream(byteArrayIn);
                    Image returnImage = Image.FromStream(ms);
                    pbMeal.Image = returnImage;
                }
                btnUpdate.Visible = true;
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
                btnUpdate.Visible = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // validate
                int id = -1;
                if (!tbMealTypeID.Text.Trim().Equals(""))
                {
                    if (!Int32.TryParse(tbMealTypeID.Text.Trim(), out id))
                    {
                        MessageBox.Show(rm.GetString("mealTypeIDNotNum", culture));
                        tbMealTypeID.Focus();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("mealTypeIDNotSet", culture));
                    tbMealTypeID.Focus();
                    return;
                }

                currentMealTypeAddData.MealTypeAddDataTO.MealTypeID = Convert.ToInt32(tbMealTypeID.Text.Trim());

                if (tbDescription.Text != null && !tbDescription.Text.Equals(""))
                {
                    currentMealTypeAddData.MealTypeAddDataTO.DescriptionAdditional = tbDescription.Text.Trim();
                }
                byte[] imgbyte = null;
                if (tbPicture.Text != null && !tbPicture.Text.Equals(""))
                {
                    picturePath = tbPicture.Text.Trim();
                    imgbyte = File.ReadAllBytes(picturePath);
                }
                else if (pbMeal.Image != null)
                {
                    imgbyte = imageToByteArray(pbMeal.Image);
                }

                currentMealTypeAddData.MealTypeAddDataTO.Picture = imgbyte;
                this.Close();
                //int inserted = currentMealTypeAddData.Save(currentMealTypeAddData.MealTypeAddDataTO.MealTypeID, currentMealTypeAddData.MealTypeAddDataTO.DescriptionAdditional, imgbyte);
                //if (inserted > 0)
                //{
                //    MessageBox.Show(rm.GetString("mealTypeAddDataInserted", culture));
                //    this.Close();                  
                //}
                //else
                //{
                //    MessageBox.Show(rm.GetString("mealTypeAddDataNotInserted", culture));
                //}
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesAdditionalData.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("mealTypeAddDataNotInserted", culture));
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

    }
}
