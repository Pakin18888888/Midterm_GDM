using UnityEngine;

public static class NetworkHelper
{
    public static bool IsOnline()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
}