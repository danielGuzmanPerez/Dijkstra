/*
 * Created by SharpDevelop.
 * User: vdgp_
 * Date: 13/05/2020
 * Time: 05:14 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace actividad2
{
	/// <summary>
	/// Description of opcion.
	/// </summary>
	public class opcion
	{
		public int ID;
		public int distancia;
		public opcion(int id_,int distancia_)
		{
			ID=id_;
			distancia=distancia_;
		}
		public int getId(){
			return ID;
		}
		public int GetDistancia(){
			return distancia;
		}
	}
}
