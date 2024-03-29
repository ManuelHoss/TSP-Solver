﻿using System;
using System.Security.Principal;
using PortableGeneticAlgorithm.Analytics;

namespace PortableGeneticAlgorithm
{
    public class Helper
    {
        public static Random RandomGenerator { get; } = new Random(GePrModel.GetSeed());

        /// <summary>
        ///     Returns a string of information about the device
        /// </summary>
        /// <returns>string with informations</returns>
        public static string DeviceInfo()
        {
            return /*WindowsIdentity.GetCurrent().Name + " | " +*/ " Processor Count: " + Environment.ProcessorCount + " | " +
                   Log.PathAnalytics;
        }
    }
}