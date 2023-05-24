using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Texture[] CharactersImage = new Texture[2];
    public Text Stats;
    public Text[] Bios;
    public string[] Statsstring;
    public string[] Biosstring;
    private int ImageIndex = 0;
    public RawImage ImageOg;
    public void ChangeCountryback()
    {
        if (ImageIndex == 0)
        {
            Debug.Log("Dangfer");
        }
        else if(ImageIndex >=1)
        {
            ImageIndex--;
            ImageOg.texture = CharactersImage[ImageIndex];
            Debug.Log(CharactersImage.Rank);
        }
    }
    public void ChangeCountryForward()
    {
        if (ImageIndex >= CharactersImage.Length)
        {
            Debug.Log("Dangfer");
        }
        else
        {
            ImageIndex++;
            ImageOg.texture = CharactersImage[ImageIndex];
            Debug.Log(CharactersImage.Rank);

        }
    }
}
