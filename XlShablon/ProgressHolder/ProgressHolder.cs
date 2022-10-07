using DevExpress.XtraEditors;
using GH.Windows;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static GH.Utils.ProgressHelper;

namespace GH.XlShablon
{
    public enum ProgressModes { None, Load, Test }
    public class ProgressHolder
    {
        private Dictionary<InfoNames, InfoItem> infos = new Dictionary<InfoNames, InfoItem>();

        private Timer _timer;
        private InfoControl _infoControl;
        private ProgressBarControl _progressBar;
        public ProgressHolder(XlShablon shablon, ProgressBarControl progressBar)
        {
            _infoControl = new InfoControl(this);
            _progressBar = progressBar;
            _progressBar.DataBindings.Add(new Binding(nameof(_progressBar.Position), _infoControl.DataSource, nameof(Current), true, DataSourceUpdateMode.OnPropertyChanged));

            shablon.Disposed += Shablon_Disposed;
            _timer = new Timer();
            _timer.Enabled = false;
            _timer.Interval = 1000;
            _timer.Tick += _timer_Tick;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (InProgress == false)
                return;

            InvokeIfRequired(() =>
            {
                foreach (KeyValuePair<InfoNames, InfoItem> item in infos)
                {
                    switch (item.Key)
                    {
                        case InfoNames.Info:
                            item.Value.Visible = ProgressMode != ProgressModes.None && !string.IsNullOrEmpty(Message);
                            item.Value.Text = Message;
                            break;
                        case InfoNames.Progress:
                            item.Value.Visible = ProgressMode != ProgressModes.None && Current > 0 && Total > 0 && Current != Total;
                            item.Value.Text = Progress;
                            break;
                        case InfoNames.Duration:
                            item.Value.Visible = ProgressMode != ProgressModes.None && (Current > 0 || Total > 0);
                            item.Value.Text = Duration;
                            break;
                        case InfoNames.Remaining:
                            item.Value.Visible = ProgressMode == ProgressModes.Test && Current > 0 && Total > 0 && Current != Total;
                            item.Value.Text = Remaining;
                            break;
                        case InfoNames.Summary:
                            item.Value.Visible = ProgressMode != ProgressModes.None && !string.IsNullOrEmpty(Summary);
                            item.Value.Text = Summary;
                            break;
                        default:
                            break;
                    }
                }
                _progressBar.EditValue = Current;
                _progressBar.Refresh();
                _infoControl.Refresh();
            });
        }

        internal void AddDictInfo(InfoNames info, InfoItem infoItem)
        {
            infos.Add(info, infoItem);
        }

        private ProgressModes _progressMode;
        public ProgressModes ProgressMode { get => _progressMode; set => _progressMode = value; }

        private object locker = new object();

        private int _total;
        public int Total => _total;

        private int _current;
        public int Current => _current;

        private DateTime _processStarted;

        public string Message { get; set; }
        public string Progress
        {
            get
            {
                switch (ProgressMode)
                {
                    case ProgressModes.Load:
                        return ProcessedText(Current, Total);
                    case ProgressModes.Test:
                        return ProcessedText(Current, Total);
                    default:
                        break;
                }
                return "";

            }
        }
        public string Duration => DurationText(_processStarted);
        public string Remaining => RemainingText(_processStarted, Current, Total);
        private string _summary;

        public string Summary
        {
            get => _summary;
            set
            {
                _summary = value ?? "";
                //InvokeIfRequired(() =>
                //{
                //    infos[InfoNames.Summary].Visible = !string.IsNullOrEmpty(_summary);
                //});
            }
        }


        public bool InProgress
        {
            get
            {
                return _progressMode != ProgressModes.None || (_current < _total && _progressMode != ProgressModes.None);
            }
        }

        public int StepBy { get; internal set; }

        private void InvokeIfRequired(MethodInvoker action)
        {
            _infoControl.InvokeIfRequired(action);
        }

        public void Start(int total)
        {
            _timer.Start();
            _processStarted = DateTime.Now;
            _progressMode = ProgressModes.Test;
            _infoControl.ProgressMode = true;
            _current = 0;
            _total = total;
            StepBy = Math.Min(Math.Max(1, (Total / 100)), 500);
            Summary = "";
            InvokeIfRequired(() =>
            {
                _progressBar.Visible = true;
                _progressBar.Position = 0;
                _progressBar.Properties.Maximum = Total;
            });

        }

        public void StartLoading()
        {
            _timer.Start();
            _processStarted = DateTime.Now;
            _progressMode = ProgressModes.Load;

            _infoControl.ProgressMode = true;
            _current = 0;
            _total = 0;
            StepBy = 1;
            Summary = "";
        }

        internal void Restart(int total)
        {
            _current = 0;
            _total = total;
            StepBy = Math.Min(Math.Max(1, (Total / 100)), 500);
            InvokeIfRequired(() =>
            {
                _progressBar.Visible = true;
                _progressBar.Position = 0;
                _progressBar.Properties.Maximum = Total;
            });
        }

        public void NextStep()
        {
            lock (locker)
            {
                _current++;
            }
        }

        public InfoControl GetInfoControl()
        {
            return _infoControl;
        }


        private void Shablon_Disposed(object sender, EventArgs e)
        {
            _progressMode = ProgressModes.None;
            _timer.Stop();
            _timer.Dispose();
            _timer = null;
            _infoControl = null;
        }

        public void Stop()
        {
            _progressMode = ProgressModes.None;
            _timer.Stop();


            if (_total == _current)
            {
                Summary = "Работа заверщена успешно";
            }
            else
            {
                Summary = "Работа остановленя";
            }

        }

        internal void BringToFront()
        {
            _infoControl.BringToFront();
        }


    }
}
