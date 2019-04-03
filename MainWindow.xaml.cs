using System;
using System.Drawing;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
//using System.Windows.Interop;

namespace CiselneSoustavy
{

    /*
    static class ExtensionsForWPF
    {

        public static System.Windows.Forms.Screen GetScreen(this Window window)
        {

            return System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(window).Handle);

        }

    }
    */

    public partial class MainWindow : Window
    {

        string Numbers = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
 
        public MainWindow()
        {

            InitializeComponent();

        }

        private void MainWindow1_Initialized(object sender, EventArgs e)
        {
            CBOperation_DropDownClosed(sender, null);
            EFromSystemNumber1.Focus();
        }

        public class InvalidNumberSystem: Exception
        {

            public InvalidNumberSystem() { }

        }
        /*
        private int GetTextLength(string S, System.Windows.Media.FontFamily FontFamily, double FontSize)
        {

            FormattedText Text = new FormattedText(S,
                                                   System.Globalization.CultureInfo.GetCultureInfo("en-us"),
                                                   FlowDirection.LeftToRight,
                                                   new Typeface(FontFamily.ToString()),
                                                   FontSize,
                                                   System.Windows.Media.Brushes.Black);

            return (int) Math.Ceiling(Text.Width + 8);

        }
        */
/*
        private double GetMonitorWidth()
        {

            System.Windows.Forms.Screen CurrentScreen = ExtensionsForWPF.GetScreen(this);
            return CurrentScreen.WorkingArea.Width;

        }
*/
        private void CenterWindowOnCurrentMonitor()
        {
            /*
            System.Windows.Forms.Screen CurrentScreen = ExtensionsForWPF.GetScreen(this);
            double MonitorWidth = CurrentScreen.WorkingArea.Width;
            double ScreenLeft = CurrentScreen.Bounds.Left;
            double WindowWidth = this.Width;

           // this.Left = ScreenLeft + (MonitorWidth / 2) - (WindowWidth / 2);
           */
        }

        private bool NumberValid(string Number, Int16 NumSystem)
        {

            if ((NumSystem < 2) || (NumSystem > 36))
                throw new InvalidNumberSystem();

            int I = 0;

            string ValidNumbers = Numbers.Substring(0, NumSystem) + "+-";

            if (Number != "")
                while ((I < Number.Length) && (ValidNumbers.IndexOf(Char.ToUpper(Number[I])) > -1))
                    I++;

            return I == Number.Length;

        }

        private Int16 GetDigitValue(char Digit)
        {

            Int16 Value = Convert.ToInt16(Numbers.IndexOf(Char.ToUpper(Digit)));

            if (Value == -1)
                throw new InvalidNumberSystem();
            else return Value;

        }

        private char GetDigit(Int16 Value)
        {

            if ((Value < 0) || (Value > 35))
                throw new InvalidNumberSystem();

            return Numbers[Value];             

        }

        private void PrepareForOperation(ref string Number1, ref string Number2, Int16 NumSystem1, Int16 NumSystem2, Int16 ToNumSystem)
        {

            if (NumSystem1 != 0) if ((NumSystem1 < 2) || (NumSystem1 > 36))
                throw new InvalidNumberSystem();

            if (NumSystem2 != 0) if ((NumSystem2 < 2) || (NumSystem2 > 36))
                throw new InvalidNumberSystem();

            if (ToNumSystem != 0) if ((ToNumSystem < 2) || (ToNumSystem > 36))
                throw new InvalidNumberSystem();

            if (Number1 != "") if (!NumberValid(Number1, NumSystem1))
                throw new InvalidNumberSystem();

            if (Number2 != "") if (!NumberValid(Number2, NumSystem2))
                throw new InvalidNumberSystem();

            Number1 = Number1.TrimStart('0');
            Number2 = Number2.TrimStart('0');

            if (Number1 == "") Number1 = "0";
            if (Number2 == "") Number2 = "0";

        }

        private string Add(string Number1, string Number2, Int16 NumSystem1, Int16 NumSystem2, Int16 ToNumSystem)
        {

            PrepareForOperation(ref Number1, ref Number2, NumSystem1, NumSystem2, ToNumSystem);

            BigInteger Number1InSystem10 = ConvertToSystem10FromAny(Number1, NumSystem1);
            BigInteger Number2InSystem10 = ConvertToSystem10FromAny(Number2, NumSystem2);

            BigInteger SumInSystem10 = BigInteger.Add(Number1InSystem10, Number2InSystem10);

            return ConvertToSystemAnyFrom10(SumInSystem10, ToNumSystem);

        }

        private string Subtract(string Number1, string Number2, Int16 NumSystem1, Int16 NumSystem2, Int16 ToNumSystem)
        {

            PrepareForOperation(ref Number1, ref Number2, NumSystem1, NumSystem2, ToNumSystem);

            BigInteger Number1InSystem10 = ConvertToSystem10FromAny(Number1, NumSystem1);
            BigInteger Number2InSystem10 = ConvertToSystem10FromAny(Number2, NumSystem2);

            BigInteger DifferenceInSystem10 = BigInteger.Subtract(Number1InSystem10, Number2InSystem10);

            return ConvertToSystemAnyFrom10(DifferenceInSystem10, ToNumSystem);

        }

        private int CompareNumbers(string Number1, string Number2, Int16 NumSystem)
        {

            PrepareForOperation(ref Number1, ref Number2, NumSystem, NumSystem, 0);

            BigInteger Number1InSystem10 = ConvertToSystem10FromAny(Number1, NumSystem);
            BigInteger Number2InSystem10 = ConvertToSystem10FromAny(Number2, NumSystem);

            int Comparison = BigInteger.Compare(Number1InSystem10, Number2InSystem10);

            if (Comparison < 0)
                return -1;
            else if (Comparison == 0)
                return 0;
            else if (Comparison > 0)
                return 1;
            else throw new Exception("Exception in CompareNumbers");

        }

        private string Multiply(string Number1, string Number2, Int16 NumSystem1, Int16 NumSystem2, Int16 ToNumSystem)
        {

            PrepareForOperation(ref Number1, ref Number2, NumSystem1, NumSystem2, ToNumSystem);
            
            BigInteger Number1InSystem10 = ConvertToSystem10FromAny(Number1, NumSystem1);
            BigInteger Number2InSystem10 = ConvertToSystem10FromAny(Number2, NumSystem2);

            BigInteger ProductInSystem10 = BigInteger.Multiply(Number1InSystem10, Number2InSystem10);

            return ConvertToSystemAnyFrom10(ProductInSystem10, ToNumSystem);

        }

        private string Divide(string Number1, string Number2, Int16 NumSystem1, Int16 NumSystem2, Int16 ToNumSystem)
        {

            PrepareForOperation(ref Number1, ref Number2, NumSystem1, NumSystem2, ToNumSystem);

            if (Number2 == "0") return "Nulou dělit nelze";

            BigInteger Number1InSystem10 = ConvertToSystem10FromAny(Number1, NumSystem1);
            BigInteger Number2InSystem10 = ConvertToSystem10FromAny(Number2, NumSystem2);

            BigInteger QuotientInSystem10 = BigInteger.Divide(Number1InSystem10, Number2InSystem10);

            return ConvertToSystemAnyFrom10(QuotientInSystem10, ToNumSystem);

        }

        private string Modulus(string Number1, string Number2, Int16 NumSystem1, Int16 NumSystem2, Int16 ToNumSystem)
        {

            PrepareForOperation(ref Number1, ref Number2, NumSystem1, NumSystem2, ToNumSystem);

            if (Number2 == "0") return "Nulou dělit nelze";

            BigInteger Number1InSystem10 = ConvertToSystem10FromAny(Number1, NumSystem1);
            BigInteger Number2InSystem10 = ConvertToSystem10FromAny(Number2, NumSystem2);

            BigInteger ModulusInSystem10 = BigInteger.ModPow(Number1InSystem10, 1, Number2InSystem10);

            return ConvertToSystemAnyFrom10(ModulusInSystem10, ToNumSystem);

        }

        private string Power(string Number, int Exponent, Int16 NumSystem, Int16 ToNumSystem)
        {

            string Exp = "";

            PrepareForOperation(ref Number, ref Exp, NumSystem, 0, ToNumSystem);

            if (Exponent == 0) return "1"; else
            if (Exponent == 1) return Number;

            BigInteger NumberInSystem10 = ConvertToSystem10FromAny(Number, NumSystem);

            BigInteger PowerInSystem10 = BigInteger.Pow(NumberInSystem10, Exponent);

            return ConvertToSystemAnyFrom10(PowerInSystem10, ToNumSystem);

        }

        private BigInteger ConvertToSystem10FromAny(string Number, Int16 FromSystem)
        {

            string Number2 = "";
            PrepareForOperation(ref Number, ref Number2, FromSystem, 0, 0);

            bool Minus = (Number[0] == '-');
            bool Plus  = (Number[0] == '+');

            if (Minus || Plus) Number = Number.Substring(1);

            if (Number == "") return 0;

            if (FromSystem != 10)
            {

                BigInteger Addend = 0;
                BigInteger NumberInSystem10 = 0;

                for (int I = 0; I < Number.Length; I++)
                {

                    Addend = BigInteger.Multiply(GetDigitValue(Number[Number.Length - I - 1]), BigInteger.Pow(FromSystem, I));

                    NumberInSystem10 = BigInteger.Add(NumberInSystem10, Addend);

                }

                if (Minus)
                    return -NumberInSystem10;
                else
                    return NumberInSystem10;

            }
            else if (Minus)
                    return BigInteger.Parse('-' + Number);
                 else
                    return BigInteger.Parse(Number);
            
        }

        private string ConvertToSystemAnyFrom10(BigInteger Number, Int16 ToSystem)
        {

            string Number1 = "";
            string Number2 = "";
            PrepareForOperation(ref Number1, ref Number2, 0, 0, ToSystem);
            
            if (ToSystem != 10)
            {

                bool Minus = (Number < 0);

                if (Minus) Number *= -1;

                BigInteger Modulus = 0;

                StringBuilder SystemAny = new StringBuilder(1000);

                do {

                    Number = BigInteger.DivRem(Number, ToSystem, out Modulus);
                    
                    SystemAny.Insert(0, GetDigit((short) Modulus));

                } while (Number > 0);

                if (Minus)
                    return '-' + SystemAny.ToString();
                else
                    return SystemAny.ToString();

            }
            else return Number.ToString();

        }
        
        private string ConvertSystem(string Number, Int16 FromSystem, Int16 ToSysten)
        {

            string Number2 = "";
            PrepareForOperation(ref Number, ref Number2, FromSystem, 0, ToSysten);
            
            BigInteger NumberInSystem10 = ConvertToSystem10FromAny(Number, FromSystem);

            return ConvertToSystemAnyFrom10(NumberInSystem10, ToSysten);

        }

        private void ESystemNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

            string Number1 = EFromSystemNumber1.Text;
            string Number2 = EFromSystemNumber2.Text;
            Int16 NumSystem1 = Convert.ToInt16(CBFromSystem1.SelectedIndex + 2);
            Int16 NumSystem2 = Convert.ToInt16(CBFromSystem2.SelectedIndex + 2);
            Int16 ToNumSystem = Convert.ToInt16(CBToSystem.SelectedIndex + 2);

            if ((EFromSystemNumber2.Visibility == Visibility.Visible) && ((Number1 == "") || (Number2 == "")))
            {
                EToSystemNumber.Text = "";
                return;
            }

            if ((EFromSystemNumber2.Visibility == Visibility.Hidden) && (Number1 == ""))
            {
                EToSystemNumber.Text = "";
                return;
            }
            
            switch (CBOperation.SelectedIndex)
            {

                case 0:
                    try
                    {
                        EToSystemNumber.Text = ConvertSystem(Number1, NumSystem1, ToNumSystem);
                    }
                    catch (InvalidNumberSystem)
                    {
                        EToSystemNumber.Text = "Číslo nespadá do zadané soustavy.";
                    }
                    break;

                case 1:
                    try
                    {
                        EToSystemNumber.Text = Add(Number1, Number2, NumSystem1, NumSystem2, ToNumSystem);
                    }
                    catch (InvalidNumberSystem)
                    {
                        EToSystemNumber.Text = "Číslo nespadá do zadané soustavy.";
                    }
                    break;

                case 2:
                    try
                    {
                        EToSystemNumber.Text = Subtract(Number1, Number2, NumSystem1, NumSystem2, ToNumSystem);
                    }
                    catch (InvalidNumberSystem)
                    {
                        EToSystemNumber.Text = "Číslo nespadá do zadané soustavy.";
                    }
                    break;

                case 3:
                    try
                    {
                        EToSystemNumber.Text = Multiply(Number1, Number2, NumSystem1, NumSystem2, ToNumSystem);
                    }
                    catch (InvalidNumberSystem)
                    {
                        EToSystemNumber.Text = "Číslo nespadá do zadané soustavy.";
                    }
                    break;

                case 4:
                    try
                    {
                        EToSystemNumber.Text = Divide(Number1, Number2, NumSystem1, NumSystem2, ToNumSystem);
                    }
                    catch (InvalidNumberSystem)
                    {
                        EToSystemNumber.Text = "Číslo nespadá do zadané soustavy.";
                    }
                    break;

                case 5:
                    try
                    {
                        EToSystemNumber.Text = Modulus(Number1, Number2, NumSystem1, NumSystem2, ToNumSystem);
                    }
                    catch (InvalidNumberSystem)
                    {
                        EToSystemNumber.Text = "Číslo nespadá do zadané soustavy.";
                    }
                    break;

                case 6:
                    try
                    {
                        int Tmp;
                        if (!int.TryParse(Number2, out Tmp))
                            EToSystemNumber.Text = "Exponent musí být desítkové číslo";
                        else
                            EToSystemNumber.Text = Power(Number1, Convert.ToInt32(Number2), NumSystem1, ToNumSystem);
                    }
                    catch (InvalidNumberSystem)
                    {
                        EToSystemNumber.Text = "Číslo nespadá do zadané soustavy.";
                    }
                    break;

            }
    

            /*
            int MinimumLength = 259;
            int ELength1 = GetTextLength(EFromSystemNumber1.Text, EFromSystemNumber1.FontFamily, EFromSystemNumber1.FontSize);
            int ELength2;
            if (EFromSystemNumber2.Visibility == Visibility.Visible) ELength2 = GetTextLength(EFromSystemNumber2.Text, EFromSystemNumber2.FontFamily, EFromSystemNumber2.FontSize);
            else ELength2 = 0;
            int ELength3 = GetTextLength(EToSystemNumber.Text, EToSystemNumber.FontFamily, EToSystemNumber.FontSize);

            int MinLength = Math.Max(ELength1, ELength2);
            MinLength = Math.Max(MinLength, ELength3);
            MinLength = Math.Max(MinLength, MinimumLength);

            int MaxLength = (int) GetMonitorWidth() - 150;

            int RealLength = Math.Min(MinLength, MaxLength);

            EFromSystemNumber1.Width = RealLength;
            EFromSystemNumber2.Width = RealLength;
            EToSystemNumber.Width = RealLength;

            MainWindow1.Width = RealLength + 65;

            CenterWindowOnCurrentMonitor();
            */
        }

        private void CBFromSystem_DropDownClosed(object sender, EventArgs e)
        {

            ESystemNumber_TextChanged(sender, null);

        }

        private void CBToSystem_DropDownClosed(object sender, EventArgs e)
        {

            ESystemNumber_TextChanged(sender, null);

        }

        private void CBOperation_DropDownClosed(object sender, EventArgs e)
        {
            
            if (CBOperation.SelectedIndex == 0)
            {

                EFromSystemNumber2.Visibility = Visibility.Hidden;
                CBFromSystem2.Visibility = Visibility.Hidden;

                EFromSystemNumber2.Text = "";
                CBFromSystem2.SelectedIndex = CBFromSystem1.SelectedIndex;

            }
            else
            {

                EFromSystemNumber2.Visibility = Visibility.Visible;
                CBFromSystem2.Visibility = Visibility.Visible;

            }

            ESystemNumber_TextChanged(sender, null);

            if (CBOperation.SelectedIndex == 6)
            {
                CBFromSystem2.IsEnabled = false;
                CBFromSystem2.SelectedIndex = 8;
            }
            else
            {
                CBFromSystem2.IsEnabled = true;
            }

            EFromSystemNumber1.Focus();

        }
        
    }

}