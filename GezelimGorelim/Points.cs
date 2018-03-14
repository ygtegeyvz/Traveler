using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GezelimGorelim
{
    


    
        /// <summary>
        /// Create a typical Point class.
        /// </summary>
        public class Points : IComparable<Points>, IComparable
        {
            // TODO: create a an array to N dimensions, rather than a fixed 2D class.
            public double latitude;
            public double longitude;


        ///İŞE YARAMAZ MUHTEMELEN
            /// <summary>
            /// Initializes a Point object to some coordinates.
            /// </summary>
            /// <param name="latCoord">Represents the x coordinate.</param>
            /// <param name="longCoord">Represents the y coordinate.</param>
            public Points(double latCoord, double longCoord)
            {
                SetPoint(latCoord, longCoord);
            }

            /// <summary>
            /// IComparable<T>.CompareTo method.
            /// </summary>
            /// <param name="other">A Point object of which we compare objects of different types to.</param>
            /// <returns>
            /// Returns 1 if the calling object is greater than other or if other is null.
            /// Returns 0 if the calling object and other are equal.
            /// Returns -1 if other is greater than the calling object.
            /// </returns>
            public int CompareTo(Points other)
            {
                // If other is not a valid object reference, this instance is greater. 
                if (other == null) return 1;

                // The temperature comparison depends on the comparison of  
                // the underlying Double values.  
                int mX = latitude.CompareTo(other.latitude);
                int mY = longitude.CompareTo(other.longitude);

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

                Points p = obj as Points;
                if (p != null)
                {
                    int mX = this.latitude.CompareTo(p.latitude);
                    int mY = this.longitude.CompareTo(p.longitude);
                    if (mX == 0 && mY == 0) return 0;
                    else if (mX == 0 && mY == 1) return -1;
                    else if (mX == 1 && mY == 0) return 1;
                    else return 0;
                }
                else
                    throw new ArgumentException("Object is not a Temperature");
            }

           

            /// <summary>
            /// Defines an indexer in order to access the coordinate values.
            /// </summary>
            /// <param name="index">Represents the index of the coordinate values.</param>
            /// <returns>Returns the value at index.</returns>
            /// <exception cref="IndexOutOfRangeException">If index is out of range.</exception>
            public double this[int index]
            {
                get
                {
                    if (index == 0) return latitude;
                    else if (index == 1) return longitude;
                    else throw new System.IndexOutOfRangeException("index " + index + " is out of range");
                }
                set
                {
                    if (index == 0) latitude = value;
                    else if (index == 1) longitude = value;
                    else throw new System.IndexOutOfRangeException("index " + index + " is out of range");
                }

            }

            /// <summary>
            /// A setter method to set the coordinates.
            /// </summary>
            /// <param name="xCoord">Represents the x coordinate.</param>
            /// <param name="yCoord">Represents the y coordinate.</param>
            public void SetPoint(double latCoord, double longCoord)
            {
            latitude = latCoord;
            longitude = longCoord;
            }

        }
    }

