using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantBillboarding : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private void LateUpdate()
    {
        BillboardManager.Instance.UpdateSprites(cam);
    }
}
