using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public KitchenObjectSO GetKitchenObjectSO { get { return kitchenObjectSO;}}
    public IKitchenObjectParent SetKitchenObjectParent {
        get { return kitchenObject; } 
        set 
        {
            kitchenObject?.ClearKitchenObject();
            kitchenObject = value;
            kitchenObject.KitchenObject = this;
            transform.parent = kitchenObject.GetKitchenObjectFollowTransform();
            transform.localPosition = Vector3.zero;
        }
    }
    private IKitchenObjectParent kitchenObject;

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent objectParent)
    {
        GameObject spawnedObject = Instantiate(kitchenObjectSO.Prefab);
        KitchenObject kitchenObject = spawnedObject.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent = objectParent;
        return kitchenObject;
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public void DestorySelf()
    {
        kitchenObject.ClearKitchenObject();
        Destroy(gameObject);
    }
}