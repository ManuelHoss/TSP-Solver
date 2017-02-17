using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TSPSolver.Model;
using Xamarin.Forms;

namespace TSPSolver.ViewModels
{
   public class BestRouteDetailViewModel : BaseViewModel
   {
      private List<Address> _addresses;
      private Dictionary<Address, Dictionary<Address, double>> _distanceMatrix;
      private Dictionary<Address, Dictionary<Address, double>> _durationMatrix;
      private TimeSpan _startTimeOfRoute;

      public BestRouteDetailViewModel(Page page, Route bestRoute) : base(page)
      {
         Addresses = bestRoute.Addresses.ToList();
         DistanceMatrix = bestRoute.DistanceMatrix;
         DurationMatrix = bestRoute.DurationMatrix;
      }

      public List<Address> Addresses
      {
         get { return _addresses; }
         set { SetProperty(ref _addresses, value); }
      }

      public Dictionary<Address, Dictionary<Address, double>> DistanceMatrix
      {
         get { return _distanceMatrix; }
         set { SetProperty(ref _distanceMatrix, value); }
      }

      public Dictionary<Address, Dictionary<Address, double>> DurationMatrix
      {
         get { return _durationMatrix; }
         set { SetProperty(ref _durationMatrix, value); }
      }

      public TimeSpan StartTimeOfRoute
      {
         get { return _startTimeOfRoute; }
         set { SetProperty(ref _startTimeOfRoute, value); }
      }

      #region Commands

      #region UpdateArrivalTimesCommand

      private Command _updateArrivalTimesCommand { get; set; }

      public ICommand UpdateArrivalTimesCommand
      {
         get
         {
            return _updateArrivalTimesCommand ??
                   (_updateArrivalTimesCommand =
                      new Command(() =>
                      {
                         DateTime tempTimeSpan = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, StartTimeOfRoute.Hours,StartTimeOfRoute.Minutes,0);
                         Addresses[0].ArrivalTime = StartTimeOfRoute;
                         for (int i = 0; i < Addresses.Count - 1; i++)
                         {
                            tempTimeSpan = tempTimeSpan.AddSeconds(DurationMatrix[Addresses[i]][Addresses[i + 1]]);
                            Addresses[i + 1].ArrivalTime = tempTimeSpan.TimeOfDay;
                         }
                         OnPropertyChanged("Addresses");
                      }));
         }
      }

      #endregion //UpdateArrivalTimesCommand

      #endregion //Commands
   }
}
