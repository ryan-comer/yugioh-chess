using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEyesWhiteDragon : MonoBehaviour
{

    public GameObject EnergyBall_p; // Particle system for ball
    public GameObject EnergyWave_p; // Particle system for wave
    public Transform AttackOriginPoint;   // Point where attack originates from

    // Create the energy ball in the dragon's mouth
    public void StartBlueEyesAttack()
    {
        GameObject energyBall = Instantiate(EnergyBall_p, AttackOriginPoint);
        energyBall.transform.localPosition = Vector3.zero;

        Destroy(energyBall, 4);
    }

    // Create the enery wave
    public void FireBlueEyesAttack()
    {
        GameObject energyWave = Instantiate(EnergyWave_p, AttackOriginPoint);
        energyWave.transform.localPosition = Vector3.zero;
        energyWave.transform.LookAt(GetComponent<ChessPiece>().currentTarget.GetComponentInChildren<Renderer>().bounds.center);

        Destroy(energyWave, 2f);
    }

}
