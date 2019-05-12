﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageProcessing
{
    public class GrayScale : ComImgProc
    {
        public GrayScale(BitmapImage _bitmap) : base(_bitmap)
        {
        }

        ~GrayScale()
        {
        }

        public override bool GoImgProc(CancellationToken _token)
        {
            bool bRst = true;

            int nWidthSize = base.Bitmap.PixelWidth;
            int nHeightSize = base.Bitmap.PixelHeight;

            base.WriteableBitmap = new WriteableBitmap(base.Bitmap);
            base.WriteableBitmap.Lock();

            int nIdxWidth;
            int nIdxHeight;

            unsafe
            {
                for (nIdxHeight = 0; nIdxHeight < nHeightSize; nIdxHeight++)
                {
                    for (nIdxWidth = 0; nIdxWidth < nWidthSize; nIdxWidth++)
                    {
                        if (_token.IsCancellationRequested)
                        {
                            bRst = false;
                            break;
                        }

                        byte* pPixel = (byte*)base.WriteableBitmap.BackBuffer + nIdxHeight * base.WriteableBitmap.BackBufferStride + nIdxWidth * 4;
                        byte byteGrayScale = (byte)((pPixel[(int)ComInfo.Pixel.B] + pPixel[(int)ComInfo.Pixel.G] + pPixel[(int)ComInfo.Pixel.R]) / 3);

                        pPixel[(int)ComInfo.Pixel.B] = byteGrayScale;
                        pPixel[(int)ComInfo.Pixel.G] = byteGrayScale;
                        pPixel[(int)ComInfo.Pixel.R] = byteGrayScale;
                    }
                }
                base.WriteableBitmap.AddDirtyRect(new Int32Rect(0, 0, nWidthSize, nHeightSize));
                base.WriteableBitmap.Unlock();
                base.WriteableBitmap.Freeze();
            }

            return bRst;
        }
    }
}