using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Util;

namespace ACTASurveillanceManagement
{
    class SnapshootingAxis:Snapshooting
    {
        #region Snapshooting Members

        public override byte[] GetSnapshot(string IP, out int total, DebugLog log)
        {
            byte[] buffer = null;
            total = 0;
            try
            {

                string sourceURL = "http://" + IP + "/jpg/1/image.jpg";
                int read = 0;

                // create HTTP request
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sourceURL);
                req.Credentials = new NetworkCredential(Constants.CammeraUser, Constants.CammeraPass);
                // get response
                try
                {

                    WebResponse resp = req.GetResponse();
                    buffer = new byte[100000];

                    // get response stream
                    Stream stream = resp.GetResponseStream();
                    // read data from stream
                    while ((read = stream.Read(buffer, total, 1000)) != 0)
                    {
                        total += read;
                    }
                    resp.Close();

                }
                catch (WebException ex)
                {
                    log.writeLog(DateTime.Now + " Camera: " + IP + " snapshots faild." + ex.Message);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera: " + IP + " snapshots faild." + ex.Message);
            }

            return buffer;
        }

        #endregion
    }
}
