
using LosPinotsWebServicesSDK;
using UnityEngine;
using SimpleJSON;

public class Login : Http
{
    public string email, password;

    protected override void SetUpForm()
    {
        form.AddField("email", email);
        form.AddField("password", password);
    }

    protected override void FillData(string jsonString)
    {
        var json = JSON.Parse(jsonString);

        var go = new GameObject("UserData"); 
        var userData = go.AddComponent<UserData>();
        userData.SetToken(json["token"]); 
    }
}
