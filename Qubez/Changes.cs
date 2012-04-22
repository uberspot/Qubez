using System;
using System.Collections.Generic;
using Qubez.Core;

namespace Qubez {
    public class Changes : CircularList<Change>{
        private static Changes _changeHolder = null;
        private static readonly object padlock = new object();

        ///<summary> Default constructor </summary>
        private Changes() { }

        // Singleton.
        public static Changes GetChangeHolder
        {
            get
            {
                lock (padlock)
                {
                    if (_changeHolder == null)
                    {
                        _changeHolder = new Changes();
                    }
                    return _changeHolder;
                }
            }
        }
    }
}
