using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace Remote
{
    public class Remote
    {
        public T ReadCommand<T>(Opcode opcode)
        {
            var buffer = new Queue<byte>();

            OpenConnection();
            var opCodeBytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(opcode));
            _stream.Write(opCodeBytes, 0, opCodeBytes.Length);

            //this can probably be improved, maybe better method for reading?
            var res = new int();
           
           // var buffet = new byte[1024];
            //_stream.Read(buffet, 0, buffet.Length);
            while(true)
            {
                res = _stream.ReadByte();
                if (res != -1)
                {
                    buffer.Enqueue((byte)res);
                }
                else
                {
                    break;
                }

            }

            CloseConnection();
            
            return JsonConvert.DeserializeObject<T>(Encoding.ASCII.GetString(buffer.ToArray()));
        }

        private class Packet
        {
            public int Opcode;
            public Object Data;

            public Packet(int opcode, Object data)
            {
                Opcode = opcode;
                Data = data;

            }
        }

        public void SendCommand(int opcode, Object data)
        {
            OpenConnection();
            var opCodeBytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new Packet(opcode, data)));
            _stream.Write(opCodeBytes, 0, opCodeBytes.Length);
                
          /*  var dataBytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));
            _stream.Write(dataBytes, 0, dataBytes.Length);   */
            CloseConnection();
        }

        private void OpenConnection()
        {
            _tcpClient = new TcpClient(Address, Port);
            _stream = _tcpClient.GetStream();
        }

        private void CloseConnection()
        {
            _tcpClient.Close();
        }
        
        public Remote(string kheperaAddress, int port=2542)
        {
            Address = kheperaAddress;
            Port = port;

        }
        
        private string Address { get; set; }
        private int Port { get; set; }
        
        private TcpClient _tcpClient;
        private NetworkStream _stream;
    }
}