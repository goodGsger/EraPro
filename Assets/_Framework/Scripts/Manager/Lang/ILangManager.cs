using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface ILangManager : IManager
    {
        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="key"></param>
        /// <param name="text"></param>
        void SetLang(int key, string text);

        /// <summary>
        /// 获取翻译
        /// </summary>
        /// <param name="key"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        string GetLang(int key, params string[] param);

        /// <summary>
        /// 移除翻译
        /// </summary>
        /// <param name="key"></param>
        void RemoveLang(int key);
    }
}
