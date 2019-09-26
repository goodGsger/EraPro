using FairyGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class FairyGUIHelper
    {
        /// <summary>
        /// 添加资源包
        /// </summary>
        /// <param name="name"></param>
        public static void AddPackage(string name)
        {
            UIPackage.AddPackage(name, (string assetName, string extension, Type type) =>
            {
                string fileName = assetName + extension;
                byte[] bytes = FileHelper.ReadFile(Application.dataPath + "/_GameData/UI/" + fileName);
                if (type == typeof(TextAsset))
                {
                    return bytes;
                }
                else if (type == typeof(Texture2D) || type == typeof(Texture))
                {
                    Texture2D texture = new Texture2D(1, 1);
                    texture.wrapMode = TextureWrapMode.Clamp;
                    texture.filterMode = FilterMode.Bilinear;
                    texture.LoadImage(bytes);
                    return texture;
                }
                else if (type == typeof(AudioClip))
                {
                    // TODO 字节数组转AudioClip
                    return null;
                }
                else
                {
                    return Resources.Load(assetName, type);
                }
            });
        }

        /// <summary>
        /// 添加资源包
        /// </summary>
        /// <param name="name"></param>
        public static void AddPackage(AssetBundle ab)
        {
            UIPackage.AddPackage(ab, false);
        }

        /// <summary>
        /// 添加资源包
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="res"></param>
        public static void AddPackage(AssetBundle desc, AssetBundle res)
        {
            UIPackage.AddPackage(desc, res, false);
        }

        /// <summary>
        /// 添加资源包
        /// </summary>
        /// <param name="mainAssetName"></param>
        /// <param name="source"></param>
        /// <param name="res"></param>
        public static void AddPackage(string mainAssetName, byte[] source, AssetBundle res)
        {
            UIPackage.AddPackage(mainAssetName, source, res, false);
        }

        /// <summary>
        /// 移除资源包
        /// </summary>
        /// <param name="name"></param>
        /// <param name="allowDestroyingAssets"></param>
        public static void RemovePackage(string name, bool allowDestroyingAssets = true)
        {
            UIPackage.RemovePackage(name, allowDestroyingAssets);
        }

        /// <summary>
        /// 本地坐标转全局坐标
        /// </summary>
        /// <param name="go"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector2 LocalToGlobal(GObject go, float x, float y)
        {
            return go.LocalToGlobal(new Vector2(x, y));
        }

        /// <summary>
        /// 全局坐标转本地坐标
        /// </summary>
        /// <param name="go"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector2 GlobalToLocal(GObject go, float x, float y)
        {
            return go.GlobalToLocal(new Vector2(x, y));
        }

        /// <summary>
        /// 设置SortingLayer
        /// </summary>
        /// <param name="gObject"></param>
        /// <param name="sortingLayerName"></param>
        public static void SetSortingLayer(GObject gObject, string sortingLayerName)
        {

            if (gObject is GComponent)
            {
                GObject[] children = (gObject as GComponent).GetChildren();
                foreach (GObject child in children)
                {
                    SetSortingLayer(child, sortingLayerName);
                }
            }
            else
            {
                DisplayObject displayObject = null;
                if (gObject is GLoader)
                    displayObject = (gObject as GLoader).image;
                else
                    displayObject = gObject.displayObject;

                SetDisplayObjectSortingLayer(displayObject, sortingLayerName);
            }
        }

        private static void SetDisplayObjectSortingLayer(DisplayObject displayObject, string sortingLayerName)
        {
            displayObject.graphics.meshRenderer.sortingLayerName = sortingLayerName;
            if (displayObject.paintingGraphics != null)
                displayObject.paintingGraphics.meshRenderer.sortingLayerName = sortingLayerName;
        }

        /// <summary>
        /// 设置SortingLayer
        /// </summary>
        /// <param name="gObject"></param>
        /// <param name="sortingLayerName"></param>
        public static void SetSortingOrder(GObject gObject, int order)
        {

            if (gObject is GComponent)
            {
                GObject[] children = (gObject as GComponent).GetChildren();
                foreach (GObject child in children)
                {
                    SetSortingOrder(child, order);
                }
            }
            else
            {
                DisplayObject displayObject = null;
                if (gObject is GLoader)
                    displayObject = (gObject as GLoader).image;
                else
                    displayObject = gObject.displayObject;

                SetDisplayObjectSortingOrder(displayObject, order);
            }
        }

        private static void SetDisplayObjectSortingOrder(DisplayObject displayObject, int order)
        {
            if (displayObject == null || displayObject.graphics == null)
                return;
            displayObject.useNativeSorting = true;
            displayObject.renderingOrder = order; //.graphics.meshRenderer.sortingOrder = order;
            if (displayObject.paintingGraphics != null)
                displayObject.paintingGraphics.meshRenderer.sortingOrder = order;
        }

        /// <summary>
        /// 遍历获得子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static GComponent GetChild(GComponent parent, string childName)
        {
            GComponent comp = parent.GetChild(childName) as GComponent;
            if (comp != null)
                return comp;

            GComponent obj = null;
            foreach (GObject child in parent.GetChildren())
            {
                if (child is GComponent)
                {
                    obj = GetChild(child as GComponent, childName);
                    if (obj != null)
                        break;
                }
            }
            return obj;
        }
    }
}
