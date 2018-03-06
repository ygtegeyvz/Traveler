using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GezelimGorelim
{
 
        /// <summary>
        /// Create a typical KDNode class.
        /// </summary>
        public class KDNode : IComparable<KDNode>, IComparable
        {
            private Points data;
            private KDNode left;
            private KDNode right;
            private int depth;

            /// <summary>
            /// Initializes a node to a point coordinate.
            /// </summary>
            /// <param name="point">The coordinates to initialize the KDNode to.</param>
            /// <param name="leftNode">The left child of the node.</param>
            /// <param name="rightNode">The right child of the node.</param>
            /// <param name="dep">The depth (or dimension) of the node.</param>
            public KDNode(Points point, KDNode leftNode = null, KDNode rightNode = null, int dep = 0)
            {
                data = point;
                left = leftNode;
                right = rightNode;
                depth = dep;
            }

            /// <summary>
            /// IComparable<T>.CompareTo method.
            /// </summary>
            /// <param name="other">A KDNode object of which we compare objects of different types to.</param>
            /// <returns>
            /// Returns 1 if the calling object is greater than other or if other is null.
            /// Returns 0 if the calling object and other are equal.
            /// Returns -1 if other is greater than the calling object.
            /// </returns>
            public int CompareTo(KDNode other)
            {
                // If other is not a valid object reference, this instance is greater. 
                if (other == null) return 1;

                // The KDNode comparison depends on the comparison of  
                // the underlying Point values, hence the .Data call.
                int mX = Data[0].CompareTo(other.Data[0]);
                int mY = Data[1].CompareTo(other.Data[1]);

                if (mX == 0 && mY == 0) return 0;
                else if (mX == 0 && mY == 1) return -1;
                else if (mX == 1 && mY == 0) return 1;
                else return 0;
            }

            /// <summary>
            /// IComparable.CompareTo method.
            /// </summary>
            /// <param name="obj">An object of which we compare objects of the same type to.</param>
            /// <returns>
            /// Returns 1 if the calling object is greater than other or if other is null.
            /// Returns 0 if the calling object and other are equal.
            /// Returns -1 if other is greater than the calling object.
            /// </returns>
            public int CompareTo(Object obj)
            {
                if (obj == null) return 1;

                KDNode node = obj as KDNode;
                if (node != null)
                {
                    int mX = this.Data[0].CompareTo(node.Data[0]);
                    int mY = this.Data[1].CompareTo(node.Data[1]);
                    if (mX == 0 && mY == 0) return 0;
                    else if (mX == 0 && mY == 1) return -1;
                    else if (mX == 1 && mY == 0) return 1;
                    else return 0;
                }
                else
                    throw new ArgumentException("Object is not a KDNode");
            }

            /// <summary>
            /// Overloading the == operator.
            /// </summary>
            /// <param name="a">A KDNode which is on the left side of the ==.</param>
            /// <param name="b">A KDNode which is on the right side of the ==.</param>
            /// <returns>
            /// Returns whether a equals b, using the .Equals method.
            /// Returns using .ReferenceEquals method if either or both a and b are null.
            /// </returns>
            public static bool operator ==(KDNode a, KDNode b)
            {
                if (object.ReferenceEquals(a, null))
                    return object.ReferenceEquals(b, null);

                return a.Equals(b);
            }

            /// <summary>
            /// Overloading the != operator.
            /// </summary>
            /// <param name="a">A KDNode which is on the left side of the !=.</param>
            /// <param name="b">A KDNode which is on the right side of the !=.</param>
            /// <returns>Returns the inverse of the == method.</returns>
            public static bool operator !=(KDNode a, KDNode b)
            {
                return !(a == b);
            }

            /// <summary>The Data property represents the coordinate points.</summary>
            /// <value>The Data property gets the coordinates of the Point field, data.</value>
            public Points Data
            {
                get { return data; }
            }

            /// <summary>The Left property represents the left child.</summary>
            /// <value>The Left property gets/sets the left child of the KDNode field, left.</value>
            public KDNode Left
            {
                get { return left; }
                set { left = value; }
            }

            /// <summary>The Right property represents the right child.</summary>
            /// <value>The Right property gets/sets the right child of the KDNode field, right.</value>
            public KDNode Right
            {
                get { return right; }
                set { right = value; }
            }

            /// <summary>The Depth property represents the depth of KDNode.</summary>
            /// <value>The Depth property gets/sets the depth (or dimension) of the int field, depth.</value>
            public int Depth
            {
                get { return depth; }
                set { depth = value; }
            }
        }
    }