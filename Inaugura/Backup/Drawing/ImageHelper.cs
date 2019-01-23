using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Inaugura.Drawing
{
	/// <summary>
	/// Helper class for images
	/// </summary>
	public static class ImageHelper
    {
        #region Resizing

        /// <summary>
        /// Gets a resized image
        /// </summary>
        /// <param name="img">The source image</param>
        /// <param name="path">The path to the image</param>
        /// <param name="width">The image width</param>
        /// <param name="height">The image height</param>
        /// <param name="maxWidth">The max image width</param>
        /// <param name="maxHeight">The max image height</param>
        /// <returns>The image after resizing (if nessary) is applied</returns>
        /// <remarks>When the height or width exceed maxWith or maxHeight, the image will be scaled. The scale used will preserve the image aspect ratio.</remarks>
        public static Image Resize(Image img, int width, int height, int maxWidth, int maxHeight)
        {
            Size size = CalculateImageSize(img.Size, width, height, maxWidth, maxHeight);
            return Resize(img, size, SmoothingMode.None, InterpolationMode.HighQualityBilinear);
        }

        /// <summary>
        /// Gets a resized image
        /// </summary>
        /// <param name="path">The path to the image</param>
        /// <param name="size">The output image size</param>
        /// <param name="img">The source image</param>
        /// <param name="interpolationMode">The resize interpolation mode</param>
        /// <param name="smoothingMode">The resize smoothing mode</param>
        /// <returns>The scaled image</returns>
        /// <remarks>he image will be scaled. The scale used will preserve the image aspect ratio.</remarks>
        public static Image Resize(Image img, Size size, System.Drawing.Drawing2D.SmoothingMode smoothingMode, System.Drawing.Drawing2D.InterpolationMode interpolationMode)
        {
            Bitmap bitmap = new Bitmap(size.Width, size.Height, img.PixelFormat);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = smoothingMode;
                g.InterpolationMode = interpolationMode;
                g.DrawImage(img, 0, 0, bitmap.Width, bitmap.Height);
                return bitmap;
            }
        }  


        /// <summary>
        /// Creates a resized image which maintains the origional aspect ratio
        /// </summary>
        /// <param name="img">The source image</param>
        /// <param name="size">The new desired size</param>
        /// <returns>The resized image</returns>
        public static System.Drawing.Image Resize(System.Drawing.Image img, Size size)
        {
            System.Drawing.Size newSize = CalculateImageSize(img.Size, size.Width, size.Height);
            return Resize(img, newSize, SmoothingMode.None, InterpolationMode.HighQualityBilinear);
        }

        /// <summary>
        /// Creates a resized image which maintains the origional aspect ratio
        /// </summary>
        /// <param name="img">The source image</param>
        /// <param name="width">The new desired width</param>
        /// <param name="height">The new desired hight</param>
        /// <returns>The resized image</returns>
        public static System.Drawing.Image Resize(System.Drawing.Image img, int width, int height)
        {            
            return Resize(img, new Size(width, height));            
        }

        /// <summary>
        /// Calculates a new scaled image size. 
        /// ratios.
        /// </summary>
        /// <param name="imageSize">The current image size</param>
        /// <param name="width">The new image width</param>
        /// <param name="height">The new image height</param>
        /// <returns>The new image size</returns>
        private static Size CalculateImageSize(Size imageSize, int width, int height)
        {
            return CalculateImageSize(imageSize, 0, 0, width, height);
        }

        /// <summary>
        /// Calculates a new scaled image size. If both width and height are specified the image is scaled without locking aspect
        /// ratios.
        /// </summary>
        /// <param name="imageSize">The current image size</param>
        /// <param name="width">The new image width (or zero to omit)</param>
        /// <param name="height">The new image height (or zero to omit)</param>
        /// <param name="maxWidth">The maximum scaled width</param>
        /// <param name="maxHeight">The maximum scaled height</param>
        /// <returns></returns>
        private static Size CalculateImageSize(Size imageSize, int width, int height, int maxWidth, int maxHeight)
        {
            double y = imageSize.Height;
            double x = imageSize.Width;
            double factor = 1;

            if (width > 0 && height == 0)
            {
                factor = width / x;
                x = width;
                y = y * factor;
            }
            else if (height > 0 && width == 0)
            {
                factor = height / y;
                x = x * factor;
                y = height;
            }
            else if (height > 0 && width > 0)
            {
                x = width;
                y = height;
            }

            // adjust image size based on maxWidth and maxHeight limits
            double widthFactor = 0;
            double heightFactor = 0;

            if (maxWidth != 0 && x > maxWidth)
            {
                widthFactor = (double)maxWidth / (double)x;
            }
            if (maxHeight != 0 && y > maxHeight)
            {
                heightFactor = (double)maxHeight / (double)y;
            }

            factor = 1;

            // scale to the smallest factor
            if (widthFactor != 0 && widthFactor < factor)
                factor = widthFactor;
            if (heightFactor != 0 && heightFactor < factor)
                factor = heightFactor;

            return new Size(Convert.ToInt32(x * factor), Convert.ToInt32(y * factor));
        }

        #endregion

        #region Watermarking
        /// <summary>
		/// Creates an image with a watermark
		/// </summary>
		/// <param name="image">Thes source image</param>
		/// <param name="watermark">The watermark image</param>		
		/// <returns>A copy of the source image with the watermark applied</returns>
		public static Bitmap Watermark(Bitmap image, Bitmap watermark)
		{
			return Watermark(image, watermark, Color.Empty);
		}

        /// <summary>
        /// Creates an image with a watermark
        /// </summary>
        /// <param name="image">Thes source image</param>
        /// <param name="watermark">The watermark image</param>
        /// <param name="watermarkTransparentColor">The watermark transparent color</param>		
        /// <returns>A copy of the source image with the watermark applied</returns>
        public static Bitmap Watermark(Bitmap image, Bitmap watermark, Color watermarkTransparentColor)
        {
            return Watermark(image, watermark, watermarkTransparentColor, 0.5f);
        }

		/// <summary>
		/// Creates an image with a watermark
		/// </summary>
		/// <param name="image">Thes source image</param>
		/// <param name="watermark">The watermark image</param>
		/// <param name="watermarkTransparentColor">The watermark transparent color</param>		
        /// <param name="transparency">The amount of transparency</param>
		/// <returns>A copy of the source image with the watermark applied</returns>
        public static Bitmap Watermark(Bitmap image, Bitmap watermark, Color watermarkTransparentColor, float transparency)
		{
            int xPos = image.Width - watermark.Width;// -(int)(image.Width * .02);
            int yPos = image.Height - watermark.Height;// -(int)(image.Height * .02);

			return Watermark(image, watermark, watermarkTransparentColor, xPos, yPos, transparency);
		}

		
		/// <summary>
		/// Creates an image with a watermark
		/// </summary>
		/// <param name="image">Thes source image</param>
		/// <param name="watermark">The watermark image</param>
		/// <param name="watermarkTransparentColor">The watermark transparent color</param>
		/// <param name="posX">The x position of the upper left corner of the watermark</param>
		/// <param name="posY">The y poisition of the upper left corner of the watermark</param>
		/// <returns>A copy of the source image with the watermark applied</returns>
		public static Bitmap Watermark(Bitmap image, Bitmap watermark, Color watermarkTransparentColor, int posX, int posY, float transparency)
		{
			ImageAttributes imageAttributes = new ImageAttributes();
			ColorMap colorMap = new ColorMap();

			// Make the watermark transparent color actually transparent
			if (watermarkTransparentColor != Color.Empty)
			{
				colorMap.OldColor = watermarkTransparentColor;
				colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
				ColorMap[] remapTable = { colorMap };
				imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
			}

			float[][] colorMatrixElements = { 
				new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
				new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
				new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
				new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
				new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};

			ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

			imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

			Bitmap finalImage = new Bitmap(image);
			Graphics graphics = Graphics.FromImage(finalImage);
			graphics.DrawImage(watermark,
				new Rectangle(posX, posY, watermark.Width, watermark.Height),
				0,
				0,
				watermark.Width,
				watermark.Height,
				GraphicsUnit.Pixel,
				imageAttributes);

			return finalImage;
        }
        #endregion
    }
}
