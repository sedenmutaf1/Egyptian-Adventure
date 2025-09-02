using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 startPos = new Vector3(2f, 14f, 12f);
    public Vector3 startRot = new Vector3(10f, 180f, 0f);

    public Vector3 targetPos = new Vector3(1.5f, 15f, -3f);
    public Vector3 targetRot = new Vector3(10f, 0f, 0f);
    public float transitionTime = 2f;

    private Vector3 offset;
    private bool followingPlayer = false;
    private float elapsedTime = 0f;

    void Start()
    {
        transform.position = startPos;
        transform.rotation = Quaternion.Euler(startRot);
    }

    void Update()
    {
        if (!followingPlayer)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionTime);

            
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRot), Quaternion.Euler(targetRot), t);

            if (t >= 1f)
            {
                offset = transform.position - player.position;
                followingPlayer = true;
            }
        }
        else
        {
           
            transform.position = player.position + offset;
        }
    }
}
