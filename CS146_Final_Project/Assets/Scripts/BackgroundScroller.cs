using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float z_offset = 8;

    private Transform player;
    private Renderer r;

    private float posX = 0.0f;
    private float posY = 0.0f;
    private float prevX;
    private float prevY;

    /* Init vars */
    void Start()
    {
        r = GetComponent<Renderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        prevX = player.position.x;
        prevY = player.position.y;
    }

    /* Update texture coords to create scroll effect. */
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, z_offset);

        // Find coordinate shift
        posX += (player.position.x - prevX) * Time.deltaTime * speed;
        if (posX > 1.0f) posX -= 1.0f;
        else if (posX < -1.0f) posX += 1.0f;

        posY += (player.position.y - prevY) * Time.deltaTime * speed;
        if (posY > 1.0f) posY -= 1.0f;
        else if (posY < -1.0f) posY += 1.0f;

        prevX = player.position.x;
        prevY = player.position.y;

        // Update texture coords
        r.material.mainTextureOffset = new Vector2(posX, posY);
    }
}
