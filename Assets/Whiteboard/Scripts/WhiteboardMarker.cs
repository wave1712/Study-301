using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//This very heavily derives from this youtube video with modifications made to work with how our objects are layed out
//https://www.youtube.com/watch?v=sHE5ubsP-E8

public class WhiteboardMarker : MonoBehaviour
{
    [SerializeField] private Transform _tip;
    [SerializeField] private Transform _tipPoint;
    [SerializeField] private int _penSize = 5;

    private Renderer _renderer;
    [SerializeField] private Color[] _colours;
    public float _tipHeight;

    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos;
    private bool _touchedLastFrame;
    private Vector2 _lastTouchPos;
    private Quaternion _lastTouchRot;

    void Start()
    {
        _renderer = _tip.GetComponent<Renderer>();
        _colours = Enumerable.Repeat(_renderer.material.color, _penSize * _penSize).ToArray();
        
    }
    void Update()
    {
        Draw();
    }

    private void Draw()
    {

        
        if (Physics.Raycast(_tipPoint.position, transform.up, out _touch, _tipHeight))
        {
            if (_touch.transform.CompareTag("Whiteboard"))
            {

                if (_whiteboard == null)
                {
                    
                    _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                var x = (int)(_touchPos.x * _whiteboard.textureSize.x - (_penSize / 2));
                var y = (int)(_touchPos.y * _whiteboard.textureSize.y - (_penSize / 2));

                if (y < 0 || y > _whiteboard.textureSize.y || x < 0 || x > _whiteboard.textureSize.x) return;

                if (_touchedLastFrame)
                {
                    _whiteboard.tex.SetPixels(x, y, _penSize, _penSize, _colours);

                    for (float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _whiteboard.tex.SetPixels(lerpX, lerpY, _penSize, _penSize, _colours);
                    }

                    transform.rotation = _lastTouchRot;

                    _whiteboard.tex.Apply();
                }
                _lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;

                return;
            }
        }

        _whiteboard = null;
        _touchedLastFrame = false;
    }
}
