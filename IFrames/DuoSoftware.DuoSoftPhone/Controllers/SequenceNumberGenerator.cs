using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.Collections;
using System.Linq;
using System.Threading;

namespace DuoSoftware.DuoSoftPhone.Controllers
{
    /// <summary>
    /// Sequence Number Generator
    /// </summary>
    public sealed class SequenceNumberGenerator
    {
        #region Local Variables

        /// <summary>
        /// local instance
        /// </summary>
        private static volatile SequenceNumberGenerator _instance;
        /// <summary>
        /// root msg
        /// </summary>
        private static readonly object SyncRoot = new Object();

        /// <summary>
        /// no list
        /// </summary>
        private readonly SortedList _noList;
        /// <summary>
        /// end
        /// </summary>
        private int _end = 1000;
        /// <summary>
        /// is loading 
        /// </summary>
        private bool _isLoading;

        #endregion Local Variables

        #region Properties

        #endregion Properties

        #region Constructors

        /// <summary>
        /// SequenceNumberGenerator
        /// </summary>
        private SequenceNumberGenerator()
        {
            try
            {
                _noList = new SortedList(5000);
                GenerateNos();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "SequenceNumberGenerator", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        #endregion Constructors

        #region Interface Implementations

        #endregion Interface Implementations

        #region Events

        #endregion Events

        #region Methods

        #region Internal Methods

        #endregion Internal Methods

        #region Private Methods

        //public  void PrintKeysAndValues(SortedList myList)
        //{
        //    Console.WriteLine("\t-KEY-\t-VALUE-");
        //    for (int i = 0; i < myList.Count; i++)
        //    {
        //        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, String.Format("\t{0}:\t{1}", myList.GetKey(i), myList.GetByIndex(i)), Logger.LogLevel.Debug);

        //    }
        //    Console.WriteLine();
        //}

        /// <summary>
        /// GenerateNos
        /// </summary>
        private void GenerateNos()
        {
            try
            {
                _isLoading = true;
                _end = _end + 2000;
                var nos = Enumerable.Range(_end - 1999, ++_end);

                lock (_noList)
                {
                    foreach (var n in nos)
                    {
                        _noList.Add(n, n);
                    }
                    //nos.AsParallel().ForAll(n => noList.Add(n, n));
                }
                _isLoading = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GenerateNos", exception, Logger.LogLevel.Error);
            }
        }

        #endregion Private Methods

        #region protected Methods

        #endregion protected Methods

        #region Public Methods

        /// <summary>
        /// create Instance
        /// </summary>
        public static SequenceNumberGenerator Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new SequenceNumberGenerator();
                }
                return _instance;
            }
        }

        /*
        public int GetNextNo()
        {
            try
            {
                if (_noList.Count <= 500)
                    if (!_isLoading)
                        new Thread(GenerateNos).Start();
                var id = _noList.GetByIndex(0);
                lock (_noList)
                {
                    _noList.RemoveAt(0);
                }

                return (int)id;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetNextNo", exception, Logger.LogLevel.Error);
                _errCount++;
                return -999;
            }
        }

        */
        public int GetNextNo
        {
            get
            {
                try
                {
                    if (_noList.Count <= 500)
                        if (!_isLoading)
                            new Thread(GenerateNos).Start();
                    
                    lock (_noList)
                    {
                        var id = _noList.GetByIndex(0);
                        _noList.RemoveAt(0);
                        return (int)id;
                    }
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetNextNo", exception, Logger.LogLevel.Error);
                    
                    throw;
                }
            }
        }

        #endregion Public Methods

        #endregion Methods
    }
}