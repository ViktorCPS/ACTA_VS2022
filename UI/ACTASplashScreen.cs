using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;

namespace UI
{
    /// <summary>
    /// This class is responsible for a splash screen form display during ACTA application start up.
    /// </summary>
    public class ACTASplashScreen : IDisposable
    {
        /// <summary>
        /// ACTA splash screen form.
        /// </summary>
        private ACTASplashScreenForm _form;
        
        /// <summary>
        /// Description of a current activity during ACTA start up.
        /// </summary>
        private string _activityText;

        /// <summary>
        /// Splash screen image.
        /// </summary>
        private Image _splashImage;

        public ACTASplashScreen(Image splashImage)
        {
            _splashImage = splashImage;
        }

        /// <summary>
        /// Shows splash screen.
        /// </summary>
        public void Show()
        {
            _form = new ACTASplashScreenForm();
            _form.BackgroundImage = _splashImage;
            _form.ActivityText = _activityText;
            if (_splashImage != null)
            {
                _form.Size = _splashImage.Size;
            }
            _form.Show();
            _form.Refresh();
        }

        /// <summary>
        /// Closes splash screen form.
        /// </summary>
        public void Close()
        {
            if (_form != null && _form.IsHandleCreated)
            {
                _form.Close();
                _form.Dispose();
            }
        }

        public string ActivityText
        {
            get
            {
                return _activityText;
            }
            set
            {
                _activityText = value;
                if (_form != null && _form.IsHandleCreated)
                {
                    _form.ActivityText = _activityText;
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
