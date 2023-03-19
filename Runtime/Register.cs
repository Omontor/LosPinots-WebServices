using LosPinotsWebServicesSDK;

public class Register : Http
{
    public string userName, email, password;

    protected override void SetUpForm()
    {
        form.AddField("name", userName);
        form.AddField("email", email);
        form.AddField("password", password);
    }

    protected override void FillData(string jsonString)
    {
        //NO DATA
    }
}
