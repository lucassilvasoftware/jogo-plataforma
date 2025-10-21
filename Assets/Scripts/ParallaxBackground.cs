using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
  [Header("Parallax Settings")]
  public Transform cameraTransform; // Referência para a câmera
  public float parallaxMultiplier = 0.5f; // Intensidade do efeito
  private Vector3 lastCameraPosition;
  private float textureUnitSizeX;

  void Start()
  {
    if (cameraTransform == null)
      cameraTransform = Camera.main.transform;

    lastCameraPosition = cameraTransform.position;

    // Detecta largura do sprite em unidades
    Sprite sprite = GetComponent<SpriteRenderer>().sprite;
    Texture2D texture = sprite.texture;
    textureUnitSizeX = (texture.width / sprite.pixelsPerUnit) * transform.localScale.x;
  }

  void LateUpdate()
  {
    // Movimento da câmera
    Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
    transform.position += new Vector3(deltaMovement.x * parallaxMultiplier, deltaMovement.y * parallaxMultiplier, 0);
    lastCameraPosition = cameraTransform.position;

    // Rolagem infinita opcional
    if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
    {
      float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
      transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y, transform.position.z);
    }
  }
}
