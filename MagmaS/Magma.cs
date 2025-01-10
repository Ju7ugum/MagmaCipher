using System;
using System.Collections.Generic;

namespace MyMagma
{
    // Класс для шифрования и дешифрования с использованием алгоритма Magma.
    // Реализует генерацию раундовых ключей, операцию XOR и циклические сдвиги.
    class SecureMagma
    {
        // Ключ шифрования для Magma: 
        public static string key = "ffeeddccbbaa99887766554433221100f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff";
        public static List<uint> keys = new List<uint>();
        public static int[,] SBlock = {
            {1, 7, 14, 13, 0, 5, 8, 3, 4, 15, 10, 6, 9, 12, 11, 2},
            {8, 14, 2, 5, 6, 9, 1, 12, 15, 4, 11, 0, 13, 10, 3, 7},
            {5, 13, 15, 6, 9, 2, 12, 10, 11, 7, 8, 1, 4, 3, 14, 0},
            {7, 15, 5, 10, 8, 1, 6, 13, 0, 9, 3, 14, 11, 4, 2, 12},
            {12, 8, 2, 1, 13, 4, 15, 6, 7, 0, 10, 5, 3, 14, 9, 11},
            {11, 3, 5, 8, 2, 15, 10, 13, 14, 1, 7, 4, 12, 9, 6, 0},
            {6, 8, 2, 3, 9, 10, 5, 12, 1, 14, 4, 7, 11, 13, 0, 15},
            {12, 4, 6, 2, 10, 5, 11, 9, 14, 8, 13, 7, 0, 3, 15, 1},
        };

        // Генерация раундовых ключей: 
        public static void Gen_Keys()
        {
            // Повторяем 3 раза для создания 96 байт (256 бит) ключей:
            for (int q = 0; q < 3; q++)
                // Разбиваем строку ключа на блоки по 8 символов и преобразуем их в UInt32:
                for (int i = 0; i < key.Length - 1; i += 8)
                    keys.Add(Convert.ToUInt32(key.Substring(i, 8), 16));

            // Добавляем ключи в обратном порядке:
            for (int i = key.Length - 8; i >= 0; i -= 8)
                keys.Add(Convert.ToUInt32(key.Substring(i, 8), 16));
        }

        // Функция замены элементов таблицы:
        public static uint Table_replace(uint sumMod2_321)
        {
            string replaceTab = "";
            // Преобразование числа в строку в шестнадцатеричном виде:
            string sumMod2_32Hex = long.Parse(Convert.ToString(sumMod2_321)).ToString("X");
            // Дополнение строки нулями до 8 символов:
            while (sumMod2_32Hex.Length < 8) { sumMod2_32Hex = 0 + sumMod2_32Hex; }

            for (int rowTab = 0; rowTab < sumMod2_32Hex.Length; rowTab++)
            {
                // Получаем номер столбца, преобразуя шестнадцатеричный символ в число:
                long columnTab = Convert.ToInt64(Convert.ToString(sumMod2_32Hex[rowTab]), 16);
                // Находим элемент в таблице SBlock и преобразуем его в шестнадцатеричный вид:
                replaceTab += long.Parse(Convert.ToString(SBlock[rowTab, columnTab])).ToString("X");
            }
            // Преобразуем результат обратно в UInt32:
            return Convert.ToUInt32(replaceTab, 16);
        }

        // Функция операции сложения по модулю 2 в 32 бита
        public static uint Mod2_32(uint N2_UINT, uint keyI_UINT)
        {
            return N2_UINT + keyI_UINT;
        }

        // Функция циклического сдвига:
        public static uint CycleShift_11(uint replaceTab)
        {
            return replaceTab << 11 | replaceTab >> (32 - 11);
        }

        // Функция применения операции XOR:
        public static uint XOR_N1_and_Cycle(uint N1_uint, uint shift_uint)
        {
            return shift_uint ^ N1_uint;
        }

        // Итерация шифрования по ГОСТ 34.12-2015:
        public static uint GOST_34_12_2015_g(uint N1_UINT, uint N2_UINT, uint keyI_UINT)
        {
            uint summod = Mod2_32(N2_UINT, keyI_UINT);
            uint replaceTab = Table_replace(summod);
            uint shift = CycleShift_11(replaceTab);
            uint XOR = XOR_N1_and_Cycle(N1_UINT, shift);
            return XOR;
        }

        // Функция для выполнения шифрования Магма: 
        public static string EncryptMagma(string n)
        {
            if (n.Length < 16)
            {
                Console.WriteLine("Ошибка: пожалуйста, используйте 16 символов.");
                return string.Empty;
            }

            // Разбиваем входную строку на два блока по 8 символов:
            string N1_Hex = n.Substring(0, 8);
            string N2_Hex = n.Substring(8, 8);
            // Преобразуем блоки в UInt32:
            uint N1_UINT = Convert.ToUInt32(N1_Hex, 16);
            uint N2_UINT = Convert.ToUInt32(N2_Hex, 16);

            // 32 раунда шифрования:
            for (int round = 0; round < 32; round++)
            {
                if (round == 31)
                {
                    N1_UINT = GOST_34_12_2015_g(N1_UINT, N2_UINT, keys[round]);
                }
                else
                {
                    uint temp = N1_UINT;
                    N1_UINT = N2_UINT;
                    N2_UINT = GOST_34_12_2015_g(temp, N2_UINT, keys[round]);
                }
            }

            // Форматирование результатов и возврат зашифрованного блока:
            N1_Hex = N1_UINT.ToString("X").PadLeft(8, '0');
            N2_Hex = N2_UINT.ToString("X").PadLeft(8, '0');
            return N1_Hex + N2_Hex;
        }

        // Функция для дешифрования Магма:
        public static string DecryptMagma(string n)
        {
            if (n.Length < 16)
            {
                Console.WriteLine("Ошибка: входная строка слишком короткая.");
                return string.Empty;
            }

            string N1_Hex = n.Substring(0, 8);
            string N2_Hex = n.Substring(8, 8);
            uint N1_UINT = Convert.ToUInt32(N1_Hex, 16);
            uint N2_UINT = Convert.ToUInt32(N2_Hex, 16);

            for (int round = 31; round > -1; round--)
            {
                if (round == 0)
                {
                    N1_UINT = GOST_34_12_2015_g(N1_UINT, N2_UINT, keys[round]);
                }
                else
                {
                    uint temp = N1_UINT;
                    N1_UINT = N2_UINT;
                    N2_UINT = GOST_34_12_2015_g(temp, N2_UINT, keys[round]);
                }
            }

            N1_Hex = N1_UINT.ToString("X").PadLeft(8, '0');
            N2_Hex = N2_UINT.ToString("X").PadLeft(8, '0');
            return N1_Hex + N2_Hex;
        }

        // Функция для шифрования в режиме гаммирования с обратной связью
        public static string EncryptFeedbackMode(string plaintext)
        {
            if (plaintext.Length % 16 != 0)
            {
                Console.WriteLine("Ошибка: длина открытого текста должна быть кратна 16 в режиме обратной связи.");
                return string.Empty;
            }

            string ciphertext = "";
            string feedback = "0000000000000000";

            for (int i = 0; i < plaintext.Length; i += 16)
            {
                // Получаем текущий блок:
                string block = plaintext.Substring(i, 16);
                // Зашифровываем блок и получаем обратную связь:
                string feedbackBlock = EncryptMagma(feedback);
                // Применяем операцию XOR к блоку и обратной связи:
                string encryptedBlock = XORBlocks(block, feedbackBlock);
                // Добавляем зашифрованный блок к выходному тексту:
                ciphertext += encryptedBlock;
                // Обновляем обратную связь для следующего блока:
                feedback = encryptedBlock;
            }

            return ciphertext;
        }
        // Функция для расшифрования в режиме гаммирования с обратной связью
        public static string DecryptFeedbackMode(string ciphertext)
        {
            if (ciphertext.Length % 16 != 0)
            {
                Console.WriteLine("Ошибка: длина зашифрованного текста должна быть кратна 16 в режиме обратной связи.");
                return string.Empty;
            }

            string plaintext = "";
            string feedback = "0000000000000000";

            for (int i = 0; i < ciphertext.Length; i += 16)
            {
                // Получаем текущий блок:
                string block = ciphertext.Substring(i, 16);
                // Зашифровываем обратную связь:
                string feedbackBlock = EncryptMagma(feedback);
                // Применяем операцию XOR к зашифрованному блоку и обратной связи:
                string decryptedBlock = XORBlocks(block, feedbackBlock);
                // Добавляем расшифрованный блок к выходному тексту:
                plaintext += decryptedBlock;
                // Обновляем обратную связь для следующего блока:
                feedback = block;
            }

            return plaintext;
        }

        // Функция для операции XOR между двумя блоками:
        private static string XORBlocks(string block1, string block2)
        {
            if (block1.Length != block2.Length)
            {
                Console.WriteLine("Ошибка: размеры блоков не подходят для операции XOR.");
                return string.Empty;
            }

            // Преобразуем блоки в массив байт:
            byte[] resultBytes = new byte[block1.Length / 2];

            // Применяем операцию XOR к каждому байту:
            for (int i = 0; i < resultBytes.Length; i++)
            {
                resultBytes[i] = (byte)(Convert.ToByte(block1.Substring(i * 2, 2), 16) ^ Convert.ToByte(block2.Substring(i * 2, 2), 16));
            }

            // Преобразуем результат обратно в шестнадцатеричный вид:
            return BitConverter.ToString(resultBytes).Replace("-", "");
        }
    }
}
