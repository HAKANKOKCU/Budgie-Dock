﻿Imports System.Windows.Threading
Imports System.Runtime.InteropServices

Class MainWindow
    Public OptionsIcon As iconobj
    Public WithEvents ticker As DispatcherTimer
    Public WithEvents refer As DispatcherTimer
    Public iconlist As New ArrayList
    Dim agwid = 0
    Public ruwid = 0
    Dim iddd As Integer = 0
    Public aaps As New ArrayList
    Dim isdockhovered As Boolean = False
    Public rid As Integer = 0
    Dim ismw As Boolean = False
    Public disallowedpnames As New ArrayList
    'This array will be "injected" to disallowedpnames in "Loaded" Function.
    Public defaultdisallowed() As String = {"textinputhost", "textinputhost.exe", "dwm", "dwm.exe", "csrss.exe", "csrss", Process.GetCurrentProcess.ProcessName.ToLower}

    Private Declare Auto Function IsIconic Lib "user32.dll" (ByVal hwnd As IntPtr) As Boolean
    <DllImport("user32.dll")> _
    Private Shared Function GetWindowRect(ByVal hWnd As HandleRef, ByRef lpRect As Rect) As Boolean
    End Function
    Dim icc As New ArrayList
    Dim icopack As Ini = Nothing
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        ticker = New DispatcherTimer
        ticker.Interval = TimeSpan.FromMilliseconds(1)
        ticker.Start()
        refer = New DispatcherTimer
        refer.Interval = TimeSpan.FromMilliseconds(1000)
        refer.Start()
        For Each dup As String In defaultdisallowed
            disallowedpnames.Add(dup)
        Next
        My.Computer.FileSystem.CreateDirectory(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\")
        My.Computer.FileSystem.CreateDirectory(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons")
        If Not My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data") Then
            Dim dd As String = ""
            My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data", dd, False)
        End If
        If Not My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data") Then
            Dim dd As String = ""
            My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data", dd, False)
        End If
        For Each dup As String In My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data").Split("|")
            disallowedpnames.Add(dup)
        Next
        appsgrid.Width = My.Settings.Size
        reicon()
        Dim dlbtn As New ButtonStack
        dlbtn.theStackPanel = DeleteIconButton
        Dim opbtn As New ButtonStack
        opbtn.theStackPanel = OptIconButton
        Dim optbtn As New ButtonStack
        optbtn.theStackPanel = OptMainButton
        Dim listbtn As New ButtonStack
        listbtn.theStackPanel = ListingButton
        Dim asbtn As New ButtonStack
        asbtn.theStackPanel = AddspButton
        If My.Settings.pos = "Bottom" Then
            mas.Orientation = Orientation.Vertical
            appsgrid.Orientation = Orientation.Horizontal
            appsgrid.Height = My.Settings.Size
        Else
            mas.Orientation = Orientation.Horizontal
            appsgrid.Orientation = Orientation.Vertical
            appsgrid.Width = My.Settings.Size
        End If
        Me.Width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width
        Me.Left = 0
        If My.Settings.animatescale = 0 Then
            My.Settings.animatescale = 1
        End If
        Me.Height = appsgrid.Height + 163
        bdr.Background = New SolidColorBrush(Color.FromArgb((My.Settings.dockopacity / 100) * 255, My.Settings.dockRed, My.Settings.dockGreen, My.Settings.dockBlue))
        If My.Settings.ApplyDockColorAtIsAppRuning Then
            ff.Background = bdr.Background
        End If
        Try
            If My.Settings.dockcr.Contains(",") Then
                Dim crlist = My.Settings.dockcr.Split(",")
                bdr.CornerRadius = New CornerRadius(crlist(0), crlist(1), crlist(2), crlist(3))
            Else
                bdr.CornerRadius = New CornerRadius(My.Settings.dockcr)
            End If
        Catch ex As Exception
            MsgBox("Failled to set corner radius: " + ex.Message)
        End Try
        If My.Settings.pos = "Bottom" Then
            Me.Top = Forms.Screen.PrimaryScreen.WorkingArea.Height - Me.Height + Forms.Screen.PrimaryScreen.WorkingArea.Top + IIf(My.Settings.autoHide, My.Settings.Size - 2, 0) + My.Settings.paddingTop
        Else
            Me.Left = Forms.Screen.PrimaryScreen.WorkingArea.Width - Me.Width + 180
        End If
        Me.Topmost = My.Settings.topMost
        If My.Settings.UseDockAsTaskbar Then
            bdr.Width = My.Computer.Screen.WorkingArea.Width
            ff.Width = My.Computer.Screen.WorkingArea.Width
            If My.Settings.pos = "Bottom" Then
                Me.Top = Forms.Screen.PrimaryScreen.Bounds.Height - Me.Height + Forms.Screen.PrimaryScreen.Bounds.Top + IIf(My.Settings.autoHide And Not isdockhovered, My.Settings.Size - 2, 0) + My.Settings.paddingTop
            Else
                Me.Top = Forms.Screen.PrimaryScreen.Bounds.Top - IIf(My.Settings.autoHide And Not isdockhovered, My.Settings.Size - 2, 0) + My.Settings.paddingTop
            End If
        Else
            If My.Settings.pos = "Bottom" Then
                Me.Top = Forms.Screen.PrimaryScreen.WorkingArea.Height - Me.Height + Forms.Screen.PrimaryScreen.WorkingArea.Top + IIf(My.Settings.autoHide And Not isdockhovered, My.Settings.Size - 2, 0) + My.Settings.paddingTop
            Else
                Me.Top = Forms.Screen.PrimaryScreen.WorkingArea.Top - IIf(My.Settings.autoHide And Not isdockhovered, My.Settings.Size - 2, 0) + My.Settings.paddingTop
            End If
        End If
        RedesignLayout()
    End Sub

    Sub RedesignLayout()
        If My.Settings.pos = "Bottom" Then
            mas.Children.Remove(ncan)
            mas.Children.Remove(bdr)
            mas.Children.Remove(ff)
            mas.Children.Add(ncan)
            mas.Children.Add(bdr)
            mas.Children.Add(ff)
            Canvas.SetTop(appname, 130)
            Canvas.SetTop(menustack, 0)
        ElseIf My.Settings.pos = "Top" Then
            mas.Children.Remove(ncan)
            mas.Children.Remove(bdr)
            mas.Children.Remove(ff)
            mas.Children.Add(ff)
            mas.Children.Add(bdr)
            mas.Children.Add(ncan)
            Canvas.SetTop(appname, 0)
            Canvas.SetTop(menustack, 30)
        End If
        mas.Orientation = Orientation.Vertical
        appsgrid.Orientation = Orientation.Horizontal
        ff.UpdateLayout()
        ncan.UpdateLayout()
        ff.UpdateLayout()
    End Sub

    Sub LoadIconPack()
        If My.Computer.FileSystem.FileExists(My.Settings.CurrentIconThemePath) Then
            icopack = New Ini(My.Settings.CurrentIconThemePath)
        Else
            icopack = Nothing
        End If
    End Sub

    Sub reicon(Optional ByVal animate As Boolean = True)
        LoadIconPack()
        icc.Clear()
        rid += 1
        iddd = 0
        ruwid = 0
        agwid = 0
        appsgrid.Children.Clear()
        isappopen.Children.Clear()
        iconlist.Clear()
        runingapps.Children.Clear()
        aaps.Clear()
        disallowedpnames.Clear()
        'Dim wid As New RWidget("C:\Users\ASUS PC\Desktop\testextension.bdockwidget", appsgrid)
        For Each dup As String In defaultdisallowed
            disallowedpnames.Add(dup)
        Next
        Try
            For Each dup As String In My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data").Split("|")
                disallowedpnames.Add(dup)
            Next
        Catch
        End Try
        If My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data").Contains("*") Then
            For Each Iconn As String In My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data").Split("|")
                If Iconn = "sep" Then
                    Dim a As New Grid
                    a.UseLayoutRounding = True
                    If Not My.Settings.animatescale = 1 Then a.Height = 5
                    Try
                        If My.Settings.animatescale = 1 Then a.Height = My.Settings.Size - 5
                    Catch
                    End Try
                    a.Background = New SolidColorBrush(Color.FromRgb(My.Settings.separatorRed, My.Settings.SeparatorGreen, My.Settings.SeparatorBlue))
                    a.Width = 1
                    a.ClipToBounds = True
                    a.Margin = New Thickness(1, 0, 1, 0)
                    Dim sr As New Sepremover
                    sr.id = iddd
                    sr.mainwin = Me
                    sr.sepgrid = a
                    appsgrid.Children.Add(a)
                    iconlist.Add("sep")
                    iddd += 1
                    a.ClipToBounds = True
                    Dim pp As New Grid
                    pp.Width = 3
                    pp.HorizontalAlignment = Windows.HorizontalAlignment.Center
                    isappopen.Children.Add(pp)
                Else
                    Dim a As New iconobj
                    a.apppath = Iconn.Split("*")(0).Replace("{BD-STAR-}", "*").Replace("{BD-FLINE-}", "|").Split("^")(0)
                    a.iconpath = Iconn.Split("*")(1)
                    a.appname = Iconn.Split("*")(2).Replace("{BD-STAR-}", "*").Replace("{BD-FLINE-}", "|")
                    a.stackpanel = appsgrid
                    a.containerwin = Me
                    a.runid = rid
                    Try
                        a.apparams = Iconn.Split("*")(0).Replace("{BD-STAR-}", "*").Replace("{BD-FLINE-}", "|").Split("^")(1).Replace("{BD-UPL-}", "^")
                    Catch
                    End Try
                    a.endinit()
                    a.imageiconobj.ClipToBounds = True
                    Try
                        a.imageiconobj.Height = My.Settings.Size - 5
                    Catch
                    End Try
                    If animate Then
                        If Not My.Settings.animatescale = 1 Then a.imageiconobj.Height = 5
                        Try
                            If My.Settings.animatescale = 1 Then a.imageiconobj.Height = My.Settings.Size - 5
                        Catch
                        End Try
                    End If
                    a.idd = iddd
                    iconlist.Add({Iconn.Split("*")(0), Iconn.Split("*")(1), Iconn.Split("*")(2)})
                    iddd += 1
                    icc.Add(My.Computer.FileSystem.GetName(Iconn.Split("*")(0).Replace("{BD-STAR-}", "*").Replace("{BD-FLINE-}", "|").Split("^")(0)).ToLower)
                End If
            Next
            Dim sizee As Integer = 0
            For Each a As UIElement In appsgrid.Children
                If TypeOf a Is Image Then
                    Try
                        sizee += My.Settings.Size
                    Catch
                    End Try
                Else
                    sizee += 3
                End If
            Next
            agwid = sizee
            agwid = sizee
        Else
            Dim lbldrg As New Label
            lbldrg.Content = "Drag Here To Add Icons"
            appsgrid.Children.Add(lbldrg)
            agwid = 138
        End If
        refopenapps(IIf(My.Settings.animatescale = 1, False, True))
    End Sub

    Private Sub ticker_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ticker.Tick
        If My.Settings.UseDockAsTaskbar Then
            bdr.Width = My.Computer.Screen.WorkingArea.Width
            ff.Width = My.Computer.Screen.WorkingArea.Width
            If My.Settings.pos = "Bottom" Then
                Me.Top = Forms.Screen.PrimaryScreen.Bounds.Height - Me.Height + Forms.Screen.PrimaryScreen.Bounds.Top + IIf(My.Settings.autoHide And Not isdockhovered, My.Settings.Size - 2, 0) + My.Settings.paddingTop
            Else
                Me.Top = Forms.Screen.PrimaryScreen.Bounds.Top - IIf(My.Settings.autoHide And Not isdockhovered, My.Settings.Size - 2, 0) + My.Settings.paddingTop
            End If
        Else
            If My.Settings.pos = "Bottom" Then
                Me.Top = Forms.Screen.PrimaryScreen.WorkingArea.Height - Me.Height + Forms.Screen.PrimaryScreen.WorkingArea.Top + IIf(My.Settings.autoHide And Not isdockhovered, My.Settings.Size - 2, 0) + My.Settings.paddingTop
            Else
                Me.Top = Forms.Screen.PrimaryScreen.WorkingArea.Top - IIf(My.Settings.autoHide And Not isdockhovered, My.Settings.Size - 2, 0) + My.Settings.paddingTop
            End If
        End If
        'If My.Settings.pos = "Bottom" Then
        'Else
        'Me.Left = Forms.Screen.PrimaryScreen.WorkingArea.Width - Me.Width + 180
        'End If
        If My.Settings.topMost Then
            Me.Topmost = True
        End If
        Me.Left = 0
        'Me.WindowState = Windows.WindowState.Normal
        'Me.Show()
        Try
            If menustack.Visibility = Windows.Visibility.Visible Then
                appname.Content = OptionsIcon.appname
                appname.Visibility = Windows.Visibility.Visible
            End If
        Catch
        End Try
        If Not appsgrid.Height = My.Settings.Size Then
            appsgrid.Height = My.Settings.Size
            Me.Height = appsgrid.Height + 162
            reicon()
        End If
        Try
            appsgrid.Width += (agwid - appsgrid.Width) / My.Settings.animatescale
        Catch
        End Try
        Try
            runingapps.Width += (ruwid - runingapps.Width) / My.Settings.animatescale
        Catch
        End Try
        Try
            isappopen.Width += ((agwid + ruwid) - isappopen.Width) / My.Settings.animatescale
        Catch
        End Try
        Dim a As Integer = 0
        Dim ar As Integer = 0
        If Not My.Settings.animatescale = 1 Then
            Try
                For Each i As UIElement In appsgrid.Children
                    If TypeOf i Is Image Then
                        Dim ii As Image = i
                        a += My.Settings.Size - 5
                        If appsgrid.Width >= a Then
                            ii.Height += (My.Settings.Size - 6 - ii.Height) / My.Settings.animatescale
                        Else
                            ii.Height = 5
                        End If
                    ElseIf TypeOf i Is Grid Then
                        Dim ii As Grid = i
                        a += 3
                        If appsgrid.Width >= a Then
                            ii.Height += (My.Settings.Size - 6 - ii.Height) / My.Settings.animatescale
                        Else
                            ii.Height = 5
                        End If
                    End If
                Next
                For Each i As UIElement In runingapps.Children
                    If TypeOf i Is Grid Then
                        Dim ii As Grid = i
                        ar += 3
                        If appsgrid.Width >= ar Then
                            ii.Height += (My.Settings.Size - 6 - ii.Height) / My.Settings.animatescale
                        Else
                            ii.Height = 5
                        End If
                    End If
                    If TypeOf i Is Image Then
                        Dim ii As Image = i
                        ar += 3
                        If appsgrid.Width >= ar Then
                            ii.Height += (My.Settings.Size - 6 - ii.Height) / My.Settings.animatescale
                        Else
                            ii.Height = 5
                        End If
                    End If
                Next
            Catch
            End Try
        End If
    End Sub

    Private Sub DeleteIconButton_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles DeleteIconButton.MouseUp
        iconlist.RemoveAt(OptionsIcon.idd)
        'MsgBox(OptionsIcon.idd)
        menustack.Visibility = Windows.Visibility.Hidden
        appname.Visibility = Windows.Visibility.Hidden
        savicon()
        reicon(False)
    End Sub

    Private Sub MainWindow_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        savicon()
    End Sub

    Sub savicon()
        Dim filecontent As String = ""
        Dim firstone As Boolean = True
        For Each a As Object In iconlist
            If Not firstone Then
                filecontent += "|"
            End If
            If a.Length = 3 And TypeOf a Is Array Then
                filecontent += a(0) & "*" & a(1) & "*" & a(2)
            Else
                filecontent += a
            End If
            firstone = False
        Next
        My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data", filecontent, False)
    End Sub

    Private Sub OptIconButton_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles OptIconButton.MouseUp
        Dim icop As New IconOptions
        icop.iconn = OptionsIcon
        icop.mainwin = Me
        icop.iconid = OptionsIcon.idd
        menustack.Visibility = Visibility.Hidden
        icop.ShowDialog()
        savicon()
    End Sub

    Private Sub appsgrid_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles MyBase.DragEnter
        e.Effects = e.AllowedEffects
    End Sub

    Private Sub appsgrid_Drop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles MyBase.Drop
        'If e.Data.GetDataPresent(e.Data.GetDataPresent(DataFormats.FileDrop)) Then
        Try
            For Each Path As String In e.Data.GetData(DataFormats.FileDrop)
                Dim n = My.Computer.FileSystem.GetName(Path)
                If n = "" Then
                    n = Path
                End If
                Try
                    Dim fi As New IO.FileInfo(Path)
                    Dim droptemp = fi.Length 'This should give a error if drag is folder
                    n = n.Replace(fi.Extension, "")
                Catch
                    'do nothing if drop is folder.
                End Try
                Dim a As New iconobj
                Dim aiconsuccess As Boolean = False
                Dim findico = True
                If Not icopack Is Nothing Then
                    If Not icopack.GetValue("IconPaths", n) = "Code_Item.NotFound" Then
                        findico = False
                        a.iconpath = icopack.GetValue("IconPaths", n).Replace("{Budgie.BDock.ConfigDirectory}", My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\").Replace("{iniDir}", My.Settings.CurrentIconThemePath.Replace(My.Computer.FileSystem.GetName(My.Settings.CurrentIconThemePath), ""))
                        iconlist.Add({Path, icopack.GetValue("IconPaths", n).Replace("{Budgie.BDock.ConfigDirectory}", My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\"), n})
                    End If
                End If
                If findico Then
                    Try
                        Dim aa As System.Drawing.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Path)
                        Try
                            My.Computer.FileSystem.DeleteFile(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + Path.Replace(":", "+").Replace("\", "+") + ".png")
                        Catch
                        End Try
                        aa.ToBitmap().Save(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + Path.Replace(":", "+").Replace("\", "+") + ".png", System.Drawing.Imaging.ImageFormat.Png)
                        aiconsuccess = True
                    Catch
                        If My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + Path.Replace(":", "+").Replace("\", "+") + ".png") Then
                            aiconsuccess = True
                        End If
                    End Try
                End If
                a.apppath = Path
                If aiconsuccess Then
                    a.iconpath = My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + Path.Replace(":", "+").Replace("\", "+") + ".png"
                    iconlist.Add({Path, My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + Path.Replace(":", "+").Replace("\", "+") + ".png", n})
                ElseIf findico Then
                    a.iconpath = "pack://application:,,,/Budgie%20Dock;component/unknown.png"
                    iconlist.Add({Path, "pack://application:,,,/Budgie%20Dock;component/unknown.png", n})
                End If
                a.appname = n
                a.stackpanel = appsgrid
                a.containerwin = Me
                a.endinit()
                a.idd = iddd
                iddd += 1
            Next
            savicon()
            reicon(False)
            'End If
        Catch
            appname.Visibility = Windows.Visibility.Visible
            appname.Content = "Can't Add Icon Of That Type"
            Canvas.SetLeft(appname, (Me.Width / 2) - (appname.ActualWidth / 2))
        End Try
    End Sub

    Private Sub OptMainButton_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles OptMainButton.MouseUp
        Dim icop As New BDOptions
        menustack.Visibility = Visibility.Hidden
        appname.Visibility = Windows.Visibility.Hidden
        icop.ShowDialog()
        Try
            If My.Settings.dockcr.Contains(",") Then
                Dim crlist = My.Settings.dockcr.Split(",")
                bdr.CornerRadius = New CornerRadius(crlist(0), crlist(1), crlist(2), crlist(3))
            Else
                bdr.CornerRadius = New CornerRadius(My.Settings.dockcr)
            End If
        Catch ex As Exception
            MsgBox("Failled to set corner radius: " + ex.Message)
        End Try
        bdr.Background = New SolidColorBrush(Color.FromArgb((My.Settings.dockopacity / 100) * 255, My.Settings.dockRed, My.Settings.dockGreen, My.Settings.dockBlue))
        If My.Settings.ApplyDockColorAtIsAppRuning Then
            ff.Background = bdr.Background
        End If
        RedesignLayout()
        reicon()
    End Sub

    Private Sub Window_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MyBase.KeyUp
        If e.Key = Key.S Then
            Dim icop As New BDOptions
            menustack.Visibility = Visibility.Hidden
            icop.ShowDialog()
            Try
                If My.Settings.dockcr.Contains(",") Then
                    Dim crlist = My.Settings.dockcr.Split(",")
                    bdr.CornerRadius = New CornerRadius(crlist(0), crlist(1), crlist(2), crlist(3))
                Else
                    bdr.CornerRadius = New CornerRadius(My.Settings.dockcr)
                End If
            Catch ex As Exception
                MsgBox("Failled to set corner radius: " + ex.Message)
            End Try
            bdr.Background = New SolidColorBrush(Color.FromArgb((My.Settings.dockopacity / 100) * 255, My.Settings.dockRed, My.Settings.dockGreen, My.Settings.dockBlue))
            If My.Settings.ApplyDockColorAtIsAppRuning Then
                ff.Background = bdr.Background
            End If
            RedesignLayout()
            reicon()
        ElseIf e.Key = Key.R Then
            appsgrid.Width = My.Settings.Size
            isappopen.Width = My.Settings.Size
            runingapps.Width = 0
            reicon()
        ElseIf e.Key = Key.L Then
            Dim win As New ItemListing
            win.ShowDialog()
            reicon()
        End If
    End Sub

    Private Sub ListingButton_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ListingButton.MouseUp
        Dim win As New ItemListing
        appname.Visibility = Windows.Visibility.Hidden
        menustack.Visibility = Visibility.Hidden
        win.ShowDialog()
        reicon()
    End Sub

    Private Sub AddspButton_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles AddspButton.MouseUp
        iconlist.Add("sep")
        appname.Visibility = Windows.Visibility.Hidden
        menustack.Visibility = Visibility.Hidden
        savicon()
        reicon()
    End Sub

    Private Sub bdr_MouseEnter(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles Me.MouseEnter
        isdockhovered = True
    End Sub

    Private Sub bdr_MouseLeave(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles Me.MouseLeave
        isdockhovered = False
    End Sub

    Private Sub refer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles refer.Tick
        refopenapps()
    End Sub
    Sub refopenapps(Optional ByVal ani As Boolean = True)
        For Each app As Process In Process.GetProcesses
            If Not app.MainWindowHandle = IntPtr.Zero Then
                If Not app.MainWindowTitle.Trim = "" Then
                    If Not disallowedpnames.Contains(app.ProcessName.ToLower) Or disallowedpnames.Contains(app.MainWindowTitle) Then
                        If Not icc.Contains(app.ProcessName.ToLower) Then
                            If Not aaps.Contains(app.Id) Then
                                ruwid += My.Settings.Size
                                If ruwid = My.Settings.Size Then
                                    Dim a As New Grid
                                    a.UseLayoutRounding = True
                                    a.Height = 5
                                    a.Background = New SolidColorBrush(Color.FromRgb(My.Settings.separatorRed, My.Settings.SeparatorGreen, My.Settings.SeparatorBlue))
                                    a.Width = 1
                                    a.ClipToBounds = True
                                    a.Margin = New Thickness(1, 0, 1, 0)
                                    a.ClipToBounds = True
                                    runingapps.Children.Add(a)
                                    Dim pp As New Grid
                                    pp.Width = 3
                                    isappopen.Children.Add(pp)
                                    ruwid += 3
                                End If
                                Dim ico As New iconobj
                                'ico.idd = 0
                                Dim findico = True
                                If Not icopack Is Nothing Then
                                    If Not icopack.GetValue("IconPaths", app.ProcessName) = "Code_Item.NotFound" Then
                                        findico = False
                                        ico.iconpath = icopack.GetValue("IconPaths", app.ProcessName).Replace("{Budgie.BDock.ConfigDirectory}", My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\").Replace("{iniDir}", My.Settings.CurrentIconThemePath.Replace(My.Computer.FileSystem.GetName(My.Settings.CurrentIconThemePath), ""))
                                    End If
                                End If
                                If findico Then
                                    Try
                                        Dim icoa As System.Drawing.Icon = System.Drawing.Icon.ExtractAssociatedIcon(app.MainModule.FileName)
                                        icoa.ToBitmap().Save(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + app.Id.ToString + app.ProcessName + ".png", System.Drawing.Imaging.ImageFormat.Png)
                                        ico.iconpath = My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + app.Id.ToString + app.ProcessName + ".png"
                                    Catch ex As Exception
                                        If My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + app.Id.ToString + app.ProcessName + ".png") Then
                                            ico.iconpath = My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + app.Id.ToString + app.ProcessName + ".png"
                                        Else
                                            ico.iconpath = "pack://application:,,,/Budgie%20Dock;component/unknown.png"
                                        End If
                                    End Try
                                End If
                                ico.appname = app.MainWindowTitle & " - " + app.ProcessName
                                ico.stackpanel = runingapps
                                ico.apppath = app.ProcessName
                                ico.containerwin = Me
                                ico.runid = rid
                                ico.isEditingAvable = False
                                ico.hr = True
                                ico.runproc = app
                                ico.isapopen.Background = Brushes.White
                                ico.isopen = True
                                ico.isprocfound = True
                                ico.checkIfRuning = False
                                ico.endinit()
                                If ani Then ico.imageiconobj.Height = 5 'My.Settings.Size - 5
                                Try
                                    If Not ani Then ico.imageiconobj.Height = My.Settings.Size - 5
                                Catch
                                End Try
                                aaps.Add(app.Id)
                            End If
                        End If
                    End If
                End If
            End If
        Next
        ruwid = 0
        For Each itm As UIElement In runingapps.Children
            If TypeOf itm Is Image Then
                Dim itemm As Image = itm
                ruwid += itemm.Width
            Else
                ruwid += 3
            End If
        Next
    End Sub

    Private Sub Window_Initialized(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Initialized
        If Command().Replace("""", "").Split(" ")(0) = "AddIcon" Then
            If My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data").Contains("*") Then
                For Each Iconn As String In My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data").Split("|")
                    If Iconn = "sep" Then
                        iconlist.Add("sep")
                    Else
                        iconlist.Add({Iconn.Split("*")(0), Iconn.Split("*")(1), Iconn.Split("*")(2)})
                    End If
                Next
            End If
            Try
                Dim Path = Split(Command.Replace("""", ""), " ", 2)(1)
                Dim n = My.Computer.FileSystem.GetName(Path)
                If n = "" Then
                    n = Path
                End If
                Try
                    Dim fi As New IO.FileInfo(Path)
                    Dim droptemp = fi.Length 'This should give a error if drag is folder
                    n = n.Replace(fi.Extension, "")
                Catch
                    'do nothing if drop is folder.
                End Try
                Dim aiconsuccess As Boolean = False
                Dim findico = True
                If Not icopack Is Nothing Then
                    If Not icopack.GetValue("IconPaths", n) = "Code_Item.NotFound" Then
                        findico = False
                        iconlist.Add({Path, icopack.GetValue("IconPaths", n).Replace("{Budgie.BDock.ConfigDirectory}", My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\"), n})
                    End If
                End If
                If findico Then
                    Try
                        Dim aa As System.Drawing.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Path)
                        Try
                            My.Computer.FileSystem.DeleteFile(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + Path.Replace(":", "+").Replace("\", "+") + ".png")
                        Catch
                        End Try
                        aa.ToBitmap().Save(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + Path.Replace(":", "+").Replace("\", "+") + ".png", System.Drawing.Imaging.ImageFormat.Png)
                        aiconsuccess = True
                    Catch
                        If My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + Path.Replace(":", "+").Replace("\", "+") + ".png") Then
                            aiconsuccess = True
                        End If
                    End Try
                End If
                If aiconsuccess Then
                    iconlist.Add({Path, My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + Path.Replace(":", "+").Replace("\", "+") + ".png", n})
                ElseIf findico Then
                    iconlist.Add({Path, "pack://application:,,,/Budgie%20Dock;component/unknown.png", n})
                End If
                iddd += 1
                savicon()
                Console.WriteLine("Icon Added")
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
            End Try
            End
        End If
    End Sub
End Class
