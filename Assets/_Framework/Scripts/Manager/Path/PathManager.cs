using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class PathManager : Manager, IPathManager
    {

        private string _streamingPathFile;
        private string _persistentDataPathFile;
        private string _streamingPathWWW;
        private string _persistentDataPathWWW;
        private string _externalPath;

        private string _root;
        private string _lua;
        private string _ui;
        private string _config;
        private string _map;
        private string _map_ab;
        private string _action;
        private string _effect;
        private string _audio;
        private string _texture;
        private string _record;
        private string _image;

        /// <summary>
        /// 本地Streaming路径
        /// </summary>
        public string streamingAssetPathFile
        {
            get { return _streamingPathFile; }
            set { _streamingPathFile = value; }
        }

        /// <summary>
        /// 本地Persistent路径
        /// </summary>
        public string persistentDataPathFile
        {
            get { return _persistentDataPathFile; }
            set { _persistentDataPathFile = value; }
        }

        /// <summary>
        /// WWWStreaming路径
        /// </summary>
        public string streamingAssetPathWWW
        {
            get { return _streamingPathWWW; }
            set { _streamingPathWWW = value; }
        }

        /// <summary>
        /// WWWPersistent路径
        /// </summary>
        public string persistentDataPathWWW
        {
            get { return _persistentDataPathWWW; }
            set { _persistentDataPathWWW = value; }
        }

        /// <summary>
        /// 外部资源路径
        /// </summary>
        public string externalPath
        {
            get { return _externalPath; }
            set { _externalPath = value; }
        }

        /// <summary>
        /// 资源根目录
        /// </summary>
        public string root
        {
            get { return _root; }
            set { _root = value; }
        }

        /// <summary>
        /// lua目录
        /// </summary>
        public string lua
        {
            get { return _lua; }
            set { _lua = value; }
        }

        /// <summary>
        /// ui目录
        /// </summary>
        public string ui
        {
            get { return _ui; }
            set { _ui = value; }
        }

        /// <summary>
        /// config目录
        /// </summary>
        public string config
        {
            get { return _config; }
            set { _config = value; }
        }

        /// <summary>
        /// 地图目录
        /// </summary>
        public string map
        {
            get { return _map; }
            set { _map = value; }
        }

        /// <summary>
        /// 地图asssetBundle目录
        /// </summary>
        public string map_ab
        {
            get { return _map_ab; }
            set { _map_ab = value; }
        }

        /// <summary>
        /// 动作目录
        /// </summary>
        public string action
        {
            get { return _action; }
            set { _action = value; }
        }

        /// <summary>
        /// 特效目录
        /// </summary>
        public string effect
        {
            get { return _effect; }
            set { _effect = value; }
        }

        /// <summary>
        /// 音频目录
        /// </summary>
        public string audio
        {
            get { return _audio; }
            set { _audio = value; }
        }

        /// <summary>
        /// 纹理目录
        /// </summary>
        public string texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        /// <summary>
        /// 录音文件目录
        /// </summary>
        public string record
        {
            get { return _record; }
            set { _record = value; }
        }

        /// <summary>
        /// 单张图片目录
        /// </summary>
        public string image
        {
            get { return _image; }
            set { _image = value; }
        }

        protected override void Init()
        {
            _streamingPathFile = Application.streamingAssetsPath + "/";
            _persistentDataPathFile = Application.persistentDataPath + "/";
            _streamingPathWWW = Application.streamingAssetsPath + "/";
            _persistentDataPathWWW = Application.persistentDataPath + "/";

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            _streamingPathWWW = "file:///" + _streamingPathWWW;
            _persistentDataPathWWW = "file:///" + _persistentDataPathWWW;
#elif UNITY_ANDROID
            _persistentDataPathWWW = "file://" + _persistentDataPathWWW;
#elif UNITY_IPHONE
            _persistentDataPathWWW = "file://" + _persistentDataPathWWW;
            _streamingPathWWW = "file://" + _streamingPathWWW;
#endif

            _root = "assets/";
            _lua = _root + "lua_ab/";
            _ui = _root + "ui/";
            _config = _root + "config/";
            _map = _root + "map/";
            _map_ab = _root + "map_ab/";
            _action = _root + "action/";
            _effect = _root + "effect/";
            _audio = _root + "audio/";
            _texture = _root + "texture/";
            _record = _root + "record/";
            _image = _root + "image/";
        }
    }
}
