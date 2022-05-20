using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Compiladores
{
    public partial class COMPILADORES : Form
    {
        public COMPILADORES()
        {
            InitializeComponent();
        }

        StreamReader lectura;
        string ruta;
        string cadena = "";
        string arch = "";
        string[] variables = new string[50];
        string temvariables;
        string variableprincipal;
        string[] terminales = new string[50];
        string temterminales;
        string[] producciones = new string[50];
        string temproducciones;
        string[] alfa = new string[100];
        string[] beta = new string[100];
        string primerVariable = null;
        int siguiente = 0;
        int contadorvariables = 0;
        int contadorterminales = 0;
        int contadorproducciones = 0;
        int contadorcomillas = 0;
        int fila = 0;
        int filaSR = 0;
        int posicion = 0;
        string[] archivoLeido = new string[100];
        string[] archivoLeido2 = new string[100];
        string[] gramSinreCfinal = new string[100];
        bool banderaAlfa = false;
        bool banderaBeta = false;
        int contadorAlfa = 0;
        int contadorBeta = 0;
        int contadorGramFinal = 0;
        int contadorvariablesSR = 0;
        int contadorterminalesSR = 0;
        int contadorproduccionesSR = 0;
        int validarEpsilon = 0;

        private void COMPILADORES_Load(object sender, EventArgs e)
        {
            CUERPO.AllowDrop = true;
            CUERPO.DragEnter += new DragEventHandler(methodDragEnter);
            CUERPO.DragDrop += new DragEventHandler(methodDragDrop);
        }

        public void methodDragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        public static bool ValidarEspacio(string[] cadVal,string carct)
        {
            
            int forzado = 0;
            for (int i = 0; i < 50; i++)
            {
                if (cadVal[i] == null)
                {
                    cadVal[i] = carct;
                    i = 50;
                    forzado = 2;
                }
                else if(cadVal[i] != null)
                {
                    //MessageBox.Show(carct,cadVal[i]);
                    if (cadVal[i] == carct)
                    {
                        i = 50;
                        forzado = 1;
                    }
                }
            }
            if(forzado == 1)
            {
                return true;
            }else
            {
                return false;
            }

            
        }

        static void recursividadPorLaIzquierda()
        {

        }

        public void methodDragDrop(object sender, DragEventArgs e)
        {
            RUTA.Items.Clear();
            CUERPO.Text = "";
            CUERPOV.Items.Clear();
            CUERPOT.Items.Clear();


            string[] archivos = (string[])e.Data.GetData(DataFormats.FileDrop);


            foreach (string item in archivos)
            {
                ruta = item;
            }
            contadorcomillas = 0;
            contadorproducciones = 0;
            contadorterminales = 0;
            contadorvariables = 0;
            posicion = 0;
            RUTA.Items.Add(ruta);
            lectura = File.OpenText(ruta);
            arch = lectura.ReadToEnd();
            CUERPO.Text = arch;
            lectura = File.OpenText(ruta);
            cadena = lectura.ReadLine();
            while (cadena != null)
            {
                siguiente = 0;
                for (int i =0; i<cadena.Length; i++)
                {

                    if (cadena[i] != '=')
                    {
                        temvariables += cadena[i];
                    }else if(cadena[i] == '=')
                    {
                        if(ValidarEspacio(variables,temvariables) == false)
                        {
                            CUERPOV.Items.Add(temvariables);
                            contadorvariables++;
                        }
                        variableprincipal = temvariables;
                        siguiente = i + 1;
                        i = cadena.Length;
                    }
                }

                
                contadorcomillas = 0;
                temvariables = "";
                temproducciones = "";

                for(int i = siguiente; i < cadena.Length; i++)
                {
                    if (cadena[i] != '|')
                    {
                        if(cadena[i] != 39)
                        {
                            archivoLeido2[posicion] += cadena[i];
                            if (contadorcomillas == 0)
                            {
                                temvariables += cadena[i];
                                temproducciones += cadena[i];
                            }
                            else if(contadorcomillas == 1 )
                            {
                                temterminales += cadena[i];
                                temproducciones += cadena[i];
                                if (temvariables != "")
                                {
                                    if (ValidarEspacio(variables, temvariables) == false)
                                    {
                                        CUERPOV.Items.Add(temvariables);
                                        contadorvariables++;
                                    }



                                    if (variableprincipal != temvariables)
                                    {
                                        archivoLeido[posicion] += temvariables;
                                    }


                                }


                                temvariables = "";
                            }
                        }else if(cadena[i] == 39)
                        {
                            contadorcomillas++;
                            if(contadorcomillas == 2)
                            {
                                contadorcomillas = 0;
                                if (ValidarEspacio(terminales, temterminales) == false)
                                {
                                    CUERPOT.Items.Add(temterminales);
                                    CUERPOTSR.Items.Add(temterminales);
                                    contadorterminales++;
                                }
                                archivoLeido[posicion] += temterminales;
                                //MessageBox.Show(archivoLeido[posicion]);
                                //GSRI.Items.Add(archivoLeido2[posicion]);

                                temterminales = ""; 
                            }
                        }
                    }else if(cadena[i] == '|')
                    {
                        
                        fila = matrizprod.Rows.Add();
                        matrizprod.Rows[fila].Cells[0].Value = variableprincipal;
                        matrizprod.Rows[fila].Cells[1].Value = temproducciones;
                        contadorproducciones++;
                        contadorcomillas = 0;
                        if (temvariables != "")
                        {
                            if (ValidarEspacio(variables, temvariables) == false)
                            {
                                CUERPOV.Items.Add(temvariables);
                                contadorvariables++;
                            }

                            
                        }
                        if (variableprincipal != temvariables)
                        {
                            archivoLeido[posicion] += temvariables;
                        }



                        if(temterminales != "")
                        {
                            if (ValidarEspacio(terminales, temterminales) == false)
                            {
                                CUERPOT.Items.Add(temterminales);
                                CUERPOTSR.Items.Add(temterminales);
                                contadorterminales++;
                            }

                            archivoLeido[posicion] += temvariables;
                            
                        }
                        //MessageBox.Show(archivoLeido[posicion]);
                        banderaAlfa = (archivoLeido2[posicion].Contains(variableprincipal));
                        if (banderaAlfa == true)
                        {
                            alfa[contadorAlfa] = archivoLeido[posicion];
                            contadorAlfa++;
                        }else
                        {
                            beta[contadorBeta] = archivoLeido[posicion];
                            contadorBeta++;
                        }

                        posicion++;
                        temvariables = "";
                        temterminales = "";
                        temproducciones = "";
                        banderaAlfa = false;
                        banderaBeta = false;
                    }
                }
       
                fila = matrizprod.Rows.Add();
                matrizprod.Rows[fila].Cells[0].Value = variableprincipal;
                matrizprod.Rows[fila].Cells[1].Value = temproducciones;
                contadorproducciones++;
                contadorcomillas = 0;
                if(temvariables != "")
                {
                    if (ValidarEspacio(variables, temvariables) == false)
                    {
                        CUERPOV.Items.Add(temvariables);
                        contadorvariables++;
                    }

                  
                    if (variableprincipal != temvariables)
                    {
                        archivoLeido[posicion] += temvariables;
                    }
                }

                

                

                if (temterminales != "")
                {
                    if (ValidarEspacio(terminales, temterminales) == false)
                    {
                        CUERPOT.Items.Add(temterminales);
                        CUERPOTSR.Items.Add(temterminales);
                        contadorterminales++;
                    }

                    archivoLeido[posicion] += temvariables;
                    archivoLeido2[posicion] += temvariables;

                }
                //MessageBox.Show(archivoLeido[posicion]);
                banderaAlfa = (archivoLeido2[posicion].Contains(variableprincipal));
                if (banderaAlfa == true)
                {
                    alfa[contadorAlfa] = archivoLeido[posicion];
                    contadorAlfa++;
                }
                else
                {
                    beta[contadorBeta] = archivoLeido[posicion];
                    contadorBeta++;
                }

                //MessageBox.Show(Convert.ToString(contadorAlfa));
                if (contadorAlfa > 0)
                {
                    if(contadorBeta > 0)
                    {
                        for (int k = 0; k < contadorBeta; k++)
                        {
                            if (k == 0)
                            {
                                gramSinreCfinal[contadorGramFinal] += variableprincipal + " =";
                                contadorvariablesSR ++;
                                CUERPOVSR.Items.Add(variableprincipal);

                            }

                            if (k + 1 != contadorBeta)
                            {
                                gramSinreCfinal[contadorGramFinal] += beta[k] + variableprincipal + "' |";
                                filaSR = matrizprodSR.Rows.Add();
                                matrizprodSR.Rows[filaSR].Cells[0].Value = variableprincipal;
                                matrizprodSR.Rows[filaSR].Cells[1].Value = beta[k] + variableprincipal + "'";
                                contadorproduccionesSR++;
                            }
                            else
                            {
                                gramSinreCfinal[contadorGramFinal] += beta[k] + variableprincipal + "'";
                                filaSR = matrizprodSR.Rows.Add();
                                matrizprodSR.Rows[filaSR].Cells[0].Value = variableprincipal;
                                matrizprodSR.Rows[filaSR].Cells[1].Value = beta[k] + variableprincipal + "'";
                                contadorproduccionesSR++;
                            }
                        }
                        GSRI.Items.Add(gramSinreCfinal[contadorGramFinal]);
                        contadorGramFinal++;
                    }

                    for (int k = 0; k < contadorAlfa; k++)
                    {
                        if (k == 0)
                        {
                            gramSinreCfinal[contadorGramFinal] += variableprincipal + "'=";
                            contadorvariablesSR++;
                            CUERPOVSR.Items.Add(variableprincipal+"'");
                        }

                        if (k + 1 != contadorAlfa)
                        {
                            gramSinreCfinal[contadorGramFinal] += alfa[k] + variableprincipal + "' |";
                            filaSR = matrizprodSR.Rows.Add();
                            matrizprodSR.Rows[filaSR].Cells[0].Value = variableprincipal;
                            matrizprodSR.Rows[filaSR].Cells[1].Value = alfa[k] + variableprincipal + "'";
                            contadorproduccionesSR++;
                        }
                        else
                        {
                            gramSinreCfinal[contadorGramFinal] += alfa[k] + variableprincipal + "'|e";
                            filaSR = matrizprodSR.Rows.Add();
                            matrizprodSR.Rows[filaSR].Cells[0].Value = variableprincipal;
                            matrizprodSR.Rows[filaSR].Cells[1].Value = alfa[k] + variableprincipal + "'";
                            filaSR = matrizprodSR.Rows.Add();
                            matrizprodSR.Rows[filaSR].Cells[0].Value = variableprincipal;
                            matrizprodSR.Rows[filaSR].Cells[1].Value = "e";
                            contadorproduccionesSR++;
                            contadorproduccionesSR++;
                            validarEpsilon++;
                        }
                    }
                    GSRI.Items.Add(gramSinreCfinal[contadorGramFinal]);
                    contadorGramFinal++;
                }else if(contadorAlfa == 0)
                {
                    for (int k = 0; k < contadorBeta; k++)
                    {
                        if (k == 0)
                        {
                            gramSinreCfinal[contadorGramFinal] += variableprincipal + " =";
                            contadorvariablesSR++;
                            CUERPOVSR.Items.Add(variableprincipal);
                        }

                        if (k + 1 != contadorBeta)
                        {
                            gramSinreCfinal[contadorGramFinal] += beta[k] + "|";
                            filaSR = matrizprodSR.Rows.Add();
                            matrizprodSR.Rows[filaSR].Cells[0].Value = variableprincipal;
                            matrizprodSR.Rows[filaSR].Cells[1].Value = beta[k];
                            contadorproduccionesSR++;
                        }
                        else
                        {
                            gramSinreCfinal[contadorGramFinal] += beta[k];
                            filaSR = matrizprodSR.Rows.Add();
                            matrizprodSR.Rows[filaSR].Cells[0].Value = variableprincipal;
                            matrizprodSR.Rows[filaSR].Cells[1].Value = beta[k];
                            contadorproduccionesSR++;
                        }
                    }
                    GSRI.Items.Add(gramSinreCfinal[contadorGramFinal]);
                    contadorGramFinal++;
                }

                contadorAlfa = 0;
                contadorBeta = 0;
                temproducciones = "";
                temvariables = "";
                temterminales = "";
                posicion++;
                cadena = lectura.ReadLine();

            }

            V0.Text = "V." + Convert.ToString(contadorvariables);
            T0.Text = "T." + Convert.ToString(contadorterminales);
            P0.Text = "P." + Convert.ToString(contadorproducciones);

            if(validarEpsilon > 0)
            {
                contadorterminalesSR = contadorterminales + 1;
                CUERPOTSR.Items.Add("e");
            }
            else
            {
                contadorterminalesSR = contadorterminales;
            }

            lbVariablesSR.Text = "V." + Convert.ToString(contadorvariablesSR);
            lbTerminalesSR.Text = "T." + Convert.ToString(contadorterminalesSR);
            lbProduccionesSR.Text = "P." + Convert.ToString(contadorproduccionesSR);

        }
    }
}
