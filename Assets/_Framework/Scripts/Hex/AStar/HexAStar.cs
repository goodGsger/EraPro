using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    internal delegate float HexHeuristic(HexNode startNode, HexNode endNode);

    public class HexAStar
    {
        private BinaryHeap<HexNode> _open;
        private HexGrid _grid;
        private HexNode _startNode;
        private HexNode _endNode;
        private List<HexNode> _path;
        private HexHeuristic _heuristic;
        private int _currentVersion;

        public HexAStar()
        {
            _open = new BinaryHeap<HexNode>(NodeComparison);
            _grid = new HexGrid();
            _path = new List<HexNode>();
            _heuristic = Euclidian;
        }

        /// <summary>
        /// 网格
        /// </summary>
        public HexGrid grid
        {
            get { return _grid; }
            set { _grid = value; }
        }

        /// <summary>
        /// 路径
        /// </summary>
        public List<HexNode> path
        {
            get { return _path; }
        }

        /// <summary>
        /// 寻路
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<HexNode> Find(Hex start, Hex end)
        {
            _grid.SetStartNode(start);
            _grid.SetEndNode(end);
            _startNode = _grid.startNode;
            _endNode = _grid.endNode;
            if (_startNode == null || _endNode == null || !_endNode.moveable)
                return null;

            if (FindPath())
                return _path;

            return null;
        }

        /// <summary>
        /// 寻路
        /// </summary>
        /// <returns></returns>
        private bool FindPath()
        {
            _currentVersion++;
            _open.Clear();
            _startNode.g = 0;
            _startNode.h = _heuristic(_startNode, _endNode);
            _startNode.f = _startNode.g + _startNode.h;
            return Search();
        }

        /// <summary>
        /// 路径搜寻
        /// </summary>
        /// <returns></returns>
        private bool Search()
        {
            if (_endNode.moveable == false)
                return false;
            HexNode node = _startNode;
            node.version = _currentVersion;

            float g;
            float h;
            float f;
            while (node != _endNode)
            {
                for (int i = 0; i < 6; i++)
                {
                    HexNode testNode = _grid.GetNode(node.hex.GetNeighbor(i));
                    if (testNode == null || testNode.moveable == false)
                        continue;
                    g = node.g + 1;
                    h = _heuristic(testNode, _endNode);
                    f = g + h;
                    if (testNode.version == _currentVersion)
                    {
                        if (testNode.f > f)
                        {
                            testNode.f = f;
                            testNode.g = g;
                            testNode.h = h;
                            testNode.parent = node;
                        }
                    }
                    else
                    {
                        testNode.f = f;
                        testNode.g = g;
                        testNode.h = h;
                        testNode.parent = node;
                        testNode.version = _currentVersion;
                        _open.Insert(testNode);
                    }
                }

                if (_open.isEmpty)
                    return false;

                node = _open.Pop();
            }

            BuildPath();
            return true;
        }

        /// <summary>
        /// 创建路径
        /// </summary>
        private void BuildPath()
        {
            HexNode node = _endNode;
            _path.Clear();
            _path.Add(node);
            while (node != _startNode)
            {
                node = node.parent;
                _path.Insert(0, node);
            }
        }

        /// <summary>
        /// 节点对比
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool NodeComparison(HexNode x, HexNode y)
        {
            return x.f < y.f;
        }

        /// <summary>
        /// 几何估价法(Euclidian heuristic)
        /// 计算出两点之间的直线距离，本质公式为勾股定理A²+B²=C²。
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        /// <returns></returns>
        public static float Euclidian(HexNode startNode, HexNode endNode)
        {
            return endNode.hex.GetDistance(startNode.hex);
        }
    }
}
