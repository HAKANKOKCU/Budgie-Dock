Imports System.Windows.Threading

Public Class RWidget
    Dim elemtree As New ArrayList
    Sub New(ByVal ExtFilePath As String, ByVal sp As StackPanel)
        Dim ff As String = My.Computer.FileSystem.ReadAllText(ExtFilePath)
        For Each ln As String In ff.Split(Environment.NewLine)
            Dim tbc As Integer = 0
            For Each tb As String In ln.Split("	  ")
                tbc += 1
            Next
            Dim obj = Split(ln, ":", 2)
            Dim iselemfound = False
            Dim elem = Nothing
            If obj(0).Trim = "Label" Then
                elem = New Label
                iselemfound = True
            ElseIf obj(0).Trim = "Image" Then
                elem = New Image
                iselemfound = True
            ElseIf obj(0).Trim = "StackPanel" Then
                elem = New StackPanel
                iselemfound = True
            End If
            If iselemfound Then
                If tbc = 1 Then
                    elemtree.Add(elem)
                Else
                    elemtree(tbc - 1).Children.Add(elem)
                End If
            Else
                If obj(0).Trim = "Width" Then
                    elemtree(tbc - 1).Width = obj(1)
                ElseIf obj(0).Trim = "Height" Then
                    elemtree(tbc - 1).Height = obj(1)
                ElseIf obj(0).Trim = "Content" Then
                    If TypeOf elemtree(tbc - 1) Is Label Then
                        elemtree(tbc - 1).Content = obj(1).Replace("%SystemHour%", Date.Now.Hour).Replace("%SystemMinute%", Date.Now.Minute).Replace("%SystemSecond%", Date.Now.Second)
                    ElseIf TypeOf elemtree(tbc - 1) Is Image Then
                        Dim bim As New BitmapImage
                        bim.BeginInit()
                        bim.UriSource = New Uri(obj(1))
                        bim.EndInit()
                        elemtree(tbc - 1).Source = bim
                    End If
                ElseIf obj(0).Trim = "RefreshingDataString" Then
                    referDataString(tbc - 1, obj(1).Split(":")(0), obj(1).Split(":")(1))
                End If
            End If
        Next
        For Each el As UIElement In elemtree
            sp.Children.Add(el)
        Next
    End Sub
    Sub referDataString(ByVal elemtreeid As Integer, ByVal path As String, ByVal refreshinterval As Integer)
        Dim dt As New DispatcherTimer
        dt.Interval = TimeSpan.FromMilliseconds(refreshinterval)
        dt.Start()
        AddHandler dt.Tick, Sub(sender, e)
                                Try
                                    If My.Computer.FileSystem.FileExists(path) Then elemtree(elemtreeid).Content = My.Computer.FileSystem.ReadAllText(path)
                                Catch
                                End Try
                            End Sub
    End Sub
End Class
