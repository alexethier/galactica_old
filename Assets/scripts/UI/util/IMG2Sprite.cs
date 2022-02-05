    using UnityEngine;
    using System.Collections;
    using System.IO;
    using System;
     
    public class IMG2Sprite {
     
       public static Sprite LoadNewSprite(string filePath, float pixelsPerUnit = 100.0f) {
       
         // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
         
         Texture2D spriteTexture = LoadTexture(filePath);
         Sprite newSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height),new Vector2(0,0), pixelsPerUnit);
     
         return newSprite;
       }
     
       private static Texture2D LoadTexture(string filePath) {
     
         // Load a PNG or JPG file from disk to a Texture2D
         // Returns null if load fails
     
         Texture2D tex2D;
         byte[] fileData;
     
         if (File.Exists(filePath)) {
           fileData = File.ReadAllBytes(filePath);
           tex2D = new Texture2D(2, 2);           // Create new "empty" texture

           if (tex2D.LoadImage(fileData)) {
                return tex2D;                 // If data = readable -> return texture
           }           // Load the imagedata into the texture (size is set automatically)
         }
                     
        throw new Exception("Could not load image at path: " + filePath);

       }
    }
