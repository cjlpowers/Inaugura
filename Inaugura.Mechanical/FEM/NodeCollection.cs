using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Mechanical.FEM
{
    /// <summary>
    /// A class which represents a collection of nodes
    /// </summary>
    public class NodeCollection
    {
        #region Variables
        private Node[] mNodes;
        #endregion

        #region Properties
        /// <summary>
        /// The eastern node
        /// </summary>
        public Node East
        {
            get
            {
                return mNodes[0];
            }
            set
            {
                mNodes[0] = value;
            }
        }

        /// <summary>
        /// The western node
        /// </summary>
        public Node West
        {
            get
            {
                return mNodes[1];
            }
            set
            {
                mNodes[1] = value;
            }
        }

        /// <summary>
        /// The nothern node
        /// </summary>
        public Node North
        {
            get
            {
                return mNodes[2];
            }
            set
            {
                mNodes[2] = value;
            }
        }

        /// <summary>
        /// The southern node
        /// </summary>
        public Node South
        {
            get
            {
                return mNodes[3];
            }
            set
            {
                mNodes[3] = value;
            }
        }

        /// <summary>
        /// The Top node
        /// </summary>
        public Node Top
        {
            get
            {
                return mNodes[4];
            }
            set
            {
                mNodes[4] = value;
            }
        }

        /// <summary>
        /// The bottom node
        /// </summary>
        public Node Bottom
        {
            get
            {
                return mNodes[5];
            }
            set
            {
                mNodes[5] = value;
            }
        }
        #endregion
    }
}
