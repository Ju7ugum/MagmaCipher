using System;
namespace MyMagma
{
    class Program
    {
        static void Main(string[] args)
        {
            SecureMagma.Gen_Keys(); // Генерация раундовых ключей.

            while (true)
            {
                Console.WriteLine("Меню: ");
                Console.WriteLine("1 - Вывести ключи для MAGMA");
                Console.WriteLine("2 - Зашифровать текст по MAGMA");
                Console.WriteLine("3 - Расшифровать текст по MAGMA");
                Console.WriteLine("4 - Гаммирование с обратной связью");
                Console.WriteLine("5 - Расшифровать текст в режиме гаммирования с обратной связью");

                int choose = Convert.ToInt32(Console.ReadLine());

                if (choose == 1)
                {
                    Console.WriteLine("Ключи MAGMA:");
                    foreach (uint key in SecureMagma.keys)
                        Console.WriteLine(key.ToString("X").PadLeft(8, '0'));
                }
                else if (choose == 2)
                {
                    Console.WriteLine("Введите текст для шифрования по MAGMA:");
                    string plaintext = Console.ReadLine();
                    string ciphertext = SecureMagma.EncryptMagma(plaintext);
                    Console.WriteLine($"Зашифрованный текст по MAGMA: {ciphertext}");
                }
                else if (choose == 3)
                {
                    Console.WriteLine("Введите зашифрованный текст по MAGMA:");
                    string ciphertext = Console.ReadLine();
                    string decryptedText = SecureMagma.DecryptMagma(ciphertext);
                    Console.WriteLine($"Расшифрованный текст по MAGMA: {decryptedText}");
                }
                else if (choose == 4)
                {
                    Console.WriteLine("Введите текст для гаммирования:");
                    string plaintext = Console.ReadLine();
                    string ciphertext = SecureMagma.EncryptFeedbackMode(plaintext);
                    Console.WriteLine($"Зашифрованный текст с обратной связью: {ciphertext}");
                }
                else if (choose == 5)
                {
                    Console.WriteLine("Введите зашифрованный текст для расшифрования в режиме гаммирования с обратной связью:");
                    string ciphertext = Console.ReadLine();
                    string plaintext = SecureMagma.DecryptFeedbackMode(ciphertext);
                    Console.WriteLine($"Расшифрованный текст в режиме гаммирования с обратной связью: {plaintext}");
                }
            }
        }
    }
}