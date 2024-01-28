using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using Photon.Pun;

public class WeaponDamage : MonoBehaviour
{
    public int damage;

    public float fireRate;
    private float nextFire;

    public Camera camera;

    public GameObject hitEffect;

    // Update is called once per frame
    void Update()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }
        
        if (Input.GetButton("Fire1") && nextFire <= 0)
        {
            nextFire = 1 / fireRate;

            Fire();
        }
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
