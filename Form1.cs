using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Entrega__5.Properties;

namespace Entrega__5
{
    public partial class Form1 : Form
    {
        //Alamcena la ruta del archivo .txt
        public string archivo = "";

        int lineCount = File.ReadLines(@"C:\Users\Desconocido\Desktop\upc\2021-1\IA\1 corte\pesos.txt").Count();
        StreamReader texto = new StreamReader(@"C:\Users\Desconocido\Desktop\upc\2021-1\IA\1 corte\pesos.txt");

        double[,] pesos;
        int[,] matrizentrada = new int[101, 101];
        double[] umbral;

        public float rata_aprendizaje;
        public float Error_maximo;
        public int N_iteraciones;
        public Form1()
        {
            InitializeComponent();
            btn_Entrenar.Enabled = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btn_Entrenar_Click(object sender, EventArgs e)
        {
            entrenar_red();
        }

        private void LeerDatos_Click(object sender, EventArgs e)
        {
            if (validar() == true)
            {
                cargarArchivo();
                btn_Entrenar.Enabled = true;
            }
            else
            {
                MessageBox.Show("Debe llenar el error maximo, N. iteraciones y la rata de aprendizaje");
            }                     
        }

        public bool validar()
        {
            if (string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox3.Text)
                && string.IsNullOrEmpty(textBox4.Text) && string.IsNullOrEmpty(textBox5.Text) && string.IsNullOrEmpty(textBox6.Text) )
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void cargarArchivo()
        {
            try
            {
                this.openFileDialog1.ShowDialog();

                if (!string.IsNullOrEmpty(this.openFileDialog1.FileName))
                {
                    archivo = this.openFileDialog1.FileName;
                    llenar_matriz(';', archivo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
        }

        //almacenar datos del archivo en una matriz
        public void llenar_matriz(char caracter, string archivo)
        {
            StreamReader objReader = new StreamReader(archivo);
            string linea = "";
            int fila = 0;

            do
            {
                linea = objReader.ReadLine();
                if ((linea != null))
                {
                    string[] arreglo = linea.Split(caracter);
                    matrizentrada[fila, 0] = Convert.ToInt32(arreglo[0]);
                    matrizentrada[fila, 1] = Convert.ToInt32(arreglo[1]);
                    matrizentrada[fila, 2] = Convert.ToInt32(arreglo[2]);
                    fila += 1;
                }
            }while (!(linea == null));
            objReader.Close();

            //llenar los umbrales y pesos iniciales

            pesos = new double[1, 2];
            pesos[0, 0] = 0;
            pesos[0, 1] = 0;

            // ---------- aca muestro la info           -----------//

            int info_matriz = patrones();
            int info_entradas = entradas();

            label4.Text = "Patrones :" + lineCount;
            label2.Text = "Entradas : " + info_entradas;
            label3.Text = "Salidas : 1";

            MessageBox.Show("datos cargados correctamente");
        }

        public int patrones()
        {
            string line;

            line = texto.ReadLine();
            string[] partes = line.Split(new Char[] { ';' });

            int salidas = Convert.ToInt32(partes.GetLength(0));
            return salidas;
        }

        public int entradas()
        {
            string line;

            line = texto.ReadLine();
            string[] partes = line.Split(new Char[] { ';' });

            int salidas = Convert.ToInt32(partes.Length);
            return salidas - 1;
        }

        private void entrenar_red()
        {
            int yr1;
            int El1; //error lineal producido
            double Ep; //Error del patron
            double ETP=0; //Error del patron sumatoria
            double s1=0; //funcion soma
            int w11 = int.Parse(textBox4.Text);
            int w21= int.Parse(textBox5.Text);
            int u1 = int.Parse(textBox6.Text);
            rata_aprendizaje = int.Parse(textBox1.Text);
            Error_maximo = float.Parse(textBox2.Text);
            N_iteraciones = int.Parse(textBox3.Text);
            int iteraciones = 1;

            while (iteraciones <= N_iteraciones)
            {
                for (int i = 0; i <= N_iteraciones; i++)
                {
                    //      datos entrenamiento
                    int x1 = matrizentrada[i, 0];
                    int x2 = matrizentrada[i, 1];
                    int yd1 = matrizentrada[i, 2];
                    /*int[] Wx;
                    int[] W;
                    int[] Ux;*/

                    if (u1 >= 0)
                    {
                        yr1 = 1;
                    }
                    else
                    {
                        yr1 = 0;
                    }

                    //Error lineal producido
                    El1 = matrizentrada[i, 2] - yr1;

                    //error del patron 
                    Ep = Math.Abs(El1) / 1;

                    //funcion soma
                    s1 = ((x1 * w11) + (x2 * w21)) - u1;

                    //modificar pesos y humbrales 
                    pesos[0, 0] = matrizentrada[i, 0] + rata_aprendizaje * El1 * 1;
                    pesos[0, 1] = matrizentrada[i, 1] + rata_aprendizaje * El1 * 1;

                    umbral = new double[i];
                   // umbral[0] = 0.2 ;// u1 + rata_aprendizaje * El1 * 1;

                    for (int h = 0; h < 1; h++)
                    {
                        dataGridView2.Rows.Add(new object[] { pesos[h, 0], pesos[h, 1] });
                    }
                    

                    dataGridView3.Rows.Add(new object[] { 0 });
                    //sumo error de cada patron
                }

                double EPP = ETP / 6;

            //grafico el error
            string v = "Error: " + Math.Round(EPP, 2) + " en la iteracion: " + iteraciones;
            error_final.Text = v;
            this.chart1.Series["Series1"].Points.AddXY(iteraciones, EPP);
            
                iteraciones++;

                if (EPP < Error_maximo)
                {
                    for (int f = 0; f < 1; f++)
                    {
                        Console.Write(pesos[f, 0] + " " + pesos[f, 1]);
                        Console.WriteLine();
                    }

                    //muestro resultado final
                    iteraciones_.Text = "ITERACIONES: " + iteraciones;
                    resultado.Text = "W(1, 1) = " + pesos[0, 0] + "    |   W(2, 1) = " + pesos[0, 1];
                    MessageBox.Show("!!!!RED ENTRENADA!!!" + "\n\n\n", "Mensaje");


                    //guardar pesos optimos en archivo plano
                    string rutaCompleta1 = @"C:\Users\Desconocido\Desktop\upc\2021-1\IA\1 corte\pesos_optimos.txt";
                    //guardar umbral optimos en archivo plano
                    string rutaCompleta2 = @"C:\Users\Desconocido\Desktop\upc\2021-1\IA\1 corte\umbrales_optimos.txt";

                    using (StreamWriter mylogs = File.AppendText(rutaCompleta1))         //se crea el archivo
                    {
                        string texto = pesos[0, 0] + ";" + pesos[0, 1];
                        mylogs.WriteLine(texto);
                        mylogs.Close();
                    }

                    using (StreamWriter mylogs = File.AppendText(rutaCompleta2))         //se crea el archivo
                    {
                        string texto = umbral[0].ToString();
                        mylogs.WriteLine(texto);
                        mylogs.Close();
                    }

                    break;
                }
                else
                {
                    iteraciones++;
                    int times = 30;
                    while (times > 0)
                    {
                        Application.DoEvents();
                        Thread.Sleep(1);
                        times--;
                    }
                }


            }

        }


        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }// llave final
}
