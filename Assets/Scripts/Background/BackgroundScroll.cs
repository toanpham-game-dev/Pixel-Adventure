using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Scrolls a background material by updating shader offsets over time.
/// Supports horizontal and vertical scrolling.
/// </summary>
public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private Material _mat;

    [SerializeField] private bool _horizontal;
    [SerializeField] private bool _vertical;

    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _verticalSpeed;

    private float _offsetX;
    private float _offsetY;

    private void Update()
    {
        if (!Application.isPlaying) return;

        if (_horizontal)
        {
            _offsetX -= _horizontalSpeed * Time.deltaTime;
            _mat.SetFloat("_OffsetX", _offsetX);
        }

        if (_vertical)
        {
            _offsetY -= _verticalSpeed * Time.deltaTime;
            _mat.SetFloat("_OffsetY", _offsetY);
        }
    }
}