using System;

namespace Winforms.Components.ApplicationIdleData
{
    /// <summary>
    /// Class that contains the one of the ActivityMessages that the component used to consider the application not idle.
    /// </summary>
    public class ActivityEventArgs : EventArgs
    {
        #region Private Members

        private ActivityMessages _Message;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ActivityEventArgs class.
        /// </summary>
        /// <param name="message">One of the ActivityMessages.</param>
        public ActivityEventArgs(ActivityMessages message)
        {
            _Message = message;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the one of the ActivityMessages that the component used to consider the application not idle. 
        /// </summary>
        public ActivityMessages Message
        {
            get { return _Message; }
        }

        #endregion
    }
}
