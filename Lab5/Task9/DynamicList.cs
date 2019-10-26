using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MPP9.config;
using NLog;

namespace MPP9
{
    public class DynamicList<T> : IEnumerable<T>
    {
        private Logger _logger = NLogConfiguration.GetLogger("DynamicList");
        
        private const int DefaultInitialSize = 10;
        private T[] _arr;
        private int _currentIndex = 0;

        public int Count => _currentIndex;
        public T[] Items
        {
            get
            {
                T[] result = new T[_currentIndex];
                Array.Copy(_arr, 0, result, 0, _currentIndex);
                return result;
            }
        }

        public T this[int index] => Items[index];
        
        public DynamicList(int initialSize)
        {
            
            if (initialSize > 0)
            {
                _arr = new T[initialSize];
            }
            else
            {
                throw new ArgumentException("Initial size cannot be negative or zero");
            }
        }
        
        public DynamicList()
        {
            _arr = new T[DefaultInitialSize];
        }


        public void Add(T value)
        {
            if (_currentIndex == _arr.Length)
            {
                ExtendArray();
            }

            _arr[_currentIndex++] = value;
            _logger.Info("Added new element: " + value);
        }

        public void RemoveAt(int index)
        {
            if (index >= _currentIndex) 
                throw new IndexOutOfRangeException();
            
            _logger.Info("Remove element " + _arr[index] + " with index " + index);


            var copy = new T[_arr.Length];
            _arr.CopyTo(copy, 0);
            
            Array.Copy(_arr, index + 1, copy, 
                index, _arr.Length - index - 1);
            _arr = copy;

            _currentIndex--;
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf<T>(_arr, item, 0, _currentIndex);
        }
        
        public bool Remove(T value)
        {
            int index = IndexOf(value);

            if (index < 0)
            {
                _logger.Info("Element " + value + " isn't found in list");
                return false;
            }

            RemoveAt(index);
            _logger.Info("Removed element " + value);

            return true;
        }

        public void Clear()
        {
            _logger.Info("List cleared");
            _currentIndex = 0;
        }

      

        private void ExtendArray()
        {
            var newSize = (int) Math.Round(_arr.Length * 1.5);
            _logger.Info("Exending array from " + _arr.Length + " to " + newSize);
            var newArray = new T[newSize];
            _arr.CopyTo(newArray, 0);
            _arr = newArray;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var value in Items)
            {
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}