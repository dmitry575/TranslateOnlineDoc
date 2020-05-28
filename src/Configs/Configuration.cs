
namespace TranslateOnlineDoc.Configs
{
    public class Configuration
    {
        /// <summary>
        /// Default page to translate
        /// </summary>
        public const string UrlTranslate = "https://www.onlinedoctranslator.com/{0}/translationform";

        /// <summary>
        /// From language translate
        /// flag /from
        /// </summary>
        public string FromLang { get; } = "en";

        /// <summary>
        /// To language translate
        /// flag /to
        /// </summary>
        public string ToLang { get; } = "ru";

        /// <summary>
        /// Directories where files store
        /// /dir
        /// </summary>
        public string DirSrc { get; } = "./";

        /// <summary>
        /// Directories where files store
        /// /output
        /// </summary>
        public string DirOutput { get; } = "./";

        public Configuration(string[] args)
        {
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i].ToLower())
                    {
                        case "/from":
                            FromLang = args[++i];
                            break;

                        case "/to":
                            ToLang = args[++i];
                            break;

                        case "/dir":
                            DirSrc = args[++i];
                            break;

                        case "/output":
                            DirOutput = args[++i];
                            break;
                    }
                }
            }
        }

        public string GetUrlTranslate()
        {
            return string.Format(UrlTranslate, FromLang);
        }
    }
}
