/*
 * SharpDevelop tarafından düzenlendi.
 * Kullanıcı: Asus
 * Tarih: 23.10.2019
 * Zaman: 11:00
 * 
 * Bu şablonu değiştirmek için Araçlar | Seçenekler | Kodlama | Standart Başlıkları Düzenle 'yi kullanın.
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace calisanlar
{
	/// <summary>
	/// Description of maasVer
	/// </summary>
	public class maasVer : Page
	{
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Data

		protected	HtmlInputButton		ara;
		protected	HtmlInputButton		ekle;
		protected	HtmlInputText		maas;
		protected	HtmlInputText		maasAyi;
		protected	HtmlInputHidden		maasId;
		protected	HtmlInputHidden		sicilNo;
		OleDbConnection baglan;
		OleDbCommand komut;
		OleDbDataReader rd;
		bool i=false;
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Page Init & Exit (Open/Close DB connections here...)

		protected void PageInit(object sender, System.EventArgs e)
		{
			baglan= new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=calisanlar.accdb;");
			baglan.Open();
		}
		//----------------------------------------------------------------------
		protected void PageExit(object sender, System.EventArgs e)
		{
		}

		#endregion
		#region listeleme
		public void maaslistele(string id){
			komut=new OleDbCommand("select *from maaslar where sicilNo="+id+"");
			komut.Connection=baglan;
			rd=komut.ExecuteReader();
			Response.Write("<table><tr><th>Güncelle</th><th>Sil</th><th>maasAyi</th><th>maas</th></tr>");
			while(rd.Read())
			{
				Response.Write("<tr><td><a href='?guncelle="+rd["maasNo"]+"&id="+Request.QueryString["id"]+"'>güncelle</a></td><td><a onclick='Sil("+rd["maasNo"]+","+Request.QueryString["id"]+")'>sil</a></td><td>"+rd["maasAyi"]+"</td><td>"+rd["maas"]+"</td></tr>");
			}
			Response.Write("</table>");
		}
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region guncellencek olanı seç
		public void güncellenecek(){
			if(!String.IsNullOrEmpty(Request.QueryString["guncelle"])){
			komut=new OleDbCommand("select * from maaslar where maasNo="+Request.QueryString["guncelle"]+"");
			komut.Connection=baglan;
			rd=komut.ExecuteReader();
			while(rd.Read())
			{
				sicilNo.Value=rd["sicilNo"].ToString();
				maas.Value=rd["maas"].ToString();
				maasAyi.Value=rd["maasAyi"].ToString();
				maasId.Value=rd["maasNo"].ToString();
			}
			ekle.Value="güncelle";
			}
		}
		#endregion
		#region Page Load
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(String.IsNullOrEmpty(maasId.Value))
				güncellenecek();
			if(!String.IsNullOrEmpty(Request.QueryString["id"]))
			{
				sicilNo.Value=Request.QueryString["id"];
			}
			if( !String.IsNullOrEmpty(Request.QueryString["sil"]) && !String.IsNullOrEmpty(Request.QueryString["id"]))
				sil(Request.QueryString["sil"],Request.QueryString["id"]);
			//------------------------------------------------------------------
			if(!IsPostBack)
			{
				maaslistele(sicilNo.Value);
			}
			//------------------------------------------------------------------
		}
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region ara

		//----------------------------------------------------------------------

		protected void arama(object sender, System.EventArgs e)
		{
			i=false;
			String s="select *from maaslar where sicilNo="+Request.QueryString["id"]+" ";
			if(maasAyi.Value.ToString().Length!=0){
				s+=" and ";
				s+=" maasAyi="+maasAyi.Value+" ";
				i=true;
			}
			if(maas.Value.ToString().Length!=0){
				s+=" and ";
				s+="maas="+maas.Value+"";
				i=true;
			}
			komut=new OleDbCommand(s);
			komut.Connection=baglan;
			rd=komut.ExecuteReader();
			Response.Write("<table><tr><th>Güncelle</th><th>Sil</th><th>maasAyi</th><th>maas</th></tr>");
			while(rd.Read())
			{
				Response.Write("<tr><td><a href='?guncelle="+rd["maasNo"]+"&id="+Request.QueryString["id"]+"'>güncelle</a></td><td><a onclick='Sil("+rd["maasNo"]+","+Request.QueryString["id"]+")'>sil</a></td><td>"+rd["maasAyi"]+"</td><td>"+rd["maas"]+"</td></tr>");
			}
			Response.Write("</table>");
		}

		#endregion
		
		#region sil
		public void sil(string maasNo,string id){
			komut=new OleDbCommand("delete from maaslar where maasNo="+maasNo+"");
			komut.Connection=baglan;
			int l=komut.ExecuteNonQuery();
			if(l==1){
				Response.Write("<script>alert('silme işlemi başarılı!');</script>");
			}
			Response.AddHeader("REFRESH","1.1;maasVer.aspx?id="+sicilNo.Value);
		}
		#endregion
		
		#region ekleme

		//----------------------------------------------------------------------
		protected void ekleme(object sender, System.EventArgs e)
		{
			string sorgu;
			//komut=new OleDbCommand();
			if(maas.Value.ToString().Length==0){
				Response.Write("<script>alert('maas boş olmamalı!');</script>");
			}else if(maasAyi.Value.ToString().Length==0){
				Response.Write("<script>alert('maas ayi boş olmamalıdır!');</script>");
			}else{
				if(String.IsNullOrEmpty(maasId.Value))
					sorgu="insert into maaslar(sicilNo,maasAyi,maas) values" +
						"("+sicilNo.Value.ToString()+","+maasAyi.Value.ToString()+","+maas.Value.ToString()+")";
				else
					sorgu="update maaslar set maas="+maas.Value+",maasAyi="+maasAyi.Value+" where maasNo="+maasId.Value+"";
				komut=new OleDbCommand(sorgu);
				komut.Connection=baglan;
				int l=komut.ExecuteNonQuery();
				if(l==1){
					Response.Write("<script>alert('işlem başarılı!');</script>");
					
					ekle.Value="ekle";
				}
			}
			Response.AddHeader("REFRESH","1.1;maasVer.aspx?id="+sicilNo.Value);
		}

		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region

		//----------------------------------------------------------------------

		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Add more events here...

		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Initialize Component

		protected override void OnInit(EventArgs e)
		{	InitializeComponent();
			base.OnInit(e);
		}
		//----------------------------------------------------------------------
		private void InitializeComponent()
		{	//------------------------------------------------------------------
			this.Load	+= new System.EventHandler(Page_Load);
			this.Init   += new System.EventHandler(PageInit);
			this.Unload += new System.EventHandler(PageExit);
			//------------------------------------------------------------------
			ekle.ServerClick	 += new EventHandler(ekleme);
			ara.ServerClick += new EventHandler(arama);
			//------------------------------------------------------------------
		}
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	}
}
