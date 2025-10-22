using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class ProjectileHero : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;

    [Header("Dynamic")]
    public Rigidbody rigid;
    [SerializeField]                                                         // a
    private eWeaponType _type;

    public float x0; // Initial X position to use as the wave centerline
    public float birthTime;
    public float waveFrequency;
    public float waveWidth;


    // This public property masks the private field _type
    public eWeaponType type
    {                                              // c
        get { return (_type); }
        set { SetType(value); }
    }


    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();                                     // d
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (type == eWeaponType.phaser)
        {
            // Apply wave movement logic, identical to what you had in Weapon.Fire()
            float age = Time.time - birthTime;
            float theta = Mathf.PI * 2 * age / waveFrequency;
            float sin = Mathf.Sin(theta);

            // Calculate the new X position
            float x = x0 + waveWidth * sin;

            // Update position (move it forward and apply the new X)
            Vector3 tempPos = transform.position;
            tempPos += vel * Time.deltaTime; // Move forward (up in Y)
            tempPos.x = x; // Apply the wave's lateral position

            transform.position = tempPos;
        }
        else
        {
            // Standard straight movement for other weapons
            transform.position += vel * Time.deltaTime;
        }

        if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offUp))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the _type private field and colors this projectile to match the 
    ///   WeaponDefinition.
    /// </summary>
    /// <param name="eType">The eWeaponType to use.</param>
    public void SetType(eWeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GET_WEAPON_DEFINITION(_type);
        rend.material.color = def.projectileColor;
    }

    /// <summary>
    /// Allows Weapon to easily set the velocity of this ProjectileHero
    /// </summary>
    public Vector3 vel
    {
        get { return rigid.linearVelocity; }
        set { rigid.linearVelocity = value; }
    }

}
