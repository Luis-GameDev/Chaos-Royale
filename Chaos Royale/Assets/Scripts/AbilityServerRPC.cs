using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Connection;
using UnityEngine;

public class AbilityServerRPC : NetworkBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    public void SpawnAetheruptionPrefab(GameObject prefab, Vector3 position, Quaternion rotation, NetworkConnection connection) {
        GameObject proj = Instantiate(prefab, position, rotation);
        Spawn(proj, connection);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnLightningstrike(GameObject prefab, Vector3 position, Quaternion rotation, NetworkConnection connection, Vector3 direction) {
        GameObject proj = Instantiate(prefab, position, rotation);
        proj.GetComponent<ProjectileComponent>().direction = direction;
        Spawn(proj, connection);
    }
}

