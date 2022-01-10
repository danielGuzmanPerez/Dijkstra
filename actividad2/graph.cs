/*
 * Created by SharpDevelop.
 * User: vdgp_
 * Date: 16/02/2020
 * Time: 11:35 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;

namespace actividad2
{
	/// <summary>
	/// Description of graph.
	/// </summary>
	public class Edge{
		public vertex v_d;
		public float distancia;
		public Edge( vertex v_){
	        v_d = v_;
	    }
		public Edge(){
			
		}
		public void  setVd(vertex v){
			v_d=v;
		}
	    vertex getV(){
	    	return v_d;
		}
		public int getX(){
			return v_d.circulo.x;
		}
		public int getY(){
			return v_d.circulo.y;
		}
	}
	
	
	
	public class vertex{
	    public List<Edge> eL;
		public circulo circulo;
		//public List<vertex> vert;
		public int id;
		bool definitivo;
	public vertex(	circulo c, int id){
		this.circulo= c;
		this.id=id;
		definitivo= false;
	}
    public vertex(	){
		
	}
	public int getEdgesCount(){
		return eL.Count;
	}
	//public ciculo getCircle(){}
	public void llenarEdge(List<vertex> ve){
		eL=new List<Edge>();
		for(int i=0; i<ve.Count;i++)
			eL.Add(new Edge(ve[i]));
	}
	public int getX(){
		return circulo.x;
	}
	public int getY(){
		return circulo.y;
	}
	
	public List<Edge> getEdge(){
		return eL;
	}
	
	
	}
	public class graph{
		public List<vertex> v1;
		List<circulo> circulos;
		List<vertex> opciones;
		vertex v;
		Bitmap bmp;
		Color c;
		int x1,x2,y1,y2;
		
		public graph(List<circulo> c, Bitmap bmp_)
		{
			
			bmp=bmp_;
			v1= new List<vertex>();
			circulos=c;
			Graphics gra;
			Brush b1 = new SolidBrush(Color.White);
			Brush b2 = new SolidBrush(Color.Red);
			RectangleF cloneRect = new RectangleF(0, 0, bmp.Width, bmp.Height);
			for(int j =0;j<c.Count; j++){
					v= new vertex(c[j],j+1);
					v1.Add(v);
				}
			Bitmap bmpComprobar;
			for(int i=0; i<v1.Count;i++){
				x1=v1[i].getX();
				y1=v1[i].getY();
				opciones=new List<vertex>();
				for(int j=0; j<v1.Count; j++){
					if(i!=j){
					x2=v1[j].getX();
					y2=v1[j].getY();
					//copiar el bitmap
					//System.Drawing.Imaging.PixelFormat format = bmp.PixelFormat;
					//bmpComprobar=bmp.Clone(cloneRect,format);
					//gra = Graphics.FromImage(bmpComprobar);
					//PINTAR EL CIRCULO  DE BLANCO
					/*gra.FillEllipse(b1,x1-v1[i].circulo.r-4,y1-v1[i].circulo.r-4,(v1[i].circulo.r+3)*2,(v1[i].circulo.r+3)*2);
					gra.FillEllipse(b1,x1-v1[j].circulo.r-4,y1-v1[j].circulo.r-4,(v1[j].circulo.r+3)*2,(v1[j].circulo.r+3)*2);*/
					if(verificar(x1,y1,x2,y2,v1[j].circulo.r)==true){
						opciones.Add(v1[j]);     
					}
					//bmpComprobar.Dispose();
				   }
					
				}	
				v1[i].llenarEdge(opciones);	
				//v1[i].vert=opciones;
			}
			obtenerDistancia();
			//Ingresar id a los circulos
				 Font drawFont = new Font("Arial", 20);
   				 SolidBrush drawBrush = new SolidBrush(Color.Black);
   				 StringFormat drawFormat = new StringFormat();
    			 drawFormat.FormatFlags = StringFormatFlags.DisplayFormatControl;
    			 gra = Graphics.FromImage(bmp);
				for(int i=0;i<v1.Count;i++){
    			 	gra.DrawString(v1[i].id.ToString(), drawFont, drawBrush, v1[i].getX(),v1[i].getY(), drawFormat);
				}
			
	//pintar las lineas		
			for(int i=0; i<v1.Count;i++){
				//Esta lista tiene las opciones de cada circulo
				List<Edge> edge= v1[i].eL;
				x1=v1[i].getX();
				y1=v1[i].getY();
					
				for(int j=0; j<edge.Count; j++){
					if(j!=i){
					x2=edge[j].getX();
					y2=edge[j].getY();
					linea(x1,y1,x2,y2,Color.Black);
					}
				}
			}
			
		}
	
		public int getVertexCount(){
			return v1.Count;
		}
		bool verificar(int x1_, int y1_, int x2_, int y2_,int radio){
			int x1=x1_;
			int x2=x2_;
			int y1=y1_;
			int y2=y2_;
			float deltax=x2-x1;
			float deltay = y2-y1;
			int band=0;
			if(Math.Abs(deltax)> Math.Abs(deltay)){  // pendiente <1
				float m= (float)deltay/(deltax);
				float b= y1-m*x1;
				if(deltax<0)
					deltax=-1;
				else
					deltax=1;
				while(x1!= x2){
					x1+=(int)deltax;
					y1=(int)Math.Round((double)m*(double)x1+(double)b);
					c=bmp.GetPixel(x1,y1);
					if( band ==0){
						if(c.R!=255 || c.G != 0 || c.B!= 0){
							band=1;
						}
					}else{
						if(band==1){
							if(c.R!=255 || c.G != 255 || c.B!= 255){
								band=2;
							}
						}
						if(band==2){
							if((x1>=x2-radio-5) && (x1<=x2+radio+5) && (y1>=y2-radio-5) && (y1<=y2+radio+5)){
								return true;
							}
							return false;
						}
					}
			}
			}else
				if(deltay!=0){   //pendiente >=1
				float m=(float)deltax/(float)deltay;
				float b= x1-m*y1;
				if(deltay<0)
					deltay=-1;
				else
					deltay=1;
				while(y1!=y2){
					y1+= (int)deltay;
					x1=(int)Math.Round((double)m*(double)y1+(double)b);
					c=bmp.GetPixel(x1,y1);
					if( band ==0){
						if(c.R!=255 || c.G != 0 || c.B!= 0){
							band=1;
						}
					}else{
						if(band==1){
							if(c.R!=255 || c.G != 255 || c.B!= 255){
								band=2;
							}
						}
						if(band==2){
							if((x1>=x2-radio-5) && (x1<=x2+radio+5) && (y1>=y2-radio-5) && (y1<=y2+radio+5)){
								return true;
							}
							return false;
						}
					}
				}
			}
				
			
		
			return true;
		}
		void linea(int x1, int y1, int x2, int y2,Color co){
			float deltax=x2-x1;
			float deltay = y2-y1;
			if(Math.Abs(deltax)> Math.Abs(deltay)){  // pendiente <1
				float m= (float)deltay/(deltax);
				float b= y1-m*x1;
				if(deltax<0)
					deltax=-1;
				else
					deltax=1;
				while(x1!= x2){
					x1+=(int)deltax;
					y1=(int)Math.Round((double)m*(double)x1+(double)b);
					bmp.SetPixel(x1,y1,co);
					
			}
			}else
				if(deltay!=0){   //pendiente >=1
				float m=(float)deltax/(float)deltay;
				float b= x1-m*y1;
				if(deltay<0)
					deltay=-1;
				else
					deltay=1;
				while(y1!=y2){
					y1+= (int)deltay;
					x1=(int)Math.Round((double)m*(double)y1+(double)b);
					bmp.SetPixel(x1,y1,co);
					
				}
			}
		}
		public Bitmap getBmp(){
			return bmp;
		}
	
		public List<vertex>getList(){
			return v1;
		}
		void obtenerDistancia(){
			int x1,x2,y1,y2;
			int catetoA=0,catetoB=0;
			float distancia=0;
			for(int i=0; i<v1.Count;i++){
				for(int j=0; j<v1[i].eL.Count;j++){
							x1=v1[i].getX();
							y1=v1[i].getY();
							x2=v1[i].eL[j].getX();
							y2=v1[i].eL[j].getY();
							//distancias iguales en y
							if(y1==y2){
								if(x1>x2)
									catetoA=x1-x2;
								else{
									catetoA=x2-x1;
								}
								
									distancia=catetoA;
							}
							//distancia igual en x
							if(x1==x2){
								if(y1>y2)
									catetoA=y1-y2;
								else{
									catetoA=y2-y1;
								}
									distancia=catetoA;
							}
							if(x1!=x2 && y1!=y2){
								if(x1>x2){
									 catetoA = x1-x2;
								}else
									 catetoA= x2-x1;
								if(y1>y2){
									 catetoB= y1-y2;
								}else{
									catetoB = y2-y1;
								}
								catetoA=(int)Math.Pow((double)catetoA,(double)2);
								catetoB=(int)Math.Pow((double)catetoB,(double)2);
									distancia=(float)Math.Sqrt((Double)catetoA+(Double)catetoB);	
									
							}
					v1[i].eL[j].distancia=distancia;
			}
				
		}
		}
		
		}
		
}
