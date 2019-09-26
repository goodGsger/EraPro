using FairyGUI;
using FairyGUI.Utils;

namespace FairyGUI
{
    public class EmojiParser : UBBParser
    {
        static EmojiParser _instance;
        public new static EmojiParser inst
        {
            get
            {
                if (_instance == null)
                    _instance = new EmojiParser();
                return _instance;
            }
        }

        private static string[] TAGS = new string[]
            { "#01", "#02", "#03", "#04", "#05", "#06", "#07", "#08", "#09", "#10", "#11", "#12", "#13", "#14", "#15", "#16", "#17", "#18", "#19", "#20", "#21", "#22", "#23", "#24", "#25", "#26"};
        public EmojiParser()
        {
            foreach (string ss in TAGS)
            {
                this.handlers[":" + ss] = OnTag_Emoji;
            }
        }

        string OnTag_Emoji(string tagName, bool end, string attr)
        {
            return "<img src='" + UIPackage.GetItemURL("Emoji", tagName.Substring(1).ToLower()) + "'/>";
        }
    }
}
