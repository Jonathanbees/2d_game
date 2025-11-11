using UnityEngine;

public class PlayerCoin : MonoBehaviour
{
    [Header("Coin Settings")]
    public int totalCoins = 0;

    void Start()
    {
        Debug.Log("PlayerCoin script iniciado en: " + gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detectado con: " + other.gameObject.name + " - Tag: " + other.tag);
        
        // Verificar si el objeto tiene la etiqueta "Coin"
        if (other.CompareTag("Coin"))
        {
            CollectCoin(other.gameObject);
        }
    }

    void CollectCoin(GameObject coin)
    {
        // Incrementar el contador de monedas
        totalCoins++;
        Debug.Log($"¡Moneda recogida! Total: {totalCoins}");
        
        // Destruir la moneda
        Destroy(coin);
    }

    // Método público para obtener el total de monedas (útil para UI)
    public int GetTotalCoins()
    {
        return totalCoins;
    }
}