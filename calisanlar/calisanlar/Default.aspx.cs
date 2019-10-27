/*
 * SharpDevelop tarafından düzenlendi.
 * Kullanıcı: Asus
 * Tarih: 22.10.2019
 * Zaman: 09:44
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
	/// Description of MainForm.
	/// </summary>
	public class Default : Page
	{
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Data

		protected	HtmlInputButton		_Button_Ok;
		protected	HtmlInputText 		adi;
		protected	HtmlInputText 		soyadi;
		protected	HtmlInputText		dogumTarihi;
		protected	HtmlTextArea 		adres;
		protected	DropDownList		meslek;
		protected	DropDownList		calisilanBirim;
		protected	HtmlInputButton		arama;
		protected	HtmlInputHidden		sicilNo;
		
		OleDbConnection baglan;
		OleDbCommand komut;
		OleDbDataReader rd;
		bool i=false;//sorgulama için gerekli bir değişken
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Page Init & Exit (Open/Close DB connections here...)

		protected void PageInit(object sender, EventArgs e)
		{
			baglan= new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=calisanlar.accdb;");
			baglan.Open();
		}
		//----------------------------------------------------------------------
		protected void PageExit(object sender, EventArgs e)
		{
		}

		#endregion
		#region guncellencek olanı seç
		public void güncellenecek(){
			if(!String.IsNullOrEmpty(Request.QueryString["guncelle"])){
				komut=new OleDbCommand("select * from personel where sicilNo="+Request.QueryString["guncelle"]+"");
				komut.Connection=baglan;
				rd=komut.ExecuteReader();
				while(rd.Read())
				{
					sicilNo.Value=rd["sicilNo"].ToString();
					adi.Value=rd["adi"].ToString();
					soyadi.Value=rd["soyadi"].ToString();
					meslek.SelectedValue=rd["meslekKodu"].ToString();
					calisilanBirim.SelectedValue=rd["birimKodu"].ToString();
					adres.Value=rd["adres"].ToString();
					dogumTarihi.Value=rd["dogumTarihi"].ToString();
				}
				_Button_Ok.Value="güncelle";
			}
			
		}
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region meslekDropDownListFlush
		public void meslekDropDownList(){
			komut=new OleDbCommand("select  * from meslekler");
			komut.Connection=baglan;
			rd=komut.ExecuteReader();
			while(rd.Read())
			{
				meslek.Items.Add(new ListItem{Text=rd["meslekAdi"].ToString(),Value=rd["meslekKodu"].ToString()});
			}
		}
		#endregion
		#region Page baslat
		public void baslat(){
			komut=new OleDbCommand("select sicilNo,adi,soyadi,(select meslekAdi from meslekler where meslekKodu=personel.meslekKodu) as meslek,(select birimAdi from calisilanBirim where birimKodu=personel.birimKodu) as birim,dogumTarihi,adres from personel");
			komut.Connection=baglan;
			rd=komut.ExecuteReader();
			Response.Write("<table><tr><th>Güncelle</th><th>Sil</th><th>Maaslar</th><th>ad</th><th>soyad</th><th>Meslek</th><th>Çalışılan Birim</th><th>Doğum Tarihi</th><th>Adres</th></tr>");
			while(rd.Read())
			{
				Response.Write("<tr><td><a href='?guncelle="+rd["sicilNo"]+"'>güncelle</a></td><td><a onclick='Sil("+rd["sicilNo"]+")'>sil</a></td><td><a href=maasVer.aspx?id="+rd["sicilNo"]+">maasları</a></td><td>"+rd["adi"]+"</td><td>"+rd["soyadi"]+"</td><td>"+rd["meslek"]+"</td><td>"+rd["birim"]+"</td><td>"+rd["dogumTarihi"]+"</td><td>"+rd["adres"].ToString().Substring(0,10)+"...</td></tr>");
			}
			Response.Write("</table>");
		}
		#endregion
		#region sil
		public void sil(int id){
			komut=new OleDbCommand("delete from personel where sicilNo="+id+"");
			komut.Connection=baglan;
			int l=komut.ExecuteNonQuery();
			if(l==1){
				Response.Write("<script>alert('silme işlemi başarılı!');</script>");
				Response.Redirect("default.aspx");
			}
		}
		#endregion
		#region calisilanBirimDropDownListFlush
		public void calisilanDropDownList(){
			komut=new OleDbCommand("select  * from calisilanBirim");
			komut.Connection=baglan;
			rd=komut.ExecuteReader();
			while(rd.Read())
			{
				calisilanBirim.Items.Add(new ListItem{Text=rd["birimAdi"].ToString(),Value=rd["birimKodu"].ToString()});
			}
		}
		#endregion
		#region Page Load
		private void Page_Load(object sender, EventArgs e)
		{
			meslekDropDownList();
			calisilanDropDownList();
			if(String.IsNullOrEmpty(sicilNo.Value))
			güncellenecek();
			if(!String.IsNullOrEmpty(Request.QueryString["sil"]))
				sil(Convert.ToInt32(Request.QueryString["sil"]));
			//------------------------------------------------------------------
			if(!IsPostBack)
			{			
				baslat();
			}
			//------------------------------------------------------------------
		}
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region çalışan ekleme işlemi

		//----------------------------------------------------------------------
		protected void Click_Button_Ok(object sender, EventArgs e)
		{
			
			string sorgu;
			if(adi.Value.ToString().Length==0){
				Response.Write("<script>alert('ad bilgisi boş olmamalı!');</script>");
			}else if(soyadi.Value.ToString().Length==0){
				Response.Write("<script>alert('soyad bilgisi boş olmamalıdır!');</script>");
			}else if(adres.Value.ToString().Length==0){
				Response.Write("<script>alert('Adres boş geçilemez!');</script>");
			}else if(dogumTarihi.Value.ToString().Length<8){
				Response.Write("<script>alert('Doğum tarihi 8 karakterden küçük olamaz!');</script>");
			}else{
				if(String.IsNullOrEmpty(sicilNo.Value))
					sorgu="insert into personel(adi,soyadi,dogumTarihi,adres,meslekKodu,birimKodu) values" +
						"('"+adi.Value.ToString()+"','"+soyadi.Value.ToString()+"','"+dogumTarihi.Value.ToString()+"','"
						+adres.Value.ToString()+"',"+meslek.SelectedValue+","+calisilanBirim.SelectedValue+")";
				else
					sorgu="update personel set adi='"+adi.Value+"',soyadi='"+soyadi.Value.ToString()+
						"',dogumTarihi='"+dogumTarihi.Value.ToString()+"',adres='"+adres.InnerText.ToString()+
						"',meslekKodu="+meslek.SelectedValue+",birimKodu="+calisilanBirim.SelectedValue+
						"  where sicilNo="+sicilNo.Value+"";
				komut=new OleDbCommand(sorgu);
				komut.Connection=baglan;
				int l=komut.ExecuteNonQuery();
				if(l==1){
					Response.Write("<script>alert('işlem başarılı!');</script>");
					
					_Button_Ok.Value="ekle";
				}
				baslat();
				
				
			}
			Response.Redirect("Default.aspx");
		//Page.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
		}

		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Ara

		//----------------------------------------------------------------------
		public string verMiAndMi(string t)
		{
			if(!i)t+=" where ";
			else t+=" and ";
			return t;
		}
		protected void ara(object sender, System.EventArgs e)
		{
			i=false;
			String s="select sicilNo,adi,soyadi,(select meslekAdi from meslekler where meslekKodu=personel.meslekKodu) as meslek,(select birimAdi from calisilanBirim where birimKodu=personel.birimKodu) as birim,dogumTarihi,adres from personel ";
			if(adi.Value.ToString().Length!=0){
				s=verMiAndMi(s);
				s+=" adi like '%"+adi.Value+"%' ";
				i=true;
			}
			if(soyadi.Value.ToString().Length!=0){
				s=verMiAndMi(s);
				s+="soyadi like '%"+soyadi.Value+"%' ";
				i=true;
			}
			if(adres.Value.ToString().Length!=0){
				s=verMiAndMi(s);
				s+="adres like '%"+adres.Value+"%' ";
				i=true;
			}
			komut=new OleDbCommand(s);
			komut.Connection=baglan;
			rd=komut.ExecuteReader();
			Response.Write("<table><tr><th>Güncelle</th><th>Sil</th><th>Maaslar</th><th>ad</th><th>soyad</th><th>Meslek</th><th>Çalışılan Birim</th><th>Doğum Tarihi</th><th>Adres</th></tr>");
			while(rd.Read())
			{
				Response.Write("<tr><td><a href='?guncelle="+rd["sicilNo"]+"'>güncelle</a></td><td><a onclick='Sil("+rd["sicilNo"]+")'>sil</a></td><td><a href=maasVer.aspx?id="+rd["sicilNo"]+">maasları</a></td><td>"+rd["adi"]+"</td><td>"+rd["soyadi"]+"</td><td>"+rd["meslek"]+"</td><td>"+rd["birim"]+"</td><td>"+rd["dogumTarihi"]+"</td><td>"+rd["adres"].ToString().Substring(0,10)+"...</td></tr>");
			}
			Response.Write("</table>");
		}

		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Initialize Component
		protected override void RenderChildren(HtmlTextWriter output)
		{
			foreach(Control c in base.Controls)
			{
				c.RenderControl(output);
			}
		}
		//----------------------------------------------------------------------
		protected override void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		//----------------------------------------------------------------------
		private void InitializeComponent()
		{
			this.Load	+= new System.EventHandler(Page_Load);
			this.Init   += new System.EventHandler(PageInit);
			this.Unload += new System.EventHandler(PageExit);
			
			_Button_Ok.ServerClick	 += new EventHandler(Click_Button_Ok);
			arama.ServerClick += new EventHandler(ara);
		}
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	}
}