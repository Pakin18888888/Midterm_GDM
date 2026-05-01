// using System.Collections;
// using UnityEngine;

// public class PlayerMagnet : MonoBehaviour
// {
//     public float magnetRadius = 5f;
//     public float duration = 5f;

//     private bool active = false;

//     public void Activate()
//     {
//         if (!active)
//             StartCoroutine(CoMagnet());
//     }

//     IEnumerator CoMagnet()
//     {
//         active = true;

//         float timer = 0f;

//         while (timer < duration)
//         {
//             var coins = Physics2D.OverlapCircleAll(transform.position, magnetRadius);

//             foreach (var c in coins)
//             {
//                 var item = c.GetComponent<ItemController>();
//                 if (item != null)
//                 {
//                     item.ForceCollect(transform);
//                 }
//             }

//             timer += Time.deltaTime;
//             yield return null;
//         }

//         active = false;
//     }
// }