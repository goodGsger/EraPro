using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public enum AudioItemType
    {
        EFFECT = 0,
        MUSIC = 1
    }

    public class AudioItem
    {
        private AudioItemType _type;
        private AudioPlayer _player;
        private string _id;
        private string _owner;
        private Action<AudioItem> _callback;

        public AudioItem()
        {

        }

        /// <summary>
        /// 音频类型
        /// </summary>
        public AudioItemType type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 音频播放器
        /// </summary>
        public AudioPlayer player
        {
            get { return _player; }
            set { _player = value; }
        }

        /// <summary>
        /// 唯一ID
        /// </summary>
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 拥有者
        /// </summary>
        public string owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        /// <summary>
        /// 完成回调
        /// </summary>
        public Action<AudioItem> callback
        {
            get { return _callback; }
            set { _callback = value; }
        }
    }
}
