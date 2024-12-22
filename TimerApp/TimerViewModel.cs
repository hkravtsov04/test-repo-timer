using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace TimerApp
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        private readonly TimerFacade _timerFacade;
        private string _displayTime;
        private TimeSpan _selectedTime;
        private bool _isRunning;

        public string DisplayTime
        {
            get => _displayTime;
            private set
            {
                if (_displayTime != value)
                {
                    _displayTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanSetTime));
                }
            }
        }

        public bool CanSetTime => !IsRunning;

        public int Hours
        {
            get => _selectedTime.Hours;
            set
            {
                if (CanSetTime)
                {
                    _selectedTime = new TimeSpan(value, _selectedTime.Minutes, _selectedTime.Seconds);
                    UpdateDisplayTime();
                }
            }
        }

        public int Minutes
        {
            get => _selectedTime.Minutes;
            set
            {
                if (CanSetTime)
                {
                    _selectedTime = new TimeSpan(_selectedTime.Hours, value, _selectedTime.Seconds);
                    UpdateDisplayTime();
                }
            }
        }

        public int Seconds
        {
            get => _selectedTime.Seconds;
            set
            {
                if (CanSetTime)
                {
                    _selectedTime = new TimeSpan(_selectedTime.Hours, _selectedTime.Minutes, value);
                    UpdateDisplayTime();
                }
            }
        }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand ResetCommand { get; }

        public TimerViewModel()
        {
            _timerFacade = new TimerFacade();
            _selectedTime = TimeSpan.Zero;
            UpdateDisplayTime();

            StartCommand = new Command(async () =>
            {
                IsRunning = true;
                await _timerFacade.StartAsync(
                    time => DisplayTime = time,
                    OnTimerComplete);
            });

            StopCommand = new Command(() =>
            {
                _timerFacade.Stop();
                IsRunning = false;
            });

            ResetCommand = new Command(() =>
            {
                _timerFacade.Reset();
                UpdateDisplayTime();
                IsRunning = false;
            });
        }

        private void UpdateDisplayTime()
        {
            _timerFacade.SetTime(_selectedTime);
            DisplayTime = _timerFacade.GetCurrentTime();
        }

        private async void OnTimerComplete()
        {
            IsRunning = false;
            await Application.Current.MainPage.DisplayAlert("Timer", "Time's up!", "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}