using UnityEngine;

public class PlayerCoin : MonoBehaviour
{
    [Header("Coin Settings")]
    public int totalCoins = 0;
    
    [Header("Particle Effect")]
    public GameObject particleEffectPrefab; // Arrastra aquí el prefab desde el Inspector

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
        
        // Instanciar el efecto de partículas en la posición de la moneda
        if (particleEffectPrefab != null)
        {
            GameObject particle = Instantiate(particleEffectPrefab, coin.transform.position, Quaternion.identity);
            
            // Destruir el efecto después de que termine (opcional, normalmente 2-3 segundos)
            Destroy(particle, 2f);
        }
        
        // Destruir la moneda
        Destroy(coin);
    }

    // Método público para obtener el total de monedas (útil para UI)
    public int GetTotalCoins()
    {
        return totalCoins;
    }
}