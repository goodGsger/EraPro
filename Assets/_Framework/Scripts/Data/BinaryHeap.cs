using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public delegate bool BinaryHeapComparison<T>(T x, T y);

    public class BinaryHeap<T>
    {
        private BinaryHeapComparison<T> _comparison;
        private List<T> _list;

        public BinaryHeap(BinaryHeapComparison<T> comparison)
        {
            _comparison = comparison;
            _list = new List<T>();
            _list.Add(default(T));
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool isEmpty
        {
            get { return _list.Count == 1; }
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="t"></param>
        public void Insert(T t)
        {
            int p = _list.Count;
            int pp = p >> 1;
            _list.Add(t);
            while (p > 1 && _comparison(_list[p], _list[pp]))
            {
                T temp = _list[p];
                _list[p] = _list[pp];
                _list[pp] = temp;
                p = pp;
                pp = p >> 1;
            }
        }

        /// <summary>
        /// 弹出
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (_list.Count <= 1)
                return default(T);

            T min = _list[1];
            _list[1] = _list[_list.Count - 1];
            _list.RemoveAt(_list.Count - 1);
            int p = 1;
            int l = _list.Count;
            int sp1 = p << 1;
            int sp2 = sp1 + 1;
            while (sp1 < l)
            {
                int minP;
                if (sp2 < l)
                    minP = _comparison(_list[sp2], _list[sp1]) ? sp2 : sp1;
                else
                    minP = sp1;

                if (_comparison(_list[minP], _list[p]))
                {
                    T temp = _list[p];
                    _list[p] = _list[minP];
                    _list[minP] = temp;
                    p = minP;
                    sp1 = p << 1;
                    sp2 = sp1 + 1;
                }
                else
                    break;
            }
            return min;
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            _list.RemoveRange(1, _list.Count - 1);
        }
    }
}
