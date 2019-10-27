<%@ Page
Language           = "C#"
AutoEventWireup    = "false"
Inherits           = "calisanlar.Default"
ValidateRequest    = "false"
EnableSessionState = "false"
%>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>calisanlar</title>

<meta http-equiv="content-type" content="text/html; charset=utf-8" />
<meta http-equiv="CACHE-CONTROL" content="NO-CACHE" />
<meta http-equiv="PRAGMA" content="NO-CACHE" />

<link href="calisanlar.css" type="text/css" rel="stylesheet" />
<script>
function Sil(id) {
var r = confirm(id+" silmek istediğinize emin misiniz?");
if (r == true) {
window.location.href = "http://localhost:8080/?sil="+id+"";
} 
}
</script>
</head>
<body>
<form id="Form1" method="post" runat="server">
<!-- Site Code goes here! -->
<table>

<tr>
<td colspan="2">
Çalışan Sorgulama ve Ekleme
</td>
</tr>
<input type="hidden" id="sicilNo" runat="server"/>
<tr>
<td>
ad:
</td>
<td>
<input id="adi" runat="server" placeholder="ad giriniz"/>
</td>
</tr>

<tr>
<td>
soyad:
</td>
<td>
<input id="soyadi" runat="server" placeholder="soyad giriniz"/>
</td>
</tr>
<tr>
<td>
doğum tarihi:
</td>
<td>
<input id="dogumTarihi"  runat="server" placeholder="gün/ay/yıl formatında"/>
</td>
</tr>
<tr>
<td>
meslek
</td>
<td>
<asp:DropDownList ID="meslek" runat="server" placeholder="meslek bilgisini seçiniz"/>
</td>
</tr>
<tr>
<td>
calisilacak birim
</td>
<td>
<asp:DropDownList ID="calisilanBirim" runat="server" placeholder="calisılacak birimi seçiniz"/>
</td>
</tr>
<tr>
<tr>
<td>
Adres
</td>
<td>
<textarea ID="adres" runat="server" rows="5" cols="55" placeholder="adresi giriniz"></textarea>
</td>
</tr>
<tr>
<td colspan="1">
<input id="_Button_Ok" type="submit" value="Ekle" runat="server" />
</td>



<td colspan="1">
<input id="arama" type="submit" value="ara" runat="server" />
</td>
</tr>

</table>

</form>
</body>
</html>
