using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class Map
    {
        private GameObject _mapContainer;
        private MapVo _mapVo;
        private MapControl _control;
        private MapBlock[,] _blocks;
        private Dictionary<MapTile, MapLoaderItem> _renderTiles;
        private List<MapLoaderItem> _loadList;
        private HashSet<MapTile> _loadDict;
        private Dictionary<string, MapLoader> _loadingDict;
        private Dictionary<MapTile, TextureAsset> _textureDict;
        private List<MapTile> _globalMapTileList;

        private ObjectPool<MapBlock> _blockPool;
        private ObjectPool<MapLoader> _loaderPool;
        private ILoader _thumbnailLoader;
        private int _loadCount;

        private int _tileX;
        private int _tileY;
        private int _currentTileX;
        private int _currentTileY;
        private Vector2 _center;
        private Vector2 _leftTop;
        private Tweener _tweener;

        public Map()
        {
            _blockPool = new ObjectPool<MapBlock>();
            _loaderPool = new ObjectPool<MapLoader>();
            _blocks = new MapBlock[MapSetting.maxTileX, MapSetting.maxTileY];
            _renderTiles = new Dictionary<MapTile, MapLoaderItem>();
            _loadList = new List<MapLoaderItem>(30);
            _loadDict = new HashSet<MapTile>();
            _loadingDict = new Dictionary<string, MapLoader>();
            _textureDict = new Dictionary<MapTile, TextureAsset>();
            _globalMapTileList = new List<MapTile>(10);

            //if (MapSetting.assetBundleMode)
            //    _thumbnailLoader = new TextureAssetBundleLoader();
            //else
            //    _thumbnailLoader = new TextureLoader();
            _thumbnailLoader = new TextureLoader();

            _control = new MapControl();
        }

        public Map(GameObject mapContainer) : this()
        {
            this.mapContainer = mapContainer;
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
                _control.mapContainer = _mapContainer;
            }
        }

        /// <summary>
        /// 地图对象
        /// </summary>
        public MapVo mapVo
        {
            get { return _mapVo; }
        }

        /// <summary>
        /// 地图中心点
        /// </summary>
        public Vector2 center
        {
            get { return _center; }
        }

        /// <summary>
        /// 地图左上角点
        /// </summary>
        public Vector2 leftTop
        {
            get { return _leftTop; }
        }

        /// <summary>
        /// 显示地图
        /// </summary>
        /// <param name="mapVo"></param>
        /// <param name="pos"></param>
        public void Show(MapVo mapVo, Vector2 pos)
        {
            // 切换地图时清理缓存
            if (_mapVo != null)
                Reset();

            _mapVo = mapVo;
            _control.mapVo = _mapVo;
            _control.SetThumbnailScale(new Vector3(_mapVo.mapWidth, _mapVo.mapHeight, 1));
            SetCenter(pos);
            ShowThumbnail();
            Resize();

            if (_tweener != null)
            {
                _tweener.Kill();
                _tweener = null;
            }
        }

        /// <summary>
        /// 重置地图
        /// </summary>
        public void Reset()
        {
            _control.Reset();
            _renderTiles.Clear();
            _loadList.Clear();
            _loadDict.Clear();
            //_loadingDict.Clear();

            foreach (var v in _textureDict)
                v.Value.Dispose();
            _textureDict.Clear();

            _loadCount = 0;

            if (_tweener != null)
            {
                _tweener.Kill();
                _tweener = null;
            }
        }

        /// <summary>
        /// 显示缩略图
        /// </summary>
        private void ShowThumbnail()
        {
            string urlRelative = _mapVo.ThumbnailRelativePathJPG;
            if (App.fileManager.FileExistsPersistent(urlRelative))
            {
                Texture2D texutre = new Texture2D(MapSetting.thumbnailWidth, MapSetting.thumbnailHeight);
                byte[] bytes = App.fileManager.ReadFilePersistent(urlRelative);
                if (bytes != null)
                {
                    texutre.LoadImage(bytes);
                    _control.SetThumbnailActive(true);
                    _control.SetThumbnailTexture(texutre);
                    _thumbnailLoader.completeCallback = null;
                    return;
                }
            }

            _control.SetThumbnailActive(false);
            _thumbnailLoader.completeCallback = OnThumbnailComplete;
            _thumbnailLoader.urlRelative = urlRelative;
            _thumbnailLoader.Start();


            ////string urlRelative = _mapVo.ThumbnailRelativePath;
            //string urlRelative = _mapVo.ThumbnailRelativePathJPG;
            //TextureAsset asset = App.assetManager.GetAsset<TextureAsset>(urlRelative);
            //if (asset != null)
            //{
            //    // 资源存在则显示资源
            //    _control.SetThumbnailActive(true);
            //    _control.SetThumbnailTexture(asset.texture);
            //}
            //else
            //{
            //    // 资源不存在则加载资源
            //    _control.SetThumbnailActive(false);
            //    //string urlAbsolute;
            //    //if (App.fileManager.FileExistsPersistent(urlRelative))
            //    //    urlAbsolute = new StringBuilder(App.pathManager.persistentDataPathWWW).Append(urlRelative).ToString();
            //    //else
            //    //    urlAbsolute = new StringBuilder(App.pathManager.externalPath).Append(urlRelative).ToString();

            //    //_thumbnailLoader.urlRelative = urlRelative;
            //    //_thumbnailLoader.urlAbsolute = urlAbsolute;
            //    _thumbnailLoader.urlRelative = urlRelative;
            //    _thumbnailLoader.Start();
            //}
        }

        /// <summary>
        /// 缩略图加载完毕
        /// </summary>
        /// <param name="loader"></param>
        private void OnThumbnailComplete(ILoader loader)
        {
            _control.SetThumbnailActive(true);
            // 写入缓存
            //App.assetManager.AddAsset(_thumbnailLoader.urlRelative, loader.asset);
            // 保存缩略图
            //App.fileManager.WriteFilePersistentAsync(_thumbnailLoader.urlRelative, _thumbnailLoader.bytes, false);
            // 设置缩略图纹理
            _control.SetThumbnailTexture((loader.asset as TextureAsset).texture);
        }

        /// <summary>
        /// 重设尺寸
        /// </summary>
        public void Resize()
        {
            if (_mapVo == null)
                return;

            // 重新计算切片尺寸
            _tileX = Mathf.CeilToInt((float)_mapVo.renderWidth / _mapVo.tileWidth) + 1;
            _tileY = Mathf.CeilToInt((float)_mapVo.renderHeight / _mapVo.tileHeight) + 1;
            // 重新创建blocks
            CreateBlocks();
            // 强制渲染
            Render(true);
        }

        /// <summary>
        /// 创建blocks
        /// </summary>
        private void CreateBlocks()
        {
            // 清除block
            for (int i = 0; i < _tileX; i++)
            {
                for (int j = 0; j < _tileY; j++)
                {
                    MapBlock block = _blocks[i, j];
                    if (block != null)
                    {
                        _blockPool.Release(block);
                        _blocks[i, j] = null;
                    }
                }
            }

            // 创建block
            for (int x = 0; x < _tileX; x++)
                for (int y = 0; y < _tileY; y++)
                {
                    MapBlock block = _blockPool.Get();
                    block.mapVo = mapVo;
                    block.SetBlockContainer(_control.blockContainer);
                    block.SetPosition(x * _mapVo.tileWidth, y * _mapVo.tileHeight);
                    _blocks[x, y] = block;
                }
        }

        /// <summary>
        /// 渲染
        /// </summary>
        /// <param name="forced"></param>
        public void Render(bool forced = false)
        {
            int startX = (int)(_leftTop.x / _mapVo.tileWidth);
            int startY = (int)(_leftTop.y / _mapVo.tileHeight);
            _control.SetBlockPosition(new Vector3(_leftTop.x - _leftTop.x % _mapVo.tileWidth, leftTop.y - _leftTop.y % _mapVo.tileHeight));

            // 判断是否需要重绘
            if (_currentTileX == startX && _currentTileY == startY && forced == false)
                return;

            _currentTileX = startX;
            _currentTileY = startY;

            // 清理渲染列表
            _renderTiles.Clear();
            // 计算渲染列表
            int maxTileX = startX + _tileX;
            int maxTileY = startY + _tileY;

            //int fixedStartX = startX > 0 ? startX : 0;
            //int fixedStartY = startY > 0 ? startY : 0;
            for (int y = startY; y < maxTileY; y++)
            {
                for (int x = startX; x < maxTileX; x++)
                {
                    int blockX = x - _currentTileX;
                    int blockY = y - _currentTileY;
                    // block不存在
                    MapBlock block = GetBlock(blockX, blockY);
                    if (block == null)
                        continue;
                    // 坐标超过范围则清空纹理
                    if (x < 0 || x > _mapVo.maxTileX || y < 0 || y > _mapVo.maxTileY)
                    {
                        block.SetTexture(null);
                        continue;
                    }
                    // 从内存中获取纹理
                    TextureAsset asset;
                    if (_textureDict.TryGetValue(new MapTile(x, y), out asset))
                    {
                        // 内存中存在纹理
                        block.SetTexture(asset.texture);
                        continue;
                    }
                    else
                    {
                        // 内存中不存在纹理
                        block.SetTexture(null);
                        // 放入加载队列
                        MapLoaderItem loaderItem = new MapLoaderItem();
                        loaderItem.mapId = _mapVo.id;
                        loaderItem.tile = new MapTile(x, y);
                        loaderItem.mapX = x * _mapVo.tileWidth;
                        loaderItem.mapY = y * _mapVo.tileHeight;
                        _renderTiles[loaderItem.tile] = loaderItem;
                    }
                }
            }

            if (MapSetting.autoClearTexture)
            {
                // 移除缓存
                _globalMapTileList.Clear();
                foreach (var v in _textureDict)
                {
                    MapTile tile = v.Key;
                    if (tile.x < startX || tile.x >= maxTileX || tile.y < startY || tile.y >= maxTileY)
                    {
                        _globalMapTileList.Add(tile);
                    }
                }
                foreach (MapTile tile in _globalMapTileList)
                {
                    TextureAsset asset = _textureDict[tile];
                    _textureDict.Remove(tile);
                    asset.Dispose();
                }
            }

            // 移除加载队列
            for (int i = _loadList.Count - 1; i >= 0; i--)
            {
                MapLoaderItem loaderItem = _loadList[i];
                if (_renderTiles.ContainsKey(loaderItem.tile) == false)
                {
                    _loadList.RemoveAt(i);
                    _loadDict.Remove(loaderItem.tile);
                }
            }

            // 显示切片
            foreach (var v in _renderTiles)
            {
                // 判断是否需要加载
                if (_loadDict.Contains(v.Key) == false)
                {
                    _loadList.Add(v.Value);
                    _loadDict.Add(v.Key);
                }
            }

            // 加载
            if (_loadList.Count > 0)
            {
                _loadList.Sort(LoadListSortFunc);
                LoadNext();
            }
        }

        /// <summary>
        /// 加载列表排序函数
        /// </summary>
        /// <param name="loaderItemA"></param>
        /// <param name="loaderItemB"></param>
        /// <returns></returns>
        private int LoadListSortFunc(MapLoaderItem loaderItemA, MapLoaderItem loaderItemB)
        {
            float distAX = Math.Abs(loaderItemA.mapX - _center.x);
            float distAY = Math.Abs(loaderItemA.mapY - _center.y);
            float distBX = Math.Abs(loaderItemB.mapX - _center.x);
            float distBY = Math.Abs(loaderItemB.mapY - _center.y);
            float distA = distAX > distAY ? distAX : distAY;
            float distB = distBX > distBY ? distBX : distBY;
            if (distA > distB)
                return -1;
            else if (distA < distB)
                return 1;
            else if (distAY > distBY)
                return -1;
            else if (distAY < distBY)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// 加载地图切片
        /// </summary>
        private void LoadNext()
        {
            while (_loadCount < MapSetting.maxLoadCount)
            {
                if (_loadList.Count <= 0)
                    return;

                int index = _loadList.Count - 1;
                MapLoaderItem loaderItem = _loadList[index];
                _loadList.RemoveAt(index);
                _loadDict.Remove(loaderItem.tile);

                string url = _mapVo.GetTileRelativePath(loaderItem.tile.x, loaderItem.tile.y);
                if (!_loadingDict.ContainsKey(url))
                {
                    MapLoader loader = _loaderPool.Get();
                    _loadingDict.Add(url, loader);
                    loader.loaderItem = loaderItem;
                    loader.completeCallback = OnTileComplete;
                    loader.errorCallback = OnTileError;
                    loader.StartLoad(url);

                    _loadCount++;
                }
            }
        }

        /// <summary>
        /// 地图切片加载完成
        /// </summary>
        /// <param name="loader"></param>
        private void OnTileComplete(MapLoader loader)
        {
            TextureAsset asset = loader.loader.asset as TextureAsset;
            if (asset != null)
            {
                MapLoaderItem loadItem = loader.loaderItem;
                if (loadItem.mapId == _mapVo.id)
                {
                    MapTile tile = loadItem.tile;
                    asset.texture.wrapMode = TextureWrapMode.Clamp;
                    // 资源存入缓存
                    _textureDict[tile] = asset;
                    // 资源保存本地
                    //App.fileManager.WriteFilePersistentAsync(loader.loader.urlRelative, loader.loader.bytes, false);
                    // 渲染图片
                    if (_renderTiles.ContainsKey(tile))
                    {
                        MapBlock block = GetBlock(tile.x - _currentTileX, tile.y - _currentTileY);
                        block.SetTexture(asset.texture);
                        _renderTiles.Remove(tile);

                        if (_loadDict.Contains(tile))
                        {
                            _loadList.Remove(loadItem);
                            _loadDict.Remove(tile);
                        }
                    }
                }
                else
                {
                    asset.Dispose();
                }
            }

            // 移除加载字典
            if (_loadingDict.ContainsKey(loader.loader.urlRelative))
                _loadingDict.Remove(loader.loader.urlRelative);
            _loaderPool.Release(loader);

            // 继续加载
            _loadCount--;
            LoadNext();
        }

        /// <summary>
        /// 地图切片加载失败
        /// </summary>
        /// <param name="loader"></param>
        private void OnTileError(MapLoader loader)
        {
            if (loader.loader.error != "timeOut")
            {
                App.logManager.Warn("Map Load Tile Error: urlAbsolute:\"" + loader.loader.urlAbsolute + "\" urlRelative:\"" + loader.loader.urlRelative + "\" Message:" + loader.loader.error);
            }

            // 移除加载字典
            if (_loadingDict.ContainsKey(loader.loader.urlRelative))
                _loadingDict.Remove(loader.loader.urlRelative);
            _loaderPool.Release(loader);

            // 继续加载
            _loadCount--;
            LoadNext();
        }

        /// <summary>
        /// 设置中心点
        /// </summary>
        /// <param name="pos"></param>
        private void SetCenter(Vector2 pos)
        {
            _center = pos;
            _leftTop.x = _center.x - _mapVo.renderWidthHalf;
            _leftTop.y = _center.y - _mapVo.renderHeightHalf;
        }

        /// <summary>
        /// 根据索引获取MapBlock
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private MapBlock GetBlock(int x, int y)
        {
            //MapBlock block;
            //if (_blocks.TryGetValue(x + "_" + y, out block))
            //    return block;

            //return null;

            return _blocks[x, y];
        }

        /// <summary>
        /// 移动地图到指定坐标点
        /// </summary>
        /// <param name="position"></param>
        public void MoveTo(Vector2 position)
        {
            if (_mapVo == null)
                return;

            SetCenter(position);
            Render();
        }

        /// <summary>
        /// 移动地图到指定坐标点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveTo(float x, float y)
        {
            MoveTo(new Vector2(x, y));
        }

        /// <summary>
        /// 缓动移动到地图指定坐标点
        /// </summary>
        /// <param name="position"></param>
        /// <param name="time"></param>
        /// <param name="ease"></param>
        /// <param name="callback"></param>
        public void MoveTo(Vector2 position, float time, Ease ease = Ease.OutQuad, TweenCallback callback = null)
        {
            if (_tweener != null)
                _tweener.Kill();

            _tweener = DOTween.To(() => _center, pos => MoveTo(pos), position, time);
            _tweener.SetEase(ease);
            _tweener.onComplete = callback;
        }

        /// <summary>
        /// 缓动移动到地图指定坐标点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="time"></param>
        /// <param name="ease"></param>
        /// <param name="callback"></param>
        public void MoveTo(float x, float y, float time, Ease ease = Ease.OutQuad, TweenCallback callback = null)
        {
            MoveTo(new Vector2(x, y), time, ease, callback);
        }

        /// <summary>
        /// 移动地图到指定坐标点
        /// </summary>
        /// <param name="position"></param>
        public void MoveTo(Vector3 position)
        {
            MoveTo(position.x, position.y);
        }

        /// <summary>
        /// 移动到偏移坐标点
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public void MoveToOffset(float offsetX, float offsetY)
        {
            MoveTo(_center.x + offsetX, _center.y + offsetY);
        }

        /// <summary>
        /// 移动到屏幕坐标点
        /// </summary>
        /// <param name="screenX"></param>
        /// <param name="screenY"></param>
        public void MoveToScreen(float screenX, float screenY)
        {
            MoveTo(_center.x + screenX - _mapVo.renderWidthHalf, _center.y + screenY - _mapVo.renderWidthHalf);
        }

        /// <summary>
        /// 根据地图坐标获取当前屏幕坐标
        /// </summary>
        /// <param name="mapX"></param>
        /// <param name="mapY"></param>
        /// <returns></returns>
        public Vector2 GetScreenPosition(float mapX, float mapY)
        {
            return new Vector2(_mapVo.renderWidthHalf + mapX - _center.x, _mapVo.renderHeightHalf + mapY - _center.y);
        }

        /// <summary>
        /// 根据屏幕坐标获取地图坐标
        /// </summary>
        /// <param name="screenX"></param>
        /// <param name="screenY"></param>
        /// <returns></returns>
        public Vector2 GetMapPosition(float screenX, float screenY)
        {
            return new Vector2(_center.x + screenX - _mapVo.renderWidthHalf, _center.y + screenY - _mapVo.renderHeightHalf);
        }
    }
}
