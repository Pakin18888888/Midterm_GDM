// using TMPro;
// using UnityEngine;
// using Unity.Services.Authentication;

// public class AccountLinker : MonoBehaviour
// {
//     public TMP_InputField emailInput;
//     public TMP_InputField passwordInput;

//     public async void LinkAccount()
//     {
//         if (!AuthenticationService.Instance.SessionTokenExists)
//         {
//             Debug.Log("Already linked");

//             return;
//         }

//         try
//         {
//             await AuthenticationService.Instance
//                 .LinkWithEmailPasswordAsync(
//                     emailInput.text,
//                     passwordInput.text
//                 );

//             Debug.Log("ACCOUNT LINKED");
//         }
//         catch (System.Exception e)
//         {
//             Debug.LogError(e.Message);
//         }
//     }
// }