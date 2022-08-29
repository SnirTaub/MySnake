using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mySnake
{
    [Serializable]
    class Settings
    {
        public static int Height { get; set; }
        public static int Width { get; set; }

        public static string _directions;
        public Settings()
        {
            Height = 16;
            Width = 16;
            _directions = "left";
        }
    }
}
