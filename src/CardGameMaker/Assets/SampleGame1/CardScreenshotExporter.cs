using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CardScreenshotExporter : MonoBehaviour
{
    [SerializeField] private string baseExportPathDir;
    [SerializeField] private string cardDbPath;
    [SerializeField] private CardPresenter[] cardPresenters;
    [SerializeField] private Color transparentColor = Color.black;

    private Dictionary<string, CardPresenter> cardPresentersByLayout;
    
    private void Start() => this.SafeCoroutineOrNothing(Go());

    private IEnumerator Go()
    {
        cardPresentersByLayout = cardPresenters.ToDictionary(x => x.layout, x => x);
        HidePresenters();
        yield return new WaitForEndOfFrame();

        var cards = CsvImporter.LoadCsvCardTypes(cardDbPath);
        Log.Info("Loaded " + cards.Count() + " cards");

        var allValid = ValidateCardData(cards);
        if (!allValid)
        {
            Log.Error("Aborting - Card Data Invalid");
            yield break;
        }
        Log.Info("Validated " + cards.Count() + " cards");

        var i = 0;
        foreach (var c in cards)
        {
            for (var n = 0; n < (c.NumCopies > 0 ? c.NumCopies : 1); n++)
            {
                HidePresenters();
                cardPresentersByLayout[c.Layout].Activate(c);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                ExportCard(i, c.Name, false);
                
                HidePresenters();
                var bc = cards.First(x => x.Name == c.Back);
                cardPresentersByLayout[bc.Layout].Activate(bc);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                ExportCard(i, c.Name, true);
                
                i++;
            }
        }

        Log.Info("Finished Export");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private bool ValidateCardData(CsvCardType[] cards)
    {
        var succeeded = true;
        var cardNames = new HashSet<string>();
        foreach (var c in cards)
        {
            cardNames.Add(c.Name);
        }

        foreach (var c in cards)
        {
            if (!cardNames.Contains(c.Back))
            {
                Log.Error($"Missing Card Back: '{c.Back}'");
                succeeded = false;
            }

            if (!cardPresentersByLayout.ContainsKey(c.Layout))
            {
                Log.Error($"Missing Card Layout: '{c.Layout}'");
                succeeded = false;
            }

            if (c.Art == null)
            {
                Log.Error($"Missing Card Art: '{c.Name}'");
                succeeded = false;
            }
        }

        return succeeded;
    }

    private void HidePresenters()
    {
        foreach (var cardPresenter in cardPresenters)
            cardPresenter.gameObject.SetActive(false); 
    }
    
    private void ShowCard(ICardType c)
    {
        
    }
    
    private void ShowCardBack(ICardType c)
    {
        
    }
    
    private void ExportCard(int idx, string cardName, bool isBack)
        => ExportCard((isBack ? "b" : "f") + "_" + (idx + 1).ToString().PadLeft(3, '0') + "_" + cardName.Replace(" ", "").ToLowerInvariant());
    
    private void ExportCard(string fileName)
    {
        var savePath = Path.Combine(baseExportPathDir, fileName + ".png");
        var tex = ScreenCapture.CaptureScreenshotAsTexture();

        var newTexture = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        var pixels = tex.GetPixels();
        for (var i = 0; i < pixels.Length; i++)
        {
            var p = pixels[i];
            if (p.r == transparentColor.r && p.g == transparentColor.g && p.b == transparentColor.b)
                pixels[i] = Color.clear;
        }

        newTexture.SetPixels(pixels);
        var pngShot = ImageConversion.EncodeToPNG(newTexture);
        File.WriteAllBytes(savePath, pngShot);
    }
}
