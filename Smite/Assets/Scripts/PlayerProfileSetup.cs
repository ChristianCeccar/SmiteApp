using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using UnityEngine;

public class PlayerProfileSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetPlayer(Globals.playerName);
    }

    private void GetPlayer(string playerName)
    {
        // Get Signature that is specific to "getplayer"
        //5:Steam
        var signature = Globals.Instance.GetMD5Hash(Globals.DevID + "getplayer" + Globals.AuthID + Globals.dateNow);

        // Call the "getgods" API method & wait for synchronous response
        //

        WebRequest request = WebRequest.Create(Globals.urlPrefix + "getplayerjson/" + Globals.DevID + "/" + signature + "/" + Globals.session + "/" + Globals.dateNow + "/" + playerName);
        WebResponse response = request.GetResponse();

        Stream dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);

        string responseFromServer = reader.ReadToEnd();

        reader.Close();
        response.Close();

        // Parse returned JSON into "gods" data
        //
        using (var web = new WebClient())
        {
            web.Encoding = System.Text.Encoding.UTF8;
            var jsonString = responseFromServer;
            var jss = new JsonSerializer();
            var player = JsonConvert.DeserializeObject<List<Player>>(jsonString);

            Debug.Log("Player: " + player[0]);
        }
    }

    public class Player
    {
        public string ActivePlayerId { get; set; }
        public string Avatar_URL { get; set; }
        public string Created_Datetime { get; set; }
        public string HoursPlayed { get; set; }
        public string Id { get; set; }
        public string Last_Login_Datetime { get; set; }
        public string Leaves { get; set; }
        public string Level { get; set; }
        public string Losses { get; set; }
        public string MasteryLevel { get; set; }
        public string MergedPlayers { get; set; }
        public string MinutesPlayed { get; set; }
        public string Name { get; set; }
        public string Personal_Status_Message { get; set; }
        public string Platform { get; set; }
        public string Points { get; set; }
        public string PrevRank { get; set; }
        public string Rank { get; set; }
        public string Rank_Stat { get; set; }
        public string Rank_Stat_Conquestt { get; set; }
        public string Rank_Stat_Joustt { get; set; }
        public string Rank_Variance { get; set; }
        public string Round { get; set; }
        public string Season { get; set; }
        public string Tier { get; set; }
        public string Trend { get; set; }
        public string Wins { get; set; }
        public string player_id { get; set; }
        public string ret_msg { get; set; }

    }
}
