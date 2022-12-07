using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    private static Globals _instance;
    public static Globals Instance { get { return _instance; } }

    public static string playerName = "";

    public static string DevID = "4474";
    public static string AuthID = "B4AB8D92C5734DD99C23B3750AC8E3C6";
    public static string urlPrefix = "https://api.smitegame.com/smiteapi.svc/";
    public static string dateNow = "";
    public static string session = "";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public string GetMD5Hash(string input)
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
}
