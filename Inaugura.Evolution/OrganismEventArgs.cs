using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inaugura.Evolution
{
    /// <summary>
    /// The base class for classes containing organism event arguments
    /// </summary>
    public class OrganismEventArgs: EventArgs
    {
        #region Properties
        /// <summary>
        /// The organism
        /// </summary>
        public Organism Organism{get; private set;}
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="organism">The organism</param>
        public OrganismEventArgs(Organism organism)
        {
            Organism = organism;
        }
        #endregion
    }
}
