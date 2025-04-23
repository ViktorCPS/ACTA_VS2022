using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace ACTASurveillanceManagement
{
    public abstract class Snapshooting
    {
        public abstract byte[] GetSnapshot(string IP, out int total,DebugLog log);

        public static Snapshooting getSnapshootingClass(string type)
        {
            if (type.ToUpper().StartsWith(Constants.cameraTypeAxis))
            {
                return new SnapshootingAxis();
            }
            else
            {
                return new SnapshootingAxis();
            }
        }
    }
}
