using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraZoom2Dv3 : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cineCamera;

    [SerializeField] private float scrollZoomSpeed = 1f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float zoomLerpSpeed = 5f;

    private float _originalZoom; // Used to reset zoom
    private float _baseZoom;      // Controlled by mouse scroll
    private float _finalZoom;     // Final zoom after applying key modifier
    private float _scrollDelta;

    public bool isZoomedIn = false; // External flag (set when key is held)

    
    private PlayerControls _controls;

    void Awake()
    {
        _controls = new PlayerControls();
        _controls.Gameplay.CameraKeyZoom.performed += ctx => SetKeyZoom(ctx);
        _controls.Gameplay.CameraKeyZoom.canceled += ctx => SetKeyZoom(ctx);
        _controls.Gameplay.CameraScrollZoom.performed += ctx => SetScrollZoom(ctx);
    }

    private void SetScrollZoom(InputAction.CallbackContext ctx)
    {
        _scrollDelta = ctx.ReadValue<Vector2>().y;
    }

    private void SetKeyZoom(InputAction.CallbackContext ctx)
    {
        isZoomedIn = ctx.ReadValue<float>() > 0f;
    }

    private void OnEnable()
    {
        _controls.Gameplay.Enable();

        isZoomedIn = false;
        _originalZoom = cineCamera.Lens.OrthographicSize;
        _baseZoom = _originalZoom;
        _finalZoom = _baseZoom;
    }



    private void OnDisable()
    {
        _controls.Gameplay.Disable();
    }

    private void Update()
    {
        HandleMouseZoom();
        HandleKeyZoom();
        ApplyZoom();
    }

    private void HandleMouseZoom()
    {
        if (Mathf.Abs(_scrollDelta) >= 1f) // use full scroll steps only
        {
            float stepSize = (maxZoom - minZoom) / 5f; // 5 total steps

            if (_scrollDelta > 0f)
                _baseZoom -= stepSize;
            else if (_scrollDelta < 0f)
                _baseZoom += stepSize;

            _baseZoom = Mathf.Clamp(_baseZoom, minZoom, maxZoom);
            _scrollDelta = 0f; // consume scroll input after applying
        }
    }

    private void HandleKeyZoom()
    {
        float zoomModifier = isZoomedIn ? 0.9f : 1f;
        _finalZoom = Mathf.Clamp(_baseZoom * zoomModifier, minZoom, maxZoom);
    }

    private void ApplyZoom()
    {
        ref var lens = ref cineCamera.Lens;
        lens.OrthographicSize = Mathf.Lerp(
            lens.OrthographicSize,
            _finalZoom,
            Time.deltaTime * zoomLerpSpeed
        );
    }
}