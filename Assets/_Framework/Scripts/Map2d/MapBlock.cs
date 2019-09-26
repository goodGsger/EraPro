using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    internal class MapBlock : IPooledObject
    {
        private string _key;
        private MapVo _mapVo;
        private GameObject _blockContainer;
        private GameObject _quad;
        private Material _material;
        private Texture2D _texture;

        public MapBlock()
        {
            _quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            _quad.name = "block";
            UnityEngine.Object.Destroy(_quad.GetComponent<MeshCollider>());
            _material = new Material(ShaderManager.inst.GetShader(ShaderDefine.imageShader));
            MeshRenderer renderer = _quad.GetComponent<MeshRenderer>();
            renderer.sortingLayerName = MapSetting.sortingLayer;
            renderer.material = _material;
            _quad.SetActive(false);
        }

        public string key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// mapVo
        /// </summary>
        public MapVo mapVo
        {
            get { return _mapVo; }
            set
            {
                _mapVo = value;
                _quad.transform.localScale = new Vector3(_mapVo.tileWidth, _mapVo.tileHeight);
            }
        }

        /// <summary>
        /// 地图容器
        /// </summary>
        public GameObject mapContainer
        {
            get { return _blockContainer; }
        }

        /// <summary>
        /// quad
        /// </summary>
        public GameObject quad
        {
            get { return _quad; }
        }

        /// <summary>
        /// 地图纹理
        /// </summary>
        public Texture2D texture
        {
            get { return _texture; }
        }

        /// <summary>
        /// 设置地图容器
        /// </summary>
        /// <param name="mapContainer"></param>
        public void SetBlockContainer(GameObject blockContainer)
        {
            _blockContainer = blockContainer;
            _quad.transform.SetParent(_blockContainer.transform, false);
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPosition(float x, float y)
        {
            _quad.transform.localPosition = new Vector3(x + _mapVo.tileWidth * 0.5f, -y - _mapVo.tileHeight * 0.5f);
        }

        /// <summary>
        /// 设置地图纹理
        /// </summary>
        /// <param name="texture"></param>
        public void SetTexture(Texture2D texture)
        {
            _texture = texture;
            _material.mainTexture = _texture;
            if (_texture != null)
                _quad.SetActive(true);
            else
                _quad.SetActive(false);
        }

        /// <summary>
        /// 从对象池中获取
        /// </summary>
        public void OnPoolGet()
        {

        }

        /// <summary>
        /// 重置对象池对象
        /// </summary>
        public void OnPoolReset()
        {
            _mapVo = null;
            _blockContainer = null;
            _texture = null;
            _material.mainTexture = null;
            _quad.SetActive(false);
        }

        /// <summary>
        /// 销毁对象池对象
        /// </summary>
        public void OnPoolDispose()
        {
            _mapVo = null;
            _blockContainer = null;
            _texture = null;
            _material.mainTexture = null;
            _material = null;
            _blockContainer = null;
            UnityEngine.Object.Destroy(_quad);
            _quad = null;
        }
    }
}
