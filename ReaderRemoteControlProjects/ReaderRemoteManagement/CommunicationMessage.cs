using System;
using System.Text;
using System.Configuration;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using Util;

namespace ReaderRemoteManagement
{
    public enum State { WAITING_FOR_TERMINAL_REQUEST, WAITING_FOR_APPLICATION_RESPONSE, WAITING_FOR_TERMINAL_RESPONSE, WAITING_FOR_TERMINAL_RESPONSE_BUTTON };

    public class CommunicationMessage
    {
        public byte STX = 0x82;
        public byte ETX = 0x83;
        public byte ESC = 0x7D;

        private byte messageType;
        public byte TERMINAL_REQUEST = 0x84;
        public byte APPLICATION_RESPONSE = 0x85;
        public byte TERMINAL_RESPONSE = 0x86;
        public byte TERMINAL_RESPONSE_BUTTON = 0x8C;
        public byte APPLICATION_RESPONSE_BUTTON = 0x8B;

        private byte documentType;
        public byte TICKET_RFID = 0x87;
        public byte TICKET_BARCODE = 0x89;

        public byte ENTRY = 0x87;
        public byte EXIT = 0x88;

        public byte RED = 0x31;
        public byte GREEN = 0x32;

        public byte NO = 0x30;
        public byte YES = 0x31;

        private byte messageButton;
        public byte NOButton = 0;
        public byte LEFTButton = 1;
        public byte RIGHTButton = 2;
        public byte BOTHButtons = 3;

        public readonly int MESSAGE_LENGTH = 20;
        public int MESSAGE_LENGTH_RESPONSE = 20;

        public int PassTimeOut = 0;   // default pass time out in ms, if it's not read from config

        //public int ScreenTimeOut = 5000;

        public int ScreenTimeOut = 0;

        private bool correctMessage = false;
        private string documentNumber;
        public byte antenna = 0x87; //ENTRY;
        public byte BarCodeReaderDirection = 0x87;//ENTRY;
        public byte displayNum = 3;

        private byte visitorPassed;

        private byte[] messageBytes;

        private ReaderRemoteControlManager manager;

        private int pressedButton = 0;

        //text constants
        private static int lineWidth = 20;
        private static int lineNum = 6;

        public override string ToString()
        {
            string result = "";
            if (messageType == TERMINAL_REQUEST)
            {
                result += "TERMINAL_REQUEST ";
                if (documentType == TICKET_RFID)
                {
                    result += "TICKET_RFID ";
                }
                else
                {
                    result += "TICKET_BARCODE ";
                }

                result += documentNumber.ToString();
            }
            else if (messageType == APPLICATION_RESPONSE)
            {
                result += "APPLICATION_RESPONSE ";

            }
            else if (messageType == TERMINAL_RESPONSE)
            {
                result += " TERMINAL_RESPONSE ";
                if (visitorPassed == YES)
                {
                    result += "Visitor passed";
                }
                else
                {
                    result += "Visitor not passed";
                }
            }
            return result;
        }
        public CommunicationMessage(ReaderRemoteControlManager TCmanager)
        {
            manager = TCmanager;
        }
        public void StartCommunicationMessage(byte[] messageBytes)
        {
            this.messageBytes = messageBytes;

            // Test message

            // STX
            correctMessage = (messageBytes[0] == STX);

            // ETX
            correctMessage = correctMessage && (messageBytes[19] == ETX);

            // Check sum
            byte xorByte = (byte)
                (messageBytes[1] ^ messageBytes[2] ^ messageBytes[3] ^
                messageBytes[4] ^ messageBytes[5] ^ messageBytes[6] ^
                messageBytes[7] ^ messageBytes[8] ^ messageBytes[9] ^
                messageBytes[10] ^ messageBytes[11] ^ messageBytes[12] ^
                messageBytes[13] ^ messageBytes[14] ^ messageBytes[15] ^
                messageBytes[16] ^ messageBytes[17]);
            correctMessage = correctMessage && (messageBytes[18] == xorByte);

            // Message type
            messageType = messageBytes[1];
            correctMessage = correctMessage && ((messageType == TERMINAL_REQUEST) || (messageType == APPLICATION_RESPONSE) || (messageType == TERMINAL_RESPONSE) || (messageType == APPLICATION_RESPONSE_BUTTON) || (messageType == TERMINAL_RESPONSE_BUTTON));
            // Message buttons
            try
            {
                messageButton = messageBytes[15];
                string s = Encoding.ASCII.GetString(messageBytes, 14, 1);
                pressedButton = Convert.ToInt32(s);
            }
            catch { }
            // state machine
            string typeOfMessage = "";
            // state machine
            if (manager.currentAntenna.state == State.WAITING_FOR_TERMINAL_REQUEST)
            {
                typeOfMessage = "TERMINAL REQUEST";
                if (messageType != TERMINAL_REQUEST) correctMessage = false;
                else
                {

                    correctMessage = true;
                    manager.currentAntenna.state = State.WAITING_FOR_APPLICATION_RESPONSE;
                }
                if (manager.logDebugMessages)
                {
                    manager.WriteLogDebugNewLine();
                    manager.WriteLogDebugMessage("+++Transaction started+++", Constants.detailedDebugLevel, manager.currentAntenna.AntennaNum);
                }

            }
            else if (manager.currentAntenna.state == State.WAITING_FOR_APPLICATION_RESPONSE)
            {
                typeOfMessage = "APPLICATON RESPONSE";
                if (messageType != APPLICATION_RESPONSE) correctMessage = false;
                else
                {

                    correctMessage = true;
                    if (messageBytes[2] == YES) manager.currentAntenna.state = State.WAITING_FOR_TERMINAL_RESPONSE;

                    else manager.currentAntenna.state = State.WAITING_FOR_TERMINAL_REQUEST;
                }
            }
            else if (manager.currentAntenna.state == State.WAITING_FOR_TERMINAL_RESPONSE)
            {

                typeOfMessage = "TERMINAL RESPONSE";
                if (messageType != TERMINAL_RESPONSE && messageType != TERMINAL_RESPONSE_BUTTON) correctMessage = false;
                else
                {

                    correctMessage = true;
                    manager.currentAntenna.state = State.WAITING_FOR_TERMINAL_REQUEST;
                }
            }

            // communication errors handling
            if (!correctMessage)
            {

                typeOfMessage += " (incorrect)";
                if (manager.currentAntenna.ticketTransactionState != ReaderRemoteManagement.AntennaCommunication.TicketTransactionState.STARTED)
                {
                    manager.currentAntenna.state = State.WAITING_FOR_TERMINAL_REQUEST;

                }
            }

            if (correctMessage)
            {
                ParseMessage();
            }

            Console.WriteLine("\n");
            Console.Write(typeOfMessage + " : ");
            for (int i = 0; i < messageBytes.Length; i++)
            {
                Console.Write(messageBytes[i].ToString("X2") + " ");
            }
            Console.WriteLine("\n");

            if (manager.logDebugMessages)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(typeOfMessage + " : ");
                for (int i = 0; i < messageBytes.Length; i++)
                {
                    sb.Append(messageBytes[i].ToString("X2") + " ");
                }
                // sb.Append("\n");
                manager.WriteLogDebugMessage(sb.ToString(), Constants.extendedDebugLevel, manager.currentAntenna.AntennaNum);
            }
        }

        public int getAntenna(byte[] messageBytes)
        {
            int antenna = 0;
            try
            {
                if (messageBytes[2] == EXIT)
                    antenna = 1;
            }
            catch { }
            return antenna;
        }

        private void ParseMessage()
        {

            // Tip 'isprave'
            documentType = messageBytes[2];

            // Broj isprave
            byte[] stringNumber = new byte[12];
            for (int i = 0; i < 12; i++)
            {
                stringNumber[i] = messageBytes[3 + i];
            }
            documentNumber = Encoding.ASCII.GetString(stringNumber).TrimStart('0');


            if (IsTicketRFID())
            {
                antenna = messageBytes[2];
            }
            else
            {
                antenna = BarCodeReaderDirection;
            }

            if (messageType == TERMINAL_RESPONSE)
            {
                // Did visitor passed ?
                visitorPassed = messageBytes[3];
            }
        }

        // Correct message
        public bool IsMessageCorrect()
        {
            return correctMessage;
        }

        public byte MessageType
        {
            get
            {
                return messageType;
            }
        }

        public byte MessageButton
        {
            get
            {
                return messageButton;
            }
        }
        // For Terminal request
        public bool IsTicketRFID()
        {
            return (documentType == ENTRY) || (documentType == EXIT);
        }
        public bool IsTicketBarcode()
        {
            return documentType == TICKET_BARCODE;
        }
        public string GetDocumentNumber()
        {
            if (manager.logDebugMessages)
            {
                manager.WriteLogDebugMessage(" Message is valid; Ticket ID:" + documentNumber, Constants.detailedDebugLevel,manager.currentAntenna.AntennaNum);
            }
            return documentNumber;
        }

        public int GetButtonNumber()
        {
            if (manager.logDebugMessages)
            {
                manager.WriteLogDebugMessage(" Message is valid; Ticket ID:" + pressedButton, Constants.detailedDebugLevel, manager.currentAntenna.AntennaNum);
            }
            return pressedButton;
        }

        // For terminal response
        public bool IsVisitorPassed()
        {
            return visitorPassed == YES;
        }
        public byte[] GetWaitForButtonMessageMessageBytes()
        {
            byte[] byteArray = new byte[20];

            byteArray[0] = STX;
            byteArray[1] = APPLICATION_RESPONSE_BUTTON;
            byteArray[2] = antenna;
            byteArray[3] = displayNum;

            string milliseconds = "10000";
            milliseconds = ToNDigitString(milliseconds, 5);

            byte[] millisecondsBytes = Encoding.ASCII.GetBytes(milliseconds);

            for (int i = 0; i < 5; i++)
            {
                byteArray[4 + i] = millisecondsBytes[i];
            }

            for (int i = 9; i < 18; i++)
            {
                byteArray[i] = 0xFF;
            }

            byteArray[18] = (byte)
                (byteArray[1] ^ byteArray[2] ^ byteArray[3] ^
                byteArray[4] ^ byteArray[5] ^ byteArray[6] ^
                byteArray[7] ^ byteArray[8] ^ byteArray[9] ^
                byteArray[10] ^ byteArray[11] ^ byteArray[12] ^
                byteArray[13] ^ byteArray[14] ^ byteArray[15] ^
                byteArray[16] ^ byteArray[17]);

            byteArray[19] = ETX;

            return byteArray;
        }
        //public byte[] GetPassIsAllowedMessageBytes(string x)
        //{
        //    byte[] byteArray = new byte[20];

        //    byteArray[0] = STX;
        //    byteArray[1] = APPLICATION_RESPONSE;
        //    byteArray[2] = antenna;
        //    byteArray[3] = YES;

        //    string milliseconds = PassTimeOut.ToString();
        //    milliseconds = ToNDigitString(milliseconds, 5);

        //    byte[] millisecondsBytes = Encoding.ASCII.GetBytes(milliseconds);

        //    for (int i = 0; i < 5; i++)
        //    {
        //        byteArray[4 + i] = millisecondsBytes[i];
        //    }

        //    for (int i = 9; i < 18; i++)
        //    {
        //        byteArray[i] = 0xFF;
        //    }

        //    // switch-on barrier sound and light signalization
        //    byteArray[9] = GREEN;//anntena
        //    byteArray[10] = YES;
        //    byteArray[11] = YES;//device green
        //    byteArray[12] = NO;
        //    byteArray[13] = YES;

        //    byteArray[18] = (byte)
        //        (byteArray[1] ^ byteArray[2] ^ byteArray[3] ^
        //        byteArray[4] ^ byteArray[5] ^ byteArray[6] ^
        //        byteArray[7] ^ byteArray[8] ^ byteArray[9] ^
        //        byteArray[10] ^ byteArray[11] ^ byteArray[12] ^
        //        byteArray[13] ^ byteArray[14] ^ byteArray[15] ^
        //        byteArray[16] ^ byteArray[17]);

        //    byteArray[19] = ETX;

        //    return byteArray;
        //}


        public string getCenterdText(string text)
        {
            string centerdText = "";
            string[] strWords = text.Split(' ');
            List<string> Lines = new List<string>();
            foreach (string word in strWords)
            {
                string wordCopy = word;
                while (wordCopy.Length > lineWidth)
                {
                    Lines.Add(wordCopy.Substring(0, lineWidth));
                    wordCopy = wordCopy.Substring(lineWidth - 1, wordCopy.Length - lineWidth);
                }

                if (wordCopy.Length < lineWidth)
                {
                    if (Lines.Count == 0)
                    {
                        Lines.Add(wordCopy);
                    }
                    else if (Lines[Lines.Count - 1].Length < lineWidth)
                    {
                        if (Lines[Lines.Count - 1].Length < (lineWidth - wordCopy.Length - 1))
                        {
                            Lines[Lines.Count - 1] = Lines[Lines.Count - 1] + " " + wordCopy;
                        }
                        else
                        {
                            Lines.Add(wordCopy);
                        }
                    }
                }
            }
            string emptyLine = "";
            while (emptyLine.Length < lineWidth)
            {
                emptyLine = emptyLine + " ";
            }
            int addLinesNum = (lineNum - Lines.Count) / 2;
            for (int i = 0; i < addLinesNum; i++)
            {
                Lines.Insert(0, emptyLine);
            }

            foreach (string line in Lines)
            {
                string lineCopy = line;
                int k = 1;
                while (lineCopy.Length < lineWidth)
                {
                    if (k % 2 == 1)
                    {
                        lineCopy = " " + lineCopy;
                    }
                    else
                    {
                        lineCopy = lineCopy + " ";
                    }
                    k++;
                }
                centerdText = centerdText + lineCopy;
            }
            return centerdText;
        }
        public byte[] GetPassIsAllowedMessageBytes(string screenMessage)
        {
            string text = "";
            if (screenMessage.Length > 0)
                text = getCenterdText(screenMessage);
            int i = MESSAGE_LENGTH;
            if (text.Length > 0)
                i = text.Length * 2 + 36;
            MESSAGE_LENGTH_RESPONSE = i;

            byte[] byteArray = new byte[i];
            //byte[] byteArray = new byte[20];

            byteArray[0] = STX;
            byteArray[1] = APPLICATION_RESPONSE;
            byteArray[2] = antenna;
            byteArray[3] = YES;

            string milliseconds = PassTimeOut.ToString();
            milliseconds = ToNDigitString(milliseconds, 5);

            byte[] millisecondsBytes = Encoding.ASCII.GetBytes(milliseconds);

            for (int j = 0; j < 5; j++)
            {
                byteArray[4 + j] = millisecondsBytes[j];
            }

            for (int j = 9; j < 18; j++)
            {
                byteArray[j] = 0xFF;
            }

            // switch-on barrier sound and light signalization
            byteArray[9] = GREEN;//anntena
            byteArray[10] = YES;
            byteArray[11] = YES;//device green
            byteArray[12] = NO;
            byteArray[13] = YES;

            if (text != "")
            {
                byteArray[14] = 0x00;

                string messageLength = (text.Length * 2 + 17).ToString("X");
                //messageLength = ToNDigitString(messageLength, 2);
                byte[] lenght = new byte[2];
                lenght[0] = byte.Parse(messageLength, System.Globalization.NumberStyles.HexNumber);
                lenght[1] = 0x00;
                for (int j = 0; j < 2; j++)
                {
                    byteArray[15 + j] = lenght[j];
                }
                //start of TFT Message
                byteArray[17] = 0xAA;
                //TFT Comand is show text type
                byteArray[18] = 0x98;
                //X start from left up corner coordinate
                byteArray[19] = 0x00;
                byteArray[20] = 0x00;
                //Y coordinate
                byteArray[21] = 0x00;
                byteArray[22] = 0x00;
                //font type
                //byteArray[23] = 0x00;
                byteArray[23] = 0x2C;
                //first byte: C-show background and foreground, 8-show foreground, 4- show background ; secund byte: 5 for UNICODE
                byteArray[24] = 0x85;
                //bitmap letter size 08 for 32x64
                byteArray[25] = 0x06;
                //foreground color RGB - two bytes RRRRRGGGGGGBBBBB 
                byteArray[26] = 0xFF;
                byteArray[27] = 0xFF;
                //background color RGB - two bytes RRRRRGGGGGGBBBBB
                byteArray[28] = 0x00;
                byteArray[29] = 0x00;
                int ord = 30;
                //text convert to bytes and reorder bytes foreach letter (two bytes are distance of letter from beggining of the font. If font starts with 0100 and letter is at 01AA bytes are 00AA)
                for (int j = 0; j < text.Length; j++)
                {
                    byte[] screenText = Encoding.Unicode.GetBytes(text[j].ToString());

                    for (int k = screenText.Length - 1; k >= 0; k--)
                    {
                        byteArray[ord] = screenText[k];
                        ord++;
                    }
                }
                byteArray[30 + text.Length * 2] = 0xCC;
                byteArray[30 + text.Length * 2 + 1] = 0x33;
                byteArray[30 + text.Length * 2 + 2] = 0xC3;
                byteArray[30 + text.Length * 2 + 3] = 0x3C;
                byteArray[30 + text.Length * 2 + 4] = byteArray[1];
                for (int j = 2; j < (34 + text.Length * 2); j++)
                {
                    byteArray[30 + text.Length * 2 + 4] = (byte)(byteArray[30 + text.Length * 2 + 4] ^ byteArray[j]);
                }

                byteArray[30 + text.Length * 2 + 5] = ETX;
            }
            else
            {
                byteArray[18] = byteArray[1];
                for (int j = 2; j < 18; j++)
                {
                    byteArray[18] = (byte)(byteArray[18] ^ byteArray[j]);
                }

                byteArray[19] = ETX;
            }

            return byteArray;
        }

        public byte[] GetPassIsDeniedMessageBytes(string screenMessage)
        {
            string text = "";
            if (screenMessage.Length > 0)
                text = getCenterdText(screenMessage);
            int i = MESSAGE_LENGTH;
            if (text.Length > 0)
                i = text.Length * 2 + 36;
            MESSAGE_LENGTH_RESPONSE = i;

            byte[] byteArray = new byte[i];

            byteArray[0] = STX;
            byteArray[1] = APPLICATION_RESPONSE;
            byteArray[2] = antenna;
            byteArray[3] = NO;

            for (int j = 4; j < 18; j++)
            {
                byteArray[j] = 0xFF;
            }
            //if showing text send time out of display screen
            if (text.Length * 2 > 0)
            {
                string milliseconds = ScreenTimeOut.ToString();
                milliseconds = ToNDigitString(milliseconds, 5);

                byte[] millisecondsBytes = Encoding.ASCII.GetBytes(milliseconds);

                for (int j = 0; j < 5; j++)
                {
                    byteArray[4 + j] = millisecondsBytes[j];
                }
            }

            // switch-on barrier sound and light signalization
            byteArray[9] = RED;
            byteArray[10] = NO;
            byteArray[11] = NO;
            byteArray[12] = YES;
            byteArray[13] = YES;
            if (text != "")
            {
                byteArray[14] = 0x00;

                string messageLength = (text.Length * 2 + 17).ToString("X");
                //messageLength = ToNDigitString(messageLength, 2);
                byte[] lenght = new byte[2];
                lenght[0] = byte.Parse(messageLength, System.Globalization.NumberStyles.HexNumber);
                lenght[1] = 0x00;
                for (int j = 0; j < 2; j++)
                {
                    byteArray[15 + j] = lenght[j];
                }
                //start of TFT Message
                byteArray[17] = 0xAA;
                //TFT Comand is show text type
                byteArray[18] = 0x98;
                //X start from left up corner coordinate
                byteArray[19] = 0x00;
                byteArray[20] = 0x00;
                //Y coordinate
                byteArray[21] = 0x00;
                byteArray[22] = 0x00;
                //font type
                //byteArray[23] = 0x00;
                byteArray[23] = 0x2C;
                //first byte: C-show background and foreground, 8-show foreground, 4- show background ; secund byte: 5 for UNICODE
                byteArray[24] = 0x85;
                //bitmap letter size 08 for 32x64
                byteArray[25] = 0x06;
                //foreground color RGB - two bytes RRRRRGGGGGGBBBBB 
                byteArray[26] = 0xFF;
                byteArray[27] = 0xFF;
                //background color RGB - two bytes RRRRRGGGGGGBBBBB
                byteArray[28] = 0x00;
                byteArray[29] = 0x00;
                int ord = 30;
                //text convert to bytes and reorder bytes foreach letter (two bytes are distance of letter from beggining of the font. If font starts with 0100 and letter is at 01AA bytes are 00AA)
                for (int j = 0; j < text.Length; j++)
                {
                    byte[] screenText = Encoding.Unicode.GetBytes(text[j].ToString());

                    for (int k = screenText.Length - 1; k >= 0; k--)
                    {
                        byteArray[ord] = screenText[k];
                        ord++;
                    }
                }

                byteArray[30 + text.Length * 2] = 0xCC;
                byteArray[30 + text.Length * 2 + 1] = 0x33;
                byteArray[30 + text.Length * 2 + 2] = 0xC3;
                byteArray[30 + text.Length * 2 + 3] = 0x3C;

                byteArray[30 + text.Length * 2 + 4] = byteArray[1];
                for (int j = 2; j < (34 + text.Length * 2); j++)
                {
                    byteArray[30 + text.Length * 2 + 4] = (byte)(byteArray[30 + text.Length * 2 + 4] ^ byteArray[j]);
                }

                byteArray[30 + text.Length * 2 + 5] = ETX;
            }
            else
            {
                byteArray[18] = byteArray[1];
                for (int j = 2; j < 18; j++)
                {
                    byteArray[18] = (byte)(byteArray[18] ^ byteArray[j]);
                }

                byteArray[19] = ETX;
            }

            return byteArray;
        }

        private string ToNDigitString(string inputString, int digitLength)
        {
            while (inputString.Length < digitLength)
            {
                inputString = "0" + inputString;
            }
            return inputString;
        }

        public bool ReadTerminal(NetworkStream netStream, byte[] myReadBuffer)
        {
            byte[] buffer = new byte[MESSAGE_LENGTH];
            int totalBytes = 0;

            DateTime t0 = DateTime.Now;
            bool sussess = false;

            try
            {
                if (netStream.DataAvailable)
                {
                    while (totalBytes < MESSAGE_LENGTH)
                    {
                        if (netStream.DataAvailable)
                        {
                            int numberOfBytesRead = netStream.Read(buffer, 0, 1);

                            for (int i = 0; i < numberOfBytesRead; i++)
                            {
                                myReadBuffer[totalBytes + i] = buffer[i];
                            }
                            totalBytes += numberOfBytesRead;
                        }

                        if ((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 1000)
                        {
                            Console.WriteLine("\nReadTerminal() time out!\n");
                            if (manager.logDebugMessages)
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append(" ReadTerminal() time out!"); ;
                                manager.WriteLogDebugMessage(sb.ToString(), Constants.detailedDebugLevel, manager.currentAntenna.AntennaNum);
                            }
                            break;
                        }
                    }
                    return (totalBytes == MESSAGE_LENGTH);
                }
            }
            catch
            {
                sussess = false;
            }

            return sussess;
        }

        public bool FlushTerminal(NetworkStream netStream)
        {
            byte[] buffer = new byte[MESSAGE_LENGTH];

            DateTime t0 = DateTime.Now;
            bool sussess = false;

            try
            {
                while (netStream.DataAvailable)
                {
                    netStream.Read(buffer, 0, 1);

                    if ((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 1000)
                    {
                        Console.WriteLine("\nFlushTerminal() time out!\n");
                        if (manager.logDebugMessages)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append(" FlushTerminal() time out!\n"); ;
                            manager.WriteLogDebugMessage(sb.ToString(), Constants.detailedDebugLevel, -1);
                        }
                        return false;
                    }
                }
                sussess = true;
            }
            catch
            {
                sussess = false;
            }

            return sussess;
        }


        public byte[] GetDropArmMessageBytes()
        {
            byte[] byteArray = new byte[20];

            byteArray[0] = STX;
            byteArray[1] = APPLICATION_RESPONSE;
            byteArray[2] = 0x88;
            byteArray[3] = 0x31;        // fire DO 2
            byteArray[4] = 0x30;
            byteArray[5] = 0x31;
            byteArray[6] = 0x30;
            byteArray[8] = 0x30;
            byteArray[7] = 0x30;
            byteArray[9] = 0x32;
            byteArray[10] = 0x31;

            //string milliseconds = PassTimeOut.ToString();
            //milliseconds = ToNDigitString(milliseconds, 5);

            //byte[] millisecondsBytes = Encoding.ASCII.GetBytes(milliseconds);

            //for (int i = 0; i < 5; i++)
            //{
            //    byteArray[4 + i] = millisecondsBytes[i];
            //}

            for (int i = 11; i < 18; i++)
            {
                byteArray[i] = 0xFF;
            }

            //// switch-on barrier sound and light signalization
            //byteArray[9] = GREEN;//anntena
            //byteArray[10] = YES;
            //byteArray[11] = YES;//device green
            //byteArray[12] = NO;
            //byteArray[13] = YES;

            byteArray[18] = (byte)
                (byteArray[1] ^ byteArray[2] ^ byteArray[3] ^
                byteArray[4] ^ byteArray[5] ^ byteArray[6] ^
                byteArray[7] ^ byteArray[8] ^ byteArray[9] ^
                byteArray[10] ^ byteArray[11] ^ byteArray[12] ^
                byteArray[13] ^ byteArray[14] ^ byteArray[15] ^
                byteArray[16] ^ byteArray[17]);

            byteArray[19] = ETX;

            return byteArray;
        }

        public byte[] CanteenMonitorFIAT(string[] statuses, int antenna)
        {
            // constants
            const int noLines = 8; const int lineLen = 56; const int statusW = 17;
            const int xStart = 0x03; const int yStart = 0x20; const int lineH = 0x18;
            const int yStatus = 0xE6; const int cW = 12;

            byte[] rwdMsg = new byte[1024]; int end = 0;

            // compose message
            rwdMsg[0] = 0x82;   // STX
            rwdMsg[1] = 0x85;   // typeA == typeAE
            rwdMsg[2] = (byte)((antenna == 0) ? 0x87 : 0x88); // ANT0 or ANT1
            rwdMsg[3] = 0x30;   // don't change relay state
            rwdMsg[4] = 0x30; rwdMsg[5] = 0x30; rwdMsg[6] = 0x30; rwdMsg[7] = 0x30; rwdMsg[8] = 0x30;   // don't erase message (infinite timeout)
            // rwdMsg[9] = ?    // 0x32 - GREEN or 0x31 - RED, see bellow
            rwdMsg[10] = 0x30;  // turn on BEEP on antenna
            rwdMsg[11] = 0x30; rwdMsg[12] = 0x30; rwdMsg[13] = 0x30; // don't touch device's green, red and beep
            rwdMsg[14] = 0x00;  // type AE
            //rwdMsg[15] = ? rwdMsg[16] = ? // monitor message length
            rwdMsg[17] = 0xAA; rwdMsg[18] = 0x70; rwdMsg[19] = 0x00; rwdMsg[20] = 0xCC; rwdMsg[21] = 0x33; rwdMsg[22] = 0xC3; rwdMsg[23] = 0x3C; // show background (clear screen)
            try
            {
                for (int i = 0; i < noLines; i++)
                {
                    if (statuses[i] == String.Empty) break;

                    int l = 24 + i * lineLen;
                    rwdMsg[l] = 0xAA; rwdMsg[l + 1] = 0x98; // text command

                    rwdMsg[l + 2] = (byte)((xStart & 0x0000FF00) >> 8); rwdMsg[l + 3] = (byte)(xStart & 0x000000FF);                    // X
                    int y = yStart + i * lineH; rwdMsg[l + 4] = (byte)((y & 0x0000FF00) >> 8); rwdMsg[l + 5] = (byte)(y & 0x000000FF);  // Y
                    rwdMsg[l + 6] = 0x00; rwdMsg[l + 7] = 0x81; rwdMsg[l + 8] = 0x02; // Lib_ID, C_mode, C_dots (all about font)

                    string[] lineComponents = statuses[i].Split(';');
                    lineComponents[0] = lineComponents[0].PadRight(14); lineComponents[1] = lineComponents[1].PadRight(statusW);
                    if (lineComponents[1].StartsWith(Constants.RestaurantApproved))
                    {
                        if (i == 0) rwdMsg[9] = 0x32;                   // last registration approved, turn on GREEN on antenna
                        rwdMsg[l + 9] = 0x04; rwdMsg[l + 10] = 0xE0;    // foreground green
                    }
                    else
                    {
                        if (i == 0) rwdMsg[9] = 0x31;                   // last registration denied, turn on RED on antenna
                        rwdMsg[l + 9] = 0xF8; rwdMsg[l + 10] = 0x00;    // foreground red
                    }
                    rwdMsg[l + 11] = 0x00; rwdMsg[l + 12] = 0x00;       // background transparent
                    end = l + 13;

                    byte[] jmbg = Encoding.ASCII.GetBytes(lineComponents[0]); for (int j = 0; j < jmbg.Length; j++) rwdMsg[end + j] = jmbg[j]; end = end + jmbg.Length;
                    byte[] stat = Encoding.ASCII.GetBytes(lineComponents[1]); for (int j = 0; j < stat.Length; j++) rwdMsg[end + j] = stat[j]; end = end + stat.Length;
                    byte[] tren = Encoding.ASCII.GetBytes(lineComponents[2]); for (int j = 0; j < tren.Length; j++) rwdMsg[end + j] = tren[j]; end = end + tren.Length;

                    rwdMsg[end] = 0xCC; rwdMsg[end + 1] = 0x33; rwdMsg[end + 2] = 0xC3; rwdMsg[end + 3] = 0x3C; // end command
                    end = end + 4;
                }

                // addendum: status line (the 9th one)
                string[] noMeals = statuses[noLines].Split(';'); int offset = 0;
                int greenMeals = Int32.Parse(noMeals[0]); int redMeals = Int32.Parse(noMeals[1]); int allMeals = greenMeals + redMeals;
                byte[] b1 = Text2Monitor("IZDATO:       " + allMeals.ToString() + " (", xStart, yStatus, "BLUE"); offset = ((14 + allMeals.ToString().Length + 2) * cW);
                for (int i = 0; i < b1.Length; i++) rwdMsg[end + i] = b1[i]; end += b1.Length;
                byte[] b2 = Text2Monitor(greenMeals.ToString(), xStart + offset, yStatus, "GREEN"); offset += greenMeals.ToString().Length * cW;
                for (int i = 0; i < b2.Length; i++) rwdMsg[end + i] = b2[i]; end += b2.Length;
                byte[] b3 = Text2Monitor(" + ", xStart + offset, yStatus, "BLUE"); offset += 3 * cW;
                for (int i = 0; i < b3.Length; i++) rwdMsg[end + i] = b3[i]; end += b3.Length;
                byte[] b4 = Text2Monitor(redMeals.ToString(), xStart + offset, yStatus, "RED"); offset += redMeals.ToString().Length * cW;
                for (int i = 0; i < b4.Length; i++) rwdMsg[end + i] = b4[i]; end += b4.Length;
                byte[] b5 = Text2Monitor(")", xStart + offset, yStatus, "BLUE"); offset += cW;
                for (int i = 0; i < b5.Length; i++) rwdMsg[end + i] = b5[i]; end += b5.Length;

                int monitorMsgLen = end - 17;
                rwdMsg[15] = (byte)(monitorMsgLen & 0x000000FF); rwdMsg[16] = (byte)((monitorMsgLen & 0x0000FF00) >> 8); // monitor message length

                byte checksum = 0; for (int i = 1; i < end; i++) checksum ^= rwdMsg[i];
                rwdMsg[end] = checksum;   // checksum

                rwdMsg[end + 1] = 0x83;   // ETX
            }
            catch (Exception ex)
            {
                throw ex;
            }

            byte[] rwdMsgRet = new byte[end + 2]; for (int i = 0; i < rwdMsgRet.Length; i++) rwdMsgRet[i] = rwdMsg[i];

            return rwdMsgRet;
        }

        private byte[] Text2Monitor(string text, int x, int y, string color)
        {
            byte[] msg = new byte[1024]; int end = 0;

            msg[end++] = 0xAA; msg[end++] = 0x98;   // text command

            msg[end++] = (byte)((x & 0x0000FF00) >> 8); msg[end++] = (byte)(x & 0x000000FF);  // X
            msg[end++] = (byte)((y & 0x0000FF00) >> 8); msg[end++] = (byte)(y & 0x000000FF);  // Y
            msg[end++] = 0x00; msg[end++] = 0x81; msg[end++] = 0x02;                          // Lib_ID, C_mode, C_dots (all about font)

            // foreground
            if (color.ToUpper() == "GREEN") { msg[end++] = 0x04; msg[end++] = 0xE0; }
            else if (color.ToUpper() == "BLUE") { msg[end++] = 0x00; msg[end++] = 0x1F; }
            else if (color.ToUpper() == "RED") { msg[end++] = 0xF8; msg[end++] = 0x00; }

            msg[end++] = 0x00; msg[end++] = 0x00;                             // background transparent

            byte[] bytes = Encoding.ASCII.GetBytes(text); for (int i = 0; i < text.Length; i++) msg[end++] = bytes[i];

            msg[end++] = 0xCC; msg[end++] = 0x33; msg[end++] = 0xC3; msg[end++] = 0x3C; // end command

            byte[] msgRet = new byte[end]; for (int i = 0; i < msgRet.Length; i++) msgRet[i] = msg[i];

            return msgRet;
        }
    }
}
