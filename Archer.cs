using UnityEngine;

public class Archer : Unit
{

    private void Start()
    {
        this.attackDistanceMin = 5.0f;
        this.attackDistanceMax = 20.0f;
        this.attackAnimationDuration = 1.2f;
        this.canAttack = true;

        Awake();
        Update();
    }
}
