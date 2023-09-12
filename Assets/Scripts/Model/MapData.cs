using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{

    private int width, height;

    private TextAsset textAsset;
    public Texture2D textureMap;
    private string resourcePath = "MapData";

    private void Start()
    {
        string levelName = SceneManager.GetActiveScene().name;

        if (textureMap == null)
        {
            textureMap = Resources.Load(resourcePath + "/" + levelName) as Texture2D;
        }

        if (textAsset == null)
        {
            textAsset = Resources.Load(resourcePath + "/" + levelName) as TextAsset;
        }
    }

    public List<string> GetMapFromTextFile(TextAsset tAsset)
    {
        List<string> lines = new List<string>();

        if(tAsset != null)
        {
            string textData = tAsset.text;
            string[] delimiters = { "\r\n", "\n" };

            lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None));

            // This is because we want 0,0 to be on the bottom
            lines.Reverse();
        }

        return lines;
    }

    public List<string> GetMapFromTextFile()
    {
        return GetMapFromTextFile(textAsset);
    }

    public List<string> GetMapFromTexture(Texture2D texture)
    {

        List<string> lines = new List<string>();

        if (texture != null)
        {
            for (int x = 0; x < texture.height; ++x)
            {
                string newLine = "";

                for (int y = 0; y < texture.width; ++y)
                {
                    if (texture.GetPixel(y, x) == Color.black)
                    {
                        newLine += '1';
                    }
                    else if (texture.GetPixel(y, x) == Color.white)
                    {
                        newLine += '0';
                    }
                    else
                    {
                        newLine += ' ';
                    }
                }
                lines.Add(newLine);
            }
        }

        return lines;
    }

    public void SetDimensions(List<string> lines)
    {
        height = lines.Count;
        
        foreach(string line in lines)
        {
            if(line.Length > width)
            {
                width = line.Length;
            }
        }
    }

    public int[,] MakeMap()
    {
        List<string> lines = new List<string>();

        if(textureMap != null)
        {
            lines = GetMapFromTexture(textureMap);
        }
        else
        {

            lines = GetMapFromTextFile();
        }

        SetDimensions(lines);

        int[,] map = new int[width, height];

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if (lines[y].Length > x)
                {
                    map[x, y] = (int)char.GetNumericValue(lines[y][x]);
                }
            }
        }



        return map;
    }
}
