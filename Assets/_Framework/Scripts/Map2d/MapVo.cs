using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class MapVo
    {
        public string id;
        public int renderWidth;
        public int renderHeight;
        public int tileWidth;
        public int tileHeight;
        public int mapWidth;
        public int mapHeight;
        public int maxTileX;
        public int maxTileY;

        public int renderWidthHalf;
        public int renderHeightHalf;
        public float minX;
        public float minY;
        public float maxX;
        public float maxY;

        public MapVo()
        {

        }

        /// <summary>
        /// 缩略图相对路径
        /// </summary>
        public string ThumbnailRelativePath
        {
            get
            {
                if (MapSetting.assetBundleMode)
                    return new StringBuilder(App.pathManager.map_ab).Append("thumbnails/").Append(id).Append(".ab").ToString();
                else
                    return new StringBuilder(App.pathManager.map).Append("thumbnails/").Append(id).Append(".jpg").ToString();
            }
        }

        public string ThumbnailRelativePathJPG
        {
            get
            {
                return new StringBuilder(App.pathManager.map).Append("thumbnails/").Append(id).Append(".jpg").ToString();
            }
        }

        /// <summary>
        /// 刷新渲染尺寸
        /// </summary>
        public void RefreshRenderSize()
        {
            renderWidthHalf = (int)(renderWidth * 0.5);
            renderHeightHalf = (int)(renderHeight * 0.5);
            minX = renderWidthHalf;
            minY = renderHeightHalf;
            maxX = mapWidth - renderWidthHalf;
            maxY = mapHeight - renderHeightHalf;
        }

        /// <summary>
        /// 获取切片唯一Key
        /// </summary>
        /// <param name="tileX"></param>
        /// <param name="tileY"></param>
        /// <returns></returns>
        public string GetTileKey(int tileX, int tileY)
        {
            return new StringBuilder(id).Append("_").Append(tileY).Append("_").Append(tileX).ToString();
        }

        /// <summary>
        /// 获取切片相对路径
        /// </summary>
        /// <param name="tileX"></param>
        /// <param name="tileY"></param>
        /// <returns></returns>
        public string GetTileRelativePath(int tileX, int tileY)
        {
            if (MapSetting.assetBundleMode)
                return new StringBuilder(App.pathManager.map_ab).Append("tiles/").Append(id).Append("/").Append(tileY).Append("_").Append(tileX).Append(".ab").ToString();
            else
                return new StringBuilder(App.pathManager.map).Append("tiles/").Append(id).Append("/").Append(tileY).Append("_").Append(tileX).Append(".jpg").ToString();
        }
    }
}
