using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI_Design
{
    internal class GameInfo
    {
        public int BoardId { get; set; }
        public string GameType { get; set; }
        public int ThreadId { get; set; }
        public string title { get; set; }
        public int readCount { get; set; }
        public int likeCount { get; set; }
        public string userName { get; set; }
        public int createTime { get; set; }
        public int modifyTime { get; set; }
    }
}
