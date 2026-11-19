using UnityEngine;

public class PlayerInput : MonoBehaviour {
    [SerializeField] private float speed = 10f;

    void Update() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical);

        transform.Translate(direction * speed * Time.deltaTime);
    }
}
