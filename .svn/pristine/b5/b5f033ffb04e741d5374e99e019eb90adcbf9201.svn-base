Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Drawing
'Imports System.Data.OracleClient
Imports Oracle.DataAccess.Client
'Imports Oracle.Web.Management
Imports System


Partial Class _Default
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindData()
        End If

    End Sub

    Sub BindData()
        Dim mstr As String

        mstr = "select 0 donvi_id, '--  Tất cả  --' ten_dv from dual union all select donvi_id, ten_dv from tamth_agg.donvi"

        ds_donvi.DataSource = QueryView(mstr)
        ds_donvi.DataValueField = "donvi_id"
        ds_donvi.DataTextField = "ten_dv"
        ds_donvi.DataBind()


        mstr = "SELECT chukyno, chuky FROM tamth_agg.ky_cuoc"
        kybd.Controls.Clear()
        kybd.DataSource = QueryView(mstr)
        kybd.DataValueField = "chukyno"
        kybd.DataTextField = "chuky"
        kybd.DataBind()


        mstr = "SELECT chukyno, chuky FROM tamth_agg.ky_cuoc"
        kykt.DataSource = QueryView(mstr)
        kykt.DataValueField = "chukyno"
        kykt.DataTextField = "chuky"
        kykt.DataBind()

        mstr = "select 1 id, 'Thực hiện kế hoạch' ten from dual UNION  select 2 id, 'Giao kế hoạch SXKD' ten  from dual"
        loaibc.DataSource = QueryView(mstr)
        loaibc.DataValueField = "id"
        loaibc.DataTextField = "ten"
        loaibc.DataBind()

        'mstr = "select * from dm_nam order by nam_kh DESC"
        'ds_nam.DataSource = dbo.QueryView(mstr)
        'ds_nam.DataValueField = "nam_kh"
        'ds_nam.DataTextField = "nam_kh"
        'ds_nam.DataBind()
    End Sub


    Public cn_Oracle As String = "Data Source=PDBAGG; User Id=tamth_agg;Password=tam#8008200;Persist Security Info=True"
    'Public cn_Oracle As String = ConfigurationSettings.AppSettings("dhsxkd")
    Public conn As New OracleConnection(cn_Oracle)
    'Public conn As OracleConnection (cn_Oracle)

    Public Function QueryView(ByVal QueryString As String) As OracleDataReader

        Dim cn As New OracleConnection(cn_Oracle)
        cn.Open()

        Dim myReader As OracleDataReader

        Dim command As OracleCommand = New OracleCommand()
        Try
            command.CommandText = QueryString
            command.Connection = cn
            myReader = command.ExecuteReader()
        Catch ex As Exception
            Throw ex
        End Try
        Return myReader

    End Function


    Protected Sub Btn_xem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_xem.Click
        ds_baocao()
    End Sub

    Sub ds_baocao()
        Dim kehoach As OracleDataReader
        Dim i As Integer = 0
        Dim cmd As OracleCommand = New OracleCommand("tamth_agg.baocao_dt.bc_kehoach_thuchien")
        cmd.Connection = conn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.BindByName = True

        Dim mstr As String = ""

        If loaibc.SelectedValue = 1 And ds_donvi.SelectedValue = 0 Then
            cmd.Parameters.Add("vdonvi_id", OracleDbType.Int32).Value = 0
            cmd.Parameters.Add("vkybd", OracleDbType.Varchar2).Value = kybd.SelectedValue
            cmd.Parameters.Add("vkykt", OracleDbType.Varchar2).Value = kykt.SelectedValue
            cmd.Parameters.Add("vloaibc", OracleDbType.Int32).Value = 1
            cmd.Parameters.Add("returnds", OracleDbType.RefCursor).Direction = ParameterDirection.Output
            conn.Open()
            kehoach = cmd.ExecuteReader
            Me.danhsach.Controls.Clear()
            table_tieude_1(Me.danhsach)
            Me.danhsach.Visible = True

            Do While kehoach.Read                                
                table_noidung_1(kehoach.Item("TT").ToString.Trim, _
                       kehoach.Item("chitieu").ToString.Trim, _
                       kehoach.Item("dvt").ToString.Trim, _
                       kehoach.Item("tongcong").ToString, _
                      kehoach.Item("tongcong_kh").ToString, _
                       kehoach.Item("APU").ToString, _
                      kehoach.Item("APU_kh").ToString, _
                      kehoach.Item("CDC").ToString, _
                      kehoach.Item("CDC_kh").ToString, _
                      kehoach.Item("CPU").ToString, _
                      kehoach.Item("CPU_kh").ToString, _
                      kehoach.Item("CTH").ToString, _
                      kehoach.Item("CTH_kh").ToString, _
                      kehoach.Item("CMI").ToString, _
                      kehoach.Item("CMI_kh").ToString, _
                      kehoach.Item("lxn").ToString, _
                      kehoach.Item("lxn_kh").ToString, _
                      kehoach.Item("ptn").ToString, _
                      kehoach.Item("ptn_kh").ToString, _
                      kehoach.Item("tcu").ToString, _
                      kehoach.Item("tcu_kh").ToString, _
                      kehoach.Item("tsn").ToString, _
                      kehoach.Item("tsn_kh").ToString, _
                      kehoach.Item("tbn").ToString, _
                      kehoach.Item("tbn_kh").ToString, _
                      kehoach.Item("ttn").ToString, _
                      kehoach.Item("ttn_kh").ToString, _
                       (i Mod 2), Me.danhsach)
                i = i + 1
            Loop
            kehoach.Close()
            kehoach = Nothing

        ElseIf loaibc.SelectedValue = 1 And ds_donvi.SelectedValue <> 0 Then

            cmd.Parameters.Add("vdonvi_id", OracleDbType.Int32).Value = ds_donvi.SelectedValue
            cmd.Parameters.Add("vkybd", OracleDbType.Varchar2).Value = kybd.SelectedValue
            cmd.Parameters.Add("vkykt", OracleDbType.Varchar2).Value = kykt.SelectedValue
            cmd.Parameters.Add("vloaibc", OracleDbType.Int32).Value = 1
            cmd.Parameters.Add("returnds", OracleDbType.RefCursor).Direction = ParameterDirection.Output
            conn.Open()
            kehoach = cmd.ExecuteReader
            Me.danhsach.Controls.Clear()
            table_tieude_2(Me.danhsach)
            Me.danhsach.Visible = True
            Do While kehoach.Read
                table_noidung_2(kehoach.Item("TT").ToString.Trim, _
                       kehoach.Item("chitieu").ToString.Trim, _
                       kehoach.Item("dvt").ToString.Trim, _
                       kehoach.Item("kh").ToString, _
                      kehoach.Item("t_01").ToString, _
                       kehoach.Item("t_02").ToString, _
                      kehoach.Item("t_03").ToString, _
                      kehoach.Item("t_04").ToString, _
                      kehoach.Item("t_05").ToString, _
                      kehoach.Item("t_06").ToString, _
                      kehoach.Item("t_07").ToString, _
                      kehoach.Item("t_08").ToString, _
                      kehoach.Item("t_09").ToString, _
                      kehoach.Item("t_10").ToString, _
                      kehoach.Item("t_11").ToString, _
                      kehoach.Item("t_12").ToString, _
                      kehoach.Item("tongcong").ToString, _
                      kehoach.Item("kh_nam").ToString, _
                       (i Mod 2), Me.danhsach)
                i = i + 1
            Loop
            kehoach.Close()
            kehoach = Nothing
        End If

    End Sub

    Sub table_tieude_1(ByVal table_ As Table)

        Dim row_0 As New TableRow
        Dim cell0_1 As New TableCell
        Dim cell0_2 As New TableCell
        Dim cell0_3 As New TableCell
        Dim cell0_4 As New TableCell
        Dim cell0_5 As New TableCell
        Dim cell0_6 As New TableCell
        Dim cell0_7 As New TableCell
        Dim cell0_8 As New TableCell
        Dim cell0_9 As New TableCell
        Dim cell0_10 As New TableCell
        Dim cell0_11 As New TableCell
        Dim cell0_12 As New TableCell

        cell0_1.Text = "TỔNG CỘNG"
        cell0_1.Width = "100"
        cell0_1.ColumnSpan = 2

        cell0_2.Text = "APU"
        cell0_2.Width = "100"
        cell0_2.ColumnSpan = 2

        cell0_3.Text = "CDC"
        cell0_3.Width = "100"
        cell0_3.ColumnSpan = 2

        cell0_4.Text = "CPU"
        cell0_4.Width = "100"
        cell0_4.ColumnSpan = 2

        cell0_5.Text = "CTH"
        cell0_5.Width = "100"
        cell0_5.ColumnSpan = 2

        cell0_6.Text = "CMI"
        cell0_6.Width = "100"
        cell0_6.ColumnSpan = 2

        cell0_7.Text = "LXN"
        cell0_7.Width = "100"
        cell0_7.ColumnSpan = 2

        cell0_8.Text = "PTN"
        cell0_8.Width = "100"
        cell0_8.ColumnSpan = 2

        cell0_9.Text = "TCU"
        cell0_9.Width = "100"
        cell0_9.ColumnSpan = 2

        cell0_10.Text = "TSN"
        cell0_10.Width = "100"
        cell0_10.ColumnSpan = 2

        cell0_11.Text = "TBN"
        cell0_11.Width = "100"
        cell0_11.ColumnSpan = 2

        cell0_12.Text = "TTN"
        cell0_12.Width = "100"
        cell0_12.ColumnSpan = 2

        Dim row_1 As New TableRow
        Dim cell1_1 As New TableCell
        Dim cell2_1 As New TableCell
        Dim cell3_1 As New TableCell
        Dim cell4_1 As New TableCell
        Dim cell5_1 As New TableCell
        Dim cell6_1 As New TableCell
        Dim cell7_1 As New TableCell
        Dim cell8_1 As New TableCell
        Dim cell9_1 As New TableCell
        Dim cell10_1 As New TableCell
        Dim cell11_1 As New TableCell
        Dim cell12_1 As New TableCell
        Dim cell13_1 As New TableCell
        Dim cell14_1 As New TableCell
        Dim cell15_1 As New TableCell
        Dim cell16_1 As New TableCell
        Dim cell17_1 As New TableCell
        Dim cell18_1 As New TableCell
        Dim cell19_1 As New TableCell
        Dim cell20_1 As New TableCell
        Dim cell21_1 As New TableCell
        Dim cell22_1 As New TableCell
        Dim cell23_1 As New TableCell
        Dim cell24_1 As New TableCell
        Dim cell25_1 As New TableCell
        Dim cell26_1 As New TableCell
        Dim cell27_1 As New TableCell

        cell1_1.Text = "STT"
        cell1_1.Height = "20"
        cell1_1.Width = "40"
        cell1_1.RowSpan = 2


        cell2_1.Text = "YẾU TỐ"
        cell2_1.Width = "100"
        cell2_1.RowSpan = 2

        cell3_1.Text = "ĐVT"
        cell3_1.Width = "70"
        cell3_1.RowSpan = 2

        cell4_1.Text = "Lũy kế"
        cell4_1.Width = "100"

        cell5_1.Text = "% KH"
        cell5_1.Width = "100"

        cell6_1.Text = "Lũy kế"
        cell6_1.Width = "100"

        cell7_1.Text = "% KH"
        cell7_1.Width = "100"

        cell8_1.Text = "Lũy kế"
        cell8_1.Width = "100"

        cell9_1.Text = "% KH"
        cell9_1.Width = "100"

        cell10_1.Text = "Lũy kế"
        cell10_1.Width = "100"

        cell11_1.Text = "% KH"
        cell11_1.Width = "100"

        cell12_1.Text = "Lũy kế"
        cell12_1.Width = "100"

        cell13_1.Text = "% KH"
        cell13_1.Width = "100"

        cell14_1.Text = "Lũy kế"
        cell14_1.Width = "100"

        cell15_1.Text = "% KH"
        cell15_1.Width = "100"

        cell16_1.Text = "Lũy kế"
        cell16_1.Width = "100"

        cell17_1.Text = "% KH"
        cell17_1.Width = "100"

        cell18_1.Text = "Lũy kế"
        cell18_1.Width = "100"

        cell19_1.Text = "% KH"
        cell19_1.Width = "100"

        cell20_1.Text = "Lũy kế"
        cell20_1.Width = "100"

        cell21_1.Text = "% KH"
        cell21_1.Width = "100"

        cell22_1.Text = "Lũy kế"
        cell22_1.Width = "100"

        cell23_1.Text = "% KH"
        cell23_1.Width = "100"

        cell24_1.Text = "Lũy kế"
        cell24_1.Width = "100"

        cell25_1.Text = "% KH"
        cell25_1.Width = "100"

        cell26_1.Text = "Lũy kế"
        cell26_1.Width = "100"

        cell27_1.Text = "% KH"
        cell27_1.Width = "100"

        'cell8_1.Text = "Xóa"
        'cell8_1.Width = "30"

        row_0.Font.Bold = True
        row_0.ForeColor = Color.White
        row_0.BackColor = Color.RoyalBlue
        row_0.HorizontalAlign = HorizontalAlign.Center
        row_0.Font.Size = 10

        row_0.Controls.Add(cell1_1)
        row_0.Controls.Add(cell2_1)
        row_0.Controls.Add(cell3_1)

        row_0.Controls.Add(cell0_1)
        row_0.Controls.Add(cell0_2)
        row_0.Controls.Add(cell0_3)
        row_0.Controls.Add(cell0_4)
        row_0.Controls.Add(cell0_5)
        row_0.Controls.Add(cell0_6)
        row_0.Controls.Add(cell0_7)
        row_0.Controls.Add(cell0_8)
        row_0.Controls.Add(cell0_9)
        row_0.Controls.Add(cell0_10)
        row_0.Controls.Add(cell0_11)
        row_0.Controls.Add(cell0_12)

        row_1.Font.Bold = True
        row_1.ForeColor = Color.White
        row_1.BackColor = Color.RoyalBlue
        row_1.HorizontalAlign = HorizontalAlign.Center
        row_1.Font.Size = 10





        row_1.Controls.Add(cell4_1)
        row_1.Controls.Add(cell5_1)
        row_1.Controls.Add(cell6_1)
        row_1.Controls.Add(cell7_1)
        row_1.Controls.Add(cell8_1)
        row_1.Controls.Add(cell9_1)
        row_1.Controls.Add(cell10_1)
        row_1.Controls.Add(cell11_1)
        row_1.Controls.Add(cell12_1)
        row_1.Controls.Add(cell13_1)
        row_1.Controls.Add(cell14_1)
        row_1.Controls.Add(cell15_1)
        row_1.Controls.Add(cell16_1)
        row_1.Controls.Add(cell17_1)
        row_1.Controls.Add(cell18_1)
        row_1.Controls.Add(cell19_1)
        row_1.Controls.Add(cell20_1)
        row_1.Controls.Add(cell21_1)
        row_1.Controls.Add(cell22_1)
        row_1.Controls.Add(cell23_1)
        row_1.Controls.Add(cell24_1)
        row_1.Controls.Add(cell25_1)
        row_1.Controls.Add(cell26_1)
        row_1.Controls.Add(cell27_1)
        
        'row_1.Controls.Add(cell8_1)
        table_.Rows.Add(row_0)
        table_.Rows.Add(row_1)

    End Sub

    Sub table_noidung_1(ByVal st1 As String, _
                        ByVal st2 As String, _
                        ByVal st3 As String, _
                        ByVal st4 As String, _
                        ByVal st5 As String, _
                         ByVal apu As String, _
                        ByVal apu_kh As String, _
                        ByVal cdc As String, _
                        ByVal cdc_kh As String, _
                        ByVal cpu As String, _
                        ByVal cpu_kh As String, _
                        ByVal cth As String, _
                        ByVal cth_kh As String, _
                        ByVal cmi As String, _
                        ByVal cmi_kh As String, _
                         ByVal lxn As String, _
                        ByVal lxn_kh As String, _
                        ByVal ptn As String, _
                        ByVal ptn_kh As String, _
                        ByVal tcu As String, _
                        ByVal tcu_kh As String, _
                        ByVal tsn As String, _
                        ByVal tsn_kh As String, _
                        ByVal tbn As String, _
                        ByVal tbn_kh As String, _
                        ByVal ttn As String, _
                        ByVal ttn_kh As String, _
                        ByVal x_color As Integer, _
                        ByVal table_ As Table)

        Dim row_1 As New TableRow
        Dim cell1_1 As New TableCell
        Dim cell2_1 As New TableCell
        Dim cell3_1 As New TableCell
        Dim cell4_1 As New TableCell
        Dim cell5_1 As New TableCell
        Dim cell6_1 As New TableCell
        Dim cell7_1 As New TableCell
        Dim cell8_1 As New TableCell
        Dim cell9_1 As New TableCell
        Dim cell10_1 As New TableCell
        Dim cell11_1 As New TableCell
        Dim cell12_1 As New TableCell
        Dim cell13_1 As New TableCell
        Dim cell14_1 As New TableCell
        Dim cell15_1 As New TableCell
        Dim cell16_1 As New TableCell
        Dim cell17_1 As New TableCell
        Dim cell18_1 As New TableCell
        Dim cell19_1 As New TableCell
        Dim cell20_1 As New TableCell
        Dim cell21_1 As New TableCell
        Dim cell22_1 As New TableCell
        Dim cell23_1 As New TableCell
        Dim cell24_1 As New TableCell
        Dim cell25_1 As New TableCell
        Dim cell26_1 As New TableCell
        Dim cell27_1 As New TableCell

        cell1_1.Text = st1
        cell1_1.HorizontalAlign = HorizontalAlign.Right
        cell1_1.Height = "18"

        cell2_1.Text = "&nbsp;&nbsp;" & st2
        cell2_1.Attributes.Add("nowrap", "True")

        cell3_1.Text = st3
        cell3_1.Attributes.Add("nowrap", "True")
        cell3_1.HorizontalAlign = HorizontalAlign.Center

        cell4_1.Text = Format(Val(st4), "#,###,###,###,##0") & "&nbsp;"
        'cell4_1.Text = Val(st4)
        cell4_1.HorizontalAlign = HorizontalAlign.Right

        'cell5_1.Text = Format(st5, "#,###,###,###,##0") & "&nbsp;&nbsp;"
        ' Response.Write(st5)

        cell5_1.Text = Format(Val(st5), "#,###.#") & "%"
        cell5_1.Attributes.Add("nowrap", "True")
        cell5_1.HorizontalAlign = HorizontalAlign.Right

        cell6_1.Text = Format(Val(apu), "#,###,###,###,##0") & "&nbsp;"
        cell6_1.HorizontalAlign = HorizontalAlign.Right

        cell7_1.Text = Format(Val(apu_kh), "#,###.#") & "%"
        cell7_1.Attributes.Add("nowrap", "True")
        cell7_1.HorizontalAlign = HorizontalAlign.Right

        cell8_1.Text = Format(Val(cdc), "#,###,###,###,##0") & "&nbsp;"
        cell8_1.HorizontalAlign = HorizontalAlign.Right

        cell9_1.Text = Format(Val(cdc_kh), "#,###.#") & "%"
        cell9_1.Attributes.Add("nowrap", "True")
        cell9_1.HorizontalAlign = HorizontalAlign.Right

        cell10_1.Text = Format(Val(cpu), "#,###,###,###,##0") & "&nbsp;"
        cell10_1.HorizontalAlign = HorizontalAlign.Right

        cell11_1.Text = Format(Val(cpu_kh), "#,###.#") & "%"
        cell11_1.Attributes.Add("nowrap", "True")
        cell11_1.HorizontalAlign = HorizontalAlign.Right

        cell12_1.Text = Format(Val(cth), "#,###,###,###,##0") & "&nbsp;"
        cell12_1.HorizontalAlign = HorizontalAlign.Right

        cell13_1.Text = Format(Val(cth_kh), "#,###.#") & "%"
        cell13_1.Attributes.Add("nowrap", "True")
        cell13_1.HorizontalAlign = HorizontalAlign.Right

        cell14_1.Text = Format(Val(cmi), "#,###,###,###,##0") & "&nbsp;"
        cell14_1.HorizontalAlign = HorizontalAlign.Right

        cell15_1.Text = Format(Val(cmi_kh), "#,###.#") & "%"
        cell15_1.Attributes.Add("nowrap", "True")
        cell15_1.HorizontalAlign = HorizontalAlign.Right

        cell16_1.Text = Format(Val(lxn), "#,###,###,###,##0") & "&nbsp;"
        cell16_1.HorizontalAlign = HorizontalAlign.Right

        cell17_1.Text = Format(Val(lxn_kh), "#,###.#") & "%"
        cell17_1.Attributes.Add("nowrap", "True")
        cell17_1.HorizontalAlign = HorizontalAlign.Right

        cell18_1.Text = Format(Val(ptn), "#,###,###,###,##0") & "&nbsp;"
        cell18_1.HorizontalAlign = HorizontalAlign.Right

        cell19_1.Text = Format(Val(ptn_kh), "#,###.#") & "%"
        cell19_1.Attributes.Add("nowrap", "True")
        cell19_1.HorizontalAlign = HorizontalAlign.Right

        cell20_1.Text = Format(Val(tcu), "#,###,###,###,##0") & "&nbsp;"
        cell20_1.HorizontalAlign = HorizontalAlign.Right

        cell21_1.Text = Format(Val(tcu_kh), "#,###.#") & "%"
        cell21_1.Attributes.Add("nowrap", "True")
        cell21_1.HorizontalAlign = HorizontalAlign.Right

        cell22_1.Text = Format(Val(tsn), "#,###,###,###,##0") & "&nbsp;"
        cell22_1.HorizontalAlign = HorizontalAlign.Right

        cell23_1.Text = Format(Val(tsn_kh), "#,###.#") & "%"
        cell23_1.Attributes.Add("nowrap", "True")
        cell23_1.HorizontalAlign = HorizontalAlign.Right

        cell24_1.Text = Format(Val(tbn), "#,###,###,###,##0") & "&nbsp;"
        cell24_1.HorizontalAlign = HorizontalAlign.Right

        cell25_1.Text = Format(Val(tbn_kh), "#,###.#") & "%"
        cell25_1.Attributes.Add("nowrap", "True")
        cell25_1.HorizontalAlign = HorizontalAlign.Right

        cell26_1.Text = Format(Val(ttn), "#,###,###,###,##0") & "&nbsp;"
        cell26_1.HorizontalAlign = HorizontalAlign.Right

        cell27_1.Text = Format(Val(ttn_kh), "#,###.#") & "%"
        cell27_1.Attributes.Add("nowrap", "True")
        cell27_1.HorizontalAlign = HorizontalAlign.Right

        row_1.Font.Bold = False
        'If x_color = 0 Then
        'row_1.BackColor = Color.Aqua
        'Else
        'row_1.BackColor = Color.AliceBlue
        'End If


        If InStr("ABC", Trim(st1)) > 0 And Trim(st1) <> "" Then
            row_1.ForeColor = Color.Red
            row_1.BackColor = Color.YellowGreen
            row_1.Font.Bold = True
            row_1.Font.Size = 11
            'Response.Write(Trim(st1))
        ElseIf Len(Trim(st1)) = 1 Then
            row_1.ForeColor = Color.Navy
            row_1.BackColor = Color.Yellow
            row_1.Font.Bold = True
        End If


        row_1.Controls.Add(cell1_1)
        row_1.Controls.Add(cell2_1)
        row_1.Controls.Add(cell3_1)
        row_1.Controls.Add(cell4_1)
        row_1.Controls.Add(cell5_1)
        row_1.Controls.Add(cell6_1)
        row_1.Controls.Add(cell7_1)
        row_1.Controls.Add(cell8_1)
        row_1.Controls.Add(cell9_1)
        row_1.Controls.Add(cell10_1)
        row_1.Controls.Add(cell11_1)
        row_1.Controls.Add(cell12_1)
        row_1.Controls.Add(cell13_1)
        row_1.Controls.Add(cell14_1)
        row_1.Controls.Add(cell15_1)
        row_1.Controls.Add(cell16_1)
        row_1.Controls.Add(cell17_1)
        row_1.Controls.Add(cell18_1)
        row_1.Controls.Add(cell19_1)
        row_1.Controls.Add(cell20_1)
        row_1.Controls.Add(cell21_1)
        row_1.Controls.Add(cell22_1)
        row_1.Controls.Add(cell23_1)
        row_1.Controls.Add(cell24_1)
        row_1.Controls.Add(cell25_1)
        row_1.Controls.Add(cell26_1)
        row_1.Controls.Add(cell27_1)
        table_.Rows.Add(row_1)
    End Sub

    Sub table_tieude_2(ByVal table_ As Table)

        Dim row_1 As New TableRow
        Dim cell1_1 As New TableCell
        Dim cell2_1 As New TableCell
        Dim cell3_1 As New TableCell
        Dim cell4_1 As New TableCell
        Dim cell5_1 As New TableCell
        Dim cell6_1 As New TableCell
        Dim cell7_1 As New TableCell
        Dim cell8_1 As New TableCell
        Dim cell9_1 As New TableCell
        Dim cell10_1 As New TableCell
        Dim cell11_1 As New TableCell
        Dim cell12_1 As New TableCell
        Dim cell13_1 As New TableCell
        Dim cell14_1 As New TableCell
        Dim cell15_1 As New TableCell
        Dim cell16_1 As New TableCell
        Dim cell17_1 As New TableCell
        Dim cell18_1 As New TableCell
        Dim cell19_1 As New TableCell
        Dim cell20_1 As New TableCell
        Dim cell21_1 As New TableCell
        Dim cell22_1 As New TableCell
        Dim cell23_1 As New TableCell
        Dim cell24_1 As New TableCell
        Dim cell25_1 As New TableCell
        Dim cell26_1 As New TableCell
        Dim cell27_1 As New TableCell

        cell1_1.Text = "STT"
        cell1_1.Height = "20"
        cell1_1.Width = "40"

        cell2_1.Text = "YẾU TỐ"
        cell2_1.Width = "100"


        cell3_1.Text = "ĐVT"
        cell3_1.Width = "70"


        cell4_1.Text = "Chi tiêu"
        cell4_1.Width = "100"

        cell5_1.Text = "tháng 1"
        cell5_1.Width = "100"

        cell6_1.Text = "tháng 2"
        cell6_1.Width = "100"

        cell7_1.Text = "tháng 3"
        cell7_1.Width = "100"

        cell8_1.Text = "tháng 4"
        cell8_1.Width = "100"

        cell9_1.Text = "tháng 5"
        cell9_1.Width = "100"

        cell10_1.Text = "tháng 6"
        cell10_1.Width = "100"

        cell11_1.Text = "tháng 7"
        cell11_1.Width = "100"

        cell12_1.Text = "tháng 8"
        cell12_1.Width = "100"

        cell13_1.Text = "tháng 9"
        cell13_1.Width = "100"

        cell14_1.Text = "tháng 10"
        cell14_1.Width = "100"

        cell15_1.Text = "tháng 11"
        cell15_1.Width = "100"

        cell16_1.Text = "tháng 12"
        cell16_1.Width = "100"

        cell17_1.Text = "Lũy kế"
        cell17_1.Width = "100"

        cell18_1.Text = "% Năm"
        cell18_1.Width = "100"

        row_1.Font.Bold = True
        row_1.ForeColor = Color.White
        row_1.BackColor = Color.RoyalBlue
        row_1.HorizontalAlign = HorizontalAlign.Center
        row_1.Font.Size = 10




        row_1.Controls.Add(cell1_1)
        row_1.Controls.Add(cell2_1)
        row_1.Controls.Add(cell3_1)
        row_1.Controls.Add(cell4_1)
        row_1.Controls.Add(cell5_1)
        row_1.Controls.Add(cell6_1)
        row_1.Controls.Add(cell7_1)
        row_1.Controls.Add(cell8_1)
        row_1.Controls.Add(cell9_1)
        row_1.Controls.Add(cell10_1)
        row_1.Controls.Add(cell11_1)
        row_1.Controls.Add(cell12_1)
        row_1.Controls.Add(cell13_1)
        row_1.Controls.Add(cell14_1)
        row_1.Controls.Add(cell15_1)
        row_1.Controls.Add(cell16_1)
        row_1.Controls.Add(cell17_1)
        row_1.Controls.Add(cell18_1)

        table_.Rows.Add(row_1)

    End Sub

    Sub table_noidung_2(ByVal st1 As String, _
                        ByVal st2 As String, _
                        ByVal st3 As String, _
                        ByVal kh As String, _
                        ByVal t_01 As String, _
                        ByVal t_02 As String, _
                        ByVal t_03 As String, _
                        ByVal t_04 As String, _
                        ByVal t_05 As String, _
                        ByVal t_06 As String, _
                        ByVal t_07 As String, _
                        ByVal t_08 As String, _
                        ByVal t_09 As String, _
                        ByVal t_10 As String, _
                        ByVal t_11 As String, _
                        ByVal t_12 As String, _
                        ByVal tongcong As String, _
                        ByVal kh_nam As String, _                        
                        ByVal x_color As Integer, _
                        ByVal table_ As Table)

        Dim row_1 As New TableRow
        Dim cell1_1 As New TableCell
        Dim cell2_1 As New TableCell
        Dim cell3_1 As New TableCell
        Dim cell4_1 As New TableCell
        Dim cell5_1 As New TableCell
        Dim cell6_1 As New TableCell
        Dim cell7_1 As New TableCell
        Dim cell8_1 As New TableCell
        Dim cell9_1 As New TableCell
        Dim cell10_1 As New TableCell
        Dim cell11_1 As New TableCell
        Dim cell12_1 As New TableCell
        Dim cell13_1 As New TableCell
        Dim cell14_1 As New TableCell
        Dim cell15_1 As New TableCell
        Dim cell16_1 As New TableCell
        Dim cell17_1 As New TableCell
        Dim cell18_1 As New TableCell
        Dim cell19_1 As New TableCell
        Dim cell20_1 As New TableCell
        Dim cell21_1 As New TableCell
        Dim cell22_1 As New TableCell
        Dim cell23_1 As New TableCell
        Dim cell24_1 As New TableCell
        Dim cell25_1 As New TableCell
        Dim cell26_1 As New TableCell
        Dim cell27_1 As New TableCell

        cell1_1.Text = st1
        cell1_1.HorizontalAlign = HorizontalAlign.Right
        cell1_1.Height = "18"

        cell2_1.Text = "&nbsp;&nbsp;" & st2
        cell2_1.Attributes.Add("nowrap", "True")

        cell3_1.Text = st3
        cell3_1.Attributes.Add("nowrap", "True")
        cell3_1.HorizontalAlign = HorizontalAlign.Center

        cell4_1.Text = Format(Val(kh), "#,###,###,###,##0") & "&nbsp;"
        'cell4_1.Text = Val(st4)
        cell4_1.HorizontalAlign = HorizontalAlign.Right

        cell5_1.Text = Format(Val(t_01), "#,###,###,###,##0") & "&nbsp;"
        cell5_1.HorizontalAlign = HorizontalAlign.Right

        cell6_1.Text = Format(Val(t_02), "#,###,###,###,##0") & "&nbsp;"
        cell6_1.HorizontalAlign = HorizontalAlign.Right

        cell7_1.Text = Format(Val(t_03), "#,###,###,###,##0") & "&nbsp;"
        cell7_1.HorizontalAlign = HorizontalAlign.Right

        cell8_1.Text = Format(Val(t_04), "#,###,###,###,##0") & "&nbsp;"
        cell8_1.HorizontalAlign = HorizontalAlign.Right

        cell9_1.Text = Format(Val(t_05), "#,###,###,###,##0") & "&nbsp;"
        cell9_1.HorizontalAlign = HorizontalAlign.Right

        cell10_1.Text = Format(Val(t_06), "#,###,###,###,##0") & "&nbsp;"
        cell10_1.HorizontalAlign = HorizontalAlign.Right

        cell11_1.Text = Format(Val(t_07), "#,###,###,###,##0") & "&nbsp;"
        cell11_1.HorizontalAlign = HorizontalAlign.Right

        cell12_1.Text = Format(Val(t_08), "#,###,###,###,##0") & "&nbsp;"
        cell12_1.HorizontalAlign = HorizontalAlign.Right

        cell13_1.Text = Format(Val(t_09), "#,###,###,###,##0") & "&nbsp;"
        cell13_1.HorizontalAlign = HorizontalAlign.Right

        cell14_1.Text = Format(Val(t_10), "#,###,###,###,##0") & "&nbsp;"
        cell14_1.HorizontalAlign = HorizontalAlign.Right

        cell15_1.Text = Format(Val(t_11), "#,###,###,###,##0") & "&nbsp;"
        cell15_1.HorizontalAlign = HorizontalAlign.Right

        cell16_1.Text = Format(Val(t_12), "#,###,###,###,##0") & "&nbsp;"
        cell16_1.HorizontalAlign = HorizontalAlign.Right

        cell17_1.Text = Format(Val(tongcong), "#,###,###,###,##0") & "&nbsp;"
        cell17_1.HorizontalAlign = HorizontalAlign.Right

        cell18_1.Text = kh_nam & "%"
        cell18_1.Attributes.Add("nowrap", "True")
        cell18_1.HorizontalAlign = HorizontalAlign.Right

        'row_1.Font.Bold = False
        'If x_color = 0 Then
        'row_1.BackColor = Color.White
        'Else
        'row_1.BackColor = Color.AliceBlue
        'End If

        If InStr("ABC", Trim(st1)) > 0 And Trim(st1) <> "" Then
            row_1.ForeColor = Color.Red
            row_1.BackColor = Color.YellowGreen
            row_1.Font.Bold = True
            row_1.Font.Size = 11
            'Response.Write(Trim(st1))
        ElseIf Len(Trim(st1)) = 1 Then
            row_1.ForeColor = Color.Navy
            row_1.BackColor = Color.Yellow
            row_1.Font.Bold = True
        End If

        row_1.Controls.Add(cell1_1)
        row_1.Controls.Add(cell2_1)
        row_1.Controls.Add(cell3_1)
        row_1.Controls.Add(cell4_1)
        row_1.Controls.Add(cell5_1)
        row_1.Controls.Add(cell6_1)
        row_1.Controls.Add(cell7_1)
        row_1.Controls.Add(cell8_1)
        row_1.Controls.Add(cell9_1)
        row_1.Controls.Add(cell10_1)
        row_1.Controls.Add(cell11_1)
        row_1.Controls.Add(cell12_1)
        row_1.Controls.Add(cell13_1)
        row_1.Controls.Add(cell14_1)
        row_1.Controls.Add(cell15_1)
        row_1.Controls.Add(cell16_1)
        row_1.Controls.Add(cell17_1)
        row_1.Controls.Add(cell18_1)
        table_.Rows.Add(row_1)
    End Sub

    Private Function round(ByVal p1 As Object, ByVal p2 As Integer) As String
        Throw New NotImplementedException
    End Function

    Private Function IIf(ByVal p1 As Boolean, ByVal p2 As String) As String
        Throw New NotImplementedException
    End Function

    Private Function IIf(ByVal p1 As Boolean, ByVal p2 As String, ByVal p3 As String) As String
        Throw New NotImplementedException
    End Function

End Class
