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

    'Private Declare Auto Function IsIconic Lib "user32.dll" (ByVal hwnd As IntPtr) As Boolean
    '<DllImport("user32.dll")> _
    'Private Shared Function GetWindowRect(ByVal hWnd As HandleRef, ByRef lpRect As Rect) As Boolean
    'End Function
    Dim icc As New ArrayList
    Dim icopack As Ini = Nothing
    Public SystemDPI = Forms.Screen.PrimaryScreen.Bounds.Height / SystemParameters.PrimaryScreenHeight
    Public MILeft As Integer
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Try
            insertToLog("BudgieDock Launched")
            ticker = New DispatcherTimer
            ticker.Interval = TimeSpan.FromMilliseconds(1)
            ticker.Start()
            refer = New DispatcherTimer
            refer.Interval = TimeSpan.FromMilliseconds(1000)
            refer.Start()
            insertToLog("Timers Set")
            For Each dup As String In defaultdisallowed
                disallowedpnames.Add(dup)
            Next
            insertToLog("Defaultdisallowed Added To Disallowed Names")
            insertToLog("Missing Files Recreation Start")
            My.Computer.FileSystem.CreateDirectory(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\")
            My.Computer.FileSystem.CreateDirectory(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons")
            If Not My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data") Then
                Dim dd As String = ""
                My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data", dd, False)
            End If
            If Not My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Settings.ini") Then
                Dim dd As String = My.Resources.DefaultSettings
                My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Settings.ini", dd, False)
            End If
            If Not My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data") Then
                Dim dd As String = ""
                My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data", dd, False)
            End If
            insertToLog("Missing Files Recreation End")
            Try
                For Each dup As String In My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data").Split("|")
                    disallowedpnames.Add(dup)
                Next
            Catch ex As Exception
                insertToLog("Handled Error: " & vbNewLine & ex.ToString)
            End Try
            insertToLog("Updating icon data if outdated")
            UpdateIconsData.UpdateNow()
            insertToLog("init Settings")
            InitSettings()
            appsgrid.Width = GetSetting("size")
            insertToLog("Adding hover effects")
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
            insertToLog("Setting Corner Radius Of The Dock")
            Try
                If GetSetting("dockCornerRadius").Contains(",") Then
                    Dim crlist = GetSetting("dockCornerRadius").Split(",")
                    bdr.CornerRadius = New CornerRadius(crlist(0), crlist(1), crlist(2), crlist(3))
                Else
                    bdr.CornerRadius = New CornerRadius(GetSetting("dockCornerRadius"))
                End If
            Catch ex As Exception
                MsgBox("Failled to set corner radius: " + ex.Message)
                insertToLog("Handled Error: " & vbNewLine & ex.ToString)
            End Try
            insertToLog("Setting Position")
            If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                Me.Height = My.Computer.Screen.WorkingArea.Height
            Else
                Me.Width = My.Computer.Screen.WorkingArea.Width
            End If
            Me.Left = 0
            If GetSetting("animateScale") = 0 Then
                SetSetting("animateScale", 1, True)
            End If
            Me.Height = appsgrid.Height + 163
            insertToLog("Setting Colors")
            bdr.Background = New SolidColorBrush(Color.FromArgb((GetSetting("dockOpacity") / 100) * 255, GetSetting("dockRed"), GetSetting("dockGreen"), GetSetting("dockBlue")))
            If GetSetting("applyDockColorAtIsAppRuning") Then
                ff.Background = bdr.Background
            End If
            Me.Topmost = GetSetting("topMost")
            onScreenResChange()
            RedesignLayout()
            reicon()
        Catch ex As Exception
            insertToLog(ex.ToString)
        End Try
    End Sub

    Sub RedesignLayout()
        Try
            insertToLog("RedesignLayout Executing..")
            If GetSetting("pos") = "Bottom" Then
                mas.Children.Remove(ncan)
                mas.Children.Remove(bdr)
                mas.Children.Remove(ff)
                mas.Children.Add(ncan)
                mas.Children.Add(bdr)
                mas.Children.Add(ff)
                Canvas.SetTop(appname, 130)
                Canvas.SetTop(menustack, 0)
                mas.Orientation = Orientation.Vertical
                mas.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
                appsgrid.Orientation = Orientation.Horizontal
                isappopen.Orientation = Orientation.Horizontal
                runingapps.Orientation = Orientation.Horizontal
                mg.VerticalAlignment = Windows.VerticalAlignment.Bottom
                ff.Height = 3
                isappopen.Height = 3
                appsgrid.Width = 0
                runingapps.Width = 0
                bdrcont.Orientation = Orientation.Horizontal
                bdr.HorizontalAlignment = Windows.HorizontalAlignment.Center
            ElseIf GetSetting("pos") = "Top" Then
                mas.Children.Remove(ncan)
                mas.Children.Remove(bdr)
                mas.Children.Remove(ff)
                mas.Children.Add(ff)
                mas.Children.Add(bdr)
                mas.Children.Add(ncan)
                Canvas.SetTop(appname, 0)
                Canvas.SetTop(menustack, 30)
                mas.Orientation = Orientation.Vertical
                appsgrid.Orientation = Orientation.Horizontal
                isappopen.Orientation = Orientation.Horizontal
                runingapps.Orientation = Orientation.Horizontal
                mg.VerticalAlignment = Windows.VerticalAlignment.Bottom
                mas.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
                ff.Height = 3
                isappopen.Height = 3
                appsgrid.Width = 0
                runingapps.Width = 0
                bdrcont.Orientation = Orientation.Horizontal
                bdr.HorizontalAlignment = Windows.HorizontalAlignment.Center
            ElseIf GetSetting("pos") = "Right" Then
                mas.Children.Remove(ncan)
                mas.Children.Remove(bdr)
                mas.Children.Remove(ff)
                mas.Orientation = Orientation.Horizontal
                mas.Children.Add(ncan)
                mas.Children.Add(bdr)
                mas.Children.Add(ff)
                ncan.Height = Me.Height
                ncan.Margin = New Thickness(-200 - GetSetting("size"), 0, 0, 0)
                ff.Width = 3
                isappopen.Width = 3
                isappopen.Orientation = Orientation.Vertical
                runingapps.Orientation = Orientation.Vertical
                mg.VerticalAlignment = Windows.VerticalAlignment.Center
                bdr.VerticalAlignment = Windows.VerticalAlignment.Center
                appsgrid.Orientation = Orientation.Vertical
                appsgrid.Height = 0
                runingapps.Height = 0
                mas.HorizontalAlignment = Windows.HorizontalAlignment.Right
                bdrcont.Orientation = Orientation.Vertical
                bdr.HorizontalAlignment = Windows.HorizontalAlignment.Right
            ElseIf GetSetting("pos") = "Left" Then
                mas.Children.Remove(ncan)
                mas.Children.Remove(bdr)
                mas.Children.Remove(ff)
                mas.Orientation = Orientation.Horizontal
                mas.Children.Add(ff)
                mas.Children.Add(bdr)
                mas.Children.Add(ncan)
                ncan.Height = Me.Height
                ncan.Margin = New Thickness(0, 0, -200, 0)
                ff.Width = 3
                isappopen.Width = 3
                isappopen.Orientation = Orientation.Vertical
                runingapps.Orientation = Orientation.Vertical
                mg.VerticalAlignment = Windows.VerticalAlignment.Center
                bdr.VerticalAlignment = Windows.VerticalAlignment.Center
                appsgrid.Orientation = Orientation.Vertical
                appsgrid.Height = 0
                runingapps.Height = 0
                mas.HorizontalAlignment = Windows.HorizontalAlignment.Left
                bdrcont.Orientation = Orientation.Vertical
                bdr.HorizontalAlignment = Windows.HorizontalAlignment.Left
            End If
            mas.UpdateLayout()
            runingapps.UpdateLayout()
            ff.UpdateLayout()
            ncan.UpdateLayout()
            ff.UpdateLayout()
        Catch ex As Exception
            insertToLog(ex.ToString)
        End Try
    End Sub

    Sub LoadIconPack()
        Try
            insertToLog("Loading icon packs..")
            If My.Computer.FileSystem.FileExists(GetSetting("currentIconThemePath")) Then
                icopack = New Ini(GetSetting("currentIconThemePath"))
            Else
                icopack = Nothing
            End If
        Catch ex As Exception
            insertToLog(ex.ToString)
        End Try
    End Sub

    Sub reicon(Optional ByVal animate As Boolean = True)
        Try
            runingapps.Width = GetSetting("size")
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
            insertToLog("Reset Values")
            'Dim wid As New RWidget("C:\Users\ASUS PC\Desktop\testextension.bdockwidget", appsgrid)
            For Each dup As String In defaultdisallowed
                disallowedpnames.Add(dup)
            Next
            insertToLog("Defaultdisallowed Added To Disallowed Names")
            Try
                For Each dup As String In My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data").Split("|")
                    disallowedpnames.Add(dup)
                Next
            Catch ex As Exception
                insertToLog("Handled Error: " & vbNewLine & ex.ToString)
            End Try
            If My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data").Contains("*") Then
                For Each Iconn As String In My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data").Split("|")
                    insertToLog("Current icon:" & Iconn)
                    If Iconn = "sep" Then
                        Dim a As New Grid
                        a.UseLayoutRounding = True
                        If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                            If Not GetSetting("animateScale") = 1 Then a.Width = 5
                            Try
                                If GetSetting("animateScale") = 1 Then a.Width = GetSetting("size") - 5
                            Catch
                            End Try
                        Else
                            If Not GetSetting("animateScale") = 1 Then a.Height = 5
                            Try
                                If GetSetting("animateScale") = 1 Then a.Height = GetSetting("size") - 5
                            Catch
                            End Try
                        End If
                        a.Background = New SolidColorBrush(Color.FromRgb(GetSetting("separatorRed"), GetSetting("SeparatorGreen"), GetSetting("SeparatorBlue")))
                        If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                            a.Height = 1
                        Else
                            a.Width = 1
                        End If
                        a.ClipToBounds = True
                        If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                            a.Margin = New Thickness(0, 1, 0, 1)
                        Else
                            a.Margin = New Thickness(1, 0, 1, 0)
                        End If
                        Dim sr As New Sepremover
                        sr.id = iddd
                        sr.mainwin = Me
                        sr.sepgrid = a
                        appsgrid.Children.Add(a)
                        iconlist.Add("sep")
                        iddd += 1
                        a.ClipToBounds = True
                        Dim pp As New Grid
                        If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                            pp.Height = 3
                        Else
                            pp.Width = 3
                        End If
                        pp.HorizontalAlignment = Windows.HorizontalAlignment.Center
                        isappopen.Children.Add(pp)
                        agwid += 3
                    ElseIf Iconn.Split(":")(0) = "icon" Then
                        Iconn = Iconn.Split(":")(1)
                        Dim a As New iconobj
                        a.apppath = Iconn.Split("*")(0).Replace("{BD-STAR-}", "*").Replace("{BD-FLINE-}", "|").Replace("{BD-TD-}", ":").Split("^")(0)
                        a.iconpath = Iconn.Split("*")(1).Replace("{BD-TD-}", ":")
                        a.appname = Iconn.Split("*")(2).Replace("{BD-STAR-}", "*").Replace("{BD-TD-}", ":").Replace("{BD-FLINE-}", "|")
                        a.stackpanel = appsgrid
                        a.containerwin = Me
                        a.runid = rid
                        Try
                            a.apparams = Iconn.Split("*")(0).Replace("{BD-TD-}", ":").Replace("{BD-STAR-}", "*").Replace("{BD-FLINE-}", "|").Split("^")(1).Replace("{BD-UPL-}", "^")
                        Catch
                        End Try
                        a.endinit()
                        a.imageiconobj.ClipToBounds = True
                        'Try
                        'a.imageiconobj.Height = GetSetting("size") - 5
                        'Catch
                        'End Try
                        If animate Then
                            If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                                If Not GetSetting("animateScale") = 1 Then a.imageiconobj.Width = 5
                                Try
                                    If GetSetting("animateScale") = 1 Then a.imageiconobj.Width = GetSetting("size") - 5
                                Catch
                                End Try
                            Else
                                If Not GetSetting("animateScale") = 1 Then a.imageiconobj.Height = 5
                                Try
                                    If GetSetting("animateScale") = 1 Then a.imageiconobj.Height = GetSetting("size") - 5
                                Catch
                                End Try
                            End If
                        End If
                        a.idd = iddd
                        iconlist.Add({"icon", Iconn.Split("*")(0), Iconn.Split("*")(1), Iconn.Split("*")(2)})
                        iddd += 1
                        icc.Add(My.Computer.FileSystem.GetName(Iconn.Split("*")(0).Replace("{BD-STAR-}", "*").Replace("{BD-FLINE-}", "|").Replace("{BD-TD-}", ":").Split("^")(0)).ToLower)
                        agwid += GetSetting("size")
                    End If
                Next
            Else
                Dim lbldrg As New Label
                lbldrg.Content = "Drag Here To Add Icons"
                appsgrid.Children.Add(lbldrg)
                agwid = 138
            End If
            refopenapps(IIf(GetSetting("animateScale") = 1, False, True))
        Catch ex As Exception
            insertToLog(ex.ToString)
        End Try
    End Sub

    Sub onScreenResChange()
        Try
            MILeft = 0
            For i As Integer = 0 To GetSetting("placedScreenID") - 1
                MILeft += Forms.Screen.AllScreens(i).Bounds.Width / SystemDPI
            Next
            'Console.WriteLine(My.Computer.Screen.WorkingArea.Width & "-" & My.Computer.Screen.WorkingArea.Left & "-" & Me.Width)
            If GetSetting("useDockAsTaskbar") Then
                If GetSetting("pos") = "Bottom" Then
                    Me.Top = Forms.Screen.AllScreens(GetSetting("placedScreenID")).Bounds.Height / SystemDPI - Me.Height + (Forms.Screen.AllScreens(GetSetting("placedScreenID")).WorkingArea.Top / SystemDPI) + IIf(GetSetting("autoHide") And Not isdockhovered, GetSetting("size") - 2, 0) + GetSetting("paddingTop")
                    bdr.Width = Forms.Screen.AllScreens(GetSetting("placedScreenID")).Bounds.Width / SystemDPI
                    ff.Width = Forms.Screen.AllScreens(GetSetting("placedScreenID")).Bounds.Width / SystemDPI
                ElseIf GetSetting("pos") = "Top" Then
                    Me.Top = Forms.Screen.AllScreens(GetSetting("placedScreenID")).Bounds.Top / SystemDPI - IIf(GetSetting("autoHide") And Not isdockhovered, GetSetting("size") - 2, 0) + GetSetting("paddingTop")
                    bdr.Width = Forms.Screen.AllScreens(GetSetting("placedScreenID")).Bounds.Width / SystemDPI
                    ff.Width = Forms.Screen.AllScreens(GetSetting("placedScreenID")).Bounds.Width / SystemDPI
                ElseIf GetSetting("pos") = "Right" Then
                    Me.Left = Forms.Screen.AllScreens(GetSetting("placedScreenID")).Bounds.Width / SystemDPI - (Me.Width * SystemDPI) + (Forms.Screen.AllScreens(GetSetting("placedScreenID")).WorkingArea.Left / SystemDPI) + IIf(GetSetting("autoHide") And Not isdockhovered, GetSetting("size") - 2, 0) + GetSetting("paddingTop")
                    bdr.Height = Forms.Screen.AllScreens(GetSetting("placedScreenID")).Bounds.Height / SystemDPI
                    ff.Height = Forms.Screen.AllScreens(GetSetting("placedScreenID")).Bounds.Height / SystemDPI
                ElseIf GetSetting("pos") = "Left" Then
                    Me.Left = Forms.Screen.AllScreens(GetSetting("placedScreenID")).Bounds.Left / SystemDPI - IIf(GetSetting("autoHide") And Not isdockhovered, GetSetting("size") - 2, 0)
                    bdr.Height = Forms.Screen.AllScreens(GetSetting("placedScreenID")).Bounds.Height / SystemDPI
                    ff.Height = Forms.Screen.AllScreens(GetSetting("placedScreenID")).Bounds.Height / SystemDPI
                End If
            Else
                If GetSetting("pos") = "Bottom" Then
                    Me.Top = Forms.Screen.AllScreens(GetSetting("placedScreenID")).WorkingArea.Height / SystemDPI - (Me.Height * SystemDPI) + (Forms.Screen.AllScreens(GetSetting("placedScreenID")).WorkingArea.Top / SystemDPI) + IIf(GetSetting("autoHide") And Not isdockhovered, GetSetting("size") - 2, 0) + GetSetting("paddingTop")
                ElseIf GetSetting("pos") = "Top" Then
                    Me.Top = Forms.Screen.AllScreens(GetSetting("placedScreenID")).WorkingArea.Top / SystemDPI - IIf(GetSetting("autoHide") And Not isdockhovered, GetSetting("size") - 2, 0) + GetSetting("paddingTop")
                ElseIf GetSetting("pos") = "Right" Then
                    Me.Left = Forms.Screen.AllScreens(GetSetting("placedScreenID")).WorkingArea.Width / SystemDPI - (Me.Width * SystemDPI) + (Forms.Screen.AllScreens(GetSetting("placedScreenID")).WorkingArea.Left / SystemDPI) + IIf(GetSetting("autoHide") And Not isdockhovered, GetSetting("size") - 2, 0) + GetSetting("paddingTop")
                ElseIf GetSetting("pos") = "Left" Then
                    Me.Left = Forms.Screen.AllScreens(GetSetting("placedScreenID")).WorkingArea.Left / SystemDPI - IIf(GetSetting("autoHide") And Not isdockhovered, GetSetting("size") - 2, 0)
                End If
            End If
            If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                Me.Top = Forms.Screen.AllScreens(GetSetting("placedScreenID")).Bounds.Top / SystemDPI
            Else
                Me.Left = Forms.Screen.AllScreens(GetSetting("placedScreenID")).Bounds.Left / SystemDPI
            End If
        Catch ex As Exception
            insertToLog(ex.ToString)
        End Try
    End Sub

    Dim oldSres As Integer

    Private Sub ticker_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ticker.Tick
        Try
            onScreenResChange()
            If GetSetting("topMost") = 1 Then
                Me.Topmost = True
            End If
            Try
                If menustack.Visibility = Windows.Visibility.Visible Then
                    appname.Content = OptionsIcon.appname
                    appname.Visibility = Windows.Visibility.Visible
                End If
            Catch
            End Try
            If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                If Not appsgrid.Width = GetSetting("size") Then
                    appsgrid.Width = GetSetting("size")
                    runingapps.Width = GetSetting("size")
                    Me.Width = appsgrid.Height + 162
                    reicon()
                End If
            Else
                If Not appsgrid.Height = GetSetting("size") Then
                    appsgrid.Height = GetSetting("size")
                    Me.Height = appsgrid.Height + 162
                    runingapps.Height = GetSetting("size")
                    reicon()
                End If
            End If
            If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                appsgrid.Height += (agwid - appsgrid.Height) / GetSetting("animateScale")
                runingapps.Height += (ruwid - runingapps.Height) / GetSetting("animateScale")
                isappopen.Height += ((agwid + ruwid) - isappopen.Height) / GetSetting("animateScale")
            Else
                appsgrid.Width += (agwid - appsgrid.Width) / GetSetting("animateScale")
                runingapps.Width += (ruwid - runingapps.Width) / GetSetting("animateScale")
                isappopen.Width += ((agwid + ruwid) - isappopen.Width) / GetSetting("animateScale")
                'ff.Width = isappopen.Width
            End If
            Dim a As Integer = 0
            Dim ar As Integer = 0
            'Console.WriteLine(isappopen.Width & " " & isappopen.Height & " " & isappopen.Visibility.ToString)
            If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                If Not GetSetting("animateScale") = 1 Then
                    For Each i As UIElement In appsgrid.Children
                        If TypeOf i Is Image Then
                            Dim ii As Image = i
                            'Console.WriteLine(appsgrid.Height & "-" & a)
                            a += GetSetting("size") - 5
                            If appsgrid.Height >= a Then
                                ii.Width += (GetSetting("size") - 6 - ii.Width) / GetSetting("animateScale")
                            Else
                                ii.Width = 5
                            End If
                        ElseIf TypeOf i Is Grid Then
                            Dim ii As Grid = i
                            a += 3
                            If appsgrid.Height >= a Then
                                ii.Width += (GetSetting("size") - 6 - ii.Width) / GetSetting("animateScale")
                            Else
                                ii.Width = 5
                            End If
                        End If
                    Next
                    For Each i As UIElement In runingapps.Children
                        If TypeOf i Is Grid Then
                            Dim ii As Grid = i
                            ar += 3
                            If appsgrid.Height >= ar Then
                                ii.Width += (GetSetting("size") - 6 - ii.Width) / GetSetting("animateScale")
                            Else
                                ii.Width = 5
                            End If
                        End If
                        If TypeOf i Is Image Then
                            Dim ii As Image = i
                            ar += 3
                            If appsgrid.Height >= ar Then
                                ii.Width += (GetSetting("size") - 6 - ii.Width) / GetSetting("animateScale")
                            Else
                                ii.Width = 5
                            End If
                        End If
                    Next
                End If
            Else
                If Not GetSetting("animateScale") = 1 Then
                    For Each i As UIElement In appsgrid.Children
                        If TypeOf i Is Image Then
                            Dim ii As Image = i
                            a += GetSetting("size") - 5
                            If appsgrid.Width >= a Then
                                ii.Height += (GetSetting("size") - 6 - ii.Height) / GetSetting("animateScale")
                            Else
                                ii.Height = 5
                            End If
                        ElseIf TypeOf i Is Grid Then
                            Dim ii As Grid = i
                            a += 3
                            If appsgrid.Width >= a Then
                                ii.Height += (GetSetting("size") - 6 - ii.Height) / GetSetting("animateScale")
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
                                ii.Height += (GetSetting("size") - 6 - ii.Height) / GetSetting("animateScale")
                            Else
                                ii.Height = 5
                            End If
                        End If
                        If TypeOf i Is Image Then
                            Dim ii As Image = i
                            ar += 3
                            If appsgrid.Width >= ar Then
                                ii.Height += (GetSetting("size") - 6 - ii.Height) / GetSetting("animateScale")
                            Else
                                ii.Height = 5
                            End If
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            insertToLog(ex.Message)
        End Try
    End Sub

    Private Sub DeleteIconButton_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles DeleteIconButton.MouseUp
        iconlist.RemoveAt(OptionsIcon.idd)
        'MsgBox(OptionsIcon.idd)
        menustack.Visibility = Windows.Visibility.Hidden
        appname.Visibility = Windows.Visibility.Hidden
        savicon()
        reicon(False)
        insertToLog("Icon Deleted")
    End Sub

    Private Sub MainWindow_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        savicon()
    End Sub

    Sub savicon()
        Try
            insertToLog("Saving Icons..")
            Dim filecontent As String = ""
            Dim firstone As Boolean = True
            For Each a As Object In iconlist
                If Not firstone Then
                    filecontent += "|"
                End If
                If TypeOf a Is Array Then
                    filecontent += a(0) & ":" & a(1) & "*" & a(2) & "*" & a(3)
                Else
                    filecontent += a
                End If
                firstone = False
            Next
            My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data", filecontent, False)
            insertToLog("Saved")
        Catch ex As Exception
            insertToLog(ex.ToString)
        End Try
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
    Dim ie = False
    Private Sub appsgrid_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles MyBase.DragEnter
        e.Effects = e.AllowedEffects
        ie = True
    End Sub

    Private Sub MainWindow_DragLeave(ByVal sender As Object, ByVal e As System.Windows.DragEventArgs) Handles MyBase.DragLeave
        ie = False
    End Sub

    Private Sub appsgrid_Drop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles MyBase.Drop
        insertToLog("Adding Icon..")
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
                    fi.Refresh()
                Catch
                    'do nothing if drop is folder.
                End Try
                Dim a As New iconobj
                Dim aiconsuccess As Boolean = False
                Dim findico = True
                If Not icopack Is Nothing Then
                    If Not icopack.GetValue("IconPaths", n) = "Code_Item.NotFound" Then
                        findico = False
                        a.iconpath = icopack.GetValue("IconPaths", n).Replace("{Budgie.BDock.ConfigDirectory}", My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\").Replace("{iniDir}", GetSetting("currentIconThemePath").Replace(My.Computer.FileSystem.GetName(GetSetting("currentIconThemePath")), ""))
                        iconlist.Add({"icon", Path.Replace(":", "{BD-TD-}"), icopack.GetValue("IconPaths", n).Replace("{Budgie.BDock.ConfigDirectory}", My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\").Replace(":", "{BD-TD-}"), n.Replace(":", "{BD-TD-}")})
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
                        aa.Dispose()
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
                    iconlist.Add({"icon", Path.Replace(":", "{BD-TD-}"), (My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + Path.Replace(":", "+").Replace("\", "+") + ".png").Replace(":", "{BD-TD-}"), n.Replace(":", "{BD-TD-}")})
                ElseIf findico Then
                    a.iconpath = "pack://application:,,,/Budgie%20Dock;component/unknown.png"
                    iconlist.Add({"icon", Path.Replace(":", "{BD-TD-}"), "pack://application:,,,/Budgie%20Dock;component/unknown.png".Replace(":", "{BD-TD-}"), n.Replace(":", "{BD-TD-}")})
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
        Catch ex As Exception
            insertToLog("Cannot Add Icon: " & vbNewLine & ex.ToString)
            appname.Visibility = Windows.Visibility.Visible
            appname.Content = "Can't Add Icon Of That Type"
            Canvas.SetLeft(appname, (Me.Width / 2) - (appname.ActualWidth / 2))
        End Try
    End Sub

    Private Sub OptMainButton_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles OptMainButton.MouseUp
        Try
            Dim icop As New BDOptions
            menustack.Visibility = Visibility.Hidden
            appname.Visibility = Windows.Visibility.Hidden
            icop.ShowDialog()
            InitSettings()
            Try
                If GetSetting("dockCornerRadius").Contains(",") Then
                    Dim crlist = GetSetting("dockCornerRadius").Split(",")
                    bdr.CornerRadius = New CornerRadius(crlist(0), crlist(1), crlist(2), crlist(3))
                Else
                    bdr.CornerRadius = New CornerRadius(GetSetting("dockCornerRadius"))
                End If
            Catch ex As Exception
                MsgBox("Failled to set corner radius: " + ex.Message)
            End Try
            bdr.Background = New SolidColorBrush(Color.FromArgb((GetSetting("dockOpacity") / 100) * 255, GetSetting("dockRed"), GetSetting("dockGreen"), GetSetting("dockBlue")))
            If GetSetting("applyDockColorAtIsAppRuning") Then
                ff.Background = bdr.Background
            End If
            RedesignLayout()
            onScreenResChange()
            reicon()
        Catch ex As Exception
            insertToLog(ex.ToString)
        End Try
    End Sub

    Private Sub Window_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MyBase.KeyUp
        Try
            If e.Key = Key.S Then
                Dim icop As New BDOptions
                menustack.Visibility = Visibility.Hidden
                icop.ShowDialog()
                InitSettings()
                Try
                    If GetSetting("dockCornerRadius").Contains(",") Then
                        Dim crlist = GetSetting("dockCornerRadius").Split(",")
                        bdr.CornerRadius = New CornerRadius(crlist(0), crlist(1), crlist(2), crlist(3))
                    Else
                        bdr.CornerRadius = New CornerRadius(GetSetting("dockCornerRadius"))
                    End If
                Catch ex As Exception
                    MsgBox("Failled to set corner radius: " + ex.Message)
                End Try
                bdr.Background = New SolidColorBrush(Color.FromArgb((GetSetting("dockOpacity") / 100) * 255, GetSetting("dockRed"), GetSetting("dockGreen"), GetSetting("dockBlue")))
                If GetSetting("applyDockColorAtIsAppRuning") Then
                    ff.Background = bdr.Background
                End If
                RedesignLayout()
                onScreenResChange()
                reicon()
            ElseIf e.Key = Key.R Then
                If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                    appsgrid.Height = GetSetting("size")
                    isappopen.Height = GetSetting("size")
                    runingapps.Height = 0
                Else
                    appsgrid.Width = GetSetting("size")
                    isappopen.Width = GetSetting("size")
                    runingapps.Width = 0
                End If
                reicon()
            ElseIf e.Key = Key.L Then
                Dim win As New ItemListing
                win.ShowDialog()
                reicon()
            End If
        Catch ex As Exception
            insertToLog(ex.ToString)
        End Try
    End Sub

    Private Sub ListingButton_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ListingButton.MouseUp
        Try
            Dim win As New ItemListing
            appname.Visibility = Windows.Visibility.Hidden
            menustack.Visibility = Visibility.Hidden
            win.ShowDialog()
            reicon()
        Catch ex As Exception
            insertToLog(ex.Message)
        End Try
    End Sub

    Private Sub AddspButton_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles AddspButton.MouseUp
        Try
            iconlist.Add("sep")
            appname.Visibility = Windows.Visibility.Hidden
            menustack.Visibility = Visibility.Hidden
            savicon()
            reicon()
            insertToLog("Added Seperator")
        Catch ex As Exception
            insertToLog(ex.Message)
        End Try
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
            Try
                If Not aaps.Contains(app.Id) Then
                    If Not app.MainWindowHandle = IntPtr.Zero Then
                        If Not app.MainWindowTitle.Trim = "" Then
                            If Not disallowedpnames.Contains(app.ProcessName.ToLower) Or disallowedpnames.Contains(app.MainWindowTitle.ToLower) Then
                                'If Not icc.Contains(app.ProcessName.ToLower) Then
                                If runingapps.Children.Count = 0 Then
                                    Dim a As New Grid
                                    a.UseLayoutRounding = True
                                    a.Background = New SolidColorBrush(Color.FromRgb(GetSetting("separatorRed"), GetSetting("SeparatorGreen"), GetSetting("SeparatorBlue")))
                                    If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                                        a.Height = 1
                                        a.Width = 5
                                    Else
                                        a.Height = 5
                                        a.Width = 1
                                    End If
                                    a.ClipToBounds = True
                                    a.Margin = New Thickness(1, 0, 1, 0)
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
                                        ico.iconpath = icopack.GetValue("IconPaths", app.ProcessName).Replace("{Budgie.BDock.ConfigDirectory}", My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\").Replace("{iniDir}", GetSetting("currentIconThemePath").Replace(My.Computer.FileSystem.GetName(GetSetting("currentIconThemePath")), ""))
                                    End If
                                End If
                                If findico Then
                                    Try
                                        Dim icoa As System.Drawing.Icon = System.Drawing.Icon.ExtractAssociatedIcon(app.MainModule.FileName)
                                        icoa.ToBitmap().Save(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + app.Id.ToString + app.ProcessName + ".png", System.Drawing.Imaging.ImageFormat.Png)
                                        icoa.Dispose()
                                        ico.iconpath = My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + app.Id.ToString + app.ProcessName + ".png"
                                    Catch ex As Exception
                                        If My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + app.Id.ToString + app.ProcessName + ".png") Then
                                            ico.iconpath = My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + app.Id.ToString + app.ProcessName + ".png"
                                        Else
                                            ico.iconpath = "pack://application:,,,/Budgie%20Dock;component/unknown.png"
                                        End If
                                        insertToLog(ex.ToString)
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
                                ico.isopen = True
                                ico.isprocfound = True
                                ico.checkIfRuning = False
                                ico.endinit()
                                If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                                    If ani Then ico.imageiconobj.Width = 5 'My.Settings.Size - 5
                                    Try
                                        If Not ani Then ico.imageiconobj.Width = GetSetting("size") - 5
                                    Catch
                                    End Try
                                Else
                                    If ani Then ico.imageiconobj.Height = 5 'My.Settings.Size - 5
                                    Try
                                        If Not ani Then ico.imageiconobj.Height = GetSetting("size") - 5
                                    Catch
                                    End Try
                                End If
                                aaps.Add(app.Id)
                                'End If
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                insertToLog(ex.ToString)
            End Try
        Next
        sizecalc()
    End Sub

    Sub sizecalc()
        ruwid = 0
        For Each itm As UIElement In runingapps.Children
            If TypeOf itm Is Image Then
                Dim itemm As Image = itm
                If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                    ruwid += itemm.Height
                Else
                    ruwid += itemm.Width
                End If
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
                        iconlist.Add({"icon", Iconn.Split("*")(0), Iconn.Split("*")(1), Iconn.Split("*")(2)})
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
                        iconlist.Add({"icon", Path.Replace(":", "{BD-TD-}"), icopack.GetValue("IconPaths", n).Replace("{Budgie.BDock.ConfigDirectory}", My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\").Replace(":", "{BD-TD-}"), n.Replace(":", "{BD-TD-}")})
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
                    iconlist.Add({"icon", Path.Replace(":", "{BD-TD-}"), My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + Path.Replace(":", "+").Replace("\", "+").Replace(":", "{BD-TD-}") + ".png", n.Replace(":", "{BD-TD-}")})
                ElseIf findico Then
                    iconlist.Add({"icon", Path.Replace(":", "{BD-TD-}"), "pack://application:,,,/Budgie%20Dock;component/unknown.png".Replace(":", "{BD-TD-}"), n.Replace(":", "{BD-TD-}")})
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
