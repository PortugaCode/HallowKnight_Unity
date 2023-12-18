using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public bool boss = false;
    [SerializeField] new private Camera camera;

    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 verocity = Vector3.zero;

    [SerializeField] private float leftLimit;
    [SerializeField] private float rightLimit;
    [SerializeField] private float bottomLimit;
    [SerializeField] private float topLimit;

    private void Awake()
    {
        AudioManager.Instance.PlayMusic("Game Theme");
    }

    private void Update()
    {
        if (!boss)
        {
            Vector3 TargetPosition = player.transform.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref verocity, smoothTime);
            camera.fieldOfView = 60;
            transform.position = new Vector3
            (
                 Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                 Mathf.Clamp(transform.position.y, bottomLimit, topLimit),
                 transform.position.z
            );
        }
        else if(boss)
        {
            Vector3 TargetPosition = new Vector3(174.2f, 5.3f, 0f) + offset;
            transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref verocity, smoothTime);

            //transform.position = new Vector3(174.2f, 5.3f, -10f);
            StartCoroutine(ofview());
        }
    }

    private IEnumerator ofview()
    {
        yield return new WaitForSeconds(0.1f);

        while (camera.fieldOfView < 91)
        {
            camera.fieldOfView += 1f;

            yield return new WaitForSeconds(0.2f);
        }
    }
}
