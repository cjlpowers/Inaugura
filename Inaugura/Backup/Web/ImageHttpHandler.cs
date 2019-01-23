using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Drawing;
using System.Drawing.Imaging;

namespace Inaugura.Web
{
    /// <summary>
    /// A http handler which performs image loading
    /// </summary>
    public abstract class ImageHttpHandler : HttpHandler, IReadOnlySessionState
    {
        #region Internal Constructs
        public enum ImageMode
        {            
            Size80,
            Size160,
            Size200,
            Size320,
            Size480,
            Size640,
            Size800,
            Size1024        
        }
        #endregion

        #region Variables
        private string mWatermarkImagePath = string.Empty;
        private string mImageCachePath = "~/Content/Images/";
        private ImageFormat mOutputFormat = ImageFormat.Jpeg;
        private float mWatermarkTransparency;
        #endregion

        #region Properties
        /// <summary>
        /// The output image format
        /// </summary>
        protected ImageFormat OutputFormat
        {
            get
            {
                return this.mOutputFormat;
            }
            set
            {
                this.mOutputFormat = value;
            }
        }

        /// <summary>
        /// The path where images will be stored
        /// </summary>
        protected string ImageCachePath
        {
            get
            {
                return this.mImageCachePath;
            }
            set
            {
                this.mImageCachePath = value;
            }
        }

        /// <summary>
        /// The transparency of the watermark
        /// </summary>
        protected float WatermarkTransparency
        {
            get
            {
                return this.mWatermarkTransparency;
            }
            set
            {
                this.mWatermarkTransparency = value;
            }
        }

        /// <summary>
        /// The watermark image path
        /// </summary>
        protected string WatermarkPath
        {
            get
            {
                return this.mWatermarkImagePath;
            }
            set
            {
                this.mWatermarkImagePath = value;
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="imagePath">The path to the images</param>
        /// <param name="watermarkPath">The path to the watermark image</param>
        public ImageHttpHandler(string imagePath, string watermarkPath, float watermarkTransparency)
        {
            this.ImageCachePath = imagePath;
            this.WatermarkPath = watermarkPath;
            this.WatermarkTransparency = watermarkTransparency;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="imagePath">The path to the images</param>
        public ImageHttpHandler(string imagePath) : this(imagePath,string.Empty,0.5f)
        {
        }

        /// <summary>
        /// Handles the request for a report document
        /// </summary>		
        /// <remarks>The Http Handler caches the last 50 images as a performance optimization.
        /// The C1 Report tends to load every image in the report when the active page is changed, regardless if the
        /// image will infact be visible or not. The caching of the most recent 50 images attempts compensate for this (not ideal though).
        /// </remarks>
        /// <param name="context">Context.</param>
        public override void HandleRequest(HttpContext context)
        {
            ImageFormat format = this.OutputFormat;
            string imageResourceID = context.Request.Params["id"];
            context.Response.Clear();

            System.Drawing.Image img = null;
            string strMode = string.Empty;
            if (context.Request.Params["mode"] != null)
                strMode = context.Request.Params["mode"];
            
            ImageMode mode = GetMode(strMode);
            img = this.GetImage(imageResourceID, mode);

            if (img != null)
            {
                using (System.IO.MemoryStream outStream = new System.IO.MemoryStream(1024))
                {
                    img.Save(outStream, OutputFormat); // dont pass the path so we get the default image format
                    context.Response.BufferOutput = false;
                    context.Response.AddHeader("content-disposition", string.Format("filename={0}.{1}", imageResourceID, GetExtension(format)));
                    context.Response.ContentType = GetMimeType(format);
                    context.Response.OutputStream.Write(outStream.ToArray(), 0, (int)outStream.Length);
                }
                img.Dispose();
            }
            else
                this.RespondFileNotFound(context);
        }

        /// <summary>
        /// Validates the parameters.  Inheriting classes must
        /// implement this and return true if the parameters are
        /// valid, otherwise false.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <returns><c>true</c> if the parameters are valid,
        /// otherwise <c>false</c></returns>
        public override bool ValidateParameters(HttpContext context)
        {
            if (context.Request.Params["id"] == null)
                return false;

            return true;
        }


        #region Loading/Saving      

        private ImageMode GetMode(string mode)
        {
            if(mode == "1024")
                return ImageMode.Size1024;
            else if(mode == "800")
                return ImageMode.Size800;
            else if(mode == "640")
                return ImageMode.Size640;
            else if(mode == "480")
                return ImageMode.Size480;
            else if(mode == "320")
                return ImageMode.Size320;
            else if(mode == "200")
                return ImageMode.Size200;            
            else if(mode == "80")
                return ImageMode.Size80;
            else // 160
                return ImageMode.Size160;
        }

        private Size GetImageSize(ImageMode mode)
        {
            if (mode == ImageMode.Size1024)
                return new Size(1024, 768);
            else if (mode == ImageMode.Size800)
                return new Size(800, 600);
            else if (mode == ImageMode.Size640)
                return new Size(640, 480);
            else if (mode == ImageMode.Size480)
                return new Size(480, 360);
            else if (mode == ImageMode.Size320)
                return new Size(320, 240);
            else if (mode == ImageMode.Size200)
                return new Size(200, 150);
            else if (mode == ImageMode.Size160)
                return new Size(160, 120);              
            else if (mode == ImageMode.Size80)
                return new Size(80, 60);              
            else
                throw new NotSupportedException("The image mode is not supported");
        }

        private string GetCacheDirectory(ImageMode mode)
        {
            string filePath = this.ImageCachePath;

            if (filePath.StartsWith("~"))
                filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);

            string ext = this.GetExtention();

            if (mode == ImageMode.Size1024)
                filePath = System.IO.Path.Combine(filePath, "1024x768\\");
            else if (mode == ImageMode.Size800)
                filePath = System.IO.Path.Combine(filePath, "800x600\\");
            else if (mode == ImageMode.Size640)
                filePath = System.IO.Path.Combine(filePath, "640x480\\");
            else if (mode == ImageMode.Size480)
                filePath = System.IO.Path.Combine(filePath, "480x360\\");
            else if (mode == ImageMode.Size320)
                filePath = System.IO.Path.Combine(filePath, "320x240\\");
            else if (mode == ImageMode.Size200)
                filePath = System.IO.Path.Combine(filePath, "200x150\\");
            else if (mode == ImageMode.Size160)
                filePath = System.IO.Path.Combine(filePath, "160x120\\");
            else if (mode == ImageMode.Size80)
                filePath = System.IO.Path.Combine(filePath, "80x60\\");
            else
                throw new NotSupportedException("The image mode is not supported");

            return filePath;
        }

        private string GetCacheImagePath(string imageName, ImageMode mode)
        {
            string directoryPath = this.GetCacheDirectory(mode);
            string ext = this.GetExtention();
            return System.IO.Path.Combine(directoryPath,string.Format("{0}.{1}", imageName, ext));
        }

        private string GetImagePath(string imageName)
        {
            string path = this.ImageCachePath;
            string ext = this.GetExtention();
            if (path.StartsWith("~"))
                path = System.Web.HttpContext.Current.Server.MapPath(path);
            return System.IO.Path.Combine(path, string.Format("{0}.{1}", imageName, ext));
        }

        /// <summary>
        /// Loads a cached image
        /// </summary>
        /// <param name="imageName">The name of the image</param>
        /// <param name="mode">The image mode</param>
        /// <returns>The image</returns>
        protected virtual Image LoadCachedImage(string imageName, ImageMode mode)
        {
            // does the file exist in cache?
            string path = GetCacheImagePath(imageName, mode);
            
            if (!System.IO.File.Exists(path))
                return null;
            else
                return Image.FromFile(path);            
        } 

        /// <summary>
        /// Loads a uncached image
        /// </summary>
        /// <param name="imageName">The name of the image</param>
        /// <returns>The image</returns>
        protected virtual Image LoadImage(string imageName)
        {
            // does the image exist
            string path = GetImagePath(imageName);

            if (!System.IO.File.Exists(path))
                return null;
            else
                return Image.FromFile(path);            
        }
        #endregion              

        protected virtual  System.Drawing.Image GetImage(string imageName, ImageMode mode)
        {
            try
            {
                // see if we already have it cached
                Image img = this.LoadCachedImage(imageName, mode);

                if (img != null)
                    return img;

                // the image has not been cached so look for it
                using (img = this.LoadImage(imageName))
                {
                    if (img != null)
                    {
                        Size imageSize = this.GetImageSize(mode);
                        if (this.WatermarkPath != null && this.WatermarkPath != string.Empty && imageSize.Width > 320) // do a watermark
                        {
                            string watermarkPath = this.WatermarkPath;
                            if (watermarkPath.StartsWith("~"))
                                watermarkPath = System.Web.HttpContext.Current.Server.MapPath(watermarkPath);

                            using (System.Drawing.Image resizedImage = Inaugura.Drawing.ImageHelper.Resize(img, imageSize))
                            {
                                using (Image watermark = Bitmap.FromFile(watermarkPath))
                                {
                                    Bitmap watermarkedImage = Inaugura.Drawing.ImageHelper.Watermark((Bitmap)resizedImage, (Bitmap)watermark, Color.FromArgb(255, 0, 255), this.WatermarkTransparency);
                                    watermarkedImage.Save(this.GetCacheImagePath(imageName, mode), this.OutputFormat);
                                    return watermarkedImage;
                                }
                            }
                        }
                        else
                        {
                            System.Drawing.Image resizedImage = Inaugura.Drawing.ImageHelper.Resize(img, imageSize);
                            resizedImage.Save(this.GetCacheImagePath(imageName, mode), this.OutputFormat);
                            return resizedImage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO
            }
            return null;
        }

        /// <summary>
        /// Sets the cache policy.
        /// </summary>
        /// <param name="cache">Cache.</param>
        public override void SetResponseCachePolicy(HttpCachePolicy cache)
        {
            cache.SetCacheability(HttpCacheability.Public);
            cache.SetExpires(DateTime.Now.AddHours(2));
        }

        /// <summary>
        /// Gets the mime type
        /// </summary>
        /// <param name="format">The image format</param>
        /// <returns>The mime type string</returns>
        private string GetMimeType(ImageFormat format)
        {
            if (format == ImageFormat.Bmp)
                return "Image/bmp";
            if (format == ImageFormat.Gif)
                return "Image/gif";
            if (format == ImageFormat.Jpeg)
                return "Image/jpeg";
            if (format == ImageFormat.Png)
                return "Image/png";
            if (format == ImageFormat.Tiff)
                return "Image/tiff";
            return null;
        }

        /// <summary>
        /// Gets the extension for a specific image format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private string GetExtension(ImageFormat format)
        {
            if (format == ImageFormat.Bmp)
                return "bmp";
            if (format == ImageFormat.Gif)
                return "gif";
            if (format == ImageFormat.Jpeg)
                return "jpg";
            if (format == ImageFormat.Png)
                return "png";
            if (format == ImageFormat.Tiff)
                return "tiff";
            return null;
        }

        /// <summary>
        /// Gets the extention for the output format
        /// </summary>
        /// <returns></returns>
        private string GetExtention()
        {
            return this.GetExtension(this.OutputFormat);
        }

        private ImageFormat GetImageFormat(String path)
        {
            switch (System.IO.Path.GetExtension(path))
            {
                case ".bmp": return ImageFormat.Bmp;
                case ".gif": return ImageFormat.Gif;
                case ".jpg": return ImageFormat.Jpeg;
                case ".png": return ImageFormat.Png;
                default: break;
            }
            return ImageFormat.Jpeg;
        }
    }

}
