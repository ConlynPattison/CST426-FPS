using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("Behavior")] [Tooltip("Location to which the player will be sent")]
    public Transform toTransform;

    [Header("Effects")] [Tooltip("Sound played on teleport")]
    public AudioClip teleportSound;
    public AudioSource teleportSource;

    public ParticleSystem teleportParticles;

    private static bool _coolingDown = false;
    private bool _playerStanding = false;
    private GameObject _playerObject;

    /*
     * FixedUpdate used to handle the transform/physics calculations correctly
     * GetKey used for better reliability within FixedUpdate vs Update
     */
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.F) && _playerStanding && !_coolingDown)
        {
            _coolingDown = true;
            StartCoroutine(PerformTeleport());
        }
    }

    // PerformTeleport with added delay to handle GetKeyDown without multiple, unintended teleports
    private IEnumerator PerformTeleport()
    {
        teleportSource.PlayOneShot(teleportSound);
        teleportParticles.Play();

        _playerObject.transform.position = toTransform.position;
        _playerObject.transform.forward = toTransform.forward;

        yield return new WaitForSeconds(0.5f);
        
        _coolingDown = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Player")
            return;
        _playerObject = other.gameObject;
        _playerStanding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name != "Player")
            return;
        _playerStanding = false;
        _playerObject = null;
    }
}
