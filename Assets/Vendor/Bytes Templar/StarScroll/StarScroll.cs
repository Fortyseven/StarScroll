using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// ReSharper disable ConvertToConstant.Global

public class StarScroll : MonoBehaviour
{
    private const float TEXT_ROT_ANGLE = 66.0f;
    private static readonly Color CRAWL_COLOR = new Color( 0.898f, 0.694f, 0.227f );

    public Font FontBody;
    public float CrawlSpeed = 3.0f;
    public float ScrollStartY = -350.0f;
    public float ScrollEndY = 1000.0f;
    public bool PlayOnStart = true;
    [TextArea( 5, 99 )]
    public string Text;
    public int FontSize = 24;

    private Canvas _canvas;
    private GameObject _text_container;
    private Text _text;

    public void Start()
    {
        // Validate editor settings
        if ( FontBody == null )
            throw new UnityException( "No text body font specified for StarScroll" );

        // Create our UI canvas
        _canvas = this.gameObject.AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.WorldSpace;
        _canvas.transform.Rotate( TEXT_ROT_ANGLE, 0, 0 );

        RectTransform rect = _canvas.GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, 200 );
        rect.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, 200 );

        _text_container = new GameObject( "CrawlTextContainer" );
        _text_container.transform.SetParent( this.transform );
        _text_container.transform.localRotation = Quaternion.identity;

        _text = _text_container.AddComponent<Text>();
        _text.font = FontBody;
        _text.fontSize = FontSize;
        _text.text = Text;
        _text.color = CRAWL_COLOR;
        _text.supportRichText = true;
        _text.horizontalOverflow = HorizontalWrapMode.Wrap;
        _text.verticalOverflow = VerticalWrapMode.Overflow;
        _text.alignment = TextAnchor.UpperCenter;

        float width = Mathf.Abs( Screen.width / Screen.height ) * 400;
        _text.rectTransform.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, width );

        Reset();

        if ( PlayOnStart )
            Play();
    }

    private void Reset()
    {
        _text.rectTransform.localPosition = new Vector3( 0, ScrollStartY, 0 );
    }

    private void Play()
    {
        StartCoroutine( "RunCrawl" );
    }

    public IEnumerator RunCrawl()
    {
        Reset();
        RectTransform rect = _text.rectTransform;
        Vector3 pos = rect.localPosition;

        for ( float y = ScrollStartY; y < ScrollEndY; y += CrawlSpeed * Time.deltaTime ) {
            pos.y = y;
            rect.localPosition = pos;
            yield return null;
        }

        // TODO: Call on finish here
        yield return null;
    }
}
