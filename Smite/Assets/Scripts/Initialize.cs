using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Initialize : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public Button checkButton;
    private bool init = false;
    // Start is called before the first frame update
    void Start()
    {
        checkButton.onClick.AddListener(InitializePlayer);
    }

    private void InitializePlayer()
    {
        Globals.playerName = playerNameInput.text;
        Globals.dateNow = DateTime.UtcNow.ToString("yyyyMMddHHmms");
        CreateSession();

        if(init == true)
        {
            SceneManager.LoadScene("PlayerProfile");
        }
    }

    private void CreateSession()
    {
        // Get Signature that is specific to "createsession"
        //
        var signature = Globals.Instance.GetMD5Hash(Globals.DevID + "createsession" + Globals.AuthID + Globals.dateNow);

        // Call the "createsession" API method & wait for synchronous response
        //
        WebRequest request = WebRequest.Create("https://api.smitegame.com/smiteapi.svc/" + "createsessionjson/" + Globals.DevID + "/" + signature + "/" + Globals.dateNow);
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
            var g = JsonConvert.DeserializeObject<SessionInfo>(jsonString);

            Globals.session = g.session_id;

            init = true;
            Debug.Log("Session " + Globals.session);
        }
    }
    
    public class SessionInfo
    {
        public string ret_msg { get; set; }
        public string session_id { get; set; }
        public string timestamp { get; set; }
    }
}
