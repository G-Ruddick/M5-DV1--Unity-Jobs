// using UnityEngine;

// public class EnemyMove : MonoBehaviour {
//     [SerializeField] private float speed = 2.5f;
//     [SerializeField] private float distanceThreshhold = 30f;

//     private Transform playerTransform;

//     private void Awake() {
//         playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
//     }

//     private void Update() {
//         Vector2 direction = (playerTransform.position - this.transform.position).normalized;

//         float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
//         this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
//         float distance = Vector2.Distance(playerTransform.position, transform.position);

//         if (distance <= distanceThreshhold) {
//             this.transform.position += -(Vector3)direction * Time.deltaTime * speed;
//         }
//     }
// }
