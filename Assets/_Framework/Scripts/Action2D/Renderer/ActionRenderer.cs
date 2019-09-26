using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class ActionRenderer : IActionRenderer
    {
        protected int _type;
        protected int _action;
        protected ActionDirection _direction;
        protected int _frameIndex;
        protected int _maxFrameIndex = -1;
        protected int _currentDelayFrame;
        protected int _delayFrames;
        protected int[] _frames;
        protected int _sortingIndex;
        protected int _sortingOrder = int.MinValue;
        protected int _depth;
        protected bool _loop;
        protected bool _lockActionAndDirection;
        protected bool _lockDepth;
        protected bool _lockSorting;

        protected ActionAsset _actionAsset;
        protected Action _callback;
        protected string _url;
        protected string _defaultUrl;
        protected bool _isLoading;
        protected Action _loadCallback;

        protected GameObject _gameObject;
        protected Transform _transform;
        protected SpriteRenderer _renderer;
        protected Material _material;
        protected Sprite _sprite;
        protected float _alpha = 1f;
        protected ColorFilter _colorFilter;

        public ActionRenderer()
        {
            _gameObject = new GameObject("actionRenderer");
            _transform = _gameObject.transform;
            _renderer = _gameObject.AddComponent<SpriteRenderer>();
            _colorFilter = new ColorFilter();
            GetMaterial();
            ApplyShader();
        }

        protected virtual void GetMaterial()
        {
            _material = _renderer.material;
        }

        /// <summary>
        /// 应用shader
        /// </summary>
        protected virtual void ApplyShader()
        {
            _material.shader = ShaderManager.inst.GetShader(ShaderDefine.textureExtShader);
        }

        /// <summary>
        /// 获取Sprite
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual Sprite GetSprite(int index)
        {
            return _actionAsset.actionData.GetSprite(index);
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
            set { _action = value; }
        }

        /// <summary>
        /// 方向
        /// </summary>
        public virtual ActionDirection direction
        {
            get { return _direction; }
            set
            {
                bool depthChanged = _direction != value;
                _direction = value;
                if (depthChanged)
                    RefreshDepth();
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

                if (_frameIndex <= _maxFrameIndex)
                    if (_actionAsset != null)
                    {
                        if (_frames != null)
                        {
                            int frame = App.stageManager.CurrentFramesToBaseFrames(_frameIndex);
                            if (frame >= 0 && frame < _frames.Length)
                                _sprite = GetSprite(_frames[frame]);
                            else
                                _sprite = null;
                            _renderer.sprite = _sprite;
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
            set { _maxFrameIndex = value; }
        }

        /// <summary>
        /// 当前延迟帧
        /// </summary>
        public virtual int currentDelayFrame
        {
            get { return _currentDelayFrame; }
            set { _currentDelayFrame = value; }
        }

        /// <summary>
        /// 最大延迟帧
        /// </summary>
        public virtual int delayFrames
        {
            get { return _delayFrames; }
            set { _delayFrames = value; }
        }

        /// <summary>
        /// 帧索引
        /// </summary>
        public virtual int[] frames
        {
            get { return _frames; }
            set { _frames = value; }
        }

        /// <summary>
        /// 深度（排序用）
        /// </summary>
        public virtual string sortingLayer
        {
            get { return _renderer.sortingLayerName; }
            set { _renderer.sortingLayerName = value; }
        }

        /// <summary>
        /// 深度（排序用）
        /// </summary>
        public virtual int sortingIndex
        {
            get { return _sortingIndex; }
            set
            {
                _sortingIndex = value;
                RefreshSortingOrder();
            }
        }

        /// <summary>
        /// 强制深度
        /// </summary>
        public virtual int sortingOrder
        {
            get { return _sortingOrder; }
            set
            {
                _sortingOrder = value;
                RefreshSortingOrder();
            }
        }

        /// <summary>
        /// 深度
        /// </summary>
        public virtual int depth
        {
            get { return _depth; }
            set
            {
                _depth = value;
                RefreshSortingOrder();
            }
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
            get
            {
                return _alpha;
                //return _renderer.color.a;
            }
            set
            {
                //Color color = _renderer.color;
                //color.a = value;
                //_renderer.color = color;
                _alpha = value;
                _material.SetFloat("_Alpha", value);
            }
        }

        /// <summary>
        /// 颜色滤镜
        /// </summary>
        public virtual ColorFilter colorFilter
        {
            get
            {
                return _colorFilter;
            }
            set
            {
                if (value != null)
                {
                    _colorFilter.SetValues(value);
                    ShaderManager.inst.ApplyColorFilter(_material, _colorFilter);
                }
                else
                {
                    ShaderManager.inst.ApplyColorFilter(_material, null);
                }
            }
        }

        /// <summary>
        /// 是否锁定动作和方向
        /// </summary>
        public virtual bool lockActionAndDirection
        {
            get { return _lockActionAndDirection; }
            set { _lockActionAndDirection = value; }
        }

        /// <summary>
        /// 锁定深度（不进行排序）
        /// </summary>
        public virtual bool lockDepth
        {
            get { return _lockDepth; }
            set { _lockDepth = value; }
        }

        /// <summary>
        /// 锁定排序（不进行排序）
        /// </summary>
        public virtual bool lockSorting
        {
            get { return _lockSorting; }
            set { _lockSorting = value; }
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
        /// renderer
        /// </summary>
        public virtual Renderer renderer
        {
            get { return _renderer; }
        }

        /// <summary>
        /// 材质
        /// </summary>
        public virtual Material material
        {
            get { return _material; }
        }

        /// <summary>
        /// Sprite
        /// </summary>
        public virtual Sprite sprite
        {
            get { return _sprite; }
        }

        /// <summary>
        /// 动作资源
        /// </summary>
        public virtual ActionAsset actionAsset
        {
            get { return _actionAsset; }
        }

        /// <summary>
        /// 设置动作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="direction"></param>
        /// <param name="callback"></param>
        public virtual void SetAction(int action, ActionDirection direction, Action callback = null)
        {
            _callback = callback;
            if (_action == action && _direction == direction)
                return;
            _action = action;
            this.direction = direction;
        }

        /// <summary>
        /// 刷新深度
        /// </summary>
        public virtual void RefreshDepth()
        {
            if (_lockDepth)
                return;
            if (_direction != ActionDirection.NONE)
            {
                if (ActionCore.actionRendererDepthCalculator != null)
                    depth = ActionCore.actionRendererDepthCalculator.Invoke(_direction, _type);
            }
        }

        /// <summary>
        /// 跳转到下一帧
        /// </summary>
        public virtual void NextFrame()
        {
            // 处理延迟帧
            if (_currentDelayFrame > 0)
            {
                _currentDelayFrame--;
                if (_currentDelayFrame == 0)
                    _gameObject.SetActive(true);
                return;
            }

            if (_frameIndex < _maxFrameIndex)
                frameIndex++;
            else
            {
                // 播放完毕处理延迟帧
                if (_delayFrames > 0)
                {
                    _gameObject.SetActive(false);
                    _currentDelayFrame = _delayFrames;
                }

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
        /// 设置动作资源
        /// </summary>
        /// <param name="actionAsset"></param>
        public virtual void SetActionAsset(ActionAsset actionAsset)
        {
            // 清除原资源
            if (_actionAsset != null)
                _actionAsset.Unuse();

            _actionAsset = actionAsset;

            if (_actionAsset != null)
            {
                // 重设当前帧
                _gameObject.SetActive(true);
                frameIndex = _frameIndex;
                _actionAsset.Use();
                _material.SetTexture("_AlphaTex", _actionAsset.actionData.textureExtSprite.texture.alphaTexture);
            }
            else
            {
                // 隐藏当前帧
                _renderer.sprite = null;
                _gameObject.SetActive(false);
                _material.SetTexture("_AlphaTex", null);
            }
        }

        /// <summary>
        /// 刷新排序
        /// </summary>
        protected virtual void RefreshSortingOrder()
        {
            if (_lockSorting)
                return;

            if (_sortingOrder == int.MinValue)
                _renderer.sortingOrder = _sortingIndex * 10 + _depth;
            else
                _renderer.sortingOrder = _sortingOrder;
        }

        /// <summary>
        /// 动态加载动作数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="defaultUrl"></param>
        /// <param name="priority"></param>
        /// <param name="loadCallback"></param>
        public virtual void LoadAction(string url, string defaultUrl = null, LoadPriority priority = LoadPriority.LV_2, Action loadCallback = null)
        {
            if (url == null)
                return;
            if (_actionAsset != null && _actionAsset.id == url)
            {
                if (loadCallback != null)
                    loadCallback.Invoke();
                return;
            }
            else if (_isLoading == true && _url == url)
            {
                _loadCallback = loadCallback;
                return;
            }

            // 停止当前加载项
            StopLoadAction();

            if (App.assetManager.HasAsset(url))
            {
                // 内存中已存在资源
                SetActionAsset(App.assetManager.GetAsset(url) as ActionAsset);
                if (loadCallback != null)
                    loadCallback.Invoke();
            }
            else
            {
                // 内存中不存在资源
                _url = url;
                _defaultUrl = defaultUrl;

                if (_url != null)
                {
                    _loadCallback = loadCallback;
                    _isLoading = true;

                    // 显示默认纹理
                    if (_defaultUrl != null && App.assetManager.HasAsset(_defaultUrl))
                        SetActionAsset(App.assetManager.GetAsset(_defaultUrl) as ActionAsset);
                    else
                        SetActionAsset(null);

                    App.resourceManager.Load(_url, LoadType.ACTION, LoadPriority.LV_2, LoadComplete);
                }
                else
                    SetActionAsset(null);
            }
        }

        /// <summary>
        /// 加载完毕
        /// </summary>
        /// <param name="item"></param>
        protected virtual void LoadComplete(LoadItem item)
        {
            SetActionAsset(item.asset as ActionAsset);
            _url = null;
            _defaultUrl = null;
            _isLoading = false;

            // 执行加载回调
            if (_loadCallback != null)
            {
                _loadCallback.Invoke();
                //_loadCallback = null;
            }
        }

        /// <summary>
        /// 停止加载动作数据
        /// </summary>
        public virtual void StopLoadAction()
        {
            if (_isLoading == false)
                return;

            // 移除加载回调
            App.resourceManager.RemoveLoadCallback(_url, LoadComplete);

            _url = null;
            _defaultUrl = null;
            _isLoading = false;
            _loadCallback = null;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {
            SetActionAsset(null);
            StopLoadAction();
            _colorFilter.Reset();
            ShaderManager.inst.ApplyColorFilter(_material, null);
            //_type = 0;
            _action = 0;
            _direction = ActionDirection.NONE;
            _frameIndex = 0;
            _maxFrameIndex = -1;
            _currentDelayFrame = 0;
            _delayFrames = 0;
            _sortingIndex = 0;
            _sortingOrder = int.MinValue;
            _frames = null;
            _depth = 0;
            _loop = false;
            _lockActionAndDirection = false;
            _lockDepth = false;
            _lockSorting = false;
            _callback = null;
            _loadCallback = null;
            _renderer.sprite = null;
            _sprite = null;
            //Color color = _renderer.color;
            //_renderer.color = new Color(color.r, color.g, color.b);
            alpha = 1;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
        {
            Reset();
            _transform = null;
            _renderer = null;
            _material = null;
            UnityEngine.Object.Destroy(_gameObject);
            _gameObject = null;
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
            _gameObject.layer = 0;
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
