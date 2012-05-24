using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Echo.Objects;
using Echo.Vectors;

namespace orbView
{
	public partial class Form1 : Form
	{
		private readonly SolarSystem sol = new SolarSystem();
		private float cX;
		private float cY;
		private long tick;

		public Form1()
		{
			var earth = new Planet(this.sol);
			sol.OrbitSun(earth, 365d);
			earth.AddSatellite(new Moon(earth), 28d);

			var mars = new Planet(this.sol);
			sol.OrbitSun(mars, 450d);

			var phobos = new Moon(mars);
			mars.AddSatellite(phobos, 41d);

			var deimos = new Moon(phobos);
			phobos.AddSatellite(deimos, 15d);

			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.cY = ClientRectangle.Height / 2f;
			this.cX = ClientRectangle.Width / 2f;
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.cY = Size.Height/2f;
			this.cX = Size.Width/2f;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.Clear(Color.White);

			var pPen = new Pen(Color.Blue);
			var mPen = new Pen(Color.Red);

			foreach (Planet p in sol.Satellites)
			{
				Draw(pPen, e.Graphics, p, 10f);
				var stack = new Stack<CelestialBody>();
				stack.Push(p);

				while ( stack.Count > 0 )
				{
					var tmp = stack.Pop();

					foreach ( CelestialBody m in tmp.Satellites )
					{
						stack.Push(m);
						Draw(mPen, e.Graphics, m, 4f);
					}
				}
			}
		}

		private void Draw(Pen eP, Graphics graphics, ILocation obj, float size)
		{
			float o = size/2;
			graphics.DrawRectangle(eP, (float)(obj.UniversalCoordinates.X * 1.5) + this.cX - o, (float)(obj.UniversalCoordinates.Y * 1.5) - o + this.cY, size, size);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			this.sol.Tick((ulong )this.tick);
			this.tick+=2;
			Invalidate();
		}
	}
}