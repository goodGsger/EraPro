using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class ActionView : IActionView
    {
        protected int _type;
        protected int _action;
        protected ActionDirection _direction;
        protected int _frameIndex;
        protected int _maxFrameIndex = -1;
        protected string _sortingLayer;
        protected int _sortingIndex;
        protected int _sortingOrder = int.MinValue;
        protected int[] _frames;
        protected int _depth;
        protected bool _loop;
        protected float _alpha = 1f;
        protected ColorFilter _colorFilter;

        protected Action _callback;
        protected Dictionary<int, IActionRenderer> _rendererDict;

        protected GameObject _gameObject;
        protected Transform _transform;

        public ActionView()
        {
            _gameObject = new GameObject("actionView");
            _transform = _gameObject.transform;

            _rendererDict = new Dictionary<int, IActionRenderer>();
            _colorFilter = new ColorFilter();

            InitRenderers();
        }

        /// <summary>
        /// 类型
        /// </summary>
        public virtual int type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 动作
        /// </summary>
        public virtual int action
        {
            get { return _action; }
            set
            {
                if (_action != value)
                {
                    _action = value;
                    // 设置动作渲染动作
                    foreach (var v in _rendererDict)
                        v.Value.action = _action;
                }
            }
        }

        /// <summary>
        /// 方向
        /// </summary>
        public virtual ActionDirection direction
        {
            get { return _direction; }
            set
            {
                if (_direction != value)
                {
                    _direction = value;
                    // 设置动作渲染方向
                    foreach (var v in _rendererDict)
                        v.Value.direction = _direction;
                }
            }
        }

        /// <summary>
        /// 当前索引
        /// </summary>
        public virtual int frameIndex
        {
            get { return _frameIndex; }
            set
            {
                _frameIndex = value;

                if (_rendererDict != null)
                {
                    foreach (var v in _rendererDict)
                    {
                        IActionRenderer renderer = v.Value;
                        if (renderer.loop)
                            renderer.NextFrame();
                        else
                            renderer.frameIndex = _frameIndex;
                    }
                }
            }
        }

        /// <summary>
        /// 最大索引
        /// </summary>
        public virtual int maxFrameIndex
        {
            get { return _maxFrameIndex; }
            set
            {
                _maxFrameIndex = value;

                foreach (var v in _rendererDict)
                {
                    IActionRenderer renderer = v.Value;
                    if (renderer.loop == false)
                        renderer.maxFrameIndex = _maxFrameIndex;
                }
            }
        }

        /// <summary>
        /// 排序层级
        /// </summary>
        public virtual string sortingLayer
        {
            get { return _sortingLayer; }
            set
            {
                _sortingLayer = value;
                foreach (var v in _rendererDict)
                    v.Value.sortingLayer = _sortingLayer;
            }
        }

        /// <summary>
        /// 排序索引
        /// </summary>
        public virtual int sortingIndex
        {
            get { return _sortingIndex; }
            set
            {
                _sortingIndex = value;
                foreach (var v in _rendererDict)
                    v.Value.sortingIndex = _sortingIndex;
            }
        }

        /// <summary>
        /// 强制排序
        /// </summary>
        public virtual int sortingOrder
        {
            get { return _sortingOrder; }
            set
            {
                _sortingOrder = value;
                foreach (var v in _rendererDict)
                    v.Value.sortingOrder = _sortingIndex;
            }
        }

        /// <summary>
        /// 帧索引
        /// </summary>
        public virtual int[] frames
        {
            get { return _frames; }
            set
            {
                _frames = value;
                foreach (var v in _rendererDict)
                    v.Value.frames = _frames;
            }
        }

        /// <summary>
        /// 深度（排序用）
        /// </summary>
        public virtual int depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        /// <summary>
        /// 是否循环
        /// </summary>
        public virtual bool loop
        {
            get { return _loop; }
            set { _loop = value; }
        }

        /// <summary>
        /// 透明度
        /// </summary>
        public virtual float alpha
        {
            get { return _alpha; }
            set
            {
                _alpha = value;
                foreach (var v in _rendererDict)
                    v.Value.alpha = _alpha;
            }
        }

        /// <summary>
        /// 设置滤镜
        /// </summary>
        public virtual ColorFilter colorFilter
        {
            get { return _colorFilter; }
            set
            {
                _colorFilter = value;
                foreach (var v in _rendererDict)
                    v.Value.colorFilter = value;
            }
        }

        /// <summary>
        /// gameObject
        /// </summary>
        public virtual GameObject gameObject
        {
            get { return _gameObject; }
        }

        /// <summary>
        /// transform
        /// </summary>
        public virtual Transform transform
        {
            get { return _transform; }
        }

        /// <summary>
        /// 初始化Renders
        /// </summary>
        protected virtual void InitRenderers()
        {

        }

        /// <summary>
        /// 添加动作渲染
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void AddRenderer(IActionRenderer renderer)
        {
            if (_rendererDict.ContainsKey(renderer.type))
                return;

            renderer.sortingIndex = _sortingIndex;
            renderer.sortingOrder = _sortingOrder;
            renderer.frames = _frames;
            if (_alpha != 1f)
                renderer.alpha = _alpha;
            if (_colorFilter != null)
                renderer.colorFilter = _colorFilter;

            // 设置动作渲染当前动作
            if (renderer.lockActionAndDirection == false)
            {
                renderer.action = _action;
                renderer.direction = _direction;
            }

            // 设置动作渲染当前帧
            if (renderer.loop == false)
            {
                renderer.maxFrameIndex = _maxFrameIndex;
                renderer.frameIndex = _frameIndex;
            }

            renderer.transform.SetParent(_transform, false);
            _rendererDict[renderer.type] = renderer;
        }

        /// <summary>
        /// 移除动作渲染
        /// </summary>
        /// <param name="type"></param>
        public virtual IActionRenderer RemoveRenderer(int type)
        {
            IActionRenderer renderer;
            if (_rendererDict.TryGetValue(type, out renderer))
            {
                _rendererDict.Remove(type);
                renderer.Reset();
                return renderer;
            }

            return null;
        }

        /// <summary>
        /// 移除动作渲染
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void RemoveRenderer(IActionRenderer renderer)
        {
            RemoveRenderer(renderer.type);
        }

        /// <summary>
        /// 获取动作渲染
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IActionRenderer GetRenderer(int type)
        {
            IActionRenderer renderer;
            if (_rendererDict.TryGetValue(type, out renderer))
                return renderer;

            return null;
        }

        /// <summary>
        /// 刷新动作渲染
        /// </summary>
        public virtual void RefreshRenderers()
        {

        }

        /// <summary>
        /// 设置动作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="direction"></param>
        /// <param name="callback"></param>
        public virtual void SetAction(int action, ActionDirection direction, Action callback = null)
        {
            if (_action == action && _direction == direction)
                return;

            ActionDirection oldDirection = _direction;
            _action = action;
            _direction = direction;
            _callback = callback;
            SetRenderersAction();
            OnDirectionChanged(oldDirection, _direction);
        }

        /// <summary>
        /// 设置动作渲染动作
        /// </summary>
        protected virtual void SetRenderersAction()
        {
            foreach (var v in _rendererDict)
            {
                IActionRenderer renderer = v.Value;
                if (renderer.lockActionAndDirection == false)
                    renderer.SetAction(_action, _direction);
            }
        }

        /// <summary>
        /// 方向发生改变
        /// </summary>
        /// <param name="oldDirection"></param>
        /// <param name="newDirection"></param>
        protected virtual void OnDirectionChanged(ActionDirection oldDirection, ActionDirection newDirection)
        {
            // 检查动作视图深度是否发生改变
            if (ActionCore.actionViewDepthChangeChecker != null)
            {
                if (ActionCore.actionViewDepthChangeChecker.Invoke(oldDirection, newDirection))
                    RefreshRenderersDepth();
            }
        }

        /// <summary>
        /// 刷新动作渲染深度
        /// </summary>
        protected virtual void RefreshRenderersDepth()
        {

        }

        /// <summary>
        /// 跳转到下一帧
        /// </summary>
        public virtual void NextFrame()
        {
            if (_frameIndex < _maxFrameIndex)
                frameIndex++;
            else
            {
                if (_callback != null)
                    _callback.Invoke();

                if (_loop)
                    frameIndex = 0;
            }
        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="position"></param>
        public virtual void SetPosition(Vector3 position)
        {
            SetPosition(position.x, position.y, position.z);
        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public virtual void SetPosition(float x = 0, float y = 0, float z = 0)
        {
            _transform.localPosition = new Vector3(x, -y, z);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {
            foreach (var v in _rendererDict)
                v.Value.Reset();

            _type = 0;
            _action = 0;
            _direction = ActionDirection.NONE;
            _frameIndex = 0;
            _maxFrameIndex = -1;
            _sortingIndex = 0;
            _sortingOrder = int.MinValue;
            _frames = null;
            _depth = 0;
            _loop = false;
            _alpha = 1f;
            _colorFilter.Reset();
            _callback = null;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
        {
            Reset();
            foreach (var v in _rendererDict)
                v.Value.Dispose();

            _rendererDict = null;
            _transform = null;
            UnityEngine.Object.Destroy(_gameObject);
            _gameObject = null;
            _colorFilter = null;
        }

        /// <summary>
        /// 从对象池中获取
        /// </summary>
        public virtual void OnPoolGet()
        {
            _gameObject.SetActive(true);
        }

        /// <summary>
        /// 重置对象池对象
        /// </summary>
        public virtual void OnPoolReset()
        {
            Reset();
            _transform.SetParent(null, false);
            _gameObject.SetActive(false);
        }

        /// <summary>
        /// 销毁对象池对象
        /// </summary>
        public virtual void OnPoolDispose()
        {
            Dispose();
        }
    }
}
