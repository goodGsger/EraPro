using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class ResourceManagerEventArgs : EventArguments, IPooledObject
    {
        public const string ITEM_START = "ItemStart";
        public const string ITEM_COMPLETE = "ItemComplete";
        public const string ITEM_PROGRESS = "ItemProgress";
        public const string ITEM_ERROR = "ItemError";

        public const string QUEUE_START = "QueueStart";
        public const string QUEUE_COMPLETE = "QueueComplete";
        public const string QUEUE_PROGRESS = "QueueProgress";
        public const string QUEUE_ERROR = "QueueError";

        private LoadItem _loadItem;
        private LoadQueue _loadQueue;

        public ResourceManagerEventArgs()
        {

        }

        public ResourceManagerEventArgs(string type, LoadItem loadItem)
        {
            _type = type;
            _loadItem = loadItem;
        }

        public ResourceManagerEventArgs(string type, LoadQueue loadQueue)
        {
            _type = type;
            _loadQueue = loadQueue;
        }

        public LoadItem loadItem
        {
            get { return _loadItem; }
            set { _loadItem = value; }
        }

        public LoadQueue loadQueue
        {
            get { return _loadQueue; }
            set { _loadQueue = value; }
        }

        public void OnPoolGet()
        {

        }

        public void OnPoolReset()
        {
            _type = null;
            _data = null;
            _loadItem = null;
            _loadQueue = null;
        }

        public void OnPoolDispose()
        {
            _type = null;
            _data = null;
            _loadItem = null;
            _loadQueue = null;
        }
    }
}
