﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using TSPSolver.Interfaces;
using TSPSolver.UWP.PlatformSpecific;

[assembly: Xamarin.Forms.Dependency(typeof(GeocoderUwp))]
namespace TSPSolver.UWP.PlatformSpecific
{
   public class GeocoderUwp : IGeocoder
   {
      public async Task<List<double>> GetGeoCoordinatesOfAddress(string address)
      {
         // The address or business to geocode.
         string addressToGeocode = "Microsoft";

         // The nearby location to use as a query hint.
         BasicGeoposition queryHint = new BasicGeoposition();
         queryHint.Latitude = 47.643;
         queryHint.Longitude = -122.131;
         Geopoint hintPoint = new Geopoint(queryHint);

         // Geocode the specified address, using the specified reference point
         // as a query hint. Return no more than 3 results.
         MapLocationFinderResult result =
               await MapLocationFinder.FindLocationsAsync(
                                 addressToGeocode,
                                 hintPoint,
                                 3);

         // If the query returns results, display the coordinates
         // of the first result.
         if (result.Status == MapLocationFinderStatus.Success)
         {
            return new List<double>() { result.Locations[0].Point.Position.Latitude , result.Locations[0].Point.Position.Longitude };
         }
         else
         {
            return new List<double>();
         }
      }
   }
}
