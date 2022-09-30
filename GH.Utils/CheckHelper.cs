using System;
using System.Text.RegularExpressions;

namespace GH.Utils
{
    public static class CheckHelper
    {
        public static string FormatAdress(string address)
        {
            address = " " + address.Replace(" .", ".").Replace(".", ". ").Replace("  ", " ");
            string[] comaChars = { "," };
            string[] splitComa = address.Split(comaChars, StringSplitOptions.RemoveEmptyEntries);
            address = "";
            foreach (string textComa in splitComa)
            {
                string[] dotChars = { ".", " " };
                string[] splitDot = textComa.Split(dotChars, StringSplitOptions.RemoveEmptyEntries);
                string clearTextComa = textComa;
                string newTextComa = "";

                bool toUp = false;
                foreach (string item in splitDot)
                {
                    int pos = clearTextComa.IndexOf(" " + item);
                    int posDot = clearTextComa.IndexOf(" " + item + ".");

                    if (posDot >= 0)
                    {
                        newTextComa += item + ". ";
                        toUp = true;
                    }
                    else
                    if (pos >= 0)
                    {
                        if (toUp)
                        {
                            newTextComa += item.WordToProperCase() + " ";
                            toUp = false;
                        }
                        else
                            newTextComa += item + " ";
                    }


                }

                if (address == "")
                    address = newTextComa.Trim();
                else
                    address += ", " + clearTextComa.Trim();
            }
            return address.Trim();
        }


        public static bool IsZip(string zip)
        {
            if (zip.Length == 6)
            {
                string pattern = "[0-9]{6}";
                Match isMatch = Regex.Match(zip, pattern, RegexOptions.IgnoreCase);

                return isMatch.Success;
            }
            else
                return false;
        }

        public static bool IsPhone(string phone)
        {

            string pattern = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";
            Match isMatch = Regex.Match(phone, pattern, RegexOptions.IgnoreCase);

            return isMatch.Success;
        }

        public static bool EmailIsValid(string email)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);

            if (isMatch.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string CountSumm(string barcode)
        {
            if (barcode.Length < 12)
                return "";

            barcode = barcode.Substring(0, 12);
            int cnt = 0;
            int sum = 0;
            for (int j = barcode.Length - 1; j > -1; j--)
            {
                cnt += 1;
                if (cnt % 2 == 0)
                {
                    sum += int.Parse(barcode[j].ToString());
                }
                else
                {
                    sum += int.Parse(barcode[j].ToString()) * 3;
                }
            }
            sum = (10 - (sum % 10)) % 10;

            return sum.ToString();

        }

        private static bool Ean13(string barcode)
        {
            if (barcode.Substring(barcode.Length - 1, 1) == CountSumm(barcode))
                return true;
            return false;
        }

        public static string CheckBarcode(string barcode)
        {
            string cleaned = Regex.Replace(barcode.Trim(), @"\D", "");
            string result = cleaned;
            if (result.Length <= 13 && result.Length >= 10)
            {
                while (result.Length < 13) // не хватает впереди
                    result = "0" + result;

                if (!Ean13(result)) // проверяем контрольную сумму 
                {
                    if (cleaned.Length == 13)
                    {
                        // неправильная контрольная сумма 
                        result = result.Substring(0, 12);
                    }
                    else
                    {
                        // нехватает контрольной суммы 
                        result = cleaned;
                        while (result.Length < 12)
                            result = "0" + result;
                    }
                    result += CountSumm(result); // добавляем контрольную сумму
                }
            }
            else
            {
                result = barcode.Trim();
            }

            return result;
        }



    }
}
