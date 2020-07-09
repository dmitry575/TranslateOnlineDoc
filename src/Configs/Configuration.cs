
namespace TranslateOnlineDoc.Configs
{
    public class Configuration
    {
        /// <summary>
        /// Default page to translate
        /// </summary>
        public const string UrlTranslate = "https://www.onlinedoctranslator.com/{0}/translationform";

        /// <summary>
        /// MAximal working threads
        /// </summary>
        private const int MAX_THREADS = 10;

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

        /// <summary>
        /// Counts of work threads
        /// /threads
        /// </summary>
        public int Threads { get; } = 3;

        /// <summary>
        /// Timeout for load page and another element
        /// /timeout
        /// </summary>
        public int Timeout { get; } = 300;

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

                        case "/timeout":
                            if (int.TryParse(args[++i], out var temp))
                            {
                                Timeout = temp;
                            }
                            break;

                        case "/threads":
                            if (int.TryParse(args[++i], out var tempt))
                            {
                                if (tempt > MAX_THREADS) tempt = MAX_THREADS;
                                Threads = tempt;
                            }
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
