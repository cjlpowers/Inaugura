using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Maps
{
    /// <summary>
    /// A interface for object which may be geocoded
    /// </summary>
    public interface IGeocode
    {
        /// <summary>
        /// The latitude
        /// </summary>
        double Latitude
        {
            get;
            set;
        }

        /// <summary>
        /// The longitude
        /// </summary>
        double Longitude
        {
            get;
            set;
        }        
    }
}
