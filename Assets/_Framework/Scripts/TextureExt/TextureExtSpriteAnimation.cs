using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class TextureExtSpriteAnimation : IPooledObject
    {
        protected int _frameIndex;
        protected int _maxFrameIndex = -1;
        protected float _interval = App.stageManager.interval;
        protected bool _loop;
        protected bool _isPlaying;

        protected TextureExtSpriteAsset _textureExtSpriteAsset;
        protected Action _callback;
        protected string _url;
        protected bool _isLoading;
        protected Action _loadCallback;

        protected GameObject _gameObject;
        protected Transform _transform;
        protected SpriteRenderer _renderer;
        protected Material _material;
        protected Sprite _sprite;
        protected float _alpha = 1f;

        public TextureExtSpriteAnimation()
        {
            _gameObject = new GameObject("textureExtRenderer");
            _transform = _gameObject.transform;
            _renderer = _gameObject.AddComponent<SpriteRenderer>();
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
        /// 纹理资源
        /// </summary>
        public virtual TextureExtSpriteAsset textureExtSpriteAsset
        {
            get { return _textureExtSpriteAsset; }
        }

        /// <summary>
        /// 回调函数
        /// </summary>
        public virtual Action callback
        {
            get { return _callback; }
            set { _callback = value; }
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
        /// 当前索引
        /// </summary>
        public virtual int frameIndex
        {
            get { return _frameIndex; }
            set
            {
                if (_frameIndex <= _maxFrameIndex)
                {
                    _frameIndex = value;
                    if (_textureExtSpriteAsset != null)
                    {
                        if (_frameIndex < _textureExtSpriteAsset.textureExtSprite.spriteCount)
                            _sprite = _textureExtSpriteAsset.textureExtSprite.GetSprite(_frameIndex);
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
        /// 间隔
        /// </summary>
        public virtual float interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                if (_isPlaying)
                    Play();
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
        /// 是否正在播放
        /// </summary>
        public virtual bool isPlaying
        {
            get { return _isPlaying; }
            set
            {
                _isPlaying = value;
                if (_isPlaying)
                    Play();
                else
                    Stop();
            }
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
        public virtual int sortingOrder
        {
            get { return _renderer.sortingOrder; }
            set { _renderer.sortingOrder = value; }
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
        /// 跳转到下一帧
        /// </summary>
        public virtual void NextFrame()
        {
            if (_maxFrameIndex > 0)
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
        }

        /// <summary>
        /// 播放
        /// </summary>
        public virtual void Play()
        {
            _isPlaying = true;
            App.timerManager.RegisterLoop(_interval, OnPlay);
        }

        /// <summary>
        /// 停止
        /// </summary>
        public virtual void Stop()
        {
            _isPlaying = false;
            App.timerManager.Unregister(OnPlay);
        }

        /// <summary>
        /// 播放
        /// </summary>
        protected virtual void OnPlay()
        {
            if (_isPlaying)
                NextFrame();
        }

        /// <summary>
        /// 从指定帧播放
        /// </summary>
        /// <param name="frame"></param>
        public virtual void GotoAndPlay(int frame)
        {
            frameIndex = frame;
            Play();
        }

        /// <summary>
        /// 停留到指定帧
        /// </summary>
        /// <param name="frame"></param>
        public virtual void GotoAndStop(int frame)
        {
            Stop();
            frameIndex = frame;
        }

        /// <summary>
        /// 设置纹理资源
        /// </summary>
        /// <param name="textureExtSpriteAsset"></param>
        public virtual void SetTextureExtSpriteAsset(TextureExtSpriteAsset textureExtSpriteAsset)
        {
            // 清除原资源
            if (_textureExtSpriteAsset != null)
                _textureExtSpriteAsset.Unuse();

            _textureExtSpriteAsset = textureExtSpriteAsset;

            if (_textureExtSpriteAsset != null)
            {
                // 重设当前帧
                _gameObject.SetActive(true);
                _maxFrameIndex = _textureExtSpriteAsset.textureExtSprite.spriteCount - 1;
                frameIndex = _frameIndex;
                _textureExtSpriteAsset.Use();
                _material.SetTexture("_AlphaTex", _textureExtSpriteAsset.textureExtSprite.texture.alphaTexture);
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
        /// 动态加载纹理数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="priority"></param>
        /// <param name="loadCallback"></param>
        public virtual void LoadTexture(string url, LoadPriority priority = LoadPriority.LV_2, Action loadCallback = null)
        {
            if (_textureExtSpriteAsset != null && _textureExtSpriteAsset.id == url || _isLoading == true && _url == url)
                return;

            // 停止当前加载项
            StopLoadTexture();

            if (App.assetManager.HasAsset(url))
            {
                // 内存中已存在资源
                SetTextureExtSpriteAsset(App.assetManager.GetAsset(url) as TextureExtSpriteAsset);
                if (loadCallback != null)
                    loadCallback.Invoke();
            }
            else
            {
                // 内存中不存在资源
                _url = url;
                _loadCallback = loadCallback;
                _isLoading = true;

                SetTextureExtSpriteAsset(null);

                App.resourceManager.Load(_url, LoadType.TEXTURE_EXT_SPRITE, LoadPriority.LV_2, LoadComplete);
            }
        }

        /// <summary>
        /// 加载完毕
        /// </summary>
        /// <param name="item"></param>
        protected virtual void LoadComplete(LoadItem item)
        {
            SetTextureExtSpriteAsset(item.asset as TextureExtSpriteAsset);
            _url = null;
            _isLoading = false;

            // 执行加载回调
            if (_loadCallback != null)
            {
                _loadCallback.Invoke();
                _loadCallback = null;
            }
        }

        /// <summary>
        /// 停止加载纹理数据
        /// </summary>
        public virtual void StopLoadTexture()
        {
            if (_isLoading == false)
                return;

            // 移除加载回调
            App.resourceManager.RemoveLoadCallback(_url, LoadComplete);

            _url = null;
            _isLoading = false;
            _loadCallback = null;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            SetTextureExtSpriteAsset(null);
            StopLoadTexture();
            _frameIndex = 0;
            _maxFrameIndex = -1;
            _loop = false;
            _callback = null;
            _renderer.sprite = null;
            _sprite = null;
            //Color color = _renderer.color;
            //_renderer.color = new Color(color.r, color.g, color.b);
            alpha = 1;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
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
