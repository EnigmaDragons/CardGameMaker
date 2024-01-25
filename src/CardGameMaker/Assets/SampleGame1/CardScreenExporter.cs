﻿// using System;
// using System.Collections;
// using System.IO;
// using UnityEngine;
//
// public class CardScreenshotExporter : MonoBehaviour
// {
//     [SerializeField] private string baseExportPathDir;
//     [SerializeField] private Library heroes;
//     [SerializeField] private AllCards cards;
//     [SerializeField] private CardPresenter cardPresenter;
//     [SerializeField] private GameObject cardBackView;
//     [SerializeField] private Vector2 captureSize;
//     [SerializeField] private Deck additionalCards;
//     [SerializeField] private bool takeAll = false;
//     [SerializeField] private bool includeExtraSetOfStarters = true;
//     [SerializeField] private bool includeHeroes = true;
//     [SerializeField] private bool includeAdditional = true;
//     [SerializeField] private bool includeBack = true;
//     [SerializeField] private Color transparentColor = Color.black;
//
//     private void Start() => this.SafeCoroutineOrNothing(Go());
//
//     private IEnumerator Go()
//     {
//         yield return new WaitForEndOfFrame();
//         if (includeBack)
//         {
//             cardPresenter.gameObject.SetActive(false);
//             cardBackView.SetActive(true);
//             yield return new WaitForEndOfFrame();
//             yield return new WaitForEndOfFrame();
//             yield return new WaitForEndOfFrame();
//             ExportCard("CardBack");
//         }
//         
//         cardPresenter.gameObject.SetActive(true);
//         cardBackView.SetActive(false);
//
//         if (includeHeroes)
//         {
//             yield return new WaitForEndOfFrame();
//             foreach (var h in heroes.UnlockedHeroes)
//             {
//                 var member = GetHeroCards(h, out var heroCards);
//
//                 foreach (var c in heroCards)
//                 {
//                     var card = new Card(-1, member, c, h.Tint, h.Bust);
//                     var err = false;
//                     try
//                     {
//                         cardPresenter.Set(card);
//                     }
//                     catch (Exception e)
//                     {
//                         err = true;
//                         Log.Warn($"Unable to Render {card.Name}");
//                         Log.Error(e);
//                     }
//
//                     if (!err)
//                     {
//                         yield return new WaitForEndOfFrame();
//                         yield return new WaitForEndOfFrame();
//                         yield return new WaitForEndOfFrame();
//                         ExportCard(h, c);
//
//                         if (!takeAll)
//                             yield break;
//                     }
//                 }
//             }
//         }
//
//         if (includeExtraSetOfStarters)
//         {
//             yield return new WaitForEndOfFrame();
//             foreach (var h in heroes.UnlockedHeroes)
//             {
//                 var member = GetHeroStarterCards(h, out var heroCards);
//
//                 foreach (var c in heroCards)
//                 {
//                     var card = new Card(-1, member, c, h.Tint, h.Bust);
//                     var err = false;
//                     try
//                     {
//                         cardPresenter.Set(card);
//                     }
//                     catch (Exception e)
//                     {
//                         err = true;
//                         Log.Warn($"Unable to Render {card.Name}");
//                         Log.Error(e);
//                     }
//
//                     if (!err)
//                     {
//                         yield return new WaitForEndOfFrame();
//                         yield return new WaitForEndOfFrame();
//                         yield return new WaitForEndOfFrame();
//                         ExportCard("Starter-" + h.NameTerm().ToEnglish() + "-" + c.Name.Replace(" ", "").Replace("\"", ""));
//
//                         if (!takeAll)
//                             yield break;
//                     }
//                 }
//             }
//         }
//
//         
//         if (includeAdditional)
//         {
//             foreach (var c in additionalCards.CardTypes)
//             {
//                 var err = false;
//                 try
//                 {
//                     cardPresenter.Set(c);
//                 }
//                 catch (Exception e)
//                 {
//                     err = true;
//                     Log.Warn($"Unable to Render {c.Name}");
//                     Log.Error(e);
//                 }
//
//                 if (!err)
//                 {
//                     yield return new WaitForEndOfFrame();
//                     yield return new WaitForEndOfFrame();
//                     yield return new WaitForEndOfFrame();
//                     ExportCard(c.Name);
//                         
//                     if (!takeAll)
//                         yield break;
//                 }
//             }
//         }
//         
//         Log.Info("Finished Export");
// #if UNITY_EDITOR
//         UnityEditor.EditorApplication.isPlaying = false;
// #endif
//     }
//
//     private void ExportCard(BaseHero h, CardTypeData c)
//         => ExportCard(h.NameTerm().ToEnglish() + "-" + c.Name.Replace(" ", "").Replace("\"", ""));
//     
//     private void ExportCard(string fileName)
//     {
//         var savePath = Path.Combine(baseExportPathDir, fileName + ".png");
//         var tex = ScreenCapture.CaptureScreenshotAsTexture();
//
//         var newTexture = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
//         var pixels = tex.GetPixels();
//         for (var i = 0; i < pixels.Length; i++)
//         {
//             var p = pixels[i];
//             if (p.r == transparentColor.r && p.g == transparentColor.g && p.b == transparentColor.b)
//                 pixels[i] = Color.clear;
//         }
//
//         newTexture.SetPixels(pixels);
//         var pngShot = ImageConversion.EncodeToPNG(newTexture);
//         File.WriteAllBytes(savePath, pngShot);
//     }
// }