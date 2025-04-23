using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Winforms.Components.ApplicationIdleData;

namespace Winforms.Components
{
    /// <summary>
    /// A WinForms component that determines whether an application has received any defined ActivityMessages for a specified TimeSpan.
    /// </summary>
    [ToolboxBitmap(typeof(Timer)),
    DefaultProperty("IdleTime"),
    DefaultEvent("Idle")]
    public class ApplicationIdle : Component, IMessageFilter
    {
        #region Synchronous Events

        /// <summary>
        /// Raised when the IdleTime is reached.
        /// </summary>
        [Category("Behavior"),
        Description("Raised when the IdleTime is reached.")]
        public event EventHandler Idle;

        /// <summary>
        /// Raised when the component is started.
        /// </summary>
        [Category("Behavior"),
        Description("Raised when the component is started.")]
        public event EventHandler Started;

        /// <summary>
        /// Raised when the component is paused.
        /// </summary>
        [Category("Behavior"),
        Description("Raised when the component is paused.")]
        public event EventHandler Paused;

        /// <summary>
        /// Raised when the component is unpaused.
        /// </summary>
        [Category("Behavior"),
        Description("Raised when the component is unpaused.")]
        public event EventHandler UnPaused;

        /// <summary>
        /// Raised when the component is stopped.
        /// </summary>
        [Category("Behavior"),
        Description("Raised when the component is stopped.")]
        public event EventHandler Stopped;

        /// <summary>
        /// Raised when the component 'ticks'.
        /// </summary>
        [Category("Behavior"),
        Description("Raised when the component 'ticks'")]
        public event EventHandler<TickEventArgs> Tick;

        /// <summary>
        /// May be raised when the WarnTime is reached and on each subsequent Tick depending on the WarnSetting.
        /// </summary>
        [Category("Behavior"),
        Description("May be raised when the WarnTime is reached and on each subsequent Tick depending on the WarnSetting.")]
        public event EventHandler Warn;

        /// <summary>
        /// Raised when the component detects an activity that is defined in ActivityMessages.
        /// </summary>
        [Category("Behavior"),
        Description("Raised when the component detects an activity that is defined in ActivityMessages.")]
        public event EventHandler<ActivityEventArgs> Activity;

        /// <summary>
        /// Raised when the IdleTime has been changed.
        /// </summary>
        [Category("Property Changed"),
        Description("Raised when the IdleTime has been changed.")]
        public event EventHandler IdleTimeChanged;

        /// <summary>
        /// Raised when the TickInterval has been changed.
        /// </summary>
        [Category("Property Changed"),
        Description("Raised when the TickInterval has been changed.")]
        public event EventHandler TickIntervalChanged;

        /// <summary>
        /// Raised when the WarnTime has been changed.
        /// </summary>
        [Category("Property Changed"),
        Description("Raised when the WarnTime has been changed.")]
        public event EventHandler WarnTimeChanged;

        /// <summary>
        /// Raised when the WarnSetting has been changed.
        /// </summary>
        [Category("Property Changed"),
        Description("Raised when the WarnSetting has been changed.")]
        public event EventHandler WarnSettingChanged;

        #endregion

        #region Asynchronous Events

        /// <summary>
        /// Raised asynchronously on a seperate thread when the IdleTime is reached.
        /// </summary>
        [Category("Behavior Async"),
        Description("Raised asynchronously on a seperate thread when the IdleTime is reached.")]
        public event EventHandler IdleAsync;

        /// <summary>
        /// Raised asynchronously on a seperate thread when the component is started.
        /// </summary>
        [Category("Behavior Async"),
        Description("Raised asynchronously on a seperate thread when the component is started.")]
        public event EventHandler StartedAsync;

        /// <summary>
        /// Raised asynchronously on a seperate thread when the component is paused.
        /// </summary>
        [Category("Behavior Async"),
        Description("Raised asynchronously on a seperate thread when the component is paused.")]
        public event EventHandler PausedAsync;

        /// <summary>
        /// Raised asynchronously on a seperate thread when the component is unpaused.
        /// </summary>
        [Category("Behavior Async"),
        Description("Raised asynchronously on a seperate thread when the component is unpaused.")]
        public event EventHandler UnPausedAsync;

        /// <summary>
        /// Raised asynchronously on a seperate thread when the component is stopped.
        /// </summary>
        [Category("Behavior Async"),
        Description("Raised asynchronously on a seperate thread when the component is stopped.")]
        public event EventHandler StoppedAsync;

        /// <summary>
        /// Raised asynchronously on a seperate thread when the component 'ticks'.
        /// </summary>
        [Category("Behavior Async"),
        Description("Raised asynchronously on a seperate thread when the component 'ticks'.")]
        public event EventHandler<TickEventArgs> TickAsync;

        /// <summary>
        /// May be raised asynchronously on a seperate thread when the WarnTime is reached and on each subsequent Tick depending on the WarnSetting.
        /// </summary>
        [Category("Behavior Async"),
        Description("May be raised asynchronously on a seperate thread when the WarnTime is reached and on each subsequent Tick depending on the WarnSetting.")]
        public event EventHandler WarnAsync;

        /// <summary>
        /// Raised asynchronously on a seperate thread when the component detects an activity that is defined in ActivityMessages.
        /// </summary>
        [Category("Behavior Async"),
        Description("Raised asynchronously on a seperate thread when the component detects an activity that is defined in ActivityMessages.")]
        public event EventHandler<ActivityEventArgs> ActivityAsync;

        #endregion

        #region Static Fields

        /// <summary>
        /// A System.TimeSpan with all values zero.
        /// </summary>
        public static readonly TimeSpan ZeroTime = new TimeSpan(0, 0, 0);
        private static readonly TimeSpan oneSecond = new TimeSpan(0, 0, 1);

        #endregion

        #region Private Members etc

        private IContainer components = null;
        private Timer timer;

        private TimeSpan _IdleTime;
        private TimeSpan _TickInterval;
        private TimeSpan _WarnTime;
        private WarnSettings _WarnSetting;
        private TimeSpan _TimeRemaining;
        private TimeSpan _TimeElapsed;
        private bool _IsRunning;
        private bool _IsPaused;

        bool warnRecalculated;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ApplicationIdle component.
        /// </summary>
        public ApplicationIdle()
        {
            InitializeComponent();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);

            _IdleTime = new TimeSpan(0, 2, 0);
            _TickInterval = new TimeSpan(0, 0, 1);
            _WarnTime = new TimeSpan(0, 0, 10);
            _WarnSetting = WarnSettings.Tick;
            _TimeRemaining = ZeroTime;
            _TimeElapsed = ZeroTime;
            _IsRunning = false;
            _IsPaused = false;
        }

        #endregion

        #region Read/Write Properties

        /// <summary>
        /// Gets or sets the TimeSpan after which the application should be considered idle if no defined ActivityMessages are received.
        /// </summary>
        [DefaultValue(typeof(TimeSpan), "00:02:00"),
        Category("Behavior"),
        Description("The TimeSpan after which the application should be considered idle if no defined ActivityMessages are received."),
        RefreshProperties(RefreshProperties.All)]
        public TimeSpan IdleTime
        {
            get { return _IdleTime; }
            set
            {
                if (!_IsRunning)
                {
                    TimeSpan altered = TimeSpanRound(value);
                    if (_IdleTime != altered)
                    {
                        _IdleTime = altered;
                        OnIdleTimeChanged(EventArgs.Empty);
                        warnRecalculated = false;
                        TickInterval = _TickInterval; // force recalculation
                        if(!warnRecalculated)
                            WarnTime = _WarnTime; // force recalculation
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the TimeSpan at which the component 'ticks'.
        /// </summary>
        [DefaultValue(typeof(TimeSpan), "00:00:01"),
        Category("Behavior"),
        Description("The TimeSpan at which the component 'ticks'."),
        RefreshProperties(RefreshProperties.All)]
        public TimeSpan TickInterval
        {
            get { return _TickInterval; }
            set
            {
                if (!_IsRunning)
                {
                    TimeSpan altered;
                    if (_IdleTime == oneSecond)
                    {
                        altered = oneSecond;
                        if (altered != _TickInterval)
                        {
                            _TickInterval = altered;
                            timer.Interval = 1000;
                            OnTickIntervalChanged(EventArgs.Empty);
                            WarnTime = _WarnTime; // force recalculation
                            warnRecalculated = true;
                        }
                    }
                    else
                    {
                        altered = TimeSpanRound(value);
                        long requestedSeconds = (long)altered.TotalSeconds;
                        long lastFactor = 1;
                        for (long second = 1; second <= _IdleTime.TotalSeconds; second++)
                        {
                            if (_IdleTime.TotalSeconds % second == 0)
                            {
                                if (second <= requestedSeconds)
                                    lastFactor = second;
                                else
                                    break;
                            }
                        }
                        altered = new TimeSpan(lastFactor * TimeSpan.TicksPerSecond);
                        if (_TickInterval != altered)
                        {
                            _TickInterval = altered;
                            timer.Interval = (int)_TickInterval.TotalSeconds * 1000;
                            OnTickIntervalChanged(EventArgs.Empty);
                            WarnTime = _WarnTime; // force recalculation
                            warnRecalculated = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the TimeSpan at which warning events will be generated depending on the WarnSettings.
        /// </summary>
        [DefaultValue(typeof(TimeSpan), "00:00:10"),
        Category("Behavior"),
        Description("The TimeSpan at which warning events will be generated depending on the WarnSettings.")]
        public TimeSpan WarnTime
        {
            get { return _WarnTime; }
            set
            {
                TimeSpan altered = TimeSpanRound(value);
                if (altered.TotalSeconds % _TickInterval.TotalSeconds == 0 && _WarnTime <= _IdleTime)
                {
                    if (_WarnTime != altered)
                    {
                        _WarnTime = altered;
                        OnWarnTimeChanged(EventArgs.Empty);
                    }
                }
                else if (altered <= _TickInterval)
                {
                    if (_WarnTime != _TickInterval)
                    {
                        _WarnTime = _TickInterval;
                        OnWarnTimeChanged(EventArgs.Empty);
                    }
                }
                else
                {
                    long requestedSeconds = (long)altered.TotalSeconds;
                    long tickSeconds = (long)_TickInterval.TotalSeconds;
                    long lastMultiple = tickSeconds;
                    for (long second = tickSeconds; second <= _IdleTime.TotalSeconds; second += tickSeconds)
                    {
                        if (second <= requestedSeconds)
                            lastMultiple = second;
                        else
                            break;
                    }
                    altered = new TimeSpan(lastMultiple * TimeSpan.TicksPerSecond);
                    if (_WarnTime != altered)
                    {
                        _WarnTime = altered;
                        OnWarnTimeChanged(EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the WarnSettings value used to control warning events generation.
        /// </summary>
        [DefaultValue(WarnSettings.Tick),
        Category("Behavior"),
        Description("The WarnSettings value used to control warning events generation.")]
        public WarnSettings WarnSetting
        {
            get { return _WarnSetting; }
            set
            {
                if (value != _WarnSetting)
                {
                    _WarnSetting = value;
                    OnWarnSettingChanged(EventArgs.Empty);
                }
            }
        }

        #endregion

        #region ReadOnly Properties

        /// <summary>
        /// Gets the TimeSpan representing the time until Idle assuming no activity is detected.
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan TimeRemaining
        {
            get { return _TimeRemaining; }
        }

        /// <summary>
        /// Gets the TimeSpan representing the time since the last activity was detected.
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan TimeElapsed
        {
            get { return _TimeElapsed; }
        }

        /// <summary>
        /// Gets whether the component is currently running.
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsRunning
        {
            get { return _IsRunning; }
        }

        /// <summary>
        /// Gets whether the component is currently paused.
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPaused
        {
            get { return _IsPaused; }
        }

        #endregion

        #region Private Methods

        private void InitializeComponent()
        {
            components = new Container();
            timer = new Timer(components);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            _TimeElapsed = _TimeElapsed.Add(_TickInterval);
            _TimeRemaining = _TimeRemaining.Subtract(_TickInterval);
            if (_WarnSetting != WarnSettings.Off && _TimeRemaining <= _WarnTime && _TimeRemaining != ZeroTime)
            {
                OnTickAsync(new TickEventArgs(true));
                OnTick(new TickEventArgs(true));
            }
            else
            {
                OnTickAsync(new TickEventArgs(false));
                OnTick(new TickEventArgs(false));
            }
            switch (_WarnSetting)
            {
                case WarnSettings.Off:
                    break;
                case WarnSettings.Once:
                    if (_TimeRemaining == _WarnTime)
                    {
                        OnWarnAsync(EventArgs.Empty);
                        OnWarn(EventArgs.Empty);
                    }
                    break;
                default:
                    if (_TimeRemaining <= _WarnTime)
                    {
                        OnWarnAsync(EventArgs.Empty);
                        OnWarn(EventArgs.Empty);
                    }
                    break;
            }
            if (_TimeRemaining == ZeroTime)
            {
                OnIdleAsync(EventArgs.Empty);
                OnIdle(EventArgs.Empty);
                Stop();
            }
        }

        private TimeSpan TimeSpanRound(TimeSpan timeSpan)
        {
            double seconds = Math.Round(timeSpan.TotalSeconds, 0, MidpointRounding.AwayFromZero);
            if (seconds == 0)
                seconds = 1;
            return new TimeSpan((long)seconds * TimeSpan.TicksPerSecond);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the component.
        /// </summary>
        public void Start()
        {
            if (!_IsRunning)
            {
                _TimeRemaining = _IdleTime;
                _TimeElapsed = ZeroTime;
                Application.AddMessageFilter(this);
                timer.Start();
                _IsRunning = true;
                OnStartedAsync(EventArgs.Empty);
                OnStarted(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Toggles the pause state of the component.
        /// </summary>
        public void TogglePause()
        {
            if (_IsRunning)
            {
                _IsPaused = !_IsPaused;
                timer.Enabled = !_IsPaused;
                if (_IsPaused)
                {
                    OnPausedAsync(EventArgs.Empty);
                    OnPaused(EventArgs.Empty);
                }
                else
                {
                    OnUnPausedAsync(EventArgs.Empty);
                    OnUnPaused(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Stops the component.
        /// </summary>
        public void Stop()
        {
            if (_IsRunning)
            {
                timer.Stop();
                _TimeRemaining = ZeroTime;
                _TimeElapsed = ZeroTime;
                _IsRunning = false;
                _IsPaused = false;
                Application.RemoveMessageFilter(this);
                OnStoppedAsync(EventArgs.Empty);
                OnStopped(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Stops, and then starts the component.
        /// </summary>
        public void Restart()
        {
            if (_IsRunning)
            {
                Stop();
                Start();
            }
        }

        /// <summary>
        /// Gets the System.Version of this component.
        /// </summary>
        /// <returns>The System.Version of this component</returns>
        public static Version GetVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        #endregion

        #region Event Raising and Asynchronous Callback Methods

        /// <summary>
        /// Raises the Idle event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnIdle(EventArgs e)
        {
            EventHandler eh = Idle;
            if (eh != null)
                eh(this, e);
        }

        /// <summary>
        /// Raises the Started event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnStarted(EventArgs e)
        {
            EventHandler eh = Started;
            if (eh != null)
                eh(this, e);
        }

        /// <summary>
        /// Raises the Paused event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnPaused(EventArgs e)
        {
            EventHandler eh = Paused;
            if (eh != null)
                eh(this, e);
        }

        /// <summary>
        /// Raises the UnPaused event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnUnPaused(EventArgs e)
        {
            EventHandler eh = UnPaused;
            if (eh != null)
                eh(this, e);
        }

        /// <summary>
        /// Raises the Stopped event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnStopped(EventArgs e)
        {
            EventHandler eh = Stopped;
            if (eh != null)
                eh(this, e);
        }

        /// <summary>
        /// Raises the Tick event.
        /// </summary>
        /// <param name="e">A TickEventArgs that contains the event data.</param>
        protected virtual void OnTick(TickEventArgs e)
        {
            EventHandler<TickEventArgs> eh = Tick;
            if (eh != null)
                eh(this, e);
        }

        /// <summary>
        /// Raises the WarnEvent.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnWarn(EventArgs e)
        {
            EventHandler eh = Warn;
            if (eh != null)
                eh(this, e);
        }

        /// <summary>
        /// Raises the Activity event.
        /// </summary>
        /// <param name="e">An ActivityEventArgs that contains the event data.</param>
        protected virtual void OnActivity(ActivityEventArgs e)
        {
            EventHandler<ActivityEventArgs> eh = Activity;
            if (eh != null)
                eh(this, e);
        }

        /// <summary>
        /// Raises the IdleTimeChanged event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnIdleTimeChanged(EventArgs e)
        {
            EventHandler eh = IdleTimeChanged;
            if (eh != null)
                eh(this, e);
        }

        /// <summary>
        /// Raises the TickIntervalChanged event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnTickIntervalChanged(EventArgs e)
        {
            EventHandler eh = TickIntervalChanged;
            if (eh != null)
                eh(this, e);
        }

        /// <summary>
        /// Raises the WarnTimeChanged event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnWarnTimeChanged(EventArgs e)
        {
            EventHandler eh = WarnTimeChanged;
            if (eh != null)
                eh(this, e);
        }

        /// <summary>
        /// Raises the WarnSettingChanged event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnWarnSettingChanged(EventArgs e)
        {
            EventHandler eh = WarnSettingChanged;
            if (eh != null)
                eh(this, e);
        }

        /// <summary>
        /// Raises the IdleAsync event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnIdleAsync(EventArgs e)
        {
            EventHandler eh = IdleAsync;
            if (eh != null)
                foreach (EventHandler subscriber in eh.GetInvocationList())
                    subscriber.BeginInvoke(
                        this, e, new AsyncCallback(
                            OnAsyncCompleted), subscriber);
        }

        /// <summary>
        /// Raises the StartedAsync event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnStartedAsync(EventArgs e)
        {
            EventHandler eh = StartedAsync;
            if (eh != null)
                foreach (EventHandler subscriber in eh.GetInvocationList())
                    subscriber.BeginInvoke(
                        this, e, new AsyncCallback(
                            OnAsyncCompleted), subscriber);
        }

        /// <summary>
        /// Raises the PausedAsync event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnPausedAsync(EventArgs e)
        {
            EventHandler eh = PausedAsync;
            if (eh != null)
                foreach (EventHandler subscriber in eh.GetInvocationList())
                    subscriber.BeginInvoke(
                        this, e, new AsyncCallback(
                            OnAsyncCompleted), subscriber);
        }

        /// <summary>
        /// Raises the UnPausedAsync event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnUnPausedAsync(EventArgs e)
        {
            EventHandler eh = UnPausedAsync;
            if (eh != null)
                foreach (EventHandler subscriber in eh.GetInvocationList())
                    subscriber.BeginInvoke(
                        this, e, new AsyncCallback(
                            OnAsyncCompleted), subscriber);
        }

        /// <summary>
        /// Raises the StoppedAsync event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnStoppedAsync(EventArgs e)
        {
            EventHandler eh = StoppedAsync;
            if (eh != null)
                foreach (EventHandler subscriber in eh.GetInvocationList())
                    subscriber.BeginInvoke(
                        this, e, new AsyncCallback(
                            OnAsyncCompleted), subscriber);
        }

        /// <summary>
        /// Raises the TickAsync event.
        /// </summary>
        /// <param name="e">A TickEventArgs that contains the event data.</param>
        protected virtual void OnTickAsync(TickEventArgs e)
        {
            EventHandler<TickEventArgs> eh = TickAsync;
            if (eh != null)
                foreach (EventHandler<TickEventArgs> subscriber in eh.GetInvocationList())
                    subscriber.BeginInvoke(
                        this, e, new AsyncCallback(
                            OnTickAsyncCompleted), subscriber);
        }

        /// <summary>
        /// Raises the WarnAsync event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected virtual void OnWarnAsync(EventArgs e)
        {
            EventHandler eh = WarnAsync;
            if (eh != null)
                foreach (EventHandler subscriber in eh.GetInvocationList())
                    subscriber.BeginInvoke(
                        this, e, new AsyncCallback(
                            OnAsyncCompleted), subscriber);
        }

        /// <summary>
        /// Raises the ActivityAsync event.
        /// </summary>
        /// <param name="e">An ActivityEventArgs that contains the event data.</param>
        protected virtual void OnActivityAsync(ActivityEventArgs e)
        {
            EventHandler<ActivityEventArgs> eh = ActivityAsync;
            if (eh != null)
                foreach (EventHandler<ActivityEventArgs> subscriber in eh.GetInvocationList())
                    subscriber.BeginInvoke(
                        this, e, new AsyncCallback(
                            OnActivityAsyncCompleted), subscriber);
        }

        /// <summary>
        /// Raised when an asynchronous event callsback.
        /// </summary>
        /// <param name="asyncResult">An IAsyncResult that represents the status of the asynchronous operation.</param>
        protected virtual void OnAsyncCompleted(IAsyncResult asyncResult)
        {
            EventHandler subscriber = (EventHandler)asyncResult.AsyncState;
            subscriber.EndInvoke(asyncResult);
        }

        /// <summary>
        /// Raised when the TickAsync event callsback.
        /// </summary>
        /// <param name="asyncResult">An IAsyncResult that represents the status of the asynchronous operation.</param>
        protected virtual void OnTickAsyncCompleted(IAsyncResult asyncResult)
        {
            EventHandler<TickEventArgs> subscriber = (EventHandler<TickEventArgs>)asyncResult.AsyncState;
            subscriber.EndInvoke(asyncResult);
        }

        /// <summary>
        /// Raised when the ActivityAsync event callsback.
        /// </summary>
        /// <param name="asyncResult">An IAsyncResult that represents the status of the asynchronous operation.</param>
        protected virtual void OnActivityAsyncCompleted(IAsyncResult asyncResult)
        {
            EventHandler<ActivityEventArgs> subscriber = (EventHandler<ActivityEventArgs>)asyncResult.AsyncState;
            subscriber.EndInvoke(asyncResult);
        }

        #endregion

        #region Overridden Methods
        
        /// <summary>
        /// Releases all resources used by the component.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (_IsRunning)
                Stop();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region IMessageFilter Members

        bool IMessageFilter.PreFilterMessage(ref Message m)
        {
            if (Enum.IsDefined(typeof(ActivityMessages), m.Msg))
            {
                _TimeRemaining = _IdleTime;
                _TimeElapsed = ZeroTime;
                ActivityEventArgs e = new ActivityEventArgs((ActivityMessages)m.Msg);
                OnActivityAsync(e);
                OnActivity(e);
            }
            return false;
        }

        #endregion
    }
}