using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LinkedList
{
    public class LinkedListEnumerator<T> : IEnumerator<T>
    {
        private ListNode<T> current;

        public LinkedListEnumerator(ListNode<T> current)
        {
            ListNode<T> dummy = new ListNode<T>(default(T));
            dummy.Next = current;
            this.current = dummy;
        }

        public T Current => current.Data;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (current == null)
            {
                return false;
            }
            current = current.Next;

            return (current != null);
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }

    public class ListNode<T>
    {
        public ListNode<T> Next { get; set; }
        public ListNode<T> Prev { get; set; }
        public T Data { get; set; }
        public ListNode(T Data)
        {
            this.Data = Data;
            Next = Prev = null;
        }
    }

    public class LinkedList<T> : IEnumerable<T>
    {
        private ListNode<T> head, tail;
        public int Size { get; private set; }

        public LinkedList()
        {
            head = tail = null;
        }

        /// <summary>
        /// Adding from front
        /// </summary>
        /// <param name="data"></param>
        private void AddFirst(T data)
        {
            ListNode<T> newData = new ListNode<T>(data);
            if (head == tail && head == null)
            {
                head = tail = newData;
            }
            else
            {
                newData.Next = head;
                head.Prev = newData;
                head = newData;
            }
            Size++;
        }

        /// <summary>
        /// Adding from end
        /// </summary>
        /// <param name="data"></param>
        public void AddLast(T data)
        {
            ListNode<T> newData = new ListNode<T>(data);
            if (head == tail && head == null)
            {
                head = tail = newData;
            }
            else
            {
                newData.Prev = tail;
                tail.Next = newData;
                tail = newData;
            }
            Size++;
        }

        /// <summary>
        /// Adding by Index (0 indexed)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        public void Insert(T data, int index)
        {
            if (index < 0 || index > Size)
                return;
            if (index == 0)
            {
                AddFirst(data);
            }
            else if (index == Size)
            {
                AddLast(data);
            }
            else
            {
                ListNode<T> currentNode = head, newNode = new ListNode<T>(data);
                while (currentNode != null && index-- > 1)
                {
                    currentNode = currentNode.Next;
                }
                ListNode<T> nextNode = currentNode.Next;
                currentNode.Next = newNode;
                nextNode.Prev = newNode;
                newNode.Next = nextNode;
                newNode.Prev = currentNode;
                Size++;
            }
        }

        /// <summary>
        /// Deleting from front
        /// </summary>
        private void DeleteFirst()
        {
            if (head == null)
                return;

            if(head == tail)
            {
                head = tail = null;
            }
            else
            {
                head = head.Next;
            }
            Size--;
        }

        /// <summary>
        /// Deleting from end
        /// </summary>
        private void DeleteLast()
        {
            if (tail == null)
                return;

            if (head == tail)
            {
                head = tail = null;
            }
            else
            {
                tail = tail.Prev;
            }
            Size--;
        }

        /// <summary>
        /// Deleting by Index
        /// </summary>
        /// <param name="index"></param>
        public void DeleteByIndex(int index)
        {
            if (index < 0 || index >= Size)
                return;

            if (index == 0)
            {
                DeleteFirst();
            }
            else if (index == Size - 1)
            {
                DeleteLast();
            }
            else
            {
                ListNode<T> currentNode = head;
                while (currentNode != null && index-- > 0)
                {
                    currentNode = currentNode.Next;
                }
                currentNode.Prev.Next = currentNode.Next;
                currentNode.Next.Prev = currentNode.Prev;
                Size--;
            }
        }
        /// <summary>
          /// Get Element of type T by Index
          /// </summary>
          /// <param name="index"></param>
          /// <returns></returns>
        public T GetByIndex(int index)
        {
            if (index < 0 || index >= Size)
                return default(T);

            ListNode<T> currentNode = head;
            while (currentNode != null && index-- > 0)
            {
                currentNode = currentNode.Next;
            }
            return currentNode.Data;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LinkedListEnumerator<T>(head);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Print()
        {
            Console.WriteLine("Elements: ");
            foreach(T data in this)
            {
                Console.Write("{0} ", data);
            }
            Console.WriteLine();
        }

        private void Swap(ListNode<T> leftNode, ListNode<T> rightNode)
        {
            T data = leftNode.Data;
            leftNode.Data = rightNode.Data;
            rightNode.Data = data;
        }

        private ListNode<T> Partition(ListNode<T> headNode, ListNode<T> tailNode)
        {
            T data = tailNode.Data;
            ListNode<T> leftNode = headNode, rightNode = headNode;
            while(rightNode != tailNode)
            {
                if(Comparer<T>.Default.Compare(rightNode.Data, data) < 0)
                {
                    Swap(leftNode, rightNode);
                    leftNode = leftNode.Next;
                }
                rightNode = rightNode.Next;
            }
            Swap(leftNode, tailNode);
            return leftNode;
        }

        private void QuickSort(ListNode<T> headNode, ListNode<T> tailNode)
        {
            if(headNode != null && headNode != tailNode && headNode != tailNode.Next)
            {
                ListNode<T> pivotNode = Partition(headNode, tailNode);
                QuickSort(headNode, pivotNode.Prev);
                QuickSort(pivotNode.Next, tailNode);
            }
        }

        public void Sort()
        {
            QuickSort(head, tail);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            LinkedList<int> linkedList = new LinkedList<int>();
            linkedList.AddLast(3);
            linkedList.AddLast(1);
            linkedList.AddLast(2);
            linkedList.AddLast(5);
            linkedList.AddLast(4);
            Console.WriteLine("Size: {0}", linkedList.Size);
            linkedList.Print();
            linkedList.Insert(10, 0);
            linkedList.Insert(20, 6);
            Console.WriteLine("Size: {0}", linkedList.Size);
            linkedList.Print();
            linkedList.DeleteByIndex(0);
            linkedList.DeleteByIndex(2);
            linkedList.Print();
            Console.WriteLine("Element at index 0: {0}", linkedList.GetByIndex(0));
            Console.WriteLine("Element at index 1: {0}", linkedList.GetByIndex(1));
            Console.WriteLine("Element at index 3: {0}", linkedList.GetByIndex(3));
            Console.WriteLine("After Sorting:");
            linkedList.Sort();
            linkedList.Print();
            Console.WriteLine("Size: {0}", linkedList.Size);
        }
    }
}
