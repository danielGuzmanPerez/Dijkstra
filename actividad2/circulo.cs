/*
 * Created by SharpDevelop.
 * User: vdgp_
 * Date: 16/02/2020
 * Time: 12:40 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace actividad2
{
	/// <summary>
	/// Description of Datos.
	/// </summary>
	public class circulo
	{
		public int x;
		public int y;
		public int r;
		public circulo(int x_,int y_, int r_)
		{
			x=x_;
			y=y_;
			r=r_;
		}
		public override string ToString()
{
	return string.Format("[ X={0}, Y={1}, R={2}]", x, y, r);
}
		public int getX(){
			return x;
		}
		public int getY(){
			return y;
		}

	}
}
