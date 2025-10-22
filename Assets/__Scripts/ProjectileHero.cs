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
            // Calculate time elapsed since firing
            float age = Time.time - birthTime;

            // Calculate the argument for the Sine function (theta)
            // Multiplying by 2*PI converts cycles/second (frequency) to radians/second
            float theta = age * waveFrequency * Mathf.PI * 2;

            // Calculate the final absolute X position of the projectile
            float x = x0 + waveWidth * Mathf.Sin(theta);

            // Update position:
            Vector3 tempPos = transform.position;

            // 1. Apply vertical movement (Y component)
            tempPos += vel * Time.deltaTime;

            // 2. Apply the calculated wave's lateral position (overwrites X component)
            tempPos.x = x;

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
