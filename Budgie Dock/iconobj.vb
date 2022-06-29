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
    Sub endinit()
        Try
            Dim img As New BitmapImage(New Uri(iconpath))
            imageiconobj.Source = img
        Catch
        End Try
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
        If Not isEditingAvable Then
            If hr Then
                prcrem = New procremove
                prcrem.processName = runproc.ProcessName
            End If
        Else
            runBG = New BackgroundWorker
        End If
    End Sub

    Private Sub imageiconobj_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles imageiconobj.MouseDown
        imageiconobj.Opacity = 0.5
        containerwin.menustack.Visibility = Visibility.Hidden
    End Sub

    Private Sub imageiconobj_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles imageiconobj.MouseEnter
        imageiconobj.Opacity = 0.8
        containerwin.appname.Content = appname
        containerwin.appname.UpdateLayout()
        Canvas.SetLeft(containerwin.appname, System.Windows.Forms.Control.MousePosition.X - containerwin.Left - (containerwin.appname.ActualWidth / 2))
        containerwin.appname.Visibility = Visibility.Visible
        If hr And Not isEditingAvable Then
            Try
                If runproc.MainWindowTitle = "" Then
                    containerwin.aaps.Remove(runproc.Id)
                    'If Not isremoved Then containerwin.ruwid -= My.Settings.Size
                    Try
                        remove()
                    Catch
                    End Try
                End If
            Catch
            End Try
        End If
    End Sub

    Private Sub imageiconobj_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles imageiconobj.MouseLeave
        imageiconobj.Opacity = 1
        containerwin.appname.Visibility = Visibility.Hidden
    End Sub

    Private Sub imageiconobj_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles imageiconobj.MouseMove
        Canvas.SetLeft(containerwin.appname, System.Windows.Forms.Control.MousePosition.X - containerwin.Left - (containerwin.appname.ActualWidth / 2))
    End Sub

    Private Sub imageiconobj_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles imageiconobj.MouseUp
        imageiconobj.Opacity = 1
        If e.ChangedButton = 0 Then
            If Not isopen Then
                runBG.RunWorkerAsync()
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
        iconpath = ""
        apppath = ""
        Dim img As New BitmapImage()
        imageiconobj.Source = img
        imageiconobj.Visibility = Visibility.Collapsed
        imageiconobj.Width = 1
        imageiconobj = Nothing
        containerwin.OptionsIcon = Nothing
        containerwin.menustack.Visibility = Visibility.Hidden
        stackpanel.Children.Remove(imageiconobj)
        containerwin.isappopen.Children.Remove(isapopen)
        isremoved = True
        tick.Stop()
        If containerwin.isappopen.Children.Count <= 3 Then
            containerwin.isappopen.Children.Clear()
        End If
        If Not isEditingAvable Then
            containerwin.aaps.Remove(aid)
            containerwin.refopenapps(False)
            containerwin.ruwid -= My.Settings.Size
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
                        runproc.Refresh()
                        runproc = Process.GetProcessById(idd)
                    Else
                        If Not isremoved Then remove()
                    End If
                Catch
                End Try
            End If
            Try
                If runproc.HasExited Then
                    containerwin.aaps.Remove(runproc.Id)
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
        'If Not checkIfRuning Then
        'containerwin.aaps.Remove(runproc.Id)
        'End If
        'If Not isremoved Then containerwin.ruwid -= My.Settings.Size
        'Try
        'If Not isremoved Then remove()
        'Catch
        'End Try
    End Sub

    Private Sub isapopen_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles isapopen.MouseEnter
        If checkIfRuning Then
            isapopen.Background = Brushes.Lime
        Else
            isapopen.Background = Brushes.Red
        End If
    End Sub

    Private Sub isapopen_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles isapopen.MouseLeave
        isapopen.Background = Brushes.White
    End Sub

    Private Sub isapopen_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles isapopen.MouseUp
        If checkIfRuning Then
            runBG.RunWorkerAsync()
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
        e.Result = prc
    End Sub

    Private Sub runBG_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles runBG.RunWorkerCompleted
        imageiconobj.Opacity = 1
        runproc = e.Result
        hr = True
        isprocfound = True
    End Sub
End Class
