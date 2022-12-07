using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class Testing : MonoBehaviour
{
    public string DevID = "4474";
    public string AuthID = "B4AB8D92C5734DD99C23B3750AC8E3C6";
    [SerializeField]
    private string dateNow;
    public string urlPrefix = "https://api.smitegame.com/smiteapi.svc/";
    string session = "";
    string signature = "";
    string playerName = "";
    string playerID = "";
    public TMP_InputField playerNameInput;
    public Button checkButton;

    // Start is called before the first frame update
    void Start()
    {
        dateNow = DateTime.UtcNow.ToString("yyyyMMddHHmms");

        checkButton.onClick.AddListener(Initialize);
    }

    private void Initialize()
    {
        playerName = playerNameInput.text;

        CreateSession();

        GetListGods();

        //GetPlayer(playerName);

        GetMatchHistory();

        GetPlayerId();

        GetGodRanks();
    }

    private static string GetMD5Hash(string input)
    {
        var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        bytes = md5.ComputeHash(bytes);
        var sb = new System.Text.StringBuilder();
        foreach (byte b in bytes)
        {
            sb.Append(b.ToString("x2").ToLower());
        }
        return sb.ToString();
    }

    private void CreateSession()
    {
        // Get Signature that is specific to "createsession"
        //
        signature = GetMD5Hash(DevID + "createsession" + AuthID + dateNow);

        // Call the "createsession" API method & wait for synchronous response
        //
        WebRequest request = WebRequest.Create(urlPrefix + "createsessionjson/" + DevID + "/" + signature + "/" + dateNow);
        WebResponse response = request.GetResponse();

        Stream dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);

        string responseFromServer = reader.ReadToEnd();

        reader.Close();
        response.Close();

        // Parse returned JSON into "session" data
        //
        using (var web = new WebClient())
        {
            web.Encoding = System.Text.Encoding.UTF8;
            var jsonString = responseFromServer;
            var jss = new JsonSerializer();//new JavaScriptSerializer();
            var g = JsonConvert.DeserializeObject<SessionInfo>(jsonString);

            session = g.session_id;

            Debug.Log("Session " + session);

        }
    }

    private void GetListGods()
    {
        // Get Signature that is specific to "getgods"
        //
        signature = GetMD5Hash(DevID + "getgods" + AuthID + dateNow);

        // Call the "getgods" API method & wait for synchronous response
        //
        string languageCode = "1";

        WebRequest request = WebRequest.Create(urlPrefix + "getgodsjson/" + DevID + "/" + signature + "/" + session + "/" + dateNow + "/" + languageCode);
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
            var GodsList = JsonConvert.DeserializeObject<List<Gods>>(jsonString);
            string GodsListStr = "";

            foreach (Gods x in GodsList)
                GodsListStr = GodsListStr + ", " + x.Name;

            Debug.Log("Here are the Gods: " + GodsListStr);
        }
    }

    //private void GetPlayer(string playerName)
    //{
    //    // Get Signature that is specific to "getgods"
    //    //5:Steam
    //    signature = GetMD5Hash(DevID + "getplayer" + AuthID + dateNow);
    //
    //    // Call the "getgods" API method & wait for synchronous response
    //    //
    //
    //    WebRequest request = WebRequest.Create(urlPrefix + "getplayerjson/" + DevID + "/" + signature + "/" + session + "/" + dateNow + "/" + playerName);
    //    WebResponse response = request.GetResponse();
    //
    //    Stream dataStream = response.GetResponseStream();
    //    StreamReader reader = new StreamReader(dataStream);
    //
    //    string responseFromServer = reader.ReadToEnd();
    //
    //    reader.Close();
    //    response.Close();
    //
    //    // Parse returned JSON into "gods" data
    //    //
    //    using (var web = new WebClient())
    //    {
    //        web.Encoding = System.Text.Encoding.UTF8;
    //        var jsonString = responseFromServer;
    //        var jss = new JsonSerializer();
    //        var player = JsonConvert.DeserializeObject<List<Player>>(jsonString);
    //
    //        Debug.Log("Player: " + player);
    //    }
    //}

    private void GetMatchHistory()
    {
        // Get Signature that is specific to "getgods"
        //5:Steam
        signature = GetMD5Hash(DevID + "getmatchhistory" + AuthID + dateNow);

        // Call the "getgods" API method & wait for synchronous response
        //

        WebRequest request = WebRequest.Create(urlPrefix + "getmatchhistoryjson/" + DevID + "/" + signature + "/" + session + "/" + dateNow + "/" + "11303650");//last playerid
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
            var matchHistory = JsonConvert.DeserializeObject<List<MatchHistory>>(jsonString);

            Debug.Log("MatchHistory: " + matchHistory);
        }
    }

    private void GetPlayerId()
    {
        if (!string.IsNullOrEmpty(playerName))
        {
            // Get Signature that is specific to "getgods"
            //5:Steam
            signature = GetMD5Hash(DevID + "getplayeridbyname" + AuthID + dateNow);

            // Call the "getgods" API method & wait for synchronous response
            //

            WebRequest request = WebRequest.Create(urlPrefix + "getplayeridbynamejson/" + DevID + "/" + signature + "/" + session + "/" + dateNow + "/" + playerName);
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
                var player = JsonConvert.DeserializeObject<List<PlayerID>>(jsonString);

                Debug.Log("PlayerID: " + player);
                playerID = player[0].player_id;
            }
        }
    }

    private void GetGodRanks()
    {
        if (!string.IsNullOrEmpty(playerName))
        {
            // Get Signature that is specific to "getgods"
            signature = GetMD5Hash(DevID + "getgodranks" + AuthID + dateNow);

            // Call the "getgods" API method & wait for synchronous response
            //

            WebRequest request = WebRequest.Create(urlPrefix + "getgodranksjson/" + DevID + "/" + signature + "/" + session + "/" + dateNow + "/" + playerID);
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
                var godRanks = JsonConvert.DeserializeObject<List<GodRanks>>(jsonString);

                Debug.Log("God Ranks: " + godRanks);
            }
        }
    }

    public class GodRanks
    {
        public string Assists { get; set; }
        public string Deaths { get; set; }
        public string Kills { get; set; }
        public string Losses { get; set; }
        public string MinionKills { get; set; }
        public string Rank { get; set; }
        public string Wins { get; set; }
        public string Worshippers { get; set; }
        public string god { get; set; }
        public string god_id { get; set; }
        public string player_id { get; set; }
        public string ret_msg { get; set; }

    }

    public class PlayerID
    {
        public string player_id { get; set; }
        public string portal { get; set; }
        public string portal_id { get; set; }
        public string privacy_flag { get; set; }
        public string ret_msg { get; set; }

    }

    public class MatchHistory
    {
        public string ActiveId1 { get; set; }
        public string ActiveId2 { get; set; }
        public string Active_1 { get; set; }
        public string Active_2 { get; set; }
        public string Active_3 { get; set; }
        public string Assists { get; set; }
        public string Ban1 { get; set; }
        public string Ban10 { get; set; }
        public string Ban10Id { get; set; }
        public string Ban11 { get; set; }
        public string Ban11Id { get; set; }
        public string Ban12 { get; set; }
        public string Ban12Id { get; set; }
        public string Ban1Id { get; set; }
        public string Ban2 { get; set; }
        public string Ban2Id { get; set; }
        public string Ban3 { get; set; }
        public string Ban3Id { get; set; }
        public string Ban4 { get; set; }
        public string Ban4Id { get; set; }
        public string Ban5 { get; set; }
        public string Ban5Id { get; set; }
        public string Ban6 { get; set; }
        public string Ban6Id { get; set; }
        public string Ban7 { get; set; }
        public string Ban7Id { get; set; }
        public string Ban8 { get; set; }
        public string Ban8Id { get; set; }
        public string Ban9 { get; set; }
        public string Ban9Id { get; set; }
        public string Creeps { get; set; }
        public string Damage { get; set; }
        public string Damage_Bot { get; set; }
        public string Damage_Done_In_Hand { get; set; }
        public string Damage_Mitigated { get; set; }
        public string Damage_Structure { get; set; }
        public string Damage_Taken { get; set; }
        public string Damage_Taken_Magical { get; set; }
        public string Damage_Taken_Physical { get; set; }
        public string Deaths { get; set; }
        public string Distance_Traveled { get; set; }
        public string First_Ban_Side { get; set; }
        public string God { get; set; }
        public string GodId { get; set; }
        public string Gold { get; set; }
        public string Healing { get; set; }
        public string Healing_Bot { get; set; }
        public string Healing_Player_Self { get; set; }
        public string ItemId1 { get; set; }
        public string ItemId2 { get; set; }
        public string ItemId3 { get; set; }
        public string ItemId4 { get; set; }
        public string ItemId5 { get; set; }
        public string ItemId6 { get; set; }
        public string Item_1 { get; set; }
        public string Item_2 { get; set; }
        public string Item_3 { get; set; }
        public string Item_4 { get; set; }
        public string Item_5 { get; set; }
        public string Item_6 { get; set; }
        public string Killing_Spree { get; set; }
        public string Kills { get; set; }
        public string Level { get; set; }
        public string Map_Game { get; set; }
        public string Match { get; set; }
        public string Match_Queue_Id { get; set; }
        public string Match_Time { get; set; }
        public string Minutes { get; set; }
        public string Multi_kill_Max { get; set; }
        public string Objective_Assists { get; set; }
        public string Queue { get; set; }
        public string Region { get; set; }
        public string Role { get; set; }
        public string Skin { get; set; }
        public string SkinId { get; set; }
        public string Surrendered { get; set; }
        public string TaskForce { get; set; }
        public string Team1Score { get; set; }
        public string Team2Score { get; set; }
        public string Time_In_Match_Seconds { get; set; }
        public string Wards_Placed { get; set; }
        public string Win_Status { get; set; }
        public string Winning_TaskForce { get; set; }
        public string playerId { get; set; }
        public string playerName { get; set; }
        public string ret_msg { get; set; }

    }
    public class SessionInfo
    {
        public string ret_msg { get; set; }
        public string session_id { get; set; }
        public string timestamp { get; set; }
    }

    public class Menuitem
    {
        public string description { get; set; }
        public string value { get; set; }
    }

    public class Rankitem
    {
        public string description { get; set; }
        public string value { get; set; }
    }

    public class AbilityDescription
    {
        public string description { get; set; }
        public string secondaryDescription { get; set; }
        public List<Menuitem> menuitems { get; set; }
        public List<Rankitem> rankitems { get; set; }
        public string cooldown { get; set; }
        public string cost { get; set; }
    }

    public class AbilityRoot
    {
        public AbilityDescription itemDescription { get; set; }
    }

    public class Gods
    {
        public int abilityId1 { get; set; }
        public int abilityId2 { get; set; }
        public int abilityId3 { get; set; }
        public int abilityId4 { get; set; }
        public int abilityId5 { get; set; }
        public AbilityRoot abilityDescription1 { get; set; }
        public AbilityRoot abilityDescription2 { get; set; }
        public AbilityRoot abilityDescription3 { get; set; }
        public AbilityRoot abilityDescription4 { get; set; }
        public AbilityRoot abilityDescription5 { get; set; }
        public int id { get; set; }
        public string Pros { get; set; }
        public string Type { get; set; }
        public string Roles { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string OnFreeRotation { get; set; }
        public string Lore { get; set; }
        public int Health { get; set; }
        public Double HealthPerLevel { get; set; }
        public Double Speed { get; set; }
        public Double HealthPerFive { get; set; }
        public Double HP5PerLevel { get; set; }
        public Double Mana { get; set; }
        public Double ManaPerLevel { get; set; }
        public Double ManaPerFive { get; set; }
        public Double MP5PerLevel { get; set; }
        public Double PhysicalProtection { get; set; }
        public Double PhysicalProtectionPerLevel { get; set; }
        public Double MagicProtection { get; set; }
        public Double MagicProtectionPerLevel { get; set; }
        public Double PhysicalPower { get; set; }
        public Double PhysicalPowerPerLevel { get; set; }
        public Double AttackSpeed { get; set; }
        public Double AttackSpeedPerLevel { get; set; }
        public string Pantheon { get; set; }
        public string Ability1 { get; set; }
        public string Ability2 { get; set; }
        public string Ability3 { get; set; }
        public string Ability4 { get; set; }
        public string Ability5 { get; set; }
        public string Item1 { get; set; }
        public string Item2 { get; set; }
        public string Item3 { get; set; }
        public string Item4 { get; set; }
        public string Item5 { get; set; }
        public string Item6 { get; set; }
        public string Item7 { get; set; }
        public string Item8 { get; set; }
        public string Item9 { get; set; }
        public int ItemId1 { get; set; }
        public int ItemId2 { get; set; }
        public int ItemId3 { get; set; }
        public int ItemId4 { get; set; }
        public int ItemId5 { get; set; }
        public int ItemId6 { get; set; }
        public int ItemId7 { get; set; }
        public int ItemId8 { get; set; }
        public int ItemId9 { get; set; }
        public string ret_msg { get; set; }
    }



}
