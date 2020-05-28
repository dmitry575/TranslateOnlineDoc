using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TranslateOnlineDoc.Common
{
    public class Files
    {
        private readonly string _dir;

        public Files(string dir)
        {
            _dir = dir;
        }

        public List<string> GetList()
        {
            return Directory.GetFiles(_dir).ToList();
        }
    }
}
