using System.Drawing;
using System.Drawing.Imaging;

using NUnit.Framework;

namespace Echo.Testing
{
	[TestFixture]
	public class ImageTests
	{
		[Test, Ignore]
		public void CreateAlphaImage()
		{
			Image alpha = Image.FromFile(@"c:\temp\alpha-mask.png");
			Image image = Image.FromFile(@"c:\temp\image.png");

			Bitmap alphaG = new Bitmap(alpha);
			Bitmap imageG = new Bitmap(image);

			Bitmap surface = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					Color a = alphaG.GetPixel(x, y);
					Color i = imageG.GetPixel(x, y);

					if (i.A == 0 && a.A == 0)
						continue;

					bool lastTransparent = LeftClose(y, x, imageG, 4);
					bool nextTransparent = RightClose(y, x, imageG, 4);
					
					if (i.A == 0)
						i = a;
                    
					var brightness = 1.0 - ((nextTransparent || lastTransparent) ? i.GetBrightness() : a.GetBrightness());
                    var c2 = Color.FromArgb((byte )(brightness * 255), i.R, i.G, i.B);

					surface.SetPixel(x, y, c2);
				}

			surface.Save(@"c:\temp\result.png", ImageFormat.Png);
		}

		private bool RightClose(int y, int x, Bitmap imageG, int size)
		{
			if (x + size > imageG.Width)
				return true;

			for (int i = 0; i < size; i++)
			{
				if (imageG.GetPixel(x + i, y).A == 0)
					return true;
			}

			return false;
		}

		private bool LeftClose(int y, int x, Bitmap imageG, int size)
		{
			if (x < size)
				return true;

			for (int i = 0; i < size; i++)
			{
				if (imageG.GetPixel(x - i, y).A == 0)
					return true;
			}
            
			return false;
		}
	}
}