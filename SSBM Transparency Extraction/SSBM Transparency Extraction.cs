// Name: SSBM Transparency Extraction
// Submenu: Object
// Author: Neil Harbin
// Title: SSBM Transparency Extraction
// Version: 1.2
// Desc: Using a black or white background image from the clipboard, remove the background from the same image with the opposite background color, leaving only the foreground objects with proper transparency intact.
// Keywords: SSBM Transparency
// URL:
// Help:
#region UICode
CheckboxControl AlphaMapOnly = false; // Alpha Map Only
#endregion

//Red0 = FF313180
//Green1 = 7CFC0080
//Cyan2 = 00FFFF80
//Purple3 = 9600FF80

// Aux surface
Surface aux = null;

private Surface clipboardSurface = null;
private bool readClipboard = false;

protected override void OnDispose(bool disposing)
{
    if (disposing)
    {
        // Release any surfaces or effects you've created
        if (aux != null) aux.Dispose();
        aux = null;
        if (clipboardSurface != null) clipboardSurface.Dispose();
        clipboardSurface = null;
    }

    base.OnDispose(disposing);
}

// This single-threaded function is called after the UI changes and before the Render function is called
// The purpose is to prepare anything you'll need in the Render function
void PreRender(Surface dst, Surface src)
{
    if (aux == null)
    {
        aux = new Surface(src.Size);
    }

    if (!readClipboard)
    {
        readClipboard = true;
        clipboardSurface = Services.GetService<IClipboardService>().TryGetSurface();
    }
    // Copy from the Clipboard to the aux surface
    for (int y = 0; y < aux.Size.Height; y++)
    {
        if (IsCancelRequested) return;
        for (int x = 0; x < aux.Size.Width; x++)
        {
            if (clipboardSurface != null)
            {
                //aux[x,y] = clipboardSurface.GetBilinearSample(x, y);
                //aux[x,y] = clipboardSurface.GetBilinearSampleClamped(x, y);
                aux[x,y] = clipboardSurface.GetBilinearSampleWrapped(x, y);
            }
            else
            {
                aux[x,y] = Color.Transparent;
            }
        }
    }
}    

    private byte AverageColor(ColorBgra pixel) => (byte) ((pixel.B + pixel.G + pixel.R) / 3);

    private void Render(Surface dst, Surface src, Rectangle rect)
    {
        // Step through each row of the current rectangle
        for (int y = rect.Top; y < rect.Bottom; y++)
        {
            if (IsCancelRequested) return;
            // Step through each pixel on the current row of the rectangle
            for (int x = rect.Left; x < rect.Right; x++)
            {
                ColorBgra SrcPixel = src[x,y]; //Thing we are pasting onto
                ColorBgra AuxPixel = aux[x,y]; //Clipboard

                ColorBgra CurrentPixel = AuxPixel;
                byte redDif = (byte)Math.Abs(SrcPixel.R - AuxPixel.R);
                byte greenDif = (byte)Math.Abs(SrcPixel.G - AuxPixel.G);
                byte blueDif = (byte)Math.Abs(SrcPixel.B - AuxPixel.B);
                byte rAlpha = (byte) (byte.MaxValue - redDif);
                byte gAlpha = (byte) (byte.MaxValue - greenDif);
                byte bAlpha = (byte) (byte.MaxValue - blueDif);
                byte alpha = (byte) (byte.MaxValue - AverageColor(Color.FromArgb(redDif,greenDif,blueDif)));
                if (AlphaMapOnly){
                    dst[x,y] = ColorBgra.FromBgra(bAlpha,gAlpha,rAlpha,255);
                }else{                
                    //alpha will be 255 if there it is purely the background
                    if (alpha < byte.MaxValue){
                        float rpercentage = rAlpha / 255f;
                        float gpercentage = gAlpha / 255f;
                        float bpercentage = bAlpha / 255f;
                        float percentage = alpha / 255f;
                        ColorBgra blackBackPixel = AverageColor(SrcPixel) >= AverageColor(AuxPixel) ? AuxPixel : SrcPixel;
                        if (percentage == 0){
                            dst[x,y] = Color.FromArgb(0,0,0,0);
                        }else{
                            byte newRed = (byte)(Math.Min(255, blackBackPixel.R / rpercentage));
                            byte newGreen = (byte)(Math.Min(255, blackBackPixel.G / gpercentage));
                            byte newBlue = (byte)(Math.Min(255, blackBackPixel.B / bpercentage));
                            dst[x,y] = ColorBgra.FromBgra(newBlue,newGreen,newRed,alpha);                        
                        }
                    }else{
                        //Opaque
                        dst[x,y] = SrcPixel;
                    }
                    //dst[x,y] = Color.FromArgb(redDif,greenDif,blueDif);
                }
            }
        }
    }
