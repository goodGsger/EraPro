using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class HexGrid
    {
        private HexNode _startNode;
        private HexNode _endNode;
        private Dictionary<Hex, HexNode> _nodes;

        public HexGrid()
        {
            _nodes = new Dictionary<Hex, HexNode>();
        }

        /// <summary>
        /// 起始点
        /// </summary>
        public HexNode startNode
        {
            get { return _startNode; }
        }

        /// <summary>
        /// 结束点
        /// </summary>
        public HexNode endNode
        {
            get { return _endNode; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _startNode = null;
            _endNode = null;
            _nodes.Clear();
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="moveable"></param>
        public void AddNode(Hex hex, bool moveable)
        {
            _nodes[hex] = new HexNode(hex, moveable);
        }

        /// <summary>
        /// 移除节点
        /// </summary>
        /// <param name="hex"></param>
        public void RemoveNode(Hex hex)
        {
            _nodes.Remove(hex);
        }

        /// <summary>
        /// 是否存在节点
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public bool HasNode(Hex hex)
        {
            return _nodes.ContainsKey(hex);
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public HexNode GetNode(Hex hex)
        {
            if (_nodes.TryGetValue(hex, out HexNode node))
                return node;
            return null;
        }

        /// <summary>
        /// 设置起始节点
        /// </summary>
        /// <param name="node"></param>
        public void SetStartNode(HexNode node)
        {
            _startNode = node;
        }

        /// <summary>
        /// 设置起始节点
        /// </summary>
        /// <param name="hex"></param>
        public void SetStartNode(Hex hex)
        {
            _startNode = GetNode(hex);
        }

        /// <summary>
        /// 设置结束节点
        /// </summary>
        /// <param name="node"></param>
        public void SetEndNode(HexNode node)
        {
            _endNode = node;
        }

        /// <summary>
        /// 设置结束节点
        /// </summary>
        /// <param name="hex"></param>
        public void SetEndNode(Hex hex)
        {
            _endNode = GetNode(hex);
        }

        /// <summary>
        /// 获取节点是否可行走
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public bool GetWalkable(Hex hex)
        {
            return GetNode(hex).moveable;
        }

        /// <summary>
        /// 设置节点是否可行走
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="walkable"></param>
        public void SetWalkable(Hex hex, bool walkable)
        {
            GetNode(hex).moveable = walkable;
        }
    }
}
