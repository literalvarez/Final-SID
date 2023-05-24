using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public List<GameObject> objectsToChange; // Lista de objetos a los que se cambiará el color
    public List<Color> colorOptions; // Lista de colores disponibles

    public float transitionDuration; // Duración de la transición entre colores

    private Coroutine colorChangeCoroutine; // Referencia al coroutine para detenerlo si es necesario

    private void Start()
    {
        // Iniciar el coroutine para cambiar continuamente los colores
        colorChangeCoroutine = StartCoroutine(ChangeColors());
    }

    private void OnDestroy()
    {
        // Detener el coroutine si el objeto se destruye para evitar errores
        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine);
        }
    }

    private IEnumerator ChangeColors()
    {
        int currentIndex = 0; // Índice del color actual en la lista de colores disponibles

        while (true)
        {
            Color startColor = objectsToChange[0].GetComponent<Renderer>().material.color;
            Color targetColor = colorOptions[currentIndex];

            float elapsedTime = 0f;

            while (elapsedTime < transitionDuration)
            {
                // Calcular el progreso de la transición (0 a 1)
                float t = elapsedTime / transitionDuration;

                // Interpolar suavemente entre el color inicial y el color objetivo
                Color interpolatedColor = Color.Lerp(startColor, targetColor, t);

                // Aplicar el color interpolado a cada objeto en la lista
                foreach (GameObject obj in objectsToChange)
                {
                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = interpolatedColor;
                    }
                }

                // Actualizar el tiempo transcurrido
                elapsedTime += Time.deltaTime;

                // Esperar un frame antes de continuar
                yield return null;
            }

            // Establecer el color objetivo como el próximo color en la lista circularmente
            currentIndex = (currentIndex + 1) % colorOptions.Count;
        }
    }
}
