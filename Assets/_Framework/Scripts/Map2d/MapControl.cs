using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    internal class MapControl
    {
        private GameObject _mapContainer;
        private GameObject _thumbnailContainer;
        private GameObject _blockContainer;
        private Renderer _thumbnailRenderer;
        private Material _thumbnailMaterial;
        private Texture2D _thumbnailTexture;
        private Transform _mapTransform;
        private Transform _thumbnailTransform;
        private Transform _blockTransform;
        private MapVo _mapVo;

        public MapControl()
        {
            _thumbnailContainer = GameObject.CreatePrimitive(PrimitiveType.Quad);
            _thumbnailContainer.name = "thumbnailContainer";
            _thumbnailRenderer = _thumbnailContainer.GetComponent<MeshRenderer>();
            _thumbnailRenderer.sortingLayerName = MapSetting.sortingLayer;
            _thumbnailMaterial = new Material(ShaderManager.inst.GetShader(ShaderDefine.imageShader));
            _thumbnailRenderer.material = _thumbnailMaterial;
            _thumbnailTransform = _thumbnailContainer.transform;
            _thumbnailContainer.SetActive(false);

            _blockContainer = new GameObject("blockContainer");
            _blockTransform = _blockContainer.transform;
            _blockContainer.SetActive(false);
        }

        public MapControl(GameObject mapContainer) : this()
        {
            this.mapContainer = mapContainer;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            if (_thumbnailTexture != null)
            {
                _thumbnailMaterial.mainTexture = null;
                UnityEngine.Object.DestroyImmediate(_thumbnailTexture, true);
                _thumbnailTexture = null;
            }
        }

        /// <summary>
        /// 地图容器
        /// </summary>
        public GameObject mapContainer
        {
            get { return _mapContainer; }
            set
            {
                _mapContainer = value;
                _mapTransform = _mapContainer.transform;

                _thumbnailContainer.transform.SetParent(_mapContainer.transform, false);
                _blockContainer.transform.SetParent(_mapContainer.transform, false);

                _thumbnailContainer.SetActive(true);
                _blockContainer.SetActive(true);
            }
        }

        /// <summary>
        /// 缩略图容器
        /// </summary>
        public GameObject thumbnailContainer
        {
            get { return _thumbnailContainer; }
        }

        /// <summary>
        /// 切片容器
        /// </summary>
        public GameObject blockContainer
        {
            get { return _blockContainer; }
        }

        /// <summary>
        /// 地图数据
        /// </summary>
        public MapVo mapVo
        {
            get { return _mapVo; }
            set
            {
                _mapVo = value;

                _thumbnailTransform.localPosition = new Vector3(_mapVo.mapWidth * 0.5f, -_mapVo.mapHeight * 0.5f, 0.1f);
            }
        }

        /// <summary>
        /// 设置缩略图纹理
        /// </summary>
        /// <param name="texture"></param>
        public void SetThumbnailTexture(Texture2D texture)
        {
            if (_thumbnailTexture != null)
            {
                UnityEngine.Object.DestroyImmediate(_thumbnailTexture, true);
            }
            _thumbnailTexture = texture;
            _thumbnailMaterial.mainTexture = texture;
        }

        /// <summary>
        /// 设置缩略图激活状态
        /// </summary>
        /// <param name="active"></param>
        public void SetThumbnailActive(bool active)
        {
            _thumbnailContainer.SetActive(active);
        }

        /// <summary>
        /// 设置切片坐标
        /// </summary>
        /// <param name="position"></param>
        public void SetBlockPosition(Vector3 position)
        {
            position.y = -position.y;
            _blockTransform.localPosition = position;
        }

        /// <summary>
        /// 设置缩略图缩放
        /// </summary>
        /// <param name="scale"></param>
        public void SetThumbnailScale(Vector3 scale)
        {
            _thumbnailTransform.localScale = scale;
        }
    }
}
