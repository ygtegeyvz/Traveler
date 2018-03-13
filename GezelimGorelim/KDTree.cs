using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace GezelimGorelim
{
    /// <summary>
    /// Create KDTree class of N dimensions.
    /// As of now, KDTree supports only two dimensions.
    /// </summary>
    public class KDTree
    {
        public static KDNode root;
        private int dimensions;
        public Queue<KDNode> queue;

        /// <summary>
        /// Initializes a root node and number of dimensions.
        /// </summary>
        /// <param name="point">The coordinates to initialize the root node to.</param>
        /// <param name="dim">The number of dimensions of the KDTree.</param>
        public KDTree(Points point, int dim)
        {
            root = new KDNode(point, null, null, 0);
            dimensions = dim;
            queue = new Queue<KDNode>();
            queue.Enqueue(root);
        }

        /// <summary>The Root property represents the root KDNode.</summary>
        /// <value>The Root property gets/sets the value of the KDNode field, root.</value>
        public KDNode Root
        {
            get { return root; }
            set { root = value; }
        }

        /// <summary>The Dim property represents the dimensions of KDTree.</summary>
        /// <value>The Dim property gets the value of int field, dimensions.</value>
        public int Dim
        {
            get { return dimensions; }
        }

        /// <summary>
        /// Prints all elements in the entire tree in pre-order.
        /// </summary>
        /// <param name="node">The node at which we traverse the tree.</param>
            List<KDNode> kDNodes = new List<KDNode>(); KDNode near, far;
        public List<KDNode> RangeSearch(KDNode currentNode,Points nokta1,Points nokta2)
        {
            if (currentNode.Left != null) RangeSearch(currentNode.Left,nokta1,nokta2);
            if (currentNode.Right != null) RangeSearch(currentNode.Right,nokta1,nokta2);
            if ((nokta1.latitude < currentNode.data.latitude && currentNode.data.latitude < nokta2.latitude &&
             nokta1.longitude < currentNode.data.longitude && currentNode.data.longitude < nokta2.longitude)
             ||
             (nokta1.latitude > currentNode.data.latitude && currentNode.data.latitude > nokta2.latitude &&
             nokta1.longitude < currentNode.data.longitude && currentNode.data.longitude < nokta2.longitude)
             ||
             (nokta1.latitude < currentNode.data.latitude && currentNode.data.latitude < nokta2.latitude &&
             nokta1.longitude > currentNode.data.longitude && currentNode.data.longitude > nokta2.longitude)
             ||
             (nokta1.latitude > currentNode.data.latitude && currentNode.data.latitude > nokta2.latitude &&
             nokta1.longitude > currentNode.data.longitude && currentNode.data.longitude > nokta2.longitude))
            {
                kDNodes.Add(currentNode);
                near = currentNode.Left;
                far = currentNode.Right;
            }
            else
            {

                near = currentNode.Right;
                far = currentNode.Left;
            }
            return kDNodes;
        }   
      
        /// <summary>
        /// Calculates the euclidean distance between two points (distance formula).
        /// </summary>
        /// <param name="a">The parameter a represents the first point.</param>
        /// <param name="a">The parameter b  represents the second point.</param>
        /// <returns>The square root of the squared distances between the two points.</returns>
        public static double EuclideanDistance(Points a, Points b)
        {
            return Math.Sqrt(Math.Pow((a[1] - b[0]), 2) + Math.Pow((b[1] - a[0]), 2));
        }

        /// <summary>
        /// A private method to calculate the difference between two points at a given dimension.
        /// This method is essential to NNSearch's functions.
        /// </summary>
        /// <param name="a">The parameter a represents the first point.</param>
        /// <param name="a">The parameter b represents the second point.</param>
        /// <returns>The difference between two points at a given dimension.</returns>
        private static double Subtract(Points a, Points b, int dim)
        {
            return a[dim] - b[dim];
        }

        /// <summary>
        /// Recursive insertion method to add nodes to the KDTree. 
        /// Translated from http://users.dcc.uchile.cl/~rbaeza/handbook/algs/3/352.ins.c
        /// </summary>
        /// <param name="node">The node we insert into the tree.</param>
        /// <param name="root">The root at which we initiate the insertion from.</param>
        /// <param name="lev">The level (or dimension) of the node.</param>
        /// <returns>The root node is returned in order to continue with recursion.</returns>
        public KDNode Insert(KDNode node, KDNode root, int lev = 0)
        {
            if (root == null) { node.Depth = lev % Dim; return node; }

            if (lev == 0)
            {
                if (node.Data[lev] > root.Data[lev])
                    root.Right = Insert(node, root.Right, (lev + 1) % Dim);

                else if (node.Data[lev] < root.Data[lev])
                    root.Left = Insert(node, root.Left, (lev + 1) % Dim);
                else
                    return root;
            }
            else
            {
                if (node.Data[lev] > root.Data[lev])
                    root.Right = Insert(node, root.Right, (lev + 1) % Dim);

                else if (node.Data[lev] < root.Data[lev])
                    root.Left = Insert(node, root.Left, (lev + 1) % Dim);
                else
                    return root;
            }

            return root;
        }

     

     

    }
}

