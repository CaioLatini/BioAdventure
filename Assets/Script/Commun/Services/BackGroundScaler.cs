using UnityEngine;

// BackgroundScaler.cs
/*
Responsável por redimensionar automaticamente a imagem de fundo para preencher
toda a visão da câmera, independentemente da proporção (Aspect Ratio) da tela.
*/

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundScaler : MonoBehaviour
{
    // Ajusta a escala no início da cena
    private void Start()
    {
        Resize();
    }

    // Calcula os limites da câmera e estica o SpriteRenderer para cobrir todo o espaço
    void Resize()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        transform.localScale = Vector3.one;

        float width = sr.bounds.size.x;
        float height = sr.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * Camera.main.aspect;

        float scaleX = worldScreenWidth / width;
        float scaleY = worldScreenHeight / height;

        transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}