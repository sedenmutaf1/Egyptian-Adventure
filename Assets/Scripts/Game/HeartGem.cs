using UnityEngine;

public class HeartGem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.SaveRespawnPoint(transform.position);
            }

            Destroy(gameObject);
        }
    }
    private void Update()
    {
        transform.Rotate(0f, 45f * Time.deltaTime, 0f);
    }
}
