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

        private InfoControl _infoControl;
        private ProgressBarControl _progressBar;
        public ProgressHolder(XlShablon shablon, ProgressBarControl progressBar)
        {
            _infoControl = new InfoControl(this);
            _progressBar = progressBar;
            _progressBar.DataBindings.Add(new Binding(nameof(_progressBar.Position), _infoControl.DataSource, nameof(Current), true, DataSourceUpdateMode.OnPropertyChanged));

            shablon.Disposed += Shablon_Disposed;
        }

        public void RefreshInfo(bool first = false)
        {
            InvokeIfRequired(() =>
            {
                foreach (KeyValuePair<InfoNames, InfoItem> item in infos)
                {
                    switch (item.Key)
                    {
                        case InfoNames.Info:
                            item.Value.Visible = ProgressMode != ProgressModes.None && !string.IsNullOrEmpty(Info);
                            item.Value.Text = Info;
                            if (first)
                                item.Value.BringToFront();
                            break;
                        case InfoNames.Progress:
                            item.Value.Visible = ProgressMode != ProgressModes.None && Current > 0 && Total > 0;
                            item.Value.Text = Progress;
                            if (first)
                                item.Value.BringToFront();
                            break;
                        case InfoNames.Duration:
                            item.Value.Visible = ProgressMode != ProgressModes.None && (Current > 0 || Total > 0);
                            item.Value.Text = Duration;
                            if (first)
                                item.Value.BringToFront();
                            break;
                        case InfoNames.Remaining:
                            item.Value.Visible = ProgressMode == ProgressModes.Test && Current > 0 && Total > 0 && Current != Total;
                            item.Value.Text = Remaining;
                            if (first)
                                item.Value.BringToFront();
                            break;
                        case InfoNames.Summary:
                            item.Value.Visible = ProgressMode != ProgressModes.None && !string.IsNullOrEmpty(Summary);
                            item.Value.Text = Summary;
                            if (first)
                                item.Value.BringToFront();
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
        public int Total
        {
            get => _total;
            set
            {

                _total = value;
                switch (ProgressMode)
                {
                    case ProgressModes.Load:
                        InvokeIfRequired(() =>
                        {
                            infos[InfoNames.Progress].Visible = _total > 0;
                            infos[InfoNames.Progress].Text = Progress;
                        });
                        break;
                    default:
                        break;
                }
            }
        }

        private int _current;
        public int Current => _current;

        private DateTime _processStarted;

        public string Info
        {
            get => _info;
            set
            {
                _info = value ?? "";
                InvokeIfRequired(() =>
                {
                    infos[InfoNames.Info].Visible = !string.IsNullOrEmpty(_info);
                    switch (ProgressMode)
                    {
                        case ProgressModes.Load:
                            infos[InfoNames.Info].Text = _info;
                            break;
                        default:
                            break;
                    }

                });

            }
        }
        public string Progress
        {
            get
            {
                switch (ProgressMode)
                {
                    case ProgressModes.Load:
                        return string.Format("Загружено {0:n0} записей...", _total);
                    case ProgressModes.Test:
                        return ProcessedText(Current, Total);
                    default:
                        return "";
                }
            }
        }
        public string Duration => DurationText(_processStarted);
        public string Remaining => RemainingText(_processStarted, Current, Total);
        private string _summary;
        private string _info;

        public string Summary
        {
            get => _summary;
            set
            {
                _summary = value ?? "";
                InvokeIfRequired(() =>
                {
                    infos[InfoNames.Summary].Visible = !string.IsNullOrEmpty(_summary);
                    switch (ProgressMode)
                    {
                        case ProgressModes.Load:
                            infos[InfoNames.Summary].Text = _summary;
                            break;
                        default:
                            break;
                    }

                });
            }
        }


        public bool InProgress
        {
            get
            {
                return _progressMode == ProgressModes.Load || (_current < _total && _progressMode == ProgressModes.Test);
            }
        }

        public int StepBy { get; internal set; }

        private void InvokeIfRequired(MethodInvoker action)
        {
            _infoControl.InvokeIfRequired(action);
        }

        public void Start(int total)
        {
            _processStarted = DateTime.Now;
            _progressMode = ProgressModes.Test;
            _infoControl.ProgressMode = true;
            _current = 0;
            Total = total;
            StepBy = Math.Min(Math.Max(1, (Total / 100)), 500);
            Summary = "";
            InvokeIfRequired(() =>
            {
                _progressBar.Visible = true;
                _progressBar.Position = 0;
                _progressBar.Properties.Maximum = Total;
            });

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
            _infoControl = null;
        }

        internal void StartLoading()
        {
            _progressMode = ProgressModes.Load;

            _infoControl.ProgressMode = true;
            _current = 0;
            Total = 0;
            StepBy = 1;
            Summary = "";
            Info = "Ждите: идёт загрузка данных...";
        }

        internal void StopLoading()
        {
            InvokeIfRequired(() =>
            {
                foreach (KeyValuePair<InfoNames, InfoItem> item in infos)
                {
                    switch (item.Key)
                    {
                        case InfoNames.Summary:
                            item.Value.Visible = Total > 0;
                            item.Value.Text = Progress;
                            break;
                        default:
                            item.Value.Visible = false;
                            break;
                    }
                }
            });
            _progressMode = ProgressModes.None;
        }

        public void Stop()
        {
            if (_total == _current)
            {
                Summary = "Работа заверщена успешно";
            }
            else
            {
                Summary = "Работа остановленя";
            }
            _progressMode = ProgressModes.None;
        }

        internal void BringToFront()
        {
            _infoControl.BringToFront();
        }

    }
}
