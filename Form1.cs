using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1_Colors_translations
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // расширенное окно для выбора цвета
            colorDialog1.FullOpen = true;
            // установка начального цвета для colorDialog
            colorDialog1.Color = this.BackColor;
        }


        void setColor(object sender, EventArgs e)
        {
            double C = Double.Parse(textBox11.Text);
            double M = Double.Parse(textBox12.Text);
            double Y = Double.Parse(textBox13.Text);
            double K = Double.Parse(textBox14.Text);

            String errStr = "";
            if (C > 1)
            {
                errStr += "C was lowed; ";
                C = 1;
            }
            if (M > 1)
            {
                errStr += "M was lowed; ";
                M = 1;
            }
            if (Y > 1)
            {
                errStr += "Y was lowed; ";
                Y = 1;
            }
            if (K > 1)
            {
                errStr += "K was lowed; ";
                K = 1;
            }
            if (C < 0)
            {
                errStr += "C was upped; ";
                C = 0;
            }
            if (M < 0)
            {
                errStr += "M was upped; ";
                M = 0;
            }
            if (Y < 0)
            {
                errStr += "Y was upped; ";
                Y = 0;
            }
            if (K < 0)
            {
                errStr += "K was upped; ";
                K = 0;
            }
            ep1.SetError(lErr1, errStr);
            ep2.Clear();
            SetCMYKvalues(C, M, Y, K);

            double Rd = 255 * (1 - C) * (1 - K);
            double Gd = 255 * (1 - M) * (1 - K);
            double Bd = 255 * (1 - Y) * (1 - K);

            Int32 R = (Int32)Math.Round(Rd);
            Int32 G = (Int32)Math.Round(Gd);
            Int32 B = (Int32)Math.Round(Bd);

            lColor.BackColor = Color.FromArgb(R, G, B);

            //CMYK to HLS -> RGB to HLS
            double h, l, s;
            RgbToHls(R, G, B, out h, out l, out s);
            SetHLSvalues(h, l, s);
            RGBtoXYZ(R, G, B);
        }

        private void SetHLSvalues(double h, double l, double s)
        {
            textBox21.Text = ((Int32)h).ToString();
            textBox22.Text = Math.Round(s, 3).ToString();
            textBox23.Text = Math.Round(l, 3).ToString();

            tb21.Value = (Int32)(h);
            tb22.Value = (Int32)(s * 1000);
            tb23.Value = (Int32)(l * 1000);
        }
        private void SetCMYKvalues(double c, double m, double y, double k)
        {
            textBox11.Text = Math.Round(c, 3).ToString();
            textBox12.Text = Math.Round(m, 3).ToString();
            textBox13.Text = Math.Round(y, 3).ToString();
            textBox14.Text = Math.Round(k, 3).ToString();

            tb11.Value = (Int32)(c * 1000);
            tb12.Value = (Int32)(m * 1000);
            tb13.Value = (Int32)(y * 1000);
            tb14.Value = (Int32)(k * 1000);
        }


        private void RgbToHls(int r, int g, int b,
            out double h, out double l, out double s)
        {
            // Convert RGB to a 0.0 to 1.0 range.
            double double_r = r / 255.0;
            double double_g = g / 255.0;
            double double_b = b / 255.0;

            // Get the maximum and minimum RGB components.
            double max = double_r;
            if (max < double_g) max = double_g;
            if (max < double_b) max = double_b;

            double min = double_r;
            if (min > double_g) min = double_g;
            if (min > double_b) min = double_b;

            double diff = max - min;
            l = (max + min) / 2;
            if (Math.Abs(diff) < 0.00001)
            {
                s = 0;
                h = 0;  // H is really undefined.
            }
            else
            {
                if (l <= 0.5) s = diff / (max + min);
                else s = diff / (2 - max - min);

                double r_dist = (max - double_r) / diff;
                double g_dist = (max - double_g) / diff;
                double b_dist = (max - double_b) / diff;

                if (double_r == max) h = b_dist - g_dist;
                else if (double_g == max) h = 2 + r_dist - b_dist;
                else h = 4 + g_dist - r_dist;

                h = h * 60;
                if (h < 0) h += 360;
            }
        }


        private void Tb11_Scroll(object sender, EventArgs e)
        {
            textBox11.Text = ((double)tb11.Value / 1000).ToString();
        }

        private void Tb12_Scroll(object sender, EventArgs e)
        {
            textBox12.Text = ((double)tb12.Value / 1000).ToString();
        }

        private void Tb13_Scroll(object sender, EventArgs e)
        {
            textBox13.Text = ((double)tb13.Value / 1000).ToString();
        }

        private void Tb14_Scroll(object sender, EventArgs e)
        {
            textBox14.Text = ((double)tb14.Value / 1000).ToString();
        }

        private void Tb21_Scroll(object sender, EventArgs e)
        {
            textBox21.Text = tb21.Value.ToString();
        }

        private void Tb22_Scroll(object sender, EventArgs e)
        {
            textBox22.Text = ((double)tb22.Value / 1000).ToString();
        }

        private void Tb23_Scroll(object sender, EventArgs e)
        {
            textBox23.Text = ((double)tb23.Value / 1000).ToString();
        }
        private Color HLStoRGB(int h, double s, double l)
        {
            if (s == 0)
            {
                return Color.FromArgb(
                    Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                        l * 255.0))),
                    Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                        l * 255.0))),
                    Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                        l * 255.0)))
                    );
            }
            else
            {
                double q = (l < 0.5) ? (l * (1.0 + s)) : (l + s - (l * s));
                double p = (2.0 * l) - q;

                double Hk = h / 360.0;
                double[] T = new double[3];
                T[0] = Hk + (1.0 / 3.0);    // Tr
                T[1] = Hk;                // Tb
                T[2] = Hk - (1.0 / 3.0);    // Tg

                for (int i = 0; i < 3; i++)
                {
                    if (T[i] < 0) T[i] += 1.0;
                    if (T[i] > 1) T[i] -= 1.0;

                    if ((T[i] * 6) < 1)
                    {
                        T[i] = p + ((q - p) * 6.0 * T[i]);
                    }
                    else if ((T[i] * 2.0) < 1) //(1.0/6.0)<=T[i] && T[i]<0.5
                    {
                        T[i] = q;
                    }
                    else if ((T[i] * 3.0) < 2) // 0.5<=T[i] && T[i]<(2.0/3.0)
                    {
                        T[i] = p + (q - p) * ((2.0 / 3.0) - T[i]) * 6.0;
                    }
                    else T[i] = p;
                }

                return Color.FromArgb(
                    Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                        T[0] * 255.0))),
                    Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                        T[1] * 255.0))),
                    Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                        T[2] * 255.0)))
                    );
            }
        }


        private void BtSet2_Click(object sender, EventArgs e)
        {
            Int32 h = Int32.Parse(textBox21.Text);
            Double s = Double.Parse(textBox22.Text);
            Double l = Double.Parse(textBox23.Text);

            String errStr = "";

            if (h > 360)
            {
                h = 360;
                errStr += "h was lowed; ";
            }
            if (s > 1)
            {
                s = 1;
                errStr += "s was lowed; ";
            }
            if (l > 1)
            {
                l = 1;
                errStr += "l was lowed; ";
            }
            if (h < 0)
            {
                h = 0;
                errStr += "h was upped; ";
            }
            if (s < 0)
            {
                s = 0;
                errStr += "s was upped; ";
            }
            if (l < 0)
            {
                l = 0;
                errStr += "l was upped; ";
            }
            ep2.SetError(lErr2, errStr);
            ep1.Clear();
            Color color = HLStoRGB(h, s, l);

            SetHLSvalues(h, l, s);
            lColor.BackColor = color;

            //перевожу HLS в CMYK
            RGBtoCMYK(color);
            RGBtoXYZ(color.R, color.G, color.B);
        }

        private double Min3(double a, double b, double c)
        {
            double min = a;
            if (b < min)
                min = b;
            if (c < min)
                min = c;
            return min;
        }

        private void RGBtoCMYK(Color color)
        {
            double R = color.R;
            double G = color.G;
            double B = color.B;

            double K = Math.Round(Min3(1.0 - R / 255, 1.0 - G / 255, 1.0 - B / 255), 3);
            double C = 0;
            double M = 0;
            double Y = 0;
            if (K != 1) //чёрный
            {
                C = Math.Round((1.0 - R / 255 - K) / (1.0 - K), 3);
                M = Math.Round((1.0 - G / 255 - K) / (1.0 - K), 3);
                Y = Math.Round((1.0 - B / 255 - K) / (1.0 - K), 3);
            }
            C = C > 0 ? C : 0;
            M = M > 0 ? M : 0;
            Y = Y > 0 ? Y : 0;

            textBox11.Text = C.ToString();
            textBox12.Text = M.ToString();
            textBox13.Text = Y.ToString();
            textBox14.Text = K.ToString();

            tb11.Value = (Int32)(C * 1000);
            tb12.Value = (Int32)(M * 1000);
            tb13.Value = (Int32)(Y * 1000);
            tb14.Value = (Int32)(K * 1000);
        }

        private void Tb31_Scroll(object sender, EventArgs e)
        {
            textBox31.Text = ((double)tb31.Value / 1000).ToString();
        }

        private void Tb32_Scroll(object sender, EventArgs e)
        {
            textBox32.Text = ((double)tb32.Value / 1000).ToString();
        }

        private void Tb33_Scroll(object sender, EventArgs e)
        {
            textBox33.Text = ((double)tb33.Value / 1000).ToString();
        }

        //XYZ какие макс значения, почему система переводов из лекции не работает?
        private Color XYZtoRGB(double x, double y, double z)
        {

            double[] Clinear = new double[3];
            Clinear[0] = x * 3.2404 - y * 1.5371 - z * 0.4985; // красный
            Clinear[1] = -x * 0.9692 + y * 1.8760 + z * 0.0415; // зеленый
            Clinear[2] = x * 0.0556 - y * 0.2040 + z * 1.0572; // синий

            for (int i = 0; i < 3; i++)
            {
                Clinear[i] = f(Clinear[i]);
                Clinear[i] = normalize(Clinear[i]);
            }

            return Color.FromArgb(
                Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                    Clinear[0] * 255.0))),
                Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                    Clinear[1] * 255.0))),
                Convert.ToInt32(Double.Parse(String.Format("{0:0.00}",
                    Clinear[2] * 255.0)))
                );
        }
        private void RGBtoXYZ(double red, double green, double blue)
        {
            // нормализовать значения красного, зеленого, синего
            double rLinear = red / 255.0;
            double gLinear = green / 255.0;
            double bLinear = blue / 255.0;

            // преобразовать в форму sRGB
            double r = (rLinear > 0.04045) ? Math.Pow((rLinear + 0.055) / (
                1 + 0.055), 2.2) : (rLinear / 12.92);
            double g = (gLinear > 0.04045) ? Math.Pow((gLinear + 0.055) / (
                1 + 0.055), 2.2) : (gLinear / 12.92);
            double b = (bLinear > 0.04045) ? Math.Pow((bLinear + 0.055) / (
                1 + 0.055), 2.2) : (bLinear / 12.92);

            // преобразует
            double x = r * 0.4124 + g * 0.3576 + b * 0.1805;
            double y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            double z = r * 0.0193 + g * 0.1192 + b * 0.9505;

            SetXYZvalues(x, y, z);
        }
        private void SetXYZvalues(double x, double y, double z)
        { 
            x = normalize(x);
            y = normalize(y);
            z = normalize(z);

            textBox31.Text = Math.Round(x, 3).ToString();
            textBox32.Text = Math.Round(y, 3).ToString();
            textBox33.Text = Math.Round(z, 3).ToString();
            
            tb31.Value = (Int32)(x * 1000);
            tb32.Value = (Int32)(y * 1000);
            tb33.Value = (Int32)(z * 1000);
        }

        private double f(double x)
        {
            if (Math.Abs(x) < 0.0031308)
                return 12.92 * x;
            return 1.055 * Math.Pow(Math.Abs(x), 0.41666) - 0.055;
        }

        private double normalize(double x)
        {
            if (x > 1.0)
                x = 1.0;
            if (x < 0.0)
                x = 0.0;
            return x;
        }

        private void BtSet3_Click(object sender, EventArgs e)
        {
            double x = Double.Parse(textBox31.Text);
            double y = Double.Parse(textBox32.Text);
            double z = Double.Parse(textBox33.Text);

            Color color = XYZtoRGB(x, y, z);
            lColor.BackColor = color;

            double h, l, s;
            RgbToHls(color.R, color.G, color.B, out h, out l, out s);
            SetHLSvalues(h, l, s);
            RGBtoCMYK(color);
        }

        private void BtChoose_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            Color color = colorDialog1.Color;
            lColor.BackColor = color;

            double h, l, s;
            RGBtoCMYK(color);
            RgbToHls(color.R, color.G, color.B, out h, out l, out s);
            SetHLSvalues(h, l, s);
            SetXYZvalues(color.R, color.G, color.B);
        }

    }
}
