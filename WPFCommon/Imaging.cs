using System;
using System.Windows.Media.Imaging;

namespace WPFCommon
{
    public static class Imaging
    {
        public static Uri LoadResource(string file)
        {
            try
            {
                return new Uri(file, UriKind.RelativeOrAbsolute);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// リソースから画像を取得して返却します。
        /// </summary>
        /// <param name="imagePath">読み込み対象の画像ファイルのパス</param>
        /// <returns>読み込んだ画像のImageオブジェクト</returns>
        public static BitmapImage LoadImageResource(string imagePath)
        {
            try
            {
                var img = new BitmapImage(LoadResource(imagePath))
                {
                    CacheOption = BitmapCacheOption.OnLoad
                };
                img.Freeze();
                return img;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}