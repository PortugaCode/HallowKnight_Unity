using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    [SerializeField] private GameObject GhostPrefab;
    [SerializeField] private float delay = 1f;
    [SerializeField] private float destroyTime = 1f;
    private float delta = 0;

    private PlayerConroll player;
    private SpriteRenderer render;
    [SerializeField] private Color color;
    [SerializeField] private Material mat;

    private void Awake()
    {
        TryGetComponent(out player);
    }

    private void Update()
    {
        if(delta > 0) { delta -= Time.deltaTime; }
        else { delta = delay;  CreatGhost(); }
    }

    private void CreatGhost()
    {
        GameObject ghostobj = Instantiate(GhostPrefab, transform.position, transform.rotation);
        ghostobj.transform.localScale = player.transform.localScale;
        Destroy(ghostobj, destroyTime);

        render = ghostobj.GetComponent<SpriteRenderer>();
        render.sprite = player.render.sprite;
        render.color = color;
        if(render.material != null)
        {
            render.material = mat;
        }
    }
}
