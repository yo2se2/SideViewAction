using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnCompleteAnimation()
    {
        Destroy(this.gameObject);
    }
}
