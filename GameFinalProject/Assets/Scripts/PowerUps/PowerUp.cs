using System.Collections;
using UnityEngine;
public class PowerUp : MonoBehaviour
{
    public PlayerDecorator decorator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerController = collision.GetComponent<PlayerController>();
            decorator.SetPlayer(playerController);
            decorator.ApplyEffect();
            //move -50 in y
            transform.position = new Vector3(transform.position.x, transform.position.y - 50, transform.position.z);
            // Optionally, schedule removal after some time
            StartCoroutine(RemoveAfterTime(5f));
        }
    }

    private IEnumerator RemoveAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        decorator.RemoveEffect();
        Destroy(gameObject);
    }
}
