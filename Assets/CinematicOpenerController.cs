using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CinematicOpenerController : MonoBehaviour
{
    public static CinematicOpenerController instance;
    private void Awake()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
            Debug.LogError("Duplicate CinematicOpenerController detected!");
        }
        instance = this;
    }

    [SerializeField]
    public Canvas cinematicCanvas;
    [SerializeField]
    public TextMeshProUGUI textContainer;
    [SerializeField]
    public Image spriteContainer;
    [SerializeField]
    public List<CinematicScene> scenes = new List<CinematicScene>();

    public UnityEvent OnCinematicOver;

    void Start() {
        cinematicCanvas.enabled  = false;
    }

    public void StartCinematic() {
        cinematicCanvas.enabled = true;
        StartCoroutine(CinematicCoroutine());
    }

        public void SkipScene() {
        if (cinematicCanvas.enabled) {
            // Cancel the next scene and start it immediately
            StopAllCoroutines();
            StartCoroutine(CinematicCoroutine());
        }
    }

    public IEnumerator CinematicCoroutine() {
        if (scenes.Count == 0) {
            textContainer.text = null;
            spriteContainer.sprite = null;
            cinematicCanvas.enabled = false;
            OnCinematicOver?.Invoke();
            yield break;
        }

        var scene = scenes.First();
        scenes.RemoveAt(0);
        
        textContainer.text = scene.Text;
        spriteContainer.sprite = scene.Sprite;

        yield return new WaitForSeconds(scene.Duration);
        StartCoroutine(CinematicCoroutine());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
