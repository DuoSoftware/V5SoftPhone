using System;

namespace DuoSoftware.DuoSoftPhone.Controllers.Common
{
    /// <summary>
    ///
    /// </summary>
    public static class ComMethods
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        public class SwitchOnType<T1>
        {
            /// <summary>
            ///
            /// </summary>
            private bool _break = false;

            /// <summary>
            ///
            /// </summary>
            private T1 _object;

            /// <summary>
            ///
            /// </summary>
            /// <param name="obj"></param>
            public SwitchOnType(T1 obj)
            {
                _object = obj;
            }

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="T2"></typeparam>
            /// <param name="action"></param>
            /// <returns></returns>
            public SwitchOnType<T1> Case<T2>(Action<T2> action) where T2 : class
            {
                if (action == null) return null;
                if (_break) return this as SwitchOnType<T1>;
                var t = _object as T2;
                if (t == null) return this as SwitchOnType<T1>;
                action(t);
                _break = true;
                return this as SwitchOnType<T1>;
            }

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="T2"></typeparam>
            /// <param name="action"></param>
            public void Default<T2>(Action<T2> action) where T2 : class
            {
                if (action == null) return;
                if (_break) return;
                var t = _object as T2;
                if (t != null)
                    action(t);
            }
        }
    }
}