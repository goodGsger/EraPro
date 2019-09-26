using LuaInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class TableManager : Manager, ITableManager
    {
        // 数据表结构目录地址
        private string _txtStructFolder;
        private string _jsonStructFolder;
        private string _xmlStructFolder;

        // 数据字典
        private Dictionary<string, Dictionary<int, ITxtDataTable>> _txtDataTableDict;
        private Dictionary<string, Dictionary<int, IJSONTable>> _jsonTableDict;
        private Dictionary<string, Dictionary<int, IXMLTable>> _xmlTableDict;

        // 原型字典
        private Dictionary<string, ITxtDataTable> _txtDataTablePrototypeDict;
        private Dictionary<string, IJSONTable> _jsonTablePrototypeDict;
        private Dictionary<string, IXMLTable> _xmlTablePrototypeDict;

        protected override void Init()
        {
            _txtDataTableDict = new Dictionary<string, Dictionary<int, ITxtDataTable>>();
            _jsonTableDict = new Dictionary<string, Dictionary<int, IJSONTable>>();
            _xmlTableDict = new Dictionary<string, Dictionary<int, IXMLTable>>();

            _txtDataTablePrototypeDict = new Dictionary<string, ITxtDataTable>();
            _jsonTablePrototypeDict = new Dictionary<string, IJSONTable>();
            _xmlTablePrototypeDict = new Dictionary<string, IXMLTable>();
        }

        /// <summary>
        /// txt数据表结构目录地址
        /// </summary>
        public string txtStructFolder
        {
            get { return _txtStructFolder; }
            set { _txtStructFolder = value; }
        }

        /// <summary>
        /// JSON数据表结构目录地址
        /// </summary>
        public string jsonStructFolder
        {
            get { return _jsonStructFolder; }
            set { _jsonStructFolder = value; }
        }

        /// <summary>
        /// XML数据表结构目录地址
        /// </summary>
        public string xmlStructFolder
        {
            get { return _xmlStructFolder; }
            set { _xmlStructFolder = value; }
        }

        /// <summary>
        /// 根据表名获取Txt数据表字典
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Dictionary<int, ITxtDataTable> GetTxtDataTables(string name)
        {
            if (_txtDataTableDict.TryGetValue(name, out Dictionary<int, ITxtDataTable> dict))
                return dict;

            return null;
        }

        /// <summary>
        /// 根据表名获取JSON数据表字典
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Dictionary<int, IJSONTable> GetJSONTables(string name)
        {
            if (_jsonTableDict.TryGetValue(name, out Dictionary<int, IJSONTable> dict))
                return dict;

            return null;
        }

        /// <summary>
        /// 根据表名获取XML数据表字典
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Dictionary<int, IXMLTable> GetXMLTables(string name)
        {
            if (_xmlTableDict.TryGetValue(name, out Dictionary<int, IXMLTable> dict))
                return dict;

            return null;
        }

        /// <summary>
        /// 根据表名、ID获取Txt数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ITxtDataTable GetTxtDataTable(string name, int ID)
        {
            Dictionary<int, ITxtDataTable> dict = GetTxtDataTables(name);
            if (dict != null)
            {
                if (dict.TryGetValue(ID, out ITxtDataTable txtDataTable))
                    return txtDataTable;
            }

            return null;
        }

        /// <summary>
        /// 根据表名、ID获取Txt数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T GetTxtDataTable<T>(string name, int ID) where T : ITxtDataTable
        {
            ITxtDataTable table = GetTxtDataTable(name, ID);
            return (T)table;
        }

        /// <summary>
        /// 根据表名、ID获取JSON数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IJSONTable GetJSONTable(string name, int ID)
        {
            Dictionary<int, IJSONTable> dict = GetJSONTables(name);
            if (dict != null)
            {
                if (dict.TryGetValue(ID, out IJSONTable jsonTable))
                    return jsonTable;
            }

            return null;
        }

        /// <summary>
        /// 根据表名、ID获取JSON数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T GetJSONTable<T>(string name, int ID) where T : IJSONTable
        {
            IJSONTable table = GetJSONTable(name, ID);
            return (T)table;
        }

        /// <summary>
        /// 根据表名、ID获取XML数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IXMLTable GetXMLTable(string name, int ID)
        {
            Dictionary<int, IXMLTable> dict = GetXMLTables(name);
            if (dict != null)
            {
                if (dict.TryGetValue(ID, out IXMLTable xmlTable))
                    return xmlTable;
            }

            return null;
        }

        /// <summary>
        /// 根据表名、ID获取XML数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T GetXMLTable<T>(string name, int ID) where T : IXMLTable
        {
            IXMLTable table = GetXMLTable(name, ID);
            return (T)table;
        }

        /// <summary>
        /// 注册Txt数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="txtStr"></param>
        public void RegisterTxtTables(string name, string txtStr)
        {
            if (_txtStructFolder == null)
            {
                App.logManager.Error("TableManager.RegisterTxtTables Error:TxtStructFolder has not set！");
                return;
            }

            string[] tableLines = txtStr.Split(StringUtil.WARP, StringSplitOptions.None);
            foreach (string line in tableLines)
                RegisterTxtTable(name, line.Split('\t'));
        }

        /// <summary>
        /// 注册Txt数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="txtStr"></param>
        public void RegisterTxtTable(string name, string[] txtStr)
        {
            if (_txtStructFolder == null)
            {
                App.logManager.Error("TableManager.RegisterTxtTable Error:TxtStructFolder has not set！");
                return;
            }

            // 判断原型字典中是否存在该数据原型
            ITxtDataTable txtDataTable = GetTxtDataTablePrototype(name);
            if (txtDataTable == null)
                txtDataTable = (ITxtDataTable)ReflectionHelper.CreateInstance(_txtStructFolder + name);
            if (txtDataTable == null)
                return;
            _txtDataTablePrototypeDict[name] = txtDataTable;

            // 获取数据表字典
            Dictionary<int, ITxtDataTable> dict;
            if (_txtDataTableDict.TryGetValue(name, out dict) == false)
                dict = _txtDataTableDict[name] = new Dictionary<int, ITxtDataTable>();

            // 根据原型生成新数据并填充数据
            txtDataTable = txtDataTable.ClonePrototype();
            dict[txtDataTable.FillData(txtStr)] = txtDataTable;
            txtDataTable.InitData();
        }

        /// <summary>
        /// 注册Txt数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="table"></param>
        /// <param name="ID"></param>
        public void RegisterTxtTable(string name, string[] txtStr, int ID)
        {
            if (_txtStructFolder == null)
            {
                App.logManager.Error("TableManager.RegisterTxtTable Error:TxtStructFolder has not set！");
                return;
            }

            Dictionary<int, ITxtDataTable> dict;
            if (_txtDataTableDict.TryGetValue(name, out dict) == false)
                dict = _txtDataTableDict[name] = new Dictionary<int, ITxtDataTable>();

            ITxtDataTable txtDataTable;
            // 已存在该数据表则覆盖数据
            if (dict.TryGetValue(ID, out txtDataTable))
                txtDataTable.FillData(txtStr);
            else
            {
                // 未存在数据表则创建新数据
                // 判断原型字典中是否存在该数据原型
                txtDataTable = GetTxtDataTablePrototype(name);
                if (txtDataTable == null)
                    txtDataTable = (ITxtDataTable)ReflectionHelper.CreateInstance(_txtStructFolder + name);
                if (txtDataTable == null)
                    return;

                _txtDataTablePrototypeDict[name] = txtDataTable;

                // 根据原型生成新数据并填充数据
                txtDataTable = dict[ID] = txtDataTable.ClonePrototype();
                txtDataTable.FillData(txtStr);
                txtDataTable.InitData();
            }
        }

        /// <summary>
        /// 注销Txt数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        public void UnregisterTxtTable(string name, int ID)
        {
            if (_txtDataTableDict.TryGetValue(name, out Dictionary<int, ITxtDataTable> dict))
            {
                if (dict.ContainsKey(ID))
                    dict.Remove(ID);
            }
        }

        /// <summary>
        /// 注册JSON数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xmlStr"></param>
        public void RegisterJSONTable(string name, string jsonStr)
        {
            if (_jsonStructFolder == null)
            {
                App.logManager.Error("TableManager.RegisterJSONTable Error:JSONStructFolder has not set！");
                return;
            }

            // 判断原型字典中是否存在该数据原型
            IJSONTable jsonTable = GetJSONTablePrototype(name);
            if (jsonTable == null)
                jsonTable = (IJSONTable)ReflectionHelper.CreateInstance(_jsonStructFolder + name);
            if (jsonTable == null)
                return;
            _jsonTablePrototypeDict[name] = jsonTable;

            // 获取数据表字典
            Dictionary<int, IJSONTable> dict;
            if (_jsonTableDict.TryGetValue(name, out dict) == false)
                dict = _jsonTableDict[name] = new Dictionary<int, IJSONTable>();

            // 根据原型生成新数据并填充数据
            jsonTable = jsonTable.ClonePrototype();
            dict[jsonTable.FillData(jsonStr)] = jsonTable;
            jsonTable.InitData();
        }

        /// <summary>
        /// 注册JSON数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="jsonStr"></param>
        /// <param name="ID"></param>
        public void RegisterJSONTable(string name, string jsonStr, int ID)
        {
            if (_jsonStructFolder == null)
            {
                App.logManager.Error("TableManager.RegisterJSONTable Error:JSONStructFolder has not set！");
                return;
            }

            Dictionary<int, IJSONTable> dict;
            if (_jsonTableDict.TryGetValue(name, out dict) == false)
                dict = _jsonTableDict[name] = new Dictionary<int, IJSONTable>();

            IJSONTable jsonTable;
            // 已存在该数据表则覆盖数据
            if (dict.TryGetValue(ID, out jsonTable))
                jsonTable.FillData(jsonStr);
            else
            {
                // 未存在数据表则创建新数据
                // 判断原型字典中是否存在该数据原型
                jsonTable = GetJSONTablePrototype(name);
                if (jsonTable == null)
                    jsonTable = (IJSONTable)ReflectionHelper.CreateInstance(_jsonStructFolder + name);
                if (jsonTable == null)
                    return;

                // 根据原型生成新数据并填充数据
                jsonTable = dict[ID] = jsonTable.ClonePrototype();
                jsonTable.FillData(jsonStr);
                jsonTable.InitData();
            }
        }

        /// <summary>
        /// 注销JSON数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        public void UnregisterJSONTable(string name, int ID)
        {
            if (_jsonTableDict.TryGetValue(name, out Dictionary<int, IJSONTable> dict))
            {
                if (dict.ContainsKey(ID))
                    dict.Remove(ID);
            }
        }

        /// <summary>
        /// 注册JSON数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xmlStr"></param>
        public void RegisterXMLTable(string name, string xmlStr)
        {
            if (_xmlStructFolder == null)
            {
                App.logManager.Error("TableManager.RegisterXMLTable Error:XMLStructFolder has not set！");
                return;
            }

            // 判断原型字典中是否存在该数据原型
            IXMLTable xmlTable = GetXMLTablePrototype(name);
            if (xmlTable == null)
                xmlTable = (IXMLTable)ReflectionHelper.CreateInstance(_xmlStructFolder + name);
            if (xmlTable == null)
                return;
            _xmlTablePrototypeDict[name] = xmlTable;

            // 获取数据表字典
            Dictionary<int, IXMLTable> dict;
            if (_xmlTableDict.TryGetValue(name, out dict) == false)
                dict = _xmlTableDict[name] = new Dictionary<int, IXMLTable>();

            // 根据原型生成新数据并填充数据
            xmlTable = xmlTable.ClonePrototype();
            dict[xmlTable.FillData(xmlStr)] = xmlTable;
            xmlTable.InitData();
        }

        /// <summary>
        /// 注册XML数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xmlStr"></param>
        /// <param name="ID"></param>
        public void RegisterXMLTable(string name, string xmlStr, int ID)
        {
            if (_xmlStructFolder == null)
            {
                App.logManager.Error("TableManager.RegisterXMLTable Error:XMLStructFolder has not set！");
                return;
            }

            Dictionary<int, IXMLTable> dict;
            if (_xmlTableDict.TryGetValue(name, out dict) == false)
                dict = _xmlTableDict[name] = new Dictionary<int, IXMLTable>();

            IXMLTable xmlTable;
            // 已存在该数据表则覆盖数据
            if (dict.TryGetValue(ID, out xmlTable))
                xmlTable.FillData(xmlStr);
            else
            {
                // 未存在数据表则创建新数据
                // 判断原型字典中是否存在该数据原型
                xmlTable = GetXMLTablePrototype(name);
                if (xmlTable == null)
                    xmlTable = (IXMLTable)ReflectionHelper.CreateInstance(_xmlStructFolder + name);
                if (xmlTable == null)
                    return;

                // 根据原型生成新数据并填充数据
                xmlTable = dict[ID] = xmlTable.ClonePrototype();
                xmlTable.FillData(xmlStr);
                xmlTable.InitData();
            }
        }

        /// <summary>
        /// 注销XML数据表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        public void UnregisterXMLTable(string name, int ID)
        {
            if (_xmlTableDict.TryGetValue(name, out Dictionary<int, IXMLTable> dict))
            {
                if (dict.ContainsKey(ID))
                    dict.Remove(ID);
            }
        }

        /// <summary>
        /// 获取数据表原型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected ITxtDataTable GetTxtDataTablePrototype(string name)
        {
            if (_txtDataTablePrototypeDict.TryGetValue(name, out ITxtDataTable prototype))
                return prototype;

            return null;
        }

        /// <summary>
        /// 获取JSON数据表原型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected IJSONTable GetJSONTablePrototype(string name)
        {
            if (_jsonTablePrototypeDict.TryGetValue(name, out IJSONTable prototype))
                return prototype;

            return null;
        }

        /// <summary>
        /// 获取XML数据表原型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected IXMLTable GetXMLTablePrototype(string name)
        {
            if (_xmlTablePrototypeDict.TryGetValue(name, out IXMLTable prototype))
                return prototype;

            return null;
        }
    }
}
