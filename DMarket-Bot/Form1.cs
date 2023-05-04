using DMarket_Bot.Models;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Encodings.Web;
using System.Xml.Serialization;
using NaCl.Core;

namespace DMarket_Bot
{
    public partial class Form1 : Form
    {
        // insert your api keys
        static string publicKey = "cb6dc82eb0f03a64a75bf7825818c265665ccbce930a5659521e14210e2c056a";
        static string secretKey = "849c157ec5271dc3a681a0c50b8b31cb4760170f31ff2829ace0209bcf8cb491cb6dc82eb0f03a64a75bf7825818c265665ccbce930a5659521e14210e2c056a";
        static string host = "api.dmarket.com";
        public Form1()
        {
            Console.WriteLine("Selam, Moruq!");
            InitializeComponent();
            Console.WriteLine("Selam Dunyali");
        }

        private void EncodeTextt()
        {
            string apiUrlPath = "/exchange/v1/target/create";
            RequestOptions rOptions = new RequestOptions()
            {
                host = host,
                path = apiUrlPath,
                method = "GET",
                headers =
                {
                    XApiKey = publicKey,
                    //XRequestSign = "dmar ed25519" + 
                }
            };
            rOptions.host = host;
            Console.WriteLine("Selam, Moruq!");
        }

        public string sign(string s)
        {
            //const encoder = new TextEncoder();
            //string signatureBytes = TextEncoder.("utf-8"). nacl.sign(new TextEncoder('utf-8').encode(string), hexStringToByte(secretKey));
            //return byteToHexString(signatureBytes).substr(0, 128);
            return null;
        }

        public void EncodeText(object sender, EventArgs e)
        {
            using var rng = RandomNumberGenerator.Create();

            Curve25519XSalsa20Poly1305.KeyPair(out var aliceSecretKey, out var alicePublicKey);
            Curve25519XSalsa20Poly1305.KeyPair(out var bobSecretKey, out var bobPublicKey);

            Curve25519XSalsa20Poly1305 aliceBox = new Curve25519XSalsa20Poly1305(aliceSecretKey, bobPublicKey);
            Curve25519XSalsa20Poly1305 bobBox = new Curve25519XSalsa20Poly1305(bobSecretKey, alicePublicKey);

            // Generating random nonce
            byte[] nonce = new byte[Curve25519XSalsa20Poly1305.NonceLength];
            rng.GetBytes(nonce);

            // Plaintext message
            byte[] message = Encoding.UTF8.GetBytes("Hey Bob");

            // Prepare the buffer for the ciphertext, must be message length and extra 16 bytes for the authentication tag
            byte[] cipher = new byte[message.Length + Curve25519XSalsa20Poly1305.TagLength];

            // Encrypting using alice box
            aliceBox.Encrypt(cipher, message, nonce);

            // Decrypting using bob box
            byte[] plain = new byte[cipher.Length - Curve25519XSalsa20Poly1305.TagLength];
            bool isVerified = bobBox.TryDecrypt(plain, cipher, nonce);

            Console.WriteLine("Verified: {0}", isVerified);
            Console.WriteLine("Message: {0}", Encoding.UTF8.GetString(plain));
        }
    }
}