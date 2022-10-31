using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading;
using System.Windows.Forms;

namespace UI_Design.Class
{
    internal class SQLiteControl
    {
        private SQLiteConnection conn = null;
        public List<GameInfo> SQLiteSetup()
        {
            List<GameInfo> gameInfos = new List<GameInfo>();
            conn = new SQLiteConnection("Data Source=C:\\Users\\KRKimwoo\\source\\repos\\UI Design\\database.db;Version=3;");
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM GameInfo", conn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                gameInfos.Add(new GameInfo()
                {
                    BoardId = reader.GetInt32(0),
                    GameType = reader.GetString(1),
                    ThreadId = reader.GetInt32(2),
                    title = reader.GetString(3),
                    readCount = reader.GetInt32(4),
                    likeCount = reader.GetInt32(5),
                    userName = reader.GetString(6),
                    createTime = reader.GetInt32(7),
                    modifyTime = reader.GetInt32(8)
                });
            }
            reader.Close();
            conn.Close();
            return gameInfos;
        }
        public void SQLiteClose(List<GameInfo> gameInfos)
        {
            conn = new SQLiteConnection("Data Source=C:\\Users\\KRKimwoo\\source\\repos\\UI Design\\database.db;Version=3;");
            conn.Open();
            foreach (var gameInfo in gameInfos)
            {
                SQLiteCommand valuecmd = new SQLiteCommand("SELECT * FROM GameInfo WHERE GameType = '" + gameInfo.GameType + "'", conn);
                SQLiteDataReader reader = valuecmd.ExecuteReader();
                int count = 0;
                while (reader.Read())
                    count += 1;
                reader.Close();
                if (count == 0)
                {
                    SQLiteCommand insertcmd = new SQLiteCommand("INSERT INTO GameInfo (BoardId, GameType, ThreadId, title, readCount, likeCount, userName, createTime, modifyTime) " +
                        "values (" + gameInfo.BoardId +", '" + gameInfo.GameType + "'," + gameInfo.ThreadId + ", '" + gameInfo.title + "', " + gameInfo.readCount + ", " + gameInfo.likeCount + ", " + gameInfo.userName + ", " + gameInfo.createTime + ", " + gameInfo.modifyTime, conn);
                    insertcmd.ExecuteNonQuery();
                }
                else
                {
                    SQLiteCommand updatecmd = new SQLiteCommand("UPDATE GameInfo SET ThreadId = " + gameInfo.ThreadId + ", title = '" + gameInfo.title + "', readCount = " + gameInfo.readCount + ", likeCount = " + gameInfo.likeCount + ", userName = '" + gameInfo.userName + "', createTime = " + gameInfo.createTime + ", modifyTime = " + gameInfo.modifyTime + " WHERE BoardId = " + gameInfo.BoardId, conn);
                    updatecmd.ExecuteNonQuery();
                }
            }
            conn.Close();
        }

        public void SQLiteDelete(string GameType)
        {
            conn = new SQLiteConnection("Data Source=C:\\Users\\KRKimwoo\\source\\repos\\UI Design\\database.db;Version=3;");
            conn.Open();
            SQLiteCommand deletecmd = new SQLiteCommand("DELETE FROM GameInfo WHERE GameType = " + GameType, conn);
            deletecmd.ExecuteNonQuery();
        }
    }
}
