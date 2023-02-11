using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;

namespace MediaCollector.ViewModels
{
    public abstract class AbstractViewModel<TModel> : IViewModel where TModel : class
    {
        private bool _isBusy = false;

        public event PropertyChangedEventHandler PropertyChanged;

        protected TModel Model { get; set; }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetValue(_isBusy, value);
            }
        }

        protected AbstractViewModel(TModel model)
        {
            Model = model;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void SetValue<T>(ref T backingFiled, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingFiled, value)) return;
            backingFiled = value;
            OnPropertyChanged(propertyName);
        }

        protected void SetValue<T>(T backingFiled, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingFiled, value)) return;
            backingFiled = value;
            OnPropertyChanged(propertyName);
        }
    }
}

