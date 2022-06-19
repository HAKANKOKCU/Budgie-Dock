Imports System.Windows.Threading

Class MainWindow
    Public OptionsIcon As icon
    Public WithEvents ticker As DispatcherTimer
    Public WithEvents animater As DispatcherTimer
    Public iconlist As New ArrayList
    Dim agwid = 0
    Dim iddd As Integer = 0
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.Height = appsgrid.Height + 106
        ticker = New DispatcherTimer
        ticker.Interval = TimeSpan.FromMilliseconds(10)
        ticker.Start()
        animater = New DispatcherTimer
        animater.Interval = TimeSpan.FromMilliseconds(1)
        animater.Start()
        My.Computer.FileSystem.CreateDirectory(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\")
        If Not My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data") Then
            Dim dd As String = ""
            My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data", dd, False)
        End If
        reicon()
        Dim dlbtn As New ButtonStack
        dlbtn.theStackPanel = DeleteIconButton
        Dim opbtn As New ButtonStack
        opbtn.theStackPanel = OptIconButton
        Dim optbtn As New ButtonStack
        optbtn.theStackPanel = OptMainButton
        If My.Settings.pos = "Bottom" Then
            mas.Orientation = Orientation.Vertical
            appsgrid.Orientation = Orientation.Horizontal
            appsgrid.Height = My.Settings.Size
        Else
            mas.Orientation = Orientation.Horizontal
            appsgrid.Orientation = Orientation.Vertical
            appsgrid.Width = My.Settings.Size
        End If
    End Sub

    Sub reicon()
        iddd = 0
        appsgrid.Children.Clear()
        iconlist.Clear()
        If My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data").Contains("*") Then
            For Each Iconn As String In My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons.data").Split("|")
                Dim a As New icon
                a.apppath = Iconn.Split("*")(0)
                a.iconpath = Iconn.Split("*")(1)
                a.appname = Iconn.Split("*")(2)
                a.stackpanel = appsgrid
                a.containerwin = Me
                a.endinit()
                a.idd = iddd
                iconlist.Add({Iconn.Split("*")(0), Iconn.Split("*")(1), Iconn.Split("*")(2)})
                iddd += 1
            Next
            Dim sizee As Integer = 0
            For Each a As UIElement In appsgrid.Children
                If TypeOf a Is Image Then
                    sizee += My.Settings.Size
                End If
            Next
            agwid = sizee
        Else
            Dim lbldrg As New Label
            lbldrg.Content = "Drag Here To Add Icons"
            appsgrid.Children.Add(lbldrg)
            agwid = 138
        End If
    End Sub

    Private Sub ticker_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ticker.Tick
        If My.Settings.pos = "Bottom" Then
            Me.Top = Forms.Screen.PrimaryScreen.WorkingArea.Height - My.Settings.Size - 310
        Else
            Me.Left = Forms.Screen.PrimaryScreen.WorkingArea.Width - Me.Width + 180
        End If
        Dim sizee As Integer = 0
        For Each a As UIElement In appsgrid.Children
            If TypeOf a Is Image Then
                sizee += My.Settings.Size
            End If
        Next
        If sizee < 200 Then
            sizee = 200
        End If
        If My.Settings.pos = "Bottom" Then
            Me.Width = sizee
            Me.Left = (Forms.Screen.PrimaryScreen.WorkingArea.Width / 2) - (Me.Width / 2)
        Else
            Me.Height = sizee
            Me.Top = (Forms.Screen.PrimaryScreen.WorkingArea.Height / 2) - (Me.Height / 2)
        End If
        'Console.WriteLine(Me.Left)
        'Console.WriteLine(Me.Top)
        Me.WindowState = Windows.WindowState.Normal
        Me.Show()
        Try
            If menustack.Visibility = Windows.Visibility.Visible Then
                appname.Content = OptionsIcon.appname
                appname.Visibility = Windows.Visibility.Visible
            End If
        Catch
        End Try
    End Sub

    Private Sub DeleteIconButton_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles DeleteIconButton.MouseUp
        iconlist.RemoveAt(OptionsIcon.idd)
        'MsgBox(OptionsIcon.idd)
        menustack.Visibility = Windows.Visibility.Hidden
        appname.Visibility = Windows.Visibility.Hidden
        savicon()
        reicon()
    End Sub

    Private Sub MainWindow_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        savicon()
    End Sub

    Sub savicon()
        Dim filecontent As String = ""
        Dim firstone As Boolean = True
        For Each a As Array In iconlist
            If Not firstone Then
                filecontent += "|"
            End If
            filecontent += a(0) & "*" & a(1) & "*" & a(2)
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
    End Sub

    Private Sub appsgrid_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles appsgrid.DragEnter
        e.Effects = DragDropEffects.Link
    End Sub

    Private Sub appsgrid_Drop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles appsgrid.Drop
        'If e.Data.GetDataPresent(e.Data.GetDataPresent(DataFormats.FileDrop)) Then
        Try
            For Each Path As String In e.Data.GetData(DataFormats.FileDrop)
                Dim a As New icon
                a.apppath = Path
                a.iconpath = "pack://application:,,,/Budgie%20Dock;component/unknown.png"
                a.appname = My.Computer.FileSystem.GetName(Path)
                a.stackpanel = appsgrid
                a.containerwin = Me
                a.endinit()
                a.idd = iddd
                iconlist.Add({Path, "pack://application:,,,/Budgie%20Dock;component/unknown.png", My.Computer.FileSystem.GetName(Path)})
                iddd += 1
            Next
            savicon()
            reicon()
            'End If
        Catch
            appname.Visibility = Windows.Visibility.Visible
            appname.Content = "Can't Add Icon Of That Type"
        End Try
    End Sub

    Private Sub OptMainButton_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles OptMainButton.MouseUp
        Dim icop As New BDOptions
        menustack.Visibility = Visibility.Hidden
        icop.ShowDialog()
    End Sub

    Private Sub animater_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles animater.Tick
        appsgrid.Width += (agwid - appsgrid.Width) / 8
    End Sub

End Class
