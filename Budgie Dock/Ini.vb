Imports System.IO
Public Class Ini

    Private _Sections As New Dictionary(Of String, Dictionary(Of String, String))
    Private _FileName As String
    ''' <summary>
    ''' </summary>
    ''' <param name="IniFileName">Drive,Path and Filname for the inifile</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal IniFileName As String)

        Dim Rd As StreamReader
        Dim Content As String
        Dim Lines() As String
        Dim Line As String
        Dim Key As String
        Dim Value As String
        Dim SectionValues As Dictionary(Of String, String) = Nothing
        Dim Name As String

        _FileName = IniFileName

        'check if the file exists
        If Not File.Exists(IniFileName) Then
            Throw New FileLoadException(String.Format("The file {0} is not found", IniFileName))
        Else
            'Read the file if present.
            Rd = New StreamReader(IniFileName)
            Content = Rd.ReadToEnd
            'Split It into lines
            Lines = Content.Split(vbCrLf)

            'Place the content in an object sructure
            For Each Line In Lines

                'Trim the line
                Line = Line.Trim
                If Line.Length <= 2 OrElse Line.Substring(0, 1) = "'" OrElse Line.Substring(0, 3).ToUpper = "REM" Then
                    'There's no valid data or it's commented out... Do nothing 
                ElseIf Line.IndexOf("[") = 0 AndAlso Line.IndexOf("]") = Line.Length - 1 Then
                    'We hit a section
                    Name = Line.Replace("]", String.Empty).Replace("[", String.Empty).Trim.ToUpper
                    SectionValues = New Dictionary(Of String, String)
                    _Sections.Add(Name.ToUpper, SectionValues)

                    'An = character as the firstcharacter is an invalid line... let's be relaxed an just ignore it.
                ElseIf Line.IndexOf("=") > 0 AndAlso SectionValues IsNot Nothing Then
                    'We hit a value line , empty line or out commented line
                    'we don't use split as that character could be part of the value as well
                    Key = Line.Substring(0, Line.IndexOf("=")).Trim
                    If Line.IndexOf("=") = Line.Length - 1 Then
                        Value = String.Empty
                    Else
                        Value = Line.Substring(Line.IndexOf("=") + 1, Line.Length - (Line.IndexOf("=") + 1)).Trim
                    End If
                    'Add the valu to 
                    SectionValues.Add(Key.ToUpper, Value)
                End If
            Next

            Rd.Close()
            Rd.Dispose()
            Rd = Nothing

        End If
    End Sub
    Function GetFilePath()
        Return _FileName
    End Function
    Public Function GetValue(ByVal Section As String, ByVal Name As String) As String

        If _Sections.ContainsKey(Section.ToUpper) Then
            Dim SectionValues As Dictionary(Of String, String) = Nothing
            SectionValues = _Sections(Section.ToUpper)
            If SectionValues.ContainsKey(Name.ToUpper) Then
                Return SectionValues(Name.ToUpper)
            Else
                Return "Code_Item.NotFound"
            End If
        Else
            Return "Code_Item.NotFound"
        End If

        Return Nothing 'if preferred return String.empty here

    End Function

    Public Function SetValue(ByVal Section As String, ByVal Name As String, ByVal Value As String, Optional ByVal Save As Boolean = False) As Boolean
        Dim SectionValues As Dictionary(Of String, String) = Nothing
        Name = Name.ToUpper.Trim
        Section = Section.ToUpper.Trim
        If _Sections.ContainsKey(Section) Then
            SectionValues = _Sections(Section)
            If SectionValues.ContainsKey(Name) Then
                SectionValues.Remove(Name)
            End If
            SectionValues.Add(Name, Value)
        Else
            SectionValues = New Dictionary(Of String, String)
            _Sections.Add(Section, SectionValues)
            SectionValues.Add(Name, Value)
        End If

        If Save Then
            Return SaveIniFile()
        Else
            Return True
        End If

    End Function

    ''' <summary>
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Saving the inifile will remove all comments and invalid lines and all sections and valuenames will be in capitals.</remarks>
    Public Function SaveIniFile() As Boolean

        Dim Rw As StreamWriter
        Dim SectionPair As KeyValuePair(Of String, Dictionary(Of String, String))
        Dim ValuePair As KeyValuePair(Of String, String)

        Dim Pth As String = Path.GetDirectoryName(_FileName)

        If Directory.Exists(Pth) Then
            Rw = New StreamWriter(_FileName, False)
            For Each SectionPair In _Sections
                Rw.WriteLine("[" & SectionPair.Key & "]")
                If SectionPair.Value IsNot Nothing Then
                    For Each ValuePair In SectionPair.Value
                        Rw.WriteLine(ValuePair.Key & "=" & ValuePair.Value)
                    Next
                End If
            Next
            Rw.WriteLine("")
            Rw.Flush()
            Rw.Close()
            Rw.Dispose()
            Rw = Nothing
            SaveIniFile = True
        End If
        Return True
    End Function

    Function DeleteValue(ByVal Section As String, ByVal Name As String, Optional ByVal Save As Boolean = False) As Boolean

        Dim SectionValues As Dictionary(Of String, String) = Nothing

        Name = Name.ToUpper.Trim
        Section = Section.ToUpper.Trim
        If _Sections.ContainsKey(Section) Then
            SectionValues = _Sections(Section)
            If SectionValues.ContainsKey(Name) Then
                SectionValues.Remove(Name)
            End If
        End If

        If Save Then
            Return SaveIniFile()
        Else
            Return True
        End If

    End Function

    Function DeleteSection(ByVal Section As String, Optional ByVal Save As Boolean = False) As Boolean

        Dim SectionValues As Dictionary(Of String, String) = Nothing

        Section = Section.ToUpper.Trim
        If _Sections.ContainsKey(Section) Then
            _Sections.Remove(Section)
        End If

        If Save Then
            Return SaveIniFile()
        Else
            Return True
        End If

    End Function


End Class