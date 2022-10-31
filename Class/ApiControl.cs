using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_Design.Class;

namespace UI_Design.Class
{
    internal class ApiControl
    {
        public int getBoardId(string GameType)
        {
            string url, result;
            int boardId = 0;
            url = "https://forum.nexon.com/api/v1/community/" + GameType + "?alias=" + GameType + "&countryCode=kr&_=" + DateTimeOffset.Now.ToUnixTimeSeconds();
            result = networkConnect(url);
            if (result == null)
                return 0;
            JObject obj = JObject.Parse(result);
            return (int)obj["communityId"];
        }
        
        public GameInfo getBoardList(GameInfo gameInfo)
        {
            string url, result;
            url = "https://forum.nexon.com/api/v1/community/" + gameInfo.BoardId + "/stickyThreads?alias=" + gameInfo.GameType + "&pageSize=1&_=" + DateTimeOffset.Now.ToUnixTimeSeconds();
            //TODO : 게시글 정보 가져오기
            result = networkConnect(url);
            if (result == null)
                return gameInfo;
            JObject obj = JObject.Parse(result);
            JToken jToken = obj["stickyThreads"];
            GameInfo ResultGameInfo = new GameInfo();
            foreach (JToken data in jToken)
            {
                if ((int)data["threadId"] > gameInfo.ThreadId)
                {
                    ResultGameInfo.BoardId = gameInfo.BoardId;
                    ResultGameInfo.GameType = gameInfo.GameType;
                    ResultGameInfo.ThreadId = (int)data["threadId"];
                    ResultGameInfo.title = (string)data["title"];
                    ResultGameInfo.readCount = (int)data["readCount"];
                    ResultGameInfo.likeCount = (int)data["likeCount"];
                    ResultGameInfo.createTime = (int)data["createDate"];
                    ResultGameInfo.modifyTime = (int)data["modifyDate"];
                    JObject userinfo = (JObject)data["user"];
                    ResultGameInfo.userName = userinfo["nickname"].ToString();
                    break;
                }
            }
            return ResultGameInfo;
        }
        public string networkConnect(string url)
        {
            string result;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                result = reader.ReadToEnd();
                stream.Close();
                response.Close();
                return result;
            }
            catch (Exception e)
            {
                MessageBox.Show("네트워크 오류가 발생했습니다. \n인터넷 연결 확인 후 다시 시도해주세요.", "네트워크 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
