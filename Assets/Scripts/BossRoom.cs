using UnityEngine;

public class BossRoom : MonoBehaviour
{

    [SerializeField] Transform topWall;
    [SerializeField] Transform bottomWall;
    [SerializeField] Transform leftWall;
    [SerializeField] Transform rightWall;

    [SerializeField] float extraMargin = 4f;
    [SerializeField] float wallThickness = 1f;


    void Awake()
    {
        Camera cam = Camera.main;

        float halfHeight = cam.orthographicSize + extraMargin;
        float halfWidth = halfHeight * cam.aspect;

        topWall.position = new Vector3(0f, halfHeight, 0f);
        topWall.localScale = new Vector3((halfWidth + wallThickness) * 2f, wallThickness, 1f);

        bottomWall.position = new Vector3(0f, -halfHeight, 0f);
        bottomWall.localScale = new Vector3((halfWidth + wallThickness) * 2f, wallThickness, 1f);

        leftWall.position = new Vector3(-halfWidth,0f,0f);
        leftWall.localScale = new Vector3(wallThickness, (halfHeight + wallThickness) * 2f, 1f);

        rightWall.position = new Vector3(halfWidth, 0f, 0f);
        rightWall.localScale = new Vector3(wallThickness, (halfHeight + wallThickness)*2f,1f);

    }



}
