using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GezelimGorelim
{
        /// <summary>
        /// Represent a data type where elements with higher priority are "served" before elements with lower priority.
        /// Elements of same priority are served with random order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class MinPriorityQueue<T>
        {
            private List<T> dataHeap;
            private readonly Comparer<T> comp;

            /// <summary>
            /// Initializes a new instance of the Priority Queue that is empty.
            /// Declares an explicit default Comparer to handle comparisons between classes.
            /// </summary>
            public MinPriorityQueue() : this(Comparer<T>.Default) { }

            public MinPriorityQueue(Comparer<T> comp)
            {
                this.dataHeap = new List<T>();
                this.comp = comp;
            }

            /// <summary>
            /// Adds an element to the queue and then heapifies so that the element with highest priority is at front.
            /// </summary>
            /// <param name="value">The element to enqueue.</param>
            public void Enqueue(T value)
            {
                this.dataHeap.Add(value);
                BubbleUp();
            }

            /// <summary>
            /// Removes the element at the front and returns it.
            /// </summary>
            /// <returns>The element that is removed from the queue front.</returns>
            /// <exception cref="InvalidOperationException">The Queue is empty.</exception>
            public T Dequeue()
            {
                if (this.dataHeap.Count <= 0)
                {
                    throw new InvalidOperationException("Cannot Dequeue from empty queue!");
                }

                T result = dataHeap[0];
                int count = this.dataHeap.Count - 1;
                dataHeap[0] = dataHeap[count];
                dataHeap.RemoveAt(count);
                ShiftDown();

                return result;
            }

            /// <summary>
            /// A method to maintain the heap order of the elements after enqueue. If the parent of the newly added 
            /// element is with less priority - swap them.
            /// </summary>
            private void BubbleUp()
            {
                int childIndex = dataHeap.Count - 1;

                while (childIndex > 0)
                {
                    int parentIndex = (childIndex - 1) / 2;

                    if (comp.Compare(dataHeap[childIndex], dataHeap[parentIndex]) >= 0)
                    {
                        break;
                    }

                    SwapAt(childIndex, parentIndex);
                    childIndex = parentIndex;
                }
            }

            /// <summary>
            /// A method to maintain the heap order of the elements after denqueue. We check priorities of both children and parent node.
            /// </summary>
            private void ShiftDown()
            {
                int count = this.dataHeap.Count - 1;
                int parentIndex = 0;

                while (true)
                {
                    int childIndex = parentIndex * 2 + 1;
                    if (childIndex > count)
                    {
                        break;
                    }

                    int rightChild = childIndex + 1;
                    if (rightChild <= count && comp.Compare(dataHeap[rightChild], dataHeap[childIndex]) < 0)
                    {
                        childIndex = rightChild;
                    }
                    if (comp.Compare(dataHeap[parentIndex], dataHeap[childIndex]) <= 0)
                    {
                        break;
                    }

                    SwapAt(parentIndex, childIndex);
                    parentIndex = childIndex;
                }
            }

            /// <summary>Returns the element at the front of the Priority Queue without removing it.</summary>
            /// <returns>The element at the front of the queue.</returns>
            /// <exception cref="InvalidOperationException">The Queue is empty.</exception>
            public T Peek()
            {
                if (this.dataHeap.Count == 0)
                {
                    throw new InvalidOperationException("Queue is empty.");
                }

                T frontItem = dataHeap[0];
                return frontItem;
            }

            /// <summary>
            /// Gets the number of elements currently contained in the <see cref="PriorityQueue"/>
            /// </summary>
            /// <returns>The number of elements contained in the <see cref="PriorityQueue"/></returns>
            public int Count()
            {
                return dataHeap.Count;
            }

            /// <summary>Removes all elements from the queue.</summary>
            public void Clear()
            {
                this.dataHeap.Clear();
            }

            /// <summary>Copies the queue elements to an existing array, starting at the specified index.</summary>
            /// <exception cref="ArgumentNullException">Array is null. </exception>
            /// <exception cref="IndexOutOfRangeException">Index is less than zero or bigger than array length. </exception>
            /// <exception cref="ArgumentException">The number of elements int the source is greater than the available space.</exception>
            public void CopyToArray(T[] array, int index)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("Array");
                }

                int length = array.Length;
                if (index < 0 || index >= length)
                {
                    throw new IndexOutOfRangeException("Index must be between zero and array length.");
                }
                if (length - index < this.dataHeap.Count - 1)
                {
                    throw new ArgumentException("Queue is bigger than array");
                }

                T[] data = this.dataHeap.ToArray();
                Array.Copy(data, 0, array, index, data.Length);
            }

            /// <summary>
            /// Checks the consistency of the heap.
            /// </summary>
            /// <returns>True if the heap property is ok.</returns>
            public bool IsConsistent()
            {
                if (dataHeap.Count == 0)
                {
                    return true;
                }

                int lastIndex = dataHeap.Count - 1;
                for (int parentIndex = 0; parentIndex < dataHeap.Count; ++parentIndex)
                {
                    int leftChildIndex = 2 * parentIndex + 1;
                    int rightChildIndex = 2 * parentIndex + 2;

                    if (leftChildIndex <= lastIndex && comp.Compare(dataHeap[parentIndex], dataHeap[leftChildIndex]) > 0)
                    {
                        return false;
                    }
                    if (rightChildIndex <= lastIndex && comp.Compare(dataHeap[parentIndex], dataHeap[rightChildIndex]) > 0)
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// A method that swaps the elements at the given indices of the heap.
            /// </summary>
            /// <param name="first">The first element index.</param>
            /// <param name="second">The second element index.</param>
            private void SwapAt(int first, int second)
            {
                T value = dataHeap[first];
                dataHeap[first] = dataHeap[second];
                dataHeap[second] = value;
            }

            public override string ToString()
            {
                string queueString = string.Join("\n", dataHeap.ToArray());
                return queueString;
            }
        }
    }

