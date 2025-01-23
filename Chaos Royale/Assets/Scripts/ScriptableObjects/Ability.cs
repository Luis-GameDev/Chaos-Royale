using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class Ability : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public float Cooldown;
    public float CooldownLeft = 0;
    [SerializeField] private Sprite Icon;
    [SerializeField] public float CastTime;
    public Camera mainCam;

    public virtual void Use(GameObject caster) {

    }

    public virtual void UpdateCooldown() {

    }
}
