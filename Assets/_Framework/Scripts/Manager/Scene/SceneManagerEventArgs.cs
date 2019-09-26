using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class SceneManagerEventArgs : EventArguments, IPooledObject
    {
        public const string ENTERING_SCENE = "EnteringScene";
        public const string ENTER_SCENE = "EnterScene";
        public const string EXITING_SCENE = "ExitingScene";
        public const string EXIT_SCENE = "ExitScene";

        private IScene _scene;

        public SceneManagerEventArgs()
        {

        }

        public SceneManagerEventArgs(string type, IScene scene)
        {
            _type = type;
            _scene = scene;
        }

        /// <summary>
        /// 场景
        /// </summary>
        public IScene scene
        {
            get { return _scene; }
            set { _scene = value; }
        }

        public void OnPoolGet()
        {

        }

        public void OnPoolReset()
        {
            _type = null;
            _data = null;
            _scene = null;
        }

        public void OnPoolDispose()
        {
            _type = null;
            _data = null;
            _scene = null;
        }
    }
}
