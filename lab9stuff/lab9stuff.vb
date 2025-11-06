Imports System.IO.Ports
Imports System.Windows.Forms.VisualStyles.VisualStyleElement


Public Class lab9stuff
    'Private receivedBytes As New List(Of Byte)
    'Private closingForm As Boolean = False

    Sub PopulatePorts()
        Dim portNames() As String = SerialPort.GetPortNames()
        Dim previousSelection As String = If(ComboBox1.SelectedItem IsNot Nothing, ComboBox1.SelectedItem.ToString(), String.Empty)

        ComboBox1.BeginUpdate()
        ComboBox1.Items.Clear()
        For Each pn In portNames
            ComboBox1.Items.Add(pn)
        Next

        If Not String.IsNullOrEmpty(previousSelection) AndAlso Array.IndexOf(portNames, previousSelection) >= 0 Then
            ComboBox1.SelectedItem = previousSelection
        ElseIf ComboBox1.Items.Count > 0 Then
            ComboBox1.SelectedIndex = 0
        End If
        ComboBox1.EndUpdate()
    End Sub

    Sub Connect()
        ' Use the combo box selection; do not repopulate here (call PopulatePorts separately)
        If ComboBox1.SelectedItem Is Nothing Then
            Console.WriteLine("No COM port selected.")
            Return
        End If

        Dim selectedPort As String = ComboBox1.SelectedItem.ToString()
        Dim available = SerialPort.GetPortNames()
        If Array.IndexOf(available, selectedPort) < 0 Then
            Console.WriteLine($"Selected port {selectedPort} is not available. Available: {String.Join(", ", available)}")
            Return
        End If

        Try
            If SerialPort1.IsOpen Then SerialPort1.Close()
            SerialPort1.BaudRate = 9600 'Q@ Board Default
            SerialPort1.Parity = Parity.None
            SerialPort1.StopBits = StopBits.One
            SerialPort1.DataBits = 8
            SerialPort1.PortName = selectedPort

            SerialPort1.Open()
            Console.WriteLine($"Opened {selectedPort}")
        Catch ex As Exception
            Console.WriteLine($"Failed to open {selectedPort}: {ex.Message}")
        End Try

        ReadDataTimer.Start()

    End Sub

    Sub Write()
        Dim data(1) As Byte
        data(0) = &H24
        Select Case TrackBar1.Value
            Case 0
                data(1) = &H32
            Case 1
                data(1) = &H3F
            Case 2
                data(1) = &H4C
            Case 3
                data(1) = &H59
            Case 4
                data(1) = &H66
            Case 5
                data(1) = &H73
            Case 6
                data(1) = &H80
            Case 7
                data(1) = &H8D
            Case 8
                data(1) = &H9A
            Case 9
                data(1) = &HA7
            Case 10
                data(1) = &HB4
            Case 11
                data(1) = &HC1
            Case 12
                data(1) = &HCE
            Case 13
                data(1) = &HDB
            Case 14
                data(1) = &HE8
            Case 15
                data(1) = &HFA
        End Select



        'writes a $ to the serial port to request data from the Q@ board
        If SerialPort1.IsOpen Then
            ' SerialPort1.Write(TextBox1.Text)
            SerialPort1.Write(data, 0, 2)
            'SerialPort1.Write(TrackBar1.Value)
        Else
            Console.WriteLine("Serial port is not open. Cannot write data.")
        End If
    End Sub

    '-------------------------------------------------EVENT HANDLERS----------------------------------------------------------------------------------------------------------


    ' Class-level variables (you already have these)
    Private receivedBytes As New List(Of Byte)
    Private closingForm As Boolean = False
    Private Const START_MARKER As Byte = &H7E ' '~'

    ' DataReceived: append bytes to buffer, schedule processing (non-blocking)
    Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        If closingForm Then Return

        Try
            Dim count = SerialPort1.BytesToRead
            If count <= 0 Then Return
            Dim buffer(count - 1) As Byte
            SerialPort1.Read(buffer, 0, count)

            SyncLock receivedBytes
                receivedBytes.AddRange(buffer)
            End SyncLock

            ' Use BeginInvoke to avoid blocking when form is closing
            If Not closingForm Then
                Try
                    Me.BeginInvoke(Sub() ProcessReceivedData())
                Catch ex As Exception
                    ' ignore if form closing
                End Try
            End If
        Catch ex As Exception
            ' You can optionally log ex.Message for debugging
        End Try
    End Sub

    ' Class-level
    Private adcHistory As New Queue(Of Integer)
    Private Const MaxHistory As Integer = 5


    Private Sub ProcessReceivedData()
        If closingForm Then Return

        SyncLock receivedBytes
            While receivedBytes.Count >= 3
                If receivedBytes(0) <> &H7E Then
                    receivedBytes.RemoveAt(0)
                    Continue While
                End If

                If receivedBytes.Count < 3 Then Exit While

                Dim highByte As Integer = receivedBytes(1)
                Dim lowByte As Integer = receivedBytes(2)

                ' Left-justified reconstruction
                Dim adcValue As Integer = (highByte << 2) Or (lowByte >> 6)

                ' --- Add to history and compute average ---
                adcHistory.Enqueue(adcValue)
                If adcHistory.Count > MaxHistory Then adcHistory.Dequeue()
                Dim avgADC As Integer = CInt(adcHistory.Average())

                ' Convert ADC to LM34 temperature (°F)
                ' LM34 outputs 10 mV per °F, and your PIC ADC is 0–1023 over 5V
                Dim voltage As Double = avgADC * 5.0 / 1023.0
                Dim temperatureF As Double = voltage * 100.0   ' 5V/1023 → volts, *100 to get °F
                TextBox1.Text = temperatureF.ToString("F1")

                ' Remove processed packet
                receivedBytes.RemoveRange(0, 3)
            End While
        End SyncLock
    End Sub


    Private Sub lab9stuff_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        closingForm = True
        Try
            If SerialPort1 IsNot Nothing AndAlso SerialPort1.IsOpen Then
                SerialPort1.DiscardInBuffer()
                SerialPort1.Close()
            End If
        Catch ex As Exception
            ' ignore
        End Try
    End Sub

    Private Sub ComboBox1_DropDown(sender As Object, e As EventArgs) Handles ComboBox1.DropDown
        ' Refresh list when the user opens the dropdown
        PopulatePorts()
    End Sub

    Private Sub ConnectButton_Click(sender As Object, e As EventArgs) Handles ConnectButton.Click
        Connect()
    End Sub
    Private Sub SendButton_Click(sender As Object, e As EventArgs) Handles SendButton.Click
        Write()
    End Sub

    'Private Sub ReadDataTimer_Tick(sender As Object, e As EventArgs) Handles ReadDataTimer.Tick
    '    Dim byteMyData(SerialPort1.BytesToRead) As Byte
    '    Dim temp As String
    '    SerialPort1.Read(byteMyData, 0, SerialPort1.BytesToRead)
    '    For Each thing In byteMyData
    '        temp &= Chr(thing)
    '    Next
    '    Me.Text = temp

    'End Sub

    Private Sub TrackBar1_ValueChanged(sender As Object, e As EventArgs) Handles TrackBar1.ValueChanged
        Write()
    End Sub

    'Private Sub ADCRequestButton_Click(sender As Object, e As EventArgs) Handles ADCRequestButton.Click
    '    ' --- Ask PIC for ADC data ---
    '    If SerialPort1.IsOpen Then
    '        SerialPort1.Write("!")   ' Send request character
    '    Else
    '        MessageBox.Show("serial port not open")
    '    End If

    'End Sub

    Private Async Sub ADCRequestButton_Click(sender As Object, e As EventArgs) Handles ADCRequestButton.Click
        ' --- Ask PIC for ADC data ---
        If SerialPort1.IsOpen Then
            ' Disable button to prevent multiple fast requests
            ADCRequestButton.Enabled = False

            ' Send request character to PIC
            SerialPort1.Write("!")

            ' Wait ~50 ms (adjust if needed) before re-enabling
            Await Task.Delay(50)
            ADCRequestButton.Enabled = True
        Else
            MessageBox.Show("Serial port not open")
        End If
    End Sub




End Class