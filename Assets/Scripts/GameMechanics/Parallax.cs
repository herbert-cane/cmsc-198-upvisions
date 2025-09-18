using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public RectTransform segmentA;
        public RectTransform segmentB;
        public float speed;
    }

    public ParallaxLayer[] layers;

    void Update()
    {
        foreach (var l in layers)
        {
            MoveLayer(l.segmentA, l.speed);
            MoveLayer(l.segmentB, l.speed);

            float width = l.segmentA.sizeDelta.x;

            // If a segment goes fully off-screen, move it to the right of the other
            if (l.segmentA.anchoredPosition.x <= -width)
                l.segmentA.anchoredPosition += Vector2.right * width * 2f;

            if (l.segmentB.anchoredPosition.x <= -width)
                l.segmentB.anchoredPosition += Vector2.right * width * 2f;
        }
    }

    void MoveLayer(RectTransform segment, float speed)
    {
        segment.anchoredPosition += Vector2.left * speed * Time.deltaTime;
    }
}
