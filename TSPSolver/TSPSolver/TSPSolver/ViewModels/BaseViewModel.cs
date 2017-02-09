using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace TSPSolver.ViewModels
{
   public class BaseViewModel : INotifyPropertyChanged
   {
      #region Fields

      internal readonly Page Page;

      #endregion // Fields

      #region Constructor

      public BaseViewModel(Page page)
      {
         this.Page = page;
      }

      #endregion // Constructor

      #region Properties

      #endregion // Properties

      #region Protected Methods

      protected void SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "",
          Action onChanged = null)
      {
         if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return;

         backingStore = value;

         if (onChanged != null)
            onChanged();

         OnPropertyChanged(propertyName);
      }

      #endregion

      #region INotifyPropertyChanged implementation

      public event PropertyChangedEventHandler PropertyChanged;

      public void OnPropertyChanged(string propertyName)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      #endregion
   }
}