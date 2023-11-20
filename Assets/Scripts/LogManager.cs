using System;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class LogManager
{
    static string HTTP_SERVER = "https://20ga4y6nah.execute-api.ap-northeast-2.amazonaws.com/Test/DatePlusMetricDBManager";
    Guid currentID;
    HttpClient client;
    public LogManager()
    {
        client = new HttpClient();
    }

    public void GameStart(float standardTime)
    {
        currentID = Guid.NewGuid();
        Log("gamestart", "standardTime", standardTime);
    }

    public void MiniGameStart(string name, float money, float positive, float time)
    {
        Log("minigame_start", "name", name, "money", money, "positive", positive, "time", time);
    }

    public void MiniGameRefused(string name)
    {
        Log("minigame_refused", "name", name);
    }

    public void MiniGameFinished(string name, float money, float positive, float time)
    {
        Log("minigame_finished", "name", name, "money", money, "positive", positive, "time", time);
    }

    public void CoinskillEnter(bool enter, float money, float positive, float time)
    {
        Log("coinskill_enter", "enter", enter, "money", money, "positive", positive, "time", time);
    }
    public void CoinskillEnd(float money, float positive, float time)
    {
        Log("coinskill_end", "money", money, "positive", positive, "time", time);
    }

    public void ShoppingEnter(float money, float positive, float time)
    {
        Log("shopping_enter", "money", money, "positive", positive, "time", time);
    }

    public void ShoppingEnd(string gift, float money, float positive, float time)
    {
        Log("shopping_end", "gift", gift, "money", money, "positive", positive, "time", time);
    }

    public void GameEnd(EEnding result, EGift gift, int finishedGames)
    {
        Log("game_end", "ending", result, "gift", gift, "finished", finishedGames);
    }

    public async void Log(string method, params dynamic[] data)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.FloatParseHandling = FloatParseHandling.Decimal;
        settings.Formatting = Formatting.None;

        var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        dynamic obj = new System.Dynamic.ExpandoObject();


        for (int i = 0; i < data.Length; i += 2)
        {
            ((IDictionary<String, object>)obj)[data[i]] = data[i+1];
        }

        obj._id = currentID.ToString();
        obj._timestamp = timestamp;
        obj._method = method;

        dynamic reqStr = new System.Dynamic.ExpandoObject();
        reqStr.payload = obj;

        var content = new StringContent(JsonConvert.SerializeObject(reqStr, settings), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(HTTP_SERVER, content);

        Debug.Log(response);

        string path = Application.persistentDataPath + "/telemetrylog";
        Debug.Log(path);

        await File.AppendAllTextAsync(path, JsonConvert.SerializeObject(obj, settings) + "\n");
    }
}
