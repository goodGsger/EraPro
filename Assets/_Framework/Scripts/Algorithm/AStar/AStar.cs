using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    internal delegate float Heuristic(Node startNode, Node endNode);

    public class AStar
    {
        private BinaryHeap<Node> _open;
        private Grid _grid;
        private Node _startNode;
        private Node _endNode;
        private List<Node> _path;
        private Heuristic _heuristic;
        private int _currentVersion;

        public AStar()
        {
            _open = new BinaryHeap<Node>(NodeComparison);
            _path = new List<Node>();
            _heuristic = Euclidian;
        }

        /// <summary>
        /// 网格
        /// </summary>
        public Grid grid
        {
            get { return _grid; }
            set { _grid = value; }
        }

        /// <summary>
        /// 路径
        /// </summary>
        public List<Node> path
        {
            get { return _path; }
        }

        /// <summary>
        /// 节点对比
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool NodeComparison(Node x, Node y)
        {
            return x.f < y.f;
        }

        public List<Node> Find(int startX, int startY, int endX, int endY)
        {
            if (startX < 0 || startY < 0 || startX >= _grid.numCols || startY >= _grid.numRows ||
                endX < 0 || endY < 0 || endX >= _grid.numCols || endY >= _grid.numRows)
                return null;

            // 设置起始点
            _grid.SetStartNode(startX, startY);
            // 判断目标点是否可移动
            if (_grid.GetMoveable(endX, endY) == false)
            {
                // 寻找目标点附近最近可移动点
                Node node = _grid.FindMoveableNode(startX, startY, endX, endY);
                if (node == null)
                    return null;
                else
                {
                    endX = node.x;
                    endY = node.y;
                }
            }

            // 设置结束点
            _grid.SetEndNode(endX, endY);
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
            _startNode = _grid.startNode;
            _endNode = _grid.endNode;
            _startNode.g = 0f;
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
            // 设置起始节点
            Node node = _startNode;
            node.version = _currentVersion;
            Node testNode = null;
            int sx;
            int sy;
            int ex;
            int ey;
            float g;
            float h;
            float f;
            while (node != _endNode)
            {
                sx = node.x - 1;
                if (sx < 0)
                    sx = 0;
                sy = node.y - 1;
                if (sy < 0)
                    sy = 0;
                ex = node.x + 1;
                if (ex > _grid.maxCol)
                    ex = _grid.maxCol;
                ey = node.y + 1;
                if (ey > _grid.maxRow)
                    ey = _grid.maxRow;
                // 遍历起始点周围8个点
                for (int i = sx; i <= ex; i++)
                {
                    for (int j = sy; j <= ey; j++)
                    {
                        testNode = _grid.GetNode(i, j);
                        // 对于每一个节点来说，如果它是当前节点或不可通过的，或者临接节点都不能通过，那么就跳过该节点就忽略它，直接跳到下一个
                        if (testNode == node || testNode.moveable == false)
                            continue;
                        g = node.g + 1;
                        h = _heuristic(testNode, _endNode);
                        f = g + h;
                        // 如果一个节点在待考察表/已考察表里，因为它已经被考察过了，所以我们不需要再考察。
                        // 不过这次计算出的结果有可能小于你之前计算的结果。
                        // 所以，就算一个节点在待考察表/已考察表里面，最好还是比较一下当前值和之前值之间的大小。
                        // 具体做法是比较测试节点的总代价与以前计算出来的总代价。
                        // 如果以前的大，我们就找到了更好的节点，我们就需要重新给测试点的f，g，h赋值。
                        // 同时，我们还要把测试点的父节点设为当前点。这就要我们向后追溯。
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
            Node node = _endNode;
            _path.Clear();
            _path.Add(node);
            while (node != _startNode)
            {
                node = node.parent;
                _path.Insert(0, node);
            }
        }

        /// <summary>
        /// 曼哈顿估价法(Manhattan heuristic)
        /// 它忽略所有的对角移动，只添加起点节点和终点节点之间的行、列数目。
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        /// <returns></returns>
        public static float Manhattan(Node startNode, Node endNode)
        {
            return Math.Abs(startNode.x - endNode.x) + Math.Abs(startNode.y + endNode.y);
        }

        /// <summary>
        /// 几何估价法(Euclidian heuristic)
        /// 计算出两点之间的直线距离，本质公式为勾股定理A²+B²=C²。
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        /// <returns></returns>
        public static float Euclidian(Node startNode, Node endNode)
        {
            float dx = startNode.x - endNode.x;
            float dy = startNode.y - endNode.y;
            return dx * dx + dy * dy;
        }

        /// <summary>
        /// 角线估价法(Diagonal heuristic)
        /// 三个估价方法里面最精确的，如果没有障碍，它将返回实际的消耗。
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        /// <returns></returns>
        public static float Diagonal(Node startNode, Node endNode)
        {
            float dx = Math.Abs(startNode.x - endNode.x);
            float dy = Math.Abs(startNode.y - endNode.y);
            float diag = Math.Min(dx, dy);
            float straight = dx + dy;
            return Grid.diagCost * diag + (straight - 2 * diag);
        }
    }
}
