using System;
using System.Collections.Generic;
using System.Text;

public class Md5
{
    UInt32[] s, K;
    delegate byte Block(UInt32 g);

    public Md5()
    {
        s = new UInt32[]{ 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22 ,    5, 9, 14, 20, 5,  9, 14, 20, 5,  9, 14, 20, 5, 9, 14, 20,    4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,    6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21 };
        K = new UInt32[64];
        for (int i = 0; i < K.Length; i++)
        {
            K[i] = (UInt32)Math.Floor((1 << 31) * Math.Abs(Math.Sin(i + 1.0)));
        }

    }

    public string FindHash(string inputvalue)
    {
        UInt32 block_size = 512;

        UInt32 a0 = 0x67452301;   //A
        UInt32 b0 = 0xefcdab89;   //B
        UInt32 c0 = 0x98badcfe;   //C
        UInt32 d0 = 0x10325476;   //D

        ASCIIEncoding encoderer = new ASCIIEncoding();
        byte[] bytearray = encoderer.GetBytes(inputvalue);

        UInt32 bits = ((UInt32)bytearray.Length) * 8;
        UInt32 new_length = bits + block_size - (bits % block_size); //512bit blocks

        byte[] newarray = new byte[new_length];

        bytearray.CopyTo(newarray, 0);

        for(UInt32 i = 0; i < new_length; i += 512)
        {
            Block M = n => newarray[i + n];
            UInt32 A = a0;
            UInt32 B = b0;
            UInt32 C = c0;
            UInt32 D = d0;

            //Main Loop:
            for(UInt32 j = 0; j < 64; j++)
            {
                UInt32 F, g;
                if (j < 16)
                {
                    F = (B & C) | ((~B) & D);
                    g = j;
                }
                else if (j < 32)
                {
                    F = (D & B) | ((~D) & C);
                    g = (5 * j + 1) % 16;
                }
                else if (j < 48)
                {
                    F = B ^ C ^ D;
                    g = (3 * j + 5) % 16;
                }
                else
                {
                    F = C ^ (B | (~D));
                    g = (7 * j) % 16;
                }

                F = F + A + K[j] + M(g);
                A = D;
                D = C;
                C = B;
                B = B + leftrotate((Int32)F, (Int32)s[j]);
            }

            //Add this chunk's hash to the result so far
            a0 += A;
            b0 += B;
            c0 += C;
            d0 += D;
        }

        List<byte> m_Bytelist = new List<byte>();
        m_Bytelist.AddRange(BitConverter.GetBytes(a0));
        m_Bytelist.AddRange(BitConverter.GetBytes(b0));
        m_Bytelist.AddRange(BitConverter.GetBytes(c0));
        m_Bytelist.AddRange(BitConverter.GetBytes(d0));

        string returnString = BitConverter.ToString(m_Bytelist.ToArray()).Replace("-", "");
        return returnString;
    }

    private UInt32 leftrotate(Int32 f, Int32 v)
    {
        return (UInt32)((f << v) | (f >> (32 - v)));
    }
}