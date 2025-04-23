using System;
using System.IO;
using System.Security.Cryptography;
using System.Configuration;
using System.Windows.Forms;

namespace Util
{
	/// <summary>
	/// This class containes miscellaneous functions.
	/// </summary>
	public class Misc
	{
		public Misc()
		{
		}

		#region String Time functions

		public static string transformMinToStringTime(int minutes)
		{
            string stringTime = "";
            int hrs = minutes / 60;
            int min = minutes % 60;
            if (minutes >= 0)
            {           
                stringTime = transformTimeToStringTime(hrs, min);
            }
            else if (minutes < 0)
            {
                min = -1 * min;
                stringTime = transformTimeToStringTime(hrs, min);
                if (hrs == 0)
                {
                    stringTime = "   -" + stringTime.Substring(4, stringTime.Length - 4);
                }
                else
                {
                    stringTime = " " + stringTime;
                }
            }

			return stringTime;
		}

		public static string transformTimeToStringTime(int hrs, int min)
		{
			string stringTime = "";		
			stringTime = ((hrs.ToString().Length > 2) ? hrs.ToString() : ((hrs.ToString().Length > 1) ? ("  " + hrs.ToString()) : ("    " + hrs.ToString())))
				+ " " + Constants.Hour + " "
				+ ((min.ToString().Length > 1) ? min.ToString() : ("  " + min.ToString()))
				+ " " + Constants.Minute;

			return stringTime;
		}

		public static int transformStringTimeToMin(string stringTime)
		{
            bool neagtive = false;
            if (stringTime[0].Equals("-"))
            {
                neagtive = true;
                stringTime = stringTime.Substring(1, stringTime.Length - 1);
            }
                int minutes = 0;
                int index = stringTime.LastIndexOf(Constants.Hour);
                if (index > 0)
                {
                    minutes += 60 * Int32.Parse(stringTime.Substring(0, index).Trim());
                    stringTime = stringTime.Substring(index + Constants.Hour.Length);

                    index = stringTime.LastIndexOf(Constants.Minute);
                    if (index > 0)
                    {
                        minutes += Int32.Parse(stringTime.Substring(0, index).Trim());
                    }
                }
                else
                {
                    index = stringTime.LastIndexOf(":");
                    if (index > 0)
                    {
                        minutes += 60 * Int32.Parse(stringTime.Substring(0, index).Trim());
                        stringTime = stringTime.Substring(index + ":".Length);

                        minutes += Int32.Parse(stringTime.Trim());
                    }
                }
                if (neagtive)
                {
                    minutes = -1 * minutes;
                }
			return minutes;
		}

		#endregion

        #region Encryption Decryption functions

        // Encrypt the string
        public static byte[] encrypt(string text)
        {
            DESCryptoServiceProvider key = new DESCryptoServiceProvider();

            // set encription key and initialization vector
            key.Key = Convert.FromBase64String(Constants.DESKey);
            key.IV = Convert.FromBase64String(Constants.DESIV);

            // Create a memory stream.
            MemoryStream ms = new MemoryStream();

            // Create a CryptoStream using the memory stream and the 
            // CSP DES key.  
            CryptoStream encStream = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write);

            // Create a StreamWriter to write a string
            // to the stream.
            StreamWriter sw = new StreamWriter(encStream);

            // Write the plaintext to the stream.
            sw.WriteLine(text);

            // Close the StreamWriter and CryptoStream.
            sw.Close();
            encStream.Close();

            // Get an array of bytes that represents
            // the memory stream.
            byte[] buffer = ms.ToArray();

            // Close the memory stream.
            ms.Close();

            // Return the encrypted byte array.
            return buffer;
        }

        // Decrypt the byte array.
        public static string decrypt(byte[] text)
        {
            DESCryptoServiceProvider key = new DESCryptoServiceProvider();

            // set decription key and initialization vector
            key.Key = Convert.FromBase64String(Constants.DESKey);
            key.IV = Convert.FromBase64String(Constants.DESIV);

            // Create a memory stream to the passed buffer.
            MemoryStream ms = new MemoryStream(text);

            // Create a CryptoStream using the memory stream and the 
            // CSP DES key. 
            CryptoStream encStream = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read);

            // Create a StreamReader for reading the stream.
            StreamReader sr = new StreamReader(encStream);

            // Read the stream as a string.
            string val = sr.ReadLine();

            // Close the streams.
            sr.Close();
            encStream.Close();
            ms.Close();

            return val;
        }

        # endregion

        # region config Add/Remove functions

        // add new or replace existing config key
        public static void configAdd(string key, string value)
        {
            try
            {
                // write to App.config
                Configuration config = null;

                // open App.Config of executable
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // add new key
                if (ConfigurationManager.AppSettings[key] != null)
                {
                    config.AppSettings.Settings.Remove(key);
                    config.AppSettings.Settings.Add(key, value);
                }
                else
                {
                    config.AppSettings.Settings.Add(key, value);
                }

                // save the changes in App.config file.
                config.Save(ConfigurationSaveMode.Modified);

                // force a reload of a changed section.
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // remove specified config key
        public static void configRemoveOne(string key)
        {
            try
            {
                Configuration config = null;

                // open App.Config of executable
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // remove key
                if (ConfigurationManager.AppSettings[key] != null)
                {
                    config.AppSettings.Settings.Remove(key);
                }

                // save the changes in App.config file.
                config.Save(ConfigurationSaveMode.Modified);

                // force a reload of a changed section.
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // remove all config keys that start as keyPart
        public static void configRemove(string keyPart)
        {
            try
            {
                Configuration config = null;

                // open App.Config of executable
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

               
                foreach (string key in ConfigurationManager.AppSettings.Keys)
                {
                    if (key.StartsWith(keyPart))
                    {
                        config.AppSettings.Settings.Remove(key);
                    }
                }

                // save the changes in App.config file.
                config.Save(ConfigurationSaveMode.Modified);

                // force a reload of a changed section.
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        # endregion

        public static void helpManualHtml(string formName)
        {
            try
            { 
                WebBrowser webBrowser = new WebBrowser();
                string navigationString = Constants.HelpManualHtmlPath;
                if (Constants.HelpLinks.ContainsKey(formName))
                {
                    navigationString += Constants.HelpLinks[formName].ToString();
                }
                webBrowser.Navigate(navigationString,true);
                webBrowser.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
