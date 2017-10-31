using UnityEngine;
using System.Collections;
using System;

// Weapon types
public enum WeaponType
{
    Vulcan,
}

public class WeaponController : MonoBehaviour
{
    // Current firing socket
    public int curSocket = 0;          
    // Timer reference                
    public int timerID = -1;

    [Header("Turret setup")]
    public Transform[] TurretSocket;            // Sockets reference
   
    public WeaponType type;             // Default starting weapon type
    
    [Header("Vulcan")]    
    public Transform vulcanProjectile;          // Projectile prefab
    public Transform vulcanMuzzle;              // Muzzle flash prefab  
    public Transform vulcanImpact;              // Impact prefab

    // Fire turret weapon
    public void Fire()
    {
        switch (type)
        {
            case WeaponType.Vulcan:
                // Fire vulcan at specified rate until canceled
				timerID = GetComponent<WeaponTimer>().AddTimer(0.7f, Vulcan);
                // Invoke manually before the timer ticked to avoid initial delay
                Vulcan();
                break;

            default:
                break;
        }
    }

    // Stop firing 
    public void Stop()
    {
        // Remove firing timer
        if (timerID != -1)
        {
            GetComponent<WeaponTimer>().RemoveTimer(timerID);
            timerID = -1;
        }
    }

    // Fire vulcan weapon
    void Vulcan()
    {
        // Get random rotation that offset spawned projectile
        Quaternion offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);

        // Spawn muzzle flash and projectile with the rotation offset at current socket position
        GetComponent<WeaponPool>().Spawn(vulcanMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		GetComponent<WeaponPool>().Spawn(vulcanProjectile, TurretSocket[curSocket].position + TurretSocket[curSocket].forward, offset * TurretSocket[curSocket].rotation, null);

        // Play shot sound effect
        //F3DAudioController.instance.VulcanShot(TurretSocket[curSocket].position);
    }

    // Spawn vulcan weapon impact
    public void VulcanImpact(Vector3 pos)
    {
        // Spawn impact prefab at specified position
		GetComponent<WeaponPool>().Spawn(vulcanImpact, pos, Quaternion.identity, null);
        // Play impact sound effect
        //F3DAudioController.instance.VulcanHit(pos);
    }
}
