using UnityEngine;

public class Hazard : MonoBehaviour
{
    // Hazard Script
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
                player.Die();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
