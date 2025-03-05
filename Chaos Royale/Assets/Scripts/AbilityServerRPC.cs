using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Connection;
using UnityEngine;

public class AbilityServerRPC : NetworkBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    public void SpawnStaticPrefab(Character character, GameObject prefab, Vector3 position, Quaternion rotation, NetworkConnection connection) {
        GameObject proj = Instantiate(prefab, position, rotation);
        proj.GetComponent<ExplosionComponent>().owner = character;
        Spawn(proj);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnTransformPrefab(Character character, GameObject prefab, Vector3 position, Quaternion rotation, NetworkConnection connection, Vector3 direction) {
        GameObject proj = Instantiate(prefab, position, rotation);
        proj.GetComponent<ProjectileComponent>().direction = direction;
        proj.GetComponent<ProjectileComponent>().owner = character;
        Spawn(proj);
    }
}

