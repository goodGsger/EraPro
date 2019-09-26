using LuaInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    /// <summary>
    /// 数据表管理器，通过反射填充数据表
    /// </summary>
    public interface ITableManager : IManager
    {
        /// <summary>
        /// Txt数据表结构目录地址
        /// </summary>
        string txtStructFolder { get; set; }

        /// <summary>
        /// JSON数据表结构目录地址
        /// </summary>
        string jsonStructFolder { get; set; }

        /// <summary>
        /// XML数据表结构目录地址
        /// </summary>
        string xmlStructFolder { get; set; }

        /// <summary>
        /// 根据表名获取Txt数据表字典
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Dictionary<int, ITxtDataTable> GetTxtDataTables(string name);

        /// <summary>
        /// 根据表名获取JSON数据表字典
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Dictionary<int, IJSONTable> GetJSONTables(string name);

        /// <summary>
        /// 根据表名获取XML数据表字典
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Dictionary<int, IXMLTable> GetXMLTables(string name);

        /// <summary>
        /// 根据表名、ID获取Txt数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        ITxtDataTable GetTxtDataTable(string name, int ID);

        /// <summary>
        /// 根据表名、ID获取Txt数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        T GetTxtDataTable<T>(string name, int ID) where T : ITxtDataTable;

        /// <summary>
        /// 根据表名、ID获取JSON数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        IJSONTable GetJSONTable(string name, int ID);

        /// <summary>
        /// 根据表名、ID获取JSON数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        T GetJSONTable<T>(string name, int ID) where T : IJSONTable;

        /// <summary>
        /// 根据表名、ID获取XML数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        IXMLTable GetXMLTable(string name, int ID);

        /// <summary>
        /// 根据表名、ID获取XML数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        T GetXMLTable<T>(string name, int ID) where T : IXMLTable;

        /// <summary>
        /// 注册Txt数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="txtStr"></param>
        void RegisterTxtTables(string name, string txtStr);

        /// <summary>
        /// 注册Txt数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="txtStr"></param>
        void RegisterTxtTable(string name, string[] txtStr);

        /// <summary>
        /// 注册Txt数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="txtStr"></param>
        /// <param name="ID"></param>
        void RegisterTxtTable(string name, string[] txtStr, int ID);

        /// <summary>
        /// 注销Txt数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        void UnregisterTxtTable(string name, int ID);

        /// <summary>
        /// 注册JSON数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="jsonStr"></param>
        void RegisterJSONTable(string name, string jsonStr);

        /// <summary>
        /// 注册JSON数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="jsonStr"></param>
        /// <param name="ID"></param>
        void RegisterJSONTable(string name, string jsonStr, int ID);

        /// <summary>
        /// 注销JSON数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        void UnregisterJSONTable(string name, int ID);

        /// <summary>
        /// 注册XML数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xmlStr"></param>
        void RegisterXMLTable(string name, string xmlStr);

        /// <summary>
        /// 注册XML数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xmlStr"></param>
        /// <param name="ID"></param>
        void RegisterXMLTable(string name, string xmlStr, int ID);

        /// <summary>
        /// 注销XML数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        void UnregisterXMLTable(string name, int ID);
    }
}
