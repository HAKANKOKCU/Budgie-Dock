﻿Imports System.Windows.Threading
Imports System.ComponentModel

Public Class iconobj

    Dim alreadyadded As Boolean = False
    Public WithEvents imageiconobj As New Image
    Public WithEvents isapopen As New Grid
    Property iconpath As String = ""
    Property apppath As String = ""
    Property stackpanel As StackPanel
    Property containerwin As MainWindow
    Property idd As Integer = Nothing
    Property isremoved As Boolean = False
    Property appname As String = ""
    Property apparams As String = ""
    Public WithEvents runproc As Process
    Public WithEvents animater As DispatcherTimer
    Public WithEvents tick As New DispatcherTimer
    Public WithEvents animeRemoveLM As DispatcherTimer
    Public hr = False
    Public isopen = False
    Public isprocfound = False
    Private fi As IO.FileInfo = Nothing
    Property runid As Integer = 0
    Property checkIfRuning As Boolean = True
    Property isEditingAvable As Boolean = True
    Dim prcrem As procremove
    Dim aid As Integer
    Public WithEvents runBG As BackgroundWorker
    Public WithEvents waitBG As BackgroundWorker
    Dim anispeed = 10
    Property winPTR
    Sub endinit()
        Try
            Try
                Dim img As New BitmapImage
                img.BeginInit()
                img.CacheOption = BitmapCacheOption.OnDemand
                img.UriSource = New Uri(iconpath)
                img.EndInit()
                img.Freeze()
                imageiconobj.Source = img
            Catch
            End Try
            imageiconobj.Focusable = True
            imageiconobj.Width = GetSetting("size")
            Try
                If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                    imageiconobj.Height = GetSetting("size")
                Else
                    imageiconobj.Height = GetSetting("size") - 5
                End If
            Catch
            End Try
            imageiconobj.ClipToBounds = True
            If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                isapopen.Height = (GetSetting("size") / 3)
                isapopen.Margin = New Thickness(0, (GetSetting("size") / 3), 0, (GetSetting("size") / 3))
                isapopen.Width = 3
            Else
                isapopen.Width = (GetSetting("size") / 3)
                isapopen.Margin = New Thickness((GetSetting("size") / 3), 0, (GetSetting("size") / 3), 0)
                isapopen.Height = 3
            End If
            isapopen.Background = Brushes.Transparent
            isapopen.ClipToBounds = True
            tick.Interval = TimeSpan.FromMilliseconds(2000)
            Try
                If My.Computer.FileSystem.FileExists(apppath) Then fi = New IO.FileInfo(apppath)
            Catch
            End Try
            tick.Start()
            If Not alreadyadded Then
                stackpanel.Children.Add(imageiconobj)
                If stackpanel Is containerwin.rdc Then
                    containerwin.isappopenr.Children.Add(isapopen)
                Else
                    containerwin.isappopen.Children.Add(isapopen)
                End If
                If containerwin.lriID = idd Then
                    If Not idd = Nothing Then
                        imageiconobj.Margin = New Thickness(GetSetting("size"), 0, 0, 0)
                        animeRemoveLM = New DispatcherTimer
                        animeRemoveLM.Interval = TimeSpan.FromMilliseconds(1)
                        animeRemoveLM.Start()
                    End If
                Else
                    imageiconobj.Margin = New Thickness(0)

                End If
                alreadyadded = True
            End If
            animater = New DispatcherTimer
            animater.Interval = TimeSpan.FromMilliseconds(1)
            If Not isEditingAvable Then
                If hr Then
                    prcrem = New procremove
                    prcrem.process = runproc
                    prcrem.hw = winPTR
                    waitBG = New BackgroundWorker
                    waitBG.RunWorkerAsync()
                    animater.Start()
                End If
            Else
                runBG = New BackgroundWorker
                runBG.WorkerReportsProgress = True
                runBG.WorkerSupportsCancellation = True
            End If
        Catch ex As Exception
            insertToLog(ex.ToString)
        End Try
    End Sub

    Private Sub imageiconobj_GotFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles imageiconobj.GotFocus
        imageiconobj.Opacity = 0.8
        If isEditingAvable Then
            If runBG.IsBusy Then
                imageiconobj.Opacity = 0.1
            Else
                imageiconobj.Opacity = 0.8
            End If
        Else
            imageiconobj.Opacity = 0.8
        End If
        If Not containerwin.menustack.Visibility = Visibility.Visible Then
            containerwin.appname.Content = appname
            containerwin.appname.UpdateLayout()
            Canvas.SetLeft(containerwin.appname, (containerwin.Width / 2) - (containerwin.appname.ActualWidth / 2))
            containerwin.appname.Visibility = Visibility.Visible
        End If
    End Sub

    Private Sub imageiconobj_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles imageiconobj.KeyDown
        If e.Key = Key.Space Then
            imageiconobj.Opacity = 0.5
        ElseIf e.Key = Key.N Then
            Try
                runBG.RunWorkerAsync()
                imageiconobj.Opacity = 0.1
                If Not GetSetting("animateScale") = 1 Then animater.Start()
            Catch
                runBG.CancelAsync()
            End Try
        ElseIf e.Key = Key.Enter Then
            runapp()
        End If
    End Sub

    Private Sub imageiconobj_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles imageiconobj.KeyUp
        If e.Key = Key.Space Then
            imageiconobj.Opacity = 1
            runapp()
        End If
    End Sub

    Private Sub imageiconobj_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles imageiconobj.LostFocus
        imageiconobj.Opacity = 1
    End Sub

    Private Sub imageiconobj_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles imageiconobj.MouseDown
        imageiconobj.Opacity = 0.5
        containerwin.menustack.Visibility = Visibility.Hidden
    End Sub

    Private Sub imageiconobj_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles imageiconobj.MouseEnter
        If isEditingAvable Then
            If Not runBG.IsBusy Then
                imageiconobj.Opacity = 0.8
            End If
        Else
            imageiconobj.Opacity = 0.8
        End If
        If hr And Not isEditingAvable Then
            Try
                If Not arWinnameFromDict.ContainsKey(winPTR) Then
                    remove()
                    Exit Try
                End If
                If arWinnameFromDict(winPTR) = "" Then
                    'containerwin.aaps.Remove(runproc.Id)
                    'If Not isremoved Then containerwin.ruwid -= My.Settings.Size
                    remove()
                End If
            Catch
            End Try
        End If
        If Not isremoved Then
            If Not containerwin.menustack.Visibility = Visibility.Visible Then
                containerwin.appname.Content = appname
                containerwin.appname.UpdateLayout()
                If GetSetting("pos") = "Right" Then
                    Canvas.SetTop(containerwin.appname, ((System.Windows.Forms.Control.MousePosition.Y / containerwin.SystemDPI) - 30))
                    Canvas.SetRight(containerwin.appname, 0)
                ElseIf GetSetting("pos") = "Left" Then
                    Canvas.SetTop(containerwin.appname, ((System.Windows.Forms.Control.MousePosition.Y / containerwin.SystemDPI) - 30))
                    Canvas.SetLeft(containerwin.appname, 0)
                Else
                    Canvas.SetLeft(containerwin.appname, ((System.Windows.Forms.Control.MousePosition.X / containerwin.SystemDPI) - (containerwin.appname.ActualWidth / 2)))
                End If
                containerwin.appname.Visibility = Visibility.Visible
            End If
        End If
    End Sub

    Private Sub imageiconobj_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles imageiconobj.MouseLeave
        If isEditingAvable Then
            If Not runBG.IsBusy Then
                imageiconobj.Opacity = 1
            End If
        Else
            imageiconobj.Opacity = 1
        End If
        containerwin.appname.Visibility = Visibility.Hidden
    End Sub

    Private Sub imageiconobj_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles imageiconobj.MouseMove
        If Not containerwin.menustack.Visibility = Visibility.Visible Or containerwin.OptionsIcon Is Me Then
            If GetSetting("pos") = "Right" Or GetSetting("pos") = "Left" Then
                Canvas.SetTop(containerwin.appname, ((System.Windows.Forms.Control.MousePosition.Y / containerwin.SystemDPI) - (containerwin.appname.ActualHeight / 2)))
            Else
                Canvas.SetLeft(containerwin.appname, (((System.Windows.Forms.Control.MousePosition.X - containerwin.MILeft) / containerwin.SystemDPI) - (containerwin.appname.ActualWidth / 2)))
            End If
        End If
    End Sub

    Sub runapp()
        If Not apppath.StartsWith("!") Then
            If Not isopen Then
                Try
                    runBG.RunWorkerAsync()
                    imageiconobj.Opacity = 0.1
                    If Not GetSetting("animateScale") = 1 Then animater.Start()
                Catch
                    runBG.CancelAsync()
                End Try
            Else
                ActivateApp(winPTR)
            End If
        Else
            showSpecialDiag(apppath)
        End If
    End Sub

    Private Sub imageiconobj_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles imageiconobj.MouseUp
        imageiconobj.Opacity = 1
        If e.ChangedButton = 0 Then
            runapp()
        ElseIf e.ChangedButton = 2 And isEditingAvable Then
            containerwin.menustack.Visibility = Visibility.Visible
            If GetSetting("pos") = "Right" Then
                'Canvas.SetTop(containerwin.appname, ((System.Windows.Forms.Control.MousePosition.Y / containerwin.SystemDPI) - 30))
                Canvas.SetTop(containerwin.menustack, (System.Windows.Forms.Control.MousePosition.Y / containerwin.SystemDPI) - 80)
                Canvas.SetRight(containerwin.menustack, 0)
            ElseIf GetSetting("pos") = "Left" Then
                Canvas.SetTop(containerwin.menustack, (System.Windows.Forms.Control.MousePosition.Y / containerwin.SystemDPI) - 80)
                Canvas.SetRight(containerwin.menustack, 0)
            Else
                Canvas.SetLeft(containerwin.menustack, ((System.Windows.Forms.Control.MousePosition.X + containerwin.MILeft) / containerwin.SystemDPI) - 100)
            End If
            containerwin.OptionsIcon = Me
        ElseIf e.ChangedButton = 2 And Not isEditingAvable Then
            prcrem.hide()
        End If
    End Sub

    Private Sub imageiconobj_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Input.MouseWheelEventArgs) Handles imageiconobj.MouseWheel
        If e.Delta > 0 Then
            containerwin.menustack.Visibility = Visibility.Visible
            containerwin.OptionsIcon = Me
        Else
            containerwin.menustack.Visibility = Visibility.Hidden
        End If
    End Sub

    Sub remove()
        Try
            If isremoved Then Exit Sub
            iconpath = ""
            apppath = ""
            Dim img As New BitmapImage()
            imageiconobj.Source = img
            containerwin.OptionsIcon = Nothing
            containerwin.menustack.Visibility = Visibility.Hidden
            If stackpanel Is containerwin.runingapps Then
                containerwin.aaps.Remove(winPTR)
            Else
                'containerwin.lriID = idd
            End If
            stackpanel.Children.Remove(imageiconobj)
            containerwin.sizecalc()
            containerwin.isappopen.Children.Remove(isapopen)
            imageiconobj = Nothing
            isremoved = True
            tick.Stop()
            If containerwin.runingapps.Children.Count = 1 Then
                containerwin.runingapps.Children.Clear()
            End If
        Catch ex As Exception
            insertToLog(ex.ToString)
        End Try
    End Sub

    Sub TickLoop() Handles tick.Tick
        'Try
        If checkIfRuning Then
            isopen = False
            isapopen.Background = Brushes.Transparent
            If fi Is Nothing Then Exit Sub
            If Not fi.Exists Then Exit Sub
            If Not fi.Extension = "" Then
                For Each prc As Process In containerwin.proclist
                    If prc.ProcessName.ToLower = My.Computer.FileSystem.GetName(apppath).Replace(fi.Extension, "").ToLower Then
                        If Not prc.MainWindowTitle = "" And Not prc.MainWindowHandle = IntPtr.Zero Then
                            Try
                                If prc.MainModule.FileName.ToLower = apppath.ToLower Then
                                    isapopen.Background = New SolidColorBrush(Color.FromArgb(255, GetSetting("isAppRuningRed"), GetSetting("isAppRuningGreen"), GetSetting("isAppRuningBlue")))
                                    isopen = True
                                    Try
                                        If hr Then If runproc.HasExited Then isprocfound = False
                                        If Not isprocfound Then If Not prc.MainWindowTitle = "" Then runproc = prc
                                    Catch
                                    End Try
                                    Exit For
                                End If
                            Catch
                                isapopen.Background = New SolidColorBrush(Color.FromArgb(255, GetSetting("isAppRuningRed"), GetSetting("isAppRuningGreen"), GetSetting("isAppRuningBlue")))
                                isopen = True
                                Try
                                    If hr Then If runproc.HasExited Then isprocfound = False
                                    If Not isprocfound Then If Not prc.MainWindowTitle = "" Then runproc = prc
                                Catch
                                End Try
                                Exit For
                            End Try
                        End If
                    End If
                Next
            End If
        Else
            If hr Then
                Try
                    If Not runproc.HasExited Then
                        aid = runproc.Id
                        Dim prc = Process.GetProcessById(aid)
                        If runproc.ProcessName = prc.ProcessName Then
                            runproc.Refresh()
                            runproc = prc
                        End If
                    Else
                        remove()
                    End If
                Catch ex As Exception
                    insertToLog(ex.ToString)
                End Try
            End If
            Try
                appname = arWinnameFromDict(winPTR)
                'If runproc.HasExited Then
                'remove()
                'End If
            Catch
            End Try
            If Not arWinnameFromDict.ContainsKey(winPTR) Then
                remove()
            End If
        End If
        If Not containerwin.rid = runid Then
            'tick.Stop()
            If stackpanel Is containerwin.appsgrid Then remove()
        End If
        'Catch ex As Exception
        'insertToLog(ex.ToString)
        'End Try
    End Sub

    'Private Sub runproc_Exited(ByVal sender As Object, ByVal e As System.EventArgs) Handles runproc.Exited
    '    isapopen.Background = Brushes.Transparent
    '    isprocfound = False
    'End Sub

    Private Sub isapopen_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles isapopen.MouseEnter
        If checkIfRuning Then
            isapopen.Background = Brushes.Lime
        Else
            isapopen.Background = Brushes.Red
        End If
    End Sub

    Private Sub isapopen_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles isapopen.MouseLeave
        If isEditingAvable Then
            isapopen.Background = New SolidColorBrush(Color.FromArgb(255, GetSetting("isAppRuningRed"), GetSetting("isAppRuningGreen"), GetSetting("isAppRuningBlue")))
        Else
            isapopen.Background = Brushes.Transparent
        End If
    End Sub

    Private Sub isapopen_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles isapopen.MouseUp
        If checkIfRuning Then
            Try
                runapp()
                'runBG.RunWorkerAsync()
                'imageiconobj.Opacity = 0.1
                'If Not GetSetting("animateScale") = 1 Then animater.Start()
            Catch
                runBG.CancelAsync()
            End Try
        Else
            Try
                runproc.CloseMainWindow()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
            End Try
        End If
    End Sub

    Private Sub runBG_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles runBG.DoWork
        Try
            Dim prc = Process.Start(apppath, apparams)
            runBG.ReportProgress(50, prc)
            Try
                If Not runBG.CancellationPending Then
                    prc.WaitForInputIdle()
                End If
            Catch
            End Try
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub runBG_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles runBG.ProgressChanged
        runproc = e.UserState
        hr = True
        isprocfound = True
    End Sub

    Private Sub runBG_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles runBG.RunWorkerCompleted
        Try
            imageiconobj.Opacity = 1
            animater.Stop()
        Catch
        End Try
    End Sub

    Private Sub animater_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles animater.Tick
        Try
            If imageiconobj.Opacity + (anispeed / 1000) >= 1 Or imageiconobj.Opacity + (anispeed / 1000) <= 0.1 Then
                anispeed = -anispeed
                imageiconobj.Opacity += anispeed / 1000
            Else
                imageiconobj.Opacity += anispeed / 1000
            End If
        Catch
            Try
                animater.Stop()
            Catch
            End Try
        End Try
    End Sub

    Private Sub waitBG_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles waitBG.DoWork
        Try
            runproc.WaitForInputIdle()
        Catch
        End Try
    End Sub

    Private Sub waitBG_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles waitBG.RunWorkerCompleted
        Try
            animater.Stop()
            imageiconobj.Opacity = 1
        Catch
        End Try
    End Sub

    Private Sub animeRemoveLM_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles animeRemoveLM.Tick
        imageiconobj.Margin = New Thickness((-imageiconobj.Margin.Left) / GetSetting("animateScale"), 0, 0, 0)
    End Sub
End Class
