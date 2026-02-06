using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Shared.Enums
{
    public enum ConnectionMode
    {
        /// <summary>
        /// read/write
        /// </summary>
        Master = 0,
        /// <summary>
        /// read
        /// </summary>
        Slave = 1
    }
}
