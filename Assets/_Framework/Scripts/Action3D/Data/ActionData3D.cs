using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class ActionData3D : IPooledObject
    {
        public ActionDataPackage3D package;
        public GameObject gameObject;

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {
            package = null;
            if (gameObject != null)
            {
                UnityEngine.Object.Destroy(gameObject);
                gameObject = null;
            }
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
        {
            Reset();
        }

        /// <summary>
        /// 从对象池中获取
        /// </summary>
        public virtual void OnPoolGet()
        {

        }

        /// <summary>
        /// 重置对象池对象
        /// </summary>
        public virtual void OnPoolReset()
        {
            Reset();
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
