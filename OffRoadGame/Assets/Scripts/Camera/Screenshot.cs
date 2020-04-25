// Screenshot Utility: 
// 
// Create a screenshot by pressing a keyboard shortcut. In Unity a "Screenshots" folder is created in paralletl to the "Assets" folder.
// Screenshots are saved in there.
//
// This is a modified version, original license text:
//
// Written by Chris Bellini and published as public domain to http://untitledgam.es
// You may freely use, license or sell any portion of this code for any legal purpose
// All code is presented as is and without warranty or liability and is used at your own risk
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Screenshot : MonoBehaviour
{
    private string screenshotPath;

    [SerializeField]
    private string inputButton = "Screenshot";

    private bool inputButtonIsMapped;

    [SerializeField]
    private KeyCode key = KeyCode.F12;

    [SerializeField]
    [Range(1, 4)]
    private int superSize = 1;

    public KeyCode Key { get { return this.key; } set { this.key = value; } }

    public int SuperSize { get { return this.superSize; } set { this.superSize = Mathf.Clamp(value, 1, 4); } }

    /// <summary>
    /// Path for the screenshots: Screenshots in parallel to the Assets folder
    /// </summary>
    /// <returns></returns>
    public string GetPath()
    {

        string path = Application.dataPath;
        path = path.Substring(0, path.Length - 7);
        path = Path.Combine(path, "Screenshots");

        return path;
    }

    /// <summary>
    /// Filename is a string formatted as "MyScene - 2018.12.09 - 08.12.28.08.png"
    /// </summary>
    /// <returns></returns>
    private static string GetFilename()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        return string.Format("{0} - {1:yyyy.MM.dd - HH.mm.ss.ff}.png", sceneName, DateTime.Now);
    }

    public void CaptureScreenshot()
    {
        string filepath = Path.Combine(this.screenshotPath, Screenshot.GetFilename());
        ScreenCapture.CaptureScreenshot(filepath, this.SuperSize);

        Debug.Log(string.Format("[<color=blue>Screenshot</color>] Screenshot captured.\n<color=grey>{0}</color>", filepath));
    }


    /// <summary>
    /// Set the screenshot path variable and ensure the path exists.
    /// </summary>
    private void SetupScreenshotPath()
    {
        // ensure the screenshot path exists
        this.screenshotPath = GetPath();

        if (!Directory.Exists(this.screenshotPath))
        {
            Directory.CreateDirectory(this.screenshotPath);
        }

    }

    private void Awake()
    {
        SetupScreenshotPath();

        this.inputButtonIsMapped = this.IsInputButtonMapped();
    }

    /// <summary>
    /// Check if "Screenshot" is mapped to another button.
    /// </summary>
    /// <returns></returns>
    private bool IsInputButtonMapped()
    {
        if (string.IsNullOrEmpty(this.inputButton))
        {
            return false;
        }

        try
        {
            Input.GetButton(this.inputButton);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void Update()
    {
        // if screenshot button is pressed, capture a screenshot
        if (Input.GetKeyDown(this.Key) || (this.inputButtonIsMapped && Input.GetButton(this.inputButton)))
        {
            this.CaptureScreenshot();
        }
    }
}