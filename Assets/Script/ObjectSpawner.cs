using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] spawnObjects;   // prefab buah
    public Transform player;            // referensi player
    public Vector3 offset = new Vector3(0, 1f, 0); // posisi relatif (di atas player)

    private GameObject currentObject;   // buah yang ready

    void Start()
    {
        SpawnNewObject();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropCurrentObject();
        }
    }

    void SpawnNewObject()
    {
        if (spawnObjects.Length == 0) return;

        int randIndex = Random.Range(0, spawnObjects.Length);

        // spawn buah tepat di atas player
        currentObject = Instantiate(
            spawnObjects[randIndex],
            player.position + offset,
            Quaternion.identity
        );

        // jadikan child dari player → biar ngikutin player
        currentObject.transform.SetParent(player);

        // matiin gravity biar diam dulu
        Rigidbody2D rb = currentObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
    }

    void DropCurrentObject()
    {
        if (currentObject == null) return;

        // lepas dari parent (biar bisa jatuh sendiri)
        currentObject.transform.SetParent(null);

        // aktifkan gravitasi
        Rigidbody2D rb = currentObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 1;
        }

        currentObject = null;
        SpawnNewObject(); // langsung bikin buah baru lagi di atas player
    }
}
