using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData Instance;
    
    [SerializeField]private string token;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetToken(string newToken)
    {
        token = newToken;
        
    }
    public string GetToken()
    {
        return token;
    }
}
