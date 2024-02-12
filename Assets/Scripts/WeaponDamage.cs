using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class WeaponDamage : MonoBehaviour
{
    public int damage;

    public float fireRate;
    private float nextFire;

    public Camera camera;

    public GameObject hitEffect;

    // Ammo
    public int mag = 5;
    public int ammo = 30;
    public int magAmmo = 10;


    public TextMeshProUGUI magText;
    public TextMeshProUGUI amoText;

    public Animation animation;
    public AnimationClip reloadAnimation;

    void Start()
    {
        magText.text = mag.ToString();
        amoText.text = ammo + " / " + magAmmo;
    }


    // Update is called once per frame
    void Update()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }
        
        if (Input.GetButton("Fire1") && nextFire <= 0 && ammo > 0 && !animation.isPlaying) 
        {
            nextFire = 1 / fireRate;

            ammo--;

            magText.text = mag.ToString();
            amoText.text = ammo + " / " + magAmmo;

            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R) && mag > 0)
        {
            Reload();
        }
    }

    void Reload()
    {

        animation.Play(reloadAnimation.name);

        if (magAmmo > 0)
        {
            mag--;

            ammo = magAmmo;

        }

        magText.text = mag.ToString();
        amoText.text = ammo + " / " + magAmmo;

    }

    void Fire()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward); 

        RaycastHit hit;

        if(Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {

            PhotonNetwork.Instantiate(hitEffect.name, hit.point, Quaternion.identity);
            
            if(hit.transform.gameObject.GetComponent<Health>() != null)
            {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
    }
}
