using Mono.Xml;
using System.Security;

namespace Framework
{
    public class XML
    {
        private SecurityParser _parser;
        private SecurityElement _element;

        public XML()
        {
            _parser = new SecurityParser();
        }

        public XML(string text) : this()
        {
            SetText(text);
        }

        /// <summary>
        /// 转换器
        /// </summary>
        public SecurityParser parser
        {
            get { return _parser; }
        }

        /// <summary>
        /// 节点
        /// </summary>
        public SecurityElement element
        {
            get { return _element; }
        }

        /// <summary>
        /// 设置字符串
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            _parser.LoadXml(text);
            _element = _parser.ToXml();
        }

        /// <summary>
        /// 转换为string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_element != null)
                return _element.ToString();

            return "";
        }
    }
}
