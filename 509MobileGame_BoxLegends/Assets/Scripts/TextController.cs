using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public static TextController instance;

    public Text textdialogue;

    private void Awake()
    {
        instance = this;
    }
    public void Changetext(string text,int Size)
    {
        textdialogue.text = text;
        textdialogue.fontSize = Size;
    }
}
