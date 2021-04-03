<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="margin-top:0;margin-left:0;margin-right:0;margin-bottom:0">
    <form id="form1" runat="server">
    <asp:Table runat="server" ID="table_idxx" GridLines="Both"  BorderWidth="0" Width="95%" CellPadding=0 CellSpacing=0>            
            <asp:TableRow>
                <asp:TableCell>                                        
                    <asp:Panel ID="paneldanhsach" GroupingText=""
                        runat="server" Font-Names="Times New Roman" Font-Bold=false 
                        Font-Size="12px"  Width="100%" ForeColor="blue">
                        <asp:Table runat="server" ID="Table_TD" ForeColor="White" Font-Bold=true Width="100%" BackColor="RoyalBlue">
                            <asp:TableRow>
                                <asp:TableCell>  
                                    Đơn vị:                                    
                                    <asp:DropDownList ID="ds_donvi" runat="server" 
                                        Font-Names="Tahoma"  
                                        Font-Size="12px" ForeColor="blue"
                                        AppendDataBoundItems="True" >                                    
                                    </asp:DropDownList>      
                                    Tháng BĐ:                          
                                    <asp:DropDownList ID="kybd" runat="server" 
                                        Font-Names="Tahoma"  
                                        Font-Size="12px" ForeColor="blue"
                                        AppendDataBoundItems="false" >                                    
                                    </asp:DropDownList>
                                    Tháng KT:
                                    <asp:DropDownList ID="kykt" runat="server" 
                                        Font-Names="Tahoma"  
                                        Font-Size="12px" ForeColor="blue"
                                        AppendDataBoundItems="True" >                                    
                                    </asp:DropDownList>
                                    Loại BC:
                                    <asp:DropDownList ID="loaibc" runat="server" 
                                        Font-Names="Tahoma"  
                                        Font-Size="12px" ForeColor="blue"
                                        AppendDataBoundItems="True" >                                    
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="Btn_xem" runat="server" Text="Đồng ý" 
                                    Font-Names="Times New Roman" Font-Size="12px"
                                    Font-Bold="True" ForeColor="blue" /> 
                               <!---     
                                    <asp:Button ID="btn_excel" runat="server" Text="Excel" 
                                    Font-Names="Times New Roman" Font-Size="12px"
                                    Font-Bold="True" ForeColor="blue" />                        
                               --->                                       
                                </asp:TableCell>
                            </asp:TableRow>                
                        </asp:Table> 
                        <asp:Table runat="server" ID="danhsach" Font-Names="Times New Roman" Visible="false" width="100%" Font-Bold="false" style="border-collapse: collapse; border-color:Aqua" Border="1" CellPadding=1 CellSpacing=0>
                            <asp:TableRow HorizontalAlign="Center" BackColor="#b19fbb">
                                <asp:TableCell RowSpan="4">STT</asp:TableCell>
                                <asp:TableCell RowSpan="4">Đơn vị</asp:TableCell>
                                <asp:TableCell ColumnSpan="5">
                                    <asp:Label ID="kehoach" runat="server"
                                    Text="" Font-Size="12px" Font-Italic="true"
                                    ForeColor="blue" />  
                                </asp:TableCell>
                                <asp:TableCell ColumnSpan="11">
                                    <asp:Label ID="Thuchien" runat="server"
                                    Text="" Font-Size="12px" Font-Italic="true"
                                    ForeColor="blue" />  
                                </asp:TableCell>
                            </asp:TableRow>                
                            <asp:TableRow HorizontalAlign="Center" BackColor="#b19fbb">
                                <asp:TableCell RowSpan="2">Tổng cộng</asp:TableCell>
                                <asp:TableCell ColumnSpan="4">Trong đó</asp:TableCell>
                                <asp:TableCell RowSpan="2">Tổng cộng</asp:TableCell>
                                <asp:TableCell RowSpan="2">%TH/KH</asp:TableCell>
                                <asp:TableCell ColumnSpan="8">Trong đó</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow HorizontalAlign="Center" BackColor="#b19fbb">
                                <asp:TableCell>Ghi nợ</asp:TableCell>
                                <asp:TableCell>Di động TS</asp:TableCell>
                                <asp:TableCell>Sim thẻ VNP</asp:TableCell>
                                <asp:TableCell>KD TM</asp:TableCell>

                                <asp:TableCell>Ghi nợ</asp:TableCell>
                                <asp:TableCell>% Ghi nợ</asp:TableCell>
                                <asp:TableCell>Di động TS</asp:TableCell>
                                <asp:TableCell>% Di động</asp:TableCell>
                                <asp:TableCell>Sim thẻ VNP</asp:TableCell>
                                <asp:TableCell>% Sim thẻ VNP</asp:TableCell>
                                <asp:TableCell>KD TM</asp:TableCell>
                                <asp:TableCell>%KD TM</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow HorizontalAlign="Center" BackColor="#b19fbb">
                                <asp:TableCell>A</asp:TableCell>
                                <asp:TableCell>B</asp:TableCell>
                                <asp:TableCell>C</asp:TableCell>
                                <asp:TableCell>D</asp:TableCell>
                                <asp:TableCell>E</asp:TableCell>
                                <asp:TableCell>F</asp:TableCell>
                                <asp:TableCell>G</asp:TableCell>
                                <asp:TableCell>H</asp:TableCell>
                                <asp:TableCell>I</asp:TableCell>
                                <asp:TableCell>J</asp:TableCell>
                                <asp:TableCell>K</asp:TableCell>
                                <asp:TableCell>L</asp:TableCell>
                                <asp:TableCell>M</asp:TableCell>
                                <asp:TableCell>N</asp:TableCell>
                                <asp:TableCell>O</asp:TableCell>
                            </asp:TableRow>
                        </asp:Table> 
                    </asp:Panel>                                      
                </asp:TableCell>
            </asp:TableRow>
    </asp:Table>    
        <asp:HiddenField ID="capnhat" runat="server" Value="" />
        <asp:HiddenField ID="mstr_all" runat="server" Value="" />
        <asp:HiddenField ID="idks" runat="server" Value="" />        
        <asp:HiddenField ID="id_update" runat="server" Value="" />
       
    </form>
</body>
</html>
