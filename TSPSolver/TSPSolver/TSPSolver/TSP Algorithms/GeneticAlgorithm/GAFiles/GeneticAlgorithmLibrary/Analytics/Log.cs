using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace PortableGeneticAlgorithm.Analytics
{
    /// <summary>
    ///     Class to create a logfile for debugging and analytics.
    /// </summary>
    public class Log
    {
        /// <summary>
        ///     Inner class, representing an analytics line. As the analytics file will be parsed again, standardization is
        ///     important.
        /// </summary>
        public enum InfoType
        {
            /// <summary>
            ///     The next generation will be printed.
            /// </summary>
            Generation,

            /// <summary>
            ///     The current generation ended.
            /// </summary>
            GenerationEnd,

            /// <summary>
            ///     A genome will be printed.
            /// </summary>
            Genome,

            /// <summary>
            ///     A genome ended.
            /// </summary>
            GenomeEnd,

            /// <summary>
            ///     The genomes string representation.
            /// </summary>
            Process,

            /// <summary>
            ///     A list of all included activities.
            /// </summary>
            Activitylist,

            /// <summary>
            ///     The calculated fitness-value.
            /// </summary>
            Fitness,

            /// <summary>
            ///     The process' expected cashflow.
            /// </summary>
            Mue,

            /// <summary>
            ///     The process' variance, based on MUE.
            /// </summary>
            Sigma2,

            /// <summary>
            ///     The information about the current device the algo is running on
            /// </summary>
            DeviceInfo,

            /// <summary>
            ///     The process' information wheter the process is valid or not
            /// </summary>
            ValidGenome,

            /// <summary>
            ///     Indicated, that everything is finished
            /// </summary>
            Finished,
            GenomeTimeStart,
            GenomeTimeEnd,
            JSON
        }

        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffff";

        private static readonly string FilePath = "";
        private static readonly string FileName = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");

        /// <summary>
        ///     Indicates the filepath for analytics file
        /// </summary>
        public static string PathAnalytics = Path.Combine(FilePath, FileName + "_Analytics.txt");

        /// <summary>
        /// </summary>
        public static string PathDebug = Path.Combine(FilePath, FileName + "_Debug.txt");

        private static readonly Queue<string> FifoAnalytics = new Queue<string>();
        private static readonly Queue<string> FifoDebug = new Queue<string>();
        //private static readonly Thread ThreadWrite = new Thread(Write);


        /// <summary>
        ///     Writes into the debug file a new line, starting with an timestamp and a string.
        /// </summary>
        /// <param name="s">Information to append the debug-file.</param>
        public static void D(string s)
        {
            var output = "";
            output += DateTime.Now.ToString(DateTimeFormat);

            output += "\t";
            /*
            output += new StackTrace()
                .GetFrame(1).GetMethod().Name.PadRight(20, ' ');*/

            output += "\t";

            output += s;

            FifoDebug.Enqueue(output);
            /*
            if (!ThreadWrite.IsAlive)
            {
                ThreadWrite.IsBackground = true;
                ThreadWrite.Start();
            }*/
        }

        /// <summary>
        ///     Writes into the analytics file a new line, starting with an timestamp,
        ///     InfoType (please refer to AlgorithmLog.InfoType for reassembling later!) and a string.
        /// </summary>
        /// <param name="name">InfoType from AlgorithmLog.InfoTyp</param>
        /// <param name="value">Information to append the analytics-file.</param>
        public static void A(Solution s)
        {
            if (GePrModel.AnalyticsEnabled())
            {
                Analytics.SolutionQueue.Enqueue(s);
            }

            /*
            FifoAnalytics.Enqueue(output);

            if (!ThreadWrite.IsAlive)
            {
                ThreadWrite.IsBackground = true;
                ThreadWrite.Start();
            }*/
        }

        private static void Write()
        {/*
            while (true)
            {
                if (!FifoAnalytics.IsEmpty)
                    WriteInContext(FifoAnalytics, PathAnalytics);

                if (!FifoDebug.IsEmpty)
                    WriteInContext(FifoDebug, PathDebug);

                Thread.Sleep(1000);
            }*/
        }
        /*
        private static void WriteInContext(ConcurrentQueue<string> concurrentQueue, string path)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = File.Open(path, FileMode.Append, FileAccess.Write, FileShare.Read);

                // in datei schrieben
                using (var outfile = new StreamWriter(fileStream))
                {
                    string s;
                    while (concurrentQueue.TryDequeue(out s))
                        outfile.WriteLine(s);
                }
            }
            catch (Exception)
            {
                Thread.Sleep(1000);
                // passt scho
            }
            finally
            {
                fileStream?.Dispose();
            }
        }*/
    }
}