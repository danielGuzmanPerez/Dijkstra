/*
 * Created by SharpDevelop.
 * User: vdgp_
 * Date: 15/02/2020
 * Time: 11:54 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace actividad2
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		String imagen;
		Bitmap bmp,temporal;
		Color c;
		List<circulo> Lista;
		graph Grafo;
		List<vertex> vertice; 
		Graphics gra;
		int cont;
		bool validarOrigen;
		List<candidato> listaDijkstra;
		Bitmap temp;
		List<int> camino;
		int orig;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			Lista=new List<circulo>();
			cont=0;
			
			//label1.Text= "Seleccione Origen";
			
		
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void Button2Click(object sender, EventArgs e)
		{
				for (int i = 1; i < camino.Count; i++) {
					animar(Lista[camino[i-1]-1].x,Lista[camino[i-1]-1].y,Lista[camino[i]-1].x,Lista[camino[i]-1].y);		
				}
				//la ultima particula sera el nuevo inicio
				int origen=camino[camino.Count-1];
				listaDijkstra= new List<candidato>();
				inicializarListaDijkstra(origen);
					orig=origen;
				try{
					dijkstra(origen);
				}catch(Exception ){
					mensaje m= new mensaje();
					//m.Visible=true;
					m.ShowDialog();
				}
				label1.Text= "Seleccione Destino";
				validarOrigen=true;
		}
		void Button1Click(object sender, EventArgs e)
		{
			if(openFileDialog1.ShowDialog()==DialogResult.OK){
				if(bmp!=null){
					bmp.Dispose();
					pictureBox1.BackgroundImage.Dispose();
				}
				validarOrigen=false;
				
				//NOTA ################ BUSCAR COMO AGREGAR MAS DE UNA IMAGEN A UN BITMAP
				imagen= openFileDialog1.FileName;
				bmp = new Bitmap(imagen);
				Lista.Clear();
				listBox1.Items.Clear();
				FindCircle();
				Grafo= new graph(Lista,bmp);
				pictureBox1.BackgroundImage=bmp=Grafo.getBmp();
				System.Drawing.Imaging.PixelFormat format = bmp.PixelFormat;
				RectangleF cloneRect = new RectangleF(0, 0, bmp.Width, bmp.Height);
				 temp=new Bitmap(bmp.Clone(cloneRect,format));
				 pictureBox1.Image=temp;
				 validarOrigen= false;
				listBox1.Enabled=false;
				for(int i=0; i<Lista.Count;i++){
					listBox1.Items.Add(i+1);
				}
			}
		}
		void FindCircle(){
			
			for(int y=0;y<bmp.Height;y++){
				for(int x=0; x<bmp.Width;x++){
					c=bmp.GetPixel(x,y);
					if(c.R==0 && c.G==0 && c.B== 0){
						Lista.Add( SaveCircle(x,y));
			
					}
				}
			}
		 }
		circulo SaveCircle(int x, int y){
			int j=x;
			int i=y;
			//coordenadas del centro del circulo
			int puntoX;
			int puntoY;
			// guardan la cantidad de pixeles que hay del centro hasta la orilla
			int orillaDer;
			int orillaInf;
			//valor de pixel de cada limite
			int izqX;
			int derX;
			int arribaY;
			int abajoY;
			
			//Encontrar el punto medio de  arriba x
			while(j<bmp.Width ){
				c= bmp.GetPixel(j,i);
				if(c.R !=0 || c.G !=0 || c.B !=0){
					c= bmp.GetPixel(j+4,i);
					if(c.R !=255 || c.G !=255 || c.B !=255){
						bmp.SetPixel(j,i,Color.Black);
						
					}else{
						bmp.SetPixel(j,i,Color.White);
						break;
					}
				}else{
					j++;
				}
				
			}
			j=x+(j-x)/2;
			puntoX=j;
			arribaY=i;
			
			//Encontrar el medio de abajo  y centro
			while(i<bmp.Height ){
				c= bmp.GetPixel(j,i);
				if(c.R !=0 || c.G !=0 || c.B !=0){
					break;
				}else{
					i++;
				}
				
			}
			puntoY =(y+i)/2 ;
			//encontrar orilla derecha
			while(j<bmp.Width){
				c= bmp.GetPixel(j,puntoY);
				if(c.R !=0 || c.G !=0 || c.B !=0){
					break;
				}else{
					j++;
				}
				
			}
			derX=j;
			orillaDer=j-puntoX;
			orillaInf=i-puntoY;
			
			//encontrar limite izquierda
			j--;
			while(j>1 ){
				c= bmp.GetPixel(j,puntoY);
				if(c.R !=0 || c.G !=0 || c.B !=0){
					break;
				}else{
					j--;
				}
				
			}
			izqX=j;
			j=puntoY;//
			//encontrar limite abajo
			while(j<bmp.Height ){
				c= bmp.GetPixel(puntoX,j);
				if(c.R !=0 || c.G !=0 || c.B !=0){
					break;
				}else{
					j++;
				}
				
			}
			abajoY=j;
			pintarCirculo(puntoX,puntoY,izqX-4,arribaY-4,derX+4,abajoY+4);
			int radio=orillaDer;
			if(orillaInf>orillaDer){
				radio=orillaInf;
			}
			
				
			return new circulo(puntoX,puntoY,radio+3);
				
			
		}
		void pintarCirculo(int puntox,int puntoy,int izquierdax,int arribay,int derechax,int abajoy){
			
			//pintar 1/4
			for(int j=puntox; j>izquierdax;j--){
				for(int i=puntoy;i>arribay;i--){
					c=bmp.GetPixel(j,i);
						if(c.G!=255 && c.B!=255 && c.R!=255){
							bmp.SetPixel(j,i,Color.Red);
							
					}else{
						break;
					}
				}
			}
			//pintar 2/4
			for(int j=puntox; j<derechax;j++){
				for(int i=puntoy;i>arribay;i--){
					c=bmp.GetPixel(j,i);
						if(c.G!=255 && c.B!=255 && c.R!=255){
							bmp.SetPixel(j,i,Color.Red);
					}else{
						break;
					}
				}
			}
			//pintar 3/4
			for(int j=puntox; j<derechax;j++){
				for(int i=puntoy+1;i<abajoy;i++){
					c=bmp.GetPixel(j,i);
						if(c.G!=255 && c.B!=255 && c.R!=255){
							bmp.SetPixel(j,i,Color.Red);
					}else{
						break;
					}
				}
			}
			
			//pintar 4/4
			for(int j=puntox; j>izquierdax;j--){
				for(int i=puntoy+1;i<abajoy;i++){
					c=bmp.GetPixel(j,i);
						if(c.G!=255 && c.B!=255 && c.R!=255){
							bmp.SetPixel(j,i,Color.Red);
					}else{
						break;
					}
				}
			}
		}
		void Button3Click(object sender, EventArgs e)
		{
			validarOrigen=false;
			label1.Text= "Seleccione Origen";
			listBox1.Enabled=true;
			
		}
		
		void PictureBox1MouseClick(object sender, MouseEventArgs e)
		{
			
		}
		public int encontrarCirculo(int x, int y){
			for(int i=0; i<Lista.Count;i++){
				if((x>=Lista[i].x-50) && (x<=Lista[i].x+50) && (y>=Lista[i].y-50) && (y<=Lista[i].x+50)){
					return i;
				}
			}
			return -1;
		}
		
		
		void animar(int x1_,int y1_,int x2_,int y2_ ){
			int x1=x1_;
			int x2=x2_;
			int y1=y1_;
			int y2=y2_;
			int xAnterior=x1_;
			int yAnterior=y1_;
			float deltax=x2-x1;
			float deltay = y2-y1;
			//esto sirve para copiar el bitmap
			System.Drawing.Imaging.PixelFormat format = bmp.PixelFormat;
			RectangleF cloneRect = new RectangleF(0, 0, bmp.Width, bmp.Height);
			
			//esto es para pintar el circulo
			Graphics gra2;
			Brush b1 = new SolidBrush(Color.Blue);
			Brush b2 = new SolidBrush(Color.FromArgb(0,Color.White));
			//Brush b2 = new SolidBrush(Color.White);
			temporal=new Bitmap(bmp.Clone(cloneRect,format));
			gra2 = Graphics.FromImage(temporal);
			pictureBox1.Image=temporal;
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
					if(x1<=x2+4 && x1>=x2-4)break;
					if(xAnterior< x1){
						x1+=8;
					}else x1-=8;
					/*if(yAnterior< y1){
						y1+=2;
					}else y1-=2;*/
					//aqui se dibuja el circulo
					gra2.Clear(Color.Transparent);
					gra2.FillEllipse(b1,x1-20,y1-20,40,40);
					pictureBox1.Refresh();
					xAnterior=x1;
					yAnterior=y1;
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
					//acelerar el proceso
					if(y1<=y2+4 && y1>=y2-4)break;
					if(yAnterior< y1){
						y1+=8;
					}else y1-=8;
					gra2.Clear(Color.Transparent);
					gra2.FillEllipse(b1,x1-20,y1-20,40,40);
					pictureBox1.Refresh();
					xAnterior=x1;
					yAnterior=y1;
				}
			}
			gra2.Clear(Color.Transparent);
			pictureBox1.Refresh();
			gra2.Dispose();
		}
		void ListBox1MouseClick(object sender, MouseEventArgs e)
		{
				orig=0;
			if(!validarOrigen){//si el origen no ha sido seleccionado
				listaDijkstra= new List<candidato>();
				int origen=Int32.Parse(listBox1.GetItemText(listBox1.SelectedItem));
				inicializarListaDijkstra(origen);
					orig=origen;
				try{
					dijkstra(origen);
				}catch(Exception ){
					mensaje m= new mensaje();
					//m.Visible=true;
					m.ShowDialog();
				}
				label1.Text= "Seleccione Destino";
				validarOrigen=true;
			}else{
				//validarOrigen=false;
				int destino=Int32.Parse(listBox1.GetItemText(listBox1.SelectedItem));
				 camino= GuardarCamino(orig,destino);
				dibujar(camino);
				
			}
		}
		void inicializarListaDijkstra(int origen){
			candidato opc;
			opcion op;
			for (int i = 0; i < Grafo.v1.Count; i++) {
				List<opcion>ids= new List<opcion>();
				for (int j = 0; j <Grafo.v1[i].eL.Count; j++) {
					op= new opcion(Grafo.v1[i].eL[j].v_d.id,(int)Math.Round(Grafo.v1[i].eL[j].distancia));
					ids.Add(op);
				}
				if(Grafo.v1[i].id== origen)
				 opc= new candidato(Grafo.v1[i].id,0,ids);
				else
					opc= new candidato(Grafo.v1[i].id,int.MaxValue-1,ids);
				
				listaDijkstra.Add(opc);
					
			}
		}
		void dijkstra(int origen){
			int definitivo;
			//listaDijkstra[origen].definitivo=true;
			while(!terminar()){
				
				definitivo=seleccionarDefinitivo();//elegir el menor(posición)
				actualizaVD(definitivo);//actualiza los valores
				
			}
				
			}
		bool terminar(){//revisa si ya todas las opciones son definitivos
			bool mandar=true;
			for (int i = 0; i < listaDijkstra.Count; i++) {
				if(!listaDijkstra[i].definitivo)
					mandar=false;
			}
			return mandar;
		}
		int seleccionarDefinitivo(){//busca el menor valor y lo hace definitivo
			int minimo=int.MaxValue;
			int x=0;
			for (int i = 0; i <listaDijkstra.Count; i++) {
				if(listaDijkstra[i].peso<minimo && !listaDijkstra[i].definitivo){
					minimo=listaDijkstra[i].peso;
					x=i;
				}
			}
			listaDijkstra[x].setDefinitivo(true);
			return x;
		}
		void actualizaVD(int definitivo){
			int valor=0;
			int n=0;
			int posicion=0;
			for (int i = 0; i < listaDijkstra[definitivo].opciones.Count; i++) {
				valor=listaDijkstra[definitivo].peso+listaDijkstra[definitivo].opciones[i].distancia;
				posicion=listaDijkstra[definitivo].opciones[i].ID-1;
				if( valor<listaDijkstra[posicion].peso && !listaDijkstra[posicion].definitivo){
					//n=listaDijkstra[definitivo].opciones[posicion].ID-1;
					listaDijkstra[posicion].peso=valor;
					listaDijkstra[posicion].setProveniente(definitivo+1);
				}
				
			}
		}
		List<int> GuardarCamino(int origen,int destino){
			List<int>list= new List<int>();
			int dest=destino;
			list.Add(destino);
			while(true){
				if(dest==origen)
					break;
				dest=listaDijkstra[dest-1].proveniente;
				list.Add(dest);
			}
			list.Reverse();
			list.RemoveAt(0);
			return list;
				
		}
		void dibujar(List<int>camino){
			Graphics gra2;
			Pen p= new Pen(Color.LawnGreen,10);
			//esto sirve para copiar el bitmap
			System.Drawing.Imaging.PixelFormat format = bmp.PixelFormat;
			RectangleF cloneRect = new RectangleF(0, 0, bmp.Width, bmp.Height);
			Brush b2 = new SolidBrush(Color.FromArgb(0,Color.White));
			 temp=new Bitmap(bmp.Clone(cloneRect,format));
			gra2 = Graphics.FromImage(temp);
			//pintar las lineas
			for (int i = 1; i < camino.Count; i++) {
				gra2.DrawLine(p,(float)Lista[camino[i-1]-1].getX(),(float)Lista[camino[i-1]-1].getY(),(float)Lista[camino[i]-1].getX(),(float)Lista[camino[i]-1].getY());
			}
			pictureBox1.Image=temp;
		}
		void MainFormLoad(object sender, EventArgs e)
		{
	
		}
	
	}
}
