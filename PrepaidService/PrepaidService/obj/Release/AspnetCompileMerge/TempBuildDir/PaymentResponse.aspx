<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentResponse.aspx.cs" Inherits="PrepaidService.PaymentResponse" %>


<%=Request["msg"] %>
<script type="text/javascript">
	function getMsg() {
	  var msg = '<%=Request["msg"] %>';
	  AndroidFunction.gotMsg(msg);
	}
    getMsg();
</script>
