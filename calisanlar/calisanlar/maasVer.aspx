<%@ Page
	Language           = "C#"
	AutoEventWireup    = "false"
	Inherits           = "calisanlar.maasVer"
	ValidateRequest    = "false"
	EnableSessionState = "false"
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Maas Verme Sayfası</title>

		<meta http-equiv="content-type" content="text/html; charset=utf-8" />
		<meta http-equiv="CACHE-CONTROL" content="NO-CACHE" />
		<meta http-equiv="PRAGMA" content="NO-CACHE" />

		<link href="calisanlar.css" type="text/css" rel="stylesheet" />
		<script>
function Sil(maas,id) {
var r = confirm(maas+" silmek istediğinize emin misiniz?");
if (r == true) {
window.location.href = "http://localhost:8080/maasVer.aspx?sil="+maas+"&id="+id+"";
} 
}
</script>
	</head>
	<body>
		<form id="Form_maasVer" method="post" runat="server">

			<table>
					<input type="hidden" id="maasId" runat="server" />
					<input type="hidden" id="sicilNo"	runat="server"/>
				<tr>
					<td colspan="2">
						Maasları
					</td>
				</tr>

				<tr>
				<td>
						Maas ayı <input id="maasAyi" runat="server" />
					</td>
					</tr>
					<tr>
					<td>
					Maas <input id="maas" runat="server" />
					</td>
				</tr>

				<tr>
					<td colspan="2">
						<input id="ekle" type="submit" value="Ekle" runat="server" />
						<input id="ara" type="submit" value="Ara" runat="server" />
					</td>
				</tr>

	

			</table>

		</form>
	</body>
</html>
