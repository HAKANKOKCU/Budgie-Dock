Imports System.Windows.Threading
Imports System.Runtime.InteropServices
Imports System.ComponentModel

Public Class iconobj
    <DllImport("user32.dll", EntryPoint:="SetForegroundWindow")> _
    Private Shared Function SetForegroundWindow(ByVal hWnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    Private Declare Auto Function IsIconic Lib "user32.dll" (ByVal hwnd As IntPtr) As Boolean
    Private Sub ActivateApp(ByVal aid As Integer)

        'Minimize Window
        SendMessage(runproc.MainWindowHandle,
          WM_SYSCOMMAND, SC_MINIMIZE, CType(0, IntPtr))

        'Restore Window
        SendMessage(runproc.MainWindowHandle,
          WM_SYSCOMMAND, SC_RESTORE, CType(0, IntPtr))
    End Sub

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As UInteger, ByVal lParam As IntPtr) As IntPtr
    End Function

    Const WM_SYSCOMMAND As UInt32 = &H112
    Const SC_RESTORE As UInt32 = &HF120
    Const SC_MAXIMIZE As UInt32 = &HF030
    Const SC_MINIMIZE As UInt32 = &HF020

    Dim alreadyadded As Boolean = False
    Public WithEvents imageiconobj As New Image
    Public WithEvents isapopen As New Grid
    Public WithEvents tick As New DispatcherTimer
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
    Public hr = False
    Public isopen = False
    Public isprocfound = False
    Private fi As IO.FileInfo
    Property runid As Integer = 0
    Property checkIfRuning As Boolean = True
    Property isEditingAvable As Boolean = True
    Dim prcrem As procremove
    Dim aid As Integer
    Public WithEvents runBG As BackgroundWorker
    Public WithEvents waitBG As BackgroundWorker
    Dim anispeed = 10
    Sub endinit()
        Try
            Dim img As New BitmapImage(New Uri(iconpath))
            imageiconobj.Source = img
        Catch
        End Try
        imageiconobj.Focusable = True
        imageiconobj.Width = My.Settings.Size
        Try
            imageiconobj.Height = My.Settings.Size - 5
        Catch
        End Try
        imageiconobj.ClipToBounds = True
        isapopen.Width = My.Settings.Size / 3
        isapopen.Margin = New Thickness(My.Settings.Size / 3, 0, My.Settings.Size / 3, 0)
        isapopen.Height = 3
        isapopen.Background = Brushes.Transparent
        isapopen.ClipToBounds = True
        tick.Interval = TimeSpan.FromMilliseconds(2000)
        Try
            fi = New IO.FileInfo(apppath)
        Catch
        End Try
        tick.Start()
        If Not alreadyadded Then
            stackpanel.Children.Add(imageiconobj)
            containerwin.isappopen.Children.Add(isapopen)
            alreadyadded = True
        End If
        animater = New DispatcherTimer
        animater.Interval = TimeSpan.FromMilliseconds(1)
        If Not isEditingAvable Then
            If hr Then
                prcrem = New procremove
                prcrem.process = runproc
                waitBG = New BackgroundWorker
                waitBG.RunWorkerAsync()
                animater.Start()
            End If
        Else
            runBG = New BackgroundWorker
            runBG.WorkerReportsProgress = True
            runBG.WorkerSupportsCancellation = True
        End If
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
                If Not My.Settings.animatescale = 1 Then animater.Start()
            Catch
                runBG.CancelAsync()
            End Try
        ElseIf e.Key = Key.Enter Then
            If Not isopen Then
                Try
                    runBG.RunWorkerAsync()
                    imageiconobj.Opacity = 0.1
                    If Not My.Settings.animatescale = 1 Then animater.Start()
                Catch
                    runBG.CancelAsync()
                End Try
            Else
                ActivateApp(runproc.Id)
            End If
        End If
    End Sub

    Private Sub imageiconobj_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles imageiconobj.KeyUp
        If e.Key = Key.Space Then
            imageiconobj.Opacity = 1
            If Not isopen Then
                Try
                    runBG.RunWorkerAsync()
                    imageiconobj.Opacity = 0.1
                    If Not My.Settings.animatescale = 1 Then animater.Start()
                Catch
                    runBG.CancelAsync()
                End Try
            Else
                ActivateApp(runproc.Id)
            End If
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
            Canvas.SetLeft(containerwin.appname, System.Windows.Forms.Control.MousePosition.X - containerwin.Left - (containerwin.appname.ActualWidth / 2))
            containerwin.appname.Visibility = Visibility.Visible
        End If
        If hr And Not isEditingAvable Then
            Try
                If runproc.MainWindowTitle = "" Then
                    'containerwin.aaps.Remove(runproc.Id)
                    'If Not isremoved Then containerwin.ruwid -= My.Settings.Size
                    Try
                        If Not isremoved Then remove()
                    Catch
                    End Try
                End If
            Catch
            End Try
        End If
    End Sub

    Private Sub imageiconobj_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles imageiconobj.MouseLeave
        If isEditingAvable Then
            If runBG.IsBusy Then
                imageiconobj.Opacity = 0.1
            Else
                imageiconobj.Opacity = 1
            End If
        Else
            imageiconobj.Opacity = 1
        End If
        containerwin.appname.Visibility = Visibility.Hidden
    End Sub

    Private Sub imageiconobj_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles imageiconobj.MouseMove
        If Not containerwin.menustack.Visibility = Visibility.Visible Or containerwin.OptionsIcon Is Me Then Canvas.SetLeft(containerwin.appname, System.Windows.Forms.Control.MousePosition.X - containerwin.Left - (containerwin.appname.ActualWidth / 2))
    End Sub

    Private Sub imageiconobj_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles imageiconobj.MouseUp
        imageiconobj.Opacity = 1
        If e.ChangedButton = 0 Then
            If Not isopen Then
                Try
                    runBG.RunWorkerAsync()
                    imageiconobj.Opacity = 0.1
                    If Not My.Settings.animatescale = 1 Then animater.Start()
                Catch
                    runBG.CancelAsync()
                End Try
            Else
                ActivateApp(runproc.Id)
            End If
        ElseIf e.ChangedButton = 2 And isEditingAvable Then
            containerwin.menustack.Visibility = Visibility.Visible
            Canvas.SetLeft(containerwin.menustack, System.Windows.Forms.Control.MousePosition.X - containerwin.Left - 100)
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
        If isremoved Then Exit Sub
        iconpath = ""
        apppath = ""
        Dim img As New BitmapImage()
        imageiconobj.Source = img
        imageiconobj.Visibility = Visibility.Collapsed
        imageiconobj.Width = 0
        containerwin.OptionsIcon = Nothing
        containerwin.menustack.Visibility = Visibility.Hidden
        stackpanel.Children.Remove(imageiconobj)
        containerwin.isappopen.Children.Remove(isapopen)
        imageiconobj = Nothing
        isremoved = True
        tick.Stop()
        If Not isEditingAvable Then
            containerwin.aaps.Remove(aid)
            containerwin.refopenapps(False)
        End If
        If containerwin.runingapps.Children.Count = 1 Then
            containerwin.runingapps.Children.Clear()
        End If
    End Sub

    Private Sub tick_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tick.Tick
        If checkIfRuning Then
            isopen = False
            isapopen.Background = Brushes.Transparent
            If Not fi.Extension = "" Then
                For Each prc As Process In Process.GetProcesses
                    If prc.ProcessName.ToLower = My.Computer.FileSystem.GetName(apppath).Replace(fi.Extension, "").ToLower Then
                        isapopen.Background = Brushes.White
                        isopen = True
                        If hr Then If runproc.HasExited Then isprocfound = False
                        If Not isprocfound Then If Not prc.MainWindowTitle = "" Then runproc = prc
                    End If
                Next
            End If
            If runBG.IsBusy Then
                imageiconobj.Opacity = 0.1
            End If
        Else
            If hr Then
                Try
                    If Not runproc.HasExited Then
                        Dim idd = runproc.Id
                        aid = idd
                        Dim prc = Process.GetProcessById(idd)
                        If runproc.ProcessName = prc.ProcessName And Not prc.ProcessName = "" Then
                            runproc.Refresh()
                            runproc = Process.GetProcessById(idd)
                        End If
                    Else
                        If Not isremoved Then remove()
                    End If
                Catch
                End Try
            End If
            Try
                If runproc.HasExited Then
                    Try
                        If Not isremoved Then remove()
                    Catch
                    End Try
                Else
                    appname = runproc.MainWindowTitle
                End If
            Catch
            End Try
            'If runproc.MainWindowTitle = "" Then
            'containerwin.aaps.Remove(runproc.Id)
            'If Not isremoved Then containerwin.ruwid -= My.Settings.Size
            'Try
            'remove()
            'Catch
            'End Try
            'End If
        End If
        If Not containerwin.rid = runid Then
            If isEditingAvable Then
                tick.Stop()
                Try
                    If Not isremoved Then remove()
                Catch
                End Try
            End If
        End If
    End Sub
    Private Sub ActivateAppl(ByVal pID As Integer)
        Dim p As Process = Process.GetProcessById(pID)
        If p IsNot Nothing Then
            SetForegroundWindow(p.MainWindowHandle)
        End If
    End Sub

    Private Sub runproc_Exited(ByVal sender As Object, ByVal e As System.EventArgs) Handles runproc.Exited
        isapopen.Background = Brushes.Transparent
        isprocfound = False
    End Sub

    Private Sub isapopen_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles isapopen.MouseEnter
        If checkIfRuning Then
            isapopen.Background = Brushes.Lime
        Else
            isapopen.Background = Brushes.Red
        End If
    End Sub

    Private Sub isapopen_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles isapopen.MouseLeave
        If isEditingAvable Then
            isapopen.Background = Brushes.White
        Else
            isapopen.Background = Brushes.Transparent
        End If
    End Sub

    Private Sub isapopen_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles isapopen.MouseUp
        If checkIfRuning Then
            Try
                runBG.RunWorkerAsync()
                imageiconobj.Opacity = 0.1
                If Not My.Settings.animatescale = 1 Then animater.Start()
            Catch
                runBG.CancelAsync()
            End Try
        Else
            Try
                runproc.Kill()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
            End Try
        End If
    End Sub

    Private Sub runBG_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles runBG.DoWork
        Dim prc = Process.Start(apppath, apparams)
        runBG.ReportProgress(50, prc)
        Try
            If Not runBG.CancellationPending Then
                prc.WaitForInputIdle()
            End If
        Catch
        End Try
    End Sub

    Private Sub runBG_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles runBG.ProgressChanged
        runproc = e.UserState
        hr = True
        isprocfound = True
    End Sub

    Private Sub runBG_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles runBG.RunWorkerCompleted
        imageiconobj.Opacity = 1
        animater.Stop()
    End Sub

    Private Sub animater_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles animater.Tick
        If imageiconobj.Opacity = 1 Then
            anispeed = -10
        ElseIf imageiconobj.Opacity < 0.1 Then
            anispeed = 10
        End If
        imageiconobj.Opacity += anispeed / 1000
    End Sub

    Private Sub waitBG_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles waitBG.DoWork
        Try
            runproc.WaitForInputIdle()
        Catch
        End Try
    End Sub

    Private Sub waitBG_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles waitBG.RunWorkerCompleted
        animater.Stop()
    End Sub
End Class
