using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundScaler : MonoBehaviour
{
    private void Start()
    {
        Resize();
    }

    void Resize()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Primeiro remove escala para medir corretamente
        transform.localScale = Vector3.one;

        // Tamanho da sprite no mundo
        float width = sr.bounds.size.x;
        float height = sr.bounds.size.y;

        // Tamanho da câmera no mundo
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * Camera.main.aspect;

        // Escala independente — aqui ocorre a DEFORMAÇÃO
        float scaleX = worldScreenWidth / width;
        float scaleY = worldScreenHeight / height;

        // Aplica a deformação sem manter proporção
        transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}
